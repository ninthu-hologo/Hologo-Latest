using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
public class localizationUISwitcher : MonoBehaviour
{
    // Start is called before the first frame update
[Header("DATA")]
[SerializeField]
private settings_SO mainSettings;

[SerializeField]
private RectTransform backButton;
[Header("UI CONNECTOR")]
        [SerializeField]
        private localizationUIElement localizeUIMobile;

        [SerializeField]
        private localizationUIElement localizeUITablet;

    
    
    public localizationUIElement getlocalizeUI()
    {
        if(mainSettings.device == deviceType.TABLET)
        {
            
            return localizeUITablet;
        }
        return localizeUIMobile;
    }

    public void Start(){
        if(mainSettings.device == deviceType.IPHONEX)
        {
            backButton.sizeDelta = new Vector2(backButton.sizeDelta.x,backButton.sizeDelta.y+30);
            backButton.offsetMax = new Vector2(backButton.offsetMax.x,backButton.offsetMax.y-30);
        }
    }
}
