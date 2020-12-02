package com.example.prosthetic.ui.terminal;

import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.IBinder;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.speech.tts.TextToSpeech;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.format.DateUtils;
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
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import com.example.prosthetic.R;
import com.example.prosthetic.SerialListener;
import com.example.prosthetic.SerialService;
import com.example.prosthetic.SerialSocket;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;


/*
 *  This file contains the e terminal fragment
 */

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

    private String timeStamp;
    private SimpleDateFormat dateFormat;

    private static final int MY_PERMISSIONS_REQUEST_RECORD_AUDIO = 1;

    private TextToSpeech myTTS;
    private SpeechRecognizer mySpeechRecognizer;


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

        // Timestamp
        dateFormat = new SimpleDateFormat("hh:mm:ss");
        dateFormat.setTimeZone(TimeZone.getTimeZone("GMT-7"));

        // Initialize Voice Recognition
        initializeTextToSpeech();
        initializeSpeechRecognizer();
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
            // prevents service destroy on unbind from recreated
            // activity caused by orientation change
            getActivity().startService(new Intent(getActivity(), SerialService.class));
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

        // OnClickListener for Macros [Future Use]
//        buttonM1.setOnClickListener(this);
//        buttonM2.setOnClickListener(this);
//        buttonM3.setOnClickListener(this);
//        buttonM4.setOnClickListener(this);

        // Voice Button
        buttonVoice = root.findViewById(R.id.button_voice);

        // OnClickListener for Voice
        buttonVoice.setOnClickListener(v -> sendVoice());

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
            status("Connecting...");
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
        Log.d("Disconnect","Disconnect");
    }

    private void send(String str) {
        if(connected != Connected.True) {
            Toast.makeText(getActivity(), "Not connected", Toast.LENGTH_SHORT).show();
            return;
        }
        try {
            SpannableStringBuilder spn = new SpannableStringBuilder(str+'\n');
            spn.setSpan(new ForegroundColorSpan(getResources().getColor(R.color.colorSendText)),
                    0, spn.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
            receiveText.append(spn);
            byte[] data = (str + newline).getBytes();
            socket.write(data);
        } catch (Exception e) {
            onSerialIoError(e);
        }
    }

    private void sendVoice() {
        if(connected != Connected.True) {
            Toast.makeText(getActivity(), "Not connected", Toast.LENGTH_SHORT).show();
            return;
        }
        try {
            if (ContextCompat.checkSelfPermission(getActivity(),
                    Manifest.permission.RECORD_AUDIO)
                    != PackageManager.PERMISSION_GRANTED) {

                if (ActivityCompat.shouldShowRequestPermissionRationale(getActivity(),
                        Manifest.permission.RECORD_AUDIO)) {
                    Log.d("Record Audio", "Audio permission ");
                } else {
                    ActivityCompat.requestPermissions(getActivity(),
                            new String[]{Manifest.permission.RECORD_AUDIO}, MY_PERMISSIONS_REQUEST_RECORD_AUDIO);
                }
            } else {
                Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
                intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
                        RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
                intent.putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 1);
                mySpeechRecognizer.startListening(intent);
            }
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

        timeStamp = dateFormat.format(new Date()) + ": ";

        receiveText.append(timeStamp);
        receiveText.append(spn);
    }

    /*
     * SerialListener
     */
    @Override
    public void onSerialConnect() {
        status("Connected");
        connected = Connected.True;
    }

    @Override
    public void onSerialConnectError(Exception e) {
        status("Connection failed: " + e.getMessage() + "\n" +
                "Press back and reconnect to device.");
        disconnect();
    }

    @Override
    public void onSerialRead(byte[] data) {
        receive(data);
    }

    @Override
    public void onSerialIoError(Exception e) {
        status("Connection lost: " + e.getMessage() + "\n" +
                "Press back and reconnect to device.");
        disconnect();
    }

    /*
     * Voice Recognition
     */

    private void initializeSpeechRecognizer() {
        if(SpeechRecognizer.isRecognitionAvailable(getActivity())){
            mySpeechRecognizer = SpeechRecognizer.createSpeechRecognizer(getActivity());
            mySpeechRecognizer.setRecognitionListener(new RecognitionListener() {
                @Override
                public void onReadyForSpeech(Bundle bundle) {

                }

                @Override
                public void onBeginningOfSpeech() {

                }

                @Override
                public void onRmsChanged(float v) {

                }

                @Override
                public void onBufferReceived(byte[] bytes) {

                }

                @Override
                public void onEndOfSpeech() {

                }

                @Override
                public void onError(int i) {

                }

                @Override
                public void onResults(Bundle bundle) {
                    long startResults = System.nanoTime();

                    List<String> results = bundle.getStringArrayList(
                            SpeechRecognizer.RESULTS_RECOGNITION
                    );
                    processResult(results.get(0), startResults);
                }

                @Override
                public void onPartialResults(Bundle bundle) {

                }

                @Override
                public void onEvent(int i, Bundle bundle) {

                }
            });
        }
    }

    // Recognize voice command
    private void processResult(String command, long startResults) {
        command = command.toLowerCase();

        long startTime = System.nanoTime();

        // What is your name?
        // What is the time?
        // Open the browser

        if(command.contains("hand")){
            if(command.contains("wave")){
                speak("Hand wave");
                send("handwave");
                System.out.println("Hand");
            }
        }


        if(command.contains("what")) {
            if (command.contains("your name")) {
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("My name is Koko");
                send("My name is Koko");
                System.out.println( "Total Time (Speech: 'What is your name'): " + (System.nanoTime() - startTime));
            }
            if (command.contains("time")) {
                Date now = new Date();
                String time = DateUtils.formatDateTime(getActivity(), now.getTime(),
                        DateUtils.FORMAT_SHOW_TIME);
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("The time now is " + time);
                send("The time now is " + time);
                System.out.println( "Total Time (Speech: 'What is the time'): " + (System.nanoTime() - startTime));
            }
        } else if (command.contains("activate")) {
            // Prosthetic components
            if (command.contains("shoulder")) {
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("Activating shoulder.");
                send(terminalViewModel.shoulderText);
                System.out.println( "Total Time (Speech: 'Activate Shoulder'): " + (System.nanoTime() - startTime));
            }
            if (command.contains("forearm")) {
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("Activating forearm.");
                send(terminalViewModel.forearmText);
                System.out.println( "Total Time (Speech: 'Activate Forearm'): " + (System.nanoTime() - startTime));
            }
            if (command.contains("wrist")) {
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("Activating wrist.");
                send(terminalViewModel.wristText);
                System.out.println( "Total Time (Speech: 'Activate Wrist'): " + (System.nanoTime() - startTime));
            }
            if (command.contains("fingers")) {
                System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                speak("Activating Fingers.");
                send(terminalViewModel.fingerText);
                System.out.println( "Total Time (Speech: 'Activate Fingers'): " + (System.nanoTime() - startTime));
            }

            // Hand Commands //
            // Large Diameter Grasp
            if (command.contains("large")) {
                if (command.contains("diameter")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Large Diameter Grasp.");
                    send(terminalViewModel.largeDiameter);
                    System.out.println( "Total Time (Speech: 'Activate Large Diameter Grasp'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Large Diameter Reset.");
                    send(terminalViewModel.largeDiameterReset);
                    System.out.println( "Total Time (Speech: 'Activate Large Diameter Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Small Diameter Grasp
            if (command.contains("small")) {
                if (command.contains("diameter")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Small Diameter Grasp.");
                    send(terminalViewModel.smallDiameter);
                    System.out.println( "Total Time (Speech: 'Activate Small Diameter Grasp'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Small Diameter Reset.");
                    send(terminalViewModel.smallDiameterReset);
                    System.out.println( "Total Time (Speech: 'Activate Small Diameter Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Palmar Pinch Grasp
            if (command.contains("palmar")) {
                if (command.contains("pinch")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Palmar Pinch.");
                    send(terminalViewModel.palmarPinch);
                    System.out.println( "Total Time (Speech: 'Activate Palmar Pinch'): " + (System.nanoTime() - startTime));
                }
            }
            if (command.contains("pinch")){
                if (command.contains("reset")) {
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Palmar Pinch Reset.");
                    send(terminalViewModel.palmarPinchReset);
                    System.out.println("Total Time (Speech: 'Activate Palmar Pinch Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Sphere/Ball Grasp
            if (command.contains("ball")) {
                if (command.contains("grasp")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Ball Grasp.");
                    send(terminalViewModel.ball);
                    System.out.println( "Total Time (Speech: 'Activate Ball Grasp'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Ball Reset.");
                    send(terminalViewModel.ballReset);
                    System.out.println( "Total Time (Speech: 'Activate Ball Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Lateral Grasp
            if (command.contains("lateral")) {
                if (command.contains("grasp")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Lateral Grasp.");
                    send(terminalViewModel.lateral);
                    System.out.println( "Total Time (Speech: 'Activate Lateral Grasp'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println( "Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Lateral Grasp Reset.");
                    send(terminalViewModel.lateralReset);
                    System.out.println( "Total Time (Speech: 'Activate Lateral Grasp Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Thumb One
            if (command.contains("one")) {
                if (command.contains("grasp")) {
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb One.");
                    send(terminalViewModel.thumb1);
                    System.out.println("Total Time (Speech: 'Activate Thumb One'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb One Reset.");
                    send(terminalViewModel.thumb1Reset);
                    System.out.println("Total Time (Speech: 'Activate Thumb One Reset'): " + (System.nanoTime() - startTime));
                }
            }

            // Thumb Two
            if (command.contains("to")) {
                if (command.contains("grasp")) {
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb Two.");
                    send(terminalViewModel.thumb2);
                    System.out.println("Total Time (Speech: 'Activate Thumb Two.'): " + (System.nanoTime() - startTime));
                }
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb Two Reset.");
                    send(terminalViewModel.thumb2Reset);
                    System.out.println("Total Time (Speech: 'Activate Thumb Two Reset.'): " + (System.nanoTime() - startTime));
                }
            }


            if (command.contains("three")) {
                if (command.contains("grasp")) {
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb Three.");
                    send(terminalViewModel.thumb3);
                    System.out.println("Total Time (Speech: 'Activate Thumb Three.'): " + (System.nanoTime() - startTime));
                }
                //
                if (command.contains("reset")){
                    System.out.println("Start Results: " + (startResults * Math.pow(10, 9)));
                    System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                    System.out.println("Total Time: " + (System.nanoTime() - startTime));
                    speak("Activating Thumb Three Reset.");
                    send(terminalViewModel.thumb3Reset);
                    System.out.println("Total Time (Speech: 'Activate Thumb Three Reset.'): " + (System.nanoTime() - startTime));
                }
            }

            if (command.contains("grip reset")) {
                System.out.println("Start Results: " + (startResults));
                System.out.println("Begin Time: " + (System.nanoTime() - startResults));
                System.out.println("Total Time: " + (System.nanoTime() - startTime));
                speak("Activating Reset.");
                send(terminalViewModel.reset);
                System.out.println("Total Time (Speech: 'Activate Reset.'): " + (System.nanoTime() - startTime));
            }


        } else if(command.contains("open")) {
            if(command.contains("browser")) {
                Intent intent = new Intent(Intent.ACTION_VIEW,
                        Uri.parse("https://google.com"));
                startActivity(intent);
            }
        }
    }

    private void initializeTextToSpeech() {
        myTTS = new TextToSpeech(getActivity(), new TextToSpeech.OnInitListener() {
            @Override
            public void onInit(int i) {
                if(myTTS.getEngines().size() == 0){
                    Toast.makeText(getActivity(), "There is no TTS engine on your device"
                            , Toast.LENGTH_LONG).show();

                    status("No TTS engine on device");
                    Log.d("TTS", "No TTS Engine created");
                } else {
                    myTTS.setLanguage(Locale.US);
                    speak("Hello. I am ready.");
                }
            }


        });
    }

    private void speak(String message) {
        if(Build.VERSION.SDK_INT >= 21){
            myTTS.speak(message, TextToSpeech.QUEUE_FLUSH, null, null);
        } else {
            myTTS.speak(message, TextToSpeech.QUEUE_FLUSH, null);
        }
    }

}
