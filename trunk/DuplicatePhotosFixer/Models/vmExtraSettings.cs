using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.HelperClasses;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.Models
{
   public class vmExtraSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        bool _bConsiderTimeInterval;
        public  bool bConsiderTimeInterval
        {
            get
            {
               return _bConsiderTimeInterval;
            }
            set
            {
                _bConsiderTimeInterval = value;
                OnPropertyChanged("bConsiderTimeInterval");

            }
        }

        bool _bConsiderGPS;
        public  bool bConsiderGPS
        {
            get
            {
                return _bConsiderGPS;
            }
            set
            {
                _bConsiderGPS = value;
                OnPropertyChanged("bConsiderGPS");
                //App.oMainReference.RefreshHomeCompariosnSettings();
            }
        }

         bool _bConsiderTimeNoCaptureDt;
        /// <summary>
        /// Include time interval settings if capture date is not available.
        /// </summary>
        public  bool bConsiderTimeNoCaptureDt
        {
            get
            {
                return _bConsiderTimeNoCaptureDt;
            }
            set
            {
                _bConsiderTimeNoCaptureDt = value;
                OnPropertyChanged("bConsiderTimeNoCaptureDt");
            }
        }


        bool _bIncludeRotatedImages ;
        public  bool bIncludeRotatedImages
        {
            get
            {
                return _bIncludeRotatedImages;
            }
            set
            {
                _bIncludeRotatedImages = value;
                OnPropertyChanged("bIncludeRotatedImages");
                //App.oMainReference.RefreshHomeCompariosnSettings();

            }
        }

        bool _bIncludeFlippedImages;
        public  bool bIncludeFlippedImages
        {
            get
            {
                return _bIncludeFlippedImages;
            }
            set
            {
                _bIncludeFlippedImages = value;
                OnPropertyChanged("bIncludeFlippedImages");
                //App.oMainReference.RefreshHomeCompariosnSettings();


            }

        }

        public vmExtraSettings()
        {
            Init();
        }

        void Init()
        {
            LoadSavedSettings();
        }

        public void SetUIPropertiesValue()
        {
            //if (cGlobalSettings.ExtraSettings == null)
            //    LoadExtraSettings();

            //bConsiderTimeInterval = cGlobalSettings.ExtraSettings.DbConsiderTimeInterval;
            //bConsiderGPS = cGlobalSettings.ExtraSettings.DbConsiderGPS;
            //bConsiderTimeNoCaptureDt = cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt;
            //bIncludeFlippedImages = cGlobalSettings.ExtraSettings.DbIncludeFlippedImages;
            //bIncludeRotatedImages = cGlobalSettings.ExtraSettings.DbIncludeRotatedImages;

        }

       

       public void LoadDefaultSettings()
        {
            try
            {
                cGlobalSettings.ExtraSettings.DbConsiderTimeInterval = true;
                cGlobalSettings.ExtraSettings.DbConsiderGPS = true ;
                cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt = false;
                cGlobalSettings.ExtraSettings.DbIncludeRotatedImages = true;
                cGlobalSettings.ExtraSettings.DbIncludeFlippedImages = true;




                SaveSettingdToRegistry();
                LoadSavedSettings();
            }


            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("vmExtraSettings:: LoadDefaultSettings: ", ex);
            }
        }


        void LoadSavedSettings()
        {
            try
            {
                bConsiderTimeInterval = cGlobalSettings.ExtraSettings.DbConsiderTimeInterval;
                bConsiderTimeNoCaptureDt = cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt;
                bConsiderGPS = cGlobalSettings.ExtraSettings.DbConsiderGPS;
                bIncludeRotatedImages = cGlobalSettings.ExtraSettings.DbIncludeRotatedImages;
                bIncludeFlippedImages = cGlobalSettings.ExtraSettings.DbIncludeFlippedImages;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmExtraSettings:: LoadSavedSettings: ", ex);
            }
        }

        void SaveSettingdToRegistry()
        {
            cGlobalSettings.oRegSettings.SaveExtraSettingsToRegistry();
        }


        internal bool CheckSettingsChange()
        {
            bool settingChanged = false;
            bool val = false;
            try
            {
                val = bConsiderTimeInterval == true ? true : false;
                if (cGlobalSettings.ExtraSettings.DbConsiderTimeInterval != val)
                {
                    cGlobalSettings.ExtraSettings.DbConsiderTimeInterval = val;
                    settingChanged = true;
                }

                val = bConsiderGPS == true ? true : false;
                if (cGlobalSettings.ExtraSettings.DbConsiderGPS != val)
                {
                    cGlobalSettings.ExtraSettings.DbConsiderGPS = val;
                    settingChanged = true;
                }

                val = bConsiderTimeNoCaptureDt == true ? true : false;
                if (cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt != val)
                {
                    cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt = val;
                    settingChanged = true;
                }

                val = bIncludeRotatedImages == true ? true : false;
                if (cGlobalSettings.ExtraSettings.DbIncludeRotatedImages != val)
                {
                    cGlobalSettings.ExtraSettings.DbIncludeRotatedImages = val;
                    //cGlobalSettings.MatchSettingsData.bConsiderRotated = val;
                    settingChanged = true;
                }

                val = bIncludeFlippedImages == true ? true : false;
                if (cGlobalSettings.ExtraSettings.DbIncludeFlippedImages != val)
                {
                    cGlobalSettings.ExtraSettings.DbIncludeFlippedImages = val;
                    //cGlobalSettings.MatchSettingsData.bConsiderFlipped = val;
                    settingChanged = true;
                }

              

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucSettingsGeneral : CheckSettingsChange", ex);
            }
            // save settings to registy if changed
            if (settingChanged)
            {
                SaveSettingdToRegistry();
                App.oMainReference.ShowSaveSettings();

                //App.oMainReference.RefreshHomeCompariosnSettings();

                //App.oMainReference.objHomeComparisonModel.RefreshIncRotImages();

            }

            return settingChanged;


        }

        public static string VersionIndependentRegKey
        {
            get
            {
                string versionDependent = System.Windows.Forms.Application.UserAppDataRegistry.Name;
                string versionIndependent =
                       versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

                //   versionIndependent = string.Format("{0}\\{1}", versionIndependent, cGlobalSettings.nUserMachineId);

                return versionIndependent;
            }
        }

        public static string VersionIndependentRegKeyNoHive
        {
            get
            {
                string versionDependent = System.Windows.Forms.Application.UserAppDataRegistry.Name;
                string versionIndependent =
                       versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

                versionIndependent = versionIndependent.Substring(versionIndependent.IndexOf("\\") + 1);

                // versionIndependent = string.Format("{0}\\{1}", versionIndependent, cGlobalSettings.nUserMachineId);

                return versionIndependent;
            }
        }

        public static object GetUserAppDataRegistryValue(string name, object defaultValue)
        {
            return Registry.GetValue(VersionIndependentRegKey, name, defaultValue) ?? defaultValue;
        }

        public static string VersionIndependentRootRegKey
        {
            get
            {
                string versionDependent = System.Windows.Forms.Application.UserAppDataRegistry.Name;
                string versionIndependent =
                       versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

                // versionIndependent = string.Format("{0}\\{1}", versionIndependent, cGlobalSettings.nUserMachineId);

                return versionIndependent;
            }
        }
        public static object GetUserAppDataRegistryValue(string name)
        {
            return GetUserAppDataRegistryValue(name, null);
        }

        public static void SetUserAppDataRootRegistryValue(string name, object value, RegistryValueKind kind)
        {
            if (Registry.CurrentUser.OpenSubKey(VersionIndependentRegKeyNoHive) == null)
            {
                Registry.CurrentUser.CreateSubKey(VersionIndependentRegKeyNoHive);
            }
            Registry.SetValue(VersionIndependentRootRegKey, name, value, kind);
        }


        public static void SetUserAppDataRootRegistryValueNotifier(string name, object value, RegistryValueKind kind)
        {
            string strRegPath = VersionIndependentRegKeyNoHive + "\\" + AppNotifier.Notifier.notifierName;
            if (Registry.CurrentUser.OpenSubKey(strRegPath) == null)
            {
                Registry.CurrentUser.CreateSubKey(strRegPath);
            }
            Registry.SetValue(VersionIndependentRootRegKey + "\\" + AppNotifier.Notifier.notifierName, name, value, kind);
        }

        public static void SetUserAppDataRegistryValue(string name, object value, RegistryValueKind kind)
        {
            if (Registry.CurrentUser.OpenSubKey(VersionIndependentRegKeyNoHive) == null)
            {
                Registry.CurrentUser.CreateSubKey(VersionIndependentRegKeyNoHive);
            }
            Registry.SetValue(VersionIndependentRegKey, name, value, kind);
        }

       

    }
}
