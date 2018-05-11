using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameLogic : MonoBehaviour {
	private enum HandTypeStates {Left, Right};
	private enum HandStates {GetForward, GetUp, GetBack, GetDown, GetOut}

	private HandTypeStates handType = HandTypeStates.Left;
	private HandStates handState = HandStates.GetForward;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
