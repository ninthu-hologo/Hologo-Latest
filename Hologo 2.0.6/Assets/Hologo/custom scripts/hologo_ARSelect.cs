using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologo_ARSelect : MonoBehaviour
{

    public void SelectArModel(GameObject go)
    {
        LeanSelectable select = go.GetComponentInParent<LeanSelectable>();
        select.Select(true);
    }

}
