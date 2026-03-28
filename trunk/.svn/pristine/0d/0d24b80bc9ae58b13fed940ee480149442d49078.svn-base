using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class FileRecord
    {
        /// <summary>
        /// Same as dictionary key
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Unique ID of a file if any
        /// </summary>
        public int FileID { get; set; }

        //---------- File info data ---------
        /// <summary>
        /// File name with extension
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File full path 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Directory path of a file
        /// </summary>
        public string FolderPath { get; set; }
        /// <summary>
        /// File Created datetime, default value: DateTime.Min 
        /// </summary>
        public DateTime dtCreated { get; set; }
        /// <summary>
        /// File last modified datetime, default value: DateTime.Min 
        /// </summary>
        public DateTime dtModified { get; set; }
        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// File extension 
        /// </summary>
        public string Extension { get; set; }


        //--------- DB Updates ----------
        /// <summary>
        /// Data found in DB for a file, but file is now modified or not.
        /// </summary>
        public bool bModified { get; set; }
        /// <summary>
        /// Is file is scanned first time or scanned earlier also.
        /// </summary>
        public bool isAlreadyScanned { get; set; }
        /// <summary>
        /// File hash/MD5
        /// </summary>
        public string FileHash { get; set; }


        public int GroupNumber { get; set; }

    }
}
