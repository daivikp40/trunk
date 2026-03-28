using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Globalization;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer.Models;
using DuplicatePhotosFixer.ClassDictionary;

namespace DuplicatePhotosFixer.App_Code
{
    public class cRegistrySettings
    {
        public const string REGSTR_TOOL_HANDLE = "TOOLHWND";						// REG_SZ self handle value
        public const string REGSTR_PBKEY = "PbKey";
        public const string REGSTR_OLDASP_KEY = "RegisteredPass";
        public const string REGSTR_OLDASP_UNAME = "RegisteredUser";
        public const string REGSTR_OLDASP_REGDATE = "RegisterDate";
        public const string REGSTR_SILENT_ENALBELD = "issilent";
        public const string REGSTR_PHONE_NO_ENABLED = "isphone";
        public const string REGSTR_VALUE_MACHINE_UNIQUE_ID = "MachineUniqueId";

        public const string REGSTR_VALUE_IS_VIRTUAL_MACHINE = "ibv";
        public const string REGSTR_VALUE_PC_NAME = "pcname";
        public const string REGSTR_PHONENO = "TELNO";
        public const string REGSTR_IS_TEL_NO_NEEDED = "istelnoneeded";
        public const string REGSTR_IS_MS_LOGO_NEEDED = "IsMSLogoNeeded";
        public const string REGSTR_IS_USE_NOTIFIER = "isusenotifier";
        public const string REGSTR_ISSTORENEW = "IsStoreNew";
        public const string REGSTR_KEY = "Key";
        public const string REGSTR_EXPIRED = "Expired";
        public const string REGSTR_REGVER = "REGVER";
        public const string REGSTR_REGVER_UINS = "REGVER-UNINSTALL";
        public const string REGSTR_MAXFIXLIMIT = "MaxFixLimit";
        public const string REGSTR_INSTALLEDPATH = "InstalledPath";
        public const string REGSTR_APP_BUYNOW_URL = "BuyNowURL";
        public const string REGSTR_APP_RENEWNOW_URL = "RenewNowURL";
        public const string REGSTR_APP_FORGOTPASSWORD = "forgotpwdURL";

        public const string REGSTR_APP_SCAN_MODE = "scnmd"; // Scan Mode eScanMode {File System, Google Drive, Drop Box etc.}

        public const string REGSTR_APP_CONTROLPANEL_URL = "CplURL";
        public const string REGSTR_APP_FEEDBACK_URL = "feedbackurl";

        public const string REGSTR_APP_UTM_SRC = "utm_source";
        public const string REGSTR_APP_UTM_CAMPAIGN = "utm_campaign";
        public const string REGSTR_APP_UTM_MEDIUM = "utm_medium";
        public const string REGSTR_APP_UTM_AFFILIATEID = "affiliateid";
        public const string REGSTR_APP_FINALPARAMS = "FinalParams";
        public const string REGSTR_APP_UNINSTALL_FINALPARAMS = "UninstallFinalParams";
        public const string REGSTR_APP_X_AT = "x-at";
        public const string REGSTR_APP_SETUP_TYPE = "setup_type";
        public const string REGSTR_APP_TRIAL_TYPE = "trial_type";
        public const string REGSTR_APP_UTM_CID = "utm_cid";

        public const string REGSTR_CONSIDER_TIME_INTERVAL = "cti"; /// Consider Time Interval 
        public const string REGSTR_CONSIDER_GPS = "cgps"; /// Consider Time Interval
        public const string REGSTR_CONSIDER_TIME_NO_CAPTURE_DT = "ctinc"; // Include time interval settings if capture date is not available.
        public const string REGSTR_INCLUDE_ROTATED_IMAGES = "iir"; // Include images rotated.
        public const string REGSTR_INCLUDE_FLIPPED_IMAGES = "iif"; // Include images flipped.

        public const string REGSTR_MOVE_TO_BIN = "mtb"; /// Move to recycle bin
        public const string REGSTR_MOVE_TO_LOCATION = "mtl"; /// Move to a user defined location 
        public const string REGSTR_MOVE_LOCATION = "mvloc"; /// Move duplicate location 
        
        //additional aso vars
        public const string REGSTR_APP_SUPPORT_PHONE = "G_SUPPORT_PHONE_NUM";

