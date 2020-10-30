using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using UnityEngine;

/*
*   This file contains the code needed to control the Hand scene in Unity.
*   Upon successful connection with the ESP32, the program reads the ASCII
*   input via serial communication and performs animations based on the input.
*
*   A dictionary is used to check whether an animation has been set and
*   transitions the animation accordingly (i.e. reset a grip to move to the
*   next grip selected).
*
*   Using the Stopwatch() function, metrics are measured from when the command
*   is received via serial and after the command + animation has completed. The
*   Stopwatch is then reset to time next transaction
*/

public class HandInput : MonoBehaviour
{
    public GameObject hand;

    // Dictionary
    public Dictionary<int, string> myDict = new Dictionary<int, string>();
    public Dictionary<int, int> keyDict = new Dictionary<int, int>();

    // ESP32 variables
    public int message;

    // Dictionary Keys
    public int resetKey;
    public int keyValue;

    // SerialPort sp = new SerialPort("COM4", 9600);
    SerialPort sp = new SerialPort("COM4", 115200);

    // Metrics
    Stopwatch st = new Stopwatch();

    private void Start() {
      // Reset Key
      resetKey = 0;
      keyValue = 0;

      // Dictionary values
      myDict.Add(1, "Hand|LargeDiameterReset");
      myDict.Add(2, "Hand|SmallDiameterReset");
      myDict.Add(3, "Hand|PalmarPinchReset");
      myDict.Add(4, "Hand|Sphere Reset");
      myDict.Add(5, "Hand|LateralReset");
      myDict.Add(6, "Hand|Thumb1Reset");
      myDict.Add(7, "Hand|Thumb2Reset");
      myDict.Add(8, "Hand|Thumb3Reset");

      keyDict.Add(1, 49);
      keyDict.Add(2, 50);
      keyDict.Add(3, 51);
      keyDict.Add(4, 52);
      keyDict.Add(5, 53);
      keyDict.Add(6, 54);
      keyDict.Add(7, 55);
      keyDict.Add(8, 56);

      // Serial read from ESP32
      sp.Open();
      sp.ReadTimeout = 1;
    }

    // Update is called once per frame. Checks to see if an input was received
    void Update()
    {
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

      // large Diameter
      if(message == 49){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Large Diameter Grasp");
          hand.GetComponent<Animator>().Play("Hand|LargeDiameter");
        }

        resetKey = 1;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Small Diameter
      if(message == 50){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Small Diameter Grasp");
          hand.GetComponent<Animator>().Play("Hand|SmallDiameter");
        }

        resetKey = 2;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Palmar Pinch
      if(message == 51){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Palmar Pinch");
          hand.GetComponent<Animator>().Play("Hand|PalmarPinch");
        }

        resetKey = 3;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Sphere Reset
      if(message == 52){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Ball Grasp");
          hand.GetComponent<Animator>().Play("Hand|Sphere");
        }

        resetKey = 4;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Lateral Reset
      if(message == 53){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Lateral Grasp");
          hand.GetComponent<Animator>().Play("Hand|Lateral");
        }

        resetKey = 5;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 1
      if(message == 54){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Thumb + One");
          hand.GetComponent<Animator>().Play("Hand|Thumb1");
        }

        resetKey = 6;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 2
      if(message == 55){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Thumb + Two");
          hand.GetComponent<Animator>().Play("Hand|Thumb2");
        }

        resetKey = 7;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 3
      if(message == 56){
        if(resetKey != 0 && keyDict[resetKey] != message){
          UnityEngine.Debug.Log("Resetting");
          UnityEngine.Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          UnityEngine.Debug.Log("Thumb + Three");
          hand.GetComponent<Animator>().Play("Hand|Thumb3");
        }

        resetKey = 8;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Grip Reset
      if(message == 57){
        UnityEngine.Debug.Log("Resetting");
        UnityEngine.Debug.Log(myDict[resetKey]);
        hand.GetComponent<Animator>().SetBool("reset", true);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
        hand.GetComponent<Animator>().Play(myDict[resetKey]);

        // Reset values
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      //////////////
      // Resets
      //////////////

      // Large Diameter Reset
      if(message == 97){
        UnityEngine.Debug.Log("Large Diameter Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|LargeDiameterReset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Small Diameter Reset
      if(message == 98){
        UnityEngine.Debug.Log("Small Diameter Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|SmallDiameterReset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Palmar Pinch Reset
      if(message == 99){
        UnityEngine.Debug.Log("Palmar Pinch Reset");
        hand.GetComponent<Animator>().Play("Hand|PalmarPinchReset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Ball Grip Reset
      if(message == 100){
        UnityEngine.Debug.Log("Ball Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|Sphere Reset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Lateral Grasp Reset
      if(message == 101){
        UnityEngine.Debug.Log("Lateral Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|LateralReset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 1 Reset
      if(message == 102){
        UnityEngine.Debug.Log("Thumb + One Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb1Reset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 2 Reset
      if(message == 103){
        UnityEngine.Debug.Log("Thumb + Two Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb2Reset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Thumb 3 Reset
      if(message == 104){
        UnityEngine.Debug.Log("Thumb + Three Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb3Reset");
        resetKey = 0;

        // Metrics
        st.Stop();
        UnityEngine.Debug.Log ("Time taken: " + st.Elapsed);
        st.Reset();
      }

      // Reset message
      message = 0;

      return;
    }
}
