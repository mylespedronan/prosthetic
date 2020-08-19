using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class HandInput : MonoBehaviour
{
    public GameObject hand;

    // Dictionary
    public Dictionary<int, string> myDict = new Dictionary<int, string>();
    public Dictionary<int, int> keyDict = new Dictionary<int, int>();

    // ESP32 variables
    public int message;

    // Dictionary
    public int resetKey;
    public int keyValue;

    // SerialPort sp = new SerialPort("COM3", 9600);
    SerialPort sp = new SerialPort("COM4", 115200);

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

    // Update is called once per frame
    void Update()
    {
      try {
        if(sp.IsOpen){
          message = sp.ReadByte();
          print(message);
        }
      }
      catch (System.Exception ex){
        ex = new System.Exception();
      }

      if(message == 49){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Large Diameter Grasp");
          hand.GetComponent<Animator>().Play("Hand|LargeDiameter");
        }

        resetKey = 1;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 50){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Small Diameter Grasp");
          hand.GetComponent<Animator>().Play("Hand|SmallDiameter");
        }

        resetKey = 2;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 51){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Palmar Pinch");
          hand.GetComponent<Animator>().Play("Hand|PalmarPinch");
        }

        resetKey = 3;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 52){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Ball Grasp");
          hand.GetComponent<Animator>().Play("Hand|Sphere");
        }

        resetKey = 4;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }
      if(message == 53){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Lateral Grasp");
          hand.GetComponent<Animator>().Play("Hand|Lateral");
        }

        resetKey = 5;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 54){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Thumb + One");
          hand.GetComponent<Animator>().Play("Hand|Thumb1");
        }

        resetKey = 6;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 55){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Thumb + Two");
          hand.GetComponent<Animator>().Play("Hand|Thumb2");
        }
        
        resetKey = 7;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 56){
        if(resetKey != 0 && keyDict[resetKey] != message){
          Debug.Log("Resetting");
          Debug.Log(myDict[resetKey]);
          hand.GetComponent<Animator>().SetBool("reset", true);
          hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
          hand.GetComponent<Animator>().Play(myDict[resetKey]);
        } else {
          Debug.Log("Thumb + Three");
          hand.GetComponent<Animator>().Play("Hand|Thumb3");
        }

        resetKey = 8;
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      if(message == 57){
        Debug.Log("Resetting");
        Debug.Log(myDict[resetKey]);
        hand.GetComponent<Animator>().SetBool("reset", true);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], message);
        hand.GetComponent<Animator>().Play(myDict[resetKey]);

        // Reset values
        hand.GetComponent<Animator>().SetBool("reset", false);
        hand.GetComponent<Animator>().SetInteger(myDict[resetKey], 0);
      }

      // Resets
      if(message == 97){
        Debug.Log("Large Diameter Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|LargeDiameterReset");
        resetKey = 0;
      }

      if(message == 98){
        Debug.Log("Small Diameter Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|SmallDiameterReset");
        resetKey = 0;
      }

      if(message == 99){
        Debug.Log("Palmar Pinch Reset");
        hand.GetComponent<Animator>().Play("Hand|PalmarPinchReset");
        resetKey = 0;
      }

      if(message == 100){
        Debug.Log("Ball Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|Sphere Reset");
        resetKey = 0;
      }

      if(message == 101){
        Debug.Log("Lateral Grasp Reset");
        hand.GetComponent<Animator>().Play("Hand|LateralReset");
        resetKey = 0;
      }

      if(message == 102){
        Debug.Log("Thumb + One Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb1Reset");
        resetKey = 0;
      }

      if(message == 103){
        Debug.Log("Thumb + Two Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb2Reset");
        resetKey = 0;
      }

      if(message == 104){
        Debug.Log("Thumb + Three Reset");
        hand.GetComponent<Animator>().Play("Hand|Thumb3Reset");
        resetKey = 0;
      }

      message = 0;

      return;
    }
}
