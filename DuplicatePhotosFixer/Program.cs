using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotoFixer;
using DuplicatePhotosFixer.App_Code;

namespace DuplicatePhotosFixer
{
   static class Program
    {

        public static bool bSilentScan = false;
        public static MainWindow oMainReference = null;

        public static string SourceFolder = "";
        public static bool IsCommandLine = false;

        public static int MatchingLevel = 0;

        public static bool bSilentScanNLaunch = false;

        public static bool bIsFirstInstall = false;
        public static bool bIsAfterUpdate = false;



        internal static bool GetArmStatus()
        {
            cRegistrySettings oRegSettings = new cRegistrySettings();
            //CUtils oUtils = null;
            bool bRes = false;
            try
            {
                /*
                 * 
                 * 
                 * CRegistry reg;
            reg.Permission = KEY_READ;
           
            if(reg.Open(HKEY_LOCAL_MACHINE, REGSTR_BASEKEY))
            {
                  DWORD dwDep = 0;
                  if (reg.VerifyValue(_T("DepOnSPCDock")))
                  {
                        reg.ReadDWORD(_T("DepOnSPCDock"),dwDep);//This is a build that is dependent on SSD.
                       
                        if (dwDep == 1)
                        {
                              CRegistry regSPC;
                              regSPC.Permission = KEY_READ;
 
                              if(regSPC.Open(HKEY_LOCAL_MACHINE, _T("Software\\tps\\Smart PC Care Dock")))
                              {
                                    DWORD dwSPCReg = 0;
                                    //Check if support dock is registered
                                    if (regSPC.VerifyValue(_T("SPCReg")))
                                    {
                                          regSPC.ReadDWORD(_T("SPCReg"), dwSPCReg);
                                    }
 
                                    if (dwSPCReg == 1)
                                    {
                                          return STATUS_SUBSCRIPTION_REMAINING;
                                    }
                              }
                       
                        }
                  }
            }
                 * */

                if (!cGlobal.isSmartPCCareRegistered())
                {
                    //oUtils = new CUtils();
                    cArmadilloHelper oArmadilloHelper = new cArmadilloHelper();
                    cGlobalSettings.cRegDetails.IsRegistered = oArmadilloHelper.IsRegistered();
                    cGlobalSettings.cRegDetails.DaysLeft = (int)oArmadilloHelper.DaysLeft();
                    cGlobalSettings.cRegDetails.IsExpired = oArmadilloHelper.Expired();
                    cGlobalSettings.cRegDetails.Key = oArmadilloHelper.UserKey();

                    if (!string.IsNullOrEmpty(cGlobalSettings.cRegDetails.Key) && cGlobalSettings.cRegDetails.IsExpired)
                    {
                        try
                        {
                            string reason1 = string.Format("program:GetArmStatus");
                            KVO.ReportExpireCurrentKeyReason(cGlobalSettings.ProductId, reason1, KVO.ReportExpireCurrentKeyReasonType.OnLaunchHasKeyButFoundExpired);
                        }
                        catch (Exception ex)
                        {
                            cGlobalSettings.oLogger.WriteLogException("GetArmStatus:: ", ex);
                        }
                    }
                }
                else
                {
                    cGlobalSettings.cRegDetails.IsRegistered = true;
                    cGlobalSettings.cRegDetails.IsExpired = false;
                }


                /*string maxDeleteLimit;

                oArmadilloHelper.SetUserString((int)cClientEnum.eArmUserStrings.FILES_DELETED, "100");
*/




                //oArmadilloHelper.GetUserString((int)cClientEnum.eArmUserStrings.FILES_DELETED, out maxDeleteLimit, 0);

                //cGlobalSettings.maxTrialDeleteLimit = Convert.ToInt64(maxDeleteLimit);
                //string UserNameOfArm = oArmadilloHelper.UserName();


                //write details in log file
                cGlobalSettings.oLogger.WriteLog(string.Format("[Registration details]\r\nKey:{0}\r\nRegistered:{1}\r\nDaysLeft:{2}\r\nInstallDays:{3}\r\nExpired:{4}", cGlobalSettings.cRegDetails.Key,
                    cGlobalSettings.cRegDetails.IsRegistered, cGlobalSettings.cRegDetails.DaysLeft, "", cGlobalSettings.cRegDetails.IsExpired));


                if (cGlobalSettings.cRegDetails.IsRegistered && !cGlobalSettings.cRegDetails.IsExpired)
                {
                    //RemoveStartupRegLink();
                }
                else
                {
                    if (cGlobalSettings.cRegDetails.IsExpired)
                    {
                        //CreateStartupRegLink();
                    }
                }
                oRegSettings.SetRegistartionSettings();

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("main|unable to get GetArmStatus()|error:{0}", ex.Message));
                // MessageBoxEx.Show("unable to get GetArmStatus()\r\n" + ex.Message, cGlobalSettings.GetProductNameFromDesc());
            }
            return bRes;
        }
    }
}
