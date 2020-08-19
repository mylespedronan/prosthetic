package com.example.prosthetic.ui.terminal;

import androidx.lifecycle.ViewModel;

public class TerminalViewModel extends ViewModel {

    public String shoulderText;
    public String forearmText;
    public String wristText;
    public String fingerText;

    public String activateShoulder;
    public String activateForearm;
    public String activateWrist;
    public String activateFinger;

    public String largeDiameter;
    public String smallDiameter;
    public String palmarPinch;
    public String ball;
    public String lateral;
    public String thumb1;
    public String thumb2;
    public String thumb3;

    public String largeDiameterReset;
    public String smallDiameterReset;
    public String palmarPinchReset;
    public String ballReset;
    public String lateralReset;
    public String thumb1Reset;
    public String thumb2Reset;
    public String thumb3Reset;

    public String reset;


    public TerminalViewModel() {
        // Prosthetic
        shoulderText = "Shoulder";
        forearmText = "Forearm";
        wristText = "Wrist";
        fingerText = "Fingers";

        // Prosthetic Activate
        activateShoulder = "Activate Shoulder";
        activateForearm = "Activate Forearm";
        activateWrist = "Activate Wrist";
        activateFinger = "Activate Finger";

        // Grasps
        largeDiameter = "large";
        smallDiameter = "small";
        palmarPinch = "palmar";
        ball = "ball";
        lateral = "lateral";
        thumb1 = "thumb1";
        thumb2 = "thumb2";
        thumb3 = "thumb3";

        // Resets
        largeDiameterReset = "resetlarge";
        smallDiameterReset = "resetsmall";
        palmarPinchReset = "resetpalmar";
        ballReset = "resetball";
        lateralReset = "resetlateral";
        thumb1Reset = "resetthumb1";
        thumb2Reset = "resetthumb2";
        thumb3Reset = "resetthumb3";
        reset = "resetgrip";
    }
}