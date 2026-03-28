using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net;
//using DuplicateFileFixer;
//using DuplicateFileFixer.STGenuineValidator;
using HardwareHash;
//using DuplicateFileFixer.Track;
using DuplicatePhotoFixer;
using DuplicatePhotosFixer.STGenuineValidator;
using DuplicatePhotosFixer.Track;
using DuplicatePhotosFixer;

/// <summary>
/// Online key verifier
/// </summary>
public class KVO
{
    public static IntPtr UIHandle = IntPtr.Zero;
    public static string MessageBoxCaption = "";
    const string strRegAppKey = "Software\\Systweak\\Duplicate Photos Fixer Pro\\";
    private static volatile bool bIsStopped = false;
    private static bool IsStopped() { return bIsStopped; }

    const string ACT_URL_1 = "http://updateservice1.systweak.com/STGenuineValidatorDPF/STGenuineValidationService.asmx";
    const string ACT_URL_2 = "http://updateservice1.systweak.com/STGenuineValidatorDPF/STGenuineValidationService.asmx";

    const int SUCCESS = 0;
    const int FAIL = 1;
    const int INCORRECT_PARAM = 2;
    const int DEFAULT = 3;
    const int CANCEL_VERIFICATION = 4;

    const int ARM_VAR_STORE_HARDWARE_HASH = 6;


    const int ONLINE_ACTIVATION_NEEDED = 1;								// is activation needed in current project
    static readonly int ACTIVATION_TAKE_ACTION_ON_BASE_RETURNED_FLAGS = 1;		// whether to take action returned by service in current project


    enum eCallFnOptions
    {
        //NoneFns = 0,
        //STIsSoftwareGenuines = 1,
        //STIsKeyGenuines = 2,
        STIsCurKeyNeedToExpires = 4,
        //STGetSoftwareCheckUpdateURLs = 8,
        //STCheckGenuinenesss = 16,
    };


    public static void CancelOperation()
    {
        bIsStopped = true;
    }

    public static bool STCheckProductGenuineness_(
        ref DuplicatePhotoFixer.cArmadilloHelper.ACTIVATION_DATA pActivationData,
        short appId,
        short langId,
        string userEmail,
        string userKey,
        string userMachineId,
        string strRegAppKey,
        string strAppDataFolder,
        int REGISTRATION_STATUS,
        bool m_bForceRefreshVerification,
        bool b_forceTakeAction
        )
    {

        try
        {

            // check if already called activation check today            
            if (!m_bForceRefreshVerification)
            {
                if (!(CFNCALL_POINT_TIME_CHECK.CanWeCallFn(strRegAppKey)))
                {
                    // return progress
                     Logger.WriteLine("Already Checked today, returning without going to service!!!");
                    return true;
                }
            }

            // check if cancel is called
            if (IsStopped()) { return true; }

            cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness_:: Calling: STCheckProductGenuineness__");
            bool bRet = STCheckProductGenuineness__(
                ref pActivationData,
                appId,
                langId,
                userEmail,
                userKey, 
                userMachineId,
                strRegAppKey,
                strAppDataFolder,
                REGISTRATION_STATUS,
                m_bForceRefreshVerification,
                b_forceTakeAction
            );

            if (bRet)
            {
                // I am setting this time, before checking for genuineness's, because, 
                // this is if called from launcher and 
                // user will start SPC, then each will launch and didn't find the registry and will starting 
                // checking for verification.
                // so 1st create registry to inform all, that , it has done, once today
                // do try it, after 6 hours if failed
                // set current time stamp for last function call
                CFNCALL_POINT_TIME_CHECK.SetCurrentFnCallTimeStamp(strRegAppKey);

                // return progress
                //Trace.WriteLine("Setting Current TimeSpamp");
            }
        }
        catch (Exception ex)
        {
            Logger.WriteException("STCheckProductGenuineness_", ex);
        }

        return true;
    }

    public static string GetApplicationVersion()
    {
        try
        {
            return ((AssemblyFileVersionAttribute)System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true)[0]).Version;
        }
        catch (Exception ex)
        {
            Logger.WriteException("GetApplicationVersion", ex);
        }

