using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class csScanPaths : INotifyPropertyChanged
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

        /// <summary>
        /// String of Scan path added
        /// </summary>
        [XmlElement(ElementName = "ScanPath")]
        public string ScanPath { get; set; }

        [XmlElement(ElementName = "ImageIcon")]
        public string ImageIcon { get; set; }

        /// <summary>
        /// Type of path added.
        /// </summary>
        [XmlElement(ElementName = "PathType")]
        public cClientEnum.ePathType PathType { get; set; }

        /// <summary>
        /// Path selected or not for scan 
        /// </summary>
        bool _IsSelected;
        [XmlElement(ElementName = "IsSelected")]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");


            }
        }

        bool _IsCheckboxEnabled;
        [XmlElement(ElementName = "IsCheckboxEnabled")]
        public bool IsCheckboxEnabled
        {
            get { return _IsCheckboxEnabled; }
            set
            {
                _IsCheckboxEnabled = value;
                OnPropertyChanged("IsCheckboxEnabled");


            }
        }

       Visibility _myVisibility;
        [XmlElement(ElementName = "myVisibility")]
        public Visibility myVisibility
        {
            get { return _myVisibility; }
            set
            {
                if (_myVisibility != value)
                {
                    _myVisibility = value;
                    OnPropertyChanged("myVisibility");
                }
            }
        }

        /// <summary>
        /// Folder ID if any, mainly used in cloud scan type
        /// </summary>
        [XmlElement(ElementName = "FolderId")]
        public string FolderId { get; set; }



        /// <summary>
        /// Include Sub Folders of selected Scan Path
        /// </summary>
        bool _IncludeSubfolder { get; set; }
        [XmlElement(ElementName = "IncludeSubfolder")]
        public bool IncludeSubfolder
        {
            get { return _IncludeSubfolder; }
            set
            {
                _IncludeSubfolder = value;
                OnPropertyChanged("IncludeSubfolder");
            }
        }
        /// <summary>
        /// Which type of path add
        /// </summary>
        [XmlElement(ElementName = "TypeOfInclusion")]
        public eTypeOfInclusion TypeOfInclusion { get; set; }

        /// <summary>
        ///  If scan path type is a file then check for Category 
        ///  else do not consider this.
        /// </summary>
        //[XmlElement(ElementName = "CategoryFilter")]
        //public eCategoryFilter CategoryFilter { get; set; }

    }
}
