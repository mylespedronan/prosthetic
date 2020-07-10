using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace Prosthetic.Scripts.MonoBehaviours
{
  public class HandController3D : MonoBehaviour
  {
    // Original Positions
    // Index Finger
    private Quaternion originalIndexProximal;
    private Quaternion originalIndexMiddle;
    private Quaternion originalIndexDistal;

    // Middle Finger
    private Quaternion originalMiddleProximal;
    private Quaternion originalMiddleMiddle;
    private Quaternion originalMiddleDistal;

    // Ring Finger
    private Quaternion originalRingProximal;
    private Quaternion originalRingMiddle;
    private Quaternion originalRingDistal;

    // Pinky Finger
    private Quaternion originalPinkyProximal;
    private Quaternion originalPinkyMiddle;
    private Quaternion originalPinkyDistal;

    // Thumb
    private Quaternion originalThumbProximal;
    private Quaternion originalThumbMiddle;
    private Quaternion originalThumbDistal;

    // Hand Object
    // Index
    public GameObject indexProximal;
    public GameObject indexMiddle;
    public GameObject indexDistal;

    // Middle
    public GameObject middleProximal;
    public GameObject middleMiddle;
    public GameObject middleDistal;

    // Ring
    public GameObject ringProximal;
    public GameObject ringMiddle;
    public GameObject ringDistal;

    // Pinky
    public GameObject pinkyProximal;
    public GameObject pinkyMiddle;
    public GameObject pinkyDistal;

    // Thumb
    public GameObject thumbProximal;
    public GameObject thumbMiddle;
    public GameObject thumbDistal;

    // ESP32 variables
    public int buttonPressed;
    public int message;

    // SerialPort sp = new SerialPort("COM3", 9600);
    // SerialPort sp = new SerialPort("COM3", 115200);

    private void Start() {
      // Index
      indexProximal = GameObject.Find("IndexMiddle");
      indexMiddle = GameObject.Find("IndexDistal");
      indexDistal = GameObject.Find("IndexDistal_end");

      // Middle
      middleProximal = GameObject.Find("MiddleProximal");
      middleMiddle = GameObject.Find("MiddleMiddle");
      middleDistal = GameObject.Find("MiddleDistal");

      // Ring
      ringProximal = GameObject.Find("RingProximal");
      ringMiddle = GameObject.Find("RingMiddle");
      ringDistal = GameObject.Find("RingDistal");

      // Pinky
      pinkyProximal = GameObject.Find("PinkyProximal");
      pinkyMiddle = GameObject.Find("PinkyMiddle");
      pinkyDistal = GameObject.Find("PinkyDistal");

      // Thumb
      thumbProximal = GameObject.Find("ThumbProximal");
      thumbMiddle = GameObject.Find("ThumbMiddle");
      thumbDistal  = GameObject.Find("ThumbDistal");

      // Original Positions
      // Index Finger
      originalIndexProximal = indexProximal.transform.rotation;
      originalIndexMiddle = indexMiddle.transform.rotation;
      originalIndexDistal = indexDistal.transform.rotation;

      // Middle Finger
      originalMiddleProximal = middleProximal.transform.rotation;
      originalMiddleMiddle = middleMiddle.transform.rotation;
      originalMiddleDistal = middleDistal.transform.rotation;

      // Ring Finger
      originalRingProximal = ringProximal.transform.rotation;
      originalRingMiddle = ringMiddle.transform.rotation;
      originalRingDistal = ringDistal.transform.rotation;

      // Pinky Finger
      originalPinkyProximal = pinkyProximal.transform.rotation;
      originalPinkyMiddle = pinkyMiddle.transform.rotation;
      originalPinkyDistal = pinkyDistal.transform.rotation;

      // Thumb
      originalThumbProximal = thumbProximal.transform.rotation;
      originalThumbMiddle = thumbMiddle.transform.rotation;
      originalThumbDistal = thumbDistal.transform.rotation;

      // Read Keyboard Button
      buttonPressed = 0;

      // Serial read from ESP32 (9600)
      // sp.Open();
      // sp.ReadTimeout = 1;
    }

    private void Update() {
      // try {
      //   if(sp.IsOpen){
      //     message = sp.ReadByte();
      //     print(message);
      //   }
      // }
      // catch (System.Exception ex){
      //   ex = new System.Exception();
      // }

      // Select component
      if (Input.GetKey ("1") || message == 53) {
        buttonPressed = 1;
        Debug.Log ("Selected Lateral Pinch");
      }

      if (Input.GetKey ("2") || message == 54) {
        buttonPressed = 2;
        Debug.Log ("Selected Palmar Pinch");
      }

      if (Input.GetKey ("3") || message == 55) {
        buttonPressed = 3;
        Debug.Log ("Selected Sphere Grip");
      }

      if (Input.GetKey ("4") || message == 56) {
        buttonPressed = 4;
        Debug.Log ("Selected Heavy Wrap");
      }

      if (Input.GetKey ("5")) {
        buttonPressed = 5;
        Debug.Log ("Reset");
      }

      switch (buttonPressed) {
        // Lateral Pinch
        case 1:

        if (Input.GetKey(KeyCode.W) || message == 51) {
          indexProximal.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
        }
        message = 0;

        break;

        // Palmar Pinch
        case 2:
        if (Input.GetKey(KeyCode.W) || message == 51) {
          middleProximal.transform.Rotate (-0.75f, 0.0f, 0.0f, Space.Self);
        }
        message = 0;

        break;

        // Sphere Grip
        case 3:

        message = 0;

        break;

        // Heavy Wrap
        case 4:

        message = 0;

        break;

        // Reset
        case 5:

        // Original Positions
        // Index Finger
        indexProximal.transform.rotation = originalIndexProximal;
        indexMiddle.transform.rotation = originalIndexMiddle;
        indexDistal.transform.rotation = originalIndexDistal;

        // Middle Finger
        middleProximal.transform.rotation = originalMiddleProximal;
        middleMiddle.transform.rotation = originalMiddleMiddle;
        middleDistal.transform.rotation = originalMiddleDistal;

        // Ring Finger
        ringProximal.transform.rotation = originalRingProximal;
        ringMiddle.transform.rotation = originalRingMiddle;
        ringDistal.transform.rotation = originalRingDistal;

        // Pinky Finger
        pinkyProximal.transform.rotation = originalPinkyProximal;
        pinkyMiddle.transform.rotation = originalPinkyMiddle;
        pinkyDistal.transform.rotation = originalPinkyDistal;

        // Thumb
        thumbProximal.transform.rotation = originalThumbProximal;
        thumbMiddle.transform.rotation = originalThumbMiddle;
        thumbDistal.transform.rotation = originalThumbDistal;

        break;
      }
    }
  }
}
