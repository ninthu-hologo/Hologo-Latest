using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 01 august 2019
    /// Modified by: 
    /// this handles teachers
    /// </summary>
    public class teachersManager : messageLogging
    {

        // paths and names
        [SerializeField]
        private dataPaths_SO dataPath;
        [SerializeField]
        private List<followerDataClass> pendingTeacherList;
        [SerializeField]
        private List<followerDataClass> acceptedTeacherList;

        [SerializeField]
        public bool hasTeachers = false; // Shariz 28th Oct 2019 2.00



        // get all the teachers belonging to the student
        public async Task<bool> getTeachersFromServer(string token,int userId)
        {
            bool success = false;
            string result;
            string url = dataPath.getUrl(4) + "?token=" + token;
            Debug.Log(url);
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
                    List <UserDetail> ud = ApiAndStatus.GenericObject.data;
                    convertUsersToFollowers(ud,userId);
                    success = true;
                    hasTeachers = true;// Shariz 28th Oct 2019 2.00
                }
                else
                {
                    //invalid credentials
                    acceptedTeacherList.Clear(); //Shariz 13th April 2020 v2.0.5 removing accepted teacher list after log out
                    pendingTeacherList.Clear(); //Shariz 13th April 2020 v2.0.5 removing pending teacher list after log out
                    createMessage(ApiAndStatus.GenericObject.message);
                    hasTeachers = false;// Shariz 28th Oct 2019 2.00
                    Debug.Log("This user has no teacher");
                }
            }

            catch (HologoInternetException ex)
            {
                Debug.Log(ex.ToString());
                createMessage(ex.Message);
            }

            return success;
        }

        public List<followerDataClass> getTeachers()
        {
           return acceptedTeacherList;
        }


        void convertUsersToFollowers(List<UserDetail> ud,int id)
        {
            acceptedTeacherList = new List<followerDataClass>();
            for (int i = 0; i < ud.Count; i++)
            {
                followerDataClass fdc = new followerDataClass();
                fdc.acceptor_id = id;
                fdc.requester_id = ud[i].id;
                fdc.requester = ud[i];
                fdc.status = true;
                acceptedTeacherList.Add(fdc);
            }
        }

        public async Task<bool> getTeachersPending(string token)
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
                if (ApiAndStatus.GenericObject.success)
                {
                    pendingTeacherList = ApiAndStatus.GenericObject.data;
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

        public List<followerDataClass> getPendingTeachers()
        {
            //pendingStudentList.AddRange(acceptedStudentList);
            if (pendingTeacherList.Count <= 0 && acceptedTeacherList.Count > 0)
            {
                return acceptedTeacherList;
            }
            else if (pendingTeacherList.Count > 0 && acceptedTeacherList.Count <= 0)
            {
                return pendingTeacherList;
            }
            pendingTeacherList.Union(acceptedTeacherList);
            return pendingTeacherList;
        }


        /*{"success":true,
        "data":
        {"requester_id":26114,
        "acceptor_id":26117,
        "status":false,
        "updated_at":"2019-09-08 10:56:20",
        "created_at":"2019-09-08 10:56:20","id":1}
        ,"message":"follow request has been sent"}

            {"success":false,"message":"invalid email address"}

            from soruce{"success":false,"message":"Both user already has connection"}
        // add a Teacher*/
        public async Task<bool> addTeacher(string email, string token)
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
                
                if(ApiAndStatus.GenericObject.success)
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

        // remove a teacher
        public async Task<bool> removeRejectTeacher(int requestorId, string token)
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
            for (int i = 0; i < acceptedTeacherList.Count; i++)
            {
                if(acceptedTeacherList[i].requester_id == requestorId)
                {
                    acceptedTeacherList.Remove(acceptedTeacherList[i]);
                    break;
                }
            }
        }

        public async Task<bool> AcceptTeacher(int teacherId, string token)
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
                if (ApiAndStatus.GenericObject.success)
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
