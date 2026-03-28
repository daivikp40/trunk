using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
using DuplicatePhotosFixer.Helpers;
using static DuplicatePhotosFixer.Views.cNagDialogue;

namespace DuplicatePhotosFixer.Track
{
    public enum FreeTrialKeyStatus
    {
        ERROR = 0,
        SUCCESS = 1008,
        EMAIL_ID_EMPTY = 2,
        INVALID_KEY = 810,
        PRODUCTID_INVALID = 4, //PRODUCT ID IS EITHER EMPTY OR NOT INTEGER
        EMAIL_ALREADY_SEND = 811, //
        KEY_ALREADY_USED = 808, // YOU HAVE ALREADY USED THIS SERVICE, PLEASE UPGRADE YOUR PRODUCT
        EMAIL_CANNOT_BE_SENT = 809,
    }

    public enum FreeKeyAction
    {
        RequestKey,
        VerifyKey,
        UpdateCleanStatus,
    }

    public class FreeAppTrialProtection
    {

        //const int ProductId = 2374;

        /// <summary>
        /// Sample URL: http://keys.systweak.com/freekey/?action=requestkey&pid=2&emailid=jodhraj.kumawat@systweak.com&appversion=0&langcode=en&langid=0&macaddress=dsdfsdfsdfsdfsd&osid=1
        /// </summary>


