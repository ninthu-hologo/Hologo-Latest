using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;

public class localize_json : MonoBehaviour
{
    public List<localizationListDataClass> localization;
    public localizaData locData;
    public bool on = false;

    // Start is called before the first frame update
    void Start()
    {
        locData.localization = localization;

    }

    // Update is called once per frame
    void Update()
    {
        //if(on)
        //{
        //    string json = JsonUtility.ToJson(locData, true);
        //    readWriteData.WriteFileToDisk(false, json,"lan", "somefile.txt");
        //    Debug.Log(json);
        //    on = false;
        //}
    }
}

[System.Serializable]
public class localizaData
{
    public List<localizationListDataClass> localization;
}