using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 28 july 2019
    /// Modified by: 
    /// student lesson ui element 
    /// </summary>

    public class studentLessonUIElement : MonoBehaviour, IPrefabLocalize
    {

        public Button downloadButton;
        // lesson name;
        public TextMeshProUGUI lessonName;
        // sub can be lenght or category or teacher name
        public TextMeshProUGUI lessonSub;

        public Image Icon;
        // for private shared lessons
        public Image IconTeacher;

        RecordedLessonDetail lesson;

        public void fillWithData(IdataObject data, params CallbackGameObject[] actions)
        {
            lesson = data as RecordedLessonDetail;
            lessonName.text = lesson.title;
            lessonSub.text = "nothing here yet!";

            downloadButton.onClick.RemoveAllListeners();
            downloadButton.onClick.AddListener(() => actions[0](this.gameObject));

            //if (teacher)
            //{
            //    Icon.gameObject.SetActive(false);
            //    IconTeacher.gameObject.SetActive(true);
            //}
            //else
            //{
            //    Icon.gameObject.SetActive(true);
            //    IconTeacher.gameObject.SetActive(false);
            //}
        }

        public IdataObject getData()
        {
            return lesson;
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            //shariz v2 1/9/2019
            localizeSetting.setTextConfig(lessonName, language.getAFont(17));
            localizeSetting.setTextConfig(lessonSub, language.getAFont(18));
        }
    }
}
