using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scrolldragSizeRC : MonoBehaviour, IBeginDragHandler,IEndDragHandler
{

    public ScrollRect Scroller;
    public RectTransform hidden_Rect_element;
    float PosMulti;
    bool t = true;
    bool x = true;
    bool k = false;
    float stoppedValue;
    float lastValue;

    public RectTransform contentX;

    private void OnEnable()
    {
        hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, 0f);
        t = false;  
        lastValue = 0f;
    }

    private void OnDisable()
    {
        hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, 0f);
        t = false;  
        lastValue = 0f;
    }


    //private void OnRectTransformDimensionsChange()
    //{
    //   // hidden_Rect_element.preferredHeight = 0f;
    //    hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, 0f);
    //}

    public void Valss(Vector2 myvec)
    {
        Debug.Log(myvec.ToString());
    }


    public void UpdateMeValue(Vector2 myvec)
    {
        Debug.Log(contentX.offsetMin.y);
        if(lastValue <= contentX.offsetMin.y)
        {
            return;
        }
       
        if (t && x)
        {
            
            PosMulti += myvec.y *-1;

            // hidden_Rect_element.preferredHeight = PosMulti;
            hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, PosMulti);
        }

        if (hidden_Rect_element.offsetMax.y <= -60)
        {
            hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, -60);
            if (x)
            {
                StartOT();
                x = false;
                t = false;
                k = true;
            }
        }

        lastValue = contentX.offsetMin.y;
    }


    private void StartOT()
    {
        LeanTween.delayedCall(3f, SequenceStart);
    }

    void SequenceStart()
    {
       // LeanTween.value(this.gameObject, updateElementLayout, 60, 0, 0.4f).setOnComplete(ReturnToNormal);
        LeanTween.size(hidden_Rect_element, new Vector2(hidden_Rect_element.offsetMax.x, 0), 0.4f).setOnComplete(ReturnToNormal);
    }

    //void updateElementLayout(float val)
    //{
    //    hidden_Rect_element.preferredHeight = val;

    //}

    void ReturnToNormal()
    {
        lastValue = 0f;
        PosMulti = 0f;
        hidden_Rect_element.offsetMax = new Vector2(hidden_Rect_element.offsetMax.x, 0f);
        k = false;
        x = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if(k)
        {
            t = false;  
        }
        else
        {
            t = true;  

        }
    }

    void rectback()
    {
        //if (stoppedValue > 0)
        //{
            
        LeanTween.size(hidden_Rect_element, new Vector2(hidden_Rect_element.offsetMax.x, 0), 0.4f).setOnComplete(ReturnToNormal);
       // }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!k)
        {
            t = false;
            PosMulti = 0f;
            //stoppedValue = hidden_Rect_element.preferredHeight;
            rectback();
        }
       
    }
}
