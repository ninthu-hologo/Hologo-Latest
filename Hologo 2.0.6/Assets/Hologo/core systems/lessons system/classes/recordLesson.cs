using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 4 july 2019
    /// Modified by: 
    /// this class records an audio clip and saves to storage
    /// </summary>
    public class recordLesson
    {
        private int recordingLenght;
        private float timer = 0f;
        private int wavQuality;
        private int sampleEnd;
        private AudioClip recordClip;
        private AudioClip trimmedAudioClip;

        private bool isRecording = false;
        private bool isTimerStarted = false;

        private string fileName;
        private string folderName;
        //private string recordedAudioClipName;

        // set specifics when instancing class
        public recordLesson(int recordLenght, int waveQuality)
        {
            recordingLenght = recordLenght;
            this.wavQuality = waveQuality;
        }

        //Start recording audio
        public void StartVoiceRecording( string fileName , string folderName)
        {
            this.fileName = fileName;
            this.folderName = folderName;
            Debug.Log("started recording");
            if (isRecording == false)
            {
                isRecording = true;
                // actual recording starts here
                recordClip = Microphone.Start(null, false, recordingLenght + 2, wavQuality);

                isTimerStarted = true;
            }
            else
            {
                isTimerStarted = false;

                StopRecording();
            }
        }

        // stop recording
        public void StopRecording()
        {
            //Debug.Log("stopped recording");
            stopTimer();
            isRecording = false;
            sampleEnd = Microphone.GetPosition(null);
            Microphone.End(null);
            trimmedAudioClip = TrimSilence(recordClip, sampleEnd);
            //string clipName = recordedAudioClipName;
            string filePath = readWriteData.GetPath(folderName,fileName);
            //saving the file to disk
            SaveToWave mysave = new SaveToWave();
            //this saves the clip to disk
            mysave.Save(filePath, trimmedAudioClip);
        }


        public bool isAudioInRecording()
        {
            return !isRecording && getRecordedClip() != null;
        }

        // trimming audio silence
        AudioClip TrimSilence(AudioClip clip, int endSample)
        {
            if (endSample <= 0)
            {
                return clip;
            }
            else
            {
                float[] samples = new float[clip.samples * clip.channels];
                clip.GetData(samples, 0);
                float[] ClipSamples = new float[endSample];
                Array.Copy(samples, ClipSamples, ClipSamples.Length - 1);
                var RecreatedAudioClip = AudioClip.Create("TempClip", ClipSamples.Length, 1, wavQuality, false);
                RecreatedAudioClip.SetData(ClipSamples, 0);
                return RecreatedAudioClip;
            }
        }

        // Update is called once per frame// this should be run in parent monobehviors update
        public float updateTimer()
        {
            startTimer();
            autoRecordStop();
            return timer;
        }

        // starts the timer and updates the timer ui
        void startTimer()
        {
            if (isTimerStarted)
            {
                timer += Time.deltaTime;
            }
        }

        void stopTimer()
        {
            isTimerStarted = false;
            timer = 0f;
        }

        public bool timerStatus()
        {
            return isTimerStarted;
        }

        // will trigger stop recording once recording lenght has been reached
        void autoRecordStop()
        {
            if (isTimerStarted)
            {
                if (timer >= recordingLenght + 1)
                {
                    isTimerStarted = false;
                    StopRecording();
                }
            }
        }


        public AudioClip getRecordedClip()
        {
            return trimmedAudioClip;
        }

    }



}
