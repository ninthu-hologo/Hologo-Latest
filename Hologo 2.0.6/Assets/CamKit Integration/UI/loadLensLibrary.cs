using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using Hologo2;

public class loadLensLibrary : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lensTiles; //all lensTiles that should be included in lens library

    [SerializeField]
    private GameObject recentlyUsedLenses_section, mostPopularLenses_section, comingSoon_section; //these gameobjects will be used to set as parents of the instantiating tiles (to make them scrollable)

    lensUIInfo data;
    string lensInfoFile;

    string[] recentlyUsed;
    string[] mostPopular;
    //string[] comingSoon;

    //lenses in init fields will be used when a user opens the UI for the first time
    [SerializeField]
    string[] recentlyUsed_init;
    [SerializeField]
    string[] mostPopular_init;
    [SerializeField]
    string[] comingSoon_init;


    [SerializeField]
    private GameObject userInfo_canvas;

    public void Start()
    {
        data = new lensUIInfo();
        lensInfoFile = Application.persistentDataPath + "/lensInfo.txt";

        //recentlyUsedLenses_section = GameObject.Find("recentlyUsed");
        //mostPopularLenses_section = GameObject.Find("MostPopular");
        comingSoon_section = GameObject.Find("ComingSoon");

        createInfo(); //create info file if not already exists

        loadContents();

        recentlyUsed = data.recentlyUsedLenses.Split(',');
        mostPopular = data.mostPopularLenses.Split(',');
        //comingSoon = data.comingSoonLenses.Split(',');
    }

    private void createInfo()
    {
        //create lensInfo.txt file for the first time if not already exists
        if (!File.Exists(lensInfoFile))
        {
            Debug.Log("LensInfo file does not exists, creating for the first time");

            data.recentlyUsedLenses = string.Join(",", recentlyUsed_init);
            data.mostPopularLenses = string.Join(",", mostPopular_init);
            //data.comingSoonLenses = string.Join(",", comingSoon_init);

            string dataJson = JsonConvert.SerializeObject(data);

            StreamWriter sw = new StreamWriter(lensInfoFile);
            sw.Write(dataJson);
            sw.Close();
        }
        else
        {
            Debug.Log("lensInfo file exists, not creating a new one");
        }
    }

    private void loadContents()
    {
        recentlyUsedLenses_section = GameObject.Find("recentlyUsed");
        mostPopularLenses_section = GameObject.Find("MostPopular");
        comingSoon_section = GameObject.Find("ComingSoon");

        //delete previous items
        foreach (Transform child in recentlyUsedLenses_section.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in mostPopularLenses_section.transform)
        {
            Destroy(child.gameObject);
        }

        loadInfo();
        //load new items
        for (int i = 0; i < recentlyUsed.Length; i++)
        {
            foreach(GameObject lensTile in lensTiles)
            {
                if(recentlyUsed[i] == lensTile.name)
                {
                    Instantiate(lensTile, recentlyUsedLenses_section.transform);
                }
            }
        }

        for(int j = 0; j < mostPopular.Length; j++)
        {
            foreach(GameObject lensTile in lensTiles)
            {
                if(mostPopular[j] == lensTile.name && !recentlyUsed.Contains(lensTile.name))
                {
                    Instantiate(lensTile, mostPopularLenses_section.transform);
                }
            }
        }

        //yield return null;
    }

    //save recently used lenses to local storage
    public void saveRecent(string lastLensName)
    {
        lensInfoFile = Application.persistentDataPath + "/lensInfo.txt";
        loadInfo();

        string[] recentlyUsed_new = new string[recentlyUsed.Length];
        recentlyUsed_new[0] = lastLensName;

        if (!recentlyUsed.Contains(lastLensName))
        {
            for (int i = 0; i < recentlyUsed.Length - 1; i++)
            {
                recentlyUsed_new[i + 1] = recentlyUsed[i];
            }
        }
        else
        {
            //determine the position of last lenst name in the recently used lenses previously
            int pos = 0;
            for(int k = 0; k < recentlyUsed.Length; k++)
            {
                if(recentlyUsed[k] == lastLensName)
                {
                    pos = k;
                }
            }

            switch(pos)
            {
                case 0:
                    recentlyUsed_new[1] = recentlyUsed[1];
                    recentlyUsed_new[2] = recentlyUsed[2];
                    recentlyUsed_new[3] = recentlyUsed[3];
                    break;

                case 1:
                    recentlyUsed_new[1] = recentlyUsed[0];
                    recentlyUsed_new[2] = recentlyUsed[2];
                    recentlyUsed_new[3] = recentlyUsed[3];
                    break;

                case 2:
                    recentlyUsed_new[1] = recentlyUsed[0];
                    recentlyUsed_new[2] = recentlyUsed[1];
                    recentlyUsed_new[3] = recentlyUsed[3];
                    break;

                case 3:
                    recentlyUsed_new[1] = recentlyUsed[0];
                    recentlyUsed_new[2] = recentlyUsed[1];
                    recentlyUsed_new[3] = recentlyUsed[2];
                    break;
            }
        }
        

        data.recentlyUsedLenses = string.Join(",", recentlyUsed_new);

        List<string> mostPopular_new = new List<string>();
        foreach(GameObject lensTile in lensTiles)
        {
            mostPopular_new.Add(lensTile.name);
        }
       
        for(int a = 0; a < mostPopular_new.Count; a++)
        {
            if(recentlyUsed_new.Contains(mostPopular_new[a]))
            {
                mostPopular_new.Remove(mostPopular_new[a]);
            }
        }

        data.mostPopularLenses = string.Join(",", mostPopular_new.ToArray());

        Debug.Log(data.mostPopularLenses);

        string jsonContent = JsonConvert.SerializeObject(data);
        Debug.Log(jsonContent);

        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/lensInfo.txt");
        writer.Write(jsonContent);
        writer.Close();

        //loadContents();
    }

    //load recently used lenses and other types of lenses as a json object
    private void loadInfo()
    {
        if (File.Exists(lensInfoFile))
        {
            Debug.Log("lensInfoFileExists");

            StreamReader reader = new StreamReader(lensInfoFile);
            string jsonContent = reader.ReadToEnd();
            reader.Close();

            data = JsonConvert.DeserializeObject<lensUIInfo>(jsonContent);

            recentlyUsed = data.recentlyUsedLenses.Split(',');
            mostPopular = data.mostPopularLenses.Split(',');
            //comingSoon = data.comingSoonLenses.Split(',');
        }
        else
        {
            Debug.LogError("LensInfo.txt file does not exist : dev error");
        }
    }

}
