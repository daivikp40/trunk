using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.CustomExtendClass;
using DuplicatePhotosFixer.HelperClasses;
using DuplicatePhotosFixer.UserControls;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.Models
{
  public  class vmMoveSettings : INotifyPropertyChanged
    {
        public ucSettingsMove parentControl { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region MoveDuplicates Properties

        bool _bMoveToRecycleBin { get; set; }
        public bool bMoveToRecycleBin
        {
            get { return _bMoveToRecycleBin; }
            set
            {
                _bMoveToRecycleBin = value;
                
                OnPropertyChanged("bMoveToRecycleBin");
                

                //CheckSettingChanged();
            }
        }

        bool _bMoveToLocation { get; set; }
        public bool bMoveToLocation
        {
            get { return _bMoveToLocation; }
            set
            {
                _bMoveToLocation = value;
                OnPropertyChanged("bMoveToLocation");

                //CheckSettingChanged();
            }
        }

        string _bLocationPath { get; set; }
        public string bLocationPath
        {
            get { return _bLocationPath; }
            set
            {
                _bLocationPath = value;
                OnPropertyChanged("bLocationPath");

                //CheckSettingChanged();
            }
        }

        bool _IsBrowseFilePathEnable { get; set; }
        public bool IsBrowseFilePathEnable
        {
            get
            {
                return _IsBrowseFilePathEnable;
            }
            set
            {
                _IsBrowseFilePathEnable = value;
                OnPropertyChanged("IsBrowseFilePathEnable");
            }
        }

        #endregion


        public vmMoveSettings()
        {
            Init();
        }


     void Init()
        {
            LoadSavedSettings();
        }

        public void LoadMoveSettings(bool bLoadDefaultFocefully = false)
        {
            //try
            //{
            //    if (bLoadDefaultFocefully)
            //    {
            //        LoadDefaultSettings();
            //        return;
            //    }
            //    LoadSavedSettings();

            //    if (cGlobalSettings.MoveSettings == null)
            //        LoadMoveSettings(true);

            //}
            //catch (Exception ex)
            //{
            //    cGlobalSettings.oLogger.WriteLogException("vmExtraSettings:: LoadExtraSettings: ", ex);
            //}
        }

        void SaveSettingdToRegistry()
        {
            cGlobalSettings.oRegSettings.SaveMoveSettingstoRegistry();
        }

       public void LoadDefaultSettings()
        {
            try
            {
                cGlobalSettings.MoveSettings.DbMoveToRecycle = true;

                cGlobalSettings.MoveSettings.DbLocationPath = cGlobalSettings.FilePathToMoveThisLocation();


                SaveSettingdToRegistry();
                LoadSavedSettings();
            }


            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("vmMoveSettings:: LoadDefaultSettings: ", ex);
            }
        }

        void LoadSavedSettings()
        {
            try
            {
               bMoveToRecycleBin = cGlobalSettings.MoveSettings.DbMoveToRecycle; 
                
                if (bMoveToRecycleBin)
                {
                    bMoveToLocation = false;
                    IsBrowseFilePathEnable = false;
                }
                else
                {
                    bMoveToLocation = true;
                    IsBrowseFilePathEnable = true;
                }

                bLocationPath = cGlobalSettings.MoveSettings.DbLocationPath;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmMoveSettings:: LoadSavedSettings: ", ex);
            }
        }

        internal bool CheckSettingChanged()
        {
            bool settingChanged = false;
            bool val = false;
            try
            {
                val = bMoveToLocation == true ? true : false;
                if (cGlobalSettings.MoveSettings.DbMoveToLocation != val)
                {
                    cGlobalSettings.MoveSettings.DbMoveToLocation = val;
                    if (val)
                    {
                        IsBrowseFilePathEnable = true;
                        bMoveToRecycleBin = false;
                    }
                    //else
                    //{
                    //    bMoveToRecycleBin = false;
                    //}
                    ////bMoveToRecycleBin = !val;
                    //IsBrowseFilePathEnable = val;

                    settingChanged = true;
                }

                val = bMoveToRecycleBin == true ? true : false;
                if (cGlobalSettings.MoveSettings.DbMoveToRecycle != val)
                {
                    cGlobalSettings.MoveSettings.DbMoveToRecycle = val;
                    if (val)
                    {
                        IsBrowseFilePathEnable = false;
                        bMoveToLocation = false;
                    }
                    //bMoveToLocation = !val;
                    //IsBrowseFilePathEnable = !val;
                    settingChanged = true;
                }

                if (bLocationPath != cGlobalSettings.MoveSettings.DbLocationPath)
                {
                    cGlobalSettings.MoveSettings.DbLocationPath = bLocationPath;
                    settingChanged = true;
                }
            }

            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmMoveSettings:: CheckSettingChanged(): ", ex);
            }

            // save settings to registy if changed
            if (settingChanged)
            {
                SaveSettingdToRegistry();
               App.oMainReference.ShowSaveSettings();
            }

            return settingChanged;
        }



        public void BrowseDelFilePathLoc()
        {
            try
            {
                string[] SelectedPaths = HelperClasses.cSelectFilesFolders.ShowFolder_System(App.oMainReference);

                if (SelectedPaths != null && SelectedPaths.Length > 0)
                {
                    bLocationPath = SelectedPaths[0];
                    CheckSettingChanged();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucGeneralSettingsViewModel :: BrowseDelFilePathLoc : ", ex);
            }

        }

        public static object GetUserAppDataRegistryValue(string name, object defaultValue)
        {
            return Registry.GetValue(cGlobalSettings.VersionIndependentRegKey, name, defaultValue) ?? defaultValue;
        }

        public static void SetUserAppDataRegistryValue(string name, object value, RegistryValueKind kind)
        {
            if (Registry.CurrentUser.OpenSubKey(VersionIndependentRegKeyNoHive) == null)
            {
                Registry.CurrentUser.CreateSubKey(VersionIndependentRegKeyNoHive);
            }
            Registry.SetValue(cGlobalSettings.VersionIndependentRegKey, name, value, kind) ;
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


    }
}
