using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine.WinApiScan;
using DuplicatePhotosFixer.HelperClasses.MyLogger;
using DuplicatePhotosFixer.Helpers;

namespace DuplicatePhotosFixer.Engine.ScanModes.FileSystem
{
    class SystemScanHelper
    {
        public volatile bool bStop = false;
        public delegate void PauseScanning();

        //public List<string> dirs = new List<string>();
        public HashSet<string> ExcludedFiles = null;
        public HashSet<string> ExcludedFolders = null;
        List<csScanPaths> filteredScanPath;

        public bool StartScan(List<csScanPaths> ScanPathsData,
            List<string> searchPattern,
            SearchOption searchOption,
            ref List<csImageFileInfo> files,
            Callback callback,
            int PauseCounter, OnScanProgressProcess SendProgress = null)
        {
            IntPtr wow64Value = IntPtr.Zero;
            try
            {
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                {
                    // Disable redirection.
                    if (!NativeMethods.Wow64DisableWow64FsRedirection(ref wow64Value))
                    {
                        //;return false;
                    }
                }

                /// File path which are requested for scan 
                /// Case: Child and parent folder both are added.
                /// File and folder both added.
                /// ScanPathsData == > FilteredPathsList

                eFuncRetVal returnStatus = FilterScanPaths(ref ScanPathsData, out filteredScanPath);

                searchPattern = new List<string> { "*.{ jpg, png }"};
            
                bool bRet = true;
                for (int i = 0; i < filteredScanPath.Count; i++)
                {
                    if (!bRet)
                        break;

                    if (filteredScanPath[i].PathType == cClientEnum.ePathType.folder)
                    {
                        bRet = l_ScanDirectory(filteredScanPath[i].ScanPath, 
                            searchPattern, searchOption
                            , ref files, callback,
                            PauseCounter, 
                            SendProgress);

                        //l_ScanDirectory
                        //(
                        //filteredScanPath[i].ScanPath,
                        //searchPattern,
                        //searchOption,
                        //ref files,
                        //callback,
                        //PauseCounter,
                        //SendProgress
                        //);
                    }
                    else if (filteredScanPath[i].PathType == cClientEnum.ePathType.file)
                    {
                        /// Add in a list
                        /// 
                        ProcessSingleFile(filteredScanPath[i].ScanPath, ref files, SendProgress);

                    }


                }

                return bRet;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    {
                        // Re-enable redirection.
                        NativeMethods.Wow64RevertWow64FsRedirection(wow64Value);
                    }
                }
                finally
                {
                    ;
                }

            }
            return false;
        }

       
        /// <summary>
        /// Process a single file and add it to the collection if it matches criteria
        /// </summary>
        private bool ProcessSingleFile(
            string filePath,
            ref List<csImageFileInfo> files,
            OnScanProgressProcess SendProgress)
        {
            try
            {
                if (bStop) return false;

                // Check if file extension matches image patterns
                if (!Regex.IsMatch(Path.GetExtension(filePath),
                    cGlobalSettings.ImageFileExtensionsList,
                    RegexOptions.IgnoreCase))
                {
                    return false; // Not an image file
                }

                FileInfo fileInfo = new FileInfo(filePath);

                // Check minimum file size
                if (fileInfo.Length < cGlobalSettings.MinFileSize)
                    return false;

                // Check if file attributes should be ignored
                if (IsFileAttributesIgnored(filePath, fileInfo.Attributes))
                    return false;

                // Create file info record
                csImageFileInfo record = new csImageFileInfo()
                {
                    filePath = fileInfo.FullName.ToLower(),
                    fileName = fileInfo.Name,
                    fileSize = fileInfo.Length,
                    bHashFoundInDB = false,
                    isLocked = false,
                    createDate = fileInfo.CreationTime,
                    modDate = fileInfo.LastWriteTime,
                    Filetype = string.Format(fileInfo.Extension.Substring(1, fileInfo.Extension.Length - 1).ToUpper() + " File"),
                    fileSizeWithUnit = AppFunctions.StrFormatByteSize(fileInfo.Length),
                    folderPath = fileInfo.DirectoryName.AddBackSlash(),
                };

                // Add to concurrent collection
                cGlobalSettings.listImageFileInfo.TryAdd(record.key, record);

                // Check for RAW format
                bool isRawFormatFound = false;
                var extFound = cGlobalSettings.listFileType.Where(p =>
                    p.Key > -1 &&
                    p.Value.IsNewLibSupportedExt &&
                    p.Value.ImageFileExtension.IndexOf(fileInfo.Extension, StringComparison.OrdinalIgnoreCase) >= 0);

                if (extFound != null && extFound.Any())
                {
                    isRawFormatFound = true;
                }

                // Add to files list
                files.Add(record);

                // Send progress update
                SendProgress?.Invoke(filePath, -1, files.Count, 0, 0);

                return true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException($"ProcessSingleFile: {filePath}", ex);
                return false;
            }
        }

