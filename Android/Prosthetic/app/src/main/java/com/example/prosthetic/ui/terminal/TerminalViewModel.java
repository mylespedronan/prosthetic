package com.example.prosthetic.ui.terminal;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

public class TerminalViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public TerminalViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is terminal fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}