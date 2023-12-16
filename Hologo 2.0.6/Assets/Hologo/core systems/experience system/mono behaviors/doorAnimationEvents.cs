using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimationEvents : MonoBehaviour
{

    [SerializeField]
    private portalDoor pDoor;

    public void doorIsOpened()
    {
        pDoor.openDoorEvent();
    }

    public void doorIsClosed()

    {
        pDoor.closedDoorEvent();
    }
}
