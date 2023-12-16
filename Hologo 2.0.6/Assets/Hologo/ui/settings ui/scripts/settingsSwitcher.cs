using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
public class settingsSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
[Header("DATA")]
[SerializeField]
private settings_SO mainSettings;


[SerializeField]
private RectTransform backButton;

[Header("UI CONNECTOR")]
        [SerializeField]
        private settingsUI settingsUIMobile;

        [SerializeField]
        private settingsUI settingsUITablet;

        [SerializeField]
        private GameObject settingsObjectMobile;

        [SerializeField]
        private GameObject settingsObjectTablet;

        [SerializeField]
        private playBarUIElement playerMobile;// Shariz 2nd Nov 2019 2.00

        [SerializeField]
        private playBarUIElement playerTablet;// Shariz 2nd Nov 2019 2.00


    
    
    public settingsUI getSettingsUI()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return settingsUITablet;
        }
        return settingsUIMobile;
    }
    public playBarUIElement getPlayerElement() // Shariz 2nd Nov 2019 2.00

    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return playerTablet;
        }
        return playerMobile;
    }
    
    public GameObject getSettingsObject()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            return settingsObjectTablet;
        }
        return settingsObjectMobile;
    }

    public void Start(){
        if(mainSettings.device == deviceType.IPHONEX)
        {
            backButton.sizeDelta = new Vector2(backButton.sizeDelta.x,backButton.sizeDelta.y+30);
            backButton.offsetMax = new Vector2(backButton.offsetMax.x,backButton.offsetMax.y-30);
        }
    }
}
