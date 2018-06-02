using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBPhotoListener : MonoBehaviour {
    private PythonClient clientObject;

	// Use this for initialization
	void Start () {
        clientObject = GameObject.Find("/LogicScripts").GetComponent<PythonClient>();
	}
	
	// Update is called once per frame
	void Update () {
		Texture2D tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
		// byte[] rawBytes = {}
		tex.LoadRawTextureData(clientObject.serverConnection.messageBuffer);
		tex.Apply();
		GetComponent<Renderer>().material.mainTexture = tex;
		Debug.Log(clientObject.serverConnection.messageBuffer);
	}
}
