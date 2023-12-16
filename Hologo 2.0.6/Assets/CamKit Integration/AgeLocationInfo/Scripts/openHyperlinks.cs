using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class openHyperlinks : MonoBehaviour, IPointerClickHandler
{
    private const string termsOfService_link = "https://hologo.world/terms-and-conditions";
    private const string privacyPolicy_link = "https://hologo.world/policies";
    private const string learnMore_link = "https://hologo.world/hologo-lenses";

    public void OnPointerClick(PointerEventData eventData)
    {
        var linkIndex = TMP_TextUtilities.FindIntersectingLink(this.GetComponent<TMP_Text>(), Input.mousePosition, null);
        var linkID = this.GetComponent<TMP_Text>().textInfo.linkInfo[linkIndex].GetLinkID();

        string url = "";

        if(linkID == "TermsOfService")
        {
            url = termsOfService_link;
        }
        if(linkID == "PrivacyPolicy")
        {
            url = privacyPolicy_link;
        }
        if(linkID == "LearnMore")
        {
            url = learnMore_link;
        }

        Debug.Log("hyperlink clicked");

        Application.OpenURL(url);
    }
}
