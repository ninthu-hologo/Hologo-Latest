using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheCamera : MonoBehaviour {

    public Camera cameraToLookAt;


	// Use this for initialization
	void Start () {
		
	}
	
    void Update()
    {
       // Vector3 v = cameraToLookAt.transform.position - transform.position;
       // v.x = v.z = 0.0f;
       // transform.LookAt(cameraToLookAt.transform.position - v);
       // transform.Rotate(0, 180, 0);


        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.rotation = (cameraToLookAt.transform.rotation); // Take care about camera rotation

    }
}
