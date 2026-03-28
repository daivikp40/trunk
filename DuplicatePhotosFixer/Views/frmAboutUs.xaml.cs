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

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmAboutUs.xaml
    /// </summary>
    public partial class frmAboutUs : Window
    {
        public frmAboutUs()
        {

            App.oMainReference.objAboutUsViewModel = new Models.vmAboutUs();
            this.DataContext = App.oMainReference.objHomeViewModel;

            InitializeComponent();
            LoadStrings();
        }

        void LoadStrings()
            {

            try 
                { 

                    dpf_header.Text = cResourceManager.LoadString("IDS_DU_APP_NAME");
                    dpf_header_info.Text = cResourceManager.LoadString("IDS_ABOUT");
                    Privacy_policy_btn.Content = cResourceManager.LoadString("IDS_UTILITY_PRIVACYPOLICY");
                    EULA_btn.Content = cResourceManager.LoadString("IDS_UTILITY_EULA");
                    Uninstall_instructions_btn.Content = cResourceManager.LoadString("IDS_UNINSTALL_STEPS");
                    contact_info.Text = cResourceManager.LoadString("IDS_CONTACT_US");
                    
                }

    catch(Exception ex){

                cGlobalSettings.oLogger.WriteLogException("frmExitNag:: LoadStrings: ", ex);

            }

        }

      

        private void extBtn_Register_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.Activate();
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

        }

        private void Privacy_policy_btn_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.PrivacyPolicyClick();
        }

        private void EULA_btn_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.EulaClick();
        }

        private void Uninstall_instructions_btn_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.UninstallInstructionsClick();
        }

        private void support_btn_Click(object sender, RoutedEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.SupportEmailSystem();
        }

        private void lblSystweakLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.oMainReference.objAboutUsViewModel.SystweakLinkClick();
        }
    }
}
