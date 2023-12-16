using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2.library;
using TMPro;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 13 july 2019
    /// Modified by: 
    /// experience ui element 
    /// </summary>
    public class experienceUIElement : MonoBehaviour, IPrefabLocalize //IUIElement
    {

        // shariz v2 1/9/2019
        [SerializeField]
        private experienceType typeofExperience;

        public enum experienceType
        {

            home,
            featured,
            list

        }


        [Header("UI ELEMENTS")]
        [SerializeField]
        private GameObject viewGroup;
        [SerializeField]
        private Button viewButton;
        [SerializeField]
        private Button threeDots;
        [SerializeField]
        private Image thumbImage;
        [SerializeField]
        private Sprite DefaultSprite;
        [SerializeField]
        private TextMeshProUGUI TopTitleText;
        [SerializeField]
        private TextMeshProUGUI TitleText;
        // shariz v2 1/9/2019
        [SerializeField]
        private TextMeshProUGUI BodyText;
        // shariz v2 1/9/2019
        [SerializeField]
        private bool ExpFree;
        [SerializeField]
        private string FreeString;        
        [SerializeField]
        private string PremiumString;        
        [SerializeField]
        private string ComingSoonString; // Shariz 22nd Feb 2020 2.0.4
        [Header("DATA")]
        [SerializeField]
        private dataPaths_SO dataPath;


        private experienceDataClass expData;
//        float aspectRatio = 1.7f;
//        int TitleCharacterLenght = 20;
        string suffix = "...";

        private downloadImageQueue downloadQueuer;

        //WARNING: IMAGES THAT DOES NOT MEET THIS SIZE WILL THROW AN ERROR WHEN THE PRODUCT IS GENERATED IN STORE!
        private int width = appEVars.MediumResImage; // change to 716 //688 priv = 716,422
        private int height = appEVars.MediumResImage;

        private CallbackGameObject[] actionList;

        public void fillWithDataSync(IdataObject data, downloadImageQueue dq,params CallbackGameObject[] actions)
        {
            downloadQueuer = dq;
            expData = data as experienceDataClass;
            actionList = actions;
            viewButton.onClick.RemoveAllListeners();
            viewButton.onClick.AddListener(() => actions[1](this.gameObject));
            threeDots.onClick.RemoveAllListeners();
            threeDots.onClick.AddListener(() => actions[0](this.gameObject));
            // filling up ui;
            // TopTitleText.text = "explore";
            TitleText.text = expData.title;
            ExpFree = expData.experience.free;

            //shariz v2 1/9/2019
            switch (typeofExperience)
            {
                case experienceType.home:
                    //shariz v2 11/9/2019 added a limiter to description
                const int MaxLength = 90; //Shariz 15th September 2019 v2

                // Shariz 5th March 2020 v2.0.4
                if (expData.body.Length > MaxLength) {
                    BodyText.text =  expData.body.Substring(0, MaxLength) + suffix;
                } else {
                    BodyText.text =  expData.body;
                }
                    
                    break;
                case experienceType.list:
                    break;
            }
            if(expData.experience.asset_bundles.Count>0)
            {
            if(ExpFree){
                TopTitleText.text = FreeString;
            }
            else {
                TopTitleText.text = PremiumString;
            }
            }
            else
            {
                TopTitleText.text = ComingSoonString;// Shariz 22nd Feb 2020 2.0.4
            }
          

            loadImageNonAsync();
        }

        public void setDownloadQueuer(downloadImageQueue dq)
        {
            downloadQueuer = dq;
        }


        //public async Task fillWithData(IdataObject data, params CallbackGameObject[] actions)
        //{

        //    //loadImageNonAsync();
        //   // await LoadImage();

        //    throw new System.NotImplementedException();
        //}

        public Sprite giveImageSprite()
        {
            return thumbImage.sprite;
        }


        async void loadImageNonAsync()
        {
            await LoadImage();
        }

        public IdataObject getData()
        {
            return expData;
        }

        /// <summary>
        /// here we check if thumb exists if it does then load it.
        /// if not that download the thumb from server and load it.
        /// </summary>
        /// <returns></returns>
        async Task LoadImage()
        {
//            Debug.Log("loading image");

            bool success = false;
            if (success = await downloadImage())
            {
                success = await loadImage();
            }

            if (!success) // Hamid 1st nov 2019 remove broken image file
            {
                loadDefaultSprite();
                // also delete if an image exits
                if (readWriteData.CheckIfFileExists(dataPath.getFolder(0), expData.experience.image.file_name))
                {
                    readWriteData.DeleteFileOnDisk(dataPath.getFolder(0), expData.experience.image.file_name);
                }
                    
            }
        }

        public void loadImageAfterDownload()
        {
            asyncloadImage();
        }

        private async void asyncloadImage()
        {
           bool success = await loadImage();

            if (!success)
                loadDefaultSprite();
        }

        private async Task<bool> loadImage()
        {
            bool imageOnStorage = false;
            Texture2D displayImage = await readWriteData.ReadImage(width, height, dataPath.getFolder(0), expData.experience.image.file_name);
            try
            {
                Sprite sprite = Sprite.Create(displayImage, new Rect(0, 0, width, height), new Vector2(0.5f, 0.0f), 100.0f);
                thumbImage.sprite = sprite;
                imageOnStorage = true;
            }
            catch (System.Exception e)
            {
                // not throwing here.
                //Debug.Log("problem");
            }
            return imageOnStorage;
        }

        private async Task<bool> downloadImage()
        {
            if (readWriteData.CheckIfFileExists(dataPath.getFolder(0), expData.experience.image.file_name))
                return true;

            downloadQueuer.addToDownloadList(this);
            return false;
           // Debug.Log("no file");
           // string url = dataPath.getUrl(1) + expData.experience_id + dataPath.getUrl(2);
           // Debug.Log(expData.title+"---"+ url);
           // return await downloadHelper.downloadToBufferAndSave(url, dataPath.getFolder(0), expData.experience.image.file_name);
        }

        private void loadDefaultSprite()
        {
            if (thumbImage != null && DefaultSprite != null)
                thumbImage.sprite = DefaultSprite;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            RectTransform tdr = threeDots.GetComponent<RectTransform>();
            if (language.rightToLeft)
            {
                uiHelper.SetAnchor(tdr, AnchorPresets.TopLeft);
                uiHelper.SetPivot(tdr, PivotPresets.TopLeft);
            }
            else
            {
                uiHelper.SetAnchor(tdr, AnchorPresets.TopRight);
                uiHelper.SetPivot(tdr, PivotPresets.TopRight);
            }



            //shariz v2 1/9/2019
            switch (typeofExperience)
            {
                case experienceType.home:
                    localizeSetting.setTextConfig(TitleText, language.getAFont(1));
                    if (BodyText != null)
                    {
                        localizeSetting.setTextConfig(BodyText, language.getAFont(23));
                    }
                    localizeSetting.setTextConfig(TopTitleText, language.getAFont(2), language.getAButtonText(3));
                    break;

                case experienceType.featured:
                    localizeSetting.setTextConfig(TitleText, language.getAFont(1));
                    localizeSetting.setTextConfig(TopTitleText, language.getAFont(2), language.getAButtonText(3));
                    // ExpFree = expData.free;
                    break;
 
                case experienceType.list:
                    localizeSetting.setTextConfig(TitleText, language.getAFont(21));
                    FreeString =  language.getALabelText(59);
                    PremiumString =  language.getALabelText(60);
                    ComingSoonString =  language.getALabelText(90);// Shariz 22nd Feb 2020 2.0.4
                        localizeSetting.setTextConfig(TopTitleText, language.getAFont(22));
                    break;
            }
        }


    }
}
