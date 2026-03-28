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

namespace DuplicatePhotosFixer.Views
{
    /// <summary>
    /// Interaction logic for frmSelectionAssistant.xaml
    /// </summary>
    public partial class frmSelectionAssistant : Window
    {
        public frmSelectionAssistant()
        {
            InitializeComponent();
            //App.selectionAssistantfrm = this;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestoreSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowAssistantSelection()
        {
            try
            {
                App.oMainReference.ShowAssistantSelection();
                //App.selectionAssistantfrm.ShowDialog();
            }
            catch (Exception ex)
            {

            }
            
        }

      

        
    }
}
