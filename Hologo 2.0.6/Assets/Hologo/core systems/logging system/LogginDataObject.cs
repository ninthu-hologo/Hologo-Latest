using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;



namespace Hologo.Logging
{

    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com)
    /// Modified by: Hamid (hamidibrahim@gmail.com) 02-12-2018
    // scriptable object that keeps all the data for the session
    // without needing to initialize data between scene changes
    /// </summary>
    [CreateAssetMenu(fileName = "LoggerDataObject", menuName = "Hologo/Logging", order = 1)]
    public class LogginDataObject : ScriptableObject
    {

        [SerializeField]
        private LogLedger logledger;

        [SerializeField]
        private HologoLogSession Session;

        private string sessionFileName;


        [SerializeField]
        private bool SessionIsOngoing = false;

        private string FolderName = appEVars.AppConfigFolder;

        private string LedgerFileName = appEVars.LogsTrackerFileName;

        private int numberOfLogsTokeep = appEVars.NumberOfLogs;


        // init logging session
        public void createNewSessionLog()
        {

            if (SessionIsOngoing == false)
            {

                SessionIsOngoing = true;

                ReadHologoLog();

                string dateTime = System.DateTime.Now.ToString("MMddyyyy_HHmmss");

                sessionFileName = "hlog" + (logledger.Logs.Count + 1)+"_" + dateTime;

                logledger.Logs.Add(sessionFileName);
                SaveLogFile(logledger, FolderName, LedgerFileName);

                Session = new HologoLogSession();
                Session.SessionDate = System.DateTime.Now.ToString();
                Session.Device = SystemInfo.deviceModel;


                //  Session.AddLog("Logger", "successfully booted", "n/a");
                AddLog(new HologoLog("Logger", 101, "successfully booted", "n/a"));
            }
            else
            {
                Debug.Log("hologo log already booted");
            }
        }

        // ad log and save the file
        public void AddLog(HologoLog log)
        {
            Session.AddLog(log);

            SaveLogFile(Session, FolderName, sessionFileName);
        }


        //Delete Logs if the count is more then the number

        public IEnumerator DeleteLogs()
        {
            if(logledger.Logs.Count > numberOfLogsTokeep)
            {
                //deleting all logs
                for (int i = 0; i < logledger.Logs.Count-1; i++)
                {
                   readWriteData.DeleteFileOnDisk(logledger.Logs[i], FolderName);
                    yield return null;
                }
                // deleting ledger and making a new one. we could just remove logs from ledger and save but too lazy
                readWriteData.DeleteFileOnDisk(LedgerFileName, FolderName);
                yield return null;
                creatLogTrackerFile(FolderName, LedgerFileName);
                logledger.Logs.Add(sessionFileName);
                SaveLogFile(logledger, FolderName, LedgerFileName);
                yield return null;
            }
        }





        // read the list of logs that has been logged
        void ReadHologoLog()
        {
            string folderName = FolderName;
            string fileName = LedgerFileName;

            if (readWriteData.CheckIfFileExists(folderName, fileName))
            {
                string readFileContents = readWriteData.ReadFileFromDisk(fileName, folderName);

                if (string.IsNullOrEmpty(readFileContents))
                {
                    creatLogTrackerFile(folderName, fileName);

                    return;
                }

                GenericObjectAndStatus<LogLedger> HologoConfigAndStatus = jsonHelper.DeserializeFromJson<LogLedger>(readFileContents);

                if (HologoConfigAndStatus.Success)
                {
                    logledger = HologoConfigAndStatus.GenericObject;
                }


            }
            else
            {
                creatLogTrackerFile(folderName, fileName);
            }
        }


        void creatLogTrackerFile(string folderName, string fileName)
        {
            if (readWriteData.Createfolder(folderName))
            {
                logledger = new LogLedger();
                logledger.Logs = new List<string>();
                SaveLogFile(logledger, folderName, fileName);
            }
        }


        public void SaveLogFile<T>(T history, string folderName, string fileName)
        {
            string convertShoplistObjectjson = JsonUtility.ToJson(history, true);
            readWriteData.WriteFileToDisk(convertShoplistObjectjson,false, folderName,fileName);
        }

    }
}
