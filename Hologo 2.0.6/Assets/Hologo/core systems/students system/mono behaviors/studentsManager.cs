using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 01 august 2019
    /// Modified by: 
    /// this handles students
    /// </summary>
    public class studentsManager : messageLogging
    {

        // paths and names
        [SerializeField]
        private dataPaths_SO dataPath;
        [SerializeField]
        private List<followerDataClass> pendingStudentList;
        [SerializeField]
        private List<followerDataClass> acceptedStudentList;

        [SerializeField]
        public bool hasStudents = false; // Shariz 28th Oct 2019 2.00

        /*
         * {"success":true,
         * "data":[
         * {"id":26114,
         * "name":"jujuju",
         * "email":"juju@j.j",
         * "email_verified_at":"",
         * "verified":1,
         * "contact_number":"1234566",
         * "language_id":6,"country_id":1,
         * "user_type":"student",
         * "current_login_device":"ios",
         * "trial_ends_at":"2019-10-05",
         * "remember_token":"",
         * "created_at":"2019-09-05 08:01:29",
         * "updated_at":"2019-09-08 09:02:40"}]
         * ,"message":"my active connections"}
         */
        // get all the students belonging to the teacher
        public async Task<bool> getStudentsFromServer(string token, int userId)
        {
            bool success = false;
            string result;
            string url = dataPath.getUrl(4) + "?token=" + token;
            teacherServerConnect tsc = new teacherServerConnect();
            var task = tsc.loadTeachersFromServer(url);
            try
            {
                result = await task;

                // serialzing data from server to object
                GenericObjectAndStatus<HologoWebAPIGeneric<List<UserDetail>>> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<UserDetail>>>(result);
                Debug.Log(result);
                if (ApiAndStatus.GenericObject.success)
                {
                    List<UserDetail> ud = ApiAndStatus.GenericObject.data;
                    convertUsersToFollowers(ud, userId);
                    success = true;
                    hasStudents = true;// Shariz 28th Oct 2019 2.00
                }
                else
                {
                    acceptedStudentList.Clear(); //Shariz 13th April 2020 v2.0.5 removing accepted teacher list after log out
                    pendingStudentList.Clear(); //Shariz 13th April 2020 v2.0.5 removing pending teacher list after log out
                    //invalid credentials
                    createMessage(ApiAndStatus.GenericObject.message);
                    hasStudents = false;// Shariz 28th Oct 2019 2.00
                }
            }

            catch (HologoInternetException ex)
            {
                Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }

        public List<followerDataClass> getStudents()
        {
            return acceptedStudentList;
        }


        void convertUsersToFollowers(List<UserDetail> ud, int id)
        {
            acceptedStudentList = new List<followerDataClass>();
            for (int i = 0; i < ud.Count; i++)
            {
                followerDataClass fdc = new followerDataClass();
                fdc.acceptor_id = id;
                fdc.requester_id = ud[i].id;
                fdc.requester = ud[i];
                fdc.status = true;
                acceptedStudentList.Add(fdc);
            }
        }

        public async Task<bool> getStudentsPending(string token)
        {
            bool success = false;
            string result;
            string url = dataPath.getUrl(3) + "?token=" + token;
            Debug.Log(url);
            teacherServerConnect tsc = new teacherServerConnect();
            var task = tsc.loadPendingTeachers(url);

            try
            {
                result = await task;
                GenericObjectAndStatus<HologoWebAPIGeneric<List<followerDataClass>>> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<List<followerDataClass>>>(result);

                Debug.Log(result);
                if(ApiAndStatus.GenericObject.success)
                {
                    pendingStudentList = ApiAndStatus.GenericObject.data;
                }
                else
                {
                    createMessage(ApiAndStatus.GenericObject.message);
                }
            }
            catch (HologoInternetException ex)
            {
                Debug.Log(ex.Message);
                createMessage(ex.Message);
            }
            return success;
        }

        public List<followerDataClass> getPendingStudents()
        {
            //pendingStudentList.AddRange(acceptedStudentList);
            if(pendingStudentList.Count <=0 && acceptedStudentList.Count > 0)
            {
                return acceptedStudentList;
            }
            else if(pendingStudentList.Count > 0 && acceptedStudentList.Count <= 0)
            {
                return pendingStudentList;
            }

            pendingStudentList.Union(acceptedStudentList);
            return pendingStudentList;
        }




        // add a student
        public async Task<bool> addStudent(string email, string token)
        {
            bool success = false;
            string result;
            teacherServerConnect tsc = new teacherServerConnect();
            string url = dataPath.getUrl(0) + "?token=" + token;
            Debug.Log(url);
            var task = tsc.addTeacher(url, email);
            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoAPI> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);

                if (ApiAndStatus.GenericObject.success)
                {
                    Debug.Log("success");
                    createMessage(ApiAndStatus.GenericObject.message);
                    success = true;
                }
                else
                {
                    Debug.Log("false");
                    createMessage(ApiAndStatus.GenericObject.message);
                }
            }

            catch (HologoInternetException ex)
            {
                // Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }
        //{"success":true,"data":true,"message":"user connection deleted"}
        // remove a student
        // remove a teacher
        public async Task<bool> removeRejectStudent(int requestorId, string token)
        {
            bool success = false;
            string result;
            teacherServerConnect tsc = new teacherServerConnect();
            string url = dataPath.getUrl(2) + requestorId + "?token=" + token;
            Debug.Log(url);
            var task = tsc.removeTeacher(url);
            try
            {
                result = await task;
                Debug.Log(result);
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoAPI> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                if (ApiAndStatus.GenericObject.success)
                {
                    success = true;
                    removeFromList(requestorId);
                    createMessage(ApiAndStatus.GenericObject.message);
                }
                else
                {
                    success = false;
                    createMessage(ApiAndStatus.GenericObject.message);
                }
            }

            catch (HologoInternetException ex)
            {
                Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }



        void removeFromList(int requestorId)
        {
            for (int i = 0; i < acceptedStudentList.Count; i++)
            {
                if (acceptedStudentList[i].requester_id == requestorId)
                {
                    acceptedStudentList.Remove(acceptedStudentList[i]);
                    break;
                }
            }
        }

        //{"success":true,"data":true,"message":"accepted"}
        public async Task<bool> AcceptStudent(int teacherId, string token)
        {
            bool success = false;
            string result;
            teacherServerConnect tsc = new teacherServerConnect();
            string url = dataPath.getUrl(1) + teacherId + "?token=" + token;
            Debug.Log(url);
            var task = tsc.acceptTeacher(url);
            try
            {
                result = await task;
                Debug.Log(result);
                GenericObjectAndStatus<HologoAPI> ApiAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
                if(ApiAndStatus.GenericObject.success)
                {
                    success = true;
                    createMessage(ApiAndStatus.GenericObject.message);
                }
                else
                {
                    success = false;
                    createMessage(ApiAndStatus.GenericObject.message);
                }
                // serialzing data from server to object
                //TODO
                // deserialize the string to object and read it and apply it

            }

            catch (HologoInternetException ex)
            {
                Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }

    }
}