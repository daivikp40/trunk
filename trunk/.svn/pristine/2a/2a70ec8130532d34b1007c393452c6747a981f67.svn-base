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
    /// Interaction logic for frmGoogleDriveFolder.xaml
    /// </summary>
    public partial class frmGoogleDriveFolder : Window
    {
        public frmGoogleDriveFolder()
        {
            InitializeComponent();
            LoadStrings();
        }

        void LoadStrings()
        {
            try
            {
                lbl_heading.Content = cResourceManager.LoadString("DPF_HOME_UC_CUSTOM_FOLDER_DIALOG_TITLE");
                btnRefresh.Content = cResourceManager.LoadString("DPF_REFRESH");
                btnAdd.Content = cResourceManager.LoadString("DPF_MESSAGEBOX_OK_TEXT");
                btnCancel.Content = cResourceManager.LoadString("IDS_CANCEL");
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("frmGoogleDriveFolder:: LoadStrings: ", ex);
            }
           
        }
    }
}
