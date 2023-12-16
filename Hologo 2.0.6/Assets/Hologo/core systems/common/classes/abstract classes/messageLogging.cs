using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 02 june 2019
    /// Modified by: 
    /// this abstract class is for logging and error message handling. every mono-behaviour that needs that extends from this class 
    /// </summary>
    public abstract class messageLogging : MonoBehaviour, IReadMessage
    {

        // all messages are written to this class
        protected errorMessage messager;




        protected void createMessage(string message)
        {
            if (messager == null)
            {
                messager = new errorMessage();
            }
            messager.setErrorMessage(message);
        }

        // interface implementation
        public string readMessage()
        {
            if (messager == null)
            {
                messager = new errorMessage();
                messager.setErrorMessage("An error has occured! Contact support");// Shariz 22nd Feb 2020 2.0.4
            }
            return messager.ErrorMessage;

        }

    }
}