        /*
         *  1. requestkey

            URL:  http://keys.systweak.com/freekey/?action=requestkey&pid=2&emailid=jodhraj.kumawat@systweak.com&appversion=0&langcode=en&langid=0&macaddress=dsdfsdfsdfsdfsd&osid=1

            Product will request for requesting a trial key. Here ProductId (pid), emailid are mandatory fields.
            Other fields are also required for report purpose.

            2. verifykey

            URL:  http://keys.systweak.com/freekey/?action=verifykey&pid=2&emailid=jodhraj.kumawat@systweak.com&appversion=0&langcode=en&langid=0&macaddress=dsdfsdfsdfsdfsd&osid=1&key=43355-16357-28212

            This is called for verifying key that is sent to customer in email. Here ProductId (pid), emailid & key are mandatory fields.Other fields are also required for report purpose. Email ID of customer is also verified at this action...


            3. updatecleanstatus
            URL:   http://keys.systweak.com/freekey/?action=updatecleanstatus&pid=2&emailid=jodhraj.kumawat@systweak.com&appversion=0&langcode=en&langid=0&macaddress=dsdfsdfsdfsdfsd&osid=1&key=43355-16357-28212&cleancount=355

            This is called to update product cleandone date. This can be called many times. It is used to track all clean histories. Here ProductId (pid), emailid & key are mandatory fields. Other fields are also required for report purpose.
         * 
         * */
        public static FreeTrialKeyStatus PeformAction(FreeKeyAction action, string email, string key = "", int cleanCount = 0)
        {

            WebClientEx webClientEx = null;

            try
            {
                string GetFreeKeyURL = "";
                string actionStr = "";
                switch (action)
                {
                    case FreeKeyAction.RequestKey:
                        GetFreeKeyURL = "http://keys.systweak.com/freekey/?action={0}&pid={1}&emailid={2}&appversion={3}&langcode={4}&langid={5}&macaddress={6}&osid={7}";
                        actionStr = "requestkey";
                        break;
                    case FreeKeyAction.VerifyKey:
                        GetFreeKeyURL = "http://keys.systweak.com/freekey/?action={0}&pid={1}&emailid={2}&appversion={3}&langcode={4}&langid={5}&macaddress={6}&osid={7}&key={8}";
                        actionStr = "verifykey";
                        break;
                    case FreeKeyAction.UpdateCleanStatus:
                        GetFreeKeyURL = "http://keys.systweak.com/freekey/?action={0}&pid={1}&emailid={2}&appversion={3}&langcode={4}&langid={5}&macaddress={6}&osid={7}&key={8}&cleancount={9}";
                        actionStr = "updatecleanstatus";
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                string actionURL = string.Format(GetFreeKeyURL,
                    actionStr,
                    cGlobal.setupType == eSetupType.NL_ONE_TIME_EMAIL_REGISTER ? cGlobalSettings.NLProductId : cGlobalSettings.ProductId,
                    email,
                    cGlobalSettings.GetApplicationVersion(),
                    cGlobalSettings.oScanSettings.LanguageCode,
                    (int)cGlobalSettings.oScanSettings.CurrentLangSettings.LangIdentifier,
                    AppFunctions.GetMacAddress(),
                    (int)cGlobalSettings.systemOS,
                    key,
                    cleanCount
                    );

                webClientEx = new WebClientEx();
                string output = webClientEx.DownloadString(actionURL);

                int nOut = 0;
                if (int.TryParse(output, out nOut) && Enum.IsDefined(typeof(FreeTrialKeyStatus), nOut))
                {
                    return (FreeTrialKeyStatus)nOut;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetActionURL", ex);
            }
            finally
            {
                webClientEx = null;
            }

            return FreeTrialKeyStatus.ERROR;
        }


        public static FreeTrialKeyStatus RequestKey(string email)
        {
            try
            {

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("RequestKey", ex);
            }

            return FreeTrialKeyStatus.ERROR;
        }


        /// <summary>
        /// --------------------------------------
        /// When success returned for VerifyKey
        /// --------------------------------------
        /// SET registry 
        /// email = ?
        /// key = ?
        /// --------------------------------------
        /// if receive the request to register with same combination, 
        /// if IsServerInformedForFreeCleaning = 1, then don't check locally
        /// --------------------------------------
        /// else IsServerInformedForFreeCleaning = 0
        /// and if we have the IsFreeCleanDone=1 and IsServerInformedForFreeCleaning = 0
        /// then show user a message box => You have used this service.
        /// --------------------------------------
        /// </summary>
        /// <param name="email"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool VerifyKey(string email, string key, ref string message)
        {
            bool bSuccess = false;
            try
            {
                /// get key status
                FreeTrialKeyStatus status = PeformAction(FreeKeyAction.VerifyKey, email, key);

                /// return message to show
                message = cResourceManager.LoadString("IDS_FREETRIALKEYSTATUS_" + status.ToString());
                switch (status)
                {
                    case FreeTrialKeyStatus.SUCCESS:
                        {
                            bSuccess = true;
                        }
                        break;
                    case FreeTrialKeyStatus.ERROR:
                        {
                            try
                            {
                                using (var client = new WebClient())
                                using (var stream = client.OpenRead("http://www.google.com"))
                                {
                                    ;
                                }
                            }
                            catch
                            {
                                message = cResourceManager.LoadString("IDS_ERROR_CONNECTING_INTERNET");
                            }
                        }
                        break;
                    default:
                        break;
                }

                return bSuccess;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("VerifyKey", ex);
            }

            return bSuccess;
        }



        public static bool UpdateCleanStatus(string email, string key, int cleanCount)
        {
            bool isServerInformed = false;
            try
            {
                FreeTrialKeyStatus status = PeformAction(FreeKeyAction.UpdateCleanStatus, email, key, cleanCount);
                switch (status)
                {
                    case FreeTrialKeyStatus.SUCCESS:
                        {
                            RegistryTool.IsServerInformedForFreeCleaning = 1;
                            isServerInformed = true;
                        }
                        break;
                    default:
                        break;
                }

                /// set registry key 
                /// IsServerInformedForFreeCleaning = 1
                /// IsFreeCleanDone = 1               

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("UpdateCleanStatus", ex);
            }
            return isServerInformed;
        }



        /*public static void OnTimeRegister(string emialAddress, string OneTimeKey)
        {
            / *string verifyingUri = string.Format(@"http://keys.systweak.com/freekey/?action=verifykey&pid={0}&emailid={1}&appversion={2}&langcode={3}&langid={4}&macaddress={5}&osid={6}&key={7}",
                 2374, emialAddress, cGlobalSettings.GetApplicationVersion(),
                 cGlobalSettings.oScanSettings.LanguageCode, cGlobalSettings.oScanSettings.LanguageID,
                 AppFunctions.GetMacAddress(), (int)cGlobalSettings.systemOS, OneTimeKey);* /

            WebRequest request = null;
            WebResponse response = null;

            try
            {
string verifyingUri = string.Format(@"http://keys.systweak.com/freekey/?action=verifykey&pid={0}&emailid={1}&appversion={2}&langcode={3}&langid={4}&macaddress={5}&osid={6}&key={7}",
                 2374, emialAddress, cGlobalSettings.GetApplicationVersion(),
                 cGlobalSettings.oScanSettings.LanguageCode, cGlobalSettings.oScanSettings.LanguageID,
                 AppFunctions.GetMacAddress(), (int)cGlobalSettings.systemOS, OneTimeKey);

                request = WebRequest.Create(verifyingUri);
                request.Proxy = WebProxy.GetDefaultProxy();
                // Define a cache policy for this request only. 
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //response = request.GetResponse().GetResponseStream();
                Console.WriteLine("IsFromCache? {0}", response.IsFromCache);
            }
            catch (Exception ex)
            {
                

            }



            //http://keys.systweak.com/freekey/?action=verifykey&pid=2&emailid=jodhraj.kumawat@systweak.com&appversion=0&langcode=en&langid=0&macaddress=dsdfsdfsdfsdfsd&osid=1&key=43355-16357-28212
        }*/


        public static bool ValidateFreeKey(string emailAddress, string FreeKey, out string message)
        {
            bool isSuccess = false;
            message = string.Empty;
            try
            {
                if (RegistryTool.IsServerInformedForFreeCleaning == 0 && string.Compare(cGlobalSettings.oRegSettings.GetEmailAddress().Trim(), emailAddress, true) == 0 && string.Compare(RegistryTool.FreeAppTrialKey.Trim(), FreeKey, true) == 0)
                {
                    // Restrict the user
                    RegistryTool.IsFreeCleanDone = 1;
                    message = cResourceManager.LoadString("IDS_FREETRIALKEYSTATUS_KEY_ALREADY_USED");// "You have already used the One time cleanup";
                }
                else
                {
                    isSuccess = FreeAppTrialProtection.VerifyKey(emailAddress, FreeKey, ref message);

                    if (isSuccess)
                    {
                        if (RegistryTool.IsServerInformedForFreeCleaning == 0 /*&& RegistryTool.IsFreeCleanDone == 1*/)
                        {
                            // Restrict the user
                            RegistryTool.IsFreeCleanDone = 1;
                            //message = "You have already used the One time cleanup";
                            message = cResourceManager.LoadString("IDS_FREETRIALKEYSTATUS_KEY_ALREADY_USED");
                        }
                        else
                        {
                            cGlobalSettings.oRegSettings.SetEmailAddress(emailAddress);
                            RegistryTool.FreeAppTrialKey = FreeKey;
                            RegistryTool.IsFreeCleanDone = 0;
                        }
                        // Set reg value to allow user to delete
                    }
                    else
                    {
                        // Restrict user 
                        RegistryTool.IsFreeCleanDone = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FreeAppTrialProtection:: ValidateFreeKey:", ex);
            }

            return isSuccess;
        }

    }
}
