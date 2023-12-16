using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class counterCustom : MonoBehaviour
{

    public float startNumber = 0;
    public float endNumber = 10;

    // public float incrementAmount = 1;

    public bool countDown = false;

    // public float time = 1;

    public float counter = 0;

    public TextMeshProUGUI counterText;





    // Start is called before the first frame update
    void Start()
    {
        counter = startNumber;

        counterText.text = counter.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (counter <= endNumber)
        {
                counter += Time.deltaTime;
        }

 
     //string minutes = Mathf.Floor(counter / 60).ToString("00");
     string seconds = (counter % 60).ToString("00");
     float fraction = counter * 1000;
     fraction = (fraction % 100);
    //  string milliseconds = (counter * 100).ToString("00");
     counterText.text = string.Format("{0}:{1:00}", seconds, fraction.ToString("00"));

    }


}