        return "";
    }

    public static bool STCheckProductGenuineness__(
        ref DuplicatePhotoFixer.cArmadilloHelper.ACTIVATION_DATA pActivationData,
        short appId,
        short langId,
        string userEmail,
        string userKey,
        string userMachineId,
        string strRegAppKey,
        string strAppDataFolder,
        int REGISTRATION_STATUS,
        bool m_bForceRefreshVerification,
        bool b_forceTakeAction
        )
    {
        ServiceInput srvInput = null;
        STGenuineValidationService srv = null;
        ServiceOutput so = null;

        bool b_takeAction = b_forceTakeAction;

        bool b_isLastLicStatusChangedFromCurrent = false;

        try
        {
            uint daysRemaining = 0;
            //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: GetARMStatus - Start");
            KeyValidator.SYS_TRIAL_VERSION_STATUS status = KeyValidator.GetARMStatus(ref daysRemaining);
            //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: GetARMStatus - Done.\r\nStatus={0}\t daysRemaining={1}", status, daysRemaining);

            string strRegisteredKey = "", m_sUSER_HASH = "";
            srvInput = new ServiceInput();
            srvInput.InstalledApplicationInfo = new Install_App_Info();

            srvInput.InstalledApplicationInfo.app_identifier = appId; // set it
            srvInput.InstalledApplicationInfo.user_license_key = userKey;//KeyValidator.GetLastRegKey();
            srvInput.InstalledApplicationInfo.user_machine_id = DuplicatePhotosFixer.HardwareHash.GetHardwareHash();
            srvInput.InstalledApplicationInfo.cur_program_version = GetApplicationVersion(); // set file version
            srvInput.InstalledApplicationInfo.lang_code = "en";	// not used till now, as we are more concerned with LanguageID

            srvInput.CallFns = 0;

            // new params added
            // self calculated values
            srvInput.InstalledApplicationInfo.REGISTRATION_STATUS = (int)REGISTRATION_STATUS;
            srvInput.InstalledApplicationInfo.IsRetailKey = (short)(KeyValidator.ST_IsRetailKey() ? 1 : 0);
            srvInput.InstalledApplicationInfo.KeyInstalledDays = (short)KeyValidator.ST_GetInstalledDays();
            srvInput.InstalledApplicationInfo.key_validdays = 0;
            srvInput.InstalledApplicationInfo.REGISTRATION_STATUS =
                (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                ? KeyValidator.GetKeyValidDays()
                : KeyValidator.ST_GetTotalAllowedTrialPeriod2();


            // read assigned values
            srvInput.InstalledApplicationInfo.lang_id = langId;
            srvInput.InstalledApplicationInfo.nRequestFlags = 0;
            srvInput.InstalledApplicationInfo.user_activation_code = "";
            srvInput.InstalledApplicationInfo.user_email_addr = userEmail;



            string output = "";

            /// ===============================
            /// Get output from service.
            /// ===============================
            try
            {
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Get output from service.");

                srv = new STGenuineValidationService();
                srv.Url = ACT_URL_1;
                srvInput.CallFns = (int)eCallFnOptions.STIsCurKeyNeedToExpires;
                output = srv.STCheckGenuineness(srvInput);

                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Get output from service. output={0}", output);
            }
            catch (Exception ex)
            {
                Logger.WriteException("STCheckProductGenuineness__-failed-call-1", ex);

                cGlobalSettings.oLogger.WriteLogException("STCheckProductGenuineness__-failed-call-1", ex);

                try
                {
                    // try calling alternate URL
                    // currently not placing the check for Time Limit exceeded or not
                    srv.Url = ACT_URL_2;
                    output = srv.STCheckGenuineness(srvInput);

                    //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: try calling alternate URL");
                }
                catch (Exception ex1)
                {
                    Logger.WriteException("STCheckProductGenuineness__-failed-call-2", ex1);
                    cGlobalSettings.oLogger.WriteLogException("STCheckProductGenuineness__-failed-call-2", ex);
                    return false;
                }

            }
            finally
            {
                srv = null;
            }


            long svd = 0;
            
            try
            {
                if (string.IsNullOrEmpty(output))
                {
                    //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: output from service is Null Or Empty.");
                    return false;
                }

                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Deserialize service output.");
                object obj = SerializeDeserialize.DeserializeObject<ServiceOutput>(output);
                if (obj == null) return false;
                so = (ServiceOutput)obj;
                svd = so.Kes_info.Svd;
            }
            catch (Exception ex)
            {
                Logger.WriteException("STCheckProductGenuineness__-Deserialize", ex);
                cGlobalSettings.oLogger.WriteLogException("STCheckProductGenuineness__-Deserialize", ex);
            }
            

            string strLicFileSavePath = null;
            string strLastStatusFilePath = null;
            /// ===============================
            /// writing the xml
            /// ===============================
            try
            {

                strLicFileSavePath = Path.Combine(strAppDataFolder, ("status.xml"));
                strLastStatusFilePath = Path.Combine(strAppDataFolder, ("laststatus.xml"));

                // delete it if already exists
                if (File.Exists(strLicFileSavePath))
                {
                    // return progress
                    Logger.WriteLine("Old status file exist, renaming it to laststatus.lic");
                    //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Old status file exist, renaming it to laststatus.lic");

                    File.Copy(strLicFileSavePath, strLastStatusFilePath, true);
                    File.Delete(strLicFileSavePath);
                }

                // overwrite if any old file already exist
                File.WriteAllText(strLicFileSavePath, output);

                // return progress
                Logger.WriteLine("Saving current status to status.lic");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Saving current status to status.lic");

            }
            catch (Exception ex)
            {
                Logger.WriteException("STCheckProductGenuineness__- Save lic file", ex);
            }

            //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Check for valid Server");
            bool bIsValidServer = IsValidServer(svd, strRegisteredKey, m_sUSER_HASH);
            //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Check for valid Server Status={0}", bIsValidServer ? "True" : "False");

            // check if cancel is called
            if (IsStopped()) { return true; }

            // if already told not to take action, 
            // don't check previous status, 
            // as we don't have to perform anything
            // if force refresh is told, then perform action, either it is same 
            // then previous
            //if (b_takeAction && !m_bForceRefreshVerification)
            if (!b_forceTakeAction)
            {
                // return progress
                Logger.WriteLine("Force refresh is not specified, so comparing old and latest web results !!!");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Force refresh is not specified, so comparing old and latest web results !!!");

                // check if previous status returned is same as current, 
                // then do not preform any action just return
                b_takeAction = b_isLastLicStatusChangedFromCurrent = IsLicenceStatusFileChanged(strLicFileSavePath, strLastStatusFilePath);

                // return progress
                Logger.WriteLine(b_takeAction ? ("CompareResults-TakeActionAsNewStatusIsReturned") : ("CompareResults-Don'tTakeActionOldNewBothResultsAreSame"));
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__::{0}", b_takeAction ? ("CompareResults-TakeActionAsNewStatusIsReturned") : ("CompareResults-Don'tTakeActionOldNewBothResultsAreSame"));
            }

            if (m_bForceRefreshVerification)
            {
                b_takeAction = true;

                // return progress
                Logger.WriteLine("Force refresh is specified, setting force fully take action !!!");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Force refresh is specified, setting force fully take action !!!");
            }


            if (!File.Exists(strLicFileSavePath))
            {
                // return progress
                Logger.WriteLine("Status.lic not exist, Failed, Returned !!!");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Status.lic not exist, Failed, Returned !!!");

                return false;
            }

            if (so != null && so.Kes_info != null)
            {
                /// assign value
                pActivationData.KeyStatus = so.Kes_info.Kat;// _ttoi(stINI.getKeyValue(CString(enum_ini_options_Names[kat]).Trim(), _T("kes_info")));
                pActivationData.kti = so.Kes_info.Kti;// _ttoi(stINI.getKeyValue(CString(enum_ini_options_Names[kti]).Trim(), _T("kes_info")));
                pActivationData.rid = so.Kes_info.Rid;//// _ttoi(stINI.getKeyValue(CString(enum_ini_options_Names[rid]).Trim(), _T("kes_info")));
                pActivationData.msgid = so.Kes_info.Msgid;//_ttoi(stINI.getKeyValue(CString(enum_ini_options_Names[msgid]).Trim(), _T("kes_info")));                    
            }
            

            // check if cancel is called
            if (IsStopped()) { return true; }


            if (b_takeAction && (ACTIVATION_TAKE_ACTION_ON_BASE_RETURNED_FLAGS == 1))
            {
                // return progress
                Logger.WriteLine("Taking action as specified in service output - Started !!!");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Taking action as specified in service output - Started !!!");

                // execute action on base of decided for client and key
                bool bSuccess = PerformActionForClientAndKey(
                     appId,
                     langId,
                     userEmail,
                     userMachineId,
                     strRegAppKey,
                     strAppDataFolder,
                     REGISTRATION_STATUS,
                     m_bForceRefreshVerification,
                     b_forceTakeAction,
                     b_isLastLicStatusChangedFromCurrent,
                     so,
                     status
                    );


                // return progress
                Logger.WriteLine("{0} - Taking action as specified in service output - Ended !!!", bSuccess ? "Successful" : "Failed");
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: {0} - Taking action as specified in service output - Ended !!!", bSuccess ? "Successful" : "Failed");

            }
            else
            {
                // return progress
                //cGlobalSettings.oLogger.WriteLogVerbose("STCheckProductGenuineness__:: Returning as don't take action specified !!!");

                Logger.WriteLine("Returning as don't take action specified !!!");
            }


        }
        catch (Exception ex)
        {
            Logger.WriteException("STCheckProductGenuineness__", ex);
        }
        finally
        {
            srv = null;
            srvInput = null;
            so = null;
        }
        return false;
    }

    static bool PerformActionForClientAndKey
        (short appId,
        short langId,
        string userEmail,
        string userMachineId,
        string strRegAppKey,
        string strAppDataFolder,
        int REGISTRATION_STATUS,
        bool m_bForceRefreshVerification,
        bool b_forceTakeAction,
        bool b_isLastLicStatusChangedFromCurrent,
        ServiceOutput so,
        KeyValidator.SYS_TRIAL_VERSION_STATUS status
        )
    {

        if (ACTIVATION_TAKE_ACTION_ON_BASE_RETURNED_FLAGS == 0)
        {
            return true;
        }

        //int keyActionToTake = (int)eKeyActionToTake.KEY_EVERYTHING_OK;
        //ACTION_DATA actionData;


        // check if cancel is called
        if (IsStopped() == true) { return true; }


        // clear old values
        //actionData.Init();

        // check if key section exist, then perform actions provided in int
        if (so != null && so.Kes_info != null)
        {
            // return progress
            Logger.WriteLine("===========================");
            Logger.WriteLine("Reading key data - start... ");

            
            // assign value to return the status
            //pActivationData.KeyStatus = so.Kes_info.Kat;

            // check if cancel is called
            if (IsStopped() == true) { return true; }

            // check whether to take action or not
            bool b_takeAction = (b_isLastLicStatusChangedFromCurrent || (so.Kes_info.Ifea == 1) || m_bForceRefreshVerification);

            if (b_takeAction)
            {
                // return progress
                Logger.WriteLine("Performing action for key status ... ");

                // execute the action sent by service			
                bool bSuccess = PerformAction(appId,
                     langId,
                     userEmail,
                     userMachineId,
                     strRegAppKey,
                     strAppDataFolder,
                     REGISTRATION_STATUS,
                     m_bForceRefreshVerification,
                     b_forceTakeAction,
                     b_isLastLicStatusChangedFromCurrent,
                     so,
                     status
                     );

                // return progress
                Logger.WriteLine("Entering to Install New Key If Requested ... ");

                // Install key if requested through activation code or email address
                // TODO InstallKeyIfGetNewKeyRequested(pActivationData, &actionData, pCallbackFunc);
            }

            // return progress
            Logger.WriteLine("Reading key data - end... ");
            Logger.WriteLine("===========================");

        }


        return false;

    }

    //static bool InstallKeyIfGetNewKeyRequested(short appId,
    //    short langId,
    //    string userEmail,
    //    string userMachineId,
    //    string strRegAppKey,
    //    string strAppDataFolder,
    //    int REGISTRATION_STATUS,
    //    bool m_bForceRefreshVerification,
    //    bool b_forceTakeAction,
    //    bool b_isLastLicStatusChangedFromCurrent,
    //    ServiceOutput so,
    //    KeyValidator.SYS_TRIAL_VERSION_STATUS status)
    //{


    //    HRESULT hr = S_OK;
    //    switch (0)
    //    {
    //    case RETURN_DEFAULT_STATUS:	
    //    case RETURN_CLIENT_ACTION_ONLY:		
    //    case RETURN_KEY_ACTION_ONLY:
    //        return S_OK;
    //    case RETURN_NEW_KEY:
    //        // still don't have any work
    //        return S_OK;
    //    case GET_AND_INSTALL_A_NEW_KEY:
    //        // still don't have any work
    //        //return S_OK;
    //        // get new key on base of input params
    //    case INSTALL_A_NEW_KEY_ON_BASE_OF_ACTIVATION_CODE:
    //    case INSTALL_A_NEW_KEY_ON_BASE_OF_EMAILADDR:
    //        InstallANewKey(pActivationData, pActionData, pCallbackFunc);
    //        break;
    //    default:
    //        break;
    //    }

    //    return S_OK;
    //}


    static bool PerformAction(short appId,
        short langId,
        string userEmail,
        string userMachineId,
        string strRegAppKey,
        string strAppDataFolder,
        int REGISTRATION_STATUS,
        bool m_bForceRefreshVerification,
        bool b_forceTakeAction,
        bool b_isLastLicStatusChangedFromCurrent,
        ServiceOutput so,
        KeyValidator.SYS_TRIAL_VERSION_STATUS status
        )
    {
        try
        {


            if (so == null)
            {
                return false;
            }

            //////////////////////////////////////////////////////////////////////////
            // take action
            //
            switch ((eKeyActionToTake)so.Kes_info.Kat)
            {
                case eKeyActionToTake.KEY_RESERVED:
                case eKeyActionToTake.KEY_EVERYTHING_OK:
                case eKeyActionToTake.KEY_GOOD_CONTINUE:
                    ;
                    break;
                case eKeyActionToTake.KEY_BAD_EXPIRE_IT:
                case eKeyActionToTake.KEY_GOOD_EXPIRE_IT:
                    {
                        if (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                        {
                            try
                            {
                                string reason = string.Format("(eKeyActionToTake)so.Kes_info.Kat:{0}", (eKeyActionToTake)so.Kes_info.Kat);
                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("ActivateOnline:: ", ex);
                            }
                            // return progress
                            Logger.WriteLine("Expiring current key ... ");                            

                            // expire current key
                            KeyValidator.ExpireCurrentKey();

                            // return progress
                            Logger.WriteLine("Expired successfully ... ");
                        }


                    }
                    break;
                case eKeyActionToTake.KEY_BAD_DO_NOTHING:
                    ;
                    break;
                // show message cases			
                case eKeyActionToTake.KEY_BAD_ONLY_SHOW_MSG:
                case eKeyActionToTake.KEY_GOOD_ONLY_SHOW_MSG:
                    if (so.Kes_info.Msg != null && UIHandle != IntPtr.Zero)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            new WindowWrapper(UIHandle),
                            so.Kes_info.Msg,
                            MessageBoxCaption
                            );
                    }
                    break;
                case eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG:
                case eKeyActionToTake.KEY_GOOD_EXPIRE_IT_SHOW_MSG:
                    {
                        if (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                        {
                            try
                            {
                                string reason = string.Format("(eKeyActionToTake)so.Kes_info.Kat:{0}", (eKeyActionToTake)so.Kes_info.Kat);
                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("PerformAction:: ", ex);
                            }
                            // return progress
                            Logger.WriteLine("Expiring current key ... ");

                            // expire current key
                            KeyValidator.ExpireCurrentKey();

                            // return progress
                            Logger.WriteLine("Expired successfully ... ");
                        }

                        if (so.Kes_info.Msg != null && UIHandle != IntPtr.Zero)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                new WindowWrapper(UIHandle),
                                so.Kes_info.Msg,
                                MessageBoxCaption
                                );
                        }
                    }
                    break;
                case eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                case eKeyActionToTake.KEY_GOOD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                    {
                        if (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                        {
                            try
                            {
                                string reason = string.Format("(eKeyActionToTake)so.Kes_info.Kat:{0}", (eKeyActionToTake)so.Kes_info.Kat);
                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("PerformAction-KEY_GOOD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:: ", ex);
                            }
                            // return progress
                            Logger.WriteLine("Expiring current key ... ");

                            // expire current key
                            KeyValidator.ExpireCurrentKey();

                            // return progress
                            Logger.WriteLine("Expired successfully ... ");
                        }

                        if (so.Kes_info.Msg != null && UIHandle != IntPtr.Zero)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                new WindowWrapper(UIHandle),
                                so.Kes_info.Msg,
                                MessageBoxCaption
                                );
                        }

                        if (!string.IsNullOrEmpty(so.Kes_info.Url))
                            System.Diagnostics.Process.Start(so.Kes_info.Url);

                    }
                    break;

                // open browser cases	
                case eKeyActionToTake.KEY_BAD_ONLY_OPEN_BROWSER:
                case eKeyActionToTake.KEY_GOOD_ONLY_OPEN_BROWSER:
                    {
                        if (!string.IsNullOrEmpty(so.Kes_info.Url))
                            System.Diagnostics.Process.Start(so.Kes_info.Url);
                    }
                    break;
                case eKeyActionToTake.KEY_BAD_EXPIRE_IT_OPEN_BROWSER:
                case eKeyActionToTake.KEY_GOOD_EXPIRE_IT_OPEN_BROWSER:
                    {
                        if (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                        {
                            try
                            {
                                string reason = string.Format("(eKeyActionToTake)so.Kes_info.Kat:{0}", (eKeyActionToTake)so.Kes_info.Kat);
                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("PerformAction-KEY_GOOD_EXPIRE_IT_OPEN_BROWSER:: ", ex);
                            }
                            // return progress
                            Logger.WriteLine("Expiring current key ... ");

                            // expire current key
                            KeyValidator.ExpireCurrentKey();

                            // return progress
                            Logger.WriteLine("Expired successfully ... ");
                        }

                        if (!string.IsNullOrEmpty(so.Kes_info.Url))
                            System.Diagnostics.Process.Start(so.Kes_info.Url);
                    }
                    break;

                // fire and download exe WITH PARAMS AS PROVIDED BY WEBSERVICE	
                case eKeyActionToTake.KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                case eKeyActionToTake.KEY_GOOD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    {
                        string path = Path.Combine(KeyValidator.getAppDataApplicationPath(), Path.GetTempFileName());

                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(so.Kes_info.Url, path);
                        }

                        if (File.Exists(path))
                        {
                            try
                            {
                                Process pAppInstall = new Process();
                                pAppInstall.StartInfo.FileName = path;
                                pAppInstall.StartInfo.Arguments = so.Kes_info.Cmd_args;
                                pAppInstall.StartInfo.UseShellExecute = true;
                                pAppInstall.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                if (Environment.OSVersion.Version.Major > 5) pAppInstall.StartInfo.Verb = "runas";
                                pAppInstall.Start();
                                pAppInstall.Dispose();
                                pAppInstall = null;
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteException("Error-", ex);
                            }

                        }


                    }
                    break;
                case eKeyActionToTake.KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                case eKeyActionToTake.KEY_GOOD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    {
                        if (status == KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING)
                        {
                            try
                            {
                                string reason = string.Format("(eKeyActionToTake)so.Kes_info.Kat:{0}", (eKeyActionToTake)so.Kes_info.Kat);
                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("PerformAction-KEY_GOOD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:: ", ex);
                            }
                            // return progress
                            Logger.WriteLine("Expiring current key ... ");

                            // expire current key
                            KeyValidator.ExpireCurrentKey();

                            // return progress
                            Logger.WriteLine("Expired successfully ... ");
                        }

                        string path = Path.Combine(KeyValidator.getAppDataApplicationPath(), Path.GetTempFileName());

                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(so.Kes_info.Url, path);
                        }

                        if (File.Exists(path))
                        {
                            try
                            {
                                Process pAppInstall = new Process();
                                pAppInstall.StartInfo.FileName = path;
                                pAppInstall.StartInfo.Arguments = so.Kes_info.Cmd_args;
                                pAppInstall.StartInfo.UseShellExecute = true;
                                pAppInstall.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                if (Environment.OSVersion.Version.Major > 5) pAppInstall.StartInfo.Verb = "runas";
                                pAppInstall.Start();
                                pAppInstall.Dispose();
                                pAppInstall = null;
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteException("Error-", ex);
                            }

                        }
                    }
                    break;

                // new key case
                case eKeyActionToTake.INSTALL_A_NEW_KEY:
                    if (string.IsNullOrEmpty(so.Kes_info.Nk)) return false;
                    KeyValidator.InstallArmadilloCode(so.Kes_info.Nk);
                    break;
                case eKeyActionToTake.SET_DEFAULT_KEY:
                    if (string.IsNullOrEmpty(so.Kes_info.Nk)) return false;
                    KeyValidator.SetDefaultArmadilloKey();
                    break;
                case eKeyActionToTake.GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY:
                    if (string.IsNullOrEmpty(so.Kes_info.Nk)) return false;
                    KeyValidator.InstallArmadilloCode(so.Kes_info.Nk);
                    break;
                case eKeyActionToTake.KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION:
                    // do nothing here
                    // just do not register here
                    break;
                //////////////////////////////////////////////////////////////////////////
                // still their behavior is not defined
                // This could be done on base of case 11, 12, 13        
                case eKeyActionToTake.KEY_BAD_MAKE_IT_GOOD:
                // In case show campaign information to the users who are going to expire        
                case eKeyActionToTake.KEY_GOOD_DO_WE_HAVE_TO_SHOW_REMINDER:
                    return true;
                //break;
                //////////////////////////////////////////////////////////////////////////
                default:
                    Debug.Assert(false);
                    return false;
                //break;
            }
        }
        catch (Exception ex)
        {
            Logger.WriteException("PerformAction", ex);
        }

        return false;
    }



    static bool IsLicenceStatusFileChanged(string strCurFilePath, string strOldFilePath)
    {
        bool bIsContentChanged = false;

        try
        {
            if (!File.Exists(strOldFilePath))
            {
                bIsContentChanged = true;
                return bIsContentChanged;
            }

            if (File.Exists(strCurFilePath) && File.Exists(strOldFilePath))
            {
                ServiceOutput CurINI = (ServiceOutput)SerializeDeserialize.DeserializeObject<ServiceOutput>(File.ReadAllText(strCurFilePath));
                ServiceOutput LastStatusINI = (ServiceOutput)SerializeDeserialize.DeserializeObject<ServiceOutput>(File.ReadAllText(strOldFilePath));

                // checking client section
                bIsContentChanged = IsIniFileContentChanged(CurINI.Ckes_info, LastStatusINI.Ckes_info, typeof(Ckes_info)/*CLIENT_INI_SECTION*/);

                if (!bIsContentChanged)
                {
                    // checking for key section
                    bIsContentChanged = IsIniFileContentChanged(CurINI.Kes_info, LastStatusINI.Kes_info, typeof(Kes_info)/*KEY_INI_SECTION*/);
                }

            }
        }
        catch (Exception ex)
        {
            Logger.WriteException("IsLicenceStatusFileChanged", ex);
        }

        return bIsContentChanged;
    }

    static bool IsIniFileContentChanged(object CurINI, object LastStatusINI, Type typeofClass)
    {
        bool bIsContentChanged = false;
        PropertyInfo[] properties = null;
        try
        {
            properties = typeofClass/*typeof(Ckes_info)*/.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // do not consider the value of ifea IsForceExecute variable
                // case if set some function to execute forcefully
                // and it has run on user PC
                // but now if set it to zero
                // it matches it values
                // and old found 1 and new 0
                // both conflict resulting in executing it one more time
                if (string.Compare(property.Name, "ifea", true) == 0)
                {
                    continue;
                }

                if (property.GetValue(CurINI, null) != property.GetValue(LastStatusINI, null))
                {
                    bIsContentChanged = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.WriteException("IsIniFileContentChanged", ex);
        }
        finally
        {
            properties = null;
        }

        return bIsContentChanged;
    }

    public static long GetHash(string inputStr)
    {
        inputStr = inputStr.ToLower().Trim();
        byte[] inputStrBytes = System.Text.Encoding.Default.GetBytes(inputStr);
        return (long)CRC64.CRC_64.Get_CRC64(inputStrBytes);//CRC64.Crc64.Compute(0, inputStrBytes);
    }

    // Server Verification Code
    static bool IsValidServer(long svd, string strKey, string strHardwareHash)
    {
        try
        {
            long lVerificationCode = GetHash(strKey) + GetHash(strHardwareHash);

            return (svd == lVerificationCode);
        }
        catch (System.Exception ex)
        {
            Logger.WriteException("IsValidServer", ex);
        }

        // some what soft treatment
        return true;
    }



    public static bool CheckAndRemovePrependedKey(ref string key)
    {
        try
        {
            key = key.ToUpper();

            if (key.StartsWith("PB-") && key.Length > 3)
            {
                key = key.Substring(3);
            }
            else if (key.StartsWith("ASP-") && key.Length > 4)
            {
                key = key.Substring(4);
            }
            else if (key.StartsWith("ASO-") && key.Length > 4)
            {
                key = key.Substring(4);
            }
        }
        catch (System.Exception ex)
        {
            Logger.WriteException("CheckAndRemovePrependedKey", ex);
        }


        return false;
    }

    public enum ReportExpireCurrentKeyReasonType
    {
        OnRegister =1, 
        PatchRun,
        StoreProductExpiryInfo,
        OnLaunchHasKeyButFoundExpired,
    }

    public static bool bReportExpireCurrentKeyReason = false;
    public static void ReportExpireCurrentKeyReason(int productid, string reason, ReportExpireCurrentKeyReasonType runType = ReportExpireCurrentKeyReasonType.StoreProductExpiryInfo)
    {        
        WebClientEx webClientEx = null;
        try
        {
            if (!bReportExpireCurrentKeyReason)
                return;

            string strInstallDate = KeyValidator.GetUserString(KeyValidator.USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE);
            string strUserKey = KeyValidator.GetUserString(KeyValidator.USERSTRINGNUMBERS.USN_REG_USER_KEY);

            string url = string.Format("https://www.systweak.com/actissue.aspx?productid={0}&productname={1}&reason={2}&trktype={3}&userkey={4}&keyinstalldt={5}&pcdate={6}&lb={7}", 
                productid, KeyValidator.StrProductName, reason, ReportExpireCurrentKeyReasonType.StoreProductExpiryInfo, strUserKey, strInstallDate, DateTime.UtcNow.Ticks, cGlobal.IS_LB_BUILD);
            url = cGlobal.CombineUrl(url);
            webClientEx = new WebClientEx();
            string output = webClientEx.DownloadString(url);
        }
        catch (Exception ex)
        {
            cGlobalSettings.oLogger.WriteLogException("ReportExpireCurrentKeyReason", ex);
        }
        finally
        {
            webClientEx = null;
        }
    }
}


