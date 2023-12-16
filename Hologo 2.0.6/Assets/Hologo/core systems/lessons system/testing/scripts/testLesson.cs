using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Hologo2
{
    
    public class testLesson : MonoBehaviour
    {

        public AudioSource mysource;
        public AudioClip currentClip;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void loadAudio()
        {
             startAudioLoad();
            //StartCoroutine(LoadLessonToPlay("ocs.wav", "rld"));
        }

        async void startAudioLoad()
        {
            lessonsServerConnect lsc = new lessonsServerConnect();
            AudioClip song = await lsc.loadRecordedLessonFromStorage("ocs.wav", "rld");
             mysource.clip = song;
             mysource.Play();
        }

       
    }
}
