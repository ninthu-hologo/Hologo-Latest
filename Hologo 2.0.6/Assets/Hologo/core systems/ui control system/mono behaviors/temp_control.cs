using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_control : MonoBehaviour
{

    public LeanSelect UI_LeanSelect;

    public void FingerDown(LeanFinger finger)
    {
        if (UI_LeanSelect.CurrentSelectables.Count <= 0)
        {
            //Debug.Log("not selected");
            return;
        }

        for (var i = 0; i < LeanSelectable.Instances.Count; i++)
        {
            var selectable = LeanSelectable.Instances[i];

            IUIControl UIcont = selectable.GetComponent<IUIControl>();
            if (UIcont == null)
                return;
            // Is or was this selected?
            if (selectable.IsSelected == true || selectable.SelectingFinger == finger)
            {
                UIcont.OnSelected();
            }
            else
            {
                UIcont.OnDeSelected();
            }
        }
    }

    public void FingerUp(LeanFinger finger)
    {

        for (var i = 0; i < LeanSelectable.Instances.Count; i++)
        {
            var selectable = LeanSelectable.Instances[i];

            IUIControl UIcont = selectable.GetComponent<IUIControl>();
            if (UIcont == null)
                return;
            // Is or was this selected?
            if (selectable.IsSelected == true || selectable.SelectingFinger == finger)
            {
                UIcont.OnFingerUp();
            }
        }
    }



    public void SwippedButtonAction(LeanFinger finger)
    {

        for (var i = 0; i < LeanSelectable.Instances.Count; i++)
        {
            var selectable = LeanSelectable.Instances[i];

            IUIControl UIcont = selectable.GetComponent<IUIControl>();
            if (UIcont == null)
                return;
            // Is or was this selected?
            if (selectable.IsSelected == true || selectable.SelectingFinger == finger)
            {
                UIcont.OnSwiped();
            }
            else
            {
                UIcont.OnDeSelected();
            }
        }
    }


    public void TapButtonAction(LeanFinger finger)
    {
        // Debug.Log("Tap action");
        if (UI_LeanSelect.CurrentSelectables.Count <= 0)
        {
            // Debug.Log("not selected");
            return;
        }

        for (var i = 0; i < LeanSelectable.Instances.Count; i++)
        {
            var selectable = LeanSelectable.Instances[i];

            IUIControl UIcont = selectable.GetComponent<IUIControl>();

            if (UIcont == null)
                return;
            // Is or was this selected?
            if (selectable.IsSelected == true || selectable.SelectingFinger == finger)
            {
                UIcont.OnTapped();
            }
            else
            {
                UIcont.OnDeSelected();
            }
        }
    }
    public void HeldButtonAction(LeanFinger finger)
    {
        for (var i = 0; i < LeanSelectable.Instances.Count; i++)
        {
            var selectable = LeanSelectable.Instances[i];

            IUIControl UIcont = selectable.GetComponent<IUIControl>();
            if (UIcont == null)
                return;
            // Is or was this selected?
            if (selectable.IsSelected == true || selectable.SelectingFinger == finger)
            {
                UIcont.OnHeld();
            }
        }
    }
}

