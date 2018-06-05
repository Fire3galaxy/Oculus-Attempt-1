using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBPhotoListener : MonoBehaviour {
	public GameObject display;
	public float receiveFrequency = .5f;
    private PythonClient clientObject;
	private float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {
        clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (clientObject.serverConnection.isConnected && elapsedTime > receiveFrequency) {
			display.GetComponent<Renderer>().material.mainTexture = clientObject.serverConnection.GetImageTexture();
			elapsedTime = 0.0f;
		}
	}
}
