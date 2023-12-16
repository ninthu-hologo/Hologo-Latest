using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hologo2;
using System.Threading.Tasks;

public class userJsonTest : MonoBehaviour
{

    [SerializeField]
    private UserDetail userd;
    [SerializeField]
    private userData_SO userdetail;

    public string name;
    public string email;
    // Start is called before the first frame update
    void Start()
    {
        runthis();

        //GenericObjectAndStatus<HologoAPI> signupAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result.text);

        //if(signupAndStatus.Success)
        //{
        //    Debug.Log("success");
        //}

        //Debug.Log(result.text);
        //GenericObjectAndStatus<HologoWebAPIGeneric<UserDetail>> userdetailAndStatus =
        //                jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserDetail>>(result.text);

        //userd = userdetailAndStatus.GenericObject.data;
    }

    async void runthis()
    {
        await createAcc();
    }

    async Task createAcc()
    {
        serverUserConnect userConnect = new serverUserConnect();
        string path = "http://lite.hologo.world/api/register/";
        //  Debug.Log(dataPath.getUrl(1) +"*"+ userName + "*" +email + "*" +password + "*" +userType + "*" +languageId + "*" +countryId + "*" +device);
        var task = userConnect.createAccount(path, name, email, "123456789", "student", 6, 1, "ios", false, 0);

//        string datas= await task;
//        Debug.Log(datas);

////        Debug.Log(result.text);
//        GenericObjectAndStatus<HologoWebAPIGeneric<UserDetail>> userdetailAndStatus =
//                        jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserDetail>>(datas);
//        userd = new UserDetail();
//        userd = userdetailAndStatus.GenericObject.data;

        ////
        ///

        bool success = false;
        try
        {
            string result = await task;

            GenericObjectAndStatus<HologoAPI> signupAndStatus = jsonHelper.DeserializeFromJson<HologoAPI>(result);
            Debug.Log(result);

            GenericObjectAndStatus<HologoWebAPIGeneric<UserDetail>> userdetailAndStatus =
                    jsonHelper.DeserializeFromJson<HologoWebAPIGeneric<UserDetail>>(result);


            if (signupAndStatus.GenericObject.success)
            {

                userd = new UserDetail();

                userd = userdetailAndStatus.GenericObject.data;

               // userdetail.setUserAfterCreate(userd,"123456789");
                //saving userData;

                success = true;
            }
            else
            {
                
                Debug.Log("error");
            }
        }
        catch (HologoInternetException ex)
        {
            
        }
    }
   
}
