using System;
using System.Threading;
using System.Windows;
using DuplicatePhotosFixer.Models;

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmProcessing.xaml
    /// </summary>
    /// 

    public partial class frmProcessing : Window
    {
        public frmProcessing()
        {
            PreInit();
            InitializeComponent();
            LoadStrings();




        }

        void PreInit()
        {
            try
            {
                if (App.oMainReference.objScanProgressViewModel == null)
                {
                    App.oMainReference.objScanProgressViewModel = new vmScanProgress();
                }
                this.DataContext = App.oMainReference.objScanProgressViewModel;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("PreInit", ex);
            }
        }

        void LoadStrings()
        {

            btn_cancel.Content = cResourceManager.LoadString("IDS_CANCEL");
            lbl_heading.Content = cGlobalSettings.GetProductName();
            //lbl_title.Content = "";
            lbl_subInfo2.Text = string.Format(cResourceManager.LoadString("DPF_PHOTOS_COMPARED"), "").Trim();

            lbl_scan.Text = string.Format("{0}:", cResourceManager.LoadString("IDS_SCAN_PATH"));
            //btn_yes.Text = cResourceManager.LoadString("IDS_YES");
            //lbl_infoRaw.Text = cResourceManager.LoadString("IDS_COMPONENT_IMPORTANCE");
        }

        // FIX: Make sure OnProgress updates the UI thread
        public void OnProgress(cClientEnum.eScanPhase ScanPhase, string strMsg, int percentageCompleted, int nTotalFiles, int nTotalProcess, int nTotalDups)
        {
            // This should be called from your SystemScanHelper's SendProgress callback
            App.oMainReference.objScanProgressViewModel.OnScanProgress(ScanPhase, strMsg, percentageCompleted, nTotalFiles, nTotalProcess, nTotalDups);
        }

        // Rest of your methods...
        public void OnStart()
        {
            App.oMainReference.objScanProgressViewModel.OnScanStart();
        }

        public void OnComplete()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("frmProcessing.OnComplete() called - Showing results");

                // Run diagnostic to verify duplicates
                cGlobalSettings.objMainScanHelper.DiagnosticCheckDuplicates();

                // Close the progress window
               this.Close();

                // Show results UI
               
                ///just for show case have to add after scan nag first before showing Result UI
                ///
               App.oMainReference.ShowResults();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucScanProgress:: OnComplete: ", ex);
            }
        }

        private void btnStopScan_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objScanProgressViewModel.OnManualScanStop();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("btn_close_Click::", ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    App.oMainReference.StartScan();
                })
                { Name = "ThScanning" }.Start();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmProcessing:: Window_Loaded: ", ex);
            }
        }
    }
}

#if PreviousCode
    //public partial class frmProcessing : Window
    //{
    //    public frmProcessing()
    //    {
    //        InitializeComponent();

    //        LoadStrings();
    //        PreInit();
    //        //LoadStringsForScan();

    //    }

    //    void PreInit()
    //    {
    //        try
    //        {
    //            App.oMainReference.objScanProgressViewModel = new vmScanProgress();
    //            this.DataContext = App.oMainReference.objScanProgressViewModel;
    //        }
    //        catch (Exception ex)
    //        {
    //            cGlobalSettings.oLogger.WriteLogException("ucScanProgress :: PreInit : ", ex);
    //        }
    //    }

    //    void LoadStrings()
    //    {

    //        btn_cancel.Content = cResourceManager.LoadString("IDS_CANCEL");
    //        lbl_heading.Content = cGlobalSettings.GetProductName();
    //        //lbl_title.Content = "";
    //        lbl_subInfo2.Text = string.Format(cResourceManager.LoadString("DPF_PHOTOS_COMPARED"), "").Trim();

    //        lbl_scan.Text = string.Format("{0}:", cResourceManager.LoadString("IDS_SCAN_PATH"));
    //        //btn_yes.Text = cResourceManager.LoadString("IDS_YES");
    //        //lbl_infoRaw.Text = cResourceManager.LoadString("IDS_COMPONENT_IMPORTANCE");
    //    }

    //    public void OnStart()
    //    {
    //        App.oMainReference.objScanProgressViewModel.OnScanStart();

    //    }

    //    /// <summary>
    //    /// Show on process is in progress.
    //    /// </summary>
    //    /// <param name="strMsg"> String message to show </param>
    //    /// <param name="nTotalFiles"> Total files found count </param>
    //    /// <param name="nTotalProcess"> Total files processed </param>
    //    /// <param name="nTotalDups">Total duplicates found </param>
    //    public void OnProgress(string strMsg, int percentageCompleted, int nTotalFiles, int nTotalProcess, int nTotalDups)
    //    {

    //        App.oMainReference.objScanProgressViewModel.OnScanProgress(strMsg, percentageCompleted, nTotalFiles, nTotalProcess, nTotalDups);

    //    }

    //    public void OnComplete()
    //    {
    //        try
    //        {


    //        }
    //        catch (Exception ex)
    //        {
    //            cGlobalSettings.oLogger.WriteLogException("ucScanProgress:: OnComplete: ", ex);
    //        }
    //    }

    //    private void btnStopScan_Click(object sender, RoutedEventArgs e)
    //    {
    //        App.oMainReference.objScanProgressViewModel.OnManualScanStop();
    //    }


    //private void btn_close_Click(object sender, RoutedEventArgs e)
    //    {
    //        //this.Close();
    //        try
    //        {

    //            Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            cGlobalSettings.oLogger.WriteLogException("btn_close_Click::", ex);
    //        }
    //    }
    //}

}


#endif



