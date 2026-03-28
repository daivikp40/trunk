using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DuplicatePhotosFixer.Engine;

namespace DuplicatePhotosFixer.Models
{
    public class vmHomeFooter : INotifyPropertyChanged
    {
        public UserControl ctrlOwner = null;
        private bool _isScanEnabled { get; set; }
        public bool isScanEnabled
        {
            get { return _isScanEnabled; }

            set
            {
                _isScanEnabled = value;
                OnPropertyChanged(nameof(isScanEnabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public vmHomeFooter()
        {
            Init();
        }

        void Init()
        {

            IsScanbtnEnabled();


        }

        public void IsScanbtnEnabled()
        {


            var isEnabled = cGlobalSettings.listScanPaths.All(x => x.IsSelected == false);
            if (isEnabled)
            {
                isScanEnabled = false;
            }
            else
            {
                isScanEnabled = true;
            }

            
        }


        public void WatchTutorial()
        {
            try
            {

            }
            catch (Exception ex)
            {

               
            }
        }


       
    }
}
