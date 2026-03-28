using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.ClassDictionary
{
    [Serializable]
    public class ExtensionsData //: INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}
        [XmlElement(ElementName = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// True: Show checked in Included list and must unchecked in excluded list
        /// </summary>
        [XmlElement(ElementName = "bIncluded")]
        public bool bIncluded { get; set; }

        /// <summary>
        /// True: Show checked in Excluded list and must unchecked in Includedlist
        /// </summary>
        [XmlElement(ElementName = "bExcluded")]
        public bool bExcluded { get; set; }

        [XmlElement(ElementName = "FileTypeText")]
        public string FileTypeText { get; set; }

        [XmlElement(ElementName = "FileDialogFilterItem")]
        public string FileDialogFilterItem { get; set; }

        [XmlElement(ElementName = "RegexFileExtension")]
        public string RegexFileExtension { get; set; }

        [XmlElement(ElementName = "IncludeExtRegex")]
        public string IncludeExtRegex { get; set; }

        [XmlElement(ElementName = "ExcludeExtRegex")]
        public string ExcludeExtRegex { get; set; }

        //[XmlElement(ElementName = "CategoryFilter")]
        //public eCategoryFilter CategoryFilter { get; set; }

        /// <summary>
        /// This is shown in Include or exluded list or in both list
        /// </summary>
        [XmlElement(ElementName = "ExtensionStatus")]
        public eExtensionStatus TypeOfInclusion { get; set; }

        /// <summary>
        /// Can not be edited, value is constant.
        /// </summary>
        [XmlElement(ElementName = "bNonEditable")]
        public bool bNonEditable { get; set; }


        

      
    }
}
