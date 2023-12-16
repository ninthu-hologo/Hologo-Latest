using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Hologo2;

public class asyncArTestingBootUp : MonoBehaviour
{

    [Header("DATA FIELDS")]
    [SerializeField]
    private settings_SO settings;
    [Header("SCENE SCRIPTS")]
    [SerializeField]
    private experienceAssetBootUp expAssetBootUp;
    [SerializeField]
    private experienceViewController eVController;
    [SerializeField]
    private quizViewController qVController;
    [Header("UI ELEMENTS")]
    [SerializeField]
    private arLoadingSceneUICanvas arLoading;


    private void Start()
    {
        eVController.initiateExperienceViewController();
        // initiating quiz view controller
        qVController.initiateQuizViewController(eVController.getExperienceData().quiz, eVController.getExperienceLocalizationData());
        // starting the process of showing and generating the experience to the user
        arLoading.disableLoadingARCanvas();
    }


    // boot up settings.
    // runinng assetbundle boot up;
    async Task initExpAssetLoad()
    {
        await expAssetBootUp.runBootUp();
    }

}
