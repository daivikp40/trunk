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
using DuplicatePhotosFixer.Models;

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmFeedBack.xaml
    /// </summary>
    public partial class frmFeedBack : Window
    {
        public frmFeedBack()
        {

            InitializeComponent();
            PreInit();
            Init();
        }

        void PreInit()
        {
            try
            {
                if (App.oMainReference.objFeedBack == null)
                    App.oMainReference.objFeedBack = new vmFeedBack();
                this.DataContext = App.oMainReference.objFeedBack;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmFeedBack:: PreInit: ", ex);
            }
        }

        void Init()
        {
            try
            {
                LoadStrings();
                UpdateData();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmFeedBack:: Init: ", ex);
            }
        }

        private void LoadStrings()
        {
            try
            {
                dpf_header.Text = cGlobalSettings.GetProductName();
                dpf_header_info.Text = cResourceManager.LoadString("IDS_FEEDBACK");
                //lbl_ProductName.Text = cResourceManager.LoadString("IDS_BEFORE_GO_FEEDBACK");
                tbDearUser.Text = cResourceManager.LoadString("PKS_FEEDBAKNAG_DESC");
                tbProvideEmail.Text = cResourceManager.LoadString("PKS_FEEDBACKNAG_DESC2");
                tbEmail.Text = cResourceManager.LoadString("PKS_YOUR_EMAIL_ADDRESS");
                tbFeedBacktype.Text = cResourceManager.LoadString("PKS_FEEDBACK_TYPE_STATIC");
                tbBriefly.Text = cResourceManager.LoadString("PKS_BRIEF_EXPRE");
                //btnCancel.Content = cResourceManager.LoadString("IDS_CONTINUE_UNINSTALL");
                btnSubmit.Content = cResourceManager.LoadString("IDS_SUBMIT");

                //cboFeedBacktype.Items.Clear();
                //cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_SELECT_PRBTYPE"));
                //if (cGlobalSettings.oScanSettings.LanguageCode.ToLower() == "en")
                //{
                //    cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_INSTALL_COMP"));
                //    cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_PRB_DOWNLOADING"));
                //    cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_PRB_INSTALLING"));
                //}
                //cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_PRB_REGISTRING_PRODUCT"));
                //cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_PRB_UNDERSTAND"));
                //cboFeedBacktype.Items.Add(cResourceManager.LoadString("PKS_GENERAL_FEEDBACK"));
                //cboFeedBacktype.SelectedIndex = 0;

                btn_Cancel.Content = cResourceManager.LoadString("IDS_CANCEL");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmFeedBack:: LoadStrings: ", ex);
            }
        }
        void UpdateData()
        {
            try
            {

                txtEmail.Text = "";
                txtBrief.Text = "";
                if (cboFeedBacktype.SelectedIndex < 0)
                    cboFeedBacktype.SelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }
        }
       

        private void close_btn_click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Hide();
            }
            catch (Exception ex)
            {

            }

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();

            }
            catch (Exception ex)
            {
              
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
           App.oMainReference.objFeedBack.SubmitFeedBack();
            this.Hide();
        }
    }
}
