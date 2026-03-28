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

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucActionCentre.xaml
    /// </summary>
    public partial class ucActionCentre : UserControl
    {

        vmActionCenter objActionCenterViewModel = null;

        public ucActionCentre()
        {
            PreInit();
            InitializeComponent();
            Init();
        }

        void Init()
        {
            LoadStrings();

        }

        void PreInit()
        {
            try
            {
                objActionCenterViewModel = new vmActionCenter();
                this.DataContext = objActionCenterViewModel;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucActionCenter : PreInit()", ex);
            }
        }

        public void LoadStrings()
        {
            lblHeader.Text = cResourceManager.LoadString("DPF_ACTIONCERTER_HEADER1");
            lblHeadermsg.Text = cResourceManager.LoadString("DPF_ACTIONCERTER_HEADER2");
           // lblfooter.Text = cResourceManager.LoadString("DPF_ACTIONCERTER_FOOTER");

            btnBack.Content = cResourceManager.LoadString("DPF_FOOTER_UC_BACK_TEXT");
            btn_Upgrade.Content = cResourceManager.LoadString("DPF_FOOTER_UC_BUY_NOW");
          

            //if registered then hide registration link
            //if (cGlobalSettings.cRegDetails.IsRegistered && !cGlobalSettings.cRegDetails.IsExpired)
            //{
            //    btn_Upgrade.Visible = false;
            //    //btn_Upgrade.Visible = false;
            //    //picBoxKey.Visible = false;

            //    lblRegisteredVersion.Visible = true;
            //    picBoxRegistered.Visible = true;
            //}
            //else
            //    btn_Upgrade.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
           App.oMainReference.ShowHome2();

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void btn_InstallNow_Click(object sender, RoutedEventArgs e)
        {
            objActionCenterViewModel.InstallProduct(sender);
        }

        private void btn_Upgrade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objActionCenterViewModel.ActivateNow();
            }
            catch (Exception ex)
            {

                
            }
        }
    }
}
