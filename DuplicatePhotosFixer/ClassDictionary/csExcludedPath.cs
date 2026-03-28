using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class csExcludedPath : INotifyPropertyChanged
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
        [XmlElement(ElementName = "FolderPath")]
        public string FolderPath { get; set; }



        /// <summary>
        /// Folder ID if any, mainly used in cloud scan type
        /// </summary>
        [XmlElement(ElementName = "FolderId")]
        public string FolderId { get; set; }


        /// <summary>
        /// Which type of path add
        /// </summary>
        [XmlElement(ElementName = "TypeOfExclusion")]
        public eTypeOfExclusion TypeOfExclusion { get; set; }

        /// <summary>
        ///  If scan path type is a file then check for Category 
        ///  else do not consider this.
        /// </summary>
        //[XmlElement(ElementName = "CategoryFilter")]
        //public eCategoryFilter CategoryFilter { get; set; }

    }
}
