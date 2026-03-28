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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer.Models;
using static DuplicatePhotosFixer.enumClass;

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucHomeComparisonMethod.xaml
    /// </summary>
    public partial class ucHomeComparisonMethod : UserControl
    {

        vmHomeComparisonMethod objModel = null;

        public ucHomeComparisonMethod()
        {
            PreInit();
            InitializeComponent();
            Init();
        }

        void PreInit()
        {
            try
            {
                objModel = new vmHomeComparisonMethod();
                this.DataContext = objModel;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHomeComparisonMethod:: PreInit: ", ex);
            }
        }

        void Init()
        {
            try
            {
                ucTimeIntervalSettings.DataContext = objModel;

                LoadStrings();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHomeComparisonMethod:: Init: ", ex);
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUIData();
       }

        public void UpdateUIData()
        {
            try
            {
                objModel.SetUIPropertiesValue();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHomeComparisonMethod:: UpdateUIData: ", ex);
            }
        }

        private void LoadStrings()
        {
            lbl_comparisonMethod.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_COMPARISON_METHOD");
            rdbtnExactMatch.Content = cResourceManager.LoadString("DPF_SETTINGS_UC_EXACT_MATCH");
            rdbtnSimilarMatch.Content = cResourceManager.LoadString("DPF_SETTINGS_UC_SIMILAR_MATCH");
            lbl_matchingLevel.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_MATCHING_LEVEL");
            lbl_lowMatching.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_MATCHING_LEVEL_LOW");
            lbl_highMatching.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_MATCHING_LEVEL_HIGH");
            lbl_bitmapSize.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_BITMAP_SIZE");
            lbl_timeInterval.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_TIME_INTERVAL");
            lbl_GPS.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_GPS");
            lbl_performance.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_PERFORMANCE");
            lbl_quality.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_QUALITY");
            lbl_moreTime.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_TIME_INTERVAL_MORE");
            lbl_lessTime.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_TIME_INTERVAL_LESS");
            lbl_LessGPS.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_GPS_LESS");
            lbl_MoreGPS.Text = cResourceManager.LoadString("DPF_SETTINGS_UC_GPS_MORE");
            loadDefault_btn.Content = cResourceManager.LoadString("DPF_SETTINGS_UC_LOAD_DEFAULT");
            chk_rotatedImages.Content = cResourceManager.LoadString("IDS_INCLUDE_ROTATED_IMAGES");
            chk_flippedImages.Content = cResourceManager.LoadString("IDS_INCLUDE_FLIPPED_IMAGES");

            //llbl_includeGPS.Text = llbl_includeTime.Text = cResourceManager.LoadString("IDS_INCLUDE");
        }
        /*
        private void matchLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            cGlobalSettings.MatchSettingsData.MatchLevel = (eMatchingLevel)matchLevelSlider.Value;
            matchLevelSlider.ToolTip = (eMatchingLevel)matchLevelSlider.Value;
            objModel.SaveMatchSettings();
        }
        */
        private void loadDefault_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objModel.LoadScanMethodSettings(true);
                UpdateUIData();
               
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("loadDefault_btn_Click::", ex);
            }

        }
    }
}