        public const string REGSTR_AFTER_INSTALL_URL = "afterInstallUrl";
        public const string REGSTR_AFTER_UPDATE_URL = "AfterUpdateUrl";
        public const string REGSTR_FIRE_SILENT_AFT_LAUNCH_URL = "AFTINSTS"; // check whether after install URL will be fired silently or not...

        //public const string REGSTR_LAST_DUPLICATE_COUNT = "TotalDuplicates";
        public const string REGSTR_LAST_TOTAL_DUPLICATE_SIZE = "TotalDuplicateSize";

        public const string REGSTR_IS_SERVER_INFORMED_FOR_FREE_CLEANING = "IsServerInformedForFreeCleaning";
        public const string REGSTR_IS_FREE_CLEANING_DONE = "IsFreeCleanDone";

        public const string REGSTR_DEP_ON_SPC_DOCK = "DepOnSPCDock";

        public const string REGSTR_APP_UTM_UPDT = "utm_updt";
        public const string REGSTR_APP_UTM_UPDATEDATE = "utm_updatedate";
        public const string REGSTR_APP_UTM_DAYS = "utm_days";
        public const string REGSTR_APP_UTM_NAG_DAYS = "utm_nagdays";

        public const string REGSTR_IS_UPDATE_BUILD = "IsUpdateBuild";
        public const string REGSTR_IS_LB_BUILD = "IsLbBuild";
        public const string REGSTR_IS_SEND_KEY_STATUS = "IsSendKeyStatus";

        public const string REGSTR_LAST_SCAN_DATE = "lsdt";

        public const string REGSTR_PRODUCT_REGISTER_DATE = "registerdate";

        public const string REGSTR_INSTALL_DATE_TIME_STRING = "instdts";
        public const string REGSTR_INSTALL_DATE_TIME_TICKS = "instdt";

        public const string REGSTR_SETUP_BUILD_DATE_STRING = "bdts";
        public const string REGSTR_SETUP_BUILD_DATE_TICKS = "bdt";

        public const string REGSTR_LOAD_STRINGS_CHAR = "lsc";

        /// Start Update ini parameters 
        public const string REGSTR_SEND_IP_PARAMS = "lip"; /// Pass IP related params in urls 
        public const string REGSTR_SHOW_ALL_NAGS_VIA_NOTIFER = "sanvn"; /// Show all nags via notifier, either reminder or offers or cross promotions
        public const string REGSTR_APP_TUTORIAL_LINK = "ytu"; /// Youtube tutorial URL
        public const string REGSTR_WATCH_TUTORIAL_TYPE = "wtt"; /// Watch tutorial type
        public const string REGSTR_CHANNEL_LINK = "ycu"; /// Youtube tutorial URL 
        public const string REGSTR_THEME_PROGRESS_WINDOW = "tpw"; /// Theme of Progress Window
                                                         /// End Update ini parameters
        public const string REGSTR_SHOW_SUPPORT_EMAIL_FOOTER = "ssef"; // Show support email at footer depends on status enum "eUserStatusType" {Never=0,RegisterUser=1,TrialUser=2,ExpiredUser=3,Always=4}

        public const string REGSTR_GOOGLE_DRIVE_INFO_MSG = "dgdm"; //VG280121 - dgdm - display google drive messagebox, dgdm = 0 to hide messagebox, dgdm = 1 to show messagebox, default is 1


        public const string REGSTR_UPDATEOFFER_SHOW = "suo"; //show update offers
        public const string REGSTR_UPDATEOFFER_USERTYPE = "uout"; //update offer user type (0->trial/expired, 1->registered, -1->both) 

