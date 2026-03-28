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
using DuplicatePhotosFixer;

namespace DuplicatePhotosFixer.Views.Cloud
{
    /// <summary>
    /// Interaction logic for frmMessageDriveAuthorize.xaml
    /// </summary>
    public partial class frmMessageDriveAuthorize : Window
    {
        public frmMessageDriveAuthorize()
        {
            InitializeComponent();
            LoadStrings();
        }

        void LoadStrings()
        {
            try
            {
                lbl_heading.Content = cGlobalSettings.GetProductName();

                txt_heading.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_HEADER");
                txt_StepsHeader.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEPS_HEADER");
                txtStep1.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP1");
                txtStep1Sub.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP1_SUB");
                txtStep2.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP2");
                txtStep2Sub.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP2_SUB");
                txtStep3.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP3");
                txtStep3Sub.Text = cResourceManager.LoadString("DPF_DRIVE_INFO_STEP3_SUB");
                btn_allowpermissions.Content = cResourceManager.LoadString("DPF_DRIVE_INFO_ALLOW");
                btn_scan.Content = cResourceManager.LoadString("DPF_SCAN_COMPUTER");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmMessageDriveAuthorize:: LoadStrings: ", ex);
            }
        }
    }
}
