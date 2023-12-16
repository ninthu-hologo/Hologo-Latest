using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactionType { LABEL,QUIZPICK , PORTAL,ARMODEL}

public interface IClickable 
{
    interactionType returnMyType();
    void executeAction();
}
