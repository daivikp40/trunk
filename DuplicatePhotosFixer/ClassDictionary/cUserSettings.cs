using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
   public class cUserSettings
    {
        private static cClientEnum.eThemeMode _nThemeMode = cClientEnum.eThemeMode.Light;
        public static cClientEnum.eThemeMode ThemeMode
        {
            get
            {
                int nThemeMode;
                //cClientEnum.eThemeMode theme = cClientEnum.eThemeMode.Light;
                try
                {
                    int.TryParse(Convert.ToString(cGlobalSettings.GetUserAppDataRegistryValue(App_Code.cRegistrySettings.REGSTR_THEMEMODE)), out nThemeMode);
                    _nThemeMode = (cClientEnum.eThemeMode)nThemeMode;

                    return _nThemeMode;
                }
                catch (Exception ex)
                {

                }
                return cClientEnum.eThemeMode.Light;
            }
            set
            {
                cGlobalSettings.SetUserAppDataRegistryValue(App_Code.cRegistrySettings.REGSTR_THEMEMODE, (int)value, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }
    }
}