public enum eKeyActionToTake
{

    /////////
    //THE THREE VALUES BELOW ARE SAME
    //BUT ARE USED JUST TO COVER SLOPPY PROGRAMMING 
    KEY_RESERVED = -1,
    KEY_EVERYTHING_OK = 0,
    KEY_GOOD_CONTINUE = 1,
    ////////////////////////////////


    KEY_BAD_EXPIRE_IT = 2,
    KEY_BAD_DO_NOTHING = 3,

    // show message cases
    KEY_BAD_ONLY_SHOW_MSG = 4,
    KEY_BAD_EXPIRE_IT_SHOW_MSG = 5,
    KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER = 6,

    // open browser cases
    KEY_BAD_ONLY_OPEN_BROWSER = 7,
    KEY_BAD_EXPIRE_IT_OPEN_BROWSER = 8,

    // fire and download exe WITH PARAMS AS PROVIDED BY WEBSERVICE
    KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT = 9,
    KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT = 10,

    // new key actions
    INSTALL_A_NEW_KEY = 11,
    SET_DEFAULT_KEY = 12,
    GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY = 13,
    // ....

    EXTEND_KEY_EXPIRY_DAYS = 14,

    KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION = 15, /* Key is bad do not allow registration with this key */


    // In case if key         
    KEY_GOOD_EXPIRE_IT = 16,

