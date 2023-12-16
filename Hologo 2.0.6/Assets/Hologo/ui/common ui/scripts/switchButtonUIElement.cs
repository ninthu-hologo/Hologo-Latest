using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class switchButtonUIElement : MonoBehaviour
{

    // this is the button state. can be set in inspector
    [SerializeField]
    bool buttonState;
    [SerializeField]
    private Image ButtonOutline;
    [SerializeField]
    private CanvasGroup ButtonOnColor;

    [Tooltip("response to invoke when toggled")]
    public eventBool buttonResponse;

    // Start is called before the first frame update
    void Start()
    {
       // toggle(buttonState);
    }

    public void buttonToggle()
    {
        toggle(buttonState);
    }


    public void setToggleDefault(bool state)
    {
        
        if (state)
        {
            ButtonOnColor.alpha = 1;
            Color c = ButtonOutline.color;
            ButtonOutline.color = new Color(c.r, c.g, c.b, 0f);
            LeanTween.moveX(ButtonOutline.rectTransform, 35.6f, 0.2f).setEase(LeanTweenType.easeSpring);
        }
        else
        {
            ButtonOnColor.alpha = 0;
            Color c = ButtonOutline.color;
            ButtonOutline.color = new Color(c.r, c.g, c.b, 1f);
            LeanTween.moveX(ButtonOutline.rectTransform, 15.5f, 0.2f).setEase(LeanTweenType.easeSpring);
        }
    }

    public void OnToggleListener(eventBool action)
    {
        buttonResponse = action;
    }


    void toggle(bool Status)
    {
        Debug.Log("switchbuttonui element " + Status);
        if (!Status)
        {
            ButtonOnColor.alpha = 1;
            Color c = ButtonOutline.color;
            ButtonOutline.color = new Color(c.r, c.g, c.b, 0f);
            LeanTween.moveX(ButtonOutline.rectTransform, 35.6f, 0.2f).setEase(LeanTweenType.easeSpring);
            buttonState = true;
        }
        else
        {
            ButtonOnColor.alpha = 0;
            Color c = ButtonOutline.color;
            ButtonOutline.color = new Color(c.r, c.g, c.b, 1f);
            LeanTween.moveX(ButtonOutline.rectTransform, 15.5f, 0.2f).setEase(LeanTweenType.easeSpring);
            buttonState = false;
        }

        buttonResponse.Invoke(buttonState);
    }
}

[System.Serializable]
public class eventBool : UnityEvent<bool> { };