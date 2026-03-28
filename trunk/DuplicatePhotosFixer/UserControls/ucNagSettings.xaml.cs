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
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Models;

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for ucNagSettings.xaml
    /// </summary>
    public partial class ucNagSettings : UserControl
    {
        public ucNagSettings()
        {
            PreInit();
            InitializeComponent();

            Init();
           
        }

        void PreInit()
        {
            try
            {
               if(App.oMainReference.objNag == null)
              
                    App.oMainReference.objNag = new vmNagThemeSettings();
                    this.DataContext = App.oMainReference.objNag;
        
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucNagSettings:: PreInit: ", ex);
            }
        }

        void Init()
        {
            try
            {
                LoadStrings();
                UpdateUIData();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucNagSettings:: Init: ", ex);
            }
        }

        void LoadStrings()
        {
            try
            {
                move_heading.Text = cResourceManager.LoadString("IDS_NAG_THEME");
                heading_info.Text = cResourceManager.LoadString("IDS_NAG_THEME_INFO");
                rb_darkTheme.Content = cResourceManager.LoadString("IDS_DARK");
                rb_lightTheme.Content = cResourceManager.LoadString("IDS_LIGHT");
            }
            catch (Exception ex)
            {

            }
        }

        void UpdateUIData()
        {
            try
            {
                App.oMainReference.objNag.SetUIPropertiesValue();
                return;

            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("ucNagSettings:: UpdateUIData: ", ex);
            }
        }
     

        private void rb_lightTheme_Checked(object sender, RoutedEventArgs e)
        {
            cUserSettings.ThemeMode = cClientEnum.eThemeMode.Light;
            if (App.oMainReference != null)
            {
                App.oMainReference.ApplySkin(cClientEnum.eThemeMode.Light, true);
            }
            App.oMainReference.ShowSaveSettings();
        }

        private void rb_darkTheme_Checked(object sender, RoutedEventArgs e)
        {
            cUserSettings.ThemeMode = cClientEnum.eThemeMode.Dark;
            if (App.oMainReference != null)
            {
                App.oMainReference.ApplySkin(cClientEnum.eThemeMode.Dark, true);
            }
            App.oMainReference.ShowSaveSettings();
        }
    }
}
