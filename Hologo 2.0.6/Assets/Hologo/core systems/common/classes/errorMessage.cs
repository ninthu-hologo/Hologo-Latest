using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    public class errorMessage
    {
        private string errorString;

        public string ErrorMessage
        {
            get { return errorString; }
        }


        public void setErrorMessage(string message)
        {
            errorString = message;
        }
    }
}