        public const string REGSTR_SHOW_DIFF = "cppsd"; // show cross promotion products days difference
        public const string REGSTR_OLD_VERSION = "oldversion"; // Previous application version if any
        public const string REGSTR_THEMEMODE = "thememode";
        public void SetRegistartionSettings()
        {
            try
            {

                //write to system registry also
                /*cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_KEY, cGlobalSettings.cRegDetails.Key, RegistryValueKind.String);
                cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_EXPIRED, Convert.ToInt32(cGlobalSettings.cRegDetails.IsExpired), RegistryValueKind.DWord);
                cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_REGVER, Convert.ToInt32(cGlobalSettings.cRegDetails.IsRegistered), RegistryValueKind.DWord);
                cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_REGVER_UINS, Convert.ToInt32(cGlobalSettings.cRegDetails.IsRegistered), RegistryValueKind.DWord);
                cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_MAXFIXLIMIT, cGlobalSettings.cRegDetails.TrialCleanItemCount, RegistryValueKind.DWord);
                cGlobalSettings.SetCommonAppDataRegistryValue(REGSTR_INSTALLEDPATH, Application.StartupPath, RegistryValueKind.String);*/



                //cLastScanLogDetails oLastScanLogDetails = cGlobalSettings.oScanSettings.ShowLastScanLogDetails();


                //write in CU
                //DateTime oDt = new DateTime();
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_KEY, cGlobalSettings.cRegDetails.Key, RegistryValueKind.String);
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_EXPIRED, Convert.ToInt32(cGlobalSettings.cRegDetails.IsExpired), RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_REGVER, Convert.ToInt32(cGlobalSettings.cRegDetails.IsRegistered), RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_REGVER_UINS, Convert.ToInt32(cGlobalSettings.cRegDetails.IsRegistered), RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_MAXFIXLIMIT, cGlobalSettings.cRegDetails.TrialCleanItemCount, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_INSTALLEDPATH, Application.StartupPath, RegistryValueKind.String);


