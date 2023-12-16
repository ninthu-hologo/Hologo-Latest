using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Hologo2;

    
public class promptController : MonoBehaviour
{

    [SerializeField]
    private GameObject userAgeInfoCanvas;

    [SerializeField]
    private GameObject agePanel, guardianPanel, countryPanel, statePanel, failurePanel, introPanel, tosPanel;

    [SerializeField]
    private TMP_Dropdown countryDropdown;

    [SerializeField]
    private settings_SO mainSettings;

    string failed = "not set";

    private void Start()
    {
        if (mainSettings.device == deviceType.TABLET)
        {
            agePanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            guardianPanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            countryPanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            statePanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            failurePanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            introPanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            tosPanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);

        }

        userAgeInfoCanvas.gameObject.SetActive(false);
        Debug.Log(Application.persistentDataPath);
    }

    public void showPrompt()
    {
        if (File.Exists(Application.persistentDataPath + "/userInfo.hff"))
        {
            userAgeInfoCanvas.SetActive(false);

            StreamReader reader = new StreamReader(Application.persistentDataPath + "/userInfo.hff");
            string content = reader.ReadToEnd();
            reader.Close();

            failed = content;

            if(failed == "yes")
            {
                userAgeInfoCanvas.SetActive(true);

                agePanel.SetActive(false);
                guardianPanel.SetActive(false);
                countryPanel.SetActive(false);
                statePanel.SetActive(false);
                tosPanel.SetActive(false);

                failurePanel.SetActive(true);
            }
            else if(failed == "no")
            {
                agePanel.SetActive(false);
                guardianPanel.SetActive(false);
                countryPanel.SetActive(false);
                statePanel.SetActive(false);
                failurePanel.SetActive(false);
                tosPanel.SetActive(false);

                userAgeInfoCanvas.SetActive(false);
            }
            else
            {
                userAgeInfoCanvas.SetActive(true);

                agePanel.SetActive(false);
                guardianPanel.SetActive(false);
                countryPanel.SetActive(false);
                statePanel.SetActive(false);
                failurePanel.SetActive(false);

                tosPanel.SetActive(true);
            }
        }
        else
        {
            userAgeInfoCanvas.SetActive(true);

            introPanel.SetActive(true);

            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);
            tosPanel.SetActive(false);
        }
    }

    public void closeIntroPanel()
    {
        introPanel.SetActive(false);
        agePanel.SetActive(true);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);
        tosPanel.SetActive(false);
    }

    public void closeTermsOfServicePanel()
    {
        failed = "not accepted";
        saveInfo();

        introPanel.SetActive(false);
        agePanel.SetActive(false);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);
        tosPanel.SetActive(false);

        userAgeInfoCanvas.SetActive(false);
    }

    public void acceptAndCloseTosPanel()
    {
        failed = "no";
        saveInfo();

        introPanel.SetActive(false);
        agePanel.SetActive(false);
        guardianPanel.SetActive(false);
        countryPanel.SetActive(false);
        statePanel.SetActive(false);
        failurePanel.SetActive(false);
        tosPanel.SetActive(false);

        userAgeInfoCanvas.SetActive(false);
    }

    public void validateAge(string above13)
    {
        if(above13 == "yes")
        {
            failed = "no";
            saveInfo();

            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);

            tosPanel.SetActive(true);
        }
        else if(above13 == "no")
        {
            agePanel.SetActive(false);

            guardianPanel.SetActive(true);

            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);
            tosPanel.SetActive(false);
        }
    }

    public void validateGuardian(string guardian)
    {
        if(guardian == "yes")
        {
            agePanel.SetActive(false);
            guardianPanel.SetActive(false);

            countryPanel.SetActive(true);

            statePanel.SetActive(false);
            failurePanel.SetActive(false);
            tosPanel.SetActive(false);
        }
        else if(guardian == "no")
        {
            agePanel.SetActive(true);

            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);
            tosPanel.SetActive(false);
        }
    }

    public void validateCountry()
    {
        string selectedCountry = countryDropdown.options[countryDropdown.value].text;

        if(selectedCountry == "Illinois" || selectedCountry == "Texas")
        {
            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);

            statePanel.SetActive(false);

            failurePanel.SetActive(true);
        }
        else if(selectedCountry == "")
        {
            return;
        }
        else
        {
            failed = "no";
            saveInfo();

            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);

            tosPanel.SetActive(true);
        }
    }

    public void validateState(string illinoisOrTexas)
    {
        if(illinoisOrTexas == "yes")
        {
            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);

            failurePanel.SetActive(true);
        }
        else if(illinoisOrTexas == "no")
        {
            failed = "no";
            saveInfo();

            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);

            tosPanel.SetActive(true);
        }
    }

    public void controlFailurePanel(string changeDetails)
    {
        if(changeDetails == "yes")
        {
            agePanel.SetActive(true);

            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);
        }
        else if(changeDetails == "no")
        {
            failed = "yes";
            saveInfo();

            agePanel.SetActive(false);
            guardianPanel.SetActive(false);
            countryPanel.SetActive(false);
            statePanel.SetActive(false);
            failurePanel.SetActive(false);

            userAgeInfoCanvas.SetActive(false);
        }
    }

    private void saveInfo()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/userInfo.hff");
        writer.Write(failed);
        writer.Close();
    }

    public void openTermsOfServiceWeb()
    {
        Application.OpenURL("https://hologo.world/terms-and-conditions");
    }
}
