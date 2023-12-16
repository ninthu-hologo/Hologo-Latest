using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hologo2
{
    /// <summary>
    /// This class contains all the app environment variables
    /// </summary>
    public static class appEVars
    {
        #region FOLDERS
        public static string AppConfigFolder = "hcg";
        public static string libraryFolder = "hsp";
        public static string AssetBunldesFolder = "hab";
        public static string LessonsFolder = "hlss";

        #endregion

        #region FILENAMES
        public static string AppConfigFileName = "hcfg.dtm";
        public static string libraryDataFileName = "hsd.dtm";
        public static string StudentStoreProductsDataFileName = "hst.dtm";
        public static string TeacherStoreProductsDataFileName = "hts.dtm";
        public static string ShopSubjectDataFileName = "hsb.dtm";
        public static string SubscriptionsDataFilName = "hsu.dtm";
        public static string LessonDataFileName = "hlss.dtm";
        public static string AssetdlFileName = "habt.dtm";
        public static string RecordedLessonsFileName = "hlr.dtm";
        public static string UserFileName = "hud.dtm";
        public static string SubscriptionsFileName = "hsn.dtm";
        public static string StudentAddonFileName = "saf.dtm";
        public static string StudentsFileName = "suf.dtm";
        public static string LessonAddonFileName = "laf.dtm";
        public static string LogsTrackerFileName = "hlgal.dtm";
        #endregion

        #region DataURLS
         public static string DomainUrl = "https://lite.hologo.world/api";/* "http://hologo.space/api"; */  // "http://hologo.prodesigners.info/api";//"http://hologo.prodesigners.info/api";   //"https://hologo.world/api";
        // public static string DomainUrl = "https://hologo.space/api";   // "http://hologo.prodesigners.info/api";//"http://hologo.prodesigners.info/api";   //"https://hologo.world/api";
        //public static string DomainUrl = "https://beta-api.hologo.world/api";
        public static string StoreExperienceURL = DomainUrl + "/open/list/asset_experiences/";  //"/open/list/experiences/";
        public static string StorePacksURL = DomainUrl + "/open/list/experience_packs/";
        public static string SubjectDataURL = DomainUrl + "/open/list/subjects/";
        public static string PostProductBuyUrl = DomainUrl + "/my/order/create_v2/";    // "/my/order/create/";
        public static string GetMyAttachableSingleUrl = DomainUrl + "/my/single/experience/";
        public static string GetMyAttachablePackUrl = DomainUrl + "/my/single/experience/pack/";
        public static string SubscriptionsDataURL = "";
        //public static string LessonsDataURL = "http://thesyntex.com/getlesson.php";
        public static string InternetCheckUrl = "http://thesyntex.com/internetcheck.html";

        // this should be removed asap
        public static string FreeProductsUrl = DomainUrl + "/open/experience/asset_free/";  ///"/open/experience/free/";
        public static string FreeProductDownloadUrl = DomainUrl + "/open/attachables/download_free/";
        public static string ProductDownloadUrl = DomainUrl + "/my/attachables/stream/";

        public static string GetUserTypesUrl = DomainUrl + "/open/list/userTypes";
        public static string PostSignUpUrl = DomainUrl + "/signup";
        // user login url
        public static string PostAuthenticateUrl = DomainUrl + "/authenticate";
        public static string GetUserVerificationUrl = DomainUrl + "/open/verify/user/";
        public static string ResendTokenForVerification = DomainUrl + "/resendtoken/";


        public static string PostRecordedLessonUrl = DomainUrl + "/my/lessons/create/"; // not working
        public static string GetMyStudentsUrl = DomainUrl + "/my/promocodes/";
        public static string PostRedeemMyCode = DomainUrl + "/my/redeem/code/"; //Params "code"
        public static string GetStudentAddons = DomainUrl + "/open/list/subject_addon_coupons/";
        public static string GetLessonsAddons = DomainUrl + "/open/list/lesson_packages/";
        public static string GetMyExperiences = DomainUrl + "/my/experience/lists/";
        public static string GetMyPacks = DomainUrl + "/my/experience/packs/";
        public static string GetMySubscriptions = DomainUrl + "/my/subscription_packages/lists/";
        public static string GetTeacherExperiences = DomainUrl + "/my/experience/teachers_experiences/";
        public static string GetTeacherPacks = DomainUrl + "/my/packs/teachers_packs/";

        public static string GetSubscriptionsList = DomainUrl + "/open/list/subscription_packages";

        public static string GetMyTeachersUrl = DomainUrl + "/my/teachers/";

        public static string RemoveStudentUrl = DomainUrl + "/my/remove_student/";
        public static string GetMyCurrentSubscriptionDetailUrl = DomainUrl + "/my/subscription_packages/details/";

        // public static string uploadAudioUrl = "http://hologo.prodesigners.info/upload.php";

        public static string AudioDataGetOne = DomainUrl + "/my/experience/";
        public static string AudioDownloader = "https://hologo.world/api/my/attachables/download/"; //{0}/audios";
        public static string InternetStatusCheck = DomainUrl + "/open/status";
        public static string ServerMaintenance = DomainUrl + "/open/maintanace_check";
        public static string ForgotPassword = DomainUrl + "/email_new_password/";
        public static string ChangePassword = DomainUrl + "/my/reset_password/";
        public static string ProfileUpdate = DomainUrl + "/my/user_profiles/update/";
        public static string GetProfile = DomainUrl + "/my/users/profile/";

        public static string SubscriptionTransactionUrl = DomainUrl + "/my/order/online_transaction";

        public static string LogSendUrl = DomainUrl + "/my/trackers/user_log";

        #endregion

        #region DownloadURLS

        public static string ShopItemImagesDownloadLocation = DomainUrl + "/open/attachables/show/";
        public static string DownloadImageAddition = "?download=true&size=larg";
        // public static string LessonsDownloadLocation = "http://thesyntex.com/lessons/";
        #endregion


        #region Common Texts
        public static string StoreTitle = "Library";
        public static string MyExperienceTitle = "My Collection";
        #endregion

        #region Lesson Constants
        // audio lesson recording lenght
        public const int LessonRecordingLenght = 300;
        // audio lesson recording quality
        public const int LessonRecordQuality = 22000;
        #endregion

        #region StoreIdentifiers
        public const string AppleStoreId = "com.prodesigners.hologo.";
        public const string GooglePlayStoreId = "com.prodesigners.hologo.";
        #endregion



        public const int StudentTypeId = 2;
        public const int TeacherTypeId = 1;
        public const int AnonymouseTypeId = 3;
        public const int WebTimeOut = 15;
        public const int NumberOfLogs = 100;

        public const int UserNotVerifiedStatus = 0;
        public const int UserVerifiedStatus = 1;

        public const int AppVersion = 208; //Shariz 6th August 2020 v2.0.9
        //public const string normalUpdateDate = "2020-04-30";//Shariz 10th May 2020 v2.0.6
        public const string normalUpdateDate = "2019-12-05";//Shariz test value
        //public const string forcedUpdateDate = "2019-10-31";
        public const string forcedUpdateDate = "2020-08-21"; // Shariz 21st August 2020 v2.1.0
        public const int HighResImage = 700;
        public const int MediumResImage = 400;
        public const int LowResImage = 100;

    }
}
