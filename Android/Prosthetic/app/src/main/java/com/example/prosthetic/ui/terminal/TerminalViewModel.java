package com.example.prosthetic.ui.terminal;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import javax.security.auth.callback.Callback;

public class TerminalViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public String shoulderText = "Shoulder";
    public String forearmText = "Forearm";
    public String wristText = "Wrist";
    public String fingerText = "Fingers";

    public TerminalViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is terminal fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}