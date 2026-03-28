using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DuplicatePhotosFixer.App_code;
using DuplicatePhotosFixer.Helper;
using static DuplicatePhotosFixer.Views.cNagDialogue;

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmInstallUtilities.xaml
    /// </summary>
    public partial class frmInstallUtilities : Window
    {

        private Product oRecommendedItemsData { get; set; }
        private bool bInstallUtiliyTracking = false;
        private bool bInstallHere = false;
        public frmInstallUtilities(Product RecommendedItemsData)
        {
            InitializeComponent();
            oRecommendedItemsData = RecommendedItemsData;

            this.DataContext = oRecommendedItemsData;
            Init();
        }

        void Init()
        {
            LoadStrings();
        }

        private void LoadStrings()
        {
            try
            {
                if (oRecommendedItemsData.ProductId == "12")
                {
                    if (cGlobalSettings.oScanSettings.LanguageCode == "fr" || cGlobalSettings.oScanSettings.LanguageCode == "de")
                        txt_featureInfo1.FontSize = txt_featureInfo2.FontSize = txt_featureInfo3.FontSize = txt_featureInfo4.FontSize = 11.5;
                    else
                        txt_featureInfo1.FontSize = txt_featureInfo2.FontSize = txt_featureInfo3.FontSize = txt_featureInfo4.FontSize = 12;
                }
                img_banner.Source = oRecommendedItemsData.imgBannerLogo;

                if (cGlobalSettings.oScanSettings.LanguageCode.ToLower() == "en")
                {
                    lbl_info.Text = string.Format(cResourceManager.LoadString("IDS_AGREE_INSTALL"), oRecommendedItemsData.ProductName);
                }
                else
                {
                    lbl_info.Text = cResourceManager.LoadString("IDS_UTILITY_BYINSTALLING");
                }

                txt_feature1.Text = oRecommendedItemsData.dictFeatures["0"];
                txt_featureInfo1.Text = oRecommendedItemsData.dictFeatures["1"];

                txt_feature2.Text = oRecommendedItemsData.dictFeatures["2"];
                txt_featureInfo2.Text = oRecommendedItemsData.dictFeatures["3"];

                txt_feature3.Text = oRecommendedItemsData.dictFeatures["4"];
                txt_featureInfo3.Text = oRecommendedItemsData.dictFeatures["5"];

                txt_feature4.Text = oRecommendedItemsData.dictFeatures["6"];
                txt_featureInfo4.Text = oRecommendedItemsData.dictFeatures["7"];


                pnl1.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["0"]) ? Visibility.Hidden : Visibility.Visible;
                txt_featureInfo1.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["1"]) ? Visibility.Hidden : Visibility.Visible;

                pnl2.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["2"]) ? Visibility.Hidden : Visibility.Visible;
                txt_featureInfo2.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["3"]) ? Visibility.Hidden : Visibility.Visible;

                pnl3.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["4"]) ? Visibility.Hidden : Visibility.Visible;
                txt_featureInfo3.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["5"]) ? Visibility.Hidden : Visibility.Visible;

                pnl4.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["6"]) ? Visibility.Hidden : Visibility.Visible;
                txt_featureInfo4.Visibility = string.IsNullOrEmpty(oRecommendedItemsData.dictFeatures["7"]) ? Visibility.Hidden : Visibility.Visible;

                lbl_ProductName.Text = cResourceManager.LoadString("INSTALLUTILITY_HEADING");
                btn_Install.Content = cResourceManager.LoadString("PKS_SING_INSTALL");
                btn_Cancel.Content = cResourceManager.LoadString("IDS_CANCEL");
                txtAnd.Text = cResourceManager.LoadString("IDS_UTILITY_AND");
                txt_EULA.Text = cResourceManager.LoadString("IDS_UTILITY_EULA");
                tb_privacyPolicy.Text = cResourceManager.LoadString("IDS_UTILITY_PRIVACYPOLICY");

                if (cGlobalSettings.oScanSettings.LanguageCode == "ja")
                {
                    txt_productName.FontSize = 25;
                    //lbl_info.FontSize = txt_EULA.FontSize = txtAnd.FontSize = tb_privacyPolicy.FontSize = 10.2;
                }

                if (cGlobalSettings.oScanSettings.LanguageCode == "de")
                {
                    txt_productName.FontSize = 25;
                    //lbl_info.FontSize = txt_EULA.FontSize = txtAnd.FontSize = tb_privacyPolicy.FontSize = 11.5;
                }

                if (cGlobalSettings.oScanSettings.LanguageCode == "ru")
                {
                    lbl_info.FontSize = txt_EULA.FontSize = txtAnd.FontSize = tb_privacyPolicy.FontSize = 14.5;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmInstallUtilities:: LoadStrings: ", ex);
            }
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cGlobal.frmNagReturnCode = FORM_RETURN_CODES.DO_NOTHING;

                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.Close.ToString(), oRecommendedItemsData.ProductName.Replace(" ", "")), cTrackingParameters.eProductName.InstallUtilityWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bInstallUtiliyTracking, 0, (int)cTrackingParameters.eReminderEventType.close);
                this.Close();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmInstallUtilities :: close_btn_Click : ", ex);
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            cGlobal.frmNagReturnCode = FORM_RETURN_CODES.DO_NOTHING;

            App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_CancelInstall.ToString(), oRecommendedItemsData.ProductName.Replace(" ", "")), cTrackingParameters.eProductName.InstallUtilityWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bInstallUtiliyTracking, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonprimary);
            this.Close();

        }

        private void btn_Install_Click(object sender, RoutedEventArgs e)
        {
            cGlobal.frmNagReturnCode = FORM_RETURN_CODES.InstallUtility;

            if (bInstallHere)
            {
                switch (cGlobal.frmNagReturnCode)
                {
                    case FORM_RETURN_CODES.InstallUtility:
                        {
                            if (oRecommendedItemsData.nInstallSatus == (int)Helper.eInstallSatus.Installed)
                            {
                                /// Launch application
                                System.Diagnostics.Process.Start(oRecommendedItemsData.strCompletePath);
                                return;
                            }
                            // Start download and install process
                            new Thread(() =>
                            {
                                try
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        oRecommendedItemsData.nInstallSatus = (int)Helper.eInstallSatus.Installing;
                                        oRecommendedItemsData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Installing.ToString().ToUpper());
                                        btn_Install.Content = oRecommendedItemsData.strInstallSatus;
                                        btn_Cancel.IsEnabled = false;
                                        //UpdateData();
                                        //utilityAppsListView.Items.Refresh();
                                        //utilityAppsListView.UpdateLayout();
                                    }));
                                    Thread.Sleep(100);
                                    cClientEnum.eDownloadStatus downloadStatus;
                                    cGlobalSettings.DownloadNInstallRecommandedSetup(oRecommendedItemsData.ProductName, oRecommendedItemsData.DownloadLink, false, out downloadStatus);

                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        oRecommendedItemsData.strCompletePath = oRecommendedItemsData.strCompletePath;

                                        if (downloadStatus == cClientEnum.eDownloadStatus.DownloadSucessFull && System.IO.File.Exists(oRecommendedItemsData.strCompletePath))
                                        {
                                            oRecommendedItemsData.nInstallSatus = (int)Helper.eInstallSatus.Installed;
                                            oRecommendedItemsData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Installed.ToString().ToUpper());
                                            btn_Install.Content = oRecommendedItemsData.strInstallSatus;
                                        }
                                        else
                                        {
                                            if (downloadStatus == cClientEnum.eDownloadStatus.NoInternetConnection) //have to make nag of messageBox
                                            {
                                                //MessageBoxWithCheckBox.SimpleMsgBox(App.oMainReference, cResourceManager.LoadString("IDS_CHECK_INTERNET"), cGlobalSettings.GetProductNameFromDesc() + " ", MessageBoxButton.OK, MessageBoxImage.Information);
                                            }

                                            oRecommendedItemsData.nInstallSatus = (int)Helper.eInstallSatus.Not_Installed;
                                            oRecommendedItemsData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Not_Installed.ToString().ToUpper());
                                            btn_Install.Content = oRecommendedItemsData.strInstallSatus;
                                        }

                                        btn_Cancel.IsEnabled = false;
                                        //UpdateData();
                                    }));
                                }
                                catch (Exception ex)
                                {
                                    cGlobalSettings.oLogger.WriteLogException("install dmf: ", ex);
                                }
                            }).Start();
                        }
                        App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_InstallNow.ToString(), oRecommendedItemsData.ProductName.Replace(" ", "")), cTrackingParameters.eProductName.ActionCenterWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, false, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonprimary);
                        break;
                    case FORM_RETURN_CODES.DO_NOTHING:
                    default:
                        break;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void txt_EULA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_Eula.ToString(), ""), cTrackingParameters.eProductName.InstallUtilityWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bInstallUtiliyTracking, 0, (int)cTrackingParameters.eReminderEventType.other);
                Process.Start(oRecommendedItemsData.Eula);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmInstallUtilities :: txt_EULA_MouseDown : ", ex);
            }
        }

        private void tb_privacyPolicy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_PrivacyPolicy.ToString(), ""), cTrackingParameters.eProductName.InstallUtilityWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bInstallUtiliyTracking, 0, (int)cTrackingParameters.eReminderEventType.other);
                Process.Start(oRecommendedItemsData.PP);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmInstallUtilities :: tb_privacyPolicy_MouseDown : ", ex);
            }
        }
    }
}
