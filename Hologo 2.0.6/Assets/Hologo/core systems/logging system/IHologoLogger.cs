using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hologo.Logging
{
    public interface IHologoLogger
    {


        void LogOK(string title, string description, string loggedobject);
        void LogTimeOut(string title, string description, string loggedobject);
        void LogError(string title, string description, string loggedobject);


    }
}
