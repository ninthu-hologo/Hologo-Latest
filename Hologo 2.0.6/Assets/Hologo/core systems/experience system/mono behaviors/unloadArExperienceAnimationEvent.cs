using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;

public class unloadArExperienceAnimationEvent : MonoBehaviour
{

    [SerializeField]
    private experienceViewController eVController;
    [SerializeField]
    private loadScene sceneLoader;

    public void AunloadScene()
    {
        eVController.unloadAssetBundle();
        sceneLoader.goToScene();
    }


}
