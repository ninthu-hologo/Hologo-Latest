using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 19 july 2019
    /// Modified by: 
    /// this script controlls the ar slides and its functions
    /// </summary>
    public class experienceUIConnect : MonoBehaviour
    {

        [Header("UI PREFABS")]
        [SerializeField]
        private localizePrefab_SO slideStepButton;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private arSwitcher exploreStepsParent;
        [SerializeField]
        private arSwitcher learnStepsParent;

        [SerializeField]
        private List<GameObject> exploreStepButtons;
        [SerializeField]
        private List<GameObject> learnStepButtons;

        // keeps track of which mode we are in
        private ExperienceSlideMode mode;


        // make all learn and explore buttons
        // change between learn and explore mode
        // change slide steps
        // in learn mode auto advance to next step

        public void makeLearnAndExploreStepButtons(List<slideDetails> Models ,List<experienceLabelNarrationDataClass> localize, CallbackForId callBackId)
        {
            for (int i = 0; i < Models.Count; i++)
            {
                if(Models[i].isInExploreMode)
                {
                    exploreStepButtons.Add( makeStepButton(Models[i], localize[i], callBackId, i, exploreStepsParent.GetexploreStepsParent()));
                }
                if (Models[i].isInLearnMode)
                {
                    learnStepButtons.Add(makeStepButton(Models[i], localize[i], callBackId, i, learnStepsParent.GetlearnStepsParent()));
                }
            }
        }

        public void activateCurrentSlideButton(int id, bool isExplore)
        {
            if(isExplore)
            {
                for (int i = 0; i < exploreStepButtons.Count; i++)
                {
                    slide_step_button_ui_element ssb = exploreStepButtons[i].GetComponent<slide_step_button_ui_element>();
                    ssb.amIActive(id);
                }
                return;
            }
            for (int i = 0; i < exploreStepButtons.Count; i++)
            {
                
                slide_step_button_ui_element ssb = learnStepButtons[i].GetComponent<slide_step_button_ui_element>();
                ssb.amIActive(id);
            }

        }


        private GameObject makeStepButton(slideDetails model, experienceLabelNarrationDataClass localize, CallbackForId callBackId, int i , Transform parent)
        {
            GameObject go = Instantiate(slideStepButton.givePrefab());
            slide_step_button_ui_element ssb = go.GetComponent<slide_step_button_ui_element>();
            ssb.fillUpButton(i, localize.localizedName, callBackId);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.SetActive(false);
            return go;
        }


        public void changeSlideMode(ExperienceSlideMode mode)
        {
            this.mode = mode;
            switch (mode)
            {
                case ExperienceSlideMode.EXPLORE:
                    hideOrShowGameObjects(false, learnStepButtons);
                    hideOrShowGameObjects(true, exploreStepButtons);
                    break;
                case ExperienceSlideMode.LEARN:
                    hideOrShowGameObjects(false, exploreStepButtons);
                    hideOrShowGameObjects(true, learnStepButtons);
                    break;
                default:
                    break;
            }
           
        }


        void hideOrShowGameObjects(bool state, List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetActive(state);
            }
        }


        public void makeUIForPortal()
		{
            exploreStepsParent.GetExperienceDisplayUIElementDevice().makeUIForPortal();

        }

    }
}