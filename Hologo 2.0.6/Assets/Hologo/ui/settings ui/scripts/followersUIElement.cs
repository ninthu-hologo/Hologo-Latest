using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 09 August 2019
    /// Modified by: 
    /// experience ui element 
    /// </summary>
    public class followersUIElement : MonoBehaviour,IPrefabLocalize
    {

        [SerializeField]
        private TextMeshProUGUI nameText;
        [SerializeField]
        private Button clicker;
        [SerializeField]
        private GameObject notAcceptedIcon;
        [SerializeField]
        private GameObject AcceptedIcon;

        followerDataClass follower;
        private bool thisIsPending;


        public void fillData(bool isPending, followerDataClass follower, CallbackGameObject action)
        {
            nameText.text = follower.requester.email;
            this.follower = follower;
            thisIsPending = isPending;
            if (!isPending)
            {
                notAcceptedIcon.SetActive(true);
                AcceptedIcon.SetActive(false);
            }
            else
            {
                notAcceptedIcon.SetActive(false);
                AcceptedIcon.SetActive(true);
            }

            clicker.onClick.RemoveAllListeners();
            clicker.onClick.AddListener(() => action(this.gameObject));
        }

        public void acceptedStateOn()
        {
            notAcceptedIcon.SetActive(false);
            AcceptedIcon.SetActive(true);
        }

        public followerDataClass getFollower()
        {
            return follower;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            
        }
    }
}
