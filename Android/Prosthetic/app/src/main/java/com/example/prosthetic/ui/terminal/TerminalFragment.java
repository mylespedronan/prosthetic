package com.example.prosthetic.ui.terminal;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;

import com.example.prosthetic.R;

public class TerminalFragment extends Fragment {

    private TerminalViewModel terminalViewModel;

    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        terminalViewModel =
                ViewModelProviders.of(this).get(TerminalViewModel.class);
        View root = inflater.inflate(R.layout.fragment_terminal, container, false);
        return root;
    }
}