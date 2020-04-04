using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public int buttonPressed;

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

            buttonPressed = 0;
        }

        private void Update() {
            // Select component
            if (Input.GetKey("1"))
            {
                buttonPressed = 1;
                Debug.Log("Selected Shoulder");
            }

            if (Input.GetKey("2"))
            {
                buttonPressed = 2;
                Debug.Log("Selected Forearm");
            }
            
            if (Input.GetKey("3"))
            {
                buttonPressed = 3;
                Debug.Log("Selected Hand");
            }

            if (Input.GetKey("4"))
            {
                buttonPressed = 4;
                Debug.Log("Selected fingers");
            }

            // Move depending on which component selected
            switch(buttonPressed)
            {
                // Shoulder
                case 1:
                    if (Input.GetKey(KeyCode.W))
                    {
                        shoulderLeft.transform.Rotate(0.0f, 0.0f, 0.75f, Space.Self);
                        Debug.Log("Rotate arm up");
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        shoulderLeft.transform.Rotate(0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate arm right");
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        shoulderLeft.transform.Rotate(0.0f, 0.0f, -0.75f, Space.Self);
                        Debug.Log("Rotate arm down");
                    }    

                    if (Input.GetKey(KeyCode.D))
                    {
                        shoulderLeft.transform.Rotate(-0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate arm left");
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        shoulderLeft.transform.rotation = originalShoulderL;
                        Debug.Log("Reset");
                    }

                    break;

                // Forearm
                case 2:
                    if (Input.GetKey(KeyCode.W))
                    {
                        forearmLeft.transform.Rotate(0.0f, 0.0f, 0.75f, Space.Self);
                        Debug.Log("Rotate forearm up");
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        forearmLeft.transform.Rotate(0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate forearm right");
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        forearmLeft.transform.Rotate(0.0f, 0.0f, -0.75f, Space.Self);
                        Debug.Log("Rotate forearm down");
                    }    

                    if (Input.GetKey(KeyCode.D))
                    {
                        forearmLeft.transform.Rotate(-0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate forearm left");
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        forearmLeft.transform.rotation = originalForearmL;
                        Debug.Log("Reset");
                    }

                    break;

                // Hand
                case 3:
                    if (Input.GetKey(KeyCode.W))
                    {
                        handLeft.transform.Rotate(0.0f, 0.0f, 0.75f, Space.Self);
                        Debug.Log("Rotate hand up");
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        handLeft.transform.Rotate(-0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate hand right");
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        handLeft.transform.Rotate(0.0f, 0.0f, -0.75f, Space.Self);
                        Debug.Log("Rotate hand down");
                    }    

                    if (Input.GetKey(KeyCode.D))
                    {
                        handLeft.transform.Rotate(0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate hand right");
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        handLeft.transform.rotation = originalHandL;
                        Debug.Log("Reset");
                    }

                    break;
                    
                // Fingers
                case 4:
                    if (Input.GetKey(KeyCode.W))
                    {
                        handLeft.transform.Rotate(0.0f, 0.0f, 0.75f, Space.Self);
                        Debug.Log("Rotate fingers up");
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        handLeft.transform.Rotate(-0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate fingers right");
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        handLeft.transform.Rotate(0.0f, 0.0f, -0.75f, Space.Self);
                        Debug.Log("Rotate fingers down");
                    }    

                    if (Input.GetKey(KeyCode.D))
                    {
                        handLeft.transform.Rotate(0.75f, 0.0f, 0.0f, Space.Self);
                        Debug.Log("Rotate fingers right");
                    }

                    if (Input.GetKey(KeyCode.Space))
                    {
                        handLeft.transform.rotation = originalHandL;
                        Debug.Log("Reset");
                    }

                    break;
            }


            /*
            if (Input.GetKey(KeyCode.W)) 
            {
                transform.position += new Vector3(0, 0, +1);

            }

            if (Input.GetKey(KeyCode.A)) 
            {
                transform.position += new Vector3(+1, 0, 0);

            }

            if (Input.GetKey(KeyCode.S)) 
            {
                transform.position += new Vector3(0, 0, -1);

            }
            
            if (Input.GetKey(KeyCode.D)) 
            {
                transform.position += new Vector3(-1, 0, 0);

            }
            */
        }
        
    }

}