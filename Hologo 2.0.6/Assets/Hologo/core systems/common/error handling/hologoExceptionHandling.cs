using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





public class HologoInternetException : Exception
{

    private string errorMessage = "Somthing wrong with the hologo network";

    public HologoInternetException(string error)
    {
        if (!String.IsNullOrEmpty(error))
        {
            errorMessage = error;
        }
    }

    public override string ToString()
    {
        return errorMessage;
    }

    public override string Message => errorMessage;
}



