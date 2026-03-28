using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
        int nIncrementalKey = 0;

        private static readonly List<string> DefaultImageSearchPatterns = new List<string>
    {
        // Common image formats
        "*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.tif", "*.webp",
        
        // RAW camera formats
        "*.cr2", "*.cr3", "*.nef", "*.arw", "*.dng", "*.orf", "*.rw2", "*.pef", "*.srw", "*.raf",
        
        // Additional formats
        "*.ico", "*.svg", "*.heic", "*.heif", "*.avif", "*.jxl",
        
        // Adobe formats
        "*.psd", "*.psb", "*.ai",
        
        // Vector formats
        "*.eps", "*.pdf"
    };

        public bool StartScan(List<csScanPaths> ScanPathsData,
            List<string> searchPattern,
            SearchOption searchOption,
            ref HashSet<csImageFileInfo> files,
            Callback callback,
            int PauseCounter,
            OnScanProgressProcess SendProgress = null)
        {
            IntPtr wow64Value = IntPtr.Zero;
            try
            {
                eFuncRetVal returnStatus = FilterScanPaths(ref ScanPathsData, out filteredScanPath);

                bool bRet = true;
                for (int i = 0; i < filteredScanPath.Count; i++)
                {
                    if (!bRet)
                        break;

                    if (filteredScanPath[i].PathType == cClientEnum.ePathType.folder)
                    {
                        bRet = l_ScanDirectory(filteredScanPath[i].ScanPath,
                            searchPattern,
                            searchOption,
                            ref files,
                            callback,
                            PauseCounter,
                            SendProgress);


                    }
                    else if (filteredScanPath[i].PathType == cClientEnum.ePathType.file)
                    {
                        /// Add in a list
                        /// 
                        ProcessSingleFile(filteredScanPath[i].ScanPath, ref files, SendProgress);


                    }


                }

                //// ADDED: Mark Phase 1 complete at 5%
                //if (SendProgress != null)
                //{
                //    SendProgress(
                //        cClientEnum.eScanPhase.Search,
                //        "File scanning completed",
                //        5,  // Phase 1 ends at 5%
                //        cGlobalSettings.listImageFileInfo.Count,
                //        cGlobalSettings.listImageFileInfo.Count,
                //        0,
                //        "",
                //        false);
                //}

                SendProgress(cClientEnum.eScanPhase.Search, "", -1, cGlobalSettings.listImageFileInfo.Count, 0, 0);





                return bRet;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //try
                //{
                //    if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                //    {
                //        // Re-enable redirection.
                //        NativeMethods.Wow64RevertWow64FsRedirection(wow64Value);
                //    }
                //}
                //finally
                //{
                //    ;
                //}

            }
            return false;
        }


        /// <summary>
        /// Win32 API scan functions for directory scanning
        /// </summary>
        bool l_ScanDirectory(
            string directory,
            List<string> searchPattern,
            SearchOption searchOption,
            ref HashSet<csImageFileInfo> files,
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

                            cGlobalSettings.oManualReset.WaitOne();
                            if (cGlobalSettings.abortNow)
                            {
                                break;
                            }

                            try
                            {

                                ProcessSingleFile(fullpath, ref files, SendProgress);

                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger?.WriteLogException($"Error processing file: {fullpath}", ex);
                                // Continue to next file even if this one fails
                            }
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
        /// Process a single file and add it to the collection if it matches criteria
        /// </summary>
        private bool ProcessSingleFile(
            string filePath,
            ref HashSet<csImageFileInfo> files,
            OnScanProgressProcess SendProgress)
        {
            try
            {
                if (bStop) return false;

                // Normalize path to lowercase
                //filePath = filePath.ToLower();

                // Check if file extension matches image patterns
                if (!Regex.IsMatch(Path.GetExtension(filePath),
                    cGlobalSettings.ImageFileExtensionsList,
                    RegexOptions.IgnoreCase))
                {
                    return false; // Not an image file
                }

                FileInfo fileInfo = new FileInfo(filePath);

                // Check if file exists
                if (!fileInfo.Exists)
                {
                    return false;
                }

                // Check minimum file size
                if (fileInfo.Length < cGlobalSettings.MinFileSize)
                {
                    return false;
                }

                // Check if file attributes should be ignored
                if (IsFileAttributesIgnored(filePath, fileInfo.Attributes))
                {
                    return false;
                }

                // Create file info record
                csImageFileInfo record = new csImageFileInfo()
                {
                    key = nIncrementalKey++,
                    filePath = fileInfo.FullName,
                    fileName = fileInfo.Name,
                    fileSize = fileInfo.Length,
                    bHashFoundInDB = false,
                    isLocked = false,
                    createDate = fileInfo.CreationTime,
                    modDate = fileInfo.LastWriteTime,
                    Filetype = string.Format(fileInfo.Extension.Substring(1).ToUpper()),
                    fileSizeWithUnit = AppFunctions.StrFormatByteSize(fileInfo.Length),
                    folderPath = fileInfo.DirectoryName.AddBackSlash(),
                };

                // Add to concurrent collection
                cGlobalSettings.listImageFileInfo.TryAdd(record.key, record);

                // Add to index
                cGlobalSettings.MediaFilePathToFileId.TryAdd(record.filePath.ToLower(), record.key);

                // MODIFIED: Send progress updates every 125 files (Phase 1: 0-5%)
                if (SendProgress != null && cGlobalSettings.listImageFileInfo.Count % 125 == 0)
                {
                    // Calculate Phase 1 percentage (0-5%)
                    int percentage = Math.Min(5, (cGlobalSettings.listImageFileInfo.Count * 5) / Math.Max(1, 10000));

                    SendProgress(
                        cClientEnum.eScanPhase.Search,
                        record.filePath,
                        percentage,  // 0-5%
                        cGlobalSettings.listImageFileInfo.Count,
                        cGlobalSettings.listImageFileInfo.Count,
                        0,
                        "",
                        true);
                }

                return true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException($"ProcessSingleFile: {filePath}", ex);
                return false;
            }
        }


        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        bool IsDirectoryAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            FullName = FullName.ToLower().AddBackSlash();
            if (ExcludedFolders == null)
            {
                foreach (var item in cGlobalSettings.listExcludedFolderPaths)
                {
                    ExcludedFolders = new HashSet<string>();
                    ExcludedFolders.Add(item.FolderPath);
                }
            }

            else if (ExcludedFolders != null && ExcludedFolders.Any(x => FullName.StartsWith(x)))
                return true;

            if (
                ((Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                || ((Attributes & FileAttributes.System) == FileAttributes.System)
                || ((Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                )
                return true;

            return false;
        }

        public static bool IsDirectoryIgnored(string FullName/*, FileAttributes Attributes*/)
        {
            return (/*(FullName.Length < 4) || */
                FullName.ToLower().StartsWith("$rmmetadata")
                || (FullName.Length > 248)
                || IsIgnoredSpecialFolder(FullName)
                );

        }

        bool IsFileAttributesIgnored(string FullName, FileAttributes Attributes)
        {
            if (ExcludedFiles != null && ExcludedFiles.Contains(FullName.ToLower()))
                return true;

            return false;
        }

        public static bool IsIgnoredSpecialFolder(string FullName)
        {
            return string.IsNullOrEmpty(new DirectoryInfo(FullName.AddBackSlash()).Name.Trim());
        }


        private eFuncRetVal FilterScanPaths(ref List<csScanPaths> ScanPathsData, out List<csScanPaths> filteredScanFolders)
        {
            List<string> folders = null;
            List<csScanPaths> filteredFolders = new List<csScanPaths>();
            filteredScanFolders = new List<csScanPaths>();
            List<string> files = null;
            IEnumerable<csScanPaths> oScanFolderPaths = null;
            try
            {
                oScanFolderPaths = cGlobalSettings.listScanPaths.Where(p => p.PathType == cClientEnum.ePathType.folder && p.IsSelected == true);
                files = cGlobalSettings.listScanPaths.Where(p => (p.PathType == cClientEnum.ePathType.file || p.PathType == cClientEnum.ePathType.library) && p.IsSelected == true).Select(p => p.ScanPath).ToList();

                foreach (csScanPaths dirPath in oScanFolderPaths)
                {
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        return eFuncRetVal.CANCEL;
                    csScanPaths pathToAdd = dirPath;

                    for (int i = 0; i < filteredFolders.Count; i++)
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                            return eFuncRetVal.CANCEL;

                        if (AppFunctions.isChildFolder(filteredFolders[i].ScanPath, dirPath.ScanPath))
                        {
                            pathToAdd = null;
                            break;
                        }
                        else if (AppFunctions.isChildFolder(dirPath.ScanPath, filteredFolders[i].ScanPath))
                        {
                            if (dirPath.IncludeSubfolder)
                            {
                                pathToAdd = dirPath;
                                filteredFolders.RemoveAt(i);
                                filteredScanFolders.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    if (pathToAdd != null)
                    {
                        filteredScanFolders.Add(dirPath);
                        filteredFolders.Add(pathToAdd);
                    }
                }

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