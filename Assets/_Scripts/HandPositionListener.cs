using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpConnect;

// Sends positions of touch controllers to python script
public class HandPositionListener : MonoBehaviour {
    public Connector serverConnection = new Connector();
    public GameObject rightHand;
    public GameObject leftHand;

    private enum States { Setup, Playtime };
    private States currState = States.Setup;
    private float elapsedTime = 0.0f;
    
    // Order is forward, up, back, down, out
    private Vector3[] leftHandBounds = new Vector3[5];
    private Vector3[] rightHandBounds = new Vector3[5];

    private const string LARM = "LArm";
    private const string RARM = "RArm";

    // Use this for initialization
    void Start () {
        // Start server connection
        Debug.Log(serverConnection.fnConnectResult("localhost", 10000));

        // Put OVRCameraRig and LocalAvatar into scene
        if (rightHand == null)
            rightHand = GameObject.Find("LocalAvatar/hand_right");
        if (leftHand == null)
            leftHand = GameObject.Find("LocalAvatar/hand_left");
    }
	
	// Update is called once per frame
	void Update () {
        // Report hand positions to Python script once per second
        if (currState == States.Playtime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1.0f)
            {
                // Log hand positions
                Debug.Log("Right hand: " + rightHand.transform.position);
                Debug.Log("Left hand: " + leftHand.transform.position);

                // Send arm positions to server
                if (serverConnection.isConnected) {
                    serverConnection.fnPacketTest("MOVE|" + LARM + "|" + leftHand.transform.position); 
                    serverConnection.fnPacketTest("MOVE|" + RARM + "|" + rightHand.transform.position); 
                }

                // Reset timer
                elapsedTime = 0.0f;
            }
        }
        // Record limits of hands to scale to NAO hands
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                if (rightHand != null)
                {
                    Debug.Log("Right hand: " + rightHand.transform.position);
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            {
                if (leftHand != null)
                {
                    Debug.Log("Left hand: " + leftHand.transform.position);
                }
            }
        }
    }

    void OnApplicationQuit() {
        try { serverConnection.fnDisconnect(); } catch { }
    }
}