    // Campaign Cases        
    KEY_GOOD_ONLY_SHOW_MSG = 17,
    KEY_GOOD_EXPIRE_IT_SHOW_MSG = 18,
    KEY_GOOD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER = 19,

    // open browser cases        
    KEY_GOOD_ONLY_OPEN_BROWSER = 20,
    KEY_GOOD_EXPIRE_IT_OPEN_BROWSER = 21,

    // fire and download exe WITH PARAMS AS PROVIDED BY WEBSERVICE        
    KEY_GOOD_DOWNLOAD_EXE_AND_EXEC_SILENT = 22,
    KEY_GOOD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT = 23,

    // This could be done on base of case 11, 12, 13        
    KEY_BAD_MAKE_IT_GOOD = 24,

    // In case show campaign information to the users who are going to expire        
    KEY_GOOD_DO_WE_HAVE_TO_SHOW_REMINDER = 25,
};


public class WindowWrapper : System.Windows.Forms.IWin32Window
{
    public WindowWrapper(IntPtr handle)
    {
        _hwnd = handle;
    }

    public IntPtr Handle
    {
        get { return _hwnd; }
    }

    private IntPtr _hwnd;
}




public static class StringExtensions
{
    public static string Left(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        maxLength = Math.Abs(maxLength);

        return (value.Length <= maxLength
               ? value
               : value.Substring(0, maxLength)
               );
    }
}

