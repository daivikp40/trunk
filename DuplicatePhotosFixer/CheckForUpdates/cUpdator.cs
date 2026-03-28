using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
//using DuplicatePhotosFixer.TrialProtector;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.CheckForUpdates
{
    
        public class cUpdator
        {

            #region public properties

            private string m_UpdateUrl;
            private string m_UpdateParams;
            private uint m_CurrentUpdateVersion;
            private string m_ProductName;
            private string m_ApplicationId;
            private string m_MachineID;
            private int m_DaysLeft;
            private string m_UpdateParamsAdvanced;
            private bool m_Stop;
            private bool m_Pause;

            private string m_LocalSavePath;
            private string m_CurrentVersionDate;
            public string CurrentVersionDate
            {
                get { return m_CurrentVersionDate; }
                set { m_CurrentVersionDate = value; }
            }
            public string LocalSavePath
            {
                get { return m_LocalSavePath; }
                set { m_LocalSavePath = value; }
            }

            public string UpdateUrl
            {
                get { return m_UpdateUrl; }
                set { m_UpdateUrl = value; }
            }

            public string UpdateParams
            {
                get { return m_UpdateParams; }
                set { m_UpdateParams = value; }
            }

            public uint CurrentUpdateVersion
            {
                get { return m_CurrentUpdateVersion; }
                set { m_CurrentUpdateVersion = value; }
            }

            public string ProductName
            {
                get { return m_ProductName; }
                set { m_ProductName = value; }
            }

            public string ApplicationId
            {
                get { return m_ApplicationId; }
                set { m_ApplicationId = value; }
            }


            public string MachineID
            {
                get { return m_MachineID; }
                set { m_MachineID = value; }
            }

            public int DaysLeft
            {
                get { return m_DaysLeft; }
                set { m_DaysLeft = value; }
            }

            public string UpdateParamsAdvanced
            {
                get { return m_UpdateParamsAdvanced; }
                set { m_UpdateParamsAdvanced = value; }
            }


            public bool Stop
            {
                get { return m_Stop; }
                set { m_Stop = value; }
            }
            public bool Pause
            {
                get { return m_Pause; }
                set { m_Pause = value; }
            }
            //long m_totalBytes = 0;
            //long m_totalReceived = 0;
            private string sPwd = string.Empty;

            #endregion

            #region "download declarations"
            /// <summary>
            /// Thread for the download process
            /// </summary>
            //private Thread downloadThread = null;

            /// <summary>
            /// Total number of files to download.  This field 
            /// is used for the progress bar.
            /// </summary>
            //private int totalFiles = 0;

            /// <summary>
            /// Total number of files read.  This filed is used
            /// for the progress bar.
            /// </summary>
            private int filesRead = 0;

            /// <summary>
            /// Progress precentage calculated.
            /// </summary>
            //private static int percentProgress;

            /// <summary>
            /// The delegate to call from the thread to update the form.
            /// </summary>
            /// <param name="filesRead">The number of files read.</param>
            /// <param name="totalFiles">The total files read.</param>
            private delegate void UpdateProgessCallback(
                int filesRead, int totalFiles);

            /// <summary>
            /// This delegate enables asynchronous calls for setting
            /// the text property on a TextBox control.
            /// </summary>
            /// <param name="text">The text to display.</param>
            private delegate void SetTextCallback(string text);

            /// <summary>
            /// This delegate enables asynchronous calls for enabling
            /// the button download.
            /// </summary>
            private delegate void EnableBtnDownloadCallback();

            /// <summary>
            /// this delegate enables asynchronous calls for enabling the progress bar and grid to work 
            /// </summary>
            /// <param name="nNumber"></param>
            /// <param name="enumStatus"></param>
            public delegate void delUpdateUI(int nNumber, cClientEnum.eDownloadStatus enumStatus);

            public List<cUpdateDetails> LsUpdates = new List<cUpdateDetails>();
            //Int32 nTotalUpdateCount = 0;
            #endregion


            //public cSevenZip oSevenZip = new cSevenZip();

            //public cSQliteFunction oSqlFunc = new csSQliteFunction(/*"cUpdator", 45*/);
            //public cSerializer oSerializer = new cSerializer();
            //  public cLoadDatabase oLoadDatabase = new cLoadDatabase();
            // public cUnrarWrapper oUnRar = new cUnrarWrapper();


            public cProgressArgs oUpdateProgressClass = null;
            public delegate void delUpdateInitalized(string sMessage, bool PrintInNewLine);
            public event delUpdateInitalized oShowUpdateInitializedEvent;

            /// <summary>
            /// Shows the progress of currently downloading update file
            /// </summary>
            /// <param name="oProgress"></param>
            public delegate void delUpdateProgress(cProgressArgs oProgress);
            public event delUpdateProgress oShowUpdateProgress;



            /// <summary>
            /// Shows the progress of Overall update download progress initialize stet
            /// </summary>
            /// <param name="oProgress"></param>
            public delegate void delUpdateTotalInitialized(cProgressArgs oProgress, bool PrintInNewLine);
            public event delUpdateTotalInitialized oShowUpdateTotalInitializedEvent;
            /// <summary>
            /// Shows the progress of Overall update download progress
            /// </summary>
            /// <param name="oProgress"></param>
            public delegate void delUpdateTotalProgress(cProgressArgs oProgress);
            public event delUpdateTotalProgress oShowUpdateTotalProgress;

            public delegate void delUpdateFinished(cProgressArgs oProgress, bool PrintInNewLine, bool bSuccess);
            public event delUpdateFinished oShowUpdateFinishedEvent;

            /// <summary>
            /// Shows the update install status
            /// </summary>
            /// <param name="sMessage"></param>
            /// <param name="PrintInNewLine"></param>
            public delegate void delUpdateInstallStatus(string sMessage, bool PrintInNewLine);
            public event delUpdateInstallStatus oShowUpdateInstallEvent;


            public bool _RaiseEvents = true;

            public List<cUpdateAvaiableDetails> LsLastUpdateCheckedStatus = new List<cUpdateAvaiableDetails>();

            public cPhoneNoDetails PhoneDetails = null;

            public cShowLogos showLogos = null;


            public cOfferDetails oOfferDetails = null;

            public class cUpdateAvaiableDetails
            {
                public bool isIniChecked { get; set; }
                public string IniFoundFromURL { get; set; }
                public string IniDetails { get; set; }
                public DateTime LastCheckTime { get; set; }
            }


            public cUpdator()
            {
                #region old code
                //             if (cGlobal.LOAD_CONFIGURATION.Equals(cClientEnum.eLoadConfiguration.BEST_APPLICATION_PERFORMANCE))
                //             {
                //                 Hs_Md5_DeletionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_FileSign_DeletionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_FilePaths_DeletionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_Folder_DeletionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_Cookies_DeletionList = new HashSet<cMalwareInfo<string>>();
                //                 Hs_Registry_DeletionList = new HashSet<cMalwareInfo<string>>();
                // 
                //                 Hs_Md5_AdditionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_FileSign_AdditionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_Folder_AdditionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_FilePath_AdditionList = new HashSet<cMalwareInfo<ulong>>();
                //                 Hs_Registry_AdditionList = new HashSet<cMalwareInfo<string>>();
                //                 Hs_Cookies_AdditionList = new HashSet<cMalwareInfo<string>>();
                //                
                //             }
                //             else
                //             {
                //                 Hs_Md5_DeletionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_FileSign_DeletionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_FilePaths_DeletionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_Folder_DeletionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_Cookies_DeletionList = new List<cMalwareInfo<string>>();
                //                 Hs_Registry_DeletionList = new List<cMalwareInfo<string>>();
                // 
                //                 Hs_Md5_AdditionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_FileSign_AdditionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_Folder_AdditionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_FilePath_AdditionList = new List<cMalwareInfo<ulong>>();
                //                 Hs_Registry_AdditionList = new List<cMalwareInfo<string>>();
                //                 Hs_Cookies_AdditionList = new List<cMalwareInfo<string>>();               
                // 
                //             }
                #endregion



                // oSqlFunc.CreateConnection(EnumUtils.DBFileNameValueOf(cClientEnum.eDbType.Spyware),true,false);
                //Initialize basic params
                m_ApplicationId = cGlobalSettings.GetApplicationVersion();
                //m_ProductName = cGlobalSettings.GetShortProductName();
                m_UpdateUrl = cGlobalSettings.GetUpdateUrl();




                //set global database date
                //  cGlobal.DATABASE_VERSION = m_CurrentUpdateVersion;
                // cGlobal.DATABASE_DATE = m_CurrentVersionDate;



                // m_UpdateParams = oUtils.GetUpdateParamsBasic(m_ApplicationId, m_ProductName, m_CurrentUpdateVersion);
                m_LocalSavePath = cGlobalSettings.GetUpdatePath();

                sPwd = "asp123cmdscan";
                m_Pause = false;
                m_Stop = false;

                //clean the last check for update if any
                LsLastUpdateCheckedStatus.Clear();
            }



            public cClientEnum.eUpdateStatus CheckForUpdate()
            {
                _RaiseEvents = true;
                return CheckForUpdate(_RaiseEvents);
            }

            public cClientEnum.eUpdateStatus CheckForUpdate(bool _RaiseEvents)
            {
                //_RaiseEvents = true;
                return CheckForUpdate(_RaiseEvents, cClientEnum.eCheckForUpdateType.Check_All_Servers);
            }





            public bool GetLastModifiedIniDetails(out string sIniDetails)
            {
                sIniDetails = string.Empty;
                bool RequestCompleted = false;
                try
                {

                    if (LsLastUpdateCheckedStatus.Count > 0)
                    {
                        if (DateTime.Now.Subtract(LsLastUpdateCheckedStatus[0].LastCheckTime).TotalMinutes < cGlobalSettings.CHECK_FOR_UPDATE_LAST_MODIFIED_INTERAVAL) //if last time of checking update is less then 5 min
                        {
                            sIniDetails = LsLastUpdateCheckedStatus[0].IniDetails;
                            RequestCompleted = true;

                        }
                        else
                        {
                            LsLastUpdateCheckedStatus.Clear();
                        }
                    }



                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("GetLastModifiedIniTime()|", ex);

                }
                return RequestCompleted;
            }

            public WebResponse GetResponseNoCache(Uri uri, int nTimeOutInMilliSecond)
            {
                WebRequest request = null;
                WebResponse response = null;
                try
                {
                    // Set a default policy level for the "http:" and "https" schemes.
                    // HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
                    // HttpWebRequest.DefaultCachePolicy = policy;
                    // Create the request.
                    request = WebRequest.Create(uri);
                    request.Proxy = WebProxy.GetDefaultProxy();// cGlobalSettings.oScanSettings.FetchProxySettings();

                    if (nTimeOutInMilliSecond > 0)
                    {
                        request.Timeout = nTimeOutInMilliSecond;
                    }
                    // Define a cache policy for this request only. 
                    HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                    request.CachePolicy = noCachePolicy;
                    response = request.GetResponse();
                    Console.WriteLine("IsFromCache? {0}", response.IsFromCache);
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cUpdator:: GetResponseNoCache:", ex);
                }

                return response;
            }

            [RegistryPermission(SecurityAction.Assert, Read = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework")]
            private static void InitializeFallbackSettings()
            {
                bool allowFallback = false;
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework"))
                    {
                        try
                        {
                            if (key.GetValueKind("LegacyWPADSupport") == RegistryValueKind.DWord)
                            {
                                allowFallback = ((int)key.GetValue("LegacyWPADSupport")) == 1;
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                        catch (IOException)
                        {
                        }
                    }
                }
                catch (SecurityException)
                {
                }
                catch (ObjectDisposedException)
                {
                }
            }

            public cClientEnum.eUpdateStatus CheckForUpdate(bool bRaiseEvents, cClientEnum.eCheckForUpdateType eCheckUpdateType)
            {

                try
                {
                    if (oShowUpdateInitializedEvent != null && bRaiseEvents) oShowUpdateInitializedEvent(string.Empty/*cResourceManager.LoadString("IDS_SE_CHECKING_FORUPDATE")*/, true);


                    LsUpdates.Clear();

                    // DownloadFile("http://updates4.systweak.com/aspnewsigupdatedownload/914/914completedatabase.zip",@"C:\ProgramData\Systweak\Advanced System Protector\updates\914completedatabase.zip");


                    //get current database version and create link
                    //GetDatabaseVersionDetails(out m_CurrentUpdateVersion, out m_CurrentVersionDate);
                    //m_UpdateParams = oUtils.GetUpdateParamsBasic(m_ApplicationId, m_ProductName, m_CurrentUpdateVersion);               
                    string ResultValue = string.Empty;

                    List<string> lsUpdateUrl = new List<string>();


                    lsUpdateUrl.Add(cGlobalSettings.GetUpdateUrl());
                    /*if (cGlobal.IS_LB_BUILD == 1)
                    {
                            lsUpdateUrl.Add(cGlobalSettings.GetUpdateUrl());
                    }
                    else
                    {
                        if (cGlobal.setupType == eSetupType.ADG_CAPMPAIGN)
                        {
                            lsUpdateUrl.Add(cGlobalSettings.GetUpdateUrl());
                        }
                        else
                        {
                            lsUpdateUrl = cGlobalSettings.GetUpdateUrlCollection(eCheckUpdateType);
                        }
                    }*/

                    bool RequestCompleted = GetLastModifiedIniDetails(out ResultValue);


                    if (!RequestCompleted)
                    {
                        for (int nCheckIniRetry = 0; nCheckIniRetry < cGlobalSettings.CHECK_FOR_UPDATE_INI_RETRY_COUNT; nCheckIniRetry++)
                        {

                            foreach (string sUrl in lsUpdateUrl)
                            {
                                if (DownloadWebPage(sUrl + m_UpdateParams, out ResultValue))
                                {
                                    //[to do:] add sleep 
                                    LsLastUpdateCheckedStatus.Clear();
                                    LsLastUpdateCheckedStatus.Add(new cUpdateAvaiableDetails() { isIniChecked = true, IniFoundFromURL = sUrl, IniDetails = ResultValue, LastCheckTime = DateTime.Now });
                                    RequestCompleted = true;
                                    break;
                                }
                            }
                            if (RequestCompleted) break;

                            Thread.Sleep(cGlobalSettings.CHECK_FOR_UPDATE_RETRY_INTERVAL); //sleep for 5 seconds before next retry
                        }
                    }



                    if (!RequestCompleted)
                    {
                        if (oShowUpdateFinishedEvent != null && bRaiseEvents) oShowUpdateFinishedEvent(new cProgressArgs() /*cResourceManager.LoadString("IDS_ERORR_UPDATE")*//*"Error Downloading Update."*/, true, false);
                        return cClientEnum.eUpdateStatus.ConnectionError;
                    }



                    string TempPathForUpdate = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Update.ini");
#if DEBUG
                    //   cGlobal.DATABASE_VERSION = 0;
#endif
                    FileStream oStream = new FileStream(TempPathForUpdate, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    StreamWriter oWriter = new StreamWriter(oStream);
                    oWriter.Write(ResultValue);
                    oWriter.Close(); oStream.Close();



                    ReadOfferSection(TempPathForUpdate);
                    //sTelePhoneNoFromINI = Convert.ToString(cWin32APIs.IniReadValue("application", ("telno"), TempPathForUpdate));
                    //update phone no  matter what the update contains
                    PhoneDetails = ReadPhoneSection(TempPathForUpdate);

                    showLogos = ReadRequiredValues(TempPathForUpdate);

                    ////Comment by Rahul Gupta to resolved Bug DPFW-365
                    //if (Program.bIsFirstInstall && !bRaiseEvents)
                    //    return cClientEnum.eUpdateStatus.UptoDate;


                    //first check for application update 
                    /************************************************************************/
                    /* [Application]
                    PROGRAM_VERSION=2.1.1.75
                    PROGRAM_URL=http://updates3.systweak.com/updatefiles/ASPSLUpdates_new/2.1.1.75/aspsetup.exe
                    PROGRAM_SIZE=4751472
                    Open_Browser=0
                    Key=aspsetup.exe
                    Hash=01427728DDDCD13C3ED73759DBFA0D9B403D8FD1 */
                    /************************************************************************/


                    string AppUpdate = string.Empty;
                    AppUpdate = cWin32APIs.IniReadValue("application", "program_version", TempPathForUpdate);

                    string strProductWillNeverExpire = cWin32APIs.IniReadValue("application", "expprdn", TempPathForUpdate);
                    string strReportExpireCurrentKeyReason = cWin32APIs.IniReadValue("application", "exprep", TempPathForUpdate);

                    string update_langcodes = cWin32APIs.IniReadValue("application", "langcodes", TempPathForUpdate); // comma separated lang code values
                    string str_considerlangcodes = cWin32APIs.IniReadValue("application", "b_uselangcodes", TempPathForUpdate);
                    bool b_considerlangcodes = false;
                    if (!string.IsNullOrEmpty(str_considerlangcodes))
                    {
                        int n_considerlangcodes = 0;
                        if (int.TryParse(str_considerlangcodes, out n_considerlangcodes))
                        {
                            b_considerlangcodes = n_considerlangcodes == 1 ? true : false;
                        }
                    }

                    if (!string.IsNullOrEmpty(strReportExpireCurrentKeyReason))
                    {
                    // KVO.bReportExpireCurrentKeyReason = (Convert.ToInt32(strReportExpireCurrentKeyReason) == 1);
                }

                /// check whether we have stopped the never expire
                if (!string.IsNullOrEmpty(strProductWillNeverExpire))
                    {
                    // expprdn => 1 => don't expire
                    // expprdn => 0 => will behaviour normal
                    // KeyValidator.bProductWillNeverExpire = !(Convert.ToInt32(strProductWillNeverExpire) == 0); // 0 => will expire in normal behaviour, // 1 for never expire - this is default for lifeboat

                    cGlobalSettings.oLogger1.WriteLog("Get Arm status");
                    // Program.GetArmStatus();
                }

                //int version = (new Version(AppUpdate).CompareTo(new Version(cGlobal.GetApplicationVersion())));

                Version currentVersion = new Version(cGlobalSettings.GetApplicationVersion());
                    Version UpdateVersion = new Version(string.IsNullOrEmpty(AppUpdate) ? "1.0.0.0" : AppUpdate);



                    if (UpdateVersion > currentVersion/*!string.IsNullOrEmpty(AppUpdate)*/)
                    {
                        cUpdateDetails oDetails = new cUpdateDetails();
                        //if (csOSProperties.OSId != csOSProperties.eOSType.Win10)
                        //{
                        //    oDetails.UpdateUrl = cWin32APIs.IniReadValue("application", "program_url452", TempPathForUpdate);
                        //}
                        //if (string.IsNullOrEmpty(oDetails.UpdateUrl))
                        //{
                        //    oDetails.UpdateUrl = cWin32APIs.IniReadValue("application", "program_url", TempPathForUpdate);
                        //}

                        oDetails.TotalSize = Convert.ToInt32(cWin32APIs.IniReadValue("application", "program_size", TempPathForUpdate));
                        oDetails.UpdateSha1 = Convert.ToString(cWin32APIs.IniReadValue("application", ("hash"), TempPathForUpdate));
                        oDetails.OpenInBrowser = Convert.ToInt32(cWin32APIs.IniReadValue("application", "open_browser", TempPathForUpdate));

                        int nMadatory = 0;
                        int.TryParse(cWin32APIs.IniReadValue("application", "is_mandatory", TempPathForUpdate), out nMadatory);
                        oDetails.IsMandatoryUpdate = nMadatory;

                        oDetails.UpdateType = (int)cClientEnum.eUpdateType.ApplicationUpdate; // 2 will denote application update
                        oDetails.Secquence = 1;

                        string sUpdateName = oDetails.UpdateUrl.Substring(oDetails.UpdateUrl.LastIndexOf("/") + 1);
                        if (!Directory.Exists(m_LocalSavePath))
                        {
                            Directory.CreateDirectory(m_LocalSavePath);
                        }
                        oDetails.LocalDownloadPath = Path.Combine(m_LocalSavePath, sUpdateName);
                        oDetails.UpdateName = sUpdateName;

                        /// condition to check
                        /// if update is not mandatory
                        /// and we want to release the update for certain languages only
                        /// 
                        if ((oDetails.IsMandatoryUpdate != 1) && b_considerlangcodes && !string.IsNullOrEmpty(str_considerlangcodes))
                        {
                            List<string> langCodeAbbvs = update_langcodes.Split(',').Select(x => x.Trim().ToLowerInvariant()).ToList();
                            if (!langCodeAbbvs.Contains(cGlobalSettings.oScanSettings.LanguageCode.ToLower()))
                            {
                                /// 
                                /// so if we have released the update only for English users, 
                                /// don't show update to other lang users
                                /// this is to do experiment on English users first, 
                                /// then try on others.
                                /// 
                                return cClientEnum.eUpdateStatus.UptoDate;
                            }
                        }


                        LsUpdates.Add(oDetails);
                        oDetails = null;

                    }

                    if (LsUpdates.Count > 0) //if update type is application then return app update else return db update
                    {
                        if (LsUpdates[0].UpdateType == 2)
                        {
                            if (LsUpdates[0].OpenInBrowser == 1)
                            {
                                return LsUpdates[0].IsMandatoryUpdate == 1 ? cClientEnum.eUpdateStatus.NewAppUpDateOpenBrowserMandatory : cClientEnum.eUpdateStatus.NewAppUpDateOpenBrowser;
                            }
                            else
                            {
                                return LsUpdates[0].IsMandatoryUpdate == 1 ? cClientEnum.eUpdateStatus.NewAppUpdateAvailableMandatory : cClientEnum.eUpdateStatus.NewAppUpdateAvailable;

                            }

                        }
                        else
                        {
                            return cClientEnum.eUpdateStatus.NewUpdateAvailable;
                        }
                    }
                    else
                    {
                        return cClientEnum.eUpdateStatus.UptoDate;
                    }
                }
                catch (System.Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("CheckForUpdate()|", oEx);
                    //oUtils.WriteToFile("CheckForUpdate()|" + oEx.Message);
                    return cClientEnum.eUpdateStatus.ConnectionError;
                }

                //return cClientEnum.eUpdateStatus.ConnectionError; //keep complier happy

            }

            /// <summary>
            /// sample format to be found on server 
            /// 
            ///   [telno]
            ///   telno=123-345-123
            ///
            ///   [telnolist]
            ///   1=123-345-123:245-123-123
            ///   2=245-123-123:345-123-123
            ///   3=345-123-123:445-123-123
            ///   4=423-345-123:545-123-123
            /// </summary>
            /// <param name="TempPathForUpdate"></param>
            /// <returns></returns>
            public cPhoneNoDetails ReadPhoneSection(string TempPathForUpdate)
            {

                cPhoneNoDetails oDetails = null;
                try
                {
                    oDetails = new cPhoneNoDetails();

                    cIniFile oIniReader = new cIniFile(TempPathForUpdate);



                    oDetails.DefaultPhoneNo = Convert.ToString(oIniReader.IniReadValue("telno", "telno"));

                    List<string> lsAllNo = oIniReader.GetSectionValuesAsList("telnolist").Select(x => x.Value).ToList();

                    if (lsAllNo.Count > 0)
                    {

                        oDetails.PhoneListForUpdate = new Dictionary<string, string>();
                        lsAllNo.ForEach(x =>
                        {
                            if (x.IndexOf(":") > -1)
                            {
                                string[] res = x.Split(':');

                                if (!oDetails.PhoneListForUpdate.ContainsKey(res[0]))
                                    oDetails.PhoneListForUpdate.Add(res[0], res[1]);
                            }

                        });
                    }



                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("ReadPhoneSection()|", ex);
                }
                return oDetails;

            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="TempPathForUpdate">INI downloaded path</param>
            /// <param name="strSection">INI saction name</param>
            /// <param name="strKey">Key value</param>
            /// <param name="regType">Data type of registry key </param>
            /// <param name="DefaultValue"> Default value if not able to read value from ini file</param>
            /// <param name="isFirstInstall">Add value in registry only on First Install.</param>
            void SetSettingsFromIni(string TempPathForUpdate, string strSection, string strKey, RegistryValueKind regType, object DefaultValue, bool isFirstInstall)
            {
                try
                {
                    if (isFirstInstall && !Program.bIsFirstInstall)
                    {
                        return;
                    }
                    cIniFile oIniReader = new cIniFile(TempPathForUpdate);

                    switch (regType)
                    {
                        case RegistryValueKind.String:
                        case RegistryValueKind.MultiString:
                        case RegistryValueKind.ExpandString:
                            {
                                string nDefaultValue = Convert.ToString(oIniReader.IniReadValue(strSection, strKey));
                                if (string.IsNullOrEmpty(nDefaultValue))
                                {
                                    nDefaultValue = Convert.ToString(DefaultValue);
                                }
                                cGlobalSettings.SetCommonAppDataRootRegistryValue(strKey, nDefaultValue, regType);
                                cGlobalSettings.SetUserAppDataRootRegistryValue(strKey, nDefaultValue, regType);
                            }
                            break;
                        case RegistryValueKind.Binary:
                        case RegistryValueKind.DWord:
                        case RegistryValueKind.QWord:
                            {
                                int nDefaultValue = (int)DefaultValue;
                                if (!int.TryParse(oIniReader.IniReadValue(strSection, strKey), out nDefaultValue))
                                {
                                    nDefaultValue = (int)DefaultValue;
                                }
                                cGlobalSettings.SetCommonAppDataRootRegistryValue(strKey, nDefaultValue, regType);
                                cGlobalSettings.SetUserAppDataRootRegistryValue(strKey, nDefaultValue, regType);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("SetSettingsFromIni:: ", ex);
                }
            }


            public cShowLogos ReadRequiredValues(string TempPathForUpdate)
            {
                cShowLogos oDetails = null;
                try
                {
                    oDetails = new cShowLogos();

                    cIniFile oIniReader = new cIniFile(TempPathForUpdate);

                    int nTelNeeded = 0;
                    if (int.TryParse(oIniReader.IniReadValue("application", "istelnoneeded"), out nTelNeeded))
                    {
                        oDetails.IsTelNoNeeded = nTelNeeded;
                    }

                    int nMSLogoNeeded = 0;
                    int.TryParse(cWin32APIs.IniReadValue("application", "ismslogoneeded", TempPathForUpdate), out nMSLogoNeeded);
                    oDetails.IsMSLogoNeeded = nMSLogoNeeded;


                    int nUseNotifier = 0;
                    int.TryParse(cWin32APIs.IniReadValue("application", "isusenotifier", TempPathForUpdate), out nUseNotifier);
                    oDetails.IsUseNotifier = nUseNotifier;


                /*string Countries =cWin32APIs.IniReadValue("application", "tel_countries", TempPathForUpdate);
                string[] lstCountries = Countries.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (lstCountries != null && lstCountries.Length > 0)
                {
                    oDetails.countriesTelNo = new Dictionary<string, string>();
                    foreach (string strCountry in lstCountries)
                    {
                        string telNo = cWin32APIs.IniReadValue("application", "tel_" + strCountry, TempPathForUpdate);
                        oDetails.countriesTelNo.Add("tel_" + strCountry, telNo);
                    }
                }*/

                /// Values updated only on FirstInstall
                /// 
                // SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_THEME_PROGRESS_WINDOW, RegistryValueKind.DWord, (int)cGlobalSettings.ThemeProgressWindow, true);

                /// Values can be updated anytime
                SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_SHOW_ALL_NAGS_VIA_NOTIFER, RegistryValueKind.DWord, 1, false);

                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_APP_TUTORIAL_LINK, RegistryValueKind.String, "", false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_CHANNEL_LINK, RegistryValueKind.String, "", false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_WATCH_TUTORIAL_TYPE, RegistryValueKind.DWord, (int)eWatchTutorialType.NoWhere, false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_SHOW_SUPPORT_EMAIL_FOOTER, RegistryValueKind.DWord, 0, false);
                    //SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_SHOW_ALL_NAGS_VIA_NOTIFER, RegistryValueKind.DWord, 1, false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_UPDATEOFFER_SHOW, RegistryValueKind.DWord, 1, false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_UPDATEOFFER_USERTYPE, RegistryValueKind.DWord, 1, false);

                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_GOOGLE_DRIVE_INFO_MSG, RegistryValueKind.DWord, 1, false);
                    SetSettingsFromIni(TempPathForUpdate, "application", cRegistrySettings.REGSTR_SEND_IP_PARAMS, RegistryValueKind.DWord, 0, false);
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("ReadPhoneSection()|", ex);
                }
                return oDetails;
            }


            /// <summary>
            /// will update telno in registry from update call this function only after calling check for update
            /// </summary>
            /// <returns></returns>
            public bool UpdateTelephoneNo(cPhoneNoDetails m_PhoneDetails)
            {
                bool changeFound = false;
                try
                {
                    if (m_PhoneDetails == null)
                    {
                        return false;
                    }

                    string sFinalPhoneNo = string.Empty;
                    string sPhoneNo = Convert.ToString(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_PHONENO, string.Empty));

                    //check if phone change is required
                    if (string.IsNullOrEmpty(sPhoneNo))
                    {
                        sFinalPhoneNo = m_PhoneDetails.DefaultPhoneNo; // if no nUMBER found then set it to default no
                    }
                    else
                    {
                        sFinalPhoneNo = m_PhoneDetails.DefaultPhoneNo;
                        if (m_PhoneDetails.PhoneListForUpdate != null && m_PhoneDetails.PhoneListForUpdate.Count > 0)
                        {
                            if (m_PhoneDetails.PhoneListForUpdate.ContainsKey(sPhoneNo))
                            {
                                sFinalPhoneNo = m_PhoneDetails.PhoneListForUpdate[sPhoneNo];
                            }
                        }


                    }

                    if (!string.IsNullOrEmpty(sFinalPhoneNo) && sPhoneNo != sFinalPhoneNo)
                    {
                        changeFound = true;
                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_PHONENO, sFinalPhoneNo, Microsoft.Win32.RegistryValueKind.String);
                        cGlobalSettings.SetUserAppDataRootRegistryValue(cRegistrySettings.REGSTR_PHONENO, sFinalPhoneNo, Microsoft.Win32.RegistryValueKind.String);

                    }
                }
                catch (System.Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("UpdateTelphoneNo()|", oEx);
                }
                return changeFound;
            }


            /// <summary>
            /// will update telno in registry from update call this function only after calling check for update
            /// </summary>
            /// <returns></returns>
            public bool UpdateVisableChanges(cShowLogos m_VisableDetails)
            {
                bool changeFound = false;
                try
                {
                    if (m_VisableDetails == null)
                    {
                        return false;
                    }

                    //int IsTelNoNeeded = m_VisableDetails.IsTelNoNeeded;

                    int IsTelNoNeeded = Convert.ToInt32(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_TEL_NO_NEEDED, 0));

                    int IsMSLogoNeeded = Convert.ToInt32(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_MS_LOGO_NEEDED, 0));

                    int IsUseNotifier = Convert.ToInt32(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_USE_NOTIFIER, 0));

                    /*if (IsTelNoNeeded > 0)
                    {
                        try
                        {
                            /// call API and get tel number 
                            /// 
                            string strFilePath = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "TelDetails.ini"); ;
                            AppFunctions.SaveResponeToFile("https://www.systweak.com/getcountrycode.asp?product=dpfw&pid=4", ref strFilePath);

                            string strTelNo = cWin32APIs.IniReadValue("status", "Telno", strFilePath);

                            if (!string.IsNullOrEmpty(strTelNo))
                            {
                                cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_PHONENO, strTelNo, Microsoft.Win32.RegistryValueKind.String);
                                //pnl_help.Visible = false;
                            }

                            if (strTelNo != cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_PHONENO, string.Empty))
                            {
                                changeFound = true;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }*/

                    if (IsTelNoNeeded != m_VisableDetails.IsTelNoNeeded || IsMSLogoNeeded != m_VisableDetails.IsMSLogoNeeded || IsUseNotifier != m_VisableDetails.IsUseNotifier)
                    //if (!string.IsNullOrEmpty(sFinalPhoneNo) && sPhoneNo != sFinalPhoneNo)
                    {
                        changeFound = true;
                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_TEL_NO_NEEDED, m_VisableDetails.IsTelNoNeeded, Microsoft.Win32.RegistryValueKind.DWord);
                        cGlobalSettings.SetUserAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_TEL_NO_NEEDED, m_VisableDetails.IsTelNoNeeded, Microsoft.Win32.RegistryValueKind.DWord);

                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_MS_LOGO_NEEDED, m_VisableDetails.IsMSLogoNeeded, Microsoft.Win32.RegistryValueKind.DWord);
                        cGlobalSettings.SetUserAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_MS_LOGO_NEEDED, m_VisableDetails.IsMSLogoNeeded, Microsoft.Win32.RegistryValueKind.DWord);

                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_USE_NOTIFIER, m_VisableDetails.IsUseNotifier, Microsoft.Win32.RegistryValueKind.DWord);
                        cGlobalSettings.SetUserAppDataRootRegistryValue(cRegistrySettings.REGSTR_IS_USE_NOTIFIER, m_VisableDetails.IsUseNotifier, Microsoft.Win32.RegistryValueKind.DWord);
                    }
                    else
                    {

                    }

                    /*if (m_VisableDetails.countriesTelNo != null && m_VisableDetails.countriesTelNo.Count > 0)
                    {
                        changeFound = true;
                        foreach (KeyValuePair<string, string> telList in m_VisableDetails.countriesTelNo)
                        {
                            cGlobalSettings.SetUserAppDataRootRegistryValue(telList.Key, telList.Value, Microsoft.Win32.RegistryValueKind.String);
                        }
                    }*/

                }
                catch (System.Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("UpdateTelphoneNo()|", oEx);
                }
                return changeFound;
            }

            public void ReadOfferSection(string TempPathForUpdate)
            {
                try
                {
                    const string strOffer = "offer"; // Section name
                    oOfferDetails = new cOfferDetails();

                    int EachIterationDelay = 1;
                    int.TryParse(cWin32APIs.IniReadValue(strOffer, "eachiterationdelay", TempPathForUpdate), out EachIterationDelay);
                    oOfferDetails.EachIterationDelay = EachIterationDelay;

                    DateTime ExpiresOn = DateTime.MaxValue;
                    DateTime.TryParse(cWin32APIs.IniReadValue(strOffer, "expireson", TempPathForUpdate), out ExpiresOn);
                    oOfferDetails.ExpiresOn = ExpiresOn;

                    if (cWin32APIs.IniReadValue(strOffer, "ishtml", TempPathForUpdate).Trim() == "0")
                        oOfferDetails.IsHTML = false;
                    else
                        oOfferDetails.IsHTML = true;

                    if (cWin32APIs.IniReadValue(strOffer, "ismultilangual", TempPathForUpdate).Trim() == "0")
                        oOfferDetails.IsMultiLangual = false;
                    else
                        oOfferDetails.IsMultiLangual = true;

                    oOfferDetails.OfferHtmlURL = cWin32APIs.IniReadValue(strOffer, "offerhtmlurl", TempPathForUpdate);

                    if (cWin32APIs.IniReadValue(strOffer, "showtounregisteredusersonly", TempPathForUpdate).Trim() == "0")
                        oOfferDetails.ShowToUnRegisteredUsersOnly = false;
                    else
                        oOfferDetails.ShowToUnRegisteredUsersOnly = true;

                    int TotalIterations = 1;
                    int.TryParse(cWin32APIs.IniReadValue(strOffer, "totaliterations", TempPathForUpdate), out TotalIterations);
                    oOfferDetails.TotalIterations = TotalIterations;

                    oOfferDetails.OfferContent_da = cWin32APIs.IniReadValue(strOffer, "offercontent_da", TempPathForUpdate);
                    oOfferDetails.OfferContent_de = cWin32APIs.IniReadValue(strOffer, "offercontent_de", TempPathForUpdate);
                    oOfferDetails.OfferContent_el = cWin32APIs.IniReadValue(strOffer, "offercontent_el", TempPathForUpdate);
                    oOfferDetails.OfferContent_en = cWin32APIs.IniReadValue(strOffer, "offercontent_en", TempPathForUpdate);
                    oOfferDetails.OfferContent_es = cWin32APIs.IniReadValue(strOffer, "offercontent_es", TempPathForUpdate);
                    oOfferDetails.OfferContent_fi = cWin32APIs.IniReadValue(strOffer, "offercontent_fi", TempPathForUpdate);
                    oOfferDetails.OfferContent_fr = cWin32APIs.IniReadValue(strOffer, "offercontent_fr", TempPathForUpdate);
                    oOfferDetails.OfferContent_it = cWin32APIs.IniReadValue(strOffer, "offercontent_it", TempPathForUpdate);
                    oOfferDetails.OfferContent_ja = cWin32APIs.IniReadValue(strOffer, "offercontent_ja", TempPathForUpdate);
                    oOfferDetails.OfferContent_ko = cWin32APIs.IniReadValue(strOffer, "offercontent_ko", TempPathForUpdate);
                    oOfferDetails.OfferContent_nl = cWin32APIs.IniReadValue(strOffer, "offercontent_nl", TempPathForUpdate);
                    oOfferDetails.OfferContent_no = cWin32APIs.IniReadValue(strOffer, "offercontent_no", TempPathForUpdate);
                    oOfferDetails.OfferContent_pl = cWin32APIs.IniReadValue(strOffer, "offercontent_pl", TempPathForUpdate);
                    oOfferDetails.OfferContent_pt = cWin32APIs.IniReadValue(strOffer, "offercontent_pt", TempPathForUpdate);
                    oOfferDetails.OfferContent_ptbr = cWin32APIs.IniReadValue(strOffer, "offercontent_ptbr", TempPathForUpdate);
                    oOfferDetails.OfferContent_ru = cWin32APIs.IniReadValue(strOffer, "offercontent_ru", TempPathForUpdate);
                    oOfferDetails.OfferContent_sv = cWin32APIs.IniReadValue(strOffer, "offercontent_sv", TempPathForUpdate);
                    oOfferDetails.OfferContent_tr = cWin32APIs.IniReadValue(strOffer, "offercontent_tr", TempPathForUpdate);
                    oOfferDetails.OfferContent_zhcn = cWin32APIs.IniReadValue(strOffer, "offercontent_zhcn", TempPathForUpdate);
                    oOfferDetails.OfferContent_zhtw = cWin32APIs.IniReadValue(strOffer, "offercontent_zhtw", TempPathForUpdate);

                    oOfferDetails.OfferRegKey = cWin32APIs.IniReadValue(strOffer, "offerregkey", TempPathForUpdate);
                    oOfferDetails.OfferProductFolderName = cWin32APIs.IniReadValue(strOffer, "offerproductfoldername", TempPathForUpdate);
                    oOfferDetails.OfferExename = cWin32APIs.IniReadValue(strOffer, "offerexename", TempPathForUpdate);
                    oOfferDetails.sregKeyValueInstallPath = cWin32APIs.IniReadValue(strOffer, "sregkeyvalueinstallpath", TempPathForUpdate);

                    //AppUpdate = cWin32APIs.IniReadValue("application", "program_version", TempPathForUpdate);
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cUpdator:: ReadOfferSection: ", ex);
                }
            }

            public cUpdateDetails GetDetailsFromINI(string section, string TempPathForUpdate, cClientEnum.eUpdateType nUpdateType /*int nUpdateType*/, int nIndex, int sequence)
            {
                cUpdateDetails oDetails = new cUpdateDetails();
                try
                {

                    oDetails.UpdateVersion = Convert.ToInt32(cWin32APIs.IniReadValue(section, ("version" + ((nIndex > 0) ? nIndex.ToString() : string.Empty)), TempPathForUpdate));
                    oDetails.UpdateUrl = cWin32APIs.IniReadValue(section, "url" + ((nIndex > 0) ? nIndex.ToString() : string.Empty), TempPathForUpdate);
                    oDetails.TotalSize = Convert.ToInt32(cWin32APIs.IniReadValue(section, ("size" + ((nIndex > 0) ? nIndex.ToString() : string.Empty)), TempPathForUpdate));
                    oDetails.UpdateType = (int)nUpdateType/* Convert.ToInt32(cWin32APIs.IniReadValue("database", ("completedatabase" + nVar), TempPathForUpdate))*/;
                    oDetails.UpdateSha1 = Convert.ToString(cWin32APIs.IniReadValue(section, ("hash" + ((nIndex > 0) ? nIndex.ToString() : string.Empty)), TempPathForUpdate));

                    oDetails.Secquence = sequence;

                    string sUpdateName = string.Empty;
                    sUpdateName = oDetails.UpdateUrl.Substring(oDetails.UpdateUrl.LastIndexOf("/") + 1);
                    if (!Directory.Exists(m_LocalSavePath))
                    {
                        Directory.CreateDirectory(m_LocalSavePath);
                    }
                    oDetails.LocalDownloadPath = Path.Combine(m_LocalSavePath, sUpdateName);
                    oDetails.UpdateName = sUpdateName;

                }
                catch (System.Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("GetDetailsFromINI()|", oEx);
                    //oUtils.WriteToFile("GetDetailsFromINI()|" + oEx.Message);
                }
                return oDetails;
            }



            /// <summary>
            /// Initializes Update download and wait for the result of downloads
            /// Displays Resulting  message if updates were downloaded and installed successfully or not
            /// </summary>
            public bool StartDownloadUpdates()
            {

                bool bResState = false;
                try
                {

                    // download all updates Sequentially and trap the result

                    if (oUpdateProgressClass == null)
                    {
                        oUpdateProgressClass = new cProgressArgs();
                    }

                    cClientEnum.eCheckForUpdateType oServerToCheckInRetry =
                        (LsLastUpdateCheckedStatus.Count > 0) ? cGlobalSettings.GetUrlServerType(LsLastUpdateCheckedStatus[0].IniFoundFromURL) : cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1;

                    if (LsUpdates.Count > 0)
                    {

                        if (LsUpdates[0].OpenInBrowser == 1)
                        {
                            Process.Start(LsUpdates[0].UpdateUrl);
                            bResState = true;
                        }
                        else
                        {
                            bool bRetry = false;

                            //main check for update loop
                            for (int nCheckUpdateDownloadCount = 1; nCheckUpdateDownloadCount <= cGlobalSettings.CHECK_FOR_UPDATE_RETRY_COUNT; nCheckUpdateDownloadCount++)
                            {


                                //inner downloader
                                for (int nRetryCount = 0; nRetryCount < cGlobalSettings.UPDATE_DOWNLOAD_RETRY_COUNT; nRetryCount++)
                                {
                                    if (LsUpdates.Count == 0)
                                    {
                                        bResState = false; //final download state
                                        bRetry = true; //retry state
                                        break; //if after retry we do not get any count then break; from inner loop
                                    }

                                    cClientEnum.eDownloadStatus oUpdateresult = DownloadUpdates();

                                    if (oUpdateProgressClass == null)
                                    {
                                        oUpdateProgressClass = new cProgressArgs();
                                    }
                                    switch (oUpdateresult)
                                    {
                                        case cClientEnum.eDownloadStatus.DownloadSucessFull:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_SE_UPDATE_SUCESS");//"Updates Downloaded and Installed Successfully";
                                            bResState = true; //final download state
                                            bRetry = false; //retry state
                                            break;
                                        case cClientEnum.eDownloadStatus.DownloadError:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_ERORR_UPDATE");// "Error Downloading Update Please Check the Internet Connection. View Error Log for more details";
                                            bRetry = true; //download was not success full so try again to download
                                            bResState = false;
                                            break;
                                        case cClientEnum.eDownloadStatus.PartialDownload:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_SE_UPDATE_PARTIAL");//"Updates Downloaded and Installed Partially .. try updating again";
                                            bRetry = true; //download was not success full so try again to download
                                            bResState = false;
                                            break;
                                        case cClientEnum.eDownloadStatus.DownloadAborted:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_SE_UPDATE_ABORTED");//"Update Aborted";
                                            bRetry = false; // no use of retry as download is manually aborted
                                            bResState = false;
                                            break;
                                        case cClientEnum.eDownloadStatus.InstallError:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_SE_UPDATE_ERR_UNKNOWN");// "Error Installing Updates.Try Updating Later";
                                            bRetry = false; // no use of retry as download was success but install failed
                                            bResState = false;
                                            break;
                                        case cClientEnum.eDownloadStatus.DownloadFailed_LowDiskSpace:
                                            //oUpdateProgressClass.CustomMessage = cResourceManager.LoadString("IDS_SE_UPDATE_ERR_LOWDISK");//"Download interrupted.\r\n There should be at least 400 MB of free disk space for downloading and installing the definitions."; ;
                                            bRetry = false; // no use of retry as space is not available
                                            break;
                                    }
                                    oUpdateProgressClass.DownloadStatus = oUpdateresult;
                                    oUpdateProgressClass.PrintOnNewLine = true;


                                    if (!bRetry)
                                    {
                                        break; //if retry is not required then break else try downloading again
                                    }
                                    cGlobalSettings.oLogger.WriteLog("StartDownloadUpdates()|retrying download update retry no :" + nRetryCount);
                                    //oUtils.WriteToFile("StartDownloadUpdates()|retrying download update retry no :" + nRetryCount);
                                    Thread.Sleep(cGlobalSettings.CHECK_FOR_UPDATE_RETRY_INTERVAL); // sleep for 5 second before next retry
                                }




                                if (bRetry && !bResState)//if after download retries also update is not downloaded check from alternate URL if the final state (bResState) is also false
                                {

                                    if (nCheckUpdateDownloadCount < cGlobalSettings.CHECK_FOR_UPDATE_RETRY_COUNT)
                                    {
                                        cGlobalSettings.oLogger.WriteLogVerbose("StartDownloadUpdates()|retrying check for update from alternate url : " + ((nCheckUpdateDownloadCount % 2 == 0) ? cClientEnum.eCheckForUpdateType.Check_Primary_Server.ToString() : cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1.ToString())); //trace only
                                                                                                                                                                                                                                                                                                                                            //just call check for update with alternate url so that the LsUpdates is updated with new values of alternate URL                                    

                                        try
                                        {
                                            //oServerToCheckInRetry = (oServerToCheckInRetry == cClientEnum.eCheckForUpdateType.Check_Primary_Server) ? cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1 : cClientEnum.eCheckForUpdateType.Check_Primary_Server;
                                            oServerToCheckInRetry = GetDownloadServer(oServerToCheckInRetry);

                                            if (LsLastUpdateCheckedStatus.Count > 0)
                                            {
                                                if (DateTime.Now.Subtract(LsLastUpdateCheckedStatus[0].LastCheckTime).TotalMinutes < cGlobalSettings.CHECK_FOR_UPDATE_LAST_MODIFIED_INTERAVAL)
                                                {
                                                    //oServerToCheckInRetry = oUtils.GetUrlServerType(LsLastUpdateCheckedStatus[0].IniFoundFromURL);                                               

                                                    //oServerToCheckInRetry = (oUtils.GetUrlServerType(LsLastUpdateCheckedStatus[0].IniFoundFromURL) == cClientEnum.eCheckForUpdateType.Check_Primary_Server) ? cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1 : cClientEnum.eCheckForUpdateType.Check_Primary_Server;
                                                    oServerToCheckInRetry = GetDownloadServer(cGlobalSettings.GetUrlServerType(LsLastUpdateCheckedStatus[0].IniFoundFromURL));
                                                }


                                                LsLastUpdateCheckedStatus.Clear();
                                            }


                                        }
                                        catch (System.Exception ex)
                                        {
                                            cGlobalSettings.oLogger.WriteLogException("StartDownloadUpdates()|checking LsLastUpdateCheckedStatus |", ex);
                                            //oUtils.WriteToFile("StartDownloadUpdates()|checking LsLastUpdateCheckedStatus |error: " + ex.Message); //trace only
                                        }

                                        CheckForUpdate(false, oServerToCheckInRetry);
                                    }
                                }
                                else //if retry is not required
                                {
                                    break;
                                }
                            }


                        }
                    }



                    if (oShowUpdateFinishedEvent != null && _RaiseEvents) oShowUpdateFinishedEvent(oUpdateProgressClass, oUpdateProgressClass.PrintOnNewLine, bResState);
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("|StartDownloadUpdates()|", ex);
                    // oUtils.WriteToFile("StartDownloadUpdates()|" + ex.Message);
                }
                return bResState;
            }


            public cClientEnum.eCheckForUpdateType GetDownloadServer(cClientEnum.eCheckForUpdateType eCurrentServer)
            {
                cClientEnum.eCheckForUpdateType oRes = cClientEnum.eCheckForUpdateType.Check_Alternate_Server_2;
                try
                {
                    switch (eCurrentServer)
                    {
                        case cClientEnum.eCheckForUpdateType.Check_All_Servers:
                            oRes = cClientEnum.eCheckForUpdateType.Check_Primary_Server;
                            break;
                        case cClientEnum.eCheckForUpdateType.Check_Primary_Server:
                            oRes = cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1;
                            break;
                        case cClientEnum.eCheckForUpdateType.Check_Alternate_Server_1:
                            oRes = cClientEnum.eCheckForUpdateType.Check_Alternate_Server_2;
                            break;
                        case cClientEnum.eCheckForUpdateType.Check_Alternate_Server_2:
                            oRes = cClientEnum.eCheckForUpdateType.Check_Primary_Server;
                            break;
                        default:
                            oRes = cClientEnum.eCheckForUpdateType.Check_All_Servers;
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cUpdator::GetDownloadServer", ex);
                }
                return oRes;
            }

            /// <summary>
            /// This Function will Download all the Updates which are present in the download list (array)
            /// </summary>
            /// <returns></returns>
            private cClientEnum.eDownloadStatus DownloadUpdates()
            {
                cClientEnum.eDownloadStatus eReturnValue = cClientEnum.eDownloadStatus.DownloadSucessFull;
                try
                {
                    if (m_Stop)
                    {
                        if (oShowUpdateFinishedEvent != null) oShowUpdateFinishedEvent(oUpdateProgressClass, true, true);

                        return cClientEnum.eDownloadStatus.DownloadAborted;
                    }

                    if (LsUpdates.Count == 0) return cClientEnum.eDownloadStatus.DownloadError;

                    //check for disk space availability
                    if (!cWin32APIs.IsMinDiskAvalaible(Directory.GetDirectoryRoot(cGlobalSettings.getAppDataApplicationPath()))) return cClientEnum.eDownloadStatus.DownloadFailed_LowDiskSpace;


                    //raise event for total update progress so that first time the progbar is not shown empty
                    ExposeTotalEventInitializeProperties(string.Empty/*string.Format(cResourceManager.LoadString("IDS_SE_UPDATE_DOWNLOADING")/ *"Downloading Update ({0}/{1})"* /, 1, LsUpdates.Count)*/, LsUpdates[0].UpdateName, 100, 2, true);


                    filesRead = 0;
                    for (int i = 0; i < LsUpdates.Count; i++)
                    {

                        if (m_Stop)
                        {
                            if (oShowUpdateFinishedEvent != null) oShowUpdateFinishedEvent(oUpdateProgressClass, true, true);

                            return cClientEnum.eDownloadStatus.DownloadAborted;
                        }



                        filesRead++; // updating files download count


                        //m_totalBytes = /*m_totalReceived =*/ 0;

                        cClientEnum.eDownloadStatus enumStatus = cClientEnum.eDownloadStatus.DownloadError;

                        if (LsUpdates.Count > 1 && i > 0) //
                        {
                            enumStatus = cClientEnum.eDownloadStatus.PartialDownload;
                        }

                        string downloadLink = LsUpdates[i].UpdateUrl;

                        int nDownloadedCount = 0;



                        if (!CheckDownloadedUpdate(LsUpdates[i]))
                        {
                            //#if !DEBUG
                            //                        enumStatus = DownloadFile(downloadLink, LsUpdates[i].LocalDownloadPath, ref nDownloadedCount, LsUpdates[i].UpdateName, LsUpdates[i]);
                            //#else
                            // enumStatus = DownloadFile2(downloadLink, LsUpdates[i].LocalDownloadPath, ref nDownloadedCount, LsUpdates[i].UpdateName, LsUpdates[i]);
                            enumStatus = DownloadFileWithResumeSupport(downloadLink, LsUpdates[i].LocalDownloadPath, ref nDownloadedCount, LsUpdates[i].UpdateName, LsUpdates[i], false);
                            //#endif
                        }
                        else
                        {
                            nDownloadedCount = LsUpdates[i].TotalSize; // because if total update is downloaded properly earlier then do not download the file again and set the total downloaded size as completed
                            enumStatus = cClientEnum.eDownloadStatus.DownloadSucessFull;
                        }

                        ExposeTotalEventProperties(string.Empty/*string.Format(cResourceManager.LoadString("IDS_SE_UPDATE_DOWNLOADING")/ *"Downloading Update ({0}/{1})"* /, filesRead, LsUpdates.Count)*/, LsUpdates[i].UpdateName, LsUpdates.Count, filesRead, true);


                        if (enumStatus.Equals(cClientEnum.eDownloadStatus.DownloadSucessFull))
                        {
                            //if (LsUpdates[i].TotalSize == nDownloadedCount) //old check on basis of size
                            if (LsUpdates[i].UpdateType != 3)
                            {

                                if (CheckDownloadedUpdate(LsUpdates[i]))
                                {
                                    LsUpdates[i].DownloadedSize = nDownloadedCount;
                                }
                                else //if download size does not match then stop further download and install till here only
                                {
                                    eReturnValue = cClientEnum.eDownloadStatus.PartialDownload;
                                    break; // do not download further  install till the last point only
                                }
                            }
                            else
                            {
                                //check removed as it is not sure that application update of other type works fine
                                // if (LsUpdates[i].TotalSize == nDownloadedCount)
                                {
                                    LsUpdates[i].DownloadedSize = LsUpdates[i].TotalSize;//nDownloadedCount;                                
                                }
                                //  else
                                // {
                                //     eReturnValue = cClientEnum.eDownloadStatus.PartialDownload;
                                //     break; // do not download further  install till the last point only
                                // }
                            }
                        }
                        else
                        {
                            return enumStatus;
                        }

                    }
                    //install updates if download successfully
                    if (!InstallDatabaseUpdates())
                    {
                        //if (oShowUpdateFinishedEvent != null) oShowUpdateFinishedEvent(cClientEnum.eDownloadStatus.InstallError, true, false);
                        return cClientEnum.eDownloadStatus.InstallError;
                    }

                }
                catch (Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("DownloadUpdates()|", oEx);
                    // oUtils.WriteToFile("DownloadUpdates()|" + oEx.Message);
                    return cClientEnum.eDownloadStatus.DownloadError;
                }
                return eReturnValue;

            }

            /// <summary>
            /// check if update file is already present in update folder if yes compares the sha with the live sha if sha matches dont download existing update
            /// </summary>
            /// <param name="oCurrentUpdateFile"></param>
            /// <param name="sLocalDownloadFile"></param>
            public bool CheckDownloadedUpdate(cUpdateDetails oCurrentUpdateFile)
            {
                bool bRes = false;
                try
                {
                    if (File.Exists(oCurrentUpdateFile.LocalDownloadPath))
                    {
                        if (cGlobalSettings.oFileHashAlgorithm.getSha128(oCurrentUpdateFile.LocalDownloadPath).ToLower() == oCurrentUpdateFile.UpdateSha1.ToLower())
                        {
                            bRes = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("CheckDownloadedUpdate |", ex);

                }
                return bRes;
            }

            /// <summary>
            /// Private method to download the file. Reference        
            /// </summary>
            /// <param name="downloadFileName">The sting containing both 
            /// the URL text and the file name.</param>
            /// <param name="localFileName">The string containing both the 
            /// local path and the file name.</param>
            private cClientEnum.eDownloadStatus DownloadFile(string downloadFileName, string localFileName, ref int DownloadedSize, string UpdateName, cUpdateDetails oDetails)
            {

                while (Pause) Thread.Sleep(200);

                if (m_Stop)
                {
                    // UpdateStatusBox(Properties.Resources.STR_MSG_DOWNLOAD_INTRRUPED);
                    return cClientEnum.eDownloadStatus.DownloadAborted;
                }
                // The streams of data retrieved from the web server
                Stream streamResponse = null;
                // The file stream
                Stream fileStream = null;
                // The request to the web server for file information
                //HttpWebRequest webRequest = null;
                // The response from the web server containing information about the file
                HttpWebResponse webResponse = null;

                using (WebClient wcDownload = new WebClient())
                {
                    try
                    {
                        // Show the downloading file's name...
                        // UpdateStatusBox("Initializing Download for : " + updateName + "...");


                        /************************************************************************/
                        /* //  [2/1/2013 abhinav]      added later for resume suppport          */
                        /************************************************************************/

                        //long iExistLen = 0;

                        //if (System.IO.File.Exists(localFileName))
                        //{
                        //    System.IO.FileInfo fINfo =
                        //       new System.IO.FileInfo(localFileName);
                        //    iExistLen = fINfo.Length;
                        //}
                        //if (iExistLen > 0)
                        //{
                        //    fileStream = new System.IO.FileStream(localFileName,
                        //      System.IO.FileMode.Append, System.IO.FileAccess.Write,
                        //      System.IO.FileShare.ReadWrite);
                        //}
                        //else
                        //{
                        //    fileStream = new System.IO.FileStream(localFileName,
                        //      System.IO.FileMode.Create, FileAccess.Write, FileShare.None);
                        //}


                        // Create a request to the file we are downloading

                        /***********************now using nocache proxy************************************************


                        webRequest = (HttpWebRequest)WebRequest.Create(downloadFileName);

                        //webRequest.AddRange((int)iExistLen); //  [2/1/2013 abhinav] for resume support


                        // Set default authentication for retrieving the file
                      //  webRequest.Credentials = CredentialCache.DefaultCredentials;
                        webRequest.Proxy = cGlobal.oScanSettings.LoadProxySettings();
                        webRequest.Timeout = 500;
                        webRequest.ServicePoint.Expect100Continue = false;
                       // webRequest.KeepAlive = false;
                        // Retrieve the response from the server
                        webResponse = (HttpWebResponse)webRequest.GetResponse();

                        ***********************************************************************/
                        webResponse = (HttpWebResponse)GetResponseNoCache(new Uri(downloadFileName), 20000); //20 sec 

                        // Ask the server for the file size and store it
                        Int64 fileSize = webResponse.ContentLength;
                        //  webRequest.AddRange(3208012, (int)fileSize - 3208012);
                        // Open the URL for download 

                        wcDownload.Proxy = WebProxy.GetDefaultProxy();/*cGlobalSettings.oScanSettings.FetchProxySettings()*/;

                        streamResponse = wcDownload.OpenRead(downloadFileName);

                        // Create a new file stream where we will be saving the data (local drive)
                        fileStream = new FileStream(localFileName, FileMode.Create,
                            FileAccess.Write, FileShare.None);

                        // fileStream.Position = 36524;//iExistLen;

                        // It will store the current number of bytes we retrieved from the server
                        int bytesSize = 0;
                        //DownloadedSize = (int)iExistLen;
                        Double totalSizeDownloaded =  /*iExistLen*/0;
                        Double nDownloadPercentage = 0;
                        // A buffer for storing and writing the data retrieved from the server
                        byte[] downBuffer = new byte[2048 * 8]; //2*8= 16 kb


                        // Loop through the buffer until the buffer is empty
                        while ((bytesSize = streamResponse.Read(
                            downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            while (Pause) Thread.Sleep(200);

                            if (m_Stop)
                            {
                                return cClientEnum.eDownloadStatus.DownloadAborted;
                            }

                            if (totalSizeDownloaded > 0)
                            {
                                nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);
                            }

                            if (oShowUpdateProgress != null)
                            {
                                // if (nDownloadPercentage % 2 == 0 )
                                {
                                    //                                 ExposeEventProperties(string.Empty/*string.Format(cResourceManager.LoadString(oDetails.UpdateType==2 ?"IDS_SE_UPDATE_DOWNLOAD_APP_PROG": "IDS_SE_UPDATE_DOWNLOAD_PROG")/ *"Downloading Update ({0} KB of {1} KB) Total : {2}%"* /, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage)*/
                                    //                                     , downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);


                                    ExposeEventProperties(string.Empty/*string.Format(cResourceManager.LoadString(oDetails.UpdateType==2 ?"IDS_SE_UPDATE_DOWNLOAD_APP_PROG": "IDS_SE_UPDATE_DOWNLOAD_PROG")/ *"Downloading Update ({0} KB of {1} KB) Total : {2}%"* /, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage)*/
                                      , downloadFileName, totalSizeDownloaded, fileSize, false, nDownloadPercentage);
                                }
                            }


                            // Write the data from the buffer to the local hard drive
                            fileStream.Write(downBuffer, 0, bytesSize);
                            totalSizeDownloaded += bytesSize;

                            DownloadedSize += bytesSize;

                        }

                        if (oShowUpdateProgress != null)
                        {
                            nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);

                            //  ExposeEventProperties(string.Empty/*string.Format(cResourceManager.LoadString("IDS_SE_UPDATE_DOWNLOAD_PROG")/ *"Downloading Update ({0} KB of {1} KB) Total : {2}%"* /, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage)*/
                            //      , downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);

                            ExposeEventProperties(string.Empty
                                , downloadFileName, totalSizeDownloaded, fileSize, false, nDownloadPercentage);


                            // ExposeEventProperties("Download complete...", true);
                        }

                    }
                    catch (Exception oEx)
                    {
                        cGlobalSettings.oLogger.WriteLogException("DonwloadFile()||", oEx);
                        //oUtils.WriteToFile(string.Format("{0}|DonwloadFile()|error:{1}",this, oEx.ToString()));
                        return cClientEnum.eDownloadStatus.DownloadError;
                    }
                    finally
                    {
                        if (webResponse != null)
                        {
                            webResponse.Close();
                        }
                        if (streamResponse != null)
                        {
                            // When the above code has ended, close the streams
                            streamResponse.Close();
                        }
                        if (fileStream != null)
                        {
                            fileStream.Close();
                        }
                    }

                }
                return cClientEnum.eDownloadStatus.DownloadSucessFull; // if no error occured

            }


#if DEBUG1
        private cClientEnum.eDownloadStatus DownloadFile2(string downloadFileName, string localFileName, ref int DownloadedSize, string UpdateName, cUpdateDetails oDetails)
        {

            WebClient wcDownload = new WebClient();
            wcDownload.Proxy = cGlobalSettings.oScanSettings.FetchProxySettings();
            wcDownload.DownloadProgressChanged+=new DownloadProgressChangedEventHandler(wcDownload_DownloadProgressChanged);
            wcDownload.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wcDownload_DownloadFileCompleted);
            wcDownload.DownloadFileAsync(new Uri(downloadFileName), localFileName);

            while (wcDownload.IsBusy)
            {
                Thread.Sleep(100);
            }
            //while (Pause) Thread.Sleep(200);

            //if (m_Stop)
            //{
            //    // UpdateStatusBox(Properties.Resources.STR_MSG_DOWNLOAD_INTRRUPED);
            //    return cClientEnum.eDownloadStatus.DownloadAborted;
            //}
            //// The streams of data retrieved from the web server
            //Stream streamResponse = null;
            //// The file stream
            //Stream fileStream = null;
            //// The request to the web server for file information
            //HttpWebRequest webRequest = null;
            //// The response from the web server containing information about the file
            //HttpWebResponse webResponse = null;

            //using (WebClient wcDownload = new WebClient())
            //{
            //    try
            //    {
            //        // Show the downloading file's name...
            //        // UpdateStatusBox("Initializing Download for : " + updateName + "...");

            //        // Create a request to the file we are downloading
            //        webRequest = (HttpWebRequest)WebRequest.Create(downloadFileName);

            //        // Set default authentication for retrieving the file
            //        //  webRequest.Credentials = CredentialCache.DefaultCredentials;
            //        webRequest.Proxy = cGlobal.oScanSettings.LoadProxySettings();
            //        webRequest.Timeout = 500;
            //        webRequest.ServicePoint.Expect100Continue = false;
            //        webRequest.KeepAlive = false;
            //        // Retrieve the response from the server
            //        webResponse = (HttpWebResponse)webRequest.GetResponse();


            //        // Ask the server for the file size and store it
            //        Int64 fileSize = webResponse.ContentLength;
            //        //  webRequest.AddRange(3208012, (int)fileSize - 3208012);
            //        // Open the URL for download 

            //        wcDownload.Proxy = cGlobal.oScanSettings.LoadProxySettings();

            //        streamResponse = wcDownload.OpenRead(downloadFileName);

            //        // Create a new file stream where we will be saving the data (local drive)
            //        fileStream = new FileStream(localFileName, FileMode.Create,
            //            FileAccess.Write, FileShare.None);

            //        // fileStream.Position = 3208012;

            //        // It will store the current number of bytes we retrieved from the server
            //        int bytesSize = 0;
            //        Double totalSizeDownloaded = 0;
            //        Double nDownloadPercentage = 0;
            //        // A buffer for storing and writing the data retrieved from the server
            //        byte[] downBuffer = new byte[2048 * 8]; //2*8= 16 kb


            //        // Loop through the buffer until the buffer is empty
            //        while ((bytesSize = streamResponse.Read(
            //            downBuffer, 0, downBuffer.Length)) > 0)
            //        {
            //            while (Pause) Thread.Sleep(200);

            //            if (m_Stop)
            //            {
            //                return cClientEnum.eDownloadStatus.DownloadAborted;
            //            }

            //            if (totalSizeDownloaded > 0)
            //            {
            //                nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);
            //            }

            //            if (oShowUpdateProgress != null)
            //            {
            //                // if (nDownloadPercentage % 2 == 0 )
            //                {
            //                    ExposeEventProperties(string.Format(cResourceManager.LoadString(oDetails.UpdateType == 2 ? "IDS_SE_UPDATE_DOWNLOAD_APP_PROG" : "IDS_SE_UPDATE_DOWNLOAD_PROG")/*"Downloading Update ({0} KB of {1} KB) Total : {2}%"*/, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage), downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);

            //                }
            //            }


            //            // Write the data from the buffer to the local hard drive
            //            fileStream.Write(downBuffer, 0, bytesSize);
            //            totalSizeDownloaded += bytesSize;

            //            DownloadedSize += bytesSize;

            //        }

            //        if (oShowUpdateProgress != null)
            //        {
            //            nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);

            //            ExposeEventProperties(string.Format(cResourceManager.LoadString("IDS_SE_UPDATE_DOWNLOAD_PROG")/*"Downloading Update ({0} KB of {1} KB) Total : {2}%"*/, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage), downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);
            //            // ExposeEventProperties("Download complete...", true);
            //        }

            //    }
            //    catch (Exception oEx)
            //    {
            //        oUtils.WriteToFile(string.Format("{0}|DonwloadFile()|error:{1}", this, oEx.ToString()));
            //        return cClientEnum.eDownloadStatus.DownloadError;
            //    }
            //    finally
            //    {
            //        if (webResponse != null)
            //        {
            //            webResponse.Close();
            //        }
            //        if (streamResponse != null)
            //        {
            //            // When the above code has ended, close the streams
            //            streamResponse.Close();
            //        }
            //        if (fileStream != null)
            //        {
            //            fileStream.Close();
            //        }
            //    }

            //}
            return cClientEnum.eDownloadStatus.DownloadSucessFull; // if no error occured

        }

        void wcDownload_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
        }

        void wcDownload_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (oShowUpdateProgress != null)
            {
                ExposeEventProperties( e.ProgressPercentage.ToString(),"downloading ",100,e.ProgressPercentage,false, e.ProgressPercentage);
            }
        }
              
