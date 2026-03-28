using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Trial_Protection
{
    public class cArmadilloHelper
    {
        //public static bool bDoNotCheckInternetWhileRegister = true;
#if _PRODUCT_KEY_NAME_
        public bool IsRegistered()
        {
            //return oArmadillo.IsRegistered;//modified by anurag jain 2010-07-19 12-05
            //to be modified
            bool bRet = false;
            try
            {
                bRet = KeyValidator.ST_IsRegisteredVersion2();
                //bRet = ST_IsRegisteredVersion();



                // this c++ api is not working correctly.modified logic below
                ////string sUserName = string.Empty;
                ////string sKey = string.Empty;
                ////sUserName = ST_GetUserName();
                ////if (string.IsNullOrEmpty(sUserName)) return bRet;
                ////sKey = ST_GetKey();
                ////if (string.IsNullOrEmpty(sKey)) return bRet;
                ////bRet = ST_VerifyArmadilloKey(sUserName, sKey);
            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("IsRegistered()|error:{0}", oEx);
                //  //  oUtils.WriteToFile(string.Format("IsRegistered()|error:{0}", oEx.Message));
            }
            return bRet;
        }

        public string UserKey()
        {
            //return oArmadillo.UserKey;//modified by anurag jain 2010-07-19 12-05
            // to be modified
            string sRet = string.Empty;
            try
            {
                //   ST_GetUserKey2(sRet);

                sRet = Convert.ToString(cGlobalSettings.GetCommonAppDataRegistryValue("key"));

                if (string.IsNullOrEmpty(sRet) && IsRegistered())
                {


                    /*IntPtr ptrResult = KeyValidator.ST_GetUserKey2();
                    if (ptrResult != IntPtr.Zero)
                    {
                        sRet = Marshal.PtrToStringAuto(ptrResult);
                        if (!string.IsNullOrEmpty(sRet))
                            Marshal.Release(ptrResult);
                    }*/

                    sRet = KeyValidator.GetLastRegKey();
                }
                //sRet =  ST_GetKey();

            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("UserKey()|error:{0}", oEx);
                //   //  oUtils.WriteToFile(string.Format("UserKey()|error:{0}", oEx.Message));
            }
            return sRet;
        }


        public long DaysLeft()
        {
            //return oArmadillo.DaysLeft;//modified by anurag jain 2010-07-19 12-05
            // to be modified
            int nDaysLeft = 0;
            try
            {
                //changed because ST_GetTotalAllowedTrialPeriod is days left. anurag jain 2010-07-26 11-53
                //int nTrialDays = ST_GetTotalAllowedTrialPeriod();
                //int nInstalledDays = ST_GetInstalledDays();
                //nDaysLeft = nTrialDays - nInstalledDays;
                //if (nDaysLeft < 0) nDaysLeft = 0;
                //end



                //nDaysLeft = ST_GetTotalAllowedTrialPeriod();
                nDaysLeft = KeyValidator.ST_GetTotalAllowedTrialPeriod2();
            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("DaysLeft()|error:{0}", oEx);
                //  oUtils.WriteToFile(string.Format("DaysLeft()|error:{0}", oEx.Message));
            }
            return nDaysLeft;
        }


        public bool Expired()
        {
            //return oArmadillo.Expired;//modified by anurag jain 2010-07-19 12-05
            //to be modified
            bool bRet = false;
            try
            {
                //bRet = ST_ARM_IsExpired();

                KeyValidator.SYS_TRIAL_VERSION_STATUS oStatus = KeyValidator.ST_XGetRegStatus();

                //eSYS_TRIAL_VERSION_STATUS oStatus = (eSYS_TRIAL_VERSION_STATUS)nRetVal;

                switch (oStatus)
                {
                    case KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_EVAL_VERSION_EXPIRED:
                    case KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_EXPIRED:
                        bRet = true;
                        break;
                    case KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_ACTIVATION_FAILED:
                        bRet = false;
                        break;
                    case KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_EVAL_PERIOD_REMAINING:
                    case KeyValidator.SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING:
                        bRet = false;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("Expired ::", oEx);
                //  oUtils.WriteToFile(string.Format("Expired()|error:{0}", oEx.Message));
            }
            return bRet;
        }


        public bool SetExpireKey(bool bForcefully = false)
        {
            bool bres = false;
            try
            {
                //   bres = ST_ExpireCurrentKey();
                //   ST_UpdateEnviornment();


                KeyValidator.ExpireCurrentKey(bForcefully);

                KeyValidator.ST_XGetRegStatus();
            }
            catch (System.Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("SetExpireKey()|error:{0}", oEx);
                //  oUtils.WriteToFile(string.Format("SetExpireKey()|error:{0}", oEx.Message));
            }
            return bres;

        }

        public bool InstallKey(string sName, string sKey, bool bInstallOnExit)
        {
            //return oArmadillo.InstallKey(sName, sKey, bInstallOnExit);//modified by anurag jain 2010-07-19 12-05
            //to be modified
            bool bRet = false;
            try
            {
                //bRet = ST_InstallArmadilloCode(sName, sKey);

                bRet = (KeyValidator.ST_InstallArmadilloCode2(/*sName,*/ sKey) > 0);
            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("DaysInstalled ::", oEx);
                //  oUtils.WriteToFile(string.Format("InstallKey()|error:{0}", oEx.Message));
            }
            return bRet;
        }

        public bool IsValidKey(string sName, string sKey, bool bUpdateEnvironment)
        {
            //return oArmadillo.IsValidKey(sName, sKey, bUpdateEnvironment);//modified by anurag jain 2010-07-19 12-05
            //to be modified
            bool bRet = false;
            try
            {
                //                 bRet = ST_VerifyArmadilloKey(sName, sKey);                
                //                 if (bUpdateEnvironment) ST_UpdateEnviornment();

                bRet = (KeyValidator.ST_VerifyArmadilloKey2(sKey) > 0);
            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("IsValidKey ::", oEx);
                //  oUtils.WriteToFile(string.Format("IsValidKey()|error:{0}", oEx.Message));
            }
            return bRet;
        }





#else
        #region Api
        public const string ProductName = "DPFXR";

        public enum eSYS_TRIAL_VERSION_STATUS
        {
            ///not for Dev. choose variable prime numbers to confuse cracker what these return values means
            //STATUS_IS_RETAIL_VERSION = 40507, // this may be retail version do not requiring any key or activation
            //TRIALPROTECTION::GetST_IsRetailKey() please use this function instead of above constant

            STATUS_EVAL_VERSION_EXPIRED = 181991, //the evaluation version has expired
            STATUS_SUBSCRIPTION_EXPIRED = 162997, //the subscription has been ended but version may be registered
            STATUS_SUBSCRIPTION_ACTIVATION_FAILED = 324931, //the subscription is valid but activation has been failed
            STATUS_EVAL_PERIOD_REMAINING = 648973, //the evaluation period is still remaining
            STATUS_SUBSCRIPTION_REMAINING = 1896161,
        };

#if DEBUG
        private const string CONST_LIC_MANAGER_PATH = @"dpfxrsys.dll";
#else
       private const string CONST_LIC_MANAGER_PATH = @"dpfxrsys.dll";
#endif
        //public const string CONST_LIC_MANAGER_PATH = "LicenseManager.dll";

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#2", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool STGetUserString(int nIndex, byte[] bUserString, int nLength);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#4", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool STSetUserString(int nIndex, string sUserString);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#5", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_ARM_IsExpired();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#6", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_ExpireCurrentKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#8", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_GetInstalledDays();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#9", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr ST_GetKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#11", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_GetTotalAllowedTrialPeriod();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#12", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr ST_GetUSER_NAME_PERMANENT();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#13", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr ST_GetUserName();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#14", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_InstallArmadilloCode(string strUserName, string strUserPass);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#15", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_IsRegisteredVersion();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#16", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_IsRetailKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#17", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_SetDefaultArmadilloKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#18", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_UnInstallKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#3", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_UpdateEnviornment();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#19", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_VerifyArmadilloKey(string strUserName, string strUserPass);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#20", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr STGetEnvironmentvariable(string sUserString);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#22", ExactSpelling = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        internal static extern bool CheckActivationStatus(ref ACTIVATION_DATA p, IntPtr prgCallBack);


        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#23", ExactSpelling = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr STGetHardwareHash();




        //new keygen
        /*
         ; int ST_GetTotalAllowedTrialPeriod2(void)
            ?ST_GetTotalAllowedTrialPeriod2@@YGHXZ @29 NONAME

        ; void ST_GetUserKey2(wchar_t *)
            ?ST_GetUserKey2@@YGXPA_W@Z @30 NONAME

        ; int ST_InstallArmadilloCode2(wchar_t const *)
            ?ST_InstallArmadilloCode2@@YGHPB_W@Z @31 NONAME

        ; int ST_IsRegisteredVersion2(void)
            ?ST_IsRegisteredVersion2@@YGHXZ @32 NONAME

        ; int ST_VerifyArmadilloKey2(wchar_t const *)
            ?ST_VerifyArmadilloKey2@@YGHPB_W@Z @33 NONAME

        STIsRunningInWin32_2


        ; int ST_XGetRegStatus(void)
            ?ST_XGetRegStatus@@YGHXZ @34 NONAME
         * 
         * 
       ; void ST_XXExpireCurrentKey(void)
    ?ST_XXExpireCurrentKey@@YGXXZ @35 NONAME

; int ST_XXResetKey(void)
    ?ST_XXResetKey@@YGHXZ @36 NONAME
         */

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#29", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_GetTotalAllowedTrialPeriod2();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#30", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr ST_GetUserKey2();
        //internal static extern void ST_GetUserKey2(string key);


        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#31", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_InstallArmadilloCode2(/*string strUserName,*/ string strUserPass);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#32", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_IsRegisteredVersion2();


        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#33", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool ST_VerifyArmadilloKey2(/*string strUserName,*/ string strUserPass);

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#34", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_XGetRegStatus();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#35", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_XXExpireCurrentKey();

        [DllImport(CONST_LIC_MANAGER_PATH, EntryPoint = "#36", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int ST_XXResetKey();

        #endregion


#if false
        public string GetHardwareHash()
        {
            //return oArmadillo.UserName;//modified by anurag jain 2010-07-19 12-05
            //to be modified
            string sRet = string.Empty;
            try
            {
                IntPtr ptrResult = STGetHardwareHash();
                if (ptrResult != IntPtr.Zero)
                {
                    sRet = Marshal.PtrToStringAnsi(ptrResult);
                    Marshal.Release(ptrResult);
                }
                
            }
            catch (Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("UserName()", oEx);
                //  oUtils.WriteToFile();
            }
            return sRet;
        }


#endif
#endif


        const int MAX_PATH = 260;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ACTIVATION_DATA
        {

            public IntPtr hwnd;	// handle of main dialog, used to show message 

            [MarshalAsAttribute(UnmanagedType.Bool)]
            public bool b_forceActivationCheck; //check even before 24 hours

            [MarshalAsAttribute(UnmanagedType.Bool)]
            public bool b_forceTakeAction; //take the action returned by web service
            // difference between b_forceActivationCheck & b_forceTakeAction is that, 
            // when we are getting web service results at time of registration, 
            // forceful check is necessary to get web results,
            // but taking action at that time, is not necessary,


            public int app_identifier;

            public int LanguageID;	// language ID

            public int nRequestFlags;  // eRequestFlags  Service Request, What to do by Service

            public int REGISTRATION_STATUS; //eREG_STATUS like TRIAL_VERSION, REGISTERED_AND_IS_ABOUT_TOEXPIRE,  VOLUME_COPY

            public int ClientStatus;	// to be returned to get status of key at time of registration

            public int KeyStatus;	// to be returned to get status of key at time of registration

            public int GetNewActivationKey;

            public int nDaysRemaining; //from the date key first used for activation

            public int kti;// Key Type ID	

            public int rid;// Reason ID	

            public int msgid;// message id	

            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 96)]
            public string str_RegisteredKey;  //the KEY which is used to register the product 
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string str_Version;  //the version which we need to sent
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 96)]
            public string str_NewActivationKey; //whatever we will return to application new key 

            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string str_AppRegistryPath;//main application registry key (no need to specify HKLM or HKCU software\systweak\product\) 
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string str_UserActivationCode; //the activation code on basis of which we will get a new key 
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string str_UserEmailAddress; //if key is required on basis of email address

            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string str_AppDataFolder; //where the appdata folder is	



        };










        /************************************************************************/
        /* 
         * App Launch  - b_forceActivationCheck = false
         * Register - true;
         * 
         * same for b_forceTakeAction=true;
         * 
         * */
        /************************************************************************/
        internal bool GetActivationStat(ref ACTIVATION_DATA oActivationstruct)
        {
            bool bRes = false;


            /* oActivationstruct = new ACTIVATION_DATA();
             int handle = 2445;
             IntPtr hwnd = new IntPtr(handle);            
             oActivationstruct.hwnd = hwnd;	// handle of main dialog, used to show message 
             oActivationstruct.b_forceActivationCheck=true; //check even before 24 hours
             oActivationstruct.b_forceTakeAction=true; //take the action returned by web service
             oActivationstruct.app_identifier = 17; //(int)enumClass.Client_Product_Version.SYSTWEAK_ASO_V3; ;
             oActivationstruct.LanguageID = 33;// (int)enumClass.LanguageIdentifier.Lang_English;
             oActivationstruct.nRequestFlags = 0;  // eRequestFlags  Service Request, What to do by Service
             oActivationstruct.REGISTRATION_STATUS= 0; //eREG_STATUS like TRIAL_VERSION, REGISTERED_AND_IS_ABOUT_TOEXPIRE,  VOLUME_COPY
             oActivationstruct.ClientStatus= 0;	// to be returned to get status of key at time of registration
             oActivationstruct.KeyStatus= 0;	// to be returned to get status of key at time of registration
             oActivationstruct.GetNewActivationKey= 0;
             oActivationstruct.nDaysRemaining= 10; //from the date key first used for activation
             oActivationstruct.str_RegisteredKey = "00H3E5-KRT9MF-3NA062-28XTE1";  //the KEY which is used to register the product 
             oActivationstruct.str_Version="trial";  //the version which we need to sent
             oActivationstruct.str_NewActivationKey=string.Empty; //whatever we will return to application new key             
             oActivationstruct.str_AppRegistryPath = Application.UserAppDataRegistry.ToString().Replace("HKEY_CURRENT_USER\\","");//main application registry key (no need to specify HKLM or HKCU software\systweak\product\) 
             oActivationstruct.str_UserActivationCode=string.Empty;; //the activation code on basis of which we will get a new key 
             oActivationstruct.str_UserEmailAddress=string.Empty; //if key is required on basis of email address
             oActivationstruct.str_AppDataFolder = Application.CommonAppDataPath; //where the appdata folder is	
             */

#if _PRODUCT_KEY_NAME_

            string userMachineId = HardwareHash.GetHardwareHash();
            cGlobalSettings.oLogger.WriteLogVerbose("GetActivationStat:: userMachineId: {0}", userMachineId);


            bRes = KVO.STCheckProductGenuineness_(
                ref oActivationstruct,
                (short)oActivationstruct.app_identifier,
                (short)oActivationstruct.LanguageID,
                oActivationstruct.str_UserEmailAddress,
                oActivationstruct.str_RegisteredKey,
                userMachineId.ToString(),
                oActivationstruct.str_AppRegistryPath,
                oActivationstruct.str_AppDataFolder,
                oActivationstruct.REGISTRATION_STATUS,
                oActivationstruct.b_forceActivationCheck,
                oActivationstruct.b_forceTakeAction
                );
    
#else
            bRes = CheckActivationStatus(ref oActivationstruct, IntPtr.Zero);
#endif


            /* //  oUtils.WriteToFile("oActivationstruct.hwnd=" + oActivationstruct.hwnd.ToString());
            //  oUtils.WriteToFile("oActivationstruct.b_forceActivationCheck=" + oActivationstruct.b_forceActivationCheck.ToString());
            //  oUtils.WriteToFile("oActivationstruct.b_forceTakeAction=" + oActivationstruct.b_forceTakeAction.ToString());
            //  oUtils.WriteToFile("oActivationstruct.app_identifier=" + oActivationstruct.app_identifier.ToString());
            //  oUtils.WriteToFile("oActivationstruct.LanguageID=" + oActivationstruct.LanguageID.ToString());
            //  oUtils.WriteToFile("oActivationstruct.nRequestFlags=" + oActivationstruct.nRequestFlags.ToString());
            //  oUtils.WriteToFile("oActivationstruct.REGISTRATION_STATUS=" + oActivationstruct.REGISTRATION_STATUS.ToString());
            //  oUtils.WriteToFile("oActivationstruct.ClientStatus=" + oActivationstruct.ClientStatus.ToString());
            //  oUtils.WriteToFile("oActivationstruct.KeyStatus=" + oActivationstruct.KeyStatus.ToString());
            //  oUtils.WriteToFile("oActivationstruct.GetNewActivationKey=" + oActivationstruct.GetNewActivationKey.ToString());
            //  oUtils.WriteToFile("oActivationstruct.nDaysRemaining=" + oActivationstruct.nDaysRemaining.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_RegisteredKey=" + oActivationstruct.str_RegisteredKey.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_Version=" + oActivationstruct.str_Version.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_NewActivationKey=" + oActivationstruct.str_NewActivationKey.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_AppRegistryPath=" + oActivationstruct.str_AppRegistryPath.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_UserActivationCode=" + oActivationstruct.str_UserActivationCode.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_UserEmailAddress=" + oActivationstruct.str_UserEmailAddress.ToString());
            //  oUtils.WriteToFile("oActivationstruct.str_AppDataFolder=" + oActivationstruct.str_AppDataFolder.ToString());*/




            return bRes;

        }

        #region Online Activation
        public const int ACTIVATION_CHECK_FAILED = 0;
        public const int ACTIVATION_CHECK_SUCCESS = 1;
        public const int ACTIVATION_VERIFICATION_IN_PROCESS = 2;

        public string CheckAndRemovePrependedKey(string sKey)
        {
            #region license manager key
            /************************************************************************/
            /* static bool CheckAndRemovePrependedKey(CString &key)
                {
	                key.MakeUpper();

	                //check for PowerBundle
	                if((key.Find(_T("PB-")) == 0) && (key.GetLength() > 3)  )
	                {	
		                key = CString(key.Right(key.GetLength() - 3));
		                return true;
	                }	

	                if((key.Find(_T("ASP-")) == 0) && (key.GetLength() > 4)  )
	                {	
		                key = CString(key.Right(key.GetLength() - 4));
		                return true;
	                }

	                if((key.Find(_T("ASO-")) == 0) && (key.GetLength() > 4)  )
	                {	
		                key = CString(key.Right(key.GetLength() - 4));
		                return true;
	                }


	                return false;	
                }                                                                     */
            /************************************************************************/
            #endregion

            string sRes = sKey.ToUpper();
            try
            {
                if (sRes.StartsWith("PB-") && sRes.Length > 3)
                {
                    sRes = sRes.Substring(3);
                }
                if (sRes.StartsWith("ASP-") && sRes.Length > 4)
                {
                    sRes = sRes.Substring(4);
                }
                if (sRes.StartsWith("ASO-") && sRes.Length > 4)
                {
                    sRes = sRes.Substring(4);
                }
            }
            catch (System.Exception ex)
            {
                //  oUtils.WriteToFile(string.Format("{0}|ActivateOnline()|Error:{1}", this, ex.Message));
            }
            return sRes;
        }


        public int ActivateOnline(/*bool bForcrActivationCheck,*/string sKey, IntPtr oHandle, int LanguageID, enumClass.eVerificationTime oVerificationTime, out enumClass.eKeyActionToTake oKeyStatus, out int eServiceReturnStatus, out int nMsgid)
        {
            nMsgid = 0;
            oKeyStatus = enumClass.eKeyActionToTake.KEY_RESERVED;
            eServiceReturnStatus = ACTIVATION_VERIFICATION_IN_PROCESS;
            try
            {

                if (oVerificationTime == enumClass.eVerificationTime.Register)
                {

                    if (string.IsNullOrEmpty(sKey))
                    {
                        cGlobalSettings.oLogger.WriteLogVerbose("Activation:: eVerificationTime.Register: Key is empty. Activation Failed.");
                        eServiceReturnStatus = ACTIVATION_CHECK_FAILED;
                        return eServiceReturnStatus;
                    }
                }
                else //if called from App launch
                {
                    if (!cGlobalSettings.cRegDetails.IsRegistered)
                    {
                        if (!cGlobalSettings.cRegDetails.IsExpired)
                        {
                            eServiceReturnStatus = ACTIVATION_CHECK_SUCCESS;
                            return eServiceReturnStatus;
                        }
                    }

                }

                cGlobalSettings.oLogger.WriteLogVerbose("Activation:: CheckAndRemovePrependedKey. Start.");

                sKey = CheckAndRemovePrependedKey(sKey);

                cGlobalSettings.oLogger.WriteLogVerbose("Activation:: CheckAndRemovePrependedKey. Done.");

                int RetCode = 0;

                cGlobalSettings.oLogger.WriteLogVerbose("Activation:: IsInternetConnected.");
                //  oUtils.WriteToFile("is internet connected " + cWin32APIs.IsInternetConnected());
                if (cWin32APIs.IsInternetConnected())
                {


                    bool bForcrActivationCheck = (oVerificationTime == enumClass.eVerificationTime.Register);

                    ACTIVATION_DATA oActivationstruct;

                    oActivationstruct.hwnd = oHandle;	            // handle of main dialog, used to show message 
                    oActivationstruct.b_forceActivationCheck = bForcrActivationCheck;           //check even before 24 hours
                    oActivationstruct.b_forceTakeAction = false;             //take the action returned by web service
                    oActivationstruct.app_identifier = (int)enumClass.Client_Product_Version.S_ASP_V2;
                    oActivationstruct.LanguageID = LanguageID;          // (int)enumClass.LanguageIdentifier.Lang_English;
                    oActivationstruct.nRequestFlags = 0;

                    enumClass.eREG_STATUS REGISTRATION_STATUS = enumClass.eREG_STATUS.TRIAL_VERSION;

                    if (cGlobalSettings.cRegDetails.IsRegistered && !cGlobalSettings.cRegDetails.IsExpired)
                    {
                        REGISTRATION_STATUS = enumClass.eREG_STATUS.REGISTERED;
                    }
                    else if (cGlobalSettings.cRegDetails.IsExpired)
                    {
                        REGISTRATION_STATUS = enumClass.eREG_STATUS.EXPIRED;
                    }
                    else
                    {
                        REGISTRATION_STATUS = enumClass.eREG_STATUS.TRIAL_VERSION;
                    }

                    // eRequestFlags  Service Request, What to do by Service
                    oActivationstruct.REGISTRATION_STATUS = (int)REGISTRATION_STATUS;          //eREG_STATUS like TRIAL_VERSION, REGISTERED_AND_IS_ABOUT_TOEXPIRE,  VOLUME_COPY
                    oActivationstruct.ClientStatus = 0;	            // to be returned to get status of key at time of registration
                    oActivationstruct.KeyStatus = 0;	            // to be returned to get status of key at time of registration
                    oActivationstruct.GetNewActivationKey = 0;
                    oActivationstruct.nDaysRemaining = Convert.ToInt32(cGlobalSettings.cRegDetails.DaysLeft);           //from the date key first used for activation

                    /// Key Type ID	
                    oActivationstruct.kti = 0;
                    /// Reason ID	
                    oActivationstruct.rid = 0;
                    /// message id	
                    oActivationstruct.msgid = 0;

                    oActivationstruct.str_RegisteredKey = sKey;              //the KEY which is used to register the product 
                    oActivationstruct.str_Version = cGlobalSettings.cRegDetails.IsRegistered ? "Registered" : "Trial";           //the version which we need to sent
                    oActivationstruct.str_NewActivationKey = string.Empty;          //whatever we will return to application new key             
                    //oActivationstruct.str_AppRegistryPath = cGlobalSettings.VersionIndependentRegKey.ToLower().Replace("hkey_current_user\\", "");          //main application registry key (no need to specify HKLM or HKCU software\systweak\product\) 
                    oActivationstruct.str_AppRegistryPath = cGlobalSettings.VersionIndependentRegKey.ToLower().Replace("hkey_current_user\\", "");          //main application registry key (no need to specify HKLM or HKCU software\systweak\product\) ;
                    oActivationstruct.str_UserActivationCode = string.Empty;           //the activation code on basis of which we will get a new key 
                    oActivationstruct.str_UserEmailAddress = string.Empty;          //if key is required on basis of email address
                    oActivationstruct.str_AppDataFolder = cGlobalSettings.getCommonAppDataApplicationPath();          //where the appdata folder is	

                    cGlobalSettings.oLogger.WriteLogVerbose("Activation:: GetActivationStat: Start");
                    if (!GetActivationStat(ref oActivationstruct))
                    {
                        cGlobalSettings.oLogger.WriteLogVerbose("Activation:: GetActivationStat: Done -- ACTIVATION_CHECK_FAILED");
                        eServiceReturnStatus = ACTIVATION_CHECK_FAILED;


                        //return eReturnStatus;
                    }
                    else // if calling success then update all status
                    {
                        nMsgid = oActivationstruct.msgid;
                        oKeyStatus = (enumClass.eKeyActionToTake)Enum.Parse(typeof(enumClass.eKeyActionToTake), oActivationstruct.KeyStatus.ToString());
                        //  oUtils.WriteToFile("from main activateonline :" + oKeyStatus);
                        enumClass.eKeyActionToTake clientStatus = (enumClass.eKeyActionToTake)Enum.Parse(typeof(enumClass.eKeyActionToTake), oActivationstruct.ClientStatus.ToString());// not used currently

                        eServiceReturnStatus = ACTIVATION_CHECK_SUCCESS;
                        RetCode = ACTIVATION_CHECK_SUCCESS;
                        if (oVerificationTime == enumClass.eVerificationTime.Register)
                        {
                            //#if VERIFICATION_BASE == VERIFY_ON_BASE_OF_KEY_STATUS_ONLY

                            // check only on base of key status what to do
                            //--------------------------------------------------------------
                            // TURN ON THIS IF WANT TO CHECK ONLY ON BASE OF KEY			
                            //--------------------------------------------------------------

                            /* old code commented by abhinav --dont check this here check it where the function is called
                            RetCode = Convert.ToInt32(IsKeyAllowedToRegister(oKeyStatus));
                            */
                            RetCode = ACTIVATION_CHECK_SUCCESS;

                        } // end if (enumKeyCheckTime != BEFORE_REGISTRATION)		
                        else
                        {
                            // key and client status are not good, expire the current key
                            if (oActivationstruct.REGISTRATION_STATUS != (int)enumClass.eREG_STATUS.TRIAL_VERSION)
                            {
                                if (cGlobalSettings.cRegDetails.IsExpired /*!TRIALPROTECTION::ARM_IsExpired()*/)
                                {
                                    //#if VERIFICATION_BASE == VERIFY_ON_BASE_OF_KEY_STATUS_ONLY
                                    if (IsKeyNeedToExpire(clientStatus) || IsKeyNeedToExpire(oKeyStatus))
                                    // #elif VERIFICATION_BASE == VERIFY_ON_BASE_OF_KEY_AND_CLIENT_STATUS_BOTH
                                    // 					if (IsKeyNeedToExpire(info.ClientStatus) || IsKeyNeedToExpire(info.KeyStatus))
                                    // #endif
                                    {

                                        if (cGlobalSettings.cRegDetails.App_Activation_Status == enumClass.ACTIVATION_STATUS.ACTIVATION_TAKE_ACTION_ON_BASE_RETURNED_FLAGS)
                                        {
#if _PRODUCT_KEY_NAME_
                                            try
                                            {
                                                string reason = string.Format("clientStatus:{0}, IsKeyNeedToExpire(clientStatus):{1}, oKeyStatus:{2}, IsKeyNeedToExpire(oKeyStatus):{3}",
                                                    clientStatus,
                                                    IsKeyNeedToExpire(clientStatus),
                                                    oKeyStatus,
                                                    IsKeyNeedToExpire(oKeyStatus)
                                                    );
                                                KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason);
                                            }
                                            catch (Exception ex)
                                            {
                                                cGlobalSettings.oLogger.WriteLogException("ActivateOnline:: ", ex);
                                            }

                                            KeyValidator.ExpireCurrentKey();
#else
                                            ST_ExpireCurrentKey();
#endif
                                        }

                                    }
                                } // end if (!TRIALPROTECTION::ARM_IsExpired())

                            } //  end if (info.REGISTRATION_STATUS != TRIAL_VERSION)
                        }


                        return RetCode;

                    }
                }
                else
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Activation:: No Internet Connected. ACTIVATION_CHECK_FAILED");
                    return ACTIVATION_CHECK_FAILED;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(" cArmadilloHelper :: ActivateOnline : ::", ex);
                //  oUtils.WriteToFile(string.Format("{0}|ActivateOnline()|Error:{1}", this, ex.Message));
            }


            return ACTIVATION_CHECK_FAILED;
        }
        #endregion

        public bool IsKeyNeedToExpire(enumClass.eKeyActionToTake nReturnStatus)
        {
            bool b_doExireKey = false;
            switch (nReturnStatus)
            {
                case enumClass.eKeyActionToTake.KEY_RESERVED:
                case enumClass.eKeyActionToTake.KEY_EVERYTHING_OK:
                case enumClass.eKeyActionToTake.KEY_GOOD_CONTINUE:
                case enumClass.eKeyActionToTake.INSTALL_A_NEW_KEY:
                case enumClass.eKeyActionToTake.SET_DEFAULT_KEY:
                case enumClass.eKeyActionToTake.GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY:
                case enumClass.eKeyActionToTake.EXTEND_KEY_EXPIRY_DAYS:
                case enumClass.eKeyActionToTake.KEY_BAD_ONLY_OPEN_BROWSER:
                case enumClass.eKeyActionToTake.KEY_BAD_DO_NOTHING:
                case enumClass.eKeyActionToTake.KEY_BAD_ONLY_SHOW_MSG:
                case enumClass.eKeyActionToTake.KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    b_doExireKey = false;
                    break;
                case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT:
                case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG:
                case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_OPEN_BROWSER:
                case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                case enumClass.eKeyActionToTake.KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION:
                    b_doExireKey = true;
                    break;
                default:
                    break;
            }

            return b_doExireKey;
        }

        /************************************************************************
        This suggest the user key status 
        on base of key status set in database
        /************************************************************************/
        public bool IsKeyAllowedToRegister(enumClass.eKeyActionToTake nReturnStatus)
        {
            bool b_isStatusGood = true;
            try
            {
                switch (nReturnStatus)
                {
                    case enumClass.eKeyActionToTake.KEY_RESERVED:
                    case enumClass.eKeyActionToTake.KEY_EVERYTHING_OK:
                    case enumClass.eKeyActionToTake.KEY_GOOD_CONTINUE:
                    case enumClass.eKeyActionToTake.INSTALL_A_NEW_KEY:
                    case enumClass.eKeyActionToTake.SET_DEFAULT_KEY:
                    case enumClass.eKeyActionToTake.GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY:
                    case enumClass.eKeyActionToTake.EXTEND_KEY_EXPIRY_DAYS:
                        b_isStatusGood = true;
                        break;
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_DO_NOTHING:
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    case enumClass.eKeyActionToTake.KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION:
                        b_isStatusGood = false;
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(" cArmadilloHelper :: IsKeyAllowedToRegister : ::", ex);
            }


            return b_isStatusGood;
        }

        /************************************************************************
        This suggest the user key status 
        on base of client status set in database
        /************************************************************************/
        public bool IsClientAllowedToRegister(enumClass.eKeyActionToTake nReturnStatus)
        {
            bool b_isClientAllowedToRegister = true;
            try
            {
                switch (nReturnStatus)
                {
                    case enumClass.eKeyActionToTake.KEY_RESERVED:
                    case enumClass.eKeyActionToTake.KEY_EVERYTHING_OK:
                    case enumClass.eKeyActionToTake.KEY_GOOD_CONTINUE:
                    case enumClass.eKeyActionToTake.INSTALL_A_NEW_KEY:
                    case enumClass.eKeyActionToTake.SET_DEFAULT_KEY:
                    case enumClass.eKeyActionToTake.GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY:
                    case enumClass.eKeyActionToTake.EXTEND_KEY_EXPIRY_DAYS:
                        b_isClientAllowedToRegister = true;
                        break;
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_DO_NOTHING:
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                        b_isClientAllowedToRegister = true;
                        break;
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    case enumClass.eKeyActionToTake.KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION:
                        b_isClientAllowedToRegister = false;
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(" cArmadilloHelper :: IsClientAllowedToRegister : ::", ex);
            }
            return b_isClientAllowedToRegister;
        }

        public string GetKeyErrorMessage(enumClass.eKeyActionToTake nReturnStatus)
        {
            string strMsg = string.Empty;
            bool b_isStatusGood = true;
            try
            {
                switch (nReturnStatus)
                {
                    case enumClass.eKeyActionToTake.KEY_RESERVED:
                    case enumClass.eKeyActionToTake.KEY_EVERYTHING_OK:
                    case enumClass.eKeyActionToTake.KEY_GOOD_CONTINUE:
                    case enumClass.eKeyActionToTake.INSTALL_A_NEW_KEY:
                    case enumClass.eKeyActionToTake.SET_DEFAULT_KEY:
                    case enumClass.eKeyActionToTake.GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY:
                    case enumClass.eKeyActionToTake.EXTEND_KEY_EXPIRY_DAYS:
                        b_isStatusGood = true;
                        break;
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_DO_NOTHING:
                    case enumClass.eKeyActionToTake.KEY_BAD_ONLY_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_OPEN_BROWSER:
                    case enumClass.eKeyActionToTake.KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT:
                        //   strMsg = cResourceManager.LoadString("KEY_VERIFICATION_ERROR_MSG2");//_T("Entered Key is not valid");
                        strMsg = "Entered Key is not valid";
                        break;
                    case enumClass.eKeyActionToTake.KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION:
                        b_isStatusGood = false;
                        // strMsg = cResourceManager.LoadString("KEY_VERIFICATION_ERROR_MSG3"); // _T("Entered Key is already used");
                        strMsg = "Entered Key is already used";
                        break;
                    default:
                        //strMsg = cResourceManager.LoadString("KEY_VERIFICATION_ERROR_MSG2");//_T("Entered Key is not valid");
                        strMsg = "Entered Key is not valid";
                        break;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(" cArmadilloHelper :: GetKeyErrorMessage : ::", ex);
            }
            return strMsg;
        }


        public string GetActivationMessege(enumClass.ACTIVATION_MESSAGES oMsg)
        {
            string strMsg = string.Empty, strINIId = string.Empty;
            bool b_isStatusGood = true;
            try
            {

                switch (oMsg)
                {
                    /////////
                    //THE THREE VALUES BELOW ARE SAME
                    //BUT ARE USED JUST TO COVER SLOPPY PROGRAMMING 
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_RESERVED:// = -1,
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_EVERYTHING_OK:// = 0,
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_GOOD_CONTINUE:// = 1,
                        ////////////////////////////////
                        b_isStatusGood = true;
                        strMsg = string.Empty;
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_PIRATED:// = 2, // Pirated key
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_PIRATED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_FALSE:// = 3, // Invalid key == > THIS IS THE DEFAULT MESSAGE NOW ON
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_FALSE");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_BLOCKED:// = 4, // key is blocked
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_REFUND:// = 5, // Key refunded
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_EXPIRED:// = 6, // Key expired        
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_EXPIRED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_LIMIT_EXCEEDED:// = 7, // Key used more than limit
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_LIMIT_EXCEEDED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_ALREADY_NOT_IN_DB:// = 8, // Key not generated by portal
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_FALSE");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_KEY_FOUND_FOR_OTHER_PRODUCT:// = 9, // Key belongs to other product
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_FALSE");
                        break;

                    // DeactivateReasons
                    case enumClass.ACTIVATION_MESSAGES.MSG_AUTO_EXPIRE_AFTER_TIME:// = 21,
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_EXPIRED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_REFUND:// = 22,
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_CHARGEBACK:// = 23,
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_EXCEED_DOWNLOAD:// = 24,
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    case enumClass.ACTIVATION_MESSAGES.MSG_MISUSED:// = 25,
                        strINIId = cResourceManager.LoadString("IDS_MSG_KEY_BLOCKED");
                        break;
                    default:
                        break;
                }

                /*IDS_MSG_KEY_PIRATED = This license key has been blacklisted.
IDS_MSG_KEY_FALSE = Please enter a valid license key.
IDS_MSG_KEY_BLOCKED = This license key has been deactivated. 
IDS_MSG_KEY_EXPIRED = This license key has expired. Please purchase a new key.
IDS_MSG_KEY_LIMIT_EXCEEDED = You have exceeded maximum number of allowed machines for the key.
IDS_PP_REL_ISSUES = Privacy Related Issues
IDS_PP_EXPOS_TRACES = Privacy exposing traces on the PC
IDS_PP_EXPOSING_MALWARE_TOO = Privacy exposing traces which may include malware traces
IDS_MSG_UNREGISTERED_EXPIRE_VERSION = Unregister/Expired version*/


            }
            catch (System.Exception oEx)
            {
                cGlobalSettings.oLogger.WriteLogException("GetActivationMessege()|error:{0}", oEx);
                //  //  oUtils.WriteToFile(string.Format("GetActivationMessege()|error:{0}", oEx.Message));
            }
            return strINIId;
        }
    }
}
