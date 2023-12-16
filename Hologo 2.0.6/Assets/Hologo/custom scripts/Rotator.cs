using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    Transform goTrans;
    public float Speed = 20f;

	// Use this for initialization
	void Start () {
        goTrans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
        goTrans.transform.Rotate(Vector3.up * Time.deltaTime * Speed);

	}
}
