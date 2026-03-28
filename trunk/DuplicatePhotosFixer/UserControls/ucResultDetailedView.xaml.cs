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

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucResultDetailedView.xaml
    /// </summary>
    public partial class ucResultDetailedView : UserControl
    {
        public ucResultDetailedView()
        {
            InitializeComponent();
        }

        private void DetailedView_Change(object sender, RoutedEventArgs e)
        {
            resultViewGridSelection.IsOpen = true;
        }
    }
}
