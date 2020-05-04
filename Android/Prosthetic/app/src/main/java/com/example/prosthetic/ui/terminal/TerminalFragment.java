package com.example.prosthetic.ui.terminal;

import android.app.Activity;
import android.app.AlertDialog;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.method.ScrollingMovementMethod;
import android.text.style.ForegroundColorSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import com.example.prosthetic.R;
import com.example.prosthetic.SerialListener;
import com.example.prosthetic.SerialService;
import com.example.prosthetic.SerialSocket;

public class TerminalFragment extends Fragment implements ServiceConnection, SerialListener {

    private TerminalViewModel terminalViewModel;
    private Button buttonShoulder, buttonForearm, buttonWrist, buttonFingers, buttonM1, buttonM2,
            buttonM3, buttonM4, buttonVoice;
    private enum Connected { False, Pending, True }

    private String deviceAddress;
    private String newline = "\r\n";

    private TextView receiveText;

    private SerialSocket socket;
    private SerialService service;
    private boolean initialStart = true;
    private Connected connected = Connected.False;

    /*
     * Lifecycle
     */
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
        setRetainInstance(true);
        Log.d("Terminal Create", "Terminal Created");

        deviceAddress = getArguments().getString("device");
        Log.i("Device address value",deviceAddress);

        if (getArguments() == null) {
            Log.d("Device Address", "No Device Address");
        } else {
            Log.d("Device Address", "Device Address found");
        }

    }

    @Override
    public void onDestroy() {
        if (connected != Connected.False) {
            disconnect();
            Log.d("Destroy", "Terminal Destroyed");
        }
        getActivity().stopService(new Intent(getActivity(), SerialService.class));

        super.onDestroy();
    }

    @Override
    public void onStart() {
        super.onStart();
        if(service != null) {
            service.attach(this);
            Log.d("Start", "Service start");
        } else {
            getActivity().startService(new Intent(getActivity(), SerialService.class)); // prevents service destroy on unbind from recreated activity caused by orientation change
            Log.d("Start", "Get Activity Start");
        }
    }

    @Override
    public void onStop() {
        if(service != null && !getActivity().isChangingConfigurations()) {
            service.detach();
            Log.d("Stop", "Service stop");
        }
        super.onStop();
    }

    @SuppressWarnings("deprecation") // onAttach(context) was added with API 23. onAttach(activity) works for all API versions
    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);

        Activity act = getActivity();

        if (act == null){
            Log.d("Activity", "Null");
        } else {
            Log.d("Activity", "Get Activity");
        }

        getActivity().bindService(new Intent(act, SerialService.class), this, Context.BIND_AUTO_CREATE);
        Log.d("Attach", "On attach");
    }

    @Override
    public void onDetach() {
        try { getActivity().unbindService(this); } catch(Exception ignored) {}
        super.onDetach();
    }

    @Override
    public void onResume() {
        super.onResume();
        if(initialStart && service !=null) {
            initialStart = false;
            getActivity().runOnUiThread(this::connect);
        }
    }

    @Override
    public void onServiceConnected(ComponentName name, IBinder binder) {
        service = ((SerialService.SerialBinder) binder).getService();
        if(initialStart && isResumed()) {
            initialStart = false;
            getActivity().runOnUiThread(this::connect);
            Log.d("Service Connected", "Service connected");
        }
    }

    /*
     * UI
     */
    @Override
    public void onServiceDisconnected(ComponentName name) {
        service = null;
    }

    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        terminalViewModel =
                ViewModelProviders.of(this).get(TerminalViewModel.class);

        View root = inflater.inflate(R.layout.fragment_terminal, container, false);

        receiveText = root.findViewById(R.id.receive_text);
        receiveText.setTextColor(getResources().getColor(R.color.colorRecieveText));
        receiveText.setMovementMethod(ScrollingMovementMethod.getInstance());

        /*
         *  Buttons on terminal view for major components, custom macros (future use),
         *  and voice button.
         */

        // Buttons for Components
        buttonShoulder = root.findViewById(R.id.button_shoulder);
        buttonForearm = root.findViewById(R.id.button_forearm);
        buttonWrist = root.findViewById(R.id.button_wrist);
        buttonFingers = root.findViewById(R.id.button_fingers);

        // OnClickListener for Components
        buttonShoulder.setOnClickListener(v -> send(terminalViewModel.shoulderText));
        buttonForearm.setOnClickListener(v -> send(terminalViewModel.forearmText));
        buttonWrist.setOnClickListener(v -> send(terminalViewModel.wristText));
        buttonFingers.setOnClickListener(v -> send(terminalViewModel.fingerText));

        // Macro Buttons
        buttonM1 = root.findViewById(R.id.button_m1);
        buttonM2 = root.findViewById(R.id.button_m2);
        buttonM3 = root.findViewById(R.id.button_m3);
        buttonM4 = root.findViewById(R.id.button_m4);

        // OnClickListener for Macros
