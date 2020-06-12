package com.example.prosthetic.ui.terminal;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.TimeZone;

import javax.security.auth.callback.Callback;

public class TerminalViewModel extends ViewModel {

    public String shoulderText;
    public String forearmText;
    public String wristText;
    public String fingerText;

    public String activateShoulder;
    public String activateForearm;
    public String activateWrist;
    public String activateFinger;

    public TerminalViewModel() {

        shoulderText = "Shoulder";
        forearmText = "Forearm";
        wristText = "Wrist";
        fingerText = "Fingers";

        activateShoulder = "Activate Shoulder";
        activateForearm = "Activate Forearm";
        activateWrist = "Activate Wrist";
        activateFinger = "Activate Finger";
    }
}