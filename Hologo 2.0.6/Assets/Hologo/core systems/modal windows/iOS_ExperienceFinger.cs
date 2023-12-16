using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using UnityEngine.Events;

public class iOS_ExperienceFinger : MonoBehaviour, IUIControl
{
    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers = false;

    [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("Does translation require an object to be selected?")]
    public LeanSelectable RequiredSelectable;

    [SerializeField]
    public UnityEvent FingerTapActions;
    [SerializeField]
    public UnityEvent FingerHeldActions;

    public void OnDeSelected()
    {
        
    }

    public void OnFingerUp()
    {
        
    }

    public void OnHeld()
    {
        //Debug.Log("finger held on "+ this.gameObject.name);  
        FingerHeldActions.Invoke();
    }

    public void OnSelected()
    {
        //Debug.Log("Selected" + this.gameObject.name);
    }

    public void OnSwiped()
    {
        
    }

    public void OnTapped()
    {
        FingerTapActions.Invoke();
    }


    //// Update is called once per frame
    //protected virtual void Update()
    //{


    //    // If we require a selectable and it isn't selected, cancel translation
    //    if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
    //    {
    //        return;
    //    }


    //    // getting current fingers on screen
    //    var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

    //    if (fingers != null)
    //    {


    //    }


       

    //}
}