                // SetExclusionSettings();
                // SetGeneralSettings();
                // SetSchduleSettings();
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|SaveSettings()|exception:{1}", this, ex.Message));
            }
        }

        public void LoadSettings()
        {
            try
            {
                // Load general settings
                LoadGeneralSettings();

                //Load Extra Settings
                // LoadExtraSettings();

                //Load Move Settings
                LoadMoveSettings();

                LoadFileFormatMinSizeSettings();


                //  App.oMainReference.objFileFormat = new vmFileFormatSettings();


            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: LoadSettings: ", ex);
            }
        }

       public void LoadGeneralSettings()
        {
            try
            {
                cGlobalSettings.GeneralSettingsData.OpenDialogOnDeleteChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowDeleteConfirmation.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.OpenAssistantChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowAutoMarkConfirmation.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.BackbtnChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowBackConfirmation.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.SysStartupChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.ScanSystemStartup.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.ProductMesChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.ProductMessages.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.OtherProductMesChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.OtherProductMessages.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.GeneralSettingsData.PeriodicMesChecked = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.PeriodicMessages.ToString(), 1)) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: LoadGeneralSettings: ", ex);

            }
        }

        public void SaveGeneralSettingsToRegistry()
        {
            try
            {
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowDeleteConfirmation.ToString(), cGlobalSettings.GeneralSettingsData.OpenDialogOnDeleteChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowAutoMarkConfirmation.ToString(), cGlobalSettings.GeneralSettingsData.OpenAssistantChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.bNotShowBackConfirmation.ToString(), cGlobalSettings.GeneralSettingsData.BackbtnChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.ScanSystemStartup.ToString(), cGlobalSettings.GeneralSettingsData.SysStartupChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.ProductMessages.ToString(), cGlobalSettings.GeneralSettingsData.ProductMesChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.OtherProductMessages.ToString(), cGlobalSettings.GeneralSettingsData.OtherProductMesChecked, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.PeriodicMessages.ToString(), cGlobalSettings.GeneralSettingsData.PeriodicMesChecked, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: SaveGeneralSettingsToRegistry: ", ex);
            }
        }


        public void LoadExtraSettings()
        {
            try
            {
                cGlobalSettings.ExtraSettings.DbConsiderTimeInterval = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_TIME_INTERVAL.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.ExtraSettings.DbConsiderGPS = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_GPS.ToString(), 1)) == 1 ? true : false;

                cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_TIME_NO_CAPTURE_DT.ToString(), 1)) == 1 ? true : false;

                cGlobalSettings.ExtraSettings.DbIncludeRotatedImages = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_INCLUDE_ROTATED_IMAGES.ToString(), 1)) == 1 ? true : false;

                cGlobalSettings.ExtraSettings.DbIncludeFlippedImages = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_INCLUDE_FLIPPED_IMAGES.ToString(), 1)) == 1 ? true : false;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: LoadExtraSettings: ", ex);

            }
        }

        public void SaveExtraSettingsToRegistry()
        {
            try
            {
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_TIME_INTERVAL.ToString(), cGlobalSettings.ExtraSettings.DbConsiderTimeInterval, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_GPS.ToString(), cGlobalSettings.ExtraSettings.DbConsiderGPS, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_CONSIDER_TIME_NO_CAPTURE_DT.ToString(), cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_INCLUDE_ROTATED_IMAGES.ToString(), cGlobalSettings.ExtraSettings.DbIncludeRotatedImages, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_INCLUDE_FLIPPED_IMAGES.ToString(), cGlobalSettings.ExtraSettings.DbIncludeFlippedImages, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: SaveExtraSettingsToRegistry: ", ex);
            }
        }

        public void LoadMoveSettings()
        {
            try
            {
                cGlobalSettings.MoveSettings.DbLocationPath = Convert.ToString(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_LOCATION, cGlobalSettings.FilePathToMoveThisLocation()));
                cGlobalSettings.MoveSettings.DbMoveToRecycle = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_TO_BIN.ToString(), 1)) == 1 ? true : false;
                cGlobalSettings.MoveSettings.DbMoveToLocation = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_TO_LOCATION.ToString(), 1)) == 1 ? true : false;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: LoadMoveSettings: ", ex);

            }
        }

        public void SaveMoveSettingstoRegistry()
        {
            try
            {
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_LOCATION, cGlobalSettings.MoveSettings.DbLocationPath, RegistryValueKind.String);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_TO_BIN.ToString(), cGlobalSettings.MoveSettings.DbMoveToRecycle, RegistryValueKind.DWord);
                cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_MOVE_TO_LOCATION.ToString(), cGlobalSettings.MoveSettings.DbMoveToLocation, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: SaveMoveSettingstoRegistry: ", ex);
            }
        }

        public void LoadFileFormatMinSizeSettings()
        {
            try
            {
               
                cGlobalSettings.MinFileSize = (long)cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.minSize.ToString());
                cGlobalSettings.MinFileSizeIn = Convert.ToInt16(cGlobalSettings.GetUserAppDataRegistryValue(enumClass.eRegistryKeys.minSizeIn.ToString()));

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::LoadFileFormatMinSizeSettings : ", ex);

            }
        }
        public void SaveMinFileSettingstoRegistry()
        {
            try 
            {
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.minSize.ToString(), cGlobalSettings.MinFileSize, RegistryValueKind.QWord);
                cGlobalSettings.SetUserAppDataRegistryValue(enumClass.eRegistryKeys.minSizeIn.ToString(), App.oMainReference.objFileFormat.UnitIndex, RegistryValueKind.QWord);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: SaveMinFileSettingstoRegistry: ", ex);

            }
        }

        public const string REGSTR_TOTAL_MARKED = "TotalMarked";
        public void SetTotalFileMarked(long nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_MARKED, nValue, RegistryValueKind.QWord);
        }
        public long GetTotalFilesMarked()
        {
            long nVal = 0;
            try
            {
                nVal = Convert.ToInt64(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_MARKED, 0));
                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalFilesMarked", ex);
            }
            return nVal;
        }


        public const string REGSTR_TOTAL_DUPLICATES = "TotalDuplicates";
        public void SetTotalDuplicatesFound(long nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_DUPLICATES, nValue, RegistryValueKind.QWord);
        }

        public const string REGSTR_TOTAL_DUPLICATES_STR = "TotalDuplicatesStr";
        public void SetTotalDuplicatesFoundStr(long nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_DUPLICATES_STR, nValue.ToString(), RegistryValueKind.String);
        }

        public const string REGSTR_TOTAL_MARKED_STR = "TotalMarkedStr";
        public void SetTotalFileMarkedStr(long nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_MARKED_STR, nValue.ToString(), RegistryValueKind.String);
        }

        public long GetTotalDuplicatesFound()
        {
            long nVal = 0;
            try
            {
                nVal = Convert.ToInt64(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_DUPLICATES, 0));
                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalDuplicatesFound", ex);
            }
            return nVal;
        }

        public const string REGSTR_TOTAL_DUPLICATES_SPACE = "TotalDuplicatesSpace";
        public void SetTotalDuplicatesSpace(long nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_DUPLICATES_SPACE, nValue, RegistryValueKind.QWord);
        }
        public long GetTotalDuplicatesSpace()
        {
            long nVal = 0;
            try
            {
                nVal = Convert.ToInt64(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_TOTAL_DUPLICATES_SPACE, 0));

                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalDuplicatesSpace", ex);
            }
            return nVal;
        }


        public long GetSetupType()
        {
            int nVal = 1;
            try
            {
                nVal = Convert.ToInt32(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_APP_SETUP_TYPE, 1));

                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: GetSetupType", ex);
            }
            return nVal;
        }

        public long GetTrialType()
        {
            int nVal = 1;
            try
            {
                nVal = Convert.ToInt32(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_APP_TRIAL_TYPE, 1));

                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings:: GetTrialType", ex);
            }
            return nVal;
        }

        public const string REGSTR_EMAIL = "Email";
        public void SetEmailAddress(string nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_EMAIL, nValue, RegistryValueKind.String);
        }
        public string GetEmailAddress()
        {
            string nVal = string.Empty;
            try
            {
                nVal = Convert.ToString(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_EMAIL, ""));

                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalDuplicatesSpace", ex);
            }
            return nVal;
        }


        public void SetIsFreeCleanDone(string nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_IS_FREE_CLEANING_DONE, nValue, RegistryValueKind.DWord);
        }
        public string GetIsFreeCleanDone()
        {
            string nVal = "";
            try
            {
                nVal = Convert.ToString(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_IS_FREE_CLEANING_DONE, ""));

                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalDuplicatesSpace", ex);
            }
            return nVal;
        }




        public void SetLastScanDate(DateTime nValue)
        {
            cGlobalSettings.SetUserAppDataRegistryValue(cRegistrySettings.REGSTR_LAST_SCAN_DATE, nValue.ToString("ddMMyyyy"), RegistryValueKind.String);
        }
        public DateTime GetLastScanDate()
        {
            DateTime nVal = DateTime.Now;
            try
            {
                string strVal = Convert.ToString(cGlobalSettings.GetCommonAppDataRegistryValue(cRegistrySettings.REGSTR_LAST_SCAN_DATE, nVal.ToString("ddMMyyyy")));
                if (!DateTime.TryParseExact(strVal, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out nVal))
                {
                    nVal = DateTime.Now;
                    SetLastScanDate(nVal);
                }
                return nVal;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cRegistrySettings::GetTotalDuplicatesSpace", ex);
            }
            return nVal;
        }



        public const string REGSTR_INSTALL_DATE = "FirstInstallDate";
        public string FirstInstallDate
        {
            get
            {
                string val = Convert.ToString(cGlobalSettings.GetUserAppDataRegistryValue(REGSTR_INSTALL_DATE, ""));
                if (string.IsNullOrEmpty(val))
                    return "";
                else
                    return (val);
            }
            set
            {
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_INSTALL_DATE, value, RegistryValueKind.String);//reset 
            }
        }


        public const string REGSTR_FREE_TRIAL_PERIOD_DATE_TIME = "ftpdt";
        public DateTime FreeTrialPeriodDateTime
        {
            get
            {
                DateTime dtExpire = DateTime.MinValue;
                string val = Convert.ToString(cGlobalSettings.GetUserAppDataRegistryValue(REGSTR_FREE_TRIAL_PERIOD_DATE_TIME, ""));

                if (!string.IsNullOrEmpty(val))
                {
                    /// Registry Found...
                    /// 
                    dtExpire = dtExpire.AddDays(10);
                    string strdt = cGlobalSettings.DecryptString(val);

                    if (!DateTime.TryParseExact(strdt, cGlobalSettings.strDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtExpire))
                    {
                        dtExpire = DateTime.MinValue;
                        dtExpire = dtExpire.AddDays(10);
                    }
                }

                return dtExpire;
            }
            set
            {
                DateTime dt = value;

                string strDtExpire = cGlobalSettings.EncryptString(dt.ToString(cGlobalSettings.strDateTimeFormat));

                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_FREE_TRIAL_PERIOD_DATE_TIME, strDtExpire, RegistryValueKind.String);//reset 
            }
        }


        //KM19062017 CN Tracking
        public const string REGSTR_CN_TRACKING_DONE = "CN";
        public bool IsCNTrackingDone
        {
            get
            {
                return Convert.ToBoolean(
                 cGlobalSettings.GetUserAppDataRegistryValue(REGSTR_CN_TRACKING_DONE, false));
            }
            set
            {
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_CN_TRACKING_DONE, value, RegistryValueKind.DWord);
            }
        }


    }
}