#endif

            /// <summary>
            /// Private method to download the file. Reference        
            /// </summary>
            /// <param name="downloadFileName">The sting containing both 
            /// the URL text and the file name.</param>
            /// <param name="localFileName">The string containing both the 
            /// local path and the file name.</param>
            private cClientEnum.eDownloadStatus DownloadFileWithResumeSupport(string downloadFileName, string localFileName, ref int DownloadedSize, string UpdateName, cUpdateDetails oDetails, bool bIsResumeEnabled = true)
            {
                bool bDeleteExistingFile = false;

                while (Pause) Thread.Sleep(200);

                if (m_Stop)
                {
                    // UpdateStatusBox(Properties.Resources.STR_MSG_DOWNLOAD_INTRRUPED);
                    return cClientEnum.eDownloadStatus.DownloadAborted;
                }
                // The streams of data retrieved from the web server
                Stream streamResponse = null;
                // The file stream
                Stream fileStream = null;
                // The request to the web server for file information
                HttpWebRequest webRequest = null;
                // The response from the web server containing information about the file
                HttpWebResponse webResponse = null;

                //  using (WebClient wcDownload = new WebClient())
                //  {
                try
                {
                    // Show the downloading file's name...
                    // UpdateStatusBox("Initializing Download for : " + updateName + "...");


                    /************************************************************************/
                    /* //  [2/1/2013 abhinav]      added later for resume suppport          */
                    /************************************************************************/

                    //   DownloadFile(downloadFileName, localFileName);

                    long iExistLen = 0;

                    if (bIsResumeEnabled == false)
                        if (System.IO.File.Exists(localFileName))
                        {
                            File.Delete(localFileName);
                        }

                    if (System.IO.File.Exists(localFileName))
                    {
                        System.IO.FileInfo fINfo =
                           new System.IO.FileInfo(localFileName);
                        iExistLen = fINfo.Length;
                        fINfo = null;
                    }
                    if (iExistLen > 0 && iExistLen <= oDetails.TotalSize && bIsResumeEnabled)
                    {
                        fileStream = new System.IO.FileStream(localFileName,
                          System.IO.FileMode.Append, System.IO.FileAccess.Write,
                          System.IO.FileShare.ReadWrite);
                    }
                    else
                    {
                        iExistLen = 0;
                        fileStream = new System.IO.FileStream(localFileName,
                          System.IO.FileMode.Create, FileAccess.Write, FileShare.None);
                    }


                    // Create a request to the file we are downloading

                    /***********************now using nocache proxy*************************************************/

                    //                     System.Net.HttpWebRequest hwRq;
                    //                     System.Net.HttpWebResponse hwRes;
                    //                     hwRq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(downloadFileName);
                    // 
                    //                     if (iExistLen > 0) hwRq.AddRange((int)iExistLen);
                    //                     System.IO.Stream smRespStream;
                    //                     hwRes = (System.Net.HttpWebResponse)hwRq.GetResponse();
                    //                     smRespStream = hwRes.GetResponseStream();
                    // 
                    //                     long iFileSize = hwRes.ContentLength;


                    webRequest = (HttpWebRequest)WebRequest.Create(downloadFileName);

                    if (iExistLen > 0) webRequest.AddRange((int)iExistLen); //  [2/1/2013 abhinav] for resume support


                    // Set default authentication for retrieving the file                 
                    // webRequest.Proxy = cGlobal.oScanSettings.LoadProxySettings();
                    //  webRequest.Timeout = 500;
                    // webRequest.ServicePoint.Expect100Continue = false;
                    // webRequest.KeepAlive = false;
                    // Retrieve the response from the server
                    webResponse = (HttpWebResponse)webRequest.GetResponse();

                    streamResponse = webResponse.GetResponseStream();
                    /************************************************************************/
                    // webResponse = (HttpWebResponse)GetResponseNoCache(new Uri(downloadFileName), 20000); //20 sec 

                    // Ask the server for the file size and store it
                    Int64 fileSize = webResponse.ContentLength;


                    // webRequest.AddRange((int)iExistLen, (int)fileSize - (int)iExistLen);
                    // Open the URL for download 



                    // wcDownload.Proxy = cGlobal.oScanSettings.LoadProxySettings();
                    //  streamResponse = wcDownload.OpenRead(downloadFileName);

                    // Create a new file stream where we will be saving the data (local drive)
                    // fileStream = new FileStream(localFileName, FileMode.Create,
                    //    FileAccess.Write, FileShare.None);

                    // fileStream.Position = 36524;//iExistLen;

                    // It will store the current number of bytes we retrieved from the server
                    int bytesSize = 0;
                    //DownloadedSize = (int)iExistLen;
                    Double totalSizeDownloaded =  /*iExistLen*/0;
                    Double nDownloadPercentage = 0;
                    // A buffer for storing and writing the data retrieved from the server
                    byte[] downBuffer = new byte[2048 * 8]; //2*8= 16 kb


                    // Loop through the buffer until the buffer is empty
                    while ((bytesSize = streamResponse.Read(
                        downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        while (Pause) Thread.Sleep(200);

                        if (m_Stop)
                        {
                            return cClientEnum.eDownloadStatus.DownloadAborted;
                        }

                        if (totalSizeDownloaded > 0)
                        {
                            nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);
                        }

                        if (oShowUpdateProgress != null)
                        {
                            // if (nDownloadPercentage % 2 == 0 )
                            {
                                //                             ExposeEventProperties(string.Empty/*string.Format(cResourceManager.LoadString(oDetails.UpdateType == 2 ? "IDS_SE_UPDATE_DOWNLOAD_APP_PROG" : "IDS_SE_UPDATE_DOWNLOAD_PROG")/ *"Downloading Update ({0} KB of {1} KB) Total : {2}%"* /, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage)*/,
                                //                                 downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);

                                ExposeEventProperties(string.Empty
                              , downloadFileName, totalSizeDownloaded, fileSize, false, nDownloadPercentage);

                            }
                        }


                        // Write the data from the buffer to the local hard drive
                        fileStream.Write(downBuffer, 0, bytesSize);
                        totalSizeDownloaded += bytesSize;

                        DownloadedSize += bytesSize;

                    }

                    if (oShowUpdateProgress != null)
                    {
                        nDownloadPercentage = Math.Round((((double)DownloadedSize * (100 / (double)fileSize))), 1);

                        //ExposeEventProperties(string.Empty/*string.Format(cResourceManager.LoadString("IDS_SE_UPDATE_DOWNLOAD_PROG")/ *"Downloading Update ({0} KB of {1} KB) Total : {2}%"* /, Math.Round(totalSizeDownloaded / 1024, 1), Math.Round((double)fileSize / 1024, 1), nDownloadPercentage)*/
                        //    , downloadFileName, Convert.ToInt32(Math.Round((double)fileSize / 1024, 1)), Convert.ToInt32(Math.Round(totalSizeDownloaded / 1024, 1)), false);

                        ExposeEventProperties(string.Empty
                             , downloadFileName, totalSizeDownloaded, fileSize, false, nDownloadPercentage);

                        // ExposeEventProperties("Download complete...", true);
                    }

                }
                catch (WebException oEx)
                {
                    if (oEx.Status == WebExceptionStatus.ProtocolError)
                    {
                        bDeleteExistingFile = true;
                    }
                    cGlobalSettings.oLogger.WriteLogException("DonwloadFile()|WebException|", oEx);

                    return cClientEnum.eDownloadStatus.DownloadError;
                }
                catch (Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("DonwloadFile()|error|", oEx);

                    return cClientEnum.eDownloadStatus.DownloadError;
                }
                finally
                {
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                    if (streamResponse != null)
                    {
                        // When the above code has ended, close the streams
                        streamResponse.Close();
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }

                    if (bDeleteExistingFile)
                    {
                        if (File.Exists(localFileName))
                        {
                            File.Delete(localFileName);
                        }
                    }

                }

                // }
                return cClientEnum.eDownloadStatus.DownloadSucessFull; // if no error occured

            }
            public void DownloadFile(string sSourceURL, string sDestinationPath)
            {
                long iFileSize = 0;
                int iBufferSize = 1024;
                iBufferSize *= 1000;
                long iExistLen = 0;
                System.IO.FileStream saveFileStream;
                if (System.IO.File.Exists(sDestinationPath))
                {
                    System.IO.FileInfo fINfo =
                       new System.IO.FileInfo(sDestinationPath);
                    iExistLen = fINfo.Length;
                }
                if (iExistLen > 0)
                    saveFileStream = new System.IO.FileStream(sDestinationPath,
                      System.IO.FileMode.Append, System.IO.FileAccess.Write,
                      System.IO.FileShare.ReadWrite);
                else
                    saveFileStream = new System.IO.FileStream(sDestinationPath,
                      System.IO.FileMode.Create, System.IO.FileAccess.Write,
                      System.IO.FileShare.ReadWrite);

                System.Net.HttpWebRequest hwRq;
                System.Net.HttpWebResponse hwRes;
                hwRq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(sSourceURL);
                hwRq.AddRange((int)iExistLen);
                System.IO.Stream smRespStream;
                hwRes = (System.Net.HttpWebResponse)hwRq.GetResponse();
                smRespStream = hwRes.GetResponseStream();

                iFileSize = hwRes.ContentLength;

                int iByteSize;
                byte[] downBuffer = new byte[iBufferSize];

                while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    // if (bStop)
                    {
                        // DisplayStatus(iByteSize.ToString());
                        //  break;
                    }
                    saveFileStream.Write(downBuffer, 0, iByteSize);

                    // DisplayStatus("downloaded " + iByteSize.ToString());
                }
                saveFileStream.Close();
            }



            /// <summary>
            /// Returns the content of a given web address as string.
            /// </summary>
            /// <param name="Url">URL of the webpage</param>
            /// <returns>Website content</returns>
            public bool DownloadWebPage(string Url, out string PageContent)
            {
                PageContent = string.Empty;


                try
                {
                    // Open a connection
                    //                 HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);                
                    //                 WebRequestObject.Proxy = cGlobal.oScanSettings.LoadProxySettings();  //this will take default IE proxy
                    // 
                    //                 
                    //                 WebRequestObject.ServicePoint.Expect100Continue = false;
                    //                 // Request response:                               
                    //                 WebResponse Response = WebRequestObject.GetResponse();

                    WebResponse Response = GetResponseNoCache(new Uri(Url), 0);

                    // Open data stream:
                    Stream WebStream = Response.GetResponseStream();

                    // Create reader object:
                    StreamReader Reader = new StreamReader(WebStream);

                    // Read the entire stream content:
                    PageContent = Reader.ReadToEnd();

                    // Cleanup
                    Reader.Close();
                    WebStream.Close();
                    Response.Close();
                    return true;
                }
                catch (System.Exception oEx)
                {
                    PageContent = oEx.ToString();
                    cGlobalSettings.oLogger.WriteLogException("DownloadWebPage()|", oEx);
                    //oUtils.WriteToFile("DownloadWebPage()| Url: " + Url + "\n" + PageContent);
                    return false;
                }

            }


            /// <summary>
            /// This Function Will be called only after all the updates are downloaded
            /// Will Install both Complete database and Incremental Update
            /// This will work only if the update size will Match exactly
            /// </summary>
            /// <returns>true if all updates are installed properly</returns>
            internal bool InstallDatabaseUpdates()
            {
                try
                {
                    // bool bIsSafeListFilled = false;

                    //  bool bDbInstalled = false;     

                    foreach (cUpdateDetails oDetails in LsUpdates)
                    {
                        // if download size does not match break operation 
                        if (/*oDetails.UpdateType == 2 ||*/ oDetails.UpdateType == 3)// means Application Update
                        {
                            //                         int nFileSize = 0;
                            //                         try
                            //                         {
                            //                             // get the actual size again coz there is a possibility of partial update download which can change the total download size
                            //                             FileInfo oFi = new FileInfo(oDetails.LocalDownloadPath); 
                            //                             nFileSize = Convert.ToInt32(oFi.Length);
                            //                             oFi = null;
                            //                         }
                            //                         catch (System.Exception ex)
                            //                         {}

                            if (oDetails.TotalSize != oDetails.DownloadedSize)
                            {
                                cGlobalSettings.oLogger.WriteLogVerbose("InstallDatabaseUpdates()|error : update file size mismatch : |" + oDetails.UpdateName);

                                break;
                            }
                        }
                        else
                        {
                            if (!CheckDownloadedUpdate(oDetails)/*oDetails.TotalSize != oDetails.DownloadedSize*/)
                            {
                                cGlobalSettings.oLogger.WriteLogVerbose("InstallDatabaseUpdates()|error : update file size mismatch : " + oDetails.UpdateName);
                                break;
                            }
                        }
                        //if (oShowUpdateInstallEvent != null) oShowUpdateInstallEvent(string.Format(cResourceManager.LoadString(oDetails.UpdateType == 2 ? "IDS_SE_INSTALLINGUPDATE" : "IDS_SE_INSTALLINGUPDATE")/*"Installing Update ({0} / {1} ) ...."*/, oDetails.Secquence, LsUpdates.Count), true);
                        if (oShowUpdateInstallEvent != null && _RaiseEvents) oShowUpdateInstallEvent(string.Empty/*cResourceManager.LoadString("IDS_UPDATE_STEP3")*/, true);

                        if (oDetails.UpdateType == 2 || oDetails.UpdateType == 3)// means Application Update
                        {

                            InstallApplicationUpdate(oDetails, true);

                            return true;

                        }

                    }


                }
                catch (System.Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("InstallDatabaseUpdates()|", oEx);

                    return false;
                }
                finally
                {
                    //Trace.WriteLine("update install end time:" + DateTime.Now.ToString("dd-MM-yy hh:mm:ss:fff"));
                }

                return false;

            }









            public void InstallApplicationUpdate(object oDetails, bool boWaitForExit)
            {
                try
                {
                    cUpdateDetails m_Item = (cUpdateDetails)oDetails;
                    bool bInstall = false;
                    bool bWaitForExit = boWaitForExit;
                    string sExecutableFilePath = string.Empty;
                    string sCommandLineArgs = string.Empty;


                    sExecutableFilePath = m_Item.LocalDownloadPath;
                    sCommandLineArgs = "/verysilent";
                    bInstall = true;

                    if (bInstall)
                    {
                        Process pAppInstall = new Process();
                        pAppInstall.StartInfo.FileName = sExecutableFilePath;
                        pAppInstall.StartInfo.Arguments = sCommandLineArgs;
                        if (Environment.OSVersion.Version.Major > 5) pAppInstall.StartInfo.Verb = "runas";

                        pAppInstall.Start();
                        Process.GetCurrentProcess().Kill();
                        //                     if (bWaitForExit)
                        //                     {
                        //                         pAppInstall.WaitForExit();
                        //                         pAppInstall.Dispose();
                        //                     }


                    }

                }
                catch (Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("InstallApplicationUpdate()|", oEx);


                }
            }

            /// <summary>
            /// Installs the latest downloaded application update by launching the new application and closing the existing application
            /// New application will be installed through silent params provided (msiexec [appname] /qn )
            /// </summary>
            /// <param name="oDetails">details of downloaded file</param>
            public void InstallApplicationUpdate(object oDetails)
            {
                try
                {
                    cUpdateDetails m_Item = (cUpdateDetails)oDetails;
                    bool bInstall = false;
                    bool bWaitForExit = false;
                    string sExecutableFilePath = string.Empty;
                    string sCommandLineArgs = string.Empty;


                    sExecutableFilePath = m_Item.LocalDownloadPath;
                    sCommandLineArgs = "/verysilent";
                    bInstall = true;

                    if (bInstall)
                    {
                        Process pAppInstall = new Process();
                        pAppInstall.StartInfo.FileName = sExecutableFilePath;
                        pAppInstall.StartInfo.Arguments = sCommandLineArgs;
                        if (Environment.OSVersion.Version.Major > 5) pAppInstall.StartInfo.Verb = "runas";

                        pAppInstall.Start();
                        if (bWaitForExit)
                        {
                            pAppInstall.WaitForExit();
                            pAppInstall.Dispose();
                        }


                    }

                }
                catch (Exception oEx)
                {
                    cGlobalSettings.oLogger.WriteLogException("InstallApplicationUpdate()|Error : |", oEx);

                }
            }




            internal void ExposeEventProperties(string CustomMessage, bool PrintOnNewLine)
            {
                ExposeEventProperties(CustomMessage, string.Empty, 0, 0, PrintOnNewLine, 0);
            }

            internal void ExposeEventProperties(string CustomMessage, string ScanItem, double TotalSize, double DownloadedSize, bool PrintOnNewLine, double TotalPercent)
            {
                if (oShowUpdateProgress != null && _RaiseEvents)
                {
                    if (oUpdateProgressClass == null) oUpdateProgressClass = new cProgressArgs();

                    oUpdateProgressClass.CustomMessage = CustomMessage;
                    oUpdateProgressClass.PrintOnNewLine = PrintOnNewLine;
                    //oUpdateProgressClass.MaxCount = TotalCount;
                    //oUpdateProgressClass.Progress = CurrentProgress;
                    oUpdateProgressClass.ScanItem = ScanItem;
                    oUpdateProgressClass.StartTime = DateTime.Now;
                    oUpdateProgressClass.TotalPercentage = TotalPercent;
                    oUpdateProgressClass.DownloadedSize = DownloadedSize;
                    oUpdateProgressClass.TotalSize = TotalSize;
                    oShowUpdateProgress(oUpdateProgressClass);
                }
            }

            internal void ExposeTotalEventInitializeProperties(string CustomMessage, string ScanItem, int TotalCount, int CurrentProgress, bool PrintOnNewLine)
            {
                if (oShowUpdateTotalInitializedEvent != null && _RaiseEvents)
                {
                    if (oUpdateProgressClass == null) oUpdateProgressClass = new cProgressArgs();

                    oUpdateProgressClass.CustomMessage = CustomMessage;
                    oUpdateProgressClass.PrintOnNewLine = PrintOnNewLine;
                    oUpdateProgressClass.MaxCount = TotalCount;
                    oUpdateProgressClass.Progress = CurrentProgress;
                    oUpdateProgressClass.ScanItem = ScanItem;
                    oUpdateProgressClass.StartTime = DateTime.Now;
                    oShowUpdateTotalProgress(oUpdateProgressClass);
                }
            }

            internal void ExposeTotalEventProperties(string CustomMessage, string ScanItem, int TotalCount, int CurrentProgress, bool PrintOnNewLine)
            {
                if (oShowUpdateTotalProgress != null && _RaiseEvents)
                {
                    if (oUpdateProgressClass == null) oUpdateProgressClass = new cProgressArgs();

                    oUpdateProgressClass.CustomMessage = CustomMessage;
                    oUpdateProgressClass.PrintOnNewLine = PrintOnNewLine;
                    oUpdateProgressClass.MaxCount = TotalCount;
                    oUpdateProgressClass.Progress = CurrentProgress;
                    oUpdateProgressClass.ScanItem = ScanItem;
                    oUpdateProgressClass.StartTime = DateTime.Now;
                    oShowUpdateTotalProgress(oUpdateProgressClass);
                }
            }

        }

    public class cUpdateDetails
    {
        private string m_UpdateName;
        public string UpdateName
        {
            get { return m_UpdateName; }
            set { m_UpdateName = value; }
        }
        private string m_UpdateUrl;
        public string UpdateUrl
        {
            get { return m_UpdateUrl; }
            set { m_UpdateUrl = value; }
        }
        private string m_LocalDownloadPath;
        public string LocalDownloadPath
        {
            get { return m_LocalDownloadPath; }
            set { m_LocalDownloadPath = value; }
        }

        private int m_TotalSize;
        public int TotalSize
        {
            get { return m_TotalSize; }
            set { m_TotalSize = value; }
        }
        private int m_DownloadedSize;
        public int DownloadedSize
        {
            get { return m_DownloadedSize; }
            set { m_DownloadedSize = value; }
        }

        private int m_Secquence;
        public int Secquence
        {
            get { return m_Secquence; }
            set { m_Secquence = value; }
        }
        private int m_UpdateType;
        public int UpdateType
        {
            get { return m_UpdateType; }
            set { m_UpdateType = value; }
        }
        private int m_OpenInBrowser;
        public int OpenInBrowser
        {
            get { return m_OpenInBrowser; }
            set { m_OpenInBrowser = value; }
        }
        public string UpdateSha1 { get; set; }

        public int UpdateVersion { get; set; }
        public int IsMandatoryUpdate { get; set; }

    }

    public class cShowLogos
    {
        public int IsTelNoNeeded { get; set; }
        public int IsMSLogoNeeded { get; set; }

        public int IsUseNotifier { get; set; }
        //public Dictionary<string, string> countriesTelNo { get; set; }
    }

    public class cPhoneNoDetails
    {
        public string DefaultPhoneNo { get; set; }
        public Dictionary<string, string> PhoneListForUpdate { get; set; }
    }

    public class cOfferDetails
    {
        public int TotalIterations { get; set; }// Total no. of time nag is shown.
        public int EachIterationDelay { get; set; } // Total time when nag is shown after application launch
        public DateTime ExpiresOn { get; set; } // Stop showing nag after specify Date
        public bool IsHTML { get; set; } // Is content is in html file.
        public string OfferHtmlURL { get; set; } // Url of zip file, contains content.
        public bool ShowToUnRegisteredUsersOnly { get; set; } // Is message shown to Unregistered and expired user only or to all users.
        public bool IsMultiLangual { get; set; } // Is content show to English user only.
        public string OfferContent_en { get; set; } // If isHtml then fileName else message to show for English users.
        public string OfferContent_zhtw { get; set; }
        public string OfferContent_da { get; set; }
        public string OfferContent_nl { get; set; }
        public string OfferContent_fr { get; set; }
        public string OfferContent_de { get; set; }
        public string OfferContent_it { get; set; }
        public string OfferContent_ja { get; set; }
        public string OfferContent_no { get; set; }
        public string OfferContent_ptbr { get; set; }
        public string OfferContent_es { get; set; }
        public string OfferContent_sv { get; set; }
        public string OfferContent_fi { get; set; }
        public string OfferContent_pt { get; set; }
        public string OfferContent_ru { get; set; }
        public string OfferContent_el { get; set; }
        public string OfferContent_tr { get; set; }
        public string OfferContent_pl { get; set; }
        public string OfferContent_ko { get; set; }
        public string OfferContent_zhcn { get; set; }


        public string OfferRegKey { get; set; } //  "software\\systweak\\right backup"
        public string OfferProductFolderName { get; set; } // "right backup"
        public string OfferExename { get; set; } // "rightbackup.exe"
        public string sregKeyValueInstallPath { get; set; } // "installedpath"
    }

    public class cProgressArgs : EventArgs
    {
        public cClientEnum.eDownloadStatus DownloadStatus { get; set; }


        private string m_ScanItem;
        public string ScanItem
        {
            get { return m_ScanItem; }
            set { m_ScanItem = value; }
        }

        public string m_CustomMessage;
        public string CustomMessage
        {
            get { return m_CustomMessage; }
            set { m_CustomMessage = value; }
        }

        public DateTime m_StartTime;
        public System.DateTime StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }

        public int m_TimeElapsed;
        public int TimeElapsed
        {
            get { return m_TimeElapsed; }
            set { m_TimeElapsed = value; }
        }
        private bool m_PrintOnNewLine;
        public bool PrintOnNewLine
        {
            get { return m_PrintOnNewLine; }
            set { m_PrintOnNewLine = value; }
        }

        private int m_Progress;
        public int Progress
        {
            get { return m_Progress; }
            set { m_Progress = value; }
        }

        public int m_MaxCount;
        public int MaxCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }

        public double DownloadedSize { get; set; }
        public double TotalSize { get; set; }
        public double TotalPercentage { get; set; }
    }



}

