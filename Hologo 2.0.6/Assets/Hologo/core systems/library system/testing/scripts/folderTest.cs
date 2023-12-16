using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;

public class folderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(readWriteData.CombineFolderExcludeLast("somefolder", "fileName.dtm"));
        Debug.Log(readWriteData.GetPath("somefolder", "fileName.dtm"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
