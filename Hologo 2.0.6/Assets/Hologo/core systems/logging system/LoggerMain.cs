using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo;
using System;
using System.Globalization;



namespace Hologo.Logging
{


    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com)
    /// Modified by: Hamid (hamidibrahim@gmail.com) 02-12-2018
    /// this class contains all the methods that are used to manage and log hologo errors and success.
    /// </summary>

    public class LoggerMain : MonoBehaviour, IHologoLogger
    {


        [SerializeField]
        private LogginDataObject ThisSession ;

        // this makes sure that this script will remain in memory throught out the life of the app
        public static LoggerMain LoggerSingleton
        {
            get { return instance; }
        }

        private static LoggerMain instance;

        void Awake()
        {
            if (LoggerSingleton == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                ThisSession = Instantiate(ThisSession);
                StartCoroutine(BootSequence());
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        public IEnumerator BootSequence()
        {
            createNewSessionLog();

            DeleteLogFiles();

            //Debug.Log("Logger Booted");

            yield return null;
        }




        void createNewSessionLog()
        {
            ThisSession.createNewSessionLog();
        }


        //when logging the log is first added t a queue which will write to the file linearly as to avoid differenct classes
        // writing to the file at the same time.
        public void LogOK(string title, string description, string loggedobject)
        {
           ThisSession.AddLog(new HologoLog(title, 101, description, loggedobject));

        }


        public void LogTimeOut(string title, string description, string loggedobject)
        {

           ThisSession.AddLog(new HologoLog(title, 102, description, loggedobject));

        }

        public void LogError(string title, string description, string loggedobject)
        {

           ThisSession.AddLog(new HologoLog(title, 103, description, loggedobject));

        }

        public void DeleteLogFiles()
        {
            StartCoroutine(ThisSession.DeleteLogs());
        }


    }





    // hologo number of sessions logged and details
    [Serializable]
    public class LogLedger
    {
        public List<string> Logs;
    }



    // hologo sesson object
    [Serializable]
    public class HologoLogSession
    {

        // some user data like region it was and
        // when log session was created
        public string SessionDate;
        public string Region;
        public string Device;
        public List<HologoLog> SessionLogs;



        public HologoLogSession()
        {
            SessionLogs = new List<HologoLog>();
        }


        public void AddLog(HologoLog log)
        {
            SessionLogs.Add(log);
        }

        // we can use this class to separate only error and timeouts and sort stuff

    }



    // hologo log object
    [Serializable]
    public class HologoLog
    {
        public string Title;
        public int ErrorCode;   // 101 = ok, 102 = time out, 103 = error
        public string LogDate;
        public string Description;
        public string LoggedObject;
        public HologoLog(string title, int eCode, string description, string loggedobject)
        {
            Title = title;
            ErrorCode = eCode;
            LogDate = DateTime.Now.ToString();
            Description = description;
            LoggedObject = loggedobject;
        }
    }




}
