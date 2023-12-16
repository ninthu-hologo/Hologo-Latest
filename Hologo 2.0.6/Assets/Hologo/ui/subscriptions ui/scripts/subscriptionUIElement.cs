using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace Hologo2
{
    public class subscriptionUIElement : MonoBehaviour,IPrefabLocalize
    {

        public TextMeshProUGUI Name;
        public TextMeshProUGUI Price;
        public TextMeshProUGUI Description;
        public Button BuyButton;
        public GameObject BoughtIcon;
        public GameObject OrangeBG;
        public GameObject NormalIcon;
        public GameObject NormalBG;
        Color orangeCol = new Color(1f, 0.44f, 0f, 1f);
        Color White = new Color(1f, 1f, 1f, 1f);

        private SubscriptionDataClass subscriptionDataObject;



        public void fillWithData(IdataObject data, int userType, params CallbackGameObject[] actions)
        {
            subscriptionDataObject = data as SubscriptionDataClass;

            Name.text = subscriptionDataObject.name;
            if (subscriptionDataObject.isBought)
            {
                BuyButton.gameObject.SetActive(false);
                SetColors();
                NormalBG.SetActive(false);
                BoughtIcon.SetActive(true);
                NormalIcon.SetActive(false);

               // if (subDetail.id != 0)
               // {
                   // SetupSubscriptionDetails(subDetail);
              //  }
              //  else
               // {
                    if (userType == 2)
                    {
                        DateTime dateOne = DateTime.Parse(subscriptionDataObject.expire_date);
                        string date = dateOne.ToString("dd MMM yyyy").ToUpper();
                        Price.text = "Until : " + date;
                        SetupDescription(subscriptionDataObject.description);

                    }
                    else
                    {
                        DateTime dateOne = DateTime.Parse(subscriptionDataObject.expire_date);
                        string date = dateOne.ToString("dd MMM yyyy").ToUpper();
                        Price.text = "Untill : " + date;
                        string description = localizationProvider.provide.getLanguage().getAMessageText(7);// Shariz 22nd Feb 2020 2.0.4
                        // string p = "Having issues getting your subscription details at the moment!";
                        Description.text = description;// Shariz 22nd Feb 2020 2.0.4
                    }

                //}
            }
            else
            {
                // can buy
                BuyButton.onClick.RemoveAllListeners();
                BuyButton.onClick.AddListener(() => actions[0](this.gameObject));
                SetupDescription(subscriptionDataObject.description);
                double b = System.Math.Round(subscriptionDataObject.price, 2);
                Price.text = "USD" + b + " /Anually";//shariz 31st oct 2019 2.00


            }

        }

        public IdataObject getData()
        {
            return subscriptionDataObject;
        }



        void SetupSubscriptionDetails(SubscriptionDetails mysubscription)
        {


            //DateTime dateOne = DateTime.Parse(mysubscription.expire_date);
            //string date = dateOne.ToString("dd MMM yyyy").ToUpper();
            //Price.text = "Untill : " + date;

            //string incLessons = mysubscription.subscriptions.used_lessons + "/" + mysubscription.subscriptions.total_lessons;
            //string inccoup = mysubscription.subscriptions.used_coupons + "/" + mysubscription.subscriptions.total_coupons;

            //string detailText = "No of Lessons used/total : " + incLessons + ".\n" + "No of coupons used/total : " + inccoup + ".\n";

            //Description.text = detailText;
        }

        void SetupDescription(string description)
        {
            string[] keywords = description.Split(':');
            // string newdescription = "<sprite index="4">Enjoy all pro experiences."
            string newDescription = "";
            for (int i = 0; i < keywords.Length; i++)
            {
                newDescription = newDescription + "<sprite index='4'>" + keywords[i] + ".\n";
            }
            Description.text = newDescription;
        }


        void SetColors()
        {
            Name.color = White;
            Description.color = White;
            Price.color = White;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            throw new NotImplementedException();
        }
    }

}
