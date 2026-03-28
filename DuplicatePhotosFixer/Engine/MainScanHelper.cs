using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine.WinApiScan;
using DuplicatePhotosFixer.Helpers;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.Engine
{
    public class MainScanHelper
    {
        IgnoreRules ignoreRules = null;
        const float MIN_VALUE_IF_NOT_MATCHED = 0.7f;
        const float MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES = 0.85f;
        ulong maxScore = (ulong)((1.0f - MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES/*0.85f*//*sliderInitialMatching.minValue*/) * (cGlobalSettings.ImageResize * cGlobalSettings.ImageResize));
        uint totLength = (uint)(cGlobalSettings.ImageResize * cGlobalSettings.ImageResize);//reSizeVal here is governed by Preferences, default value = 64

        #region Validationfn
        //internal bool ValidateScanPath(string pathSelected, ref string statusMessage)
        //{
        //    statusMessage = string.Empty;
        //    bool isValid = false;
        //    try
        //    {
        //        if (ignoreRules == null)
        //            ignoreRules = new IgnoreRules();

        //        string strPath = pathSelected.RemoveEndBackSlash();
        //        /// Check for path length.
        //        if (strPath.Length > 248)
        //        {
        //            statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
        //            return false;
        //        }

        //        /// Check if path is a shortcut
        //        //try
        //        //{
        //        //    if (System.IO.Path.GetExtension(strPath) == ".lnk")
        //        //    {
        //        //        strPath = GetShortcutTarget(strPath);// GetShortcutTargetFile(strPath);
        //        //        if (!System.IO.File.Exists(strPath) && !Directory.Exists(strPath))
        //        //        {
        //        //            try
        //        //            {
        //        //                if (System.IO.File.Exists(pathSelected))
        //        //                {
        //        //                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell(); //Create a new WshShell Interface
        //        //                    IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(pathSelected); //Link the interface to our shortcut
        //        //                    strPath = link.TargetPath;
        //        //                }
        //        //            }
        //        //            catch (UnauthorizedAccessException ex)
        //        //            {
        //        //                cGlobalSettings.oLogger.WriteLogException(string.Format("addValidScanPath : Exception in getting the path extension {0} ", strPath), ex);
        //        //            }
        //        //            catch (Exception ex)
        //        //            {
        //        //                cGlobalSettings.oLogger.WriteLogException(string.Format("addValidScanPath : Exception in getting the path extension {0} ", pathSelected), ex);
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //catch (UnauthorizedAccessException ex)
        //        //{
        //        //    cGlobalSettings.oLogger.WriteLogException("addValidScanPath : UnauthorizedAccessException Exception in getting the path extension", ex);
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    cGlobalSettings.oLogger.WriteLogException("ValidateScanPath : Exception in getting the path extension", ex);
        //        //}

        //        cClientEnum.ePathType pathtype = cClientEnum.ePathType.folder;
        //        // ========== ========== ========== ========== 
        //        /* Check if it is a file or folder 
        //         * if file then check for supported extensions
        //        ========== ========== ========== ========== */

        //        //if (cGlobalSettings.eCurrentScanMode == cGlobalSettings.eScanMode.FileSearch ||
        //        //        cGlobalSettings.eCurrentScanMode == cGlobalSettings.eScanMode.EML)
        //        DirectoryInfo dir = null;
        //        if (Directory.Exists(strPath))
        //        {
        //            dir = new DirectoryInfo(strPath);
        //            string strPathLower = dir.FullName.ToLower();
        //            if ((dir.Attributes & FileAttributes.Temporary) != 0)
        //            {
        //                statusMessage = cResourceManager.LoadString("IDS_TEMP_PATH_NOT_ALLOWED");
        //                return false;
        //            }
        //            else if (cGlobalSettings.scannerRules.IsDirectoryIgnored(strPathLower))
        //            {
        //                isValid = false;
        //                statusMessage = cResourceManager.LoadString("IDS_WINDOW_PATH");//");// "Path cannot be added as it is system path ! ";
        //                if (strPathLower.Length > 248)
        //                {
        //                    statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
        //                }
        //                cGlobalSettings.oLogger.WriteLogVerbose(string.Format("Not able to add path: {1}, Reasons-IsDirectoryIgnored:{0} ",
        //                    "true", strPathLower));

        //                return false;
        //            }
        //        }
        //        else if (File.Exists(strPath))
        //        {
        //            DirectoryInfo info = null;
        //            try
        //            {
        //                bool bExtFound = false;
        //                /// Start from 1, skip everything(*.*) extension
        //                foreach (KeyValuePair<int, csFileTypes> data in cGlobalSettings.listFileType)
        //                {
        //                    if (string.IsNullOrEmpty(data.Value.ImageFileExtension))
        //                        continue;

        //                    string strExt = Path.GetExtension(strPath);
        //                    if (string.IsNullOrEmpty(strExt))
        //                        break;

        //                    if (Regex.IsMatch(strExt, data.Value.ImageFileExtension, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
        //                    {
        //                        pathtype = (cClientEnum.ePathType)data.Value.ID;
        //                        bExtFound = true;
        //                        break;
        //                    }
        //                }

        //                if (!bExtFound)
        //                {
        //                    pathtype = (cClientEnum.ePathType.Everything);
        //                    isValid = true;
        //                    // statusMessage = cResourceManager.LoadString("DFF_HOME_UC_ERROR_ITEM_NO_ADDED");
        //                }
        //                info = new DirectoryInfo(strPath);
        //                string FilePath = info.FullName.ToLower();
        //                info = null;
        //                if (cGlobalSettings.scannerRules.IsDirectoryIgnored(FilePath))
        //                {
        //                    statusMessage = cResourceManager.LoadString("IDS_WINDOW_PATH");// "Path is a system file/folder path";
        //                    if (FilePath.Length > 248)
        //                    {
        //                        statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
        //                    }
        //                    isValid = false;
        //                    return false;
        //                }
        //                FilePath = null;
        //            }
        //            catch (System.IO.PathTooLongException ex)
        //            {
        //                statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
        //                isValid = false;
        //                cGlobalSettings.oLogger.WriteLogException("addValidScanPath :: exception in matching extension list while adding path", ex);
        //                return false;
        //            }
        //            catch (Exception ex)
        //            {
        //                cGlobalSettings.oLogger.WriteLogException("addValidScanPath :: exception in matching extension list while adding path", ex);
        //            }
        //            finally
        //            {
        //                info = null;
        //            }
        //            FileInfo fileData = null;
        //            try
        //            {
        //                fileData = new FileInfo(strPath);
        //                string strPathLower = fileData.FullName.ToLower();
        //                if ((fileData.Attributes & FileAttributes.Temporary) != 0 || (fileData.Attributes & FileAttributes.Hidden) != 0)
        //                {
        //                    statusMessage = cResourceManager.LoadString("IDS_TEMP_PATH_NOT_ALLOWED");// "Temperary file(s) can not be added.";
        //                    isValid = false;
        //                    return false;
        //                }
        //                else if ((ignoreRules.IgnoreFileHardPattern != null && ignoreRules.IgnoreFileHardPattern.IsMatch(strPathLower))
        //                            || (ignoreRules.IgnoreExtensionHardPattern != null && ignoreRules.IgnoreExtensionHardPattern.IsMatch(strPathLower))
        //                            || (ignoreRules.IgnorePathHardPattern != null && ignoreRules.IgnorePathHardPattern.IsMatch(strPathLower))
        //                            )
        //                {
        //                    cGlobalSettings.oLogger.WriteLogVerbose(string.Format("Not able to add file path {3}, Reasons-IgnoreFileHardPattern.IsMatch:{0}, IgnoreExtensionHardPattern.IsMatch:{1}, IgnorePathHardPattern.IsMatch:{2}",
        //                       ignoreRules.IgnoreFileHardPattern.IsMatch(strPathLower),
        //                        ignoreRules.IgnoreExtensionHardPattern.IsMatch(strPathLower),
        //                        ignoreRules.IgnorePathHardPattern.IsMatch(strPathLower),
        //                        strPathLower
        //                        ));
        //                    isValid = false;
        //                    return false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                statusMessage = string.Format(cResourceManager.LoadString("IDS_PATH_ACCESS_DENIED"), strPath.AddBackSlash());
        //                isValid = false;

        //                cGlobalSettings.oLogger.WriteLogException(string.Format("addValidScanPath :: Path: {0} , exception when we are checking path is temperary or hidden. Reason:", strPath.AddBackSlash()), ex);
        //                return false;
        //            }
        //            finally
        //            {
        //                fileData = null;
        //            }
        //        }

        //        // ==========  Check if path is new i.e. not already added ========== 
        //        var ScanData = cGlobalSettings.listScanPaths.Where(p => string.Compare(p.ScanPath.AddBackSlash(), strPath.AddBackSlash(), true) == 0);
        //        if (ScanData != null)
        //        {
        //            bool bAlreadtAdded = false;
        //            foreach (csScanPaths scanPathData in ScanData)
        //            {
        //                /// Path exist in same type of list.
        //                if (scanPathData.TypeOfInclusion == cGlobalSettings.CurrentTypeOfInclusion)
        //                {
        //                    /// Path already exist in same type of list
        //                    /// 
        //                    statusMessage = string.Format(cResourceManager.LoadString("IDS_PATH_IS_ALREADY_ADDED"), strPath, cResourceManager.LoadString("IDS_" + scanPathData.TypeOfInclusion.ToString()));
        //                    bAlreadtAdded = true;
        //                    break;
        //                }
        //                /// path is already added in either include/exclude list.

        //                //else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Protected)
        //                //{
        //                //    // All good 
        //                //    continue;
        //                //}

        //                else if (scanPathData.TypeOfInclusion == eTypeOfInclusion.Included)

        //                {
        //                    statusMessage = string.Format(cResourceManager.LoadString("DPF_HOME_UC_ALREADY_ADDED"), strPath, cResourceManager.LoadString("IDS_" + scanPathData.TypeOfInclusion.ToString()));
        //                    bAlreadtAdded = true;
        //                    break;
        //                }
        //            }
        //            if (bAlreadtAdded)
        //                return false;
        //        }

        //        /// Check if parent folder is already added or not
        //        /// 
        //        var ParentPathData = cGlobalSettings.listScanPaths.Where(p => strPath.IndexOf(p.ScanPath.AddBackSlash()) >= 0);

        //        if (ParentPathData != null)
        //        {
        //            bool bParentPathAdded = false;
        //            foreach (csScanPaths scanPathData in ParentPathData)
        //            {
        //                if (scanPathData.TypeOfInclusion == cGlobalSettings.CurrentTypeOfInclusion)
        //                {
        //                    statusMessage = string.Format(cResourceManager.LoadString("DPF_HOME_UC_PARENT_PATH"), scanPathData.ScanPath, cResourceManager.LoadString("IDS_" + scanPathData.TypeOfInclusion));/*"Path is already added!";*/
        //                    bParentPathAdded = true;
        //                    break;
        //                }
        //                else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Protected)
        //                {
        //                    // All good 
        //                    continue;
        //                }

        //                else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Included)
        //                {
        //                    // Allow if parent folder is in Protect list
        //                    if (scanPathData.TypeOfInclusion == eTypeOfInclusion.Protected)
        //                        continue;

        //                    statusMessage = string.Format(cResourceManager.LoadString("DPF_HOME_UC_PARENT_PATH"), scanPathData.ScanPath, cResourceManager.LoadString("IDS_" + scanPathData.TypeOfInclusion));/*"Path is already added!";*/
        //                    bParentPathAdded = true;
        //                    break;
        //                }
        //            }

        //            if (bParentPathAdded)
        //                return false;
        //        }


        //        // ==========  Check if scanpath already added in Exclusion List ========== 
        //        var ScanPathData = cGlobalSettings.listExcludedFolderPaths.Where(p => string.Compare(p.FolderPath.AddBackSlash(), strPath.AddBackSlash(), true) == 0);
        //        if (ScanPathData != null)
        //        {
        //            bool bAlreadtAdded = false;
        //            foreach (csExcludedPath scanPathData in ScanPathData)
        //            {
        //                /// Path exist in same type of list.
        //                if (scanPathData.TypeOfExclusion == cGlobalSettings.CurrentTypeOfExclusion)
        //                {
        //                    /// Path already exist in same type of list
        //                    /// 
        //                    statusMessage = string.Format(cResourceManager.LoadString("DPF_HOME_UC_EXCLUDED_PATH"));
        //                    bAlreadtAdded = true;
        //                    break;
        //                }

        //            }
        //            if (bAlreadtAdded)
        //                return false;
        //        }

        //        /// Check if child folder is already added or not
        //        /// 
        //        var ChildPathData = cGlobalSettings.listScanPaths.Where(p => p.ScanPath.AddBackSlash().IndexOf(strPath) >= 0);

        //        if (ChildPathData != null)
        //        {
        //            bool bParentPathAdded = false;
        //            foreach (csScanPaths scanPathData in ChildPathData)
        //            {
        //                if (scanPathData.TypeOfInclusion == cGlobalSettings.CurrentTypeOfInclusion)
        //                {
        //                    continue;
        //                }
        //                else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Protected)
        //                {
        //                    // All good 
        //                    continue;
        //                }
        //                else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Included)
        //                {
        //                    // Allow to add child path in excluded list
        //                    continue;
        //                }
        //                //else if (cGlobalSettings.CurrentTypeOfInclusion == eTypeOfInclusion.Excluded)
        //                //{
        //                //    // Allow if child folder is in Protect list
        //                //    if (scanPathData.TypeOfInclusion == eTypeOfInclusion.Protected)
        //                //        continue;

        //                //    statusMessage = string.Format(cResourceManager.LoadString("IDS_CHILD_FOLDER_IS_ALREADY_ADDED"), scanPathData.ScanPath, cResourceManager.LoadString("IDS_" + scanPathData.TypeOfInclusion));/*"Path is already added!";*/
        //                //    bParentPathAdded = true;
        //                //    break;
        //                //}
        //            }

        //            if (bParentPathAdded)
        //                return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return true;
        //}
        #endregion


        internal bool DValidatePath(string pathSelected, ref string statusMessage)
        {
            bool isValid = false;

            List<string> listExcludePaths = null;
            try
            {
                if (ignoreRules == null)
                    ignoreRules = new IgnoreRules();

                string strPath = pathSelected.RemoveEndBackSlash();
                /// Check for path length.
                if (strPath.Length > 248)
                {
                    statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
                    return false;
                }
                int counter = 0;
                cClientEnum.ePathType pathtype = cClientEnum.ePathType.file;


                // ========== ========== ========== ========== 
                /* Check if it is a file or folder 
                 * if file then check for supported extensions
                 ========== ========== ========== ========== */
                if (cGlobalSettings.CurrentScanMode == eScanMode.FileSearch)
                {
                    if (Directory.Exists(strPath))
                    {
                        pathtype = cClientEnum.ePathType.folder;
                        isValid = true;
                    }
                    else if (File.Exists(strPath))
                    {

                        //string ext = string.Format(@"^.*\." + cGlobalSettings.ImageFileExtensionsList.Replace(".", "") + "$");


                        if (Regex.IsMatch(Path.GetExtension(strPath), cGlobalSettings.ImageFileExtensionsList, RegexOptions.IgnoreCase))
                        {
                            isValid = true;
                            //Program.oMainReference.Home_addSelectedPath(true, s, enumClass.ePathType.file, true);
                        }

                        else
                        {
                            counter++;
                            if (counter > 1)
                                statusMessage = cResourceManager.LoadString("DPF_HOME_UC_ERROR_ITEM_NO_ADDED");
                            else
                                statusMessage = cResourceManager.LoadString("DPF_HOME_UC_ERROR_ITEM_NO_ADDED");
                        }
                    }

                }
                else if (cGlobalSettings.CurrentScanMode == eScanMode.PicasaLibrary ||
                    cGlobalSettings.CurrentScanMode == eScanMode.Lightroom)
                {
                    if (File.Exists(strPath))
                    {
                        pathtype = (ePathType)enumClass.ePathType.library;
                        if (Regex.IsMatch(Path.GetFileName(strPath), cGlobalSettings.GetImageFileExtensionsList, RegexOptions.IgnoreCase))
                        {
                            isValid = true;
                            //Program.oMainReference.Home_addSelectedPath(true, s, enumClass.ePathType.file, true);
                        }
                        else
                        {
                            counter++;
                            if (counter > 1)
                                statusMessage = cResourceManager.LoadString("IDS_ADD_VALID_PATH");
                            else
                                statusMessage = cResourceManager.LoadString("IDS_ADD_VALID_PATH");
                        }
                    }
                    else
                    {
                        statusMessage = cResourceManager.LoadString("IDS_ADD_VALID_PATH");
                    }
                }
                else // For all other scan modes consider path as valid.
                {
                    pathtype = (ePathType)enumClass.ePathType.folder;
                    isValid = true;
                }

                // ==========  Check is path is new i.e. not already added ========== 
                if (isValid)
                {
                    string mapPath = csMappedDrive.GetUNCPath(strPath);
                    if (strPath != mapPath)
                    {

                        string userIP = csMappedDrive.ip;

                        string ip = Path.GetPathRoot(mapPath).Replace("\\\\", "");

                        if (ip.StartsWith(userIP))
                        {
                            // User add its own map drive
                        }

                    }
                    bool isPathAlreadyAdded = cGlobalSettings.listScanPaths.Where(path => path.ScanPath == strPath.Trim()).Any();
                    if (isPathAlreadyAdded)
                    {
                        //isPathAlreadyAdded = null;
                        isValid = false;
                        if (string.IsNullOrEmpty(statusMessage))
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_ALREADY_ADDED");
                        //MessageBox.Show("Path is already added! ", "Duplicate Photos Fixer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                //==========  Exclusion List check  ========== 
                if (isValid)
                {
                    listExcludePaths = new List<string>();
                    //if(cGlobalSettings.listExcludedFolderPaths == null)
                    //{
                    //    cGlobalSettings.listExcludedFolderPaths = new List<csExcludedPath>();
                    //}
                    // Get list of excluded paths
                    foreach (var item in cGlobalSettings.listExcludedFolderPaths)
                    {
                        listExcludePaths.Add(item.FolderPath);
                    }
                    var isPathAlreadyAdded = listExcludePaths.Where(p => p.Trim() == strPath.Trim()).Any();
                    if (isPathAlreadyAdded)
                    {
                        //isPathAlreadyAdded = null;
                        isValid = false;
                        statusMessage = cResourceManager.LoadString("DPF_cGlobalSettings_FOLDER_EXCLUDED") + " " + Environment.NewLine + cResourceManager.LoadString("DPF_cGlobalSettings_FOLDER_EXCLUDED_HINT");
                        //MessageBox.Show("Path is already added! ", "Duplicate Photos Fixer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                //==========  Default Exclude Folder Check  ========== 
                if (isValid)
                {
                    // If path is a system directory path, then don't add the folder
                    if (AppFunctions.isDefaultExcludePath(strPath))
                    {
                        statusMessage = cResourceManager.LoadString("DPF_HOME_UC_SYSTEM_PATH");
                        isValid = false;
                    }
                }

                // ========== New path is a Sub-folder of already added folder ========== 
                if (isValid)
                {
                    foreach (string path in cGlobalSettings.listScanPaths.Select(p => p.ScanPath).ToList())
                    {
                        if (AppFunctions.isChildFolder(path, strPath))
                        {
                            isValid = false;
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_PARENT_PATH");
                            break;
                            //MessageBox.Show("Parent folder is already added! ", "Duplicate Photos Fixer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                // ==========  New path is a Sub-folder of already excluded folder path ========== 
                if (isValid)
                {
                    foreach (string excludePath in listExcludePaths)
                    {
                        if (AppFunctions.isChildFolder(excludePath, strPath))
                        {
                            isValid = false;
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_EXCLUDED_PATH");
                            break;
                        }
                    }
                }

                // ========== New path is a Sub-folder of Default Exclude Folder path ==========
                if (isValid)
                {
                    foreach (string defaultExcludePath in cGlobalSettings.listDefaultExcludePaths)
                    {
                        if (AppFunctions.isChildFolder(defaultExcludePath, strPath))
                        {
                            isValid = false;
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_SYSTEM_DIR");
                            break;
                        }
                    }
                }


                /// ========== Only looks for the file hard exclusion ========== 
                if (isValid)
                {
                    if (pathtype == (ePathType)enumClass.ePathType.file || pathtype == (ePathType)enumClass.ePathType.library)
                    {
                        string strPathLower = strPath.ToLower();
                        if ((ignoreRules.IgnoreFileHardPattern != null && ignoreRules.IgnoreFileHardPattern.IsMatch(strPathLower))
                            || (ignoreRules.IgnoreExtensionHardPattern != null && ignoreRules.IgnoreExtensionHardPattern.IsMatch(strPathLower))
                            || (ignoreRules.IgnorePathHardPattern != null && ignoreRules.IgnorePathHardPattern.IsMatch(strPathLower))
                            )
                        {
                            isValid = false;
                        }
                    }

                    else if (pathtype == (ePathType)enumClass.ePathType.folder)
                    {
                        if (ignoreRules.IsDirectoryIgnored(strPath))
                            isValid = false;
                    }

                    if (!isValid)
                    {
                        statusMessage = cResourceManager.LoadString("DPF_HOME_UC_SYSTEM_FILE_PATH");
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainScanHelper:: ValidatePath: ", ex);
            }
            finally
            {
                if (listExcludePaths != null) listExcludePaths.Clear();
                listExcludePaths = null;
            }

            return isValid;
        }

        public bool validExcludFolderPath(string excludedFolder, ref string statusMessage)
        {
            statusMessage = string.Empty;
            bool isValid = true;
            try
            {
                if (ignoreRules == null)
                    ignoreRules = new IgnoreRules();

                string strPath = excludedFolder.RemoveEndBackSlash();
                /// Check for path length.
                if (strPath.Length > 248)
                {
                    statusMessage = cResourceManager.LoadString("IDS_PATH_TOO_LONG");
                    return false;
                }

                // -----------  Check is path is new i.e. not already added ----------
                if (isValid)
                {
                    var isPathAlreadyExcluded = cGlobalSettings.listExcludedFolderPaths.Where(p => string.Compare(p.FolderPath.AddBackSlash(), strPath.AddBackSlash(), true) == 0).SingleOrDefault();
                    if (isPathAlreadyExcluded != null)
                    {
                        isPathAlreadyExcluded = null;
                        isValid = false;
                        if (string.IsNullOrEmpty(statusMessage))
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_ALREADY_EXCLUDED"); // Path is already excluded!";
                    }
                }

                //---------------------------- Include List check  ----------------------------------
                if (isValid)
                {
                    var isPathAlreadyExcluded = cGlobalSettings.listScanPaths.Where(p => string.Compare(p.ScanPath.AddBackSlash(), strPath.AddBackSlash(), true) == 0).FirstOrDefault();
                    if (isPathAlreadyExcluded != null)
                    {
                        isPathAlreadyExcluded = null;
                        isValid = false;
                        if (string.IsNullOrEmpty(statusMessage))
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_ADDED_SCAN_PATH_LIST");
                    }
                }

                // ---------------------------------- New path is a Sub-folder of already added scan folder ---------------------------
                if (isValid)
                {

                    cGlobalSettings.listScanPaths.ForEach(p =>
                    {
                        if (AppFunctions.isChildFolder(p.ScanPath, excludedFolder))
                        {
                            isValid = true;

                            return;
                        }

                    });
                    if (!isValid)
                    {
                        statusMessage = cResourceManager.LoadString("DPF_HOME_UC_PARENT_ADDED_SCAN");
                    }

                }


                // ---------------------------------- New path is a Sub-folder of already excluded folder path ---------------------------
                if (isValid)
                {
                    foreach (csExcludedPath excludePath in cGlobalSettings.listExcludedFolderPaths)
                    {
                        if (AppFunctions.isChildFolder(excludePath.FolderPath, excludedFolder))
                        {
                            isValid = false;
                            statusMessage = cResourceManager.LoadString("DPF_HOME_UC_PARENT_ADDED_EXCLUDED");
                            break;
                        }
                    }
                }
                // -------------------- Child folder of exclude folder is added in scan list --------------------
                if (isValid)
                {
                    cGlobalSettings.listScanPaths.ForEach(p =>
                {
                    if (AppFunctions.isChildFolder(excludedFolder, p.ScanPath))
                    {
                        isValid = false;

                        return;
                    }
                });
                    if (!isValid)
                    {
                        statusMessage = cResourceManager.LoadString("DPF_HOME_UC_NOT_ABLE_TO_EXCLUDE");
                    }

                }

                //if (isValid )
                //{
                //    if (!string.IsNullOrEmpty(excludedFolder))
                //    {

                //        //check if this path is already added in global exclude list
                //        var isPathAlreadyExcluded = cGlobalSettings.listExcludedFolderPaths.Where(p => string.Compare(p.FolderPath.AddBackSlash(), strPath.AddBackSlash(), true) == 0).SingleOrDefault();
                //        if (isPathAlreadyExcluded == null)
                //            cGlobalSettings.listExcludedFolderPaths.Add(excludedFolder);
                //    }
                //}


            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("validExcludeFolder :: Folder:" + excludedFolder, ex);
                //return false;
            }

            return isValid;
        }


        private string GetShortcutTarget(string file)
        {
            try
            {
                FileStream fileStream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read);
                using (System.IO.BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                        // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                    // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                    // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                    // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                    // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public void compareForExactPhotos(OnScanProgressProcess SendProgress = null)
        {
            cGlobalSettings.arrAllFilesGrouping = null;
            cGlobalSettings.arrAllFilesGrouping = new BlockingCollection<csDuplicatesGroup>();
            CancellationTokenSource cts = null;

            try
            {
                int TotalPhotos = cGlobalSettings.listImageFileInfo.Keys.Count;
                var groupsOfFilesWithSize = cGlobalSettings.listImageFileInfo.Where(p => p.Value.fileSize > 0).GroupBy(s => s.Value.fileSize).Where(s => s.Count() > 1);

                int i = 0;
                cts = new CancellationTokenSource();
                int TotalGroupsCount = groupsOfFilesWithSize.Count();

                groupsOfFilesWithSize.AsParallel()
                .WithDegreeOfParallelism(/*AppFunctions.GetMaxDegreeOfParallelism()*/1)
                .WithCancellation(cts.Token)
                .ForAll(x =>
                {
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        return;
                    Thread.Sleep(0);
                    //OnScanProgress("Comparing duplicates", 100 * i / TotalGroupsCount, (totalPhotosProcessed += groupsOfFilesWithSize.ElementAt(i).Count()) + " processed", cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));

                    //OnScanProgress(cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_DUPLICATES"), 100 * i / TotalGroupsCount, string.Format(cResourceManager.LoadString("DPF_PROCESSOR_PROCESSED"), totalPhotosProcessed += x.Count()), cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));

                    i++;

                    foreach (var imageFile in x)
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                            break;

                        cGlobalSettings.listImageFileInfo[imageFile.Key].calcHashForExactMatch = true;
                    }

                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        return;

                    //SendProgress(
                    //        cClientEnum.eScanPhase.Compare,
                    //        cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_PHOTOS"),
                    //        percentage,
                    //        TotalPhotos,
                    //        i,
                    //        nTotalDupsFound,
                    //        cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PATIENT_WHILE_COMPARING_MSG"));

                });
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("compareForExactPhotos() :: ", ex);
            }
            finally
            {
                cts = null;
            }

        }
        // UPDATED: compareForSimilarPhotos method for Phase 3 (50-100%)
        // Replacd  existing compareForSimilarPhotos method with this one

        // Add this FIXED method to MainScanHelper.cs - Replace the existing compareForSimilarPhotos

        public void compareForSimilarPhotos(OnScanProgressProcess SendProgress = null)
        {
            cGlobalSettings.arrAllFilesGrouping = null;
            cGlobalSettings.arrAllFilesGrouping = new BlockingCollection<csDuplicatesGroup>();

            try
            {
                int i = 0;
                int TotalPhotos = cGlobalSettings.listImageFileInfo.Keys.Count;
                int nTotalDupsFound = 0;
                long ntotalDuplicateSize = 0;

                // FIX: Get the time and GPS parameters like original code
                uint maxImageTimeDiff = (uint)(cGlobalSettings.MatchSettingsData?.nTimeIntervalSec ?? 0);
                uint userSelectedGpsValue = (uint)(cGlobalSettings.MatchSettingsData?.nGPSMtr ?? 0);

                cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Starting with params - Time: {maxImageTimeDiff}s, GPS: {userSelectedGpsValue}m");

                foreach (int KeyNode in cGlobalSettings.listImageFileInfo.Keys)
                {
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                        break;

                    i++;

                    if (i % 9 == 0)
                    {
                        Thread.Sleep(0);
                        int percentage = 50 + ((i * 50) / TotalPhotos);
                        percentage = Math.Min(99, percentage);

                        SendProgress(
                            cClientEnum.eScanPhase.Compare,
                            cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_PHOTOS"),
                            percentage,
                            TotalPhotos,
                            i,
                            nTotalDupsFound,
                            cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PATIENT_WHILE_COMPARING_MSG"));
                    }

                    if (cGlobalSettings.listImageFileInfo[KeyNode].hash == null)
                        continue;

                    // FIX: Pass the parameters to ComparePhotoNode
                    ComparePhotoNode(KeyNode, maxImageTimeDiff, userSelectedGpsValue);
                }

                // Calculate duplicates
                foreach (var group in cGlobalSettings.arrAllFilesGrouping)
                {
                    if (group.childrenKey.Count > 1)
                    {
                        nTotalDupsFound += group.childrenKey.Count - 1;
                    }
                }

                CalculateDuplicateSize(ref ntotalDuplicateSize, ref nTotalDupsFound);
                cGlobal.TotalDuplicateFileSize = ntotalDuplicateSize;
                cGlobal.TotalDulicateFilesCount = nTotalDupsFound;

                cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Complete - Duplicates: {nTotalDupsFound}");
                VerifyAndFixDuplicateCount();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("compareForSimilarPhotos", ex);
            }
        }

        //  FIX: Update ComparePhotoNode signature to accept parameters
        void ComparePhotoNode(int selfNode, uint maxImageTimeDiff, uint userSelectedGpsValue)
        {
            bool isAdded = false;
            int index = 0;

            foreach (csDuplicatesGroup group in cGlobalSettings.arrAllFilesGrouping)
            {
                cGlobalSettings.oManualReset.WaitOne();
                if (cGlobalSettings.abortNow)
                    break;

                float match = 0.0f;
                int matchingFileNode = 0;
                float tMatch = 0.0f;

                try
                {
                    foreach (int fileNode in group.childrenKey)
                    {
                        //FIX: Pass parameters to comparison method
                        tMatch = CompareHashForPhotoNode(selfNode, fileNode, maxImageTimeDiff, userSelectedGpsValue);

                        if (tMatch >= (cGlobalSettings.isSimilarMatchChecked ? 0.85 : 0.99))
                        {
                            if (tMatch > match)
                            {
                                match = tMatch;
                                matchingFileNode = fileNode;
                            }
                            isAdded = true;
                        }
                        else
                        {
                            isAdded = false;
                            break;
                        }
                    }

                    if (isAdded)
                    {
                        if (group.childrenKey.Count == 1)
                        {
                            cGlobalSettings.listImageFileInfo[matchingFileNode].match = match;
                            cGlobalSettings.listImageFileInfo[matchingFileNode].matchesWithNodeKey = selfNode;
                        }
                        cGlobalSettings.listImageFileInfo[selfNode].match = match;
                        cGlobalSettings.listImageFileInfo[selfNode].matchesWithNodeKey = matchingFileNode;

                        if (cGlobalSettings.listImageFileInfo[matchingFileNode].match < match)
                        {
                            cGlobalSettings.listImageFileInfo[matchingFileNode].match = match;
                            cGlobalSettings.listImageFileInfo[matchingFileNode].matchesWithNodeKey = selfNode;
                        }

                        cGlobalSettings.listImageFileInfo[selfNode].parent = group;
                        group.childrenKey.Add(selfNode);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("ComparePhotoNode", ex);
                }

                index++;
            }

            if (cGlobalSettings.abortNow || isAdded)
                return;

            // Create new group if not added
            csDuplicatesGroup newGroup = null;
            try
            {
                cGlobalSettings.listImageFileInfo[selfNode].matchesWithNodeKey = selfNode;
                cGlobalSettings.listImageFileInfo[selfNode].match = 1.0f;
                newGroup = new csDuplicatesGroup();
                newGroup.childrenKey.Add(selfNode);
                cGlobalSettings.listImageFileInfo[selfNode].parent = newGroup;
                cGlobalSettings.arrAllFilesGrouping.Add(newGroup);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ComparePhotoNode", ex);
            }
            finally
            {
                newGroup = null;
            }
        }

        // Update CompareHashForPhotoNode signature
        float CompareHashForPhotoNode(int keynode1, int keynode2, uint maxImageTimeDiff, uint userSelectedGpsValue)
        {
            float match = 0;
            try
            {
                csImageFileInfo node1 = cGlobalSettings.listImageFileInfo[keynode1];
                csImageFileInfo node2 = cGlobalSettings.listImageFileInfo[keynode2];

                if (node1.hash == null || node2.hash == null)
                    return MIN_VALUE_IF_NOT_MATCHED;

                if (!cGlobalSettings.isSimilarMatchChecked && node1.fileSize != node2.fileSize)
                    return MIN_VALUE_IF_NOT_MATCHED;

                if (node1.exifInfo != null && node2.exifInfo != null)
                {
                    // FIX: Use PASSED parameters, not global settings
                    if (cGlobalSettings.ExtraSettings.DbConsiderTimeInterval && cGlobalSettings.isSimilarMatchChecked)
                    {
                        if (node1.exifInfo.CaptureDate != DateTime.MinValue && node1.exifInfo.CaptureDate != DateTime.MaxValue &&
                            node2.exifInfo.CaptureDate != DateTime.MinValue && node2.exifInfo.CaptureDate != DateTime.MaxValue)
                        {
                            // Use passed parameter instead of global setting
                            if (maxImageTimeDiff < System.Math.Abs(node1.exifInfo.CaptureDate.Subtract(node2.exifInfo.CaptureDate).TotalSeconds))
                            {
                                return MIN_VALUE_IF_NOT_MATCHED;
                            }
                        }
                        else
                        {
                            if (cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt)
                            {
                                //  Use passed parameter
                                if (maxImageTimeDiff < System.Math.Abs(node1.createDate.Subtract(node2.createDate).TotalSeconds))
                                {
                                    return MIN_VALUE_IF_NOT_MATCHED;
                                }
                            }
                        }
                    }

                    if (cGlobalSettings.ExtraSettings.DbConsiderGPS && cGlobalSettings.isSimilarMatchChecked)
                    {
                        if (node1.exifInfo.latitudeDegree != 360 && node2.exifInfo.latitudeDegree != 360 &&
                            node1.exifInfo.longitudeDegree != 360 && node2.exifInfo.longitudeDegree != 360)
                        {
                            var R = 6378100.0;
                            var φ1 = AppFunctions.degreeToRadian(node1.exifInfo.latitudeDegree);
                            var φ2 = AppFunctions.degreeToRadian(node2.exifInfo.latitudeDegree);
                            var Δφ = AppFunctions.degreeToRadian(node2.exifInfo.latitudeDegree - node1.exifInfo.latitudeDegree);
                            var Δλ = AppFunctions.degreeToRadian(node2.exifInfo.longitudeDegree - node1.exifInfo.longitudeDegree);

                            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                                    Math.Cos(φ1) * Math.Cos(φ2) *
                                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
                            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                            double d = R * c;

                            //  Use passed parameter instead of global setting
                            if (userSelectedGpsValue < d)
                            {
                                return MIN_VALUE_IF_NOT_MATCHED;
                            }
                        }
                    }
                }

                // Rest of hash comparison logic...
                ulong score = 0;
                bool bMatched = false;

                for (int i = 0; i < node1.hash.Length; i++)
                {
                    score += NumberOfSetBits(node1.hash[i] ^ node2.hash[i]);
                    if (score > maxScore)
                    {
                        bMatched = false;
                        break;
                    }
                    else
                        bMatched = true;
                }

                if (!bMatched && cGlobalSettings.isSimilarMatchChecked)
                {
#if ROTATE_FLIPPED_CHECK
                    // Rotation/flip check logic...
#endif
                    if (!bMatched)
                        return MIN_VALUE_IF_NOT_MATCHED;
                }

                match = (float)(totLength - score) / (float)totLength;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("compareHashForPhotoNode", ex);
            }
            return match;
        }
        //public void compareForSimilarPhotos(OnScanProgressProcess SendProgress = null)
        //{
        //    cGlobalSettings.arrAllFilesGrouping = null;
        //    cGlobalSettings.arrAllFilesGrouping = new BlockingCollection<csDuplicatesGroup>();

        //    try
        //    {
        //        int i = 0;
        //        int TotalPhotos = cGlobalSettings.listImageFileInfo.Keys.Count;
        //        int nTotalDupsFound = 0;
        //        long ntotalDuplicateSize = 0;

        //        bool bCompletionSent = false;  // ADDED: Flag to prevent sending completion multiple times

        //        cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Starting comparison of {TotalPhotos} photos");

        //        foreach (int KeyNode in cGlobalSettings.listImageFileInfo.Keys)
        //        {
        //            cGlobalSettings.oManualReset.WaitOne();
        //            if (cGlobalSettings.abortNow)
        //                break;

        //            i++;

        //            if (i % 9 == 0)
        //            {
        //                Thread.Sleep(0);

        //                // Calculate percentage from 50% to 100% (50% range for Phase 3)
        //                int percentage = 50 + ((i * 50) / TotalPhotos);
        //                percentage = Math.Min(99, percentage);  // Cap at 100%

        //                cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Progress - {percentage}%, Compared: {i}/{TotalPhotos}, Duplicates found: {nTotalDupsFound}");

        //                SendProgress(
        //                    cClientEnum.eScanPhase.Compare,
        //                    cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_PHOTOS"),
        //                    percentage,
        //                    TotalPhotos,
        //                    i,
        //                    nTotalDupsFound,
        //                    cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PATIENT_WHILE_COMPARING_MSG"));
        //            }

        //            if (cGlobalSettings.listImageFileInfo[KeyNode].hash == null)
        //                continue;

        //            ComparePhotoNode(KeyNode);
        //        }

        //        nTotalDupsFound = 0;

        //        foreach (var group in cGlobalSettings.arrAllFilesGrouping)
        //        {
        //            if (group.childrenKey.Count > 1)
        //            {
        //                nTotalDupsFound += group.childrenKey.Count - 1;
        //            }
        //        }
        //        CalculateDuplicateSize(ref ntotalDuplicateSize, ref nTotalDupsFound);

        //        // ADDED: Update global settings with results
        //        cGlobal.TotalDuplicateFileSize = ntotalDuplicateSize;

        //        cGlobal.TotalDulicateFilesCount = nTotalDupsFound;



        //        cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Comparison complete - Total duplicate files: {nTotalDupsFound}");
        //        cGlobalSettings.oLogger?.WriteLogVerbose($"[compareForSimilarPhotos] Total groups: {cGlobalSettings.arrAllFilesGrouping.Count}");

        //        //// FIXED: Only send final update ONCE at 100%
        //        //if (!bCompletionSent)
        //        //{
        //        //    SendProgress(
        //        //        cClientEnum.eScanPhase.Compare,
        //        //        "Comparison completed",
        //        //        100,  // Exactly 100%
        //        //        TotalPhotos,
        //        //        TotalPhotos,
        //        //        nTotalDupsFound,
        //        //        "");

        //        //    bCompletionSent = true;
        //        //    cGlobalSettings.oLogger?.WriteLogVerbose("[compareForSimilarPhotos] Final update sent at 100%");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger?.WriteLogException("compareForSimilarPhotos", ex);
        //    }
        //}

        //  New method to calculate duplicate size
        private void CalculateDuplicateSize(ref long totalSize, ref int duplicateCount)
        {
            try
            {
                foreach (var group in cGlobalSettings.arrAllFilesGrouping)
                {
                    // Only process groups with more than 1 file (actual duplicates)
                    if (group.childrenKey.Count > 1)
                    {
                        // Get all files in this duplicate group
                        var filesInGroup = group.childrenKey
                            .Where(key => cGlobalSettings.listImageFileInfo.ContainsKey(key))
                            .Select(key => cGlobalSettings.listImageFileInfo[key])
                            .ToList();

                        if (filesInGroup.Count > 1)
                        {
                            // Sort by file size (descending) to keep the largest one
                            // Or you can sort by date, quality, etc. based on your logic
                            var sortedFiles = filesInGroup.OrderByDescending(f => f.fileSize).ToList();

                            // The first file is the one to keep, rest are duplicates
                            // So we calculate size of all except the first one
                            for (int i = 1; i < sortedFiles.Count; i++)
                            {
                                totalSize += sortedFiles[i].fileSize;
                                duplicateCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("CalculateDuplicateSize", ex);
            }
        }

        public static void VerifyAndFixDuplicateCount()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("\n========== DUPLICATE COUNT VERIFICATION ==========");

                // 1. Check basic data
                int totalFiles = cGlobalSettings.listImageFileInfo?.Count ?? 0;
                int totalGroups = cGlobalSettings.arrAllFilesGrouping?.Count ?? 0;

                cGlobalSettings.oLogger?.WriteLogVerbose($"Total files: {totalFiles}");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Total groups: {totalGroups}");

                if (totalFiles == 0)
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("ERROR: No files loaded!");
                    return;
                }

                if (totalGroups == 0)
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("ERROR: No groups created - comparison didn't run!");
                    return;
                }

                // 2. Check hash status
                int filesWithHash = cGlobalSettings.listImageFileInfo.Count(f => f.Value.hash != null);
                cGlobalSettings.oLogger?.WriteLogVerbose($"Files with hash: {filesWithHash}/{totalFiles}");

                if (filesWithHash == 0)
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("ERROR: No hashes found - hash calculation failed!");
                    return;
                }

                if (filesWithHash < totalFiles)
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose($"WARNING: {totalFiles - filesWithHash} files missing hashes!");
                }

                // 3. Count duplicates the CORRECT way
                int duplicateCount = 0;
                long duplicateSize = 0;
                int groupsWithDuplicates = 0;

                foreach (var group in cGlobalSettings.arrAllFilesGrouping)
                {
                    if (group.childrenKey.Count > 1)
                    {
                        groupsWithDuplicates++;

                        // Count ALL files in duplicate groups
                        foreach (var key in group.childrenKey)
                        {
                            if (cGlobalSettings.listImageFileInfo.ContainsKey(key))
                            {
                                duplicateCount++;
                                duplicateSize += cGlobalSettings.listImageFileInfo[key].fileSize;
                            }
                        }
                    }
                }

                cGlobalSettings.oLogger?.WriteLogVerbose($"\nRESULTS:");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Groups with duplicates: {groupsWithDuplicates}");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Total duplicate files: {duplicateCount}");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Total duplicate size: {duplicateSize} bytes");

                // 4. Update global values
                cGlobal.TotalDulicateFilesCount = duplicateCount;
                cGlobal.TotalDuplicateFileSize = duplicateSize;

                cGlobalSettings.oLogger?.WriteLogVerbose($"\nUpdated cGlobal.TotalDulicateFilesCount to: {duplicateCount}");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Updated cGlobal.TotalDuplicateFileSize to: {duplicateSize}");

                // 5. Show sample groups
                cGlobalSettings.oLogger?.WriteLogVerbose($"\nSample duplicate groups:");
                int sampleCount = 0;
                foreach (var group in cGlobalSettings.arrAllFilesGrouping.Where(g => g.childrenKey.Count > 1).Take(5))
                {
                    sampleCount++;
                    cGlobalSettings.oLogger?.WriteLogVerbose($"\n  Group {sampleCount}: {group.childrenKey.Count} files");
                    foreach (var key in group.childrenKey)
                    {
                        if (cGlobalSettings.listImageFileInfo.ContainsKey(key))
                        {
                            var file = cGlobalSettings.listImageFileInfo[key];
                            cGlobalSettings.oLogger?.WriteLogVerbose($"    - {file.fileName} (match: {file.match:F3})");
                        }
                    }
                }

                cGlobalSettings.oLogger?.WriteLogVerbose("\n========== END VERIFICATION ==========\n");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("DuplicateVerifier.VerifyAndFixDuplicateCount", ex);
            }
        }

        // Also added this diagnostic method to verify duplicates are being found
        public void DiagnosticCheckDuplicates()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("=== DIAGNOSTIC: Duplicate Detection ===");

                if (cGlobalSettings.arrAllFilesGrouping == null)
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("ERROR: arrAllFilesGrouping is null!");
                    return;
                }

                cGlobalSettings.oLogger?.WriteLogVerbose($"Total groups created: {cGlobalSettings.arrAllFilesGrouping.Count}");

                int totalDuplicates = 0;
                int groupsWithDups = 0;

                foreach (var group in cGlobalSettings.arrAllFilesGrouping)
                {
                    int dupCount = group.childrenKey.Count;
                    if (dupCount > 1)
                    {
                        groupsWithDups++;
                        totalDuplicates += dupCount - 1;
                        cGlobalSettings.oLogger?.WriteLogVerbose($"  Group: {dupCount} files ({dupCount - 1} duplicates)");

                        foreach (var fileKey in group.childrenKey)
                        {
                            if (cGlobalSettings.listImageFileInfo.ContainsKey(fileKey))
                            {
                                var file = cGlobalSettings.listImageFileInfo[fileKey];
                                cGlobalSettings.oLogger?.WriteLogVerbose($"    - {file.fileName}");
                            }
                        }
                    }
                }

                cGlobalSettings.oLogger?.WriteLogVerbose($"Total duplicate groups: {groupsWithDups}");
                cGlobalSettings.oLogger?.WriteLogVerbose($"Total duplicate files: {totalDuplicates}");
                cGlobalSettings.oLogger?.WriteLogVerbose("=== END DIAGNOSTIC ===");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("DiagnosticCheckDuplicates", ex);
            }
        }
        //void ComparePhotoNode(int selfNode)
        //{
        //    bool isAdded = false;
        //    int index = 0;
        //    foreach (csDuplicatesGroup group in cGlobalSettings.arrAllFilesGrouping)
        //    {
        //        cGlobalSettings.oManualReset.WaitOne();
        //        if (cGlobalSettings.abortNow)
        //            break;

        //        float match = 0.0f;
        //        int matchingFileNode = 0;
        //        float tMatch = 0.0f;

        //        try
        //        {
        //            //here the group children are parsed and are compared with selfNode passed
        //            foreach (int fileNode in group.childrenKey)
        //            {
        //                //get the matching percent of selfNode here
        //                tMatch = CompareHashForPhotoNode(selfNode, fileNode);


        //                //if the match percent (tMatch) is < 0.85 the node is NOT treated as duplicate
        //                if (tMatch >= (cGlobalSettings.isSimilarMatchChecked ? 0.85 : 0.99))
        //                {
        //                    //The selfNode passed meets the match criteria and is treated as duplicate
        //                    if (tMatch > match)
        //                    {
        //                        //update "match" variable to keep the BEST Match score
        //                        match = tMatch;
        //                        //update "matchingFileNode" object to keep the track of the BEST Matching NODE
        //                        matchingFileNode = fileNode;
        //                    }
        //                    //update the BOOL "isAdded" as YES to add the node to the group children later
        //                    isAdded = true;
        //                }
        //                else
        //                {
        //                    //selfNode object will be added to the duplicate group ONLY when it matches with ALL THE GROUP CHILDREN, if NOT parse and compare with next group
        //                    //here isAdded is marked NO, break to stop comparing with the current left children and pass on to compare with next group
        //                    isAdded = false;
        //                    break;
        //                }
        //            }

        //            if (isAdded)
        //            {
        //                if (group.childrenKey.Count == 1)//Done because first node was given 1 match with itself, so now as matchingFileNode matches with selfNode, update the matches with Node and the match score
        //                {
        //                    cGlobalSettings.listImageFileInfo[matchingFileNode].match = match;
        //                    cGlobalSettings.listImageFileInfo[matchingFileNode].matchesWithNodeKey = selfNode;
        //                }
        //                //update selfNode's matching Node and the matching score here
        //                cGlobalSettings.listImageFileInfo[selfNode].match = match;
        //                cGlobalSettings.listImageFileInfo[selfNode].matchesWithNodeKey = matchingFileNode;
        //                //if the matching Node has a match node that has match score greater than the current "match" value, update the matching Node here to track the BEST MATCH NODE and score
        //                if (cGlobalSettings.listImageFileInfo[matchingFileNode].match < match)
        //                {
        //                    cGlobalSettings.listImageFileInfo[matchingFileNode].match = match;
        //                    cGlobalSettings.listImageFileInfo[matchingFileNode].matchesWithNodeKey = selfNode;
        //                }
        //                //update the parent here
        //                cGlobalSettings.listImageFileInfo[selfNode].parent = group;
        //                //add selfNode to the group children array
        //                group.childrenKey.Add(selfNode);
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            cGlobalSettings.oLogger.WriteLogException("ComparePhotoNode", ex);
        //        }
        //        finally
        //        {

        //        }

        //        index++;
        //    }

        //    if (cGlobalSettings.abortNow || isAdded)
        //    {
        //        return;
        //    }
        //    //If selfNode is NOT added to any of the groups, create a group with this node as the first child
        //    csDuplicatesGroup newGroup = null;
        //    try
        //    {
        //        cGlobalSettings.listImageFileInfo[selfNode].matchesWithNodeKey = selfNode;
        //        cGlobalSettings.listImageFileInfo[selfNode].match = 1.0f;//default match score 1 is allotted, later it will be updated if there is a match found
        //        newGroup = new csDuplicatesGroup();
        //        newGroup.childrenKey.Add(selfNode);//Create group with selfNode
        //        cGlobalSettings.listImageFileInfo[selfNode].parent = newGroup;
        //        cGlobalSettings.arrAllFilesGrouping.Add(newGroup);
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("comparePhotoNode() :: ", ex);
        //    }
        //    finally
        //    {
        //        newGroup = null;
        //    }
        //}


        float CompareHashForPhotoNode(int keynode1, int keynode2)
        {
            float match = 0;
            try
            {
                csImageFileInfo node1 = cGlobalSettings.listImageFileInfo[keynode1];
                csImageFileInfo node2 = cGlobalSettings.listImageFileInfo[keynode2];

                if (node1.hash == null || node2.hash == null)
                {
                    return MIN_VALUE_IF_NOT_MATCHED;
                }

                if (!cGlobalSettings.isSimilarMatchChecked && node1.fileSize != node2.fileSize)
                {
                    return MIN_VALUE_IF_NOT_MATCHED;
                }
                if (node1.exifInfo != null && node2.exifInfo != null)
                {
                    // compare the capture times ONLY when BOTH the images have CAPTURE DATE in EXIF Info
                    //if (((node1.exifInfo.CaptureDate != DateTime.MinValue) && (node2.exifInfo.CaptureDate != DateTime.MinValue)))


                    if (cGlobalSettings.ExtraSettings.DbConsiderTimeInterval && cGlobalSettings.isSimilarMatchChecked)
                    {
                        if (node1.exifInfo.CaptureDate != DateTime.MinValue && node1.exifInfo.CaptureDate != DateTime.MaxValue && node2.exifInfo.CaptureDate != DateTime.MinValue && node2.exifInfo.CaptureDate != DateTime.MaxValue)
                        {
                            if (cGlobalSettings.MatchSettingsData.nTimeIntervalSec < System.Math.Abs(node1.exifInfo.CaptureDate.Subtract(node2.exifInfo.CaptureDate).TotalSeconds))
                            {
                                return MIN_VALUE_IF_NOT_MATCHED;
                            }
                        }
                        else
                        {
                            if (cGlobalSettings.ExtraSettings.DbConsiderTimeNoCaptureDt)
                            {
                                if (cGlobalSettings.MatchSettingsData.nTimeIntervalSec < System.Math.Abs(node1.createDate.Subtract(node2.createDate).TotalSeconds))
                                {
                                    return MIN_VALUE_IF_NOT_MATCHED;
                                }
                            }
                        }
                    }

                    if (cGlobalSettings.ExtraSettings.DbConsiderGPS && cGlobalSettings.isSimilarMatchChecked)
                    {
                        // Compare the distance between to points, if GPS information is there
                        if (node1.exifInfo.latitudeDegree != 360 && node2.exifInfo.latitudeDegree != 360 &&
                            node1.exifInfo.longitudeDegree != 360 && node2.exifInfo.longitudeDegree != 360)
                        {

                            var R = 6378100.0; // Radius of an earth (meters)

                            var φ1 = AppFunctions.degreeToRadian(node1.exifInfo.latitudeDegree);// node1.exifInfo.latitudeRadian;
                            var φ2 = AppFunctions.degreeToRadian(node2.exifInfo.latitudeDegree); //node2.exifInfo.latitudeRadian;
                            var Δφ = AppFunctions.degreeToRadian(node2.exifInfo.latitudeDegree - node1.exifInfo.latitudeDegree);
                            //var Δφ = (lat2 - lat1).toRadians();
                            var Δλ = AppFunctions.degreeToRadian(node2.exifInfo.longitudeDegree - node1.exifInfo.longitudeDegree);
                            //var Δλ = (lon2 - lon1).toRadians();

                            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                                    Math.Cos(φ1) * Math.Cos(φ2) *
                                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
                            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                            double d = R * c;
                            if (cGlobalSettings.MatchSettingsData.nGPSMtr < d)
                            {
                                return MIN_VALUE_IF_NOT_MATCHED;
                            }
                        }
                    }
                }

                ulong score = 0;

                //maxScore is taken to optimize the comparison: Any match score < 0.85 means we will not treat it as duplicate
                //This means other ways we can say any diff in hash > 0.15 means the match score will be < 0.85
                //So we can say the mismatch counts if pass 0.15% of totLength, that means THERE IS A MISMATCH, so we do not compare the remaining hash values.

#if true
                bool bMatched = false;
                //int matchCount = 0;
                for (int i = 0; i < node1.hash.Length; i++)
                {
                    score += NumberOfSetBits(node1.hash[i] ^ node2.hash[i]);
                    if (score > maxScore)
                    {
                        bMatched = false;
                        break;
                        //return MIN_VALUE_IF_NOT_MATCHED;//For optimization, if mismatch is greater than 0.15... return 0.7 (means NO MATCH)
                    }
                    else
                        bMatched = true;
                }


                if (!bMatched && cGlobalSettings.isSimilarMatchChecked)
                {
#if ROTATE_FLIPPED_CHECK
                    int StartIndex;
                    int EndIndex;
                    if (GetStartEndIndex(out StartIndex, out EndIndex))
                    {
                        if (node1.arrHash != null && node1.arrHash.Count > 0)
                        {
                            for (int p = StartIndex; p <= EndIndex; p++)
                            {
                                if (bMatched)
                                    break;
                                if (node1.arrHash.Count <= p)
                                    break;
                                score = 0;
                                for (int i = 0; i < node1.arrHash[p].Length; i++)
                                {
                                    score += NumberOfSetBits(node1.arrHash[p][i] ^ node2.hash[i]);
                                    if (score > maxScore)
                                    {
                                        bMatched = false;
                                        break;
                                        //return MIN_VALUE_IF_NOT_MATCHED;//For optimization, if mismatch is greater than 0.15... return 0.7 (means NO MATCH)
                                    }
                                    else
                                        bMatched = true;
                                }
                            }
                        }
                        else if (node2.arrHash != null && node2.arrHash.Count > 0)
                        {
                            for (int p = StartIndex; p <= EndIndex; p++)
                            {
                                if (bMatched)
                                    break;

                                if (node2.arrHash.Count <= p)
                                    break;
                                score = 0;
                                for (int i = 0; i < node2.arrHash[p].Length; i++)
                                {
                                    score += NumberOfSetBits(node2.arrHash[p][i] ^ node1.hash[i]);
                                    if (score > maxScore)
                                    {
                                        bMatched = false;
                                        break;
                                        //return MIN_VALUE_IF_NOT_MATCHED;//For optimization, if mismatch is greater than 0.15... return 0.7 (means NO MATCH)
                                    }
                                    else
                                        bMatched = true;
                                }
                            }
                        }
                    }
#endif
                    if (!bMatched)
                        return MIN_VALUE_IF_NOT_MATCHED;
                }

#else
                for (int i = 0; i < totLength; i++)
                {
                    if (node1.hash[i] != node2.hash[i])//Node HASH is a stream of 1/0 (Array of length reSizeVal*reSizeVal)
                    {
                        score++;
                        if (score > maxScore)
                            return MIN_VALUE_IF_NOT_MATCHED;//For optimization, if mismatch is greater than 0.15... return 0.7 (means NO MATCH)
                    }
                }
#endif
                match = (float)(totLength - score) / (float)totLength;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("compareHashForPhotoNode", ex);
            }
            return match;//return the actual match score
        }


        static ulong NumberOfSetBits(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
        }

        bool GetStartEndIndex(out int StartIndex, out int EndIndex)
        {
            bool bConsiderotatedFlippedImages = false;
            StartIndex = -1;
            EndIndex = -1;
            try
            {
                if (cGlobalSettings.MatchSettingsData.bConsiderFlipped)
                {
                    StartIndex = 4;
                    EndIndex = 7;
                    bConsiderotatedFlippedImages = true;
                }

                if (cGlobalSettings.MatchSettingsData.bConsiderRotated)
                {
                    StartIndex = 1;
                    EndIndex = EndIndex > -1 ? 7 : 3;
                    bConsiderotatedFlippedImages = true;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetStartEndIndex:: ", ex);
            }
            return bConsiderotatedFlippedImages;
        }

    }
}
