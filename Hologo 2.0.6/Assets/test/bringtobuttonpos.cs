using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bringtobuttonpos : MonoBehaviour {

    public GameObject buttonObj;
    public RectTransform buttonRect;

    public GameObject imageObj;
    public RectTransform imageRect;

    public CanvasScaler scaler;

    public void SendMeToLocation()
    {
        //  imageRect.anchoredPosition = new Vector2(buttonObj.transform.position.x * scaler.referenceResolution.x / Screen.width, 
        //buttonObj.transform.position.y * scaler.referenceResolution.y / Screen.height);
        float x;
        float y;

      // imageRect.position = new Vector2(buttonRect.position.x, buttonRect.position.y - 15f);

        if(buttonRect.position.x-imageRect.sizeDelta.x <=0)
        {
            //imageRect.position = new Vector2(buttonRect.position.x + imageRect.sizeDelta.x + 10f, buttonRect.position.y - 15f);
            x = buttonRect.position.x + imageRect.sizeDelta.x + 10f;
            Debug.Log("x is less");
        }else if(buttonRect.position.x + imageRect.sizeDelta.x >= Screen.width)
        {
           // x = buttonRect.position.x - imageRect.sizeDelta.x - 10f;
            x = buttonRect.position.x;
            Debug.Log("x is greater");
        }
        else
        {
            x = buttonRect.position.x;
            Debug.Log("x is ok");
        }

        if(buttonRect.position.y - imageRect.sizeDelta.y <= 0)
        {
            //imageRect.position = new Vector2(buttonRect.position.x + imageRect.sizeDelta.x + 10f, buttonRect.position.y - 15f);  
            y = buttonRect.position.y + imageRect.sizeDelta.y + 10f;

            Debug.Log("y is less");
        }else if(buttonRect.position.y + imageRect.sizeDelta.y >= Screen.height)
        {
           // y = buttonRect.position.y - imageRect.sizeDelta.y - 10f;
            y = buttonRect.position.y - 15f;
            Debug.Log("y is greater");
        }
        else
        {
            y = buttonRect.position.y - 15f;
            Debug.Log("y is ok");
        }


      imageRect.position = new Vector2(x, y);


    }



 




}
