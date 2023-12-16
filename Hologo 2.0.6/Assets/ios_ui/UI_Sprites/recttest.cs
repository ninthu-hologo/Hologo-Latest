using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recttest : MonoBehaviour {

    public RectTransform myRect;

    public bool showRect;
    public bool position;
    public string name = "myRect";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(position)
        {
            string p = "new Vector3(" + myRect.position.x + "f," + myRect.position.y + "f," + myRect.position.z + "f)";
            Debug.Log(p);

        }


        if (showRect)
        {
            Debug.Log(myRect.name);
            string a =name+ ".offsetMax = new Vector2(" + myRect.offsetMax.x+"f,"+myRect.offsetMax.y+"f);";
            string b = name +".offsetMin = new Vector2(" + myRect.offsetMin.x + "f," + myRect.offsetMin.y + "f);";
            string c = name +".pivot = new Vector2(" + myRect.pivot.x + "f," + myRect.pivot.y + "f);";
            string d = name +".anchorMax = new Vector2(" + myRect.anchorMax.x + "f," + myRect.anchorMax.y + "f);";
            string e = name +".anchorMin = new Vector2(" + myRect.anchorMin.x + "f," + myRect.anchorMin.y + "f);";
            string f = name + " size delta x:" + myRect.sizeDelta.x + " size delta y:" + myRect.sizeDelta.y;
            Debug.Log(a +"\n"+b+ "\n" +c+ "\n" +d+ "\n" +e);
            Debug.Log(f);
        }

	}
}
