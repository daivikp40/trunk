using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DuplicatePhotosFixer.Helpers;
using static DuplicatePhotosFixer.Views.cNagDialogue;

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmAfterScan.xaml
    /// </summary>
    public partial class frmAfterScan : Window
    {
        public frmAfterScan()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            LoadStrings();

        }

        void LoadStrings()
        {
            try
            {
                lbl_heading.Content = cGlobalSettings.GetProductName();
                lbl_title.Text = "";

                label1.Text = cResourceManager.LoadString("IDS_NAG_MSG_DUPLICATE_FOUND");
                //label3.Text = cResourceManager.LoadString("IDS_AFTER_SCAN_USE_AUTO_MARK");

                btn_selectManually.Content = cResourceManager.LoadString("IDS_SELECT_MANUALLY");
                btn_autoMark.Content = cResourceManager.LoadString("DPF_RESULT_UC_AUTO_MARK_TEXT");
                label10.Content = cResourceManager.LoadString("DPF_SCANNED");
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmAfterScan:: LoadStrings: ", ex);
            }
        }

        long totalDuplicateCount = 0;
        private void frmAfterScanMark_Loaded(object sender, RoutedEventArgs e)
        {
            App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.Shown.ToString(), eOfferName.AfterScanMark.ToString()));

            totalDuplicateCount = cGlobalSettings.TotalDuplicatePhotos;
            cbtn_totalCount.Text = totalDuplicateCount + "";
            lbl_tip.Text = string.Format(cResourceManager.LoadString("IDS_AFTER_SCAN_TIP"), totalDuplicateCount, AppFunctions.StrFormatByteSize(cGlobalSettings.oRegSettings.GetTotalDuplicatesSpace()));
        }

        void CloseForm()
        {
            try
            {
                cGlobal.frmNagReturnCode = FORM_RETURN_CODES.DO_NOTHING;

                this.Close();

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.BtnClose.ToString(), eOfferName.AfterScanMark.ToString()));
            CloseForm();
        }

        private void btn_selectManually_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.BtnMarkManually.ToString(), eOfferName.AfterScanMark.ToString()));
            CloseForm();
        }

        private void btn_autoMark_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.BtnAutoMark.ToString(), eOfferName.AfterScanMark.ToString()));

            cGlobal.frmNagReturnCode = FORM_RETURN_CODES.AUTO_MARK;
            cGlobalSettings.abortNow = false;

            Program.oMainReference.Progress_Close(this);

        }

        
    }
}
