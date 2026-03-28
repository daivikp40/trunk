using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.Engine;
using DuplicatePhotosFixer.Engine.WinApiScan;

namespace DuplicatePhotosFixer.HelperClasses
{
    public class ScannerRules : IDisposable
    {
        /// <summary>
        /// Check for exclusion entries
        /// </summary>        
        public List<string> ExcludedDirectoryList = null;
        public List<string> ExcludedFilesList = null;
        public IgnoreRules ignoreRules = null;



        public ScannerRules()
        {
            ExcludedDirectoryList = new List<string>();
            ExcludedFilesList = new List<string>();

            ignoreRules = new IgnoreRules();
            ignoreRules.GenerateRegEx();
        }



        public void Dispose()
        {

            if (ExcludedDirectoryList != null)
            {
                ExcludedDirectoryList.Clear();
                ExcludedDirectoryList = null;
            }
            if (ExcludedFilesList != null)
            {
                ExcludedFilesList.Clear();
                ExcludedFilesList = null;
            }
        }




        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsDirectoryAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            return (
                ((Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                || (((Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) && (cGlobalSettings.IsSkipHiddenFile == 1))
                || ((cGlobalSettings.IsSkipSystemItems == 1) && (Attributes & FileAttributes.System) == FileAttributes.System)
                );

        }


        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsDirectoryIgnored(string FullName/*, FileAttributes Attributes*/)
        {
            return (
                FullName/*.ToLower()*/.StartsWith("$rmmetadata")
                || (FullName.Length > 248)
                || (ignoreRules.IgnoreFolderHardPattern != null && ignoreRules.IgnoreFolderHardPattern.IsMatch(FullName))
                || (ignoreRules.IgnoreFolderSoftPattern != null && ignoreRules.IgnoreFolderSoftPattern.IsMatch(FullName))
                // || (ignoreRules.LsOtherUserPaths!=null && ignoreRules.LsOtherUserPaths.BinarySearch(FullName) > -1)
                || IsIgnoredSpecialFolder(FullName)
                );

        }

        /// <summary>
        /// checks not included currently:
        /// 1. any date time check to check whether file date time last write is lesser than allowed 
        /// 2. extension is under the exclusion list
        /// 3. Depth checker - file is located under how much depth - FileInfo automatically handle this case
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsFileAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            return (
                ((cGlobalSettings.IsSkipHiddenFile == 1) && (Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                || ((cGlobalSettings.IsSkipSystemItems == 1) && (Attributes & FileAttributes.System) == FileAttributes.System)
                );


        }

        /// <summary>
        /// skip files based on ignore list
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public bool IsFileIgnored(string FullName)
        {
            return (
                ((ExcludedFilesList != null) ? (ExcludedFilesList.BinarySearch(FullName) > -1) : false)
                );
        }




        /// <summary>
        /// Is file size 
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public bool IsFileSizeAllowedToUpload(long fileSize)
        {
            /// [11/27/2013 Sudhir.Sharma]
            /// As discussed with Chandan sir, we are not putting any max file size limit to upload for the user              
            return true;
            //
            // we can also put a condition of 0 size of file
            //            
            // return (fileSize <= cGlobalSettings.nUploadFileSizeLimitForTrial);
        }


        /// <summary>
        /// To skip folders like created with symbol #255.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsIgnoredSpecialFolder(string FullName)
        {
            return string.IsNullOrEmpty(new DirectoryInfo(FullName.AddBackSlash()).Name.Trim());
        }


        /// <summary>
        /// Only looks for the file hard exclusion
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public bool IsFileInHardExclusion(string sFileName)
        {
            sFileName = sFileName.ToLower();
            return
                (
                (ignoreRules.IgnoreFileHardPattern != null && ignoreRules.IgnoreFileHardPattern.IsMatch(sFileName))
                || (ignoreRules.IgnoreExtensionHardPattern != null && ignoreRules.IgnoreExtensionHardPattern.IsMatch(sFileName))
                || (ignoreRules.IgnorePathHardPattern != null && ignoreRules.IgnorePathHardPattern.IsMatch(sFileName))
                || (ignoreRules.IgnoreFileHardPattern != null && ignoreRules.IgnoreFileHardPattern.IsMatch(sFileName))
                );
        }



        /// <summary>
        /// file path excluded
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public bool IsFilePathInHardExclusion(string FullName)
        {
            FullName = FullName.ToLower();
            return (
                (ignoreRules.IgnorePathHardPattern != null && ignoreRules.IgnorePathHardPattern.IsMatch(FullName)) ||
                (ignoreRules.IgnoreFileHardPattern != null && ignoreRules.IgnoreFileHardPattern.IsMatch(FullName))
                );
        }
    }
}