public class CFNCALL_POINT_TIME_CHECK
{
    const string ValueName = "LastGVTime1";
    const int RECHECK_TIME_PERIOD = 24 * 7;

    static string GetTimeStampKeyPath(string strAppKey)
    {
        return System.IO.Path.Combine(strAppKey, "LAST_CALL_TIME");
    }

    public static bool SetCurrentFnCallTimeStamp(string strAppKey)
    {
        RegistryKey reg = null;

        try
        {
            try
            {
                strAppKey = GetTimeStampKeyPath(strAppKey);
                reg = Registry.CurrentUser.OpenSubKey(strAppKey, true);
                if (reg == null) return false;
                reg.SetValue(ValueName, BitConverter.GetBytes(DateTime.Now.ToBinary()), RegistryValueKind.Binary);
                return true;
            }
            finally
            {
                if (reg != null) reg.Close();
                reg = null;
            }

        }
        catch (Exception)
        {
            ;
        }
        finally
        {
            reg = null;
        }


        return false;
    }



    public static bool GetLassFnCallTimeStamp(ref DateTime scantime, string strAppKey)
    {
        RegistryKey reg = null;
        byte[] dateBytes = null;

        try
        {
            try
            {
                strAppKey = GetTimeStampKeyPath(strAppKey);
                reg = Registry.CurrentUser.OpenSubKey(strAppKey);
                if (reg == null) return false;
                object srvLastCallTime = reg.GetValue(ValueName, null);
                if (srvLastCallTime == null) return false;
                dateBytes = (byte[])srvLastCallTime;
                if (dateBytes == null) return false;
                long dateArrayLongValue = BitConverter.ToInt64(dateBytes, 0);
                DateTime dtLastCallTime = DateTime.FromBinary(dateArrayLongValue);
                if (TimeZoneInfo.Local.IsInvalidTime(dtLastCallTime)) return false;

                scantime = dtLastCallTime;
                return true;
            }
            finally
            {
                if (reg != null) reg.Close();
            }

        }
        catch (Exception)
        {
            ;
        }
        finally
        {
            reg = null;
            dateBytes = null;
        }


        return false;
    }



    /// <summary>
    /// By default we allow Scan to be performed
    /// </summary>
    /// <param name="strAppKey"></param>
    /// <returns></returns>
    public static bool CanWeCallFn(string strAppKey)
    {
        try
        {
            strAppKey = GetTimeStampKeyPath(strAppKey);
            DateTime lastScanTime = DateTime.Now;
            if (!GetLassFnCallTimeStamp(ref lastScanTime, strAppKey))
            {
                return true;
            }
            return (DateTime.Now.Subtract(lastScanTime).TotalHours > RECHECK_TIME_PERIOD);

        }
        catch (Exception)
        {
            ;
        }

        return true;
    }
    public static bool ClearLastFnCallTime(string strAppKey)
    {
        RegistryKey reg = null;

        try
        {
            try
            {
                strAppKey = GetTimeStampKeyPath(strAppKey);
                if ((reg = Registry.CurrentUser.OpenSubKey(strAppKey)) != null)
                {
                    reg.DeleteValue(ValueName);
                    return true;
                }

            }
            finally
            {
                if (reg != null) reg.Close();
            }

        }
        catch (Exception)
        {
            ;
        }
        finally
        {
            reg = null;
        }


        return false;
    }
}
