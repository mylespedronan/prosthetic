package com.example.prosthetic.ui.device;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

/*
 *   This file contains the view model for the Device Fragment
 */


public class DeviceViewModel extends ViewModel {

    private MutableLiveData<String> mText;

    public DeviceViewModel() {
        mText = new MutableLiveData<>();
        mText.setValue("This is device fragment");
    }

    public LiveData<String> getText() {
        return mText;
    }
}