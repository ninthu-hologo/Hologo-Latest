using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class iOSUIDebugScript : MonoBehaviour {



    public GameObject hider;
    public bool showDebug = true;

    public TextMeshProUGUI textthis;



    public void ShowText(string t)
    {
        //if(showDebug)
        //{
            textthis.text = t;
       // }
     
    }

    public void ButtonDebug()
    {
        if(showDebug)
        {
            //showDebug = false;
            hider.SetActive(false);
        }else
        {
           // showDebug = true;
            hider.SetActive(true);
        }
       // iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
    }
}
