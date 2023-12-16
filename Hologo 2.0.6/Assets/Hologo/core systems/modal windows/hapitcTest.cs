using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo.iOSUI;

public class hapitcTest : MonoBehaviour {

    public GameObject SmartDelete;
    public GameObject SmartOptions;
    public GameObject hapticTest;
    public GameObject exp;
    public GameObject modal;

    public iOS_ModalWindowB.ButtonOptions WindowBOptions = iOS_ModalWindowB.ButtonOptions.CancelOk;
    public bool customDel = false;


    private iOS_UIModalWindowMain windowMain;

    void Start()
    {
        windowMain = iOS_UIModalWindowMain.Instance();
       
    }

	
    public void SelectionChange()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
    }
    public void Light()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
    }
    public void Medium()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
    }
    public void Heavy()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
    }

    public void success()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
    }

    public void warning()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Warning);
    }
    public void failure()
    {
        // iOSHapticFeedback.iOSFeedbackType feedbackType
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
    }




    public void ChangeView(int num)
    {
        switch (num)
        {
            case 1:
                SmartDelete.SetActive(true);
                SmartOptions.SetActive(false);
                hapticTest.SetActive(false);
                exp.SetActive(false);
                modal.SetActive(false);
                break;
            case 2:
                SmartDelete.SetActive(false);
                SmartOptions.SetActive(true);
                hapticTest.SetActive(false);
                exp.SetActive(false);
                modal.SetActive(false);
                break;
            case 3:
                SmartDelete.SetActive(false);
                SmartOptions.SetActive(false);
                hapticTest.SetActive(true);
                exp.SetActive(false);
                modal.SetActive(false);
                break;
            case 4:
                SmartDelete.SetActive(false);
                SmartOptions.SetActive(false);
                hapticTest.SetActive(false);
                exp.SetActive(true);
                modal.SetActive(false);
                break;
            case 5:
                SmartDelete.SetActive(false);
                SmartOptions.SetActive(false);
                hapticTest.SetActive(false);
                exp.SetActive(false);
                modal.SetActive(true);
                break;
                
            default:
                break;
        }
    }



    public void OpenWindowAOptions()
    {
       
        windowMain.ModelWindowA.CancelAndDeleteWithOptions(false, "Share", "Edit", "Hide", Cancelled, Deleted, funcOne);
    }
    public void OpenWindowACD()
    {
        windowMain.ModelWindowA.CancelAndDelete(Cancelled, Deleted);
    }


    public void OpenWindowB()
    {
        string title = "This is a text Title ?";
        string description = "Some description needing urgent reading before pressing a button";
        windowMain.ModelWindowB.ChoiceMaker(false, WindowBOptions,title,description,"Remove","Good Day!", null, Cancelled,Ok,Deleted,funcThree);// Shariz 29th Oct 2019 2.00
    }

    public void OpenWindowC()
    {
        string title = "A Big Title No?";
        string description = "Some description needing urgent reading before pressing a button";
        //windowMain.ModalWindowC.ChoiceMaker(title, description, "Share", "Edit", "Something different", Cancelled, funcOne, funcTwo, funcThree);
    }



    void Cancelled()
    {
        //Debug.Log("Cancelled");
    }

    void Deleted()
    {
        //Debug.Log("Deleted");
    }

    void Ok()
    {
       // Debug.Log("OK"); 
    }

    void funcOne()
    {
       // Debug.Log("Option One");
    }
    void funcTwo()
    {
        //Debug.Log("Option Two");
    }
    void funcThree()
    {
        //Debug.Log("Option Three");
    }

}
