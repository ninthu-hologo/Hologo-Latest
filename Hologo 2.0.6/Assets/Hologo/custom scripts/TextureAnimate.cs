using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this will scroll/rotate the main texture of a gameObject this script is attached to
public class TextureAnimate : MonoBehaviour {
    // rotation speed
    public float GlodeRotationSpeed = 0.1f;
    //rotate left or right toggle
    public bool RotateLeft = true;
    //rotate on or off toggle
    public bool AnimateToggle = true;
    // horizontal or vertical rotation toggle
    public bool HorizontalAxis = true;
    private Vector2 savedOffset;
    private Renderer ren;
    

    void Start()
    {
        ren = GetComponent<Renderer>();
        savedOffset = ren.sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        if (AnimateToggle)
        {
            int dir = 1;
          
            if (RotateLeft == true)
            {
                dir = -1;
            }
            float axis = Mathf.Repeat(Time.time * GlodeRotationSpeed * dir, 1);
            Vector2 axisRotation;
            if(HorizontalAxis)
            {
                axisRotation = new Vector2(axis, savedOffset.y);
            }
            else
            {
                axisRotation = new Vector2(savedOffset.x, axis);
            }
            ren.sharedMaterial.SetTextureOffset("_MainTex", axisRotation);
        }
    }

    void OnDisable() // Hamid 7th March 2020 2.0.4
    {
        if(ren==null){
            return;
        }
        ren.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}