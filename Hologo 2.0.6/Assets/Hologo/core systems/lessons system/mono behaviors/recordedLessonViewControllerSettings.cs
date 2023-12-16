using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 31 july 2019
    /// Modified by: 
    /// this controller controls lessons view in settings
    /// </summary>
    public class recordedLessonViewControllerSettings : MonoBehaviour
    {

        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private userData_SO userData;
        [Header("Scripts")]
        [SerializeField]
        private recordedLessonsManager rlManager;
        [SerializeField]
        private settingsUIConnect sConnect;

        [Header("Scene Objects")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;
        [Header("UI ELEMENTS")]
        [SerializeField]
        private settingsSwitcher player; // Shariz 2nd Nov 2019 2.00

        AudioClip currentAudioClip;
        audioClipDetail currentAudioClipDetail;
        audioClipDetail currentPlayingAudioClipDetail;

        // callback for category buttons
        private CallbackGameObject[] recordedLessonsActions = new CallbackGameObject[4];
        // for updating the ui smart button element after edit
        private recordedLessonUIElement lessonElement;

        // current playing lesson;
        private recordedLessonUIElement currentPlayingLesson;

        private bool updateTimer;
        private bool enablePlayLesson;
        private float currentClipLength;

        // initializing lessons view controller for both teacher and students depending on the user type
        public void initiateRecordedViewController()
        {
            if (userData.requestUserDetail().user_type_id == 0)
                return;
            // save it
            // play it
            // delete it
            recordedLessonsActions[0] = playClip;
            recordedLessonsActions[1] = editClip;
            recordedLessonsActions[2] = shareClip;
            recordedLessonsActions[3] = deleteClip;

            // get lessons
            rlManager.loadRecordedLessonList();
            // check for if any lessons exist
            if (rlManager.getRecordedLessonList().Count <= 0)
            {
                player.getPlayerElement().gameObject.SetActive(false);// Shariz 2nd Nov 2019 2.00
                sConnect.setStateNoLessonsIndicator(true);
                recordedLessonsGetFromServer();
            }
            else
            {
                // make lessons ui elements
                sConnect.setStateNoLessonsIndicator(false);
                recordedLessonsGetFromServer();
               // sConnect.makeLessonSmartButtons(true,rlManager.getRecordedLessonList(), recordedLessonsActions);
                // setting player to true and assiging its play /forward button functions
                player.getPlayerElement().gameObject.SetActive(true);// Shariz 2nd Nov 2019 2.00
                player.getPlayerElement().play.onClick.RemoveAllListeners();// Shariz 2nd Nov 2019 2.00
                player.getPlayerElement().forward.onClick.RemoveAllListeners();// Shariz 2nd Nov 2019 2.00

                player.getPlayerElement().play.onClick.AddListener(playCurrentClip);// Shariz 2nd Nov 2019 2.00
                player.getPlayerElement().forward.onClick.AddListener(playNextClip);// Shariz 2nd Nov 2019 2.00
            }


        }

        #region UNITY API

        private void Update()
        {
            // timer update for lesson recording
            if (updateTimer)
            {
                currentPlayingLesson.updateTime(audioSource.time);
                if (!audioSource.isPlaying)
                {
                    if (audioSource.time >= currentClipLength)
                    {
                        currentPlayingLesson.resetTime();
                        updateTimer = false;
                    }
                }
            }
        }

        #endregion


        #region AUDIOCLIP FUNCTIONS


        public void playNextClip()
        {
            Debug.Log("pressed forward");

            if (currentPlayingLesson == null)
                return;

            currentPlayingLesson.resetTime();

            int id = currentPlayingLesson.getListId();
            GameObject go = sConnect.getNextLesson(id);
            currentPlayingLesson = sConnect.getNextLesson(id).GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = currentPlayingLesson.getData() as audioClipDetail;
            player.getPlayerElement().SetCurrentNameAndPic(acd.Title, null);// Shariz 2nd Nov 2019 2.00
            enablePlayLesson = true;
            currentAudioClipDetail = acd;
            loadAndPlayAudioClip(acd.FileName, true);
        }

        async void loadAndPlayAudioClip(string fileName, bool play)
        {
            await loadAudioClip(fileName);

            if (play)
                playCurrentClip();
        }

        async Task loadAudioClip(string fileName)
        {

            // loading the clip from storage
            currentAudioClip = await rlManager.loadRecordedLesson(fileName);
            // assigning the clip to audio source
            audioSource.clip = currentAudioClip;
            currentClipLength = currentAudioClip.length;
        }

        // plays the current audio clip
        // assigned to the side play/pause button
        public void playCurrentClip()
        {
            Debug.Log("pressed play pause");
            if (currentAudioClip == null)
                return;

            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                player.getPlayerElement().PlayPauseUIChange(false);// Shariz 2nd Nov 2019 2.00
                updateTimer = false;
            }
            else
            {

                if (audioSource.time != 0)
                {
                    audioSource.Play();
                    player.getPlayerElement().PlayPauseUIChange(true);// Shariz 2nd Nov 2019 2.00
                    updateTimer = true;
                }
            }

            playCurrentClipInternal();
        }

        public void stopAudioClip()
        {
            audioSource.Stop();
            enablePlayLesson = true;
            updateTimer = false;
        }

        void playCurrentClipInternal()
        {
            if (!enablePlayLesson)
                return;

            audioSource.Play();
            player.getPlayerElement().PlayPauseUIChange(true);// Shariz 2nd Nov 2019 2.00
            updateTimer = true;
            enablePlayLesson = false;
        }

        #endregion
        #region RECORDED LESSONS SMART BUTTON FUNCTIONS

        void playClip(GameObject go)
        {
            //Play clip
            if (currentPlayingLesson != null)
                currentPlayingLesson.resetTime();

            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;

            if(string.IsNullOrEmpty(acd.FileName))
            {
                if(acd.IsUploaded)
                {
                    // download lesson and play
                    downloadAndPlay(go, acd);
                }
            }
            else
            {
                player.getPlayerElement().SetCurrentNameAndPic(acd.Title, null);// Shariz 2nd Nov 2019 2.00
                currentPlayingLesson = rluie;
                enablePlayLesson = true;
                loadAndPlayAudioClip(acd.FileName, true);
                currentPlayingAudioClipDetail = acd;
            }
        }

        async void downloadAndPlay(GameObject go, audioClipDetail acd)
        {
            Debug.Log("downloading lesson");
            bool success = await rlManager.downloadUploadedLesson(acd);
            if(success)
            {
                recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
                player.getPlayerElement().SetCurrentNameAndPic(acd.Title, null);// Shariz 2nd Nov 2019 2.00
                currentPlayingLesson = rluie;
                enablePlayLesson = true;
                loadAndPlayAudioClip(acd.FileName, true);
            }
            else
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(63);
                windowMain.InfomationToast.ShowToast(message, 3f);
            }
        }

        void editClip(GameObject go)
        {
            //edit clip
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            lessonElement = rluie;
            updateClipDetailsModalWindow(acd);
        }
        void shareClip(GameObject go)
        {
            //share clip
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            //if(acd.IsUploaded)
            //{
            //    windowMain.ModalWindowD.ShowInfo(true, false, "Alert", "This lesson has been already uploaded", "OK");
            //    return;
            //}
            string private_share = localizationProvider.provide.getLanguage().getAMessageText(32);
            string public_share = localizationProvider.provide.getLanguage().getAMessageText(33);
            windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, private_share, public_share, "",
                                                    cancel, null, () => Share(false, acd), () => Share(true, acd), null);
        }

        void cancel()
        {

        }

        void Share(bool pubic, audioClipDetail acd)
        {

            ShareLesson(pubic, acd);
        }

        async void ShareLesson(bool pubic, audioClipDetail acd)
        {
            bool success = await rlManager.uploadRecordedLesson(acd, pubic);
            if (success)
            {
                Debug.Log("uploaded");
                string message = localizationProvider.provide.getLanguage().getAMessageText(34);
                windowMain.InfomationToast.ShowToast(message, 2f);
            }
            else
            {
                Debug.Log("failed");
                string message = localizationProvider.provide.getLanguage().getAMessageText(34);
                windowMain.InfomationToast.ShowToast(message, 3f);
            }
        }

        void deleteClip(GameObject go)
        {
            //delete clip
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            string delete_from_storage = localizationProvider.provide.getLanguage().getAMessageText(36);
            string delete_from_server = localizationProvider.provide.getLanguage().getAMessageText(37);
            string delete_from_both = localizationProvider.provide.getLanguage().getAButtonText(35);
            if(acd.lessonDetailFromServer.id >0)
            {
                // can send option to delete from server as well
                windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, delete_from_storage, delete_from_server, delete_from_both,
                                                   cancel, null, () => deleteBothLocalAndServer(go), null, null);
            }
            else
            {
                windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, delete_from_storage, delete_from_server, delete_from_both,
                                                  cancel, null, () => deleteOnlyLocal(go), null, null);
            }
          
        }

        void stopAndUnloadCurrentClipBeingDeleted(audioClipDetail acd)
        {
            if(currentPlayingAudioClipDetail!=null){ //SHARIZ 28th Oct 2019 2.00
                if (currentPlayingAudioClipDetail.LessonId == acd.LessonId)
                {
                    //Debug.Log(currentPlayingAudioClipDetail.LessonId + "and" + acd.LessonId);
                    stopAudioClip();
                    player.getPlayerElement().SetCurrentNameAndPic("Not Playing", null);// Shariz 2nd Nov 2019 2.00
                    player.getPlayerElement().PlayPauseUIChange(false);// Shariz 2nd Nov 2019 2.00
                    // experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                }
            }
        }

        void deleteOnlyLocal(GameObject go)
        {
            Debug.Log("delete from local");
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            stopAndUnloadCurrentClipBeingDeleted(acd);
            rlManager.removeAndDeleteLessonFromList(acd);
            Destroy(go);
        }

        void deleteOnlyServer(GameObject go)
        {
            Debug.Log("delete from server");
            deleteLessonFromServer(go);
        }

        async void deleteLessonFromServer(GameObject go)
        {
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            bool success = await rlManager.deleteLessonFromServer(acd);
            if(success)
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(39);
                windowMain.InfomationToast.ShowToast(message, 2f);
            }else
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(40);
                windowMain.InfomationToast.ShowToast(message, 3f);
            }
        }

        void deleteBothLocalAndServer(GameObject go)
        {
            Debug.Log("delete from local and server");
            deleteLessonFromServerAndStorage(go);
        }

        async void deleteLessonFromServerAndStorage(GameObject go)
        {
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            stopAndUnloadCurrentClipBeingDeleted(acd);
            bool success = await rlManager.deleteLessonFromServer(acd);
            if (success)
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(65);
                windowMain.InfomationToast.ShowToast(message, 2f);
            }
            // else
            // {   
            //     string message = localizationProvider.provide.getLanguage().getAMessageText(40);
            //     windowMain.InfomationToast.ShowToast(message, 3f);
            // }
            rlManager.removeAndDeleteLessonFromList(acd);
            rlManager.removeLessonFromServerList(acd);
            Destroy(go);
        }


        // display modal window for audio clip details after recording a clip
        private void updateClipDetailsModalWindow(audioClipDetail clipDetail)
        {
            currentAudioClipDetail = clipDetail;
            windowMain.ModalWindowF.getiOS_ModalWindowF().FillInput(true, currentAudioClipDetail.Title, currentAudioClipDetail.Description, updateClipDetails);
        }
        // updating clip details
        void updateClipDetails(string title, string des)
        {
            // checking for empty title and description if so we just use default ones
            if (string.IsNullOrEmpty(title))
                title = currentAudioClipDetail.Title;
            if (string.IsNullOrEmpty(des))
                des = currentAudioClipDetail.Description;

            currentAudioClipDetail.Title = title;
            currentAudioClipDetail.Description = des;
            rlManager.updateRecordedLesson(currentAudioClipDetail);
            // updating the ui element of the audio clip detail
            lessonElement.updateEdit();
        }

        #endregion
        #region RECORDED LESSONS FROM SERVER METHODS

        public void recordedLessonsGetFromServer()
        {
            getLessonsFromServer();
        }

        async void getLessonsFromServer()
        {
            bool success = await rlManager.getRecordedLessonListFromServer();
            if(success)
            {
                // make the lessons buttons
                if (rlManager.getRecordedLessonList().Count > 0)
                {
                    sConnect.setStateNoLessonsIndicator(false);
                    player.getPlayerElement().gameObject.SetActive(true);// Shariz 2nd Nov 2019 2.00
                    sConnect.makeLessonSmartButtons(true, rlManager.getRecordedLessonList(), recordedLessonsActions);
                    
                }
            }
            else
            {
                // nothing
            }
        }


        #endregion
    }
}
