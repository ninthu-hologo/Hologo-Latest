using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class disableEnableExperienceInteraction : MonoBehaviour
{
    [SerializeField]
    Hologo_LeanScale leanScale;
    [SerializeField]
    LeanTranslate leanMove;
    [SerializeField]
    LeanSelectable leanSelect;
    [SerializeField]
    Hologo_Lean_Rotate leanRotate;
    [SerializeField]
    LeanFingerUp fingerUp;



    public void setExpInteractionState(bool state)
    {
        leanRotate.gameObject.SetActive(state);
        leanScale.enabled = state;
        leanMove.enabled = state;
        leanSelect.enabled = state;
        leanRotate.enabled = state;
        fingerUp.enabled = state;
    }


    public void disableEnableManipulation(bool state)
    {
        leanScale.enabled = state;
        leanMove.enabled = state;
        leanSelect.enabled = state;
        leanRotate.enabled = state;
        fingerUp.enabled = state;
    }

}
