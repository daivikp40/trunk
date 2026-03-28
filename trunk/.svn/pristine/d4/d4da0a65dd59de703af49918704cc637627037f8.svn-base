using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using DuplicatePhotosFixer.CheckForUpdates;
using DuplicatePhotosFixer.Helper;
using DuplicatePhotosFixer.HelperClasses.MyLogger;
using DuplicatePhotosFixer.Helpers;

namespace DuplicatePhotosFixer.App_code
{
   public static class UtilityKitNew
    {

        public static string bFilePath = PathEx.Combine(cGlobalSettings.getCommonAppDataApplicationPath()/*Application.StartupPath*/, @"UtilityKit");
        public static ObservableCollection<Product> oProducts = null;
        public static List<Product> lstProducts = null;
        public static Version currentVersion;
        public static int servicePack = 0;

        static string _GetPath = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Utility_kit.ini");

        public static string SaveImage(string bProductFilePath, string bImageUrl, string bFilename)
        {
            string filename = PathEx.Combine(bProductFilePath, bFilename);
            try
            {
                if (!File.Exists(filename))
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(bImageUrl);
                    Bitmap bitmap; bitmap = new Bitmap(stream);

                    if (bitmap != null)
                    {
                        bitmap.Save(filename, ImageFormat.Png);
                    }

                    stream.Flush();
                    stream.Close();
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("ActionCentre:SaveImage|", ex.Message));
            }

            return filename;
        }

        public enum eUtlKitProduct
        {
            [System.ComponentModel.Description("Systweak VPN")]
            SVPN,
            [System.ComponentModel.Description("Systweak Antivirus")]
            SAV,
            [System.ComponentModel.Description("Systweak Software Updater")]
            SSU,
            [System.ComponentModel.Description("Advanced Driver Updater")]
            ADU,
            [System.ComponentModel.Description("Systweak System Software")]
            ASP,
            [System.ComponentModel.Description("Systweak PDF Editor")]
            RCP,
            [System.ComponentModel.Description("Right Backup")]
            RB,
            [System.ComponentModel.Description("Advanced System Optimizer")]
            ASO,
            [System.ComponentModel.Description("TweakPass Password Manager")]
            TP,
            [System.ComponentModel.Description("TweakShot Screen Capture")]
            TS
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        public static string GetPath()
        {
            return Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Utility_kit.ini");
        }

        public static int GetTotalProducts()
        {
              return Convert.ToInt16(cWin32APIs.IniReadValue("utilitykit", "total_product", GetPath()));          
        }

        public static BitmapImage GetImage(string url)
        {
            BitmapImage bitmapImage = null;
            try
            {
                if (App.oMainReference != null)
                {
                   App.oMainReference.Dispatcher.Invoke(new Action(() =>
                    {
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(url); ;
                        bitmapImage.EndInit();
                    }));
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("ActionCentre:GetImage|", ex.Message));
            }

            return bitmapImage;
        }

        public static List<Product> LoadProducts()
        {
            oProducts = new ObservableCollection<Product>();

            try
            {
                if (!Directory.Exists(bFilePath))
                {
                    Directory.CreateDirectory(bFilePath);
                }

                for (int i = 1; i <= GetTotalProducts(); i++)
                {
                    Product oProduct = new Product();
                    cClientEnum.PBInstalledState oInstallState = cClientEnum.PBInstalledState.NotInstalled;
                    int nRPTotalErrorFound = 0;
                    string sCompletePath = string.Empty;

                    oProduct.ProductId = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "productid", _GetPath);
                    oProduct.ProductName = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_name", _GetPath);

                    string bProductFilePath = PathEx.Combine(bFilePath, oProduct.ProductName);

                    if (!Directory.Exists(bProductFilePath))
                    {
                        Directory.CreateDirectory(bProductFilePath);
                    }

                    oProduct.ProductKey = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_key", _GetPath);
                    oProduct.sProductFolderName = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fname", _GetPath);
                    oProduct.sExename = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_exe", _GetPath);
                    oProduct.ProductKeyValue = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_key_value", _GetPath);
                    oProduct.DownloadLink = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "installurl", _GetPath);
                    oProduct.DownloadLink = string.Format(oProduct.DownloadLink, cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN + "_fromdpfactioncentre");

                    oProduct.imgProductLogo = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "image_url", _GetPath);
                    string bUrl = SaveImage(bProductFilePath, oProduct.imgProductLogo, "imgProductLogo");
                    oProduct.imgProductLogo = bUrl;