//        buttonM1.setOnClickListener(this);
//        buttonM2.setOnClickListener(this);
//        buttonM3.setOnClickListener(this);
//        buttonM4.setOnClickListener(this);

        // Voice Button
        buttonVoice = root.findViewById(R.id.button_voice);

        // OnClickListener for Voice
//        buttonVoice.setOnClickListener(this);

        return root;
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        inflater.inflate(R.menu.menu_terminal, menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.clear){
            receiveText.setText("");
            return true;
        } else if (id == R.id.newline) {
            String [] newlineNames = getResources().getStringArray(R.array.newline_names);
            String [] newlineValues = getResources().getStringArray(R.array.newline_values);

            int pos = java.util.Arrays.asList(newlineValues).indexOf(newline);
            AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
            builder.setTitle("New line");
            builder.setSingleChoiceItems(newlineNames, pos, (dialog, item1) -> {
                newline = newlineValues[item1];
                dialog.dismiss();
            });

            builder.create().show();
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }

    /*
     * Serial + UI
     */
    private void connect() {
        Log.i("Terminal Connect", "Terminal Connected");
        try {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
            BluetoothDevice device = bluetoothAdapter.getRemoteDevice(deviceAddress);
            String deviceName = device.getName() != null ? device.getName() : device.getAddress();
            status("connecting...");
            connected = Connected.Pending;
            socket = new SerialSocket();
            service.connect(this, "Connected to " + deviceName);
            socket.connect(getContext(), service, device);
            Log.d("Connect", "Connect Success");
        } catch (Exception e) {
            onSerialConnectError(e);
            Log.d("Connect", "Connect Error");
        }
    }

    private void disconnect() {
        connected = Connected.False;
        service.disconnect();
        socket.disconnect();
        socket = null;
    }

    private void send(String str) {
        if(connected != Connected.True) {
            Toast.makeText(getActivity(), "not connected", Toast.LENGTH_SHORT).show();
            return;
        }
        try {
            SpannableStringBuilder spn = new SpannableStringBuilder(str+'\n');
            spn.setSpan(new ForegroundColorSpan(getResources().getColor(R.color.colorSendText)), 0, spn.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
            receiveText.append(spn);
            byte[] data = (str + newline).getBytes();
            socket.write(data);
        } catch (Exception e) {
            onSerialIoError(e);
        }
    }

    private void receive(byte[] data) {
        receiveText.append(new String(data));
    }

    private void status(String str) {
        SpannableStringBuilder spn = new SpannableStringBuilder(str+'\n');
        spn.setSpan(new ForegroundColorSpan(getResources().getColor(R.color.colorStatusText)), 0, spn.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
        receiveText.append(spn);
    }

    /*
     * SerialListener
     */
    @Override
    public void onSerialConnect() {
        status("connected");
        connected = Connected.True;
    }

    @Override
    public void onSerialConnectError(Exception e) {
        status("connection failed: " + e.getMessage());
        disconnect();
    }

    @Override
    public void onSerialRead(byte[] data) {
        receive(data);
    }

    @Override
    public void onSerialIoError(Exception e) {
        status("connection lost: " + e.getMessage());
        disconnect();
    }
}