        /// <summary>
        /// Win32 API scan functions for directory scanning
        /// </summary>
        bool l_ScanDirectory(
            string directory,
            List<string> searchPattern,
            SearchOption searchOption,
            ref List<csImageFileInfo> files,
            Callback callback,
            int PauseCounter,
            OnScanProgressProcess SendProgress)
        {
            IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
            NativeMethods.WIN32_FIND_DATAW findData;
            IntPtr findHandle = INVALID_HANDLE_VALUE;

            try
            {
                if (bStop) return false;

                // Loop for files
                findHandle = NativeMethods.FindFirstFileW(
                    FileFunctions.AddSearchPattern(directory, "*.*"),
                    out findData);

                if (findHandle != INVALID_HANDLE_VALUE)
                {
                    if (directory.Length > 3 &&
                        IsDirectoryAttributesIgnored(directory, findData.dwFileAttributes))
                        return true;

                    do
                    {
                        if (bStop) break;

                        if (findData.cFileName.Equals(@".") ||
                            findData.cFileName.Equals(@".."))
                            continue;

                        string fullpath = PathEx.Combine(directory, findData.cFileName).ToLower();

                        if ((searchOption == SearchOption.AllDirectories) &&
                            (findData.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            // DIRECTORY TREATMENT
                            if (bStop) break;

                            if (!IsDirectoryAttributesIgnored(fullpath, findData.dwFileAttributes))
                            {
                                // Recursively scan subdirectories
                                l_ScanDirectory(fullpath, searchPattern, searchOption,
                                    ref files, callback, PauseCounter, SendProgress);
                            }
                        }
                        else
                        {
                            // FILE TREATMENT
                            if (bStop) break;

                            // Process file using shared method
                            ProcessFileFromDirectory(fullpath, findData, ref files, SendProgress);
                        }

                        if (bStop) break;

                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                        {
                            break;
                        }
                    }
                    while (NativeMethods.FindNextFile(findHandle, out findData));
                }

                return true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("Win32APIScanner::ScanDirectory", ex);
            }
            finally
            {
                if (findHandle != INVALID_HANDLE_VALUE)
                    NativeMethods.FindClose(findHandle);
            }

            return false;
        }

        /// <summary>
        /// Process a file found during directory scanning
        /// </summary>
        private bool ProcessFileFromDirectory(
            string fullpath,
            NativeMethods.WIN32_FIND_DATAW findData,
            ref List<csImageFileInfo> files,
            OnScanProgressProcess SendProgress)
        {
            try
            {
                // Check if file extension matches image patterns
                if (!Regex.IsMatch(Path.GetExtension(fullpath),
                    cGlobalSettings.ImageFileExtensionsList,
                    RegexOptions.IgnoreCase))
                {
                    return false;
                }

                FileInfo fileInfo = new FileInfo(fullpath);

                if (fileInfo.Length < cGlobalSettings.MinFileSize)
                    return false;

                if (!IsFileAttributesIgnored(fullpath, findData.dwFileAttributes))
                {
                    csImageFileInfo record = new csImageFileInfo()
                    {
                        filePath = fileInfo.FullName.ToLower(),
                        fileName = fileInfo.Name,
                        fileSize = fileInfo.Length,
                        bHashFoundInDB = false,
                        isLocked = false,
                        createDate = fileInfo.CreationTime,
                        modDate = fileInfo.LastWriteTime,
                        Filetype = string.Format(fileInfo.Extension.Substring(1, fileInfo.Extension.Length - 1).ToUpper() + " File"),
                        fileSizeWithUnit = AppFunctions.StrFormatByteSize(fileInfo.Length),
                        folderPath = fileInfo.DirectoryName.AddBackSlash(),
                    };

                    cGlobalSettings.listImageFileInfo.TryAdd(record.key, record);

                    // Check for RAW format support
                    bool isRawFormatFound = false;
                    var extFound = cGlobalSettings.listFileType.Where(p =>
                        p.Key > -1 &&
                        p.Value.IsNewLibSupportedExt &&
                        p.Value.ImageFileExtension.IndexOf(fileInfo.Extension, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (extFound != null && extFound.Any())
                    {
                        isRawFormatFound = true;
                    }

                    files.Add(record);

                    // Send progress updates every 750 files
                    if (SendProgress != null && files.Count % 750 == 0)
                    {
                        SendProgress(fullpath, -1, files.Count, 0, 0);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException($"ProcessFileFromDirectory: {fullpath}", ex);
                return false;
            }
        }

        ///// <summary>
        ///// Win32 API scan functions
        ///// </summary>
        ///// <param name="directory"></param>
        ///// <returns></returns>
        //bool l_ScanDirectory
        //    (
        //    string directory,
        //    List<string> searchPattern,
        //    SearchOption searchOption,
        //    ref List<csImageFileInfo> files,
        //    Callback callback,
        //    int PauseCounter, OnScanProgressProcess SendProgress
        //    )
        //{
        //    IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        //    NativeMethods.WIN32_FIND_DATAW findData;
        //    IntPtr findHandle = INVALID_HANDLE_VALUE;


        //    try
        //    {
        //        if (bStop) return false;
        //        /// return if directory ignored.

        //        /// loop for files
        //        findHandle = NativeMethods.FindFirstFileW(FileFunctions.AddSearchPattern(directory, "*.*" /*searchPattern[0]*/), out findData);
        //        if (findHandle != INVALID_HANDLE_VALUE)
        //        {
        //            if (directory.Length > 3 && IsDirectoryAttributesIgnored(directory, findData.dwFileAttributes))
        //                return true;

        //            do
        //            {
        //                if (bStop) break;
        //                if (findData.cFileName.Equals(@".") || findData.cFileName.Equals(@".."))
        //                    continue;


        //                string fullpath = PathEx.Combine(directory, findData.cFileName).ToLower();

        //                if ((searchOption == SearchOption.AllDirectories) && (findData.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
        //                {
        //                    // DIRECTORY TREATMENT

        //                    if (bStop) break;
        //                    if (!IsDirectoryAttributesIgnored(fullpath, findData.dwFileAttributes)
        //                        /*&& !IsDirectoryIgnored(fullpath) */
        //                        )
        //                     {
        //                        //dirs.Add(fullpath);
        //                        // call recursively the sub directories
        //                        l_ScanDirectory(fullpath, searchPattern, searchOption, ref files, callback, PauseCounter, SendProgress);
        //                    }
        //                }
        //                else
        //                {
        //                    if (bStop) break;
        //                    // FILE TREATMENT


        //                    //if (fullpath.ToLower().Contains(@"c:\windows\system32\catroot2") ||
        //                    //    fullpath.ToLower().Contains(@"c:\windows\system32\drivers\etc") ||
        //                    //    fullpath.ToLower().Contains(@"c:\windows\system32\spool\drivers") ||
        //                    //    fullpath.ToLower().Contains(@"c:\windows\system32\spool\v4dirs")
        //                    //    )
        //                    {



        //                        bool bFound = false;
        //                        foreach (var pattern in searchPattern)
        //                        {
        //                            //if (pattern == "*.*" || fullpath.ToLower().EndsWith(pattern))
        //                            //{
        //                            //    bFound = true;
        //                            //    break;
        //                            //};

        //                            if (Regex.IsMatch(Path.GetExtension(fullpath), cGlobalSettings.ImageFileExtensionsList, RegexOptions.IgnoreCase))
        //                            {
        //                                bFound = true;
        //                                break;

        //                            };

        //                        }                        

        //                        if (!bFound) continue;
        //                        FileInfo fileInfo = new FileInfo(fullpath);
        //                        if (fileInfo.Length < cGlobalSettings.MinFileSize)
        //                            continue;

        //                        if (!IsFileAttributesIgnored(fullpath, findData.dwFileAttributes))
        //                        {


        //                            csImageFileInfo record = new csImageFileInfo()
        //                            {
        //                                filePath = fileInfo.FullName.ToLower(),
        //                                fileName = fileInfo.Name,
        //                                fileSize = fileInfo.Length,
        //                                //fileInfo = FileInfoList[i],
        //                                bHashFoundInDB = false,
        //                                isLocked = false,
        //                                createDate = fileInfo.CreationTime,
        //                                modDate = fileInfo.LastWriteTime,
        //                                Filetype = string.Format(fileInfo.Extension.Substring(1, fileInfo.Extension.Length - 1).ToUpper() + " File"),
        //                                fileSizeWithUnit = AppFunctions.StrFormatByteSize(fileInfo.Length),
        //                                folderPath = fileInfo.DirectoryName.AddBackSlash(),
        //                                //hash = AppFunctions.CreateMD5Hash(fileInfo.Name);

        //                                //dtCreated = findData.ftCreationTime.ToDateTime(),
        //                                // dtModified = findData.ftLastWriteTime.ToDateTime(),

        //                            };


        //                            cGlobalSettings.listImageFileInfo.TryAdd(record.key, record);

        //                            files.Add(record);

        //                            bool isRawFormatFound = false;
        //                            /// just an index

        //                         //   cGlobalSettings.MediaFilePathToFileId.TryAdd(record.filePath.ToLower(), record.key);

        //                            if (!isRawFormatFound)
        //                            {
        //                                var extFound = cGlobalSettings.listFileType.Where(p => p.Key > -1 && p.Value.IsNewLibSupportedExt && p.Value.ImageFileExtension.IndexOf(fileInfo.Extension, StringComparison.OrdinalIgnoreCase) >= 0);

        //                                if (extFound != null && extFound.Any())
        //                                {
        //                                    isRawFormatFound = true;
        //                                    //cGlobalSettings.listImageFileInfo[i].bRAWFormat = extFound.FirstOrDefault().Value.IsNewLibSupportedExt;
        //                                }
        //                                extFound = null;
        //                            }


        //                            if (SendProgress != null && cGlobalSettings.listImageFileInfo.Count % 750 == 0)
        //                            {
        //                                SendProgress(fullpath, -1, cGlobalSettings.listImageFileInfo.Count, 0, 0);
        //                            }


        //                            files.Add(record);



        //                            if (SendProgress != null && files.Count % 750 == 0)
        //                            {
        //                                SendProgress(fullpath, -1, files.Count, 0, 0);
        //                            }

        //                            /*
        //                            cEMLData record = new cEMLData();
        //                            record.FilePath = fullpath;
        //                            record.DirectoryPath = Path.GetDirectoryName(fullpath);
        //                            record.FilePathHash = CRC64.CRC_64.GetHash(fullpath);
        //                            record.CreationDateTime = findData.ftCreationTime.ToDateTime();
        //                            record.LastModificationDateTime = findData.ftLastWriteTime.ToDateTime();
        //                            record.FileSize = (long)findData.nFileSizeLow + (long)findData.nFileSizeHigh * 4294967296;
        //                            files.Add(record);
        //                            */
        //                        }

        //                        //if (PauseCounter > -1 && files.Count >= PauseCounter)
        //                        //{
        //                        //    if (bStop) break;

        //                        //    callback?.Invoke(ref files, false);

        //                        //    files.Clear();
        //                        //}
        //                    }

        //                }

        //                if (bStop) break;

        //                cGlobalSettings.oManualReset.WaitOne();
        //                if (cGlobalSettings.abortNow)
        //                {
        //                    break; 
        //                }
        //            }
        //            while (NativeMethods.FindNextFile(findHandle, out findData));

        //        }

        //        return true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("Win32APIScanner::ScanDirectory", ex);
        //    }
        //    finally
        //    {
        //        if (findHandle != INVALID_HANDLE_VALUE) NativeMethods.FindClose(findHandle);
        //    }

        //    return false;
        //}


        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        bool IsDirectoryAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            /// c:\ is in exclusion
            ///  c:\a
            ///  c:\a\b\c => (excluded folder) exclusion
            ///  c:\a (fullpathname) scanning 
            ///  c:\a.StartsWith("c:\a\b\c")  = false => correct way of checking
            ///  c:\a\b\c.StartsWith("c:\a\") => true => wrong way of checking
            ///  
          
            //if(cGlobalSettings.listExcludedFolderPaths == null)
            //{
            //    cGlobalSettings.listExcludedFolderPaths = new List<csExcludedPath>();
            //}
            // Get list of excluded paths
           
            FullName = FullName.ToLower().AddBackSlash();
            if(ExcludedFolders == null) 
            {
                foreach (var item in cGlobalSettings.listExcludedFolderPaths)
                {
                    ExcludedFolders = new HashSet<string>();
                    ExcludedFolders.Add(item.FolderPath);
                }
            }
           
           else if (ExcludedFolders != null && ExcludedFolders.Any(x => FullName.StartsWith(x)))
                return true;

            /// ignoring the reparse points as they make the data double like
            /// programdata = all users
            if (
                ((Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                || ((Attributes & FileAttributes.System) == FileAttributes.System)
                || ((Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                )
                return true;

            /// we are scanning all directories now
            return false;
        }

        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsDirectoryIgnored(string FullName/*, FileAttributes Attributes*/)
        {
            return (/*(FullName.Length < 4) || */
                FullName.ToLower().StartsWith("$rmmetadata")
                || (FullName.Length > 248)
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
        bool IsFileAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            if (ExcludedFiles != null && ExcludedFiles.Contains(FullName.ToLower()))
                return true;

            // we are scanning all files.
            return false;
            /*
            return (
                (SkipHiddenFile && (Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                || (IsSkipSystemItems && (Attributes & FileAttributes.System) == FileAttributes.System));            
                */

        }

        /// <summary>
        /// To skip folders like created with symbol #255.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsIgnoredSpecialFolder(string FullName)
        {
            return string.IsNullOrEmpty(new DirectoryInfo(FullName.AddBackSlash()).Name.Trim());
        }


        private eFuncRetVal FilterScanPaths( ref List<csScanPaths> ScanPathsData ,out List<csScanPaths> filteredScanFolders)
        {

            /// File path which are requested for scan 
            /// Case: Child and parent folder both are added.
            /// File and folder both added.
            /// ScanPathsData == > FilteredPathsList

            List<string> folders = null;

            List<csScanPaths> filteredFolders = new List<csScanPaths>();

            filteredScanFolders = new List<csScanPaths>();

            List<string> files = null;
            IEnumerable<csScanPaths> oScanFolderPaths = null;
            try
            {
                //if (Program.IsCommandLine)
                //{
                //    folders = new List<string>();
                //    folders.Add(Program.SourceFolder);
                //    files = new List<string>();
                //}
               
                    oScanFolderPaths = cGlobalSettings.listScanPaths.Where(p => p.PathType == cClientEnum.ePathType.folder && p.IsSelected == true); // .Select(p => p.strPath).ToList();

                    files = cGlobalSettings.listScanPaths.Where(p => (p.PathType == cClientEnum.ePathType.file || p.PathType == cClientEnum.ePathType.library) && p.IsSelected == true).Select(p => p.ScanPath).ToList();
                

                // --------------------------- Filter Folders ---------------------------
                foreach (csScanPaths dirPath in oScanFolderPaths)
                {
                    /*string mapPath = csMappedDrive.GetUNCPath(dirPath);*/
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        return eFuncRetVal.CANCEL;
                    csScanPaths pathToAdd = dirPath;

                    for (int i = 0; i < filteredFolders.Count; i++)
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                            return eFuncRetVal.CANCEL;

                        if (AppFunctions.isChildFolder(filteredFolders[i].ScanPath, dirPath.ScanPath)  /*dirPath.StartsWith(filteredFolders[i])*/)
                        {
                            pathToAdd = null;
                            break;
                        }
                        else if (AppFunctions.isChildFolder(dirPath.ScanPath, filteredFolders[i].ScanPath)/*filteredFolders[i].StartsWith(dirPath)*/)
                        {
                            if (dirPath.IncludeSubfolder)
                            {
                                pathToAdd = dirPath;
                                filteredFolders.RemoveAt(i);
                                filteredScanFolders.RemoveAt(i);
                                i--;
                            }
                            //break;
                        }
                    }
                    if (pathToAdd != null)
                    {
                        filteredScanFolders.Add(dirPath);
                        filteredFolders.Add(pathToAdd);
                    }
                }
                // --------------------------- Add files of filter folders ---------------------------
                //foreach (csScanPaths str in filteredFolders)
                //{
                //    cGlobalSettings.oManualReset.WaitOne();
                //    if (cGlobalSettings.abortNow)
                //        return eFuncRetVal.CANCEL;
                //    try
                //    {
                //        // DirSearch(str.strPath, ref ScanPathsData, str.IncludeSubfolder);
                //        NativeMethods.WIN32_FIND_DATAW findData;
                //        NativeMethods.FindFirstFileW(FileFunctions.AddSearchPattern(str.ScanPath, "*.{jpg,png}"/*searchPattern*/),  out findData);
                //    }
                //    catch (System.Exception ex)
                //    {
                //        cGlobalSettings.oLogger.WriteLogException("FilterScanPaths :: AppFunctions.DirSearch", ex);
                //    }
                //}
                // ---------------------------------------------------------------------------------

                // --------------------------- Filter Files ---------------------------
                for (int i = 0; i < files.Count; i++)
                {
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        return eFuncRetVal.CANCEL;
                    if (File.Exists(files[i]))
                    {
                        string fileDir = Path.GetDirectoryName(files[i]);
                        string pathToAdd = files[i];
                        foreach (csScanPaths dir in filteredFolders)
                        {
                            cGlobalSettings.oManualReset.WaitOne();
                            if (cGlobalSettings.abortNow)
                                return eFuncRetVal.CANCEL;
                            if (fileDir.StartsWith(dir.ScanPath))
                            {
                                pathToAdd = null;
                                break;
                            }
                        }

                        if (pathToAdd != null)
                        {
                            csScanPaths filePathObj = new csScanPaths
                            {
                                ScanPath = pathToAdd,
                                PathType = cClientEnum.ePathType.file,
                                IsSelected = true,
                                IncludeSubfolder = false
                            };

                            filteredScanFolders.Add(filePathObj);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FilterScanPaths", ex);
            }
            finally
            {
                if (folders != null) folders.Clear(); folders = null;
                if (files != null) files.Clear(); files = null;
            }
            return eFuncRetVal.SUCCESS;

        }
    }
}