                    oProduct.imgBanner = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "imagebanner_url", _GetPath);
                    string bUrl1 = SaveImage(bProductFilePath, oProduct.imgBanner, "imgBanner");
                    oProduct.imgBannerLogo = GetImage(bUrl1);

                    oProduct.Eula = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "EULA", _GetPath);
                    oProduct.PP = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "PrivacyPolicy", _GetPath);


                    cGlobalSettings.oScanSettings.GetProductDetails(out oInstallState, out nRPTotalErrorFound, oProduct.ProductKey, oProduct.sProductFolderName, oProduct.sExename, oProduct.ProductKeyValue, out sCompletePath);
                    oProduct.strCompletePath = sCompletePath;
                    oProduct.nInstallSatus = (oInstallState == cClientEnum.PBInstalledState.Installed) ? (int)eInstallSatus.Installed : (int)eInstallSatus.Not_Installed;
                    oProduct.strInstallSatus = ((oInstallState == cClientEnum.PBInstalledState.Installed) ? cResourceManager.LoadString("IDS_INSTALLSATUS_INSTALLED") : cResourceManager.LoadString("IDS_INSTALLSATUS_NOT_INSTALLED"));

                    oProduct.IsCompatible = true;
                    oProduct.IsLangCompatible = true;

                    if (oProduct.ProductName.Trim().ToLower() == GetEnumDescription(eUtlKitProduct.SAV).Trim().ToLower())
                    {
                        oProduct.IsCompatible = App.oMainReference.CheckSAVCompatible();
                    }

                    if (oProduct.ProductName.Trim().ToLower() == GetEnumDescription(eUtlKitProduct.SSU).Trim().ToLower())
                    {
                        oProduct.IsCompatible = App.oMainReference.CheckSSUCompatible();
                    }

                    if (oProduct.ProductName.Trim().ToLower() == GetEnumDescription(eUtlKitProduct.SVPN).Trim().ToLower())
                    {
                        oProduct.IsCompatible = App.oMainReference.CheckVPNCompatible();
                    }

                    if (oProduct.ProductName.Trim().ToLower() == GetEnumDescription(eUtlKitProduct.TP).Trim().ToLower())
                    {
                        oProduct.IsCompatible = App.oMainReference.CheckTweakPassCompatible();
                    }

                    if (oProduct.ProductName.Trim().ToLower() == GetEnumDescription(eUtlKitProduct.TS).Trim().ToLower())
                    {
                        oProduct.IsCompatible = App.oMainReference.CheckTweakShotCompatible();
                    }


                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "en")
                    {
                        oProduct.product_desc_EN = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_EN", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_EN", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_EN))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_EN;

                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_EN", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_EN", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_EN", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_EN", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_EN", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_EN", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_EN", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_EN", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "da")
                    {
                        oProduct.product_desc_DA = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_DA", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_DA", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_DA))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_DA;

                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_DA", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_DA", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_DA", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_DA", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_DA", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_DA", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_DA", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_DA", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "de")
                    {
                        oProduct.product_desc_DE = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_DE", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_DE", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_DE))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_DE;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_DE", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_DE", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_DE", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_DE", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_DE", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_DE", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_DE", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_DE", _GetPath));
                        }

                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "it")
                    {
                        oProduct.product_desc_IT = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_IT", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_IT", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_IT))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_IT;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_IT", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_IT", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_IT", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_IT", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_IT", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_IT", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_IT", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_IT", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "ja")
                    {
                        oProduct.product_desc_JA = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_JA", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_JA", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_JA))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_JA;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_JA", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_JA", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_JA", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_JA", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_JA", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_JA", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_JA", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_JA", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "fr")
                    {
                        oProduct.product_desc_FR = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_FR", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_FR", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_FR))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_FR;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_FR", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_FR", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_FR", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_FR", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_FR", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_FR", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_FR", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_FR", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "fi")
                    {
                        oProduct.product_desc_FI = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_FI", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_FI", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_FI))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_FI;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_FI", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_FI", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_FI", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_FI", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_FI", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_FI", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_FI", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_FI", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "nl")
                    {
                        oProduct.product_desc_NL = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_NL", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_NL", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_NL))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_NL;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_NL", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_NL", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_NL", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_NL", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_NL", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_NL", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_NL", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_NL", _GetPath));
                        }
                    }
                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "es")
                    {
                        oProduct.product_desc_ES = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_ES", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_ES", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_ES))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_ES;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_ES", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_ES", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_ES", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_ES", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_ES", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_ES", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_ES", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_ES", _GetPath));
                        }
                    }
                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "ptbr")
                    {
                        oProduct.product_desc_PTBR = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_PTBR", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_PTBR", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_PTBR))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_PTBR;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_PTBR", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_PTBR", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "ru")
                    {
                        oProduct.product_desc_RU = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_RU", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_RU", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_RU))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_RU;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_RU", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_RU", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_RU", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_RU", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_RU", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_RU", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_RU", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_RU", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "sv")
                    {
                        oProduct.product_desc_SV = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_SV", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_SV", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_SV))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_SV;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_SV", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_SV", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_SV", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_SV", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_SV", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_SV", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_SV", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_SV", _GetPath));
                        }
                    }

                    if (cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv == "no")
                    {
                        oProduct.product_desc_NO = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_desc_NO", _GetPath);
                        oProduct.Summary = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_summary_NO", _GetPath);

                        if (string.IsNullOrEmpty(oProduct.product_desc_NO))
                        {
                            oProduct.IsLangCompatible = false;
                        }

                        if (oProduct.IsLangCompatible)
                        {
                            oProduct.Description = oProduct.product_desc_NO;
                            oProduct.dictFeatures.Add("0", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f1_NO", _GetPath));
                            oProduct.dictFeatures.Add("1", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI1_NO", _GetPath));
                            oProduct.dictFeatures.Add("2", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f2_NO", _GetPath));
                            oProduct.dictFeatures.Add("3", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI2_NO", _GetPath));
                            oProduct.dictFeatures.Add("4", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f3_NO", _GetPath));
                            oProduct.dictFeatures.Add("5", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI3_NO", _GetPath));
                            oProduct.dictFeatures.Add("6", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_f4_NO", _GetPath));
                            oProduct.dictFeatures.Add("7", cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_fI4_NO", _GetPath));
                        }
                    }

                    oProducts.Add(oProduct);
                }
            }

            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("UtilityKitNew:: LoadProducts: ", ex);
            }

            return lstProducts = oProducts.Where(x => x.IsCompatible == true && x.IsLangCompatible == true).OrderByDescending(x => x.nInstallSatus).ToList();
        }


        public static void DownloadandINstallUtlKit()
        {
            try
            {

                cUpdator oUpdater = new cUpdator();

                string surl = cGlobalSettings.GetActionCentreUrl();
                string ResultValue = string.Empty;

                for (int nCheckIniRetry = 0; nCheckIniRetry < cGlobalSettings.CHECK_FOR_UPDATE_INI_RETRY_COUNT; nCheckIniRetry++)
                {
                    if (oUpdater.DownloadWebPage(surl, out ResultValue))
                    {
                        break;
                    }


                    Thread.Sleep(cGlobalSettings.CHECK_FOR_UPDATE_RETRY_INTERVAL); //sleep for 5 seconds before next retry
                }

                string TempPathForUpdate = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Utility_kit.ini");

                FileStream oStream = new FileStream(TempPathForUpdate, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter oWriter = new StreamWriter(oStream, System.Text.Encoding.Unicode);
                oWriter.Write(ResultValue);
                oWriter.Close(); oStream.Close();

                //Thread.Sleep(100);

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("UtilityKitNew::DownloadandINstallUtlKit|", ex.Message));
                // MessageBoxEx.Show("unable to get GetArmStatus()\r\n" + ex.Message, cGlobalSettings.GetProductNameFromDesc());
            }

        }

    }

    public class Product
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string DownloadLink { get; set; }

        public string imgProductLogo { get; set; }

        public string imgBanner { get; set; }

        public BitmapImage imgBannerLogo { get; set; }

        public string ProductKey { get; set; }

        public string ProductKeyValue { get; set; }
        public string sProductFolderName { get; set; }
        public string sExename { get; set; }

        public string strCompletePath { get; set; }

        public int nInstallSatus { get; set; }      

        public string strInstallSatus { get; set; }
        public bool IsCompatible { get; set; }

        public bool IsLangCompatible { get; set; }
        public string product_desc_EN { get; set; }
        public string product_desc_DA { get; set; }
        public string product_desc_NL { get; set; }
        public string product_desc_FR { get; set; }
        public string product_desc_DE { get; set; }
        public string product_desc_IT { get; set; }
        public string product_desc_JA { get; set; }
        public string product_desc_NO { get; set; }
        public string product_desc_PTBR { get; set; }
        public string product_desc_ES { get; set; }
        public string product_desc_SV { get; set; }
        public string product_desc_FI { get; set; }
        public string product_desc_RU { get; set; }


        public string Eula { get; set; }

        public string PP { get; set; }

        public string product_f1_EN { get; set; }
        public string product_f1_DA { get; set; }
        public string product_f1_NL { get; set; }
        public string product_f1_FR { get; set; }
        public string product_f1_DE { get; set; }
        public string product_f1_IT { get; set; }
        public string product_f1_JA { get; set; }
        public string product_f1_NO { get; set; }
        public string product_f1_PTBR { get; set; }
        public string product_f1_ES { get; set; }
        public string product_f1_SV { get; set; }
        public string product_f1_FI { get; set; }
        public string product_f1_RU { get; set; }

        public string product_f2_EN { get; set; }
        public string product_f2_DA { get; set; }
        public string product_f2_NL { get; set; }
        public string product_f2_FR { get; set; }
        public string product_f2_DE { get; set; }
        public string product_f2_IT { get; set; }
        public string product_f2_JA { get; set; }
        public string product_f2_NO { get; set; }
        public string product_f2_PTBR { get; set; }
        public string product_f2_ES { get; set; }
        public string product_f2_SV { get; set; }
        public string product_f2_FI { get; set; }
        public string product_f2_RU { get; set; }

        public string product_f3_EN { get; set; }
        public string product_f3_DA { get; set; }
        public string product_f3_NL { get; set; }
        public string product_f3_FR { get; set; }
        public string product_f3_DE { get; set; }
        public string product_f3_IT { get; set; }
        public string product_f3_JA { get; set; }
        public string product_f3_NO { get; set; }
        public string product_f3_PTBR { get; set; }
        public string product_f3_ES { get; set; }
        public string product_f3_SV { get; set; }
        public string product_f3_FI { get; set; }
        public string product_f3_RU { get; set; }

        public string product_f4_EN { get; set; }
        public string product_f4_DA { get; set; }
        public string product_f4_NL { get; set; }
        public string product_f4_FR { get; set; }
        public string product_f4_DE { get; set; }
        public string product_f4_IT { get; set; }
        public string product_f4_JA { get; set; }
        public string product_f4_NO { get; set; }
        public string product_f4_PTBR { get; set; }
        public string product_f4_ES { get; set; }
        public string product_f4_SV { get; set; }
        public string product_f4_FI { get; set; }
        public string product_f4_RU { get; set; }


        public string product_fI1_EN { get; set; }
        public string product_fI1_DA { get; set; }
        public string product_fI1_NL { get; set; }
        public string product_fI1_FR { get; set; }
        public string product_fI1_DE { get; set; }
        public string product_fI1_IT { get; set; }
        public string product_fI1_JA { get; set; }
        public string product_fI1_NO { get; set; }
        public string product_fI1_PTBR { get; set; }
        public string product_fI1_ES { get; set; }
        public string product_fI1_SV { get; set; }
        public string product_fI1_FI { get; set; }
        public string product_fI1_RU { get; set; }

        public string product_fI2_EN { get; set; }
        public string product_fI2_DA { get; set; }
        public string product_fI2_NL { get; set; }
        public string product_fI2_FR { get; set; }
        public string product_fI2_DE { get; set; }
        public string product_fI2_IT { get; set; }
        public string product_fI2_JA { get; set; }
        public string product_fI2_NO { get; set; }
        public string product_fI2_PTBR { get; set; }
        public string product_fI2_ES { get; set; }
        public string product_fI2_SV { get; set; }
        public string product_fI2_FI { get; set; }
        public string product_fI2_RU { get; set; }

        public string product_fI3_EN { get; set; }
        public string product_fI3_DA { get; set; }
        public string product_fI3_NL { get; set; }
        public string product_fI3_FR { get; set; }
        public string product_fI3_DE { get; set; }
        public string product_fI3_IT { get; set; }
        public string product_fI3_JA { get; set; }
        public string product_fI3_NO { get; set; }
        public string product_fI3_PTBR { get; set; }
        public string product_fI3_ES { get; set; }
        public string product_fI3_SV { get; set; }
        public string product_fI3_FI { get; set; }
        public string product_fI3_RU { get; set; }

        public string product_fI4_EN { get; set; }
        public string product_fI4_DA { get; set; }
        public string product_fI4_NL { get; set; }
        public string product_fI4_FR { get; set; }
        public string product_fI4_DE { get; set; }
        public string product_fI4_IT { get; set; }
        public string product_fI4_JA { get; set; }
        public string product_fI4_NO { get; set; }
        public string product_fI4_PTBR { get; set; }
        public string product_fI4_ES { get; set; }
        public string product_fI4_SV { get; set; }
        public string product_fI4_FI { get; set; }
        public string product_fI4_RU { get; set; }

        public Dictionary<string, string> dictFeatures = new Dictionary<string, string>();

    }

  


}
