using UnityEngine;
using System.Collections;
using System;
using Hologo2;
using Hologo.iOSUI;

public class BrowserPayment : MonoBehaviour
{

    public string paymentURL = "https://lite.hologo.world/subscriptions/make_genie_payment/";
    public string WindowTitle = "Browser";
    public event_SO paymentEvent;
    public event_SO subscriptionSuccessEvent;
    public event_SO subscriptionFailEvent;
    private string pageToOpen;

    [Header("Scene Objects")]
    [SerializeField]
    private iOS_UIModalWindowMain windowMain;


    // check readme file to find out how to change title, colors etc.
    public void OnButtonClicked()
    {
        pageToOpen = paymentURL + paymentEvent.eventDetails[0].passedInteger;
        Debug.Log("Payment URL is "+ pageToOpen);
        InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
        options.displayURLAsPageTitle = false;
        options.pageTitle = WindowTitle;
        options.hidesDefaultSpinner = true;

        //This is for testing without browser. Comment this when connecting to browser
        //TestPayment();

        // Comment this out when done testing
        InAppBrowser.OpenURL(pageToOpen, options);
    }


    public void TestPayment()
    {
        string title = "Testing Payment success and fail!";
        string description = "Testing what happens in each scenario";
        string titleOne = "Success!";
        string bodyOne = "Click to see what happens when success!";
        string titleTwo = "Fail!";
        string bodyTwo = "Click to see what happens when fail!";
        windowMain.ModalWindowC.ChoiceMaker(title, description, titleOne, titleTwo, "", false, true, false, bodyOne,
                    bodyTwo, null, null, null, null, onSuccess, OnFail, null, null);
    }

    public void onSuccess()
    {
        Debug.Log("Success!");
        StartCoroutine(CloseBrowserAfter5Seconds());
        windowMain.LoadingWindowStatus(false);
        subscriptionSuccessEvent.raise(this.gameObject);
    }

    public void OnFail()
    {
        Debug.Log("Fail!");
        string failMessage = "Fail";
        subscriptionFailEvent.eventDetails[0].passedMessage = failMessage;
        subscriptionFailEvent.raise(this.gameObject);
        StartCoroutine(CloseBrowserAfter5Seconds());
    }


    public void OnClearCacheClicked()
    {
        InAppBrowser.ClearCache();
    }

    public void Start()
    {
        InAppBrowserBridge bridge = FindObjectOfType<InAppBrowserBridge>();
        bridge.onJSCallback.AddListener(OnMessageFromJS);
        bridge.onBrowserClosed.AddListener(onCloseBrowser);
    }

    void onCloseBrowser()
    {
        windowMain.LoadingWindowStatus(false);
    }



    void OnMessageFromJS(string jsMessage)
    {
        if (jsMessage.Equals("ping"))
        {
            Debug.Log("Ping message received!");

            InAppBrowser.ExecuteJS("alert('Hello! I am Hologo!!');");             StartCoroutine(CloseBrowserAfter5Seconds());
        }
        if (jsMessage.Equals("success"))
        {
            Debug.Log("Success!");
            StartCoroutine(CloseBrowserAfter5Seconds());
            windowMain.LoadingWindowStatus(false);
            subscriptionSuccessEvent.raise(this.gameObject);
        }
        if (jsMessage.Equals("fail"))
        {
            Debug.Log("Fail!");
            string failMessage = "Fail";
            StartCoroutine(CloseBrowserAfter5Seconds());
            subscriptionFailEvent.eventDetails[0].passedMessage = failMessage;
            subscriptionFailEvent.raise(this.gameObject);
        }
    }

    private IEnumerator CloseBrowserAfter5Seconds()
    {
        yield return new WaitForSeconds(3.0f);
        InAppBrowser.CloseBrowser();
    }
}