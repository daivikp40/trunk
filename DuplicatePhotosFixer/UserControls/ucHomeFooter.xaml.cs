using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine;
using DuplicatePhotosFixer.Helper;
using DuplicatePhotosFixer.HelperClasses.MyLogger;
using DuplicatePhotosFixer.Views;
using static DuplicatePhotosFixer.Helper.cTrackingParameters;
using static DuplicatePhotosFixer.Views.cNagDialogue;
using static DuplicatePhotosFixer.Views.frmProcessing;

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucHomeFooter.xaml
    /// </summary>
    public partial class ucHomeFooter : UserControl
    {
        private bool bHomeTracking = false;

        public ucHomeFooter()
        {
            App.oMainReference.objHomeFooter = new Models.vmHomeFooter();
            this.DataContext = App.oMainReference.objHomeViewModel;

            InitializeComponent();

            LoadString();
            ValidateVideoTutorial();
        }
        public IgnoreRules ignoreRules = null;
        public ScanPhotos objScanPhotos = null;

        void LoadString()
        {
            try
            {
                btnUpgradeNow.Content = cResourceManager.LoadString("DPF_FOOTER_UC_BUY_NOW");
                btnScanForDuplicates.Content = cResourceManager.LoadString("DPF_FOOTER_UC_SCAN_FOR_DUPLICATES_TEXT");
                

            }
            catch (Exception ex)
            {

                
            }
        }


        public void Init()
        {
            App.oMainReference.objHomeFooter.ctrlOwner = this;

            App.oMainReference.objHomeFooter.IsScanbtnEnabled();
            if (ignoreRules == null)
                ignoreRules = new IgnoreRules();
            ignoreRules.GenerateRegEx();

            //if (objScanPhotos == null)
            //{
            //    objScanPhotos = new ScanPhotos();
            //    objScanPhotos.OnScanStart += new ScanPhotos.OnScanStartProcess(objScanPhotos_OnScanStart);
            //    objScanPhotos.OnScanProgress += new ScanPhotos.OnScanProgressProcess(objScanPhotos_OnScanProgress);
            //    objScanPhotos.OnScanCompleted += new ScanPhotos.OnScanCompletedProcess(objScanPhotos_OnScanCompleted);
            //    objScanPhotos.onGroupCreated += new ScanPhotos.onGroupCreatedProcess(objScanPhotos_onGroupCreated);
            //}

            //if (cGlobalSettings.scanner != null)
            //{

            //    cGlobalSettings.scanner.OnScanStart += new OnScanStartProcess(scanner_OnStartProgress);
            //  // cGlobalSettings.scanner.OnScanProgress += new OnScanProgressProcess(scanner_OnScanProgress);
            //    cGlobalSettings.scanner.OnScanCompleted += new OnScanCompletedProcess(scanner_OnScanCompleted);

            //}

        }

        void objScanPhotos_onGroupCreated(float matchingLevel)
        {
            try
            {

                /*Program.oMainReference.UIThread(() =>
                {*/
                //App.oMainReference.oResult.StartCoolDownTimer();
                /*});*/
                cGlobalSettings.bShowAfterScanMarkWindow = true;
                //App.oMainReference.ucResult_showDuplicates(matchingLevel);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("objScanPhotos_onGroupCreated:: ", ex);
            }
        }
        public static ReaderWriterLockSlim ScanLock = new ReaderWriterLockSlim();
        void objScanPhotos_OnScanStart(string startMsg)
        {
            //this.UIThread(() =>
            //{
            //    WindowsFormsSynchronizationContext.Current.Post(_ =>
            //    {
            //        Program.oMainReference.ShowProgress(startMsg);
            //    }, null);
            //});
        }

       
        void scanner_OnStartProgress(string message, int percentageCompleted, int counter)

        {
            //this.UIThread(() =>
            //{

            //});
        }


       
        private void btnScanForDuplicates_Click(object sender, RoutedEventArgs e)
        {
            //App.oMainReference.Home_startCompare();

            

            App.oMainReference.StartScanTh();
            //App.oMainReference.ShowResults();


            //try
            //{

            //    //foreach (var item in selectedFiles)
            //    //{
            //    //    if (Directory.Exists(item.filePath))
            //    //    {
            //    //        //string[] files = 
            //    //    }
            //    //}

            //}
            //catch (Exception ex)
            //{

            //}

        }

        public void showScanDuplicateButton(int btnStatus, bool visibleScanButton, bool enableScanButton)
        {
           
        }


        private void btn_footer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.oMainReference.FireEventTrackingPixelInThread("{0}", cNagDialogue.eUTM_BTN_TYPE.BtnFooterWatchTut.ToString());
                string sUrl = cGlobalSettings.AppTutorialLink;// @"Zvp-UHc4s3Q"; Get Link from registry

                if (string.IsNullOrEmpty(sUrl))
                    return;

                App.oMainReference.StartYTTutorial(sUrl);

            }
            catch (Exception ex)
            {

                
            }
        }

        bool ValidateVideoTutorial()
        {
            bool bShow = false;
            try
            {
                if (cGlobalSettings.oScanSettings.LanguageCode.ToLower() != "en")
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Tutorial btn not shown: Language not supported.");
                    return bShow;
                }
                if (!cGlobalSettings.ShowYoutubeFooterBtn())
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Tutorial btn not shown: Disabled from update ini.");
                    return bShow;
                }
                if (cGlobalSettings.getIEVersion() < 11)
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Tutorial btn not shown: IE is not supported.");
                    return bShow;
                }
                string strUrl = cGlobalSettings.AppTutorialLink;
                if (string.IsNullOrEmpty(strUrl))
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Tutorial btn not shown: Video link not found.");
                    return bShow;
                }
                bShow = true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucFooter:: ShowVideoTutorial: ", ex);
            }
            return bShow;
        }

        private void btnUpgradeNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                //if (cGlobal.IS_LB_BUILD == 0)
                //    cGlobal.BuyNow(System.Windows.Forms.eDiscountOffers.None, System.Windows.Forms.eUTM_BTN_TYPE.HomeUpgrade.ToString());

                cGlobal.NagParentStr = cTrackingParameters.eProductName.ActivateNowTab.ToString();
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_ActivateNow.ToString(), ""), cTrackingParameters.eProductName.MainWindow.ToString(), true, bHomeTracking, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonsecondary);
                if (App.oMainReference.frmRegistration == null)
                App.oMainReference.frmRegistration = new frmRegistrationDPF();

                App.oMainReference.Progress_Show(App.oMainReference.frmRegistration, App.oMainReference, true);
                App.oMainReference.Progress_Close(App.oMainReference.frmRegistration);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("ucHomeFooter |btnUpgradeNow_Click", ex.Message));
            }
        }
        /*
private void llbl_export_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
{
   cExportToCSV.ExportCurrentResultDataToCSV(true);
}
*/
    }
}

