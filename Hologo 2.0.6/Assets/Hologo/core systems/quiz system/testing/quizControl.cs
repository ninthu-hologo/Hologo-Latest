using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;

public class quizControl : MonoBehaviour
{


    [SerializeField]
    private quiz_SO currentQuiz;
    [SerializeField]
    private quizInstantiator quizMaker;
    [SerializeField]
    private experienceLocalizationData_SO experienceLocalizedData;


    private void Start()
    {
       // quizMaker.MakeQuiz(currentQuiz, experienceLocalizedData.quizSlides);
    }







}
