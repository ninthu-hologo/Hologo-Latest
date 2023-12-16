using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animate_rotate_ui : MonoBehaviour {

    float timer = 0;
    float speed = 5;
    public float w = 25;
    public float h = 25;

    public RectTransform button;


    private void Update()
    {
        timer += Time.deltaTime * speed;
        float x =Mathf.Cos(timer) * w;
        float y = Mathf.Sin(timer) * h;
        button.transform.localPosition = new Vector3(x, y, 1);
        //button.rect.position = new Vector3(x, y, 1);
    }





}
