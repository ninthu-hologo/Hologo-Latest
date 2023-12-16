using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class someMethodTesting : MonoBehaviour {

    public delegate string myname(string n);


	// Use this for initialization
	void Start () {

        // methodrunner("hologo", addsomethingtoname);
        // makeaDictionary();
        somesplit();

      

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void somesplit()
    {
        string c = "human_character.dlc";
        string[] d = c.Split('.');
        Debug.Log(d[0]);

    }



    void makeaDictionary(){

        //somevalueclass cc = new somevalueclass();
        //cc.productable_id = new int[1];
        //cc.productable_id[0] = 302;

        //string jsons = JsonUtility.ToJson(cc);
        //someejjeje[] newx = new someejjeje[1];
        //newx[0] = new someejjeje();
        //newx[0].key = "productable_id[]";
        //newx[0].value = (302).ToString();

//        jjjjj nnn = new jjjjj();
//        nnn.data = new someejjeje[1];
//        nnn.data[0] = new someejjeje();
//        nnn.data[0].key = "productable_id[]";
//        nnn.data[0].value = (302).ToString();
//        string json= JsonUtility.ToJson(nnn);
//        string[] words = json.Split('[');
//        string dd = words[2];
////        string[] g = dd.Split(']');
       //// string f = words[1] + "[]"+ g[1];
    }

    public string addsomethingtoname(string n)
    {
        return n + "..something added";
    }


    public void methodrunner(string n, myname my)
    {
        string f = my(n);
        Debug.Log(f);
    }
}


[Serializable]
public class somevalueclass
{
    public int[] productable_id;
 
}


[Serializable]
public class jjjjj
{
    public someejjeje[] data;
}


[Serializable]
public class someejjeje
{
    public string key;
    public string value;
}
