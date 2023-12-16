using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoMyTarget : MonoBehaviour {




    public float lerpTime = 2f;
    public float Speed = 0.5f;
    public float currentLerpTime;
    private float kit;

    //float moveDistance = 10f;

    //Vector3 startPos;
    //Vector3 endPos;
    public Transform Target;


    void Update()
    {
        //increment timer once per frame
        currentLerpTime += Time.deltaTime * Speed;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = 0f;
        }

        //lerp!
        //float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(transform.position, Target.position, Mathf.SmoothStep(0, 1, Time.deltaTime * Speed));
    }

}
