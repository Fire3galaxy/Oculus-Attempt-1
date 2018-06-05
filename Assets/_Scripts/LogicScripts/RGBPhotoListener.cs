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
		if (elapsedTime > receiveFrequency) {
			Texture2D tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
			tex.LoadRawTextureData(clientObject.serverConnection.messageBuffer);
			tex.Apply();
			display.GetComponent<Renderer>().material.mainTexture = tex;
			Debug.Log(clientObject.serverConnection.messageBuffer);

			elapsedTime = 0.0f;
		}
	}
}
