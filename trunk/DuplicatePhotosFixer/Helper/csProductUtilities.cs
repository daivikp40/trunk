using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Helper
{
    public class csProductsUtilities
    {
        public int productID { get; set; }
        public int Priority { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string product_summary { get; set; }
        public string strEULALink { get; set; }
        public string strPPLink { get; set; }
        public string installurl { get; set; }
        public string product_registry_key_name { get; set; }
        public string product_exe { get; set; }
        public string product_folder_name { get; set; }
        public string product_registry_path { get; set; }
        public string productInstallPath { get; set; }
        public cClientEnum.PBInstalledState productInstallStatus { get; set; }
        public string strSummary { get; set; }
        public List<string> lstFeatures { get; set; }
        public Image logo { get; set; }
        public string strInstallParams { get; set; }

        internal static csProductsUtilities getASO3Status(bool useCurentParams = false)
        {
            csProductsUtilities oUtility1 = new csProductsUtilities()
            {
                productID = 1,
                installurl = string.Format("https://cdn.systweak.com/setups/baps/aso3setup_{0}-{1}_dpfutlkit.exe?of=aso3setup.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN),
                product_exe = "aso3.exe",
                product_folder_name = "advanced system optimizer 3",
                product_name = "Advanced System Optimizer 3",
                product_registry_key_name = "installedpath",
                product_registry_path = "software\\systweak\\aso3",
                strEULALink = "http://www.systweak.com/advanced-system-optimizer/eula",
                strPPLink = "http://www.systweak.com/privacy-policy",
                //logo = Properties.Resources,
            };
            if (useCurentParams)
            {
                oUtility1.installurl = string.Format("https://cdn.systweak.com/setups/baps/aso3setup_{0}-{1}.exe?of=aso3setup.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN);
            }

            UpdateInstallStatus(ref oUtility1);

            return oUtility1;
        }

        internal static csProductsUtilities getRBStatus(bool useCurentParams = false)
        {
            csProductsUtilities oUtility2 = new csProductsUtilities()
            {
                productID = 2,
                
                /*installurl = "https://cdn.systweak.com/setups/baps/rb/rightbackupsetup.exe",*/
                product_exe = "rightbackup.exe",
                product_folder_name = "right backup",
                product_name = "Right BackUp",
                product_registry_key_name = "installedpath",
                product_registry_path = "software\\systweak\\right backup",
                strEULALink = "https://rightbackup.com/PrivacyPolicy#EULA",
                strPPLink = "http://rightbackup.com/privacypolicy#PrivacyPolicy",
                //logo = Properties.Resources.rb,
            };
            if (useCurentParams)
            {
                oUtility2.installurl = string.Format("http://cdn.systweak.com/setups/baps/rbsetup_{0}-{1}_DPFOfferAfterClean.exe?of=rbsetup_.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN);
            }

            UpdateInstallStatus(ref oUtility2);

            return oUtility2;
        }

        internal static csProductsUtilities getDFFStatus(bool useCurentParams = false)
        {
            csProductsUtilities oUtility3 = new csProductsUtilities()
            {
                productID = 3,
                
                product_exe = "duplicatefilesfixer.exe",
                product_folder_name = "Duplicate Files Fixer",
                product_name = "Duplicate Files Fixer",
                product_registry_key_name = "installedpath",
                product_registry_path = "software\\systweak\\Duplicate Files Fixer",
                strEULALink = "https://www.duplicatefilesfixer.com/eula",
                strPPLink = "https://www.duplicatefilesfixer.com/privacypolicy",
                //logo = Properties.Resources.dff,
            };
            if (useCurentParams)
            {
                oUtility3.installurl = string.Format("https://cdn.systweak.com/setups/dffw/baps/dffsetupg_{0}-{1}_dpfwutlkit.exe?of=dffsetupg_.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN);
                    
              }

            UpdateInstallStatus(ref oUtility3);

            return oUtility3;
        }
        public static csProductsUtilities getAfterCleanDFFStatus(bool useCurentParams = false)
        {
            csProductsUtilities oUtility4 = new csProductsUtilities()
            {
                productID = 4,
               
                product_exe = "duplicatefilesfixer.exe",
                product_folder_name = "Duplicate Files Fixer",
                product_name = "Duplicate Files Fixer",
                product_registry_key_name = "installedpath",
                product_registry_path = "software\\systweak\\Duplicate Files Fixer",
                strEULALink = "https://www.duplicatefilesfixer.com/eula",
                strPPLink = "https://www.duplicatefilesfixer.com/privacypolicy",
                //logo = Properties.Resources.dff_logo1,
            };
            if (useCurentParams)
            {
                oUtility4.installurl = string.Format("http://cdn.systweak.com/setups/baps/dffsetupg_{0}-{1}_DPFOfferAfterClean.exe?of=dffsetupg.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN);

            }

            UpdateInstallStatus(ref oUtility4);

            return oUtility4;
        }

        public static csProductsUtilities getAfterCleanADUStatus(bool useCurentParams = false)
        {
            csProductsUtilities oUtility5 = new csProductsUtilities()
            {
                productID = 5,
             
                product_exe = "adu.exe",
                product_folder_name = "Advanced Driver Updater",
                product_name = "Advanced Driver Updater",
                product_registry_key_name = "installedpath",
                product_registry_path = "software\\systweak\\adu",
                strEULALink = "https://www.systweak.com/advanced-driver-updater/eula",
                strPPLink = "https://www.systweak.com/privacy-policy",
                //logo = Properties.Resources.adu_logo,
            };
            if (useCurentParams)
            {
                oUtility5.installurl = string.Format("http://cdn.systweak.com/setups/baps/adug_{0}-{1}_DPFOfferAfterClean.exe?of=adug.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN);

            }

            UpdateInstallStatus(ref oUtility5);

            return oUtility5;
        }

        public static csProductsUtilities getAfterCleanSAVStatus()
        {
            csProductsUtilities oUtility6 = new csProductsUtilities()
            {
                productID = 6,
                installurl = string.Format("http://cdn.systweak.com/setups/baps/savsetupg_{0}-{1}_DPFOfferAfterClean.exe?of=savsetupg.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN),
                product_exe = "sysav.exe",
                product_folder_name = "Systweak Antivirus",
                product_name = "Systweak Antivirus",
                product_registry_key_name = "installedpath",
                product_registry_path = "Software\\systweak\\Systweak Antivirus",
                strEULALink = "http://www.systweak.com/systweak-antivirus/eula",
                strPPLink = "https://www.systweak.com/privacy-policy",
                //logo = Properties.Resources.sav_logo,
            };
            UpdateInstallStatus(ref oUtility6);
            return oUtility6;
        }
        internal static csProductsUtilities getPOStatus(string strCampaignSuffix = "_dpfutlkit")
        {
            //https://cdn.systweak.com/setups/baps/posetup_systweak-default.exe?of=posetup.exe
            csProductsUtilities oUtility1 = new csProductsUtilities()
            {
                productID = 1,
                installurl = string.Format("https://cdn.systweak.com/setups/baps/posetup_{0}-{1}{2}.exe?of=posetup.exe", cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN, strCampaignSuffix),
                product_exe = "PhotoOrganizer.exe",
                product_folder_name = "Photo Organizer",
                product_name = "Photo Organizer",
                product_registry_key_name = "InstalledPath",
                product_registry_path = "software\\Systweak\\Photo Organizer",
                strEULALink = "https://www.systweak.com/photo-organizer/eula",
                strPPLink = "https://www.systweak.com/privacy-policy",
                //logo = Properties.Resources.po_logo_small,
            };
            UpdateInstallStatus(ref oUtility1);

            return oUtility1;
        }

        static void UpdateInstallStatus(ref csProductsUtilities oUtility)
        {
            /// Check product install status and install path is installed
            /// 
            string sRPCompletePath = string.Empty;
            string sRBCompletePath = string.Empty;
            int nTotalErrorFound = 0;
            cClientEnum.PBInstalledState oinstallStatus;
            cGlobalSettings.oScanSettings.GetProductDetails(out oinstallStatus, out nTotalErrorFound, oUtility.product_registry_path, oUtility.product_folder_name, oUtility.product_exe, oUtility.product_registry_key_name, out sRBCompletePath);
            oUtility.productInstallPath = sRBCompletePath;
            oUtility.productInstallStatus = oinstallStatus;
        }

    }
}
