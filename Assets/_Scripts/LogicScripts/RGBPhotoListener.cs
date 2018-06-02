using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBPhotoListener : MonoBehaviour {
    private PythonClient clientObject;

	// Use this for initialization
	void Start () {
        clientObject = GameObject.Find("/LogicScripts/PythonClient").GetComponent<PythonClient>();

		// Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
		// byte[] rawBytes = {}
		// tex.LoadRawTextureData(rawBytes);
		// tex.Apply();
		// GetComponent<Renderer>().material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update () {
		string imageContents = clientObject.serverConnection.strMessage;
	}
}
