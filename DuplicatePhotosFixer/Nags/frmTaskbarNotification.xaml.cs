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

namespace DuplicatePhotosFixer.Nags
{
    /// <summary>
    /// Interaction logic for frmTaskbarNotification.xaml
    /// </summary>
    public partial class frmTaskbarNotification : Window
    {
        cTaskbar taskbar = new cTaskbar();
        cTaskbar.TaskbarPosition oTaskBarPosition = cTaskbar.TaskbarPosition.Unknown;

        public int nCounterNagShownTillNow = 0;

        public int SetTotalDuplicates
        {
            set
            {
                lbl_dupsCount.Text = string.Format("{0} {1}", cGlobalSettings.TotalDuplicatePhotos, cResourceManager.LoadString("IDS_NAG_MSG_DUPLICATE_FOUND").Trim(new Char[] { ' ', ':', '：' }));
               // lbl_dupsCount.LinkArea = new LinkArea(0, cGlobalSettings.TotalDuplicatePhotos.ToString().Length);
            }
        }

        public long SetTotalScace
        {
            set
            {
                //string totalSize = AppFunctions.StrFormatByteSize(cGlobalSettings.oRegSettings.GetTotalDuplicatesSpace());
                //lbl_totalSpace.Text = string.Format("{0} {1}", totalSize, cResourceManager.LoadString("IDS_NAG_MSG_SPACE_SAVED").Trim(new Char[] { ' ', ':', '：' }));
                //lbl_totalSpace.LinkArea = new LinkArea(0, totalSize.Length);
            }
        }
        public frmTaskbarNotification()
        {
            InitializeComponent();
            LoadStrings();
        }
        
        void LoadStrings()
        {
            
            lbl_head.Text = cResourceManager.LoadString("IDS_ATTENTION");
            lbl_attention.Text = cResourceManager.LoadString("IDS_PC_REQUIRES_ATTENTION");
            lbl_dupsCount.Text = string.Format("{0} {1}", cGlobalSettings.TotalDuplicatePhotos, cResourceManager.LoadString("IDS_NAG_MSG_DUPLICATE_FOUND").Trim(new Char[] { ' ', ':', '：' }));
            //lbl_dupsCount.LinkArea = new LinkArea(0, cGlobalSettings.TotalDuplicatePhotos.ToString().Length);

            //string totalSize = AppFunctions.StrFormatByteSize(cGlobalSettings.oRegSettings.GetTotalDuplicatesSpace());
            //lbl_totalSpace.Text = string.Format("{0} {1}", totalSize, cResourceManager.LoadString("IDS_NAG_MSG_SPACE_SAVED").Trim(new Char[] { ' ', ':', '：' }));
            //lbl_totalSpace.LinkArea = new LinkArea(0, totalSize.Length);

            lbl_removeAll.Text = cResourceManager.LoadString("IDS_TRIALNEW_FEATURE_1");
            lbl_upgradeNow.Text = cResourceManager.LoadString("IDS_UPGRADE_TO_CLEAN");
            btn_upgradeNow.Content = cResourceManager.LoadString("DPF_FOOTER_UC_BUY_NOW");
            llink_haveKey.Content = cResourceManager.LoadString("IDS_HAVE_LICENSE_CODE");
        }
    }
}
