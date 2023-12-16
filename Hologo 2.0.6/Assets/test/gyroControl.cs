using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyroControl : MonoBehaviour
{
    public GameObject rotationX;
    public GameObject rotationY;
    private bool enableGyroInput;
    // Start is called before the first frame update

    

    public void enableGyroNav(bool state)
    {
        enableGyroInput = state;
        Input.gyro.enabled = state;
        if(!state)
        {
            rotationY.transform.rotation = Quaternion.identity;
            rotationX.transform.rotation = Quaternion.identity;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (enableGyroInput)
        {
            rotationY.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
            rotationX.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
        }
    }


}
