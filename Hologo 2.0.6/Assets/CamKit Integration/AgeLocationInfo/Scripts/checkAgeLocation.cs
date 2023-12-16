using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.IO;

public class checkAgeLocation : MonoBehaviour
{
    
    public GameObject promptPanel, agePanel, guardianPanel, countryPanel, statePanel, failurePanel;

    [SerializeField]
    private TMP_Dropdown countryDropdown;

    private bool above13, guardian, unitedStates, IllinoisTexas;

    private GameObject promptPanelParent;

    bool clickedNext = false;

    private void Start()
    {
        /*promptPanel = GameObject.Find("promptPanel");
        agePanel = GameObject.Find("agePanel");
        guardianPanel = GameObject.Find("guardianPanel");
        countryPanel = GameObject.Find("countryPanel");
        statePanel = GameObject.Find("statePanel");
        failurePanel = GameObject.Find("failurePanel");

        promptPanel.SetActive(false);
        agePanel.SetActive(false);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);*/
        promptPanel = GameObject.Find("promptPanel");
        agePanel = promptPanel.transform.GetChild(0).gameObject;
        guardianPanel = promptPanel.transform.GetChild(1).gameObject;
        countryPanel = promptPanel.transform.GetChild(2).gameObject;
        statePanel = promptPanel.transform.GetChild(3).gameObject;
        failurePanel = promptPanel.transform.GetChild(4).gameObject;


        promptPanel.SetActive(false);
        agePanel.SetActive(false);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);
    }

    public void showPrompt()
    {
        promptPanel.SetActive(true);

        agePanel.SetActive(true);


    }

    public void promptAgeLocationInfo()
    {
        promptPanelParent = GameObject.Find("promptPanelParent");

        promptPanel = Instantiate(promptPanel, promptPanelParent.transform);
        agePanel = Instantiate(agePanel, promptPanel.transform);
        guardianPanel = Instantiate(guardianPanel, promptPanel.transform);
        countryPanel = Instantiate(countryPanel, promptPanel.transform);
        statePanel = Instantiate(statePanel, promptPanel.transform);
        failurePanel = Instantiate(failurePanel, promptPanel.transform);

        promptPanel.SetActive(false);
        agePanel.SetActive(false);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);

        if (File.Exists(Application.persistentDataPath + "/userAgeInfo.hff"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/userAgeInfo.hff");
            string info = reader.ReadToEnd();
            reader.Close();

            string age = info.Split(',')[0];
            string country = info.Split(',')[1];
            string state = info.Split(',')[2];

            if(age == "False" && country == "United States" && state == "True")
            {
                promptPanel.SetActive(true);
                failurePanel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("User age and location file does not exist");
            promptPanel.SetActive(true);
            agePanel.SetActive(true);

            if (above13)
            {
                agePanel.SetActive(false);
                promptPanel.SetActive(false);
            }
            else if (!above13 && clickedNext)
            {
                agePanel.SetActive(false);
                guardianPanel.SetActive(true);

                clickedNext = false;

                if(!guardian)
                {
                    guardianPanel.SetActive(false);
                    failurePanel.SetActive(true);
                }
                else
                {
                    guardianPanel.SetActive(false);
                    countryPanel.SetActive(true);

                    if(!unitedStates)
                    {
                        countryPanel.SetActive(false);
                        promptPanel.SetActive(false);
                    }
                    else
                    {
                        countryPanel.SetActive(false);
                        statePanel.SetActive(true);

                        if(!IllinoisTexas)
                        {
                            statePanel.SetActive(false);
                            promptPanel.SetActive(false);
                        }
                        else
                        {
                            statePanel.SetActive(false);
                            failurePanel.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void setAge(bool above13_input)
    {
        above13 = above13_input;
        clickedNext = true;
    }

    public void setGuardian(bool guardian_input)
    {
        guardian = guardian_input;
        clickedNext = true;
    }

    public void setCountry()
    {
        if(countryDropdown.options[countryDropdown.value].text == "United States")
        {
            unitedStates = true;
        }
        else
        {
            unitedStates = false;
        }
    }

    public void setState(bool illinoisTexas_input)
    {
        IllinoisTexas = illinoisTexas_input;
    }

    public void closePromptPanel()
    {
        promptPanel.SetActive(false);
    }

    public void changeInfo()
    {
        promptAgeLocationInfo();
    }
}
