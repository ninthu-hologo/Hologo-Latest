using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class updatework : MonoBehaviour ,IUIControl{



    public GameObject hidden_box;
    public RectTransform LoadFire;
    public ScrollRect ThisScroller;
    public LayoutElement thiselement;
    public float startpos;
    public float multiplier = 0.3f;
    public float animateMult = 20;
    public bool stopSize = true;
    public Animator myAnim;
    public CanvasGroup myCg;
    public TextMeshProUGUI dtext;
    public TextMeshProUGUI dtext1;
    public TextMeshProUGUI dtext2;
    float chnagedelta;
	// Use this for initialization
	void Start () {
        startpos = LoadFire.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void OnRectTransformDimensionsChange()
    { 
        startpos = LoadFire.transform.position.y;
    }

    public void SetStart()
    {
        startpos = LoadFire.transform.position.y;
    }

    public void UpdateMeValue()
    {
        
        chnagedelta = startpos - LoadFire.transform.position.y;
        if (stopSize)
        {
            
            thiselement.preferredHeight = chnagedelta / multiplier;
           
           // AnimationState animationState = anim["load_animation"];
            //myCg.alpha = 
            myAnim.Play("load_animation", 0, chnagedelta /animateMult);
        }
        if(thiselement.preferredHeight >=60)
        {
            thiselement.preferredHeight = 60;
            stopSize = false;
            if (stopSize == false)
            {
                if (ThisScroller.normalizedPosition.y >= 1)
                {
                    startUpdatingevents();
                }  
            }

            // ThisScroller.enabled = false;
           // startUpdatingevents();

        }
        dtext.text ="sp > "+startpos+ " /al > " + LoadFire.transform.position.y + " /cl > " + chnagedelta;
        dtext1.text = ThisScroller.normalizedPosition.ToString();
    }




    public void startUpdatingevents()
    {
        LeanTween.delayedCall(5f,sttst);

       
    }

    void sttst()
    {
        LeanTween.value(this.gameObject, updateLe, thiselement.preferredHeight, 0, 0.4f).setEaseOutExpo().setOnComplete(endthis);
    }

    void updateLe(float val)
    {
        thiselement.preferredHeight = val;

    }

    void endthis()
    {
        LeanTween.cancelAll();
        stopSize = true;
        thiselement.preferredHeight = 0;
    }

    public void OnSelected()
    {
        startpos = LoadFire.transform.position.y;
    }

    public void OnFingerUp()
    {
      //  throw new System.NotImplementedException();
    }

    public void OnDeSelected()
    {
       // throw new System.NotImplementedException();
    }

    public void OnSwiped()
    {
       // throw new System.NotImplementedException();
    }

    public void OnTapped()
    {
       // throw new System.NotImplementedException();
    }

    public void OnHeld()
    {
       // throw new System.NotImplementedException();
    }
}
