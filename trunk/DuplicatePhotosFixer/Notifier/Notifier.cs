using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLogger;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using DuplicatePhotosFixer;
using System.Globalization;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer.Helpers;

namespace AppNotifier
{
    public static class Notifier
    {
        public static bool IsEncrypted = false;
        public static string iniSetionName = "notifierStrings";
        public static string notifierName = "Notifier";
        public static string notifierEXEName = "DPFNotifier.exe";
        public const string REG_N_SETUP_TYPE = "nst"; //Setup Type
        public static string TaskNameEventTrigger = string.Format("{0}{1}", cGlobalSettings.GetProductName(), notifierName);
        public static string TaskName_Startup = string.Format("{0}{1}{2}", cGlobalSettings.GetProductName(), notifierName, "_startup");
        public static string TaskName_trigger = string.Format("{0}{1}{2}", cGlobalSettings.GetProductName(), notifierName, "_trigger");

        public const string strRegEventSource = "EventSource";
        public static string strInstallDateFormat = "ddMMyyyy HH:mm:ss";

        /// Create Notifier.ini
        /// 
        public static void CreateIni()
        {
            try
            {
                //return;
                //string dirName = Path.GetFileName(Application.StartupPath);
                //string folderName = PathEx.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"notifier\notifier\" + dirName);
                string iniPath = PathEx.Combine(cGlobalSettings.getAppDataApplicationPath()/* folderName*/, @"notifier.ini");
               // WriteSystemInfoInRegistry();
                if (File.Exists(iniPath))
                {
                    try
                    {
                        File.Delete(iniPath);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (!Directory.Exists(Path.GetDirectoryName(iniPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(iniPath));
                }

                /*if (!Directory.Exists(Path.GetDirectoryName(folderName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(folderName));

                }*/

                cIniFile oIniReader = new cIniFile(iniPath);

                Dictionary<string, string> resString = new Dictionary<string, string>();
                resString.Add("utm_source", cGlobal.UTM_SOURCE);
                resString.Add("utm_medium", cGlobal.UTM_MEDIUM);
                resString.Add("utm_campaign", cGlobal.UTM_CAMPAIGN);
                resString.Add("affiliateid", cGlobal.UTM_AFFILIATE);
                resString.Add("x-at", cGlobal.X_AT);
                resString.Add("isreg", cGlobalSettings.cRegDetails.IsRegistered ? "1" : "0");
                resString.Add("isexpired", cGlobalSettings.cRegDetails.IsExpired ? "1" : "0");
                //resString.Add("utm_content", cGlobal.);
                resString.Add("utm_terminfo", "");
                ////resString.Add("utm_updt", cGlobal.UTM_UPDT);
                ////resString.Add("utm_updatedate", cGlobal.UTM_UPDATEDATE);
                ////resString.Add("utm_days", cGlobal.UTM_DAYS);
                ////resString.Add("utm_nag_days", cGlobal.UTM_NAG_DAYS);
                ////resString.Add("ec", cGlobal.strEventCategory);
                if (cGlobalSettings.oScanSettings.CurrentLangSettings == null)
                {
                    cGlobalSettings.oScanSettings.GetLanguageFromRegistry();
                }
                if (cGlobalSettings.oScanSettings != null)
                    resString.Add("lang_code", cGlobalSettings.oScanSettings.LanguageCode);
                ////resString.Add("macid", DuplicatePhotoFixer.HardwareHash.GetHardwareHash());
                ////resString.Add("tid", cGlobal.tid);
                ////resString.Add("productid", cGlobalSettings.ProductId.ToString());
                ////resString.Add("productname", cGlobalSettings.GetProductNameFromAssembly());
                ////resString.Add("FINALPARAMS", cGlobal.FINALPARAMS);
                ////resString.Add("RegPathWithoutHive", cGlobalSettings.VersionIndependentRegKeyNoHive);
                ////resString.Add("RegHive", Registry.CurrentUser.ToString());
                ////resString.Add("FirstInstallDate", cGlobalSettings.oRegSettings.FirstInstallDate);
                ////resString.Add("registerdate", cGlobalSettings.ProductRegisterDate.ToString(strInstallDateFormat, CultureInfo.InvariantCulture));
                ////resString.Add("ntrialdaysremains", "0"); // Number of trial days remaining if any
                
                foreach (KeyValuePair<string, string> iniItem in resString)
                {
                    oIniReader.IniWriteValue(iniSetionName, iniItem.Key.ToUpper(), iniItem.Value);
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CreateIni:: ", ex);
            }
        }


        //public static void WriteSystemInfoInRegistry()
        //{
        //    if (cGlobalSettings.oSysBasicInfo == null)
        //    {
        //        cGlobalSettings.oSysBasicInfo = cSystemInfo.GetSystemInfo();
        //    }
        //    /// cGlobalSettings.oSysBasicInfo.ManufacturerLogo
        //    /// OS name 
        //    /// ----------
        //    cGlobalSettings.SetUserAppDataRegistryValue("OSFriendlyName", cGlobalSettings.oSysBasicInfo.Caption, RegistryValueKind.String);

        //    /// Ram
        //    /// ------
        //    cGlobalSettings.SetUserAppDataRegistryValue("RAM", cGlobalSettings.oSysBasicInfo.strMemory_RAM, RegistryValueKind.String);

        //    /// Processor
        //    /// -----------
        //    cGlobalSettings.SetUserAppDataRegistryValue("ProcessorName", cGlobalSettings.oSysBasicInfo.ProcessorName, RegistryValueKind.String);

        //    /// Model 
        //    /// --------
        //    cGlobalSettings.SetUserAppDataRegistryValue("Model", cGlobalSettings.oSysBasicInfo.Model, RegistryValueKind.String);

        //    /// Manufacturer ID 
        //    /// --------
        //    cGlobalSettings.SetUserAppDataRegistryValue("Manufacturer", (int)cGlobalSettings.oSysBasicInfo.eManufacturerID, RegistryValueKind.DWord);

        //    /// Manufacturer Name
        //    /// --------
        //    cGlobalSettings.SetUserAppDataRegistryValue("ManufacturerName", cGlobalSettings.oSysBasicInfo.Manufacturer, RegistryValueKind.String);

        //    // PCRestartTimeString
        //    cGlobalSettings.SetUserAppDataRegistryValue("pcrts", cGlobalSettings.oSysBasicInfo.PCRestartTime.ToString("dd-MM-yyyy HH:mm:ss"), RegistryValueKind.String);

        //    // PCRestartTimeTicks
        //    cGlobalSettings.SetUserAppDataRegistryValue("pcrt", cGlobalSettings.oSysBasicInfo.PCRestartTime.Ticks, RegistryValueKind.String);

        //}

        //public static bool UpdateOtherProductNotificationSettings(bool bCreate, out bool bModified)
        //{
        //    bool isSuccess = false;
        //    bModified = false;
        //    try
        //    {   
        //        isSuccess = RunHelperApp(bCreate, out bModified);

        //        if (isSuccess)
        //        {
        //            /// Operation done success
        //            /// 
        //            CheckBaseUID(bCreate);

        //        }
        //        else
        //        {
        //            /// Do nothing 

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("UpdateOtherProductNotificationSettings:: ", ex);
        //    }

        //    return isSuccess;
        //}

        //public static bool RunHelperApp(bool bCreate, out bool bModified)
        //{
        //    bModified = false;
        //    if (AppFunctions.TaskExistance(cGlobalSettings.GetProductNameFromAssembly() + notifierName + "_startup") == cGlobalSettings.bShowOtherProductMessages &&
        //        AppFunctions.TaskExistance(cGlobalSettings.GetProductNameFromAssembly() + notifierName + "_trigger") == cGlobalSettings.bShowOtherProductMessages &&
        //        AppFunctions.TaskExistance(cGlobalSettings.GetProductNameFromAssembly() + notifierName) == cGlobalSettings.bShowOtherProductMessages)
        //    {
        //        return true;
        //    }

        //    bool isSuccess = false;
        //    try
        //    {
        //        string AppExecutablePath = Path.Combine(Application.StartupPath, notifierEXEName);
        //        //AppExecutablePath = @"C:\Program Files (x86)\Duplicate Photos Fixer Pro\DuplicatePhotosFixerPro.exe";
        //        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //        process.StartInfo.FileName = Path.Combine(Application.StartupPath, "DPFHelper.exe");
        //        //if (bCreate)
        //        {
        //            process.StartInfo.Arguments = "/exepath\"" + AppExecutablePath + "\" " + (cGlobalSettings.bShowOtherProductMessages ? "/createNotifier" : "/deleteNotifier");
        //        }/*
        //        else
        //        {
        //            process.StartInfo.Arguments = "/exepath\"" + AppExecutablePath + "\" " + (cGlobalSettings.bShowProductMessages ? "/createshowoffer" : "/deleteshowoffer");
        //        }*/
        //        process.StartInfo.CreateNoWindow = true;
        //        if (Environment.OSVersion.Version.Major >= 6)
        //        {
        //            process.StartInfo.Verb = "runas";
        //        }
        //        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //        process.Start();
        //        process.WaitForExit();
        //        isSuccess = true;
        //        bModified = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("RunHelperApp:: ", ex);
        //    }
        //    /*if (!isSuccess)
        //    {
        //        if (bCreate)
        //        {
        //            //chkStartupLaunch.Checked = !chkStartupLaunch.Checked;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //            //chk_productMsg.Checked = !chk_productMsg.Checked;
        //        }
        //    }*/
        //    return isSuccess;
        //}


        //public static bool CheckBaseUID(bool bCreate)
        //{
        //    bool isBase = false;
        //    try
        //    {
        //        string strBaseID = "";

        //        RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Systweak\\" + notifierName, true);
        //        if (key != null)
        //            strBaseID = GetValue<string>(key, "BaseUID");

        //        if (string.IsNullOrEmpty(strBaseID) || string.Compare(strBaseID, cGlobalSettings.GetProductNameFromAssembly(), true) == 0)
        //        {
        //            if (bCreate)
        //            {
        //                SetRegistryValue(Registry.CurrentUser, "Software\\Systweak\\" + notifierName, "BaseUID", cGlobalSettings.GetProductNameFromAssembly());
        //                strBaseID = GetValue<string>(Registry.CurrentUser.OpenSubKey("Software\\Systweak\\" + notifierName), "BaseUID");
        //                isBase = true;
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(strBaseID))
        //                    key.DeleteValue("BaseUID");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("CheckBaseUID:: ", ex);
        //    }
        //    return isBase;
        //}


        public static void SetRegistryValue(RegistryKey regHive, string registryPath, string regKey, string regValue)
        {
            RegistryKey key = null;
            try
            {
                key = regHive.OpenSubKey(registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);

                if (key == null)
                {
                    key = regHive.CreateSubKey(registryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);

                    /*SetRegistryValue(regHive, registryPath, regKey, regValue);
                    return;*/
                }

                key.SetValue(regKey, regValue, RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SetRegistryValue:: ", ex);
            }
            finally
            {
                if (key != null) key = null;
            }
        }

        public static T GetValue<T>(this RegistryKey key, string name)
        {
            return key.GetValue<T>(name, default(T));
        }

        /// <summary>
        /// https://gunnarpeipman.com/csharp/getvalue-extension-method-for-registrykey-class/
        /// https://weblogs.asp.net/gunnarpeipman/getvalue-extension-method-for-registrykey-class
        /// var softKey = Registry.CurrentUser.OpenSubKey("Software");
        /// var myKey = softKey.OpenSubKey("MyAppSettings");
        /// 
        /// showTitle.Checked = myKey.GetValue<bool>("ShowTitle", true);
        /// showDescr.Checked = myKey.GetValue<bool>("ShowDescriptions");
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValue<T>(this RegistryKey key, string name, T defaultValue)
        {
            var obj = key.GetValue(name, defaultValue);

            if (obj == null)
                return defaultValue;

            if (obj is T)
                return (T)obj;

            if (typeof(T) == typeof(string))
            {
                obj = obj.ToString();
                return (T)obj;
            }

            var types = new[] { typeof(string), typeof(T).MakeByRefType() };

            var method = typeof(T).GetMethod("TryParse", types);
            if (method == null)
                return defaultValue;

            var valueString = obj.ToString();
            T value = defaultValue;
            var methodParams = new object[] { valueString, value };

            method.Invoke(obj, methodParams);
            return (T)methodParams[1];
        }


        //public static string getActualData(string strdata)
        //{
        //    try
        //    {
        //        if (IsEncrypted)
        //        {
        //            return EncDecOperation.DecryptString(strdata);
        //        }
        //        else
        //        {
        //            return strdata;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("getActualData:: ", ex);
        //    }
        //    return strdata;
        //}


        public const string REGSTR_SHOW_PRICE_PLANS = "bShowPricePlans";
        private static bool? m_ShowPricePlans = null;
        public static bool ShowPricePlans
        {
            get
            {
                if (m_ShowPricePlans == null)
                {
                    m_ShowPricePlans = Convert.ToInt32(cGlobalSettings.GetUserAppDataRegistryValue(REGSTR_SHOW_PRICE_PLANS, 0)) > 0 ? true : false;
                }
                if (m_ShowPricePlans == null || m_ShowPricePlans == false)
                    return false;
                else
                    return true;
            }
            set
            {
                cGlobalSettings.SetUserAppDataRegistryValue(REGSTR_SHOW_PRICE_PLANS, value == true ? true : false, RegistryValueKind.DWord);
                m_ShowPricePlans = value;
            }
        }


        //public static void triggerNotifier(EventSource eventSource)
        //{
        //    try
        //    {
        //        cGlobalSettings.SetUserAppDataRootRegistryValueNotifier(strRegEventSource, (int)eventSource + (int)EventSource.Restart, RegistryValueKind.DWord);

        //        if (GetNotifierType() == eSetupType.Scheduler)
        //        {
        //            triggerNotifierScheduler(eventSource);
        //        }
        //        else if (GetNotifierType() == eSetupType.Timer)
        //        {
        //            triggerNotifierTimer(eventSource);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("triggerNotifier:: ", ex);
        //    }
        //}

        //static void triggerNotifierTimer(EventSource eventSource)
        //{
        //    try
        //    {
        //        if (AppFunctions.TaskExistance(TaskNameEventTrigger))
        //        {
        //            System.Diagnostics.Process process = new System.Diagnostics.Process();

        //            process.StartInfo.FileName = "cmd.exe";
        //            process.StartInfo.Arguments = string.Format("/C schtasks.exe /RUN /tn \"{0}\"", TaskNameEventTrigger);
        //            process.StartInfo.CreateNoWindow = true;
        //            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //            process.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("triggerNotifier:: ", ex);
        //    }
        //}

        //static void triggerNotifierScheduler(EventSource eventSource)
        //{
        //    try
        //    {
        //        if (AppFunctions.TaskExistance(TaskName_trigger))
        //        {
        //            System.Diagnostics.Process process = new System.Diagnostics.Process();

        //            process.StartInfo.FileName = "cmd.exe";
        //            process.StartInfo.Arguments = string.Format("/C schtasks.exe /RUN /tn \"{0}\"", TaskName_trigger);
        //            process.StartInfo.CreateNoWindow = true;
        //            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //            process.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("triggerNotifierNew:: ", ex);
        //    }
        //}



        public enum eSetupType
        {
            Scheduler = 0,
            Timer
        }

        //public static eSetupType setupType = eSetupType.Scheduler;

        //public static eSetupType GetNotifierType()
        //{
        //    eSetupType sType = eSetupType.Scheduler;
        //    try
        //    {
        //        int nType = (int)eSetupType.Scheduler;
        //        int.TryParse(Convert.ToString(cGlobalSettings.GetUserAppDataRootRegistryValueNotifier(REG_N_SETUP_TYPE, nType)), out nType);

        //        sType = (eSetupType)nType;
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("", ex);
        //    }
        //    return sType;
        //}


        public enum EventSource
        {
            /// <summary>
            /// default when not specified
            /// </summary>
            WhenEverYouCanStart = 0,

            /// <summary>
            /// on PC start and login event.
            /// </summary>
            Restart = 1, // when PC starts, or log off login

            /// <summary>
            /// like show nag at fix time and after 2 hours or like any such condition.
            /// interval will define, after which time to show the nag
            /// </summary>
            Daily = 2,

            /// <summary>
            /// When setup install the app and app is launched for first time
            /// </summary>
            FirstRun = 4,
            FirstInstall = 4,

            /// <summary>
            /// Show nag after scan completed
            /// </summary>
            AfterScanCompleted = 8,

            /// <summary>
            /// Here we don't have param
            /// This way we can identify the launch
            /// </summary>
            AppLaunch = 16,

            /// <summary>
            /// lock and unlock event
            /// </summary>
            Unlock = 32,

            /// <summary>
            /// When update is installed and app is launched for first time.
            /// </summary>
            AfterUpdateInstall = 64,

            Once = 128,
            //WhenEverYouCanStart = 0 // default when not specified
            //Restart = 1
            //Login = 2
            //WakeUp = 4
            //FirstRun = FirstInstall = 8
            //AfterUpdateInstall = 16
            //UnLock = 32


            AfterClean = 256,

        }
    }
}
