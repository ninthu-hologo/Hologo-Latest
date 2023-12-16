using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 29 july 2019
    /// Modified by: 
    /// play bar ui element
    /// </summary>
    public class playBarUIElement : MonoBehaviour, IPrefabLocalize
    {

        public Image playPauseIcon;
        public Sprite pauseSprite;
        public Sprite playSprite;
        public TextMeshProUGUI playTitle;
        public Image playImage;
        public Button play;
        public Button forward;

        public RectTransform playDotAnim;
        public RectTransform forwardDotAnim;
        public CanvasGroup playDotAnimCg;
        public CanvasGroup forwardDotAnimCg;
        public Image playDotAnimImage;
        public Image forwardDotAnimImage;




        public void SetCurrentNameAndPic(string title, Image icon)
        {

            playTitle.text = title;
          //  playImage.sprite = icon.sprite;
        }


        public void Button_ForwardPlay()
        {
            //if (CurrentLessonGameObject != null)
            //{
            //    PlayForwardDotAnimation();
            //    LessonGameObject lgo = CurrentLessonGameObject.GetComponent<LessonGameObject>();
            //    int nextIndex = lgo.PlayIndex + 1;
            //    if (nextIndex >= AudioLessonGameObjects.Count)
            //    {
            //        nextIndex = 0;
            //    }

            //    LessonGameObject lgo2 = AudioLessonGameObjects[nextIndex].GetComponent<LessonGameObject>();
            //    IdataObject io = lgo2.myClipDetail;

            //    rLessonMain.PlayAudio(io, AudioLessonGameObjects[nextIndex]);
            //}
        }

        public void PlayPauseUIChange(bool isPlaying)
        {
            if (isPlaying)
            {
                playPauseIcon.sprite = pauseSprite;
            }
            else
            {
                playPauseIcon.sprite = playSprite;
            }
        }

        public void PlayDotAnimation()
        {
            playDotAnimCg.alpha = 1f;
            playDotAnim.sizeDelta = new Vector2(1, 1);
            playDotAnimImage.color = new Color(0.03f, 0.03f, 0.03f, 0.23f);
            LeanTween.scale(playDotAnim, new Vector2(40, 40), 0.2f);
            LeanTween.alpha(playDotAnim, 0f, 0.2f);
        }

        public void PlayForwardDotAnimation()
        {
            forwardDotAnimCg.alpha = 1f;
            forwardDotAnim.sizeDelta = new Vector2(1, 1);
            forwardDotAnimImage.color = new Color(0.03f, 0.03f, 0.03f, 0.23f);
            LeanTween.scale(forwardDotAnim, new Vector2(40, 40), 0.2f);
            LeanTween.alpha(forwardDotAnim, 0f, 0.2f);
        }

        public void localizePrefab(languageData_SO language, localizePrefab_SO localizeSetting)
        {
            throw new System.NotImplementedException();
        }
    }
}
