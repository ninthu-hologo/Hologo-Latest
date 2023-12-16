using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Hologo_Lean_Rotate : MonoBehaviour
{

    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers;

    [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("Does rotation require an object to be selected?")]
    public LeanSelectable RequiredSelectable;

    [Tooltip("The camera we will be moving (None = MainCamera)")]
    public Camera Camera;

    [Tooltip("The rotation axis used for non-relative rotations")]
    public Vector3 RotateAxis = Vector3.forward;

    [Tooltip("Should the rotation be performanced relative to the finger center?")]
    public bool Relative;

    public float RotationSpeed = 2f;

    public Transform RotateUpDownContainer;

    public Transform Rotater;

#if UNITY_EDITOR
    protected virtual void ResetMe()
    {
        Start();
    }
#endif

    protected virtual void Start()
    {
        if (RequiredSelectable == null)
        {
            RequiredSelectable = GetComponent<LeanSelectable>();
        }
    }

    protected virtual void Update()
    {
        // If we require a selectable and it isn't selected, cancel rotation
        if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
        {
            return;
        }

        // Get the fingers we want to use
        var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

        // Calculate the rotation values based on these fingers
        var center = LeanGesture.GetScreenCenter(fingers);
        var degrees = LeanGesture.GetScreenDelta(fingers).x / RotationSpeed * -1f; //magic number
        var degreesup = LeanGesture.GetScreenDelta(fingers).y / RotationSpeed * -1f; //magic number

        // Perform the rotation
        Rotate(center, degrees, degreesup);
    }

    private void Rotate(Vector3 center, float degrees, float degrees2)
    {
        if (Relative == true)
        {
            // Make sure the camera exists
            var camera = LeanTouch.GetCamera(Camera, gameObject);

            if (camera != null)
            {
                // World position of the reference point
                var worldReferencePoint = camera.ScreenToWorldPoint(center);

                // Rotate the transform around the world reference point
                //  transform.RotateAround(worldReferencePoint, camera.transform.forward, degrees);
                // transform.RotateAround(worldReferencePoint, camera.transform.up, degrees2);
                RotateUpDownContainer.Rotate(Vector3.right * Time.deltaTime * degrees2*15 , Space.World);

                // ...also rotate around the World's Y axis
                RotateUpDownContainer.Rotate(Vector3.up * Time.deltaTime * degrees*30);
            }
           
        }
        else
        {
            Rotater.rotation *= Quaternion.AngleAxis(degrees, RotateAxis);
        }
    }
}
