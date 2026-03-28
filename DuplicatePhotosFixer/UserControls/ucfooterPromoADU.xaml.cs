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
    /// Interaction logic for ucfooterPromoADU.xaml
    /// </summary>
    public partial class ucfooterPromoADU : UserControl
    {
        public ucfooterPromoADU()
        {
            InitializeComponent();
            LoadStrings();
        }

        public void LoadStrings()
        {
           btn_back.Content = cResourceManager.LoadString("DPF_FOOTER_UC_BACK_TEXT");
        }
    }
}
