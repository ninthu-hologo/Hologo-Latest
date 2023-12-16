using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2.library;
using TMPro;
using Hologo.iOSUI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 july 2019
    /// Modified by: 
    /// recorded lesson ui element 
    /// </summary>
    public class recordedLessonUIElement : MonoBehaviour, IPrefabLocalize
    {
        public Button edit;
        public Button share;
        public Button delete;

        public TextMeshProUGUI title;
        public TextMeshProUGUI lenght;

        private audioClipDetail audioData;

        private int listId;

        CallbackGameObject playAction;

        public iOS_SmartButtonOptions smartButton;

        public void fillWithData(IdataObject data, int id, params CallbackGameObject[] actions)
        {
            this.audioData = data as audioClipDetail;
            listId = id;
            title.text = audioData.Title;
            lenght.text = audioData.Duration.ToString();
            edit.onClick.RemoveAllListeners();
            share.onClick.RemoveAllListeners();
            delete.onClick.RemoveAllListeners();
            playAction = actions[0];
            edit.onClick.AddListener(() => actions[1](this.gameObject));
            edit.onClick.AddListener(smartButton.MoveToZeroPosition);
            share.onClick.AddListener(() => actions[2](this.gameObject));
            share.onClick.AddListener(smartButton.MoveToZeroPosition);
            delete.onClick.AddListener(() => actions[3](this.gameObject));
            delete.onClick.AddListener(smartButton.MoveToZeroPosition);

        }

        public int getListId()
        {
            return listId;
        }

        public void updateTime(float time)
        {
            lenght.text = time.ToString("00.00");
        }

        public void resetTime()
        {
            lenght.text = audioData.Duration.ToString();
        }

        public void updateEdit()
        {
            title.text = audioData.Title;
        }

        public void playLesson()
        {
            playAction(this.gameObject);
        }

        public IdataObject getData()
        {
            return audioData;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            //shariz v2 1/9/2019
            // localizeSetting.setTextConfig(title, language.getAFont(17)); //Shariz - 22 Jan 2020 v2.0.4 
            // localizeSetting.setTextConfig(lenght, language.getAFont(18)); //Shariz - 22 Jan 2020 v2.0.4 
        }
    }
}
