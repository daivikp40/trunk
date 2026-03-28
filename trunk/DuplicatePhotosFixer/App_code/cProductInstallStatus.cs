using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.App_code
{
    public class cProductInstallStatus
    {
        public static void GetProductDetails(out cClientEnum.PBInstalledState InstalledState, out int ErrorFound, string sKey, string sProductFolderName, string sExename, string sKeyValueInstallPath, out string sCompletePath, bool checkFilePathOnly = false)
        {
            sCompletePath = string.Empty;
            InstalledState = cClientEnum.PBInstalledState.NotInstalled;

            ErrorFound = 0;
            try
            {
                if (cWin32APIs.GetPlatform() == cWin32APIs.Platform.X64)
                {
                    sCompletePath = Path.Combine(cWin32APIs.GetShFolderPath(cWin32APIs.CSIDL.CSIDL_PROGRAM_FILESX86) + "\\" + sProductFolderName, sExename);
                }
                else
                {
                    sCompletePath = Path.Combine(cWin32APIs.GetShFolderPath(cWin32APIs.CSIDL.CSIDL_PROGRAM_FILES) + "\\" + sProductFolderName, sExename);
                }
                //string COMPLETEPATH = Path.Combine(cWin32APIs.GetShFolderPath(cEnums.CSIDL.CSIDL_PROGRAM_FILESX86) + "\\advanced driver updater", ADUEXENAME);


                if (checkFilePathOnly)
                {
                    if (File.Exists(sCompletePath))
                        InstalledState = cClientEnum.PBInstalledState.Installed;
                    else
                    {
                        InstalledState = cClientEnum.PBInstalledState.NotInstalled;
                    }
                    return;
                }

                int isExpired = 0;
                string sRegistered = string.Empty;
                string sInstalledPath = string.Empty;

                if (Registry.CurrentUser.OpenSubKey(sKey) != null)
                {
                    InstalledState = cClientEnum.PBInstalledState.Installed;
                    if (Registry.CurrentUser.OpenSubKey(sKey) != null)
                    {
                        //ErrorFound = Convert.ToInt32(Registry.LocalMachine.OpenSubKey(sADUKey).GetValue("TotalOutOfDateDrivers"));

                        sInstalledPath = Convert.ToString(Registry.CurrentUser.OpenSubKey(sKey).GetValue(sKeyValueInstallPath, string.Empty));
                        if (!String.IsNullOrEmpty(sInstalledPath))
                        {
                            if (File.Exists(Path.Combine(sInstalledPath, sExename)))
                            {
                                InstalledState = cClientEnum.PBInstalledState.Installed;
                            }
                            else
                            {
                                if (File.Exists(sCompletePath))
                                {
                                    InstalledState = cClientEnum.PBInstalledState.Installed;
                                }
                                else
                                {
                                    InstalledState = cClientEnum.PBInstalledState.NotInstalled;
                                }
                            }
                        }
                        else
                        {
                            InstalledState = cClientEnum.PBInstalledState.NotInstalled;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cScanSettings:: GetProductDetails: ", ex);
                //oUtils.WriteToFile(string.Format("{0}|GetProductDetails()|Error:{1}", this, ex.Message));
                InstalledState = cClientEnum.PBInstalledState.NotInstalled;
            }
        }

        public static cClientEnum.PBInstalledState isDFFInstalled()
        {
            cClientEnum.PBInstalledState oAppInstallState = cClientEnum.PBInstalledState.NotInstalled;
            int nAppTotalErrorFound = 0;
            string sAppCompletePath = string.Empty;
            GetProductDetails(out oAppInstallState, out nAppTotalErrorFound, "software\\systweak\\Duplicate Files Fixer", "Duplicate Files Fixer", "DuplicateFilesFixer.exe", "installedpath", out sAppCompletePath);

            return oAppInstallState;
        }

        public static cClientEnum.PBInstalledState isADUInstalled()
        {
            cClientEnum.PBInstalledState oAppInstallState = cClientEnum.PBInstalledState.NotInstalled;
            int nAppTotalErrorFound = 0;
            string sAppCompletePath = string.Empty;
            GetProductDetails(out oAppInstallState, out nAppTotalErrorFound, "Software\\Systweak\\adu", "Advanced Driver Updater", "adu.exe", "installedpath", out sAppCompletePath);

            return oAppInstallState;
        }

        public static cClientEnum.PBInstalledState isRCPInstalled()
        {
            cClientEnum.PBInstalledState oAppInstallState = cClientEnum.PBInstalledState.NotInstalled;
            int nAppTotalErrorFound = 0;
            string sAppCompletePath = string.Empty;
            GetProductDetails(out oAppInstallState, out nAppTotalErrorFound, "Software\\Systweak\\RegClean Pro", "RCP", "RegCleanPro.exe", "installedpath", out sAppCompletePath, true);

            return oAppInstallState;
        }

        public static cClientEnum.PBInstalledState isASPInstalled()
        {
            cClientEnum.PBInstalledState oAppInstallState = cClientEnum.PBInstalledState.NotInstalled;
            int nAppTotalErrorFound = 0;
            string sAppCompletePath = string.Empty;
            GetProductDetails(out oAppInstallState, out nAppTotalErrorFound, "Software\\Systweak\\Advanced System Protector", "Advanced System Protector", "AdvancedSystemProtector.exe", "installedpath", out sAppCompletePath);

            return oAppInstallState;
        }
    }
}
