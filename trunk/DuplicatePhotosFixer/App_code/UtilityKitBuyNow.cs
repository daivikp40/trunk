using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DuplicatePhotosFixer.App_Code;
using DuplicatePhotosFixer.CheckForUpdates;
using DuplicatePhotosFixer.Helper;
using DuplicatePhotosFixer.HelperClasses.MyLogger;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.App_code
{
    public static class UtilityKitBuyNow
    {
        public static string bFilePath = PathEx.Combine(cGlobalSettings.getCommonAppDataApplicationPath()/*Application.StartupPath*/, @"StoreBuyNow");
        public static ObservableCollection<ProductBuyNow> oProducts = null;
        public static List<ProductBuyNow> lstProducts = null;
        public static Version currentVersion;
        public static int servicePack = 0;
        static string trackingString = "_fromdffstoreBuyNow";

        static string _GetPath = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Store_BuyNow.ini");
        //@"c:\test\Store_BuyNow.ini";
        // Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Store_BuyNow.ini");

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
            [System.ComponentModel.Description("Advanced System Protector")]
            ASP,
            [System.ComponentModel.Description("RegClean Pro")]
            RCP,
            [System.ComponentModel.Description("Right Backup")]
            RB,
            [System.ComponentModel.Description("Advanced System Optimizer")]
            ASO,
            [System.ComponentModel.Description("Duplicate Photos Fixer Pro")]
            DPF,
            [System.ComponentModel.Description("TweakPass Password Manager")]
            TP,
            [System.ComponentModel.Description("TweakShot Screen Capture")]
            TS
        }

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
                cGlobalSettings.oLogger.WriteLog(string.Format("StoreBuyNow:SaveImage|", ex.Message));
            }

            return filename;
        }


        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        public static string GetPath()
        {
            return Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Store_BuyNow.ini");
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
                cGlobalSettings.oLogger.WriteLog(string.Format("StoreBuyNow:GetImage|", ex.Message));
            }

            return bitmapImage;
        }

        public static List<ProductBuyNow> LoadProducts()
        {
            oProducts = new ObservableCollection<ProductBuyNow>();

            try
            {
                if (!Directory.Exists(bFilePath))
                {
                    Directory.CreateDirectory(bFilePath);
                }

                for (int i = 1; i <= GetTotalProducts(); i++)
                {

                    ProductBuyNow oProduct = new ProductBuyNow();
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
                    oProduct.DownloadLink = string.Format(oProduct.DownloadLink, cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN + trackingString);
                    oProduct.ProductDetails = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_details", _GetPath);

                    oProduct.imgProductLogo = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "image_url", _GetPath);
                    string bUrl = SaveImage(bProductFilePath, oProduct.imgProductLogo, "imgProductLogo");
                    oProduct.imgProductLogo = bUrl;

                    oProduct.imgBanner = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "imagebanner_url", _GetPath);
                    string bUrl1 = SaveImage(bProductFilePath, oProduct.imgBanner, "imgBanner");
                    oProduct.imgBannerLogo = GetImage(bUrl1);

                    oProduct.utlImage = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "utlimage_url", _GetPath);
                    string bUrl2 = SaveImage(bProductFilePath, oProduct.utlImage, "utlImage");
                    oProduct.utlImageLogo = GetImage(bUrl2);

                    oProduct.utlhImage = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "utlhimage_url", _GetPath);
                    string bUrl3 = SaveImage(bProductFilePath, oProduct.utlhImage, "utlhImage");
                    oProduct.utlhImageLogo = GetImage(bUrl3);

                    oProduct.BuyNowUrl = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_buynowurl", _GetPath);
                    oProduct.BuyNowUrl += string.Format("&utm_campaign={0}{1}", cGlobalSettings.UTM_CAMPAIGN, trackingString);
                    oProduct.BuyNowUrl = cGlobal.GetParams(oProduct.BuyNowUrl);

                    oProduct.IsMain = Convert.ToInt16(cWin32APIs.IniReadValue("product" + Convert.ToString(i), "IsMain", _GetPath));
                    oProduct.IsLaunchNow = Convert.ToInt16(cWin32APIs.IniReadValue("product" + Convert.ToString(i), "IsLaunchNow", _GetPath));

                    string content = string.Empty;
                    if (oProduct.IsLaunchNow == 0)
                        content = cResourceManager.LoadString("DFF_SCAN_NOW");
                    else
                        content = cResourceManager.LoadString("IDS_INSTALLSATUS_INSTALLED");


                    oProduct.Discount = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_dis", _GetPath) + "%";
                    oProduct.Symbol = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_symbol", _GetPath);

                    string price = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_cost", _GetPath);
                    if (!string.IsNullOrEmpty(price))
                        oProduct.Price = price + " " + oProduct.Symbol;

                    string discountedPrice = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "product_discost", _GetPath);
                    if (!string.IsNullOrEmpty(discountedPrice))
                        oProduct.DiscountedPrice = discountedPrice + " " + oProduct.Symbol;

                    oProduct.Eula = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "EULA", _GetPath);
                    oProduct.PP = cWin32APIs.IniReadValue("product" + Convert.ToString(i), "PrivacyPolicy", _GetPath);


                    cGlobalSettings.GetProductDetails(out oInstallState, out nRPTotalErrorFound, oProduct.ProductKey, oProduct.sProductFolderName, oProduct.sExename, oProduct.ProductKeyValue, out sCompletePath);
                    oProduct.strCompletePath = sCompletePath;
                    oProduct.nInstallSatus = (oInstallState == cClientEnum.PBInstalledState.Installed) ? (int)eInstallSatus.Installed : (int)eInstallSatus.Not_Installed;


                    oProduct.IsCompatible = true;
                    oProduct.IsLangCompatible = true;
                    oProduct.IsPriceCompatible = true;

                    oProduct.IsInstalledRegistered = false;
                    oProduct.IsInstalledRegistered = IsInstalledRegistered(oInstallState, oProduct.ProductKey);
                    oProduct.IsVisible = !oProduct.IsInstalledRegistered ? Visibility.Visible : Visibility.Collapsed;
                    oProduct.strInstallSatus = !oProduct.IsInstalledRegistered ? cResourceManager.LoadString("PKS_STR_BUY_NOW") : content;


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


                    if (string.IsNullOrEmpty(oProduct.Price))
                    {
                        oProduct.IsPriceCompatible = false;
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
                cGlobalSettings.oLogger.WriteLogException("UtilityKitBuyNew:: LoadProducts: ", ex);
            }

            return lstProducts = oProducts.Where(x => x.IsCompatible == true && x.IsLangCompatible == true && x.IsPriceCompatible == true).OrderByDescending(x => x.IsInstalledRegistered == false).ToList();
        }

        public static void DownloadandINstallUtlKit()
        {
            try
            {

                cUpdator oUpdater = new cUpdator();

                string surl = cGlobalSettings.GetUtlKitUrlBuyNow();
                string ResultValue = string.Empty;

                for (int nCheckIniRetry = 0; nCheckIniRetry < cGlobalSettings.CHECK_FOR_UPDATE_INI_RETRY_COUNT; nCheckIniRetry++)
                {
                    if (oUpdater.DownloadWebPage(surl, out ResultValue))
                    {
                        break;
                    }


                    Thread.Sleep(cGlobalSettings.CHECK_FOR_UPDATE_RETRY_INTERVAL); //sleep for 5 seconds before next retry
                }

                string TempPathForUpdate = Path.Combine(cGlobalSettings.getAppDataApplicationPath(), "Store_BuyNow.ini");

                FileStream oStream = new FileStream(TempPathForUpdate, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter oWriter = new StreamWriter(oStream, System.Text.Encoding.Unicode);
                oWriter.Write(ResultValue);
                oWriter.Close(); oStream.Close();

                //Thread.Sleep(100);

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("UtilityKitBuyNow::DownloadandINstallUtlKit|", ex.Message));
                // MessageBoxEx.Show("unable to get GetArmStatus()\r\n" + ex.Message, cGlobalSettings.GetProductNameFromDesc());
            }

        }

        public static bool IsInstalledRegistered(cClientEnum.PBInstalledState InstalledState, string sKey)
        {
            bool bIsInstalledRegistered = false;
            string sRegistered = string.Empty;
            try
            {
                if (InstalledState == cClientEnum.PBInstalledState.Installed)
                {
                    if (Registry.CurrentUser != null)
                    {
                        if (Registry.CurrentUser.OpenSubKey(sKey) != null)
                        {
                            sRegistered = Convert.ToString(Registry.CurrentUser.OpenSubKey(sKey).GetValue(cRegistrySettings.REGSTR_REGVER, string.Empty));

                            if (string.IsNullOrEmpty(sRegistered) || sRegistered == "0")
                            {
                                if (Registry.LocalMachine != null)
                                {
                                    if (Registry.LocalMachine.OpenSubKey(sKey) != null)
                                    {
                                        sRegistered = Convert.ToString(Registry.LocalMachine.OpenSubKey(sKey).GetValue(cRegistrySettings.REGSTR_REGVER, string.Empty));
                                        if (string.IsNullOrEmpty(sRegistered) || sRegistered == "0")
                                        {
                                            bIsInstalledRegistered = false;
                                        }
                                        else
                                        {
                                            bIsInstalledRegistered = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bIsInstalledRegistered = true;
                            }
                        }
                    }
                }
                else
                {
                    bIsInstalledRegistered = false;
                }

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("UtilitkitBuyNow:: GetProductDetails: ", ex);
            }

            return bIsInstalledRegistered;
        }


    }

    public class ProductBuyNow
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string Discount { get; set; }

        public string Price { get; set; }

        public string DiscountedPrice { get; set; }

        public string Symbol { get; set; }

        public string DownloadLink { get; set; }

        public string BuyNowUrl { get; set; }

        public string imgProductLogo { get; set; }

        public string imgBanner { get; set; }

        public BitmapImage imgBannerLogo { get; set; }

        public string utlImage { get; set; }

        public BitmapImage utlImageLogo { get; set; }

        public string utlhImage { get; set; }

        public BitmapImage utlhImageLogo { get; set; }

        public int IsMain { get; set; }

        public int IsLaunchNow { get; set; }

        public Visibility IsVisible { get; set; }

        public string ProductDetails { get; set; }

        public string ProductKey { get; set; }

        public string ProductKeyValue { get; set; }
        public string sProductFolderName { get; set; }
        public string sExename { get; set; }

        public string strCompletePath { get; set; }

        public int nInstallSatus { get; set; }


        public string strInstallSatus { get; set; }
        public bool IsCompatible { get; set; }

        public bool IsLangCompatible { get; set; }

        public bool IsPriceCompatible { get; set; }

        public bool IsInstalledRegistered { get; set; }
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
