using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailRendererScaler : MonoBehaviour
{
    public TrailRenderer[] myTrailRenderers;
    Transform parentScale;
    // Start is called before the first frame update

    bool init = false;
    public void initTrailScaler(Transform scaler, TrailRenderer[] trs)
    {
        parentScale = scaler;
        //  myTrailRenderers = GetComponentsInChildren<TrailRenderer>();
        myTrailRenderers = trs;
        init = true;
    }


    void scaleTrails()
    {
        for (int i = 0; i < myTrailRenderers.Length; i++)
        {
            myTrailRenderers[i].startWidth = myTrailRenderers[i].startWidth * (parentScale.localScale.x/2);
           //myTrailRenderers[i].transform.localScale = new Vector3(myTrailRenderers[i].transform.localScale.x * parentScale.localScale.x, myTrailRenderers[i].transform.localScale.y * parentScale.localScale.x, myTrailRenderers[i].transform.localScale.z * parentScale.localScale.x);
           //Debug.Log(myTrailRenderers[i].startWidth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
            scaleTrails();
    }
}
