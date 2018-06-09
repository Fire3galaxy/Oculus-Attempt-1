using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotationListener : MonoBehaviour {
	private PythonClient clientObject;
	private GameObject headObject;
	private float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
		clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();
		headObject = GameObject.Find("/OVRCameraRig/TrackingSpace/CenterEyeAnchor");
	}
	
	// Update is called once per frame
	void Update () {
		float yaw = headObject.transform.eulerAngles.y;
		float pitch = headObject.transform.eulerAngles.x;
		if (yaw > 180.0f) yaw -= 360.0f;
		if (pitch > 180.0f) pitch -= 360.0f;

		Debug.Log("Yaw: " + yaw + ", Pitch: " + pitch);

		// elapsedTime += Time.deltaTime;
		// if (clientObject.serverConnection.isConnected) {
		// 	if (elapsedTime >= clientObject.SendFrequency) {
				
		// 	}
		// }
	}
}
