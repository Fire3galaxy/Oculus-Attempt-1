﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpConnect;

// Sends positions of touch controllers to python script
public class HandPositionListener : MonoBehaviour {
    private PythonClient clientObject;
    public GameObject rightHand;
    public GameObject leftHand;
    public Text textTitle, textUI, textStats;

    private enum States { Setup, Playtime };
    private enum SetupStates { AskForArmsDown, AskForArmsUp, Done };
    private States currState = States.Setup;
    private SetupStates currSetupState = SetupStates.AskForArmsDown;
    private float elapsedTime = 0.0f;
    public float SendFrequency = 1.0f;
    
    // Shoulder pos, Arm length (Arm length should be the same for both, but will be recorded)
    private Vector3[] leftHandDimens = new Vector3[2];
    private Vector3[] rightHandDimens = new Vector3[2];
    public float NaoArmLength = .21f;

    private const string LARM = "LArm";
    private const string RARM = "RArm";

    // Use this for initialization
    void Start () {
        clientObject = GameObject.Find("/LogicScripts/PythonClient").GetComponent<PythonClient>();

        // Put OVRCameraRig and LocalAvatar into scene
        if (rightHand == null)
            rightHand = GameObject.Find("LocalAvatar/hand_right");
        if (leftHand == null)
            leftHand = GameObject.Find("LocalAvatar/hand_left");
    }

    Vector3 scaleToNao(Vector3 armPosition, Vector3 shoulderPosition) {
        return Vector3.Normalize(armPosition - shoulderPosition) * NaoArmLength;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            Debug.Log("In update");
        }

        // Report hand positions to Python script once per second
        if (currState == States.Playtime)
        {
            textTitle.text = "Robot Control Phase";
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= SendFrequency)
            {
                // Send arm positions to server
                if (clientObject.serverConnection.isConnected) {
                    // Log hand positions
                    Debug.Log("Right hand: " + rightHand.transform.position);
                    Debug.Log("Left hand: " + leftHand.transform.position);

                    clientObject.serverConnection.fnPacketTest("MOVE|" + LARM + "|" + scaleToNao(leftHand.transform.position, leftHandDimens[0])); 
                    clientObject.serverConnection.fnPacketTest("MOVE|" + RARM + "|" + scaleToNao(rightHand.transform.position, rightHandDimens[0])); 
                }

                // Reset timer
                elapsedTime = 0.0f;
            }
        }
        // Record limits of hands to scale to NAO hands
        // FIXME: Add setup calibration of limits code
        else
        {
            switch (currSetupState) {
                case SetupStates.AskForArmsDown:
                    textUI.text = "Place your left and right hands straight down at your sides. Then press either your left or right hand trigger.";
                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger | OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch | OVRInput.Controller.LTouch)) {
                        // Initial left and right hand positions
                        leftHandDimens[0] = leftHand.transform.position;
                        rightHandDimens[0] = rightHand.transform.position;
                        textStats.text = "Left hand: " + leftHandDimens[0] + "\nRight Hand: " + rightHandDimens[0];
                        currSetupState = SetupStates.AskForArmsUp;
                    }
                    break;
                case SetupStates.AskForArmsUp:
                    textUI.text = "Raise your left and right hands straight up. Then press either your left or right hand trigger.";
                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger | OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch | OVRInput.Controller.LTouch)) {
                        Vector3 leftArmLength = (leftHand.transform.position - leftHandDimens[0]) / 2;
                        // Shoulder pos = halfway between lowered hand and raised hand
                        leftHandDimens[0] = leftHandDimens[0] + leftArmLength;
                        // Arm length - magnitude of vector from lowered hand to shoulder
                        leftHandDimens[1] = new Vector3(0, Vector3.Magnitude(leftArmLength), 0);
                        
                        Vector3 rightArmLength = (rightHand.transform.position - rightHandDimens[0]) / 2;
                        // Shoulder pos = halfway between lowered hand and raised hand
                        rightHandDimens[0] = rightHandDimens[0] + rightArmLength;
                        // Arm length - magnitude of vector from lowered hand to shoulder
                        rightHandDimens[1] = new Vector3(0, Vector3.Magnitude(rightArmLength), 0);

                        textStats.text = "Recorded Arm Lengths: " + leftHandDimens[1].y + " (L), " + rightHandDimens[1].y + " (R)\n"
                            + "Shoulders: " + leftHandDimens[0] + " (L), " + rightHandDimens[0] + " (R)\n";
                        currSetupState = SetupStates.Done;
                    }
                    break;
                default: // Done
                    textUI.text = "Calibrated! While controlling the robot, please keep your torso still. I use your arm length and shoulder position to "
                        + "scale down your movements to the robot's.";
                    currState = States.Playtime;
                    break;
            }
        }
    }
}
