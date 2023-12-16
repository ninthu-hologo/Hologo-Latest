using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class get_fingerDelta : MonoBehaviour {

    public LeanSelectable RequiredSelectable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var fingers = LeanTouch.GetFingers(false, 1, RequiredSelectable);

        Debug.Log("finger delta >"+LeanGesture.GetScreenDelta(fingers));


	}
}
