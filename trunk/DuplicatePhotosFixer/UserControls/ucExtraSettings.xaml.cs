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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer.Models;

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucExtraSettings.xaml
    /// </summary>
    public partial class ucExtraSettings : UserControl
    {
        
        public ucExtraSettings()
        {
            InitializeComponent();
            PreInit();
            Init();
        }

        void PreInit()
        {
            try
            {
                if (App.oMainReference.objHomeComparisonModel == null)
                    App.oMainReference.objHomeComparisonModel = new vmHomeComparisonMethod();
                this.DataContext = App.oMainReference.objHomeComparisonModel;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucExtraSettings:: PreInit: ", ex);
            }
        }

        void Init()
        {
            try
            {
                LoadStrings();
                //UpdateUIData();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucExtraSettings:: Init: ", ex);
            }
        }
        public void LoadStrings()
        {
            scan_heading.Text = cResourceManager.LoadString("IDS_MANAGE_SCAN_CRITERIA");
            heading_info.Text = cResourceManager.LoadString("IDS_SCAN_CRITERIA_INFO");
            chk_TimeInterval.Content = cResourceManager.LoadString("IDS_CONSIDER_TIME_INTERVAL");
            lbl_TimeIntervalInfo.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_TIME_INFO");
            chk_GPS.Content = cResourceManager.LoadString("IDS_CONSIDER_GPS_SETTING");
            lbl_GPSInfo.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_GPS_INFO");
            //chk_captureDate.Content = cResourceManager.LoadString("IDS_MANAGE_SCAN_CRITERIA");
            lbl_CaptureDateInfo.Text = cResourceManager.LoadString("IDS_SETTINGS_CONSIDER_CAPTURE_DATE_INFO");
            chk_rotatedImages.Content = cResourceManager.LoadString("IDS_INCLUDE_ROTATED_IMAGES");
            chk_flippedImages.Content = cResourceManager.LoadString("IDS_INCLUDE_FLIPPED_IMAGES");

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUIData();
        }

        public void UpdateUIData()
        {
            try
            {

                App.oMainReference.objHomeComparisonModel.SetUIPropertiesValue();
     
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucExtraSettings:: UpdateUIData: ", ex);
            }
        }




        private void chkSettingsChange_Click(object sender, RoutedEventArgs e)
        {
            //App.oMainReference.objHomeComparisonModel.CheckChangeSettings();
            //App.oMainReference.objHomeComparisonModel.LoadScanMethodSettings();
            App.oMainReference.ShowSaveSettings();

        }

        public void LoadDefaults()
        {
            App.oMainReference.objHomeComparisonModel.LoadDefaultSettings();
        }

       
    }
}
