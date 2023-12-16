using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class del_storeItem : MonoBehaviour {

    public RectTransform image;
    float aspectRatio = 1.62f;

    public void ResizeImage(float height)
    {
        float width = height * aspectRatio;
        image.sizeDelta= new Vector2(width, 0);
    }

}
