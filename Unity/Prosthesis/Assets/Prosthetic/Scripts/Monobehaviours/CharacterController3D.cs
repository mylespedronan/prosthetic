using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

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

    // SerialPort sp = new SerialPort("COM3", 9600);
    SerialPort sp = new SerialPort("COM3", 115200);

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


    private void Update() {
      try {
        if(sp.IsOpen){
          message = sp.ReadByte();
          print(message);
        }
      }
      catch (System.Exception ex){
        ex = new System.Exception();
      }
      // Select component
      if (Input.GetKey ("1") || message == 53) {
        buttonPressed = 1;
        Debug.Log ("Selected Shoulder");
      }

      if (Input.GetKey ("2") || message == 54) {
        buttonPressed = 2;
        Debug.Log ("Selected Forearm");
      }

      if (Input.GetKey ("3") || message == 55) {
        buttonPressed = 3;
        Debug.Log ("Selected Hand");
      }

      if (Input.GetKey ("4") || message == 56) {
        buttonPressed = 4;
        Debug.Log ("Selected fingers");
      }

      if (Input.GetKey ("5")) {
        buttonPressed = 5;
        Debug.Log ("Reset");
      }

      // Move depending on which component selected
      switch (buttonPressed) {
        // Shoulder
        case 1:

        // try {
        //   if(sp.IsOpen){
        //     message = sp.ReadByte();
        //     print(message);
        //   }
        // }
        // catch (System.Exception ex){
        //   ex = new System.Exception();
        // }

        if (Input.GetKey(KeyCode.W) || message == 51) {
          shoulderLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          Debug.Log ("Rotate arm up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          shoulderLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate arm left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          shoulderLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          Debug.Log ("Rotate arm down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          shoulderLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate arm right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          shoulderLeft.transform.rotation = originalShoulderL;
          Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Forearm
        case 2:

        // try {
        //   if(sp.IsOpen){
        //     message = sp.ReadByte();
        //     print(message);
        //   }
        // }
        // catch (System.Exception ex){
        //   ex = new System.Exception();
        // }

        if (Input.GetKey(KeyCode.W) || message == 51) {
          forearmLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          Debug.Log ("Rotate forearm up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          forearmLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate forearm left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          forearmLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          Debug.Log ("Rotate forearm down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          forearmLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate forearm right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          forearmLeft.transform.rotation = originalForearmL;
          Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Hand
        case 3:

        // try {
        //   if(sp.IsOpen){
        //     message = sp.ReadByte();
        //     print(message);
        //   }
        // }
        // catch (System.Exception ex){
        //   ex = new System.Exception();
        // }

        if (Input.GetKey(KeyCode.W) || message == 51) {
          handLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          Debug.Log ("Rotate hand up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          handLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate hand left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          handLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          Debug.Log ("Rotate hand down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          handLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate hand right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          handLeft.transform.rotation = originalHandL;
          Debug.Log ("Reset");
        }

        message = 0;

        break;

        // Fingers
        case 4:

        // try {
        //   if(sp.IsOpen){
        //     message = sp.ReadByte();
        //     print(message);
        //   }
        // }
        // catch (System.Exception ex){
        //   ex = new System.Exception();
        // }

        if (Input.GetKey(KeyCode.W) || message == 51) {
          fingersLeft.transform.Rotate (0.0f, 0.0f, 0.75f, Space.Self);
          Debug.Log ("Rotate fingers up");
        }

        if (Input.GetKey(KeyCode.A) || message == 49) {
          fingersLeft.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate fingers left");
        }

        if (Input.GetKey(KeyCode.S) || message == 52) {
          fingersLeft.transform.Rotate (0.0f, 0.0f, -0.75f, Space.Self);
          Debug.Log ("Rotate fingers down");
        }

        if (Input.GetKey (KeyCode.D) || message == 50) {
          fingersLeft.transform.Rotate (0.75f, 0.0f, 0.0f, Space.Self);
          Debug.Log ("Rotate fingers right");
        }

        if (Input.GetKey (KeyCode.Space)) {
          fingersLeft.transform.rotation = originalHandL;
          Debug.Log ("Reset");
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
