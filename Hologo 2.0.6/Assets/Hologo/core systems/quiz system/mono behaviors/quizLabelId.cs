using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 07 August 2019
    /// Modified by: 
    /// this holds id of the origin and label of quiz.
    /// </summary>
    public class quizLabelId : MonoBehaviour
    {
        public int id;
        public bool isLabel;
        // keep track of matches made
        // prevent user from clicking this again
        public bool done;

        float duration = 0.3f; // duration in seconds

        private float t = 0; // lerp control variable


        Image bg;

        Color lerpedColor = Color.green;
        Color defColor;


        void Awake()
        {
            
            bg = GetComponentInChildren<Image>();
            if (bg == null)
                return;

            defColor = bg.color;
        }

        //void Update()
        //{
        //    if(done)
        //    lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        //}


        public void selected()
        {
            if (bg == null)
                return;

            StartCoroutine(changeColor());
        }

        public void Reset()
        {
            if (bg != null)
            {
                StopAllCoroutines();
                bg.color = defColor;
                t = 0;
            }
        }

        IEnumerator changeColor()
        {

            while (t < 1)
            {
                
                bg.color = Color.Lerp(defColor, lerpedColor, t);
                t += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}
