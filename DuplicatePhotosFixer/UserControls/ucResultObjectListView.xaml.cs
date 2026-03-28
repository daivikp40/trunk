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
    /// Interaction logic for ucResultObjectListView.xaml
    /// </summary>
    public partial class ucResultObjectListView : UserControl
    {
        public ucResultObjectListView()
        {
            InitializeComponent();
            LoadStrings();
        }

        void LoadStrings()
        {
            try
            {
                grp_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_GROUP");
                fileName_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_FILENAME");
                path_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_PATH");
                size_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_SIZE");
                create_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_CREATED");
                modified_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_MODIFIED");
                fileType_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_FILETYPE");
                imageWidth_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_IMAGEWIDTH");
                imgHeight_header.Header = cResourceManager.LoadString("DPF_RESULT_OLV_IMAGEHEIGHT");
            }
            catch (Exception ex)
            {

                
            }

           

        }

        private void ObjectListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ObjectListView1_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ObjectListView1_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void cmSelectionAssistant_Opening(object sender, RoutedEventArgs e)
        {

        }
    }
}



