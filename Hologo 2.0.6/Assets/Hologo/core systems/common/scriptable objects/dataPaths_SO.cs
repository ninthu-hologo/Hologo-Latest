using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
/// <summary>
///  container for all kinds of data paths names
/// </summary>
    [CreateAssetMenu(fileName = "dataPaths.asset", menuName = "Hologo V2/new data paths")]
    public class dataPaths_SO : ScriptableObject
    {
        [SerializeField]
        private List<pathDetails> fileNames;
        [SerializeField]
        private List<pathDetails> folderNames;
        [SerializeField]
        private List<pathDetails> urls;
        
        public string getUrl(int id)
        {
            string url;
            if(urls[id].AutoDomain)
            {
                url = appEVars.DomainUrl + urls[id].pathOrName;
            }
            else
            {
                url = urls[id].pathOrName;
            }

            return url;
        }

        public string getFileName(int id)
        {
            return fileNames[id].pathOrName;
        }

        public string getFolder(int id)
        {
            return folderNames[id].pathOrName;
        }
    }

    [System.Serializable]
    public class pathDetails
    {
        public string title;
        public string pathOrName;
        public bool AutoDomain;
    }


}


