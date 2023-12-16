using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;
using System.Threading.Tasks;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 23 july 2019
    /// Modified by: 
    /// this controller controls lessons view in experience scene
    /// </summary>
    public class recordedLessonViewController : messageLogging
    {
        [Header("UI ELEMENTS")]
        [SerializeField]
        private arSwitcher experienceMainUIElement;
        [Header("Data")]
        [SerializeField]
        private settings_SO mainSettings;
        [SerializeField]
        private userData_SO userData;
        [Header("Scripts")]
        [SerializeField]
        private recordedLessonsManager rlManager;
        [SerializeField]
        private studentLessonsManager slManager;
        [SerializeField]
        private disableEnableExperienceInteraction expInteraction;
        [SerializeField]
        private lessonsUIConnect lessonsUI;
        private recordLesson lessonRecorder;
        [Header("Scene Objects")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private iOS_UIModalWindowMain windowMain;


        AudioClip currentAudioClip;
        audioClipDetail currentAudioClipDetail;
        audioClipDetail currentPlayingAudioClipDetail;

        // callback for category buttons
        private CallbackGameObject[] recordedLessonsActions = new CallbackGameObject[4];
        private CallbackGameObject[] studentLessonsActions = new CallbackGameObject[1];
        // for updating the ui smart button element after edit
        private recordedLessonUIElement lessonElement;

        private bool isLessonDownloading;


        bool updateTimer = false;
        bool updateSlider = false;
        bool enablePlayLesson = false;


        // initializing lessons view controller for both teacher and students depending on the user type
        public void initiateRecordedViewController()
        {
            if (userData.requestUserDetail().user_type_id == 3)
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().makeUIForAnonymous();
                return;
            }
                

            //check to see if the user is signed in or subscribed
            if (userData.isUserSignedIn() && userData.isUserSubscribed())
            {

                // if user is a teacher
                if (userData.requestUserDetail().user_type_id == 1)
                {
                    Debug.Log("Teacher functions");
                    // intiate recording of narration
                    // save it
                    // play it
                    // delete it
                    recordedLessonsActions[0] = playClip;
                    recordedLessonsActions[1] = editClip;
                    recordedLessonsActions[2] = shareClip;
                    recordedLessonsActions[3] = deleteClip;
                    loadLatestRecoredAudioClipForLoadedExperience();
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().makeUIForTeachers();
                    lessonRecorder = new recordLesson(appEVars.LessonRecordingLenght, appEVars.LessonRecordQuality);
                }

                // if user is a student
                if (userData.requestUserDetail().user_type_id == 2)
                {
                    Debug.Log("Student functions");
                    // iniate checking for custom teacher narrations
                    // download a narration
                    // play it
                    // initialy check if a lesson is downloaded for this experience if so load it to be played
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().makeUIForStudents();
                    studentLessonsActions[0] = downloadStudentLessonClip;
                    studentLessonAudioClipForLoadedExperience();
                }
            }
            else
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().makeUIForAnonymous();
            }


        }

        // button click method.
        // this method is for both teacher and students
        // students > will get lessons from server for the current experience
        // teachers > will load any lessons recorded for the current experience
        public void getLessons()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
           // expInteraction.setExpInteractionState(false);
            if (userData.requestUserDetail().user_type_id == 1)
            {
                Debug.Log("teacher lessons");
                showRecordedLessonsForCurrentExperience();
            }

            if (userData.requestUserDetail().user_type_id == 2)
            {
                Debug.Log("student lessons");
                getStudentLessonsFromServer();
            }
        }


        #region UNITY API
        private void Update()
        {
            
            // timer update for lesson recording
            if (updateTimer)
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().timerDisplayUpdate(lessonRecorder.updateTimer());
            }

            // audio slider update for the playing audio clip
            if (updateSlider)
            {
                if (audioSource.time < audioSource.clip.length - 0.1f)
                {
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderUpdate(audioSource.time);
                }
                else
                {
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderStop();
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseGraphicsUpdate(false);
                    updateSlider = false;
                    enablePlayLesson = true;
                }

            }

            // update download indicator for lesson download
            if (isLessonDownloading)
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().downloadSliderUpdate(downloadHelper.progress);


        }
        #endregion

        #region EVENTS
        // run by event listner attached to experience view controller
        public void getExploreLearnModeChangeEvent(GameObject go)
        {
            // setting the active state of student teacher buttons when mode changes
            experienceViewController evc = go.GetComponent<experienceViewController>();
            switch (evc.getCurrentMode())
            {
                case ExperienceSlideMode.EXPLORE:
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().setActiveStateStudentsteacherButtonsGroup(true, true);
                    break;
                case ExperienceSlideMode.LEARN:
                    stopAudioClip();
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().setActiveStateStudentsteacherButtonsGroup(false, true);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region TEACHER METHODS
        //load the last recorded audio clip for the loaded experience if it exits
        void loadLatestRecoredAudioClipForLoadedExperience()
        {
            // getting file name of the clip from lesson list
            audioClipDetail acd = rlManager.getLatestRecordedClip();

            if(acd == null)
            {
                Debug.Log("no audio clip");
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                return;
            }
            currentPlayingAudioClipDetail = acd;
            // checking for null value.
            if (string.IsNullOrEmpty(acd.FileName))
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                return;
            }
            Debug.Log("has audio clip");
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(true);
            loadAndPlayAudioClip(acd.FileName, false);
        }
        void showRecordedLessonsForCurrentExperience()
        {
            List<audioClipDetail> tempRecordedLessonList = rlManager.getfilteredRecordedListForCurrentExperience();

            if (tempRecordedLessonList == null)
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                string message = localizationProvider.provide.getLanguage().getAMessageText(64);
                windowMain.InfomationToast.ShowToast(message, 3f);
                return;
            }
            // turning off the experience for interaction
            expInteraction.setExpInteractionState(false);
            lessonsUI.enableLessonCanvas();
            lessonsUI.makeLessonSmartButtons(tempRecordedLessonList, recordedLessonsActions);
            expInteraction.setExpInteractionState(true); // Shariz hologo 2.0.2 15th dec 2019
        }
        // recording start from pressing record button
        public void startRecording()
        {
            // start recording using lessonRecorder class
            string fileName = rlManager.getFileNameForLessonRecord();
            Debug.Log("start voice recording");
            
            lessonRecorder.StartVoiceRecording(fileName, rlManager.getFolderNameForLessonRecord());
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);// Shariz 17th Oct 2019 2.00
            // setting update time status
            updateTimer = lessonRecorder.timerStatus();
            // display timer
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().timerDisplaySetState(updateTimer);
            // setting in record graphics
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().recordingGraphicsUpdate(updateTimer);
            // while in recording if user presses . the recording is saved
            // and now we get the recorded clip and set as current audio clip
            if (lessonRecorder.isAudioInRecording())
            {
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
                currentAudioClip = lessonRecorder.getRecordedClip();
                audioSource.clip = currentAudioClip;
                enablePlayLesson = true;
                newClipDetailsModalWindow(fileName, currentAudioClip.length);
            }
        }
        // display modal window for audio clip details after recording a clip
        private void newClipDetailsModalWindow(string fileName, float clipLenght)
        {
            currentAudioClipDetail = rlManager.createRecordedLessonDetail(fileName, clipLenght);
            windowMain.ModalWindowF.getiOS_ModalWindowF().FillInput(true, currentAudioClipDetail.Title, currentAudioClipDetail.Description, SaveRecordedLessonDetails);
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

     
        // saving the details to scriptable object and storage
        void SaveRecordedLessonDetails(string title, string des)
        {
            // checking for empty title and description if so we just use default ones
            if (string.IsNullOrEmpty(title))
                title = currentAudioClipDetail.Title;
            if (string.IsNullOrEmpty(des))
                des = currentAudioClipDetail.Description;
            currentAudioClipDetail.lessonDetailFromServer = new RecordedLessonDetail();
            currentAudioClipDetail.Title = title;
            currentAudioClipDetail.Description = des;
            rlManager.addRecordedLessonToList(currentAudioClipDetail);
            currentPlayingAudioClipDetail = currentAudioClipDetail;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(true);
        }
        #endregion

        #region AUDIO CLIP METHODS
        // loading latest recorded audio clip for the loaded experience
        async Task loadAudioClip(string fileName)
        {

            // loading the clip from storage
            currentAudioClip = await rlManager.loadRecordedLesson(fileName);
            // assigning the clip to audio source
            audioSource.clip = currentAudioClip;
            enablePlayLesson = true;
        }

        async void loadAndPlayAudioClip(string fileName, bool play)
        {
            await loadAudioClip(fileName);

            experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(true);
            if (play)
                playCurrentClip();
        }

        // plays the current audio clip
        // assigned to the side play/pause button
        public void playCurrentClip()
        {
            if (currentAudioClip == null)
                return;

            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseGraphicsUpdate(false);
                updateSlider = false;
            }
            else
            {

                if (audioSource.time != 0)
                {
                    audioSource.Play();
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseGraphicsUpdate(true);
                    updateSlider = true;
                }
            }
            
            playCurrentClipInternal();
        }

        void stopAudioClip()
        {
            audioSource.Stop();
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderStop();
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseGraphicsUpdate(false);
            updateSlider = false;
            enablePlayLesson = true;
        }

        void playCurrentClipInternal()
        {
            if (!enablePlayLesson)
                return;
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            audioSource.Play();
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().audioSliderStart(currentAudioClip.length);
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().playPauseGraphicsUpdate(true);
            updateSlider = true;
            enablePlayLesson = false;
        }

        #endregion

        #region STUDENT METHODS
        public void getCustomExperienceLessonsFromServer()
        {
            // here we load the private shared/ public shared lessons from teachers of the loaded experience

        }

        // assigne to button;
        public void getStudentLessonsFromServer()
        {
            getStudentLessonsFromServerForLoadedExperienceSync();
        }

        // getting lessons from server for student
        public async void getStudentLessonsFromServerForLoadedExperienceSync()
        {
            // LOADING SCREEN ON;
            bool success = await slManager.getStudentLessonsFromServerForLoadedExperience();
            // LOADING SCREEN OFF;
            if (success)
            {
                // make ui for server loaded lessons
                
                if(slManager.getStudentLessonsList().Count > 0)
                {
                    lessonsUI.enableLessonCanvas();
                    // turning off the experience for interaction
                    expInteraction.setExpInteractionState(false);
                    lessonsUI.makeStudentLessonButtons(slManager.getStudentLessonsList(), studentLessonsActions);
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(true);
                    expInteraction.setExpInteractionState(true); // Shariz hologo 2.0.2 15th dec 2019
                }
                else
                {
                    experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                    string message = localizationProvider.provide.getLanguage().getAMessageText(29);
                    windowMain.InfomationToast.ShowToast(message, 5f);
                }
                
            }
            else
            {
                Debug.Log("failed");
                // no lessons for this experience
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                string message = localizationProvider.provide.getLanguage().getAMessageText(29);
                    windowMain.InfomationToast.ShowToast(message, 5f);
            }
        }

        //load the last recorded audio clip for the loaded experience if it exits
        void studentLessonAudioClipForLoadedExperience()
        {
            // getting file name of the clip from lesson list
            RecordedLessonDetail rd = slManager.getTheLoadedExperiencesLesson();
            // checking for null value.
            if (rd == null)
            {
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
                return;
            }

            experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(true);
            loadAndPlayAudioClip(rd.fileName, false);
        }


        // assigned to the lesson ui button
        void downloadStudentLessonClip(GameObject go)
        {

            // closing lesson canvas
            lessonsUI.disableLessonCanvas();
            // turning off the experience for interaction
            expInteraction.setExpInteractionState(true);
            // enabling the download indicator group
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().downloadGroupActiveStatus(true);
            isLessonDownloading = true;
            studentLessonUIElement ldc = go.GetComponent<studentLessonUIElement>();
            downloadAudioClipButton(ldc.getData() as RecordedLessonDetail);
        }


        // 
        public async void downloadAudioClipButton(RecordedLessonDetail lesson)
        {
            bool succes = await slManager.downloadAudioClip(lesson);

            isLessonDownloading = false;
            // disabling the download indicator group
            experienceMainUIElement.GetExperienceDisplayUIElementDevice().downloadGroupActiveStatus(false);
            if (succes)
            {
                // load clip now.
                // can enable play button and what not
                loadAndPlayAudioClip(lesson.fileName, true);
            }
            else
            {
                // failed to download
            }
        }

        #endregion

        #region RECORDED LESSONS SMART BUTTON FUNCTIONS

        void playClip(GameObject go)
        {
            //Play clip
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            disableLessonCanvas();
            if(acd.FileName != null)
            {
                loadAndPlayAudioClip(acd.FileName, true);
                currentPlayingAudioClipDetail = acd;
            }
            else
            {
                string no_audio = localizationProvider.provide.getLanguage().getAMessageText(30);
                string message = localizationProvider.provide.getLanguage().getAMessageText(31);
                string ok = localizationProvider.provide.getLanguage().getAButtonText(0);
                windowMain.ModalWindowD.ShowInfo(true, false, no_audio, message, ok);
            }
            
        }

        public void disableLessonCanvas()
        {
            lessonsUI.disableLessonCanvas();
            expInteraction.setExpInteractionState(true);
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
                windowMain.InfomationToast.ShowToast(message, 3f);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {
                Debug.Log("failed");
                string message = localizationProvider.provide.getLanguage().getAMessageText(35);
                windowMain.InfomationToast.ShowToast(message, 3f);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
        }

        void deleteClip(GameObject go)
        {
            //delete clip
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
                string delete_from_storage = localizationProvider.provide.getLanguage().getAMessageText(36);
                string delete_from_server = localizationProvider.provide.getLanguage().getAMessageText(37);
                string delete_from_both = localizationProvider.provide.getLanguage().getAMessageText(38);
            if (acd.lessonDetailFromServer.id > 0)
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

        void deleteOnlyLocal(GameObject go)
        {
            Debug.Log("delete from local");
            recordedLessonUIElement rluie = go.GetComponent<recordedLessonUIElement>();
            audioClipDetail acd = rluie.getData() as audioClipDetail;
            stopAndUnloadCurrentClipBeingDeleted(acd);
            rlManager.removeAndDeleteLessonFromList(acd);
            Destroy(go);
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
            SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
        }


        void stopAndUnloadCurrentClipBeingDeleted(audioClipDetail acd)
        {
            if(currentPlayingAudioClipDetail.LessonId == acd.LessonId)
            {
                //Debug.Log(currentPlayingAudioClipDetail.LessonId + "and" + acd.LessonId);
                stopAudioClip();
                experienceMainUIElement.GetExperienceDisplayUIElementDevice().setStateSidePlayPauseGroup(false);
            }
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
            if (success)
            {
                string message = localizationProvider.provide.getLanguage().getAMessageText(39);
                windowMain.InfomationToast.ShowToast(message, 2f);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {   
                string message = localizationProvider.provide.getLanguage().getAMessageText(40);
                windowMain.InfomationToast.ShowToast(message, 3f);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
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
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            else
            {   
                string message = localizationProvider.provide.getLanguage().getAMessageText(40);
                windowMain.InfomationToast.ShowToast(message, 3f);
                iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);// Shariz 17th Oct 2019 2.00
                SoundManager.instance.PlaySoundFromList(0);// Shariz 2nd Nov 2019 2.00
            }
            rlManager.removeLessonFromServerList(acd);
            Destroy(go);
        }
        #endregion

    }
}
