using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class portalDoor : MonoBehaviour
{

    [SerializeField]
    private Animator doorAnimator;

    [SerializeField]
    public eventBool doorEvent;


    [SerializeField]
    public GameObject backPlane; // shariz 2.0.2 10th Dec 2019


    public void openDoor()
    {
        doorAnimator.SetBool("door_open", true);
    }

    public void closeDoor()
    {
        doorAnimator.SetBool("door_open", false);
        Debug.Log("run closeDoor");
    }

    public void closedDoorEvent()
    {
        doorEvent.Invoke(false);
        Debug.Log("run closed Door event");
    }

    public void openDoorEvent()
    {
        doorEvent.Invoke(true);
    }

}
