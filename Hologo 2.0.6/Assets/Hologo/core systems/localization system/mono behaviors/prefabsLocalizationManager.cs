using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 june 2019
    /// Modified by: 
    /// prefab localization .this script helps localize all the prefabs once when app starts
    /// and after language is loaded
    public class prefabsLocalizationManager : messageLogging
    {
        [Header("DATA")]
        [SerializeField]
        private languageData_SO currentLanguage;
        [Header("LOCALIZATION PREFABS UI")]
        [SerializeField]
        private List<localizePrefab_SO> localizationPrefabs;
        [SerializeField]
        private List<GameObject> localizationSceneCanvases;

        [SerializeField]
        private localizePrefab_SO commonPrefabLocalizer;

        [SerializeField]
        private bool autoLocalizeCanvases = false;


        public IEnumerator localizeUIPrefabs()
        {
            for (int i = 0; i < localizationPrefabs.Count; i++)
            {
                localizationPrefabs[i].setLanguage(currentLanguage);
                localizationPrefabs[i].localizePrefab();
                yield return null;
            }
        }

        private void Start()
        {
            commonPrefabLocalizer.setLanguage(currentLanguage);
            if (autoLocalizeCanvases)
            {
                LocalizeSceneCanvases();
            }
        }

        public void LocalizeSceneCanvases()
        {
            for (int i = 0; i < localizationSceneCanvases.Count; i++)
            {
                IPrefabLocalize ipl = localizationSceneCanvases[i].GetComponent<IPrefabLocalize>();
                ipl.localizePrefab(currentLanguage, commonPrefabLocalizer);
            }
        }

    }
}
