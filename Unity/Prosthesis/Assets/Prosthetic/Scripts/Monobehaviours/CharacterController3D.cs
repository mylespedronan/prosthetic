using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Diagnostics;

/*
*   This file contains the code needed to control the Human scene in Unity.
*   Upon successful connection with the ESP32, the program reads the ASCII
*   input via serial communication and performs animations based on the input.
*
*   Quaternion values determine the amount of rotation an object is given. The
*   initial positions are saved and each component is associated with a
*   variable. Once the input has been selected, the component's quarterion
*   values can then be altered with the joy stick/keyboard. Depending on the
*   key/direction, the selected component will move accordingly.
*
*   Using the Stopwatch() function, metrics are measured from when the command
*   is received via serial and after the command + animation has completed. The
*   Stopwatch is then reset to time next transaction.
*/

namespace Prosthetic.Scripts.MonoBehaviours
{
  public class CharacterController3D : MonoBehaviour
  {
    // Original Positions
    private Quaternion originalShoulderL;
    private Quaternion originalForearmL;
    private Quaternion originalHandL;
    private Quaternion originalFingersL;

    private Quaternion originalShoulderR;
    private Quaternion originalForearmR;
    private Quaternion originalHandR;
    private Quaternion originalFingersR;

    // Left Arm
    public GameObject shoulderLeft;
    public GameObject forearmLeft;
    public GameObject handLeft;
    public GameObject fingersLeft;

    // Right Arm
    public GameObject shoulderRight;
    public GameObject forearmRight;
    public GameObject handRight;
    public GameObject fingersRight;

    // ESP32 variables
    public int buttonPressed;
    public int message;

    // SerialPort sp = new SerialPort("COM4", 9600);
    SerialPort sp = new SerialPort("COM4", 115200);

    // Metrics
    Stopwatch st = new Stopwatch();

    private void Start() {
      // Left Arm
      shoulderLeft = GameObject.Find("upperArm.L");
      forearmLeft = GameObject.Find("forearm.L");
      handLeft = GameObject.Find("hand.L");
      fingersLeft = GameObject.Find("fingers.L");

      // Right Arm
      shoulderRight = GameObject.Find("upperArm.R");
      forearmRight = GameObject.Find("forearm.R");
      handRight = GameObject.Find("hand.R");
      fingersRight = GameObject.Find("fingers.R");

      // Original Positions
      originalShoulderL = shoulderLeft.transform.rotation;
      originalForearmL = forearmLeft.transform.rotation;
      originalHandL = handLeft.transform.rotation;
      originalFingersL = fingersLeft.transform.rotation;

      // Read Keyboard Button
      buttonPressed = 0;

      // Serial read from ESP32 (9600)
      sp.Open();
      sp.ReadTimeout = 1;
    }

    // Update is called once per frame. Checks to see if an input was received
    private void Update() {
      try {
        if(sp.IsOpen){
          message = sp.ReadByte();
          print(message);

          st.Start();
        }
      }
      catch (System.Exception ex){
        ex = new System.Exception();
      }

      ////////////////////
      // Select component
      ////////////////////

      // Shoulder
      if (Input.GetKey ("1") || message == 53) {
        buttonPressed = 1;
        UnityEngine.Debug.Log ("Selected Shoulder");

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Forearm
      if (Input.GetKey ("2") || message == 54) {
        buttonPressed = 2;
        UnityEngine.Debug.Log ("Selected Forearm");

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Hand
      if (Input.GetKey ("3") || message == 55) {
        buttonPressed = 3;
        UnityEngine.Debug.Log ("Selected Hand");

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Fingers
      if (Input.GetKey ("4") || message == 56) {
        buttonPressed = 4;
        UnityEngine.Debug.Log ("Selected fingers");

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Reset
      if (Input.GetKey ("5")) {
        buttonPressed = 5;
        UnityEngine.Debug.Log ("Reset");

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Move depending on which component selected
      switch (buttonPressed) {
        // Shoulder
        case 1:
        if (Input.GetKey(KeyCode.W) || message == 51) {
          shoulderLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate arm up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          shoulderLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate arm left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          shoulderLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate arm down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          shoulderLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate arm right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          shoulderLeft.transform.rotation = originalShoulderL;
          UnityEngine.Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Forearm
        case 2:
        if (Input.GetKey(KeyCode.W) || message == 51) {
          forearmLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate forearm up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          forearmLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate forearm left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          forearmLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate forearm down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          forearmLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate forearm right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          forearmLeft.transform.rotation = originalForearmL;
          UnityEngine.Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Hand
        case 3:
        if (Input.GetKey(KeyCode.W) || message == 51) {
          handLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate hand up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          handLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate hand left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          handLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate hand down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          handLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate hand right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          handLeft.transform.rotation = originalHandL;
          UnityEngine.Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Fingers
        case 4:
        if (Input.GetKey(KeyCode.W) || message == 51) {
          fingersLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate fingers up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          fingersLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate fingers left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          fingersLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          UnityEngine.Debug.Log ("Rotate fingers down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          fingersLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          UnityEngine.Debug.Log ("Rotate fingers right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          fingersLeft.transform.rotation = originalHandL;
          UnityEngine.Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Reset
        case 5:

        shoulderLeft.transform.rotation = originalShoulderL;
        forearmLeft.transform.rotation = originalForearmL;
        handLeft.transform.rotation = originalHandL;
        fingersLeft.transform.rotation = originalHandL;

        break;
      }
    }
  }
}
