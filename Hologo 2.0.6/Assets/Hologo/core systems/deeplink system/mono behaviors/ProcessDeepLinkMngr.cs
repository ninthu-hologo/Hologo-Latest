using System;
using UnityEngine;
using Hologo2.library;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Hologo2
{
    public class ProcessDeepLinkMngr : MonoBehaviour
    {
        public static ProcessDeepLinkMngr Instance { get; private set; }
        public string deeplinkURL;
        [SerializeField]
        private libraryData_SO libraryData;
        [SerializeField]
        public experienceDataClass expData;
        public event_SO deepLinkSetExperienceEvent;

        public bool readyForDLExp = false;
        public bool dlExpSet = false;
        //public TextMeshProUGUI TitleText;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Application.deepLinkActivated += onDeepLinkActivated;

                if (!String.IsNullOrEmpty(Application.absoluteURL))
                {
                    // cold start and Application.absoluteURL not null so process Deep Link
                    onDeepLinkActivated(Application.absoluteURL);
                    Debug.Log("AbsoluteURL: " + Application.absoluteURL);
                }
                // initialize DeepLink Manager global variable
                else deeplinkURL = "[None]";
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void testURL()
        {
            onDeepLinkActivated(deeplinkURL);
        }

        private void onDeepLinkActivated(string url)
        {
            // update DeepLink Manager global variable, so URL can be accessed from anywhere 
            deeplinkURL = url;
            // https://energyworld.mv/experience/182

            initiateDeepLinkActions();

        }

        public void initiateDeepLinkActions()
        {
            if (!string.IsNullOrEmpty(deeplinkURL) && Uri.IsWellFormedUriString(deeplinkURL, UriKind.Absolute))
            {
                Uri uri = new Uri(deeplinkURL);
                //string cleanUrl = uri.GetLeftPart(UriPartial.Path);
                string pathAfterDomain = uri.AbsolutePath;
                string urlExperienceId = pathAfterDomain.Split("/"[0])[3];
                Debug.Log(urlExperienceId);
                //TitleText.text = urlExperienceId;
                findExperienceFromLibrary(Convert.ToInt32(urlExperienceId));
            }
        }

        private void findExperienceFromLibrary(int exp_id)
        {
            
            if (!libraryData.isLibaryfilled())
                return;

            expData = libraryData.getLibraryData().Where(e => e.experience_id == exp_id).FirstOrDefault();

            if (expData.title == null)
                return;

            Debug.Log($"the title for {exp_id} is {expData.title}");

            deepLinkSetExperienceEvent.raise(this.gameObject);
            dlExpSet = true;
        }


        public void ReadyingForDLExp(bool ready)
        {
            if (dlExpSet)
                return;

            if (!ready)
                return;

            initiateDeepLinkActions();
        }
    }


}