using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class csImageFileInfo
    {//  line 31, 45 commented
        public int key { get; set; }
        public bool bFileModified { get; set; }
        public bool bHashFoundInDB { get; set; }
        public string fileID { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
        public long fileSize { get; set; }
        public string fileSizeWithUnit { get; set; }
        public object fileInfo { get; set; }
        //public string fileType { get; set; }
        public bool isMark { get; set; }
        public byte[] byteHash { get; set; }
        public ulong[] hash { get; set; }
//ROTATE_FLIPPED_CHECK
        public List<byte[]> byteHashArr { get; set; }
        public List<ulong[]> arrHash { get; set; }

        public float match { get; set; }
        public int groupNumber { get; set; }
        public string md5 { get; set; }
        public csExifInfo exifInfo { get; set; }

        public bool calcHashForExactMatch { get; set; } // in case of ExactMatch : calculate hash for this item if fileSize count>1

        public string folderPath { get; set; }
        //public int folderIndex { get; set; }

        //public PropertyItem[] exifInfo { get; set; }

        public int GroupNumber { get; set; }

        public int GroupItemCount { get; set; }

        public DateTime modDate { get; set; }
        public DateTime createDate { get; set; }
        public int matchesWithNodeKey { get; set; }//keeps the track of the matching node with best match score
        //public csImageFileInfo matchesWithNode { get; set; }//keeps the track of the matching node with best match score
        public csDuplicatesGroup parent { get; set; }//the parent (group) it belongs
        //ExifInfo *exifInfo;
        public bool isiPhotoNode { get; set; }//bool to know if it is a image from iPhoto or from a folder
        //iPhotoNode *iPhotoInfo;//For iPhoto image node
        //DataFiller *obj_iPhoto;//keeps the track of the iPhoto database manager, required when cleaning is performed
        public bool isLocked { get; set; }//to lock file from deletion

        //required for Aspect field in object ListView
        public string Filetype { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public int itemnumber { get; set; }
        public int itemtype { get; set; } //-2 for header, -1 for blank row and 0 for item in ObjectListView
        public int PhotoCountInGroup { get; set; }


        //required for folder automark priority
        [System.ComponentModel.DefaultValue(0)]
        public int FolderPriority { get; set; }

        //public FileInfo matchesWithNode { get; set; }

        // for New format/extensions
        public string ThumbnailPath { get; set; }

        public bool bRAWFormat { get; set; }

        public int nFileFormatRank { get; set; }


        /// Image Metrics
        /// 
        public double Sharpness { get; set; }
        public double FaceScore { get; set; }
        public double EyeOpenScore { get; set; }
        //public double ImageScore { get; set; }


        public double NoiseScore { get; set; }
        public double ResolutionScore { get; set; }
        public double PhotoScore { get; set; }

        public double FinalScore { get; set; }

    }
}
