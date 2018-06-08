using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveSwitcher : MonoBehaviour {
	public GameObject FirstPersonViews, ThirdPersonViews;
	private GameObject fpScreen = null, tpScreen = null;
	private RGBPhotoListener cameraScript;
	
	void Start() {
		cameraScript = GameObject.Find("LogicScripts").GetComponent<RGBPhotoListener>();

		if (FirstPersonViews != null) {
			foreach (Transform t in FirstPersonViews.transform) {
				if (t.gameObject.name == "Paper-sized Screen") {
					fpScreen = t.gameObject;
					break;
				}
				Debug.Log(t.gameObject.name);
			}
			Debug.Assert(fpScreen != null);
		} else {
			Debug.Assert(false);
		}

		if (ThirdPersonViews != null) {
			foreach (Transform t in ThirdPersonViews.transform) {
				if (t.name == "CameraDisplay") {
					tpScreen = t.gameObject;
					break;
				}
			}
			Debug.Assert(tpScreen != null);
		} else {
			Debug.Assert(false);
		}
	}

	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown(OVRInput.RawButton.B, OVRInput.Controller.RTouch) || Input.GetKeyDown("s")) {
			FirstPersonViews.SetActive(!FirstPersonViews.activeSelf);
			ThirdPersonViews.SetActive(!ThirdPersonViews.activeSelf);

			if (FirstPersonViews.activeSelf)
				cameraScript.display = fpScreen;
			else
				cameraScript.display = tpScreen;
		}
	}
}
