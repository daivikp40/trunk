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

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucImageInfo.xaml
    /// </summary>
    public partial class ucImageInfo : UserControl
    {
        public ucImageInfo()
        {
            InitializeComponent();
            LoadStrings();
        }

        void LoadStrings()
        {
            lbl_matchingLevel.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_MATCHING_LEVEL");
            lblPreview.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_PREVIEW");
            lbl_metadata.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_METADATA");
            lbl_fileName.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FILE_NAME");
            lbl_fileSize.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FILE_SIZE");
            lbl_fileLabel.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FILE_LABEL");
            lbl_fileCreated.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FILE_CREATED");
            lbl_fileModified.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FILE_MODIFIED");
            lbl_folderName.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FOLDER_NAME");
            lbl_rating.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_RATING");
            lbl_keywords.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_KEYWORDS");
            lbl_imageSize.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_IMAGE_SIZE");
            lbl_imageDPI.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_IMAGE_DPI");
            lbl_bitDepth.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_BIT_DEPTH");
            lbl_orientation.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_ORIENTATION");
            lbl_digitalZoomRatio.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_DIGITAL_ZOOM_RATIO");
            lbl_captureDate.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_CAPTURE_DATE");
            lbl_editingSoftware.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_EDITING_SOFTWARE");
            lbl_exposure.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_EXPOSURE");
            lbl_focalLength.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_FOCAL_LENGTH");
            lbl_exposureBias.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_EXPOSURE_BIAS");
            lbl_ISOSpeedRating.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_ISO_SPEED_RATING");
            lbl_exposureMode.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_EXPOSURE_MODE");
            lbl_exposureProgram.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_EXPOSURE_PROGRAM");
            lbl_meteringMode.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_METERING_MODE");
            lbl_lightSource.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_LIGHT_SOURCE");
            lbl_sensingMethod.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_SENSING_METHOD");
            lbl_scenceCaptureType.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_SCENCE_CAPTURE_TYPE");
            lbl_cameraMaker.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_CAMERA_MAKER");
            lbl_cameraModel.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_CAMERA_MODEL");
            lbl_cameraLensModel.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_CAMERA_LENS_MODEL");
            lbl_cameraFirmware.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_CAMERA_FIRMWARE");
            lbl_colorModel.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_COLOR_MODEL");
            lbl_colorSpace.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_COLOR_SPACE");
            lbl_profileName.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_PROFILE_NAME");
            lbl_GPSAltitude.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_GPS_ALTITUDE");
            lbl_GPSLatitude.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_GPS_LATITUDE");
            lbl_GPSLongitude.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_GPS_LONGITUDE");

            //lbl_noPreview.Text = cResourceManager.LoadString("DPF_IMAGEINFO_UC_NO_PHOTO_SELECTED");
        }

        private void matchLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
