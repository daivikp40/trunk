using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DuplicatePhotosFixer.Helper
{
    public class cProductList
    {

        public List<cRecommendedItemsData> lstProducts { get; private set; }

        public cProductList()
        {
            InitClass();
        }

        void InitClass()
        {
            lstProducts = new List<cRecommendedItemsData>();
            //            lstProducts.Add(GetDetails_DPFP());

            //if (IsSAVCompatibleOS())
            //{
            //    lstProducts.Add(GetDetails_SAV());
            //}
            //lstProducts.Add(GetDetails_ADU());
            //lstProducts.Add(GetDetails_ASP());
            //lstProducts.Add(GetDetails_RCP());

            //if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv.ToLower() == "en")
            //{
            //    lstProducts.Add(GetDetails_VPN());
            //    lstProducts.Add(GetDetails_SSU());
            //    lstProducts.Add(GetDetails_RB());
            //}

            //lstProducts.Add(GetDetails_ASO());
        }
        bool IsSAVCompatibleOS()
        {
            try
            {
                //Get Operating system information.
                OperatingSystem os = Environment.OSVersion;
                //Get version information about the os.
                Version vs = os.Version;


                if (os.Platform == PlatformID.Win32NT)
                {
                    switch (vs.Major)
                    {
                        case 6:
                            if (vs.Minor == 1)
                            {
                                //operatingSystem = "7";
                                if (vs.Build <= 7600)
                                {
                                    return false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("IsSAVCompatibleOS ::", ex);
                return false;
            }
        }

        cRecommendedItemsData GetDataItem(BitmapImage imgProductLogo,
            string ProductName,
            string Summary,
            string Description,
            bool isTrending,
            bool isInstalled,
            string ProductPageLink,
            string DownloadLink,
            string RegistryKey,
            string sProductFolderName,
            string sExename,
            string sKeyValueInstallPath,
            string EULA,
            string PrivacyPolicy, Dictionary<string, string> dictFeatures, BitmapImage imgBanner, string launchArgs)
        {
            try
            {
                cClientEnum.PBInstalledState oInstallState = cClientEnum.PBInstalledState.NotInstalled;
                int nRPTotalErrorFound = 0;

                string sCompletePath = string.Empty;
                cGlobalSettings.GetProductDetails(out oInstallState, out nRPTotalErrorFound, RegistryKey, sProductFolderName, sExename, sKeyValueInstallPath, out sCompletePath);



                return new cRecommendedItemsData()
                {
                    Description = Description,
                    imgProductLogo = imgProductLogo,
                    DownloadLink = DownloadLink,
                    nInstallSatus = ((oInstallState == cClientEnum.PBInstalledState.Installed) ? (int)eInstallSatus.Installed : (int)eInstallSatus.Not_Installed),
                    strInstallSatus = ((oInstallState == cClientEnum.PBInstalledState.Installed) ? cResourceManager.LoadString("IDS_INSTALLSATUS_" + eInstallSatus.Installed.ToString().ToUpper()) : cResourceManager.LoadString("IDS_INSTALLSATUS_" + eInstallSatus.Not_Installed.ToString().ToUpper())),
                    isTrending = isTrending,
                    ProductName = ProductName,
                    Summary = Summary,
                    ProductPageLink = ProductPageLink,
                    EULA = EULA,
                    PrivacyPolicy = PrivacyPolicy,
                    RegistryKey = RegistryKey,
                    sExename = sExename,
                    sKeyValueInstallPath = sKeyValueInstallPath,
                    sProductFolderName = sProductFolderName,
                    strCompletePath = sCompletePath,
                    dictFeatures = dictFeatures,
                    imgBanner = imgBanner,
                    launchArgs = launchArgs
                };

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetDataItem ::", ex);
            }

            return null;
        }

        public List<cRecommendedItemsData> GetUpdatedList()
        {
            try
            {
                if (lstProducts == null)
                {
                    InitClass();
                }
                if (lstProducts == null || lstProducts.Count <= 0)
                    return null;

                for (int i = 0; i < lstProducts.Count; i++)
                {
                    lstProducts[i] = GetDataItem(lstProducts[i].imgProductLogo,
                                                    lstProducts[i].ProductName,
                                                    lstProducts[i].Summary,
                                                    lstProducts[i].Description,
                                                    lstProducts[i].isTrending,
                                                    false,
                                                    lstProducts[i].ProductPageLink,
                                                    lstProducts[i].DownloadLink,
                                                    lstProducts[i].RegistryKey,
                                                    lstProducts[i].sProductFolderName,
                                                    lstProducts[i].sExename,
                                                    lstProducts[i].sKeyValueInstallPath,
                                                    lstProducts[i].EULA,
                                                    lstProducts[i].PrivacyPolicy,
                                                    lstProducts[i].dictFeatures,
                                                    lstProducts[i].imgBanner,
                                                    lstProducts[i].launchArgs
                                                    );
                }
            }
            catch (Exception ex)
            {

            }
            return lstProducts;
        }


        //#region Products List

        //private cRecommendedItemsData GetDetails_DPFP()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //    {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_2_FEATURE_INFO4")}
        //    };
        //        return GetDataItem(
        //            Description: cResourceManager.LoadString("IDS_RECOMMEND_2_DESC"),
        //            imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/dpf.png", UriKind.Relative)),
        //            DownloadLink: csProductsUtilities.getDFFStatus(true).installurl,
        //            ProductPageLink: "https://www.duplicatephotosfixer.com/windowsphotofixer.aspx",
        //            isInstalled: false,
        //            isTrending: false,
        //            ProductName: cResourceManager.LoadString("IDS_RECOMMEND_2_PRODUCTNAME"),
        //            Summary: cResourceManager.LoadString("IDS_RECOMMEND_2_SUMMARY"),
        //            EULA: "https://www.duplicatephotosfixer.com/eula",
        //            PrivacyPolicy: "https://www.duplicatephotosfixer.com/PrivacyPolicy.aspx",
        //            RegistryKey: "Software\\Systweak\\Duplicate Photos Fixer Pro",
        //            sProductFolderName: "Duplicate Photos Fixer Pro",
        //            sExename: "DuplicatePhotosFixerPro.exe",
        //            sKeyValueInstallPath: "installedpath",
        //            dictFeatures: dictFeatures,
        //            imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_dpf.png", UriKind.Relative)),
        //            launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_DPFP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_SDC()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_1_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_1_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/SDC_icon.png", UriKind.Relative)),
        //                DownloadLink: "http://cdn.systweak.com/setups/dffw/dffsetupg_dffutilkit.exe?of=dffsetupg_.exe",
        //                ProductPageLink: "http://www.smartdrivercare.com/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_1_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_1_SUMMARY"),
        //                EULA: "http://www.smartdrivercare.com/eula",
        //                PrivacyPolicy: "http://www.smartdrivercare.com/privacy-policy",
        //                RegistryKey: "Software\\Systweak\\Smart Driver Care",
        //                sProductFolderName: "Smart Driver Care",
        //                sExename: "SmartDriverCare.exe",
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_sdc.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_SDC ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_ADU()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_3_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_3_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/ADU_icon.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getADUStatus(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/advanced-driver-updater/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_3_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_3_SUMMARY"),
        //                EULA: "https://www.systweak.com/advanced-driver-updater/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getADUStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getADUStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getADUStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_adu.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_ADU ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_ASP()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_4_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_4_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/asp.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getASPStatus(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/advanced-system-protector/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_4_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_4_SUMMARY"),
        //                EULA: "https://www.systweak.com/advanced-system-protector/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getASPStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getASPStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getASPStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_asp.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_ASP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_ASO()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_5_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_5_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/aso.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getASO3Status(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/advanced-system-optimizer/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_5_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_5_SUMMARY"),
        //                EULA: "https://www.systweak.com/advanced-system-optimizer/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getASO3Status(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getASO3Status(true).product_folder_name,
        //                sExename: csProductsUtilities.getASO3Status(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_aso.png", UriKind.Relative)),
        //                launchArgs: "aso3.exe"
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_ASO ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_RCP()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_6_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_6_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/rcp.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getRCPStatus(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/registry-cleaner/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_6_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_6_SUMMARY"),
        //                EULA: "https://www.systweak.com/registry-cleaner/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getRCPStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getRCPStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getRCPStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_rcp.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_SAV()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_7_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_7_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/sav_logo.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getSAVStatus(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/systweak-antivirus",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_7_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_7_SUMMARY"),
        //                EULA: "https://www.systweak.com/systweak-antivirus/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getSAVStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getSAVStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getSAVStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_sav.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_VPN()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_8_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_8_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/vpn_logo.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getVPNStatus(true).installurl,
        //                ProductPageLink: "https://www.systweakvpn.com/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_8_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_8_SUMMARY"),
        //                EULA: "https://www.systweakvpn.com/eula/",
        //                PrivacyPolicy: "https://www.systweakvpn.com/privacy-policy/",
        //                RegistryKey: csProductsUtilities.getVPNStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getVPNStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getVPNStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_vpn.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_SSU()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_9_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_9_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/softwareupdater_logo.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getSSUStatus(true).installurl,
        //                ProductPageLink: "https://www.systweak.com/software-updater",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_9_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_9_SUMMARY"),
        //                EULA: "https://www.systweak.com/software-updater/eula",
        //                PrivacyPolicy: "http://www.systweak.com/privacy-policy",
        //                RegistryKey: csProductsUtilities.getSSUStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getSSUStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getSSUStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_ssu.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_RB()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_10_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_10_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/rightbackup_logo.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getRBStatus(true).installurl,
        //                ProductPageLink: "https://www.rightbackup.com/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_10_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_10_SUMMARY"),
        //                EULA: "https://www.rightbackup.com/PrivacyPolicy#EULA",
        //                PrivacyPolicy: "https://www.rightbackup.com/PrivacyPolicy#PrivacyPolicy",
        //                RegistryKey: csProductsUtilities.getRBStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getRBStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getRBStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_rb.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //private cRecommendedItemsData GetDetails_DMF()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictFeatures = new Dictionary<string, string>
        //        {
        //            {"0",cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE1")}, {"1", cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE_INFO1") },
        //            {"2",cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE2")}, {"3", cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE_INFO2") },
        //            {"4",cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE3")}, {"5", cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE_INFO3") },
        //            {"6",cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE4")}, {"7", cResourceManager.LoadString("IDS_RECOMMEND_11_FEATURE_INFO4")}
        //        };
        //        return GetDataItem(
        //                Description: cResourceManager.LoadString("IDS_RECOMMEND_10_DESC"),
        //                imgProductLogo: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/rightbackup_logo.png", UriKind.Relative)),
        //                DownloadLink: csProductsUtilities.getRBStatus(true).installurl,
        //                ProductPageLink: "https://www.rightbackup.com/",
        //                isInstalled: false,
        //                isTrending: false,
        //                ProductName: cResourceManager.LoadString("IDS_RECOMMEND_11_PRODUCTNAME"),
        //                Summary: cResourceManager.LoadString("IDS_RECOMMEND_11_SUMMARY"),
        //                EULA: "https://www.rightbackup.com/PrivacyPolicy#EULA",
        //                PrivacyPolicy: "https://www.rightbackup.com/PrivacyPolicy#PrivacyPolicy",
        //                RegistryKey: csProductsUtilities.getRBStatus(true).product_registry_path,
        //                sProductFolderName: csProductsUtilities.getRBStatus(true).product_folder_name,
        //                sExename: csProductsUtilities.getRBStatus(true).product_exe,
        //                sKeyValueInstallPath: "installedpath",
        //                dictFeatures: dictFeatures,
        //                imgBanner: new BitmapImage(new Uri(@"/DuplicateFilesFixer;component/Resources/banner_rb.png", UriKind.Relative)),
        //                launchArgs: ""
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("GetDetails_RCP ::", ex);
        //    }
        //    return null;
        //}

        //#endregion
    }
}
