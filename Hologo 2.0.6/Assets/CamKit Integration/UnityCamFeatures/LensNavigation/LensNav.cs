using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LensNav : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    private GameObject lenses;
    //[SerializeField]
    private GameObject library;
    //[SerializeField]
    private GameObject buttonGroup;
    //[SerializeField]
    private GameObject lensCanvas;

    //the lens icon of the lens button in bottom navbar
    [SerializeField]
    private GameObject lensHighlightIcon, lensInactiveIcon;

    [SerializeField]
    private GameObject libraryHighlightIcon, personHighlightIcon;

    //[SerializeField]
    private GameObject deeplinkCanvas;

    // [SerializeField]
    // private GameObject _closeButton;

    [SerializeField]
    private Camera camera;

    private GameObject settingsCanvas;


    private void Start()
    {
        lenses = GameObject.Find("lenses");
        library = GameObject.Find("library v2");
        buttonGroup = GameObject.Find("bottom_bar");
        lensCanvas = GameObject.Find("LensCanvasV2");
        deeplinkCanvas = GameObject.Find("Canvas");
        settingsCanvas = GameObject.Find("settings_only_updated");
    }

/*    public void onButtonClick()
    {

        
        library = GameObject.Find("library v2");
        settingsCanvas = GameObject.Find("settings_only_updated");
        lenses.SetActive(true);
        settingsCanvas.SetActive(false);
        library.SetActive(false);
        //buttonGroup.SetActive(false);

        lensInactiveIcon.SetActive(false);
        lensHighlightIcon.SetActive(true);
        libraryHighlightIcon.SetActive(false);
        personHighlightIcon.SetActive(false);
    }

    public void onMenuClose() // closing lens screen. i. e. navigating to another screen using bottom navbar
    {
        lensHighlightIcon.SetActive(false);
        lensInactiveIcon.SetActive(true);
    }*/

    public void onLensActive()
    {
        lenses = GameObject.Find("lenses");
        //library = GameObject.Find("library v2");
        buttonGroup = GameObject.Find("bottom_bar");
        lensCanvas = GameObject.Find("LensCanvasV2");
        deeplinkCanvas = GameObject.Find("Canvas");

        lenses.SetActive(true);
        //library.SetActive(false);
        buttonGroup.SetActive(true);
        // deeplinkCanvas.SetActive(false);
        //  SceneManager.LoadScene("Camerakit");

    }

    public void onCloseButtonClicked()
    {
        // CameraKitHandler.DismissCameraKit();
        camera.backgroundColor = new Color32(255, 255, 255, 255);
        lensCanvas.SetActive(false);
        lenses.SetActive(true);
        buttonGroup.SetActive(true);
    }


}