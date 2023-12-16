using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class forcedUpdateMessage : MonoBehaviour {

    public TextMeshProUGUI Message;


    public void ShowMessage(string text)
    {
        Message.text = text;

    }


}
