using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveSwitcher : MonoBehaviour {
	public GameObject FirstPersonViews, ThirdPersonViews;
	public GameObject FirstPersonScreen, ThirdPersonScreen;
	private RGBPhotoListener cameraScript;
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown(OVRInput.RawButton.B, OVRInput.Controller.RTouch) || Input.GetKeyDown("s")) {
			FirstPersonViews.SetActive(!FirstPersonViews.activeSelf);
			ThirdPersonViews.SetActive(!ThirdPersonViews.activeSelf);

			if (FirstPersonViews.activeSelf)
				cameraScript.display = FirstPersonScreen;
			else
				cameraScript.display = ThirdPersonScreen;
		}
	}
}
