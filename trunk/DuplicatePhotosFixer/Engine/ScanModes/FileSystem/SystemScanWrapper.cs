using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Helpers;
using ImageMagick;
using DuplicatePhotosFixer.Sqlite;
using MyLogger;
using DuplicatePhotosFixer.HelperClasses;

namespace DuplicatePhotosFixer.Engine.ScanModes.FileSystem
{
    class SystemScanWrapper : IScannerWrapper
    {
        SystemScanHelper objSystemScanHelper = null;
        string lastFolderPath = string.Empty;
        int FolderIndex = 0;
        public override bool AuthCode(string clientId, string clientSecret, string redirectUri, Callback callback)
        {
            throw new NotImplementedException();
        }

        public override bool Browse(out string[] SelectedPaths)
        {
            SelectedPaths = null;
            try
            {
                SelectedPaths = HelperClasses.cSelectFilesFolders.ShowFolder_System(App.oMainReference);

                if (SelectedPaths == null || SelectedPaths.Length <= 0)
                    return false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SystemScanWrapper:: Browse: ", ex);
            }
            return true;

        }

       

        public override bool Delete(object client, List<object> list)
        {
            throw new NotImplementedException();
        }

        public override string FormatProgressMessage(object folder)
        {
            throw new NotImplementedException();
        }

        public override bool ProcessMessage(object client, object folder, object mail, ref object record, Callback callback, int PauseCounter, string directoryPath)
        {
            throw new NotImplementedException();
        }

        public static string GetThumbnailImage(string SourcePath, bool bCreateThumbIfNotFound = false)
        {
            //return null;
            string ThumbPath = null;

            try
            {
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(SourcePath));
                ThumbPath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName_Thumb);

                if (File.Exists(ThumbPath))
                {
                    return ThumbPath;
                }
                else
                {
                    bool bNew = false;
                    var extFound = cGlobalSettings.listFileType.Where(p => p.Key > -1 && p.Value.ImageFileExtension.IndexOf(Path.GetExtension(SourcePath), StringComparison.OrdinalIgnoreCase) >= 0);

                    if (extFound != null)
                    {
                        bNew = extFound.FirstOrDefault().Value.IsNewLibSupportedExt;
                    }

                    if (!bNew)
                        return null;

                    if (bCreateThumbIfNotFound && SaveImageThumb(SourcePath, ref ThumbPath))
                    {
                        return ThumbPath;
                    }
                    else
                    {
                        ThumbPath = null;
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: GetThumbnailImage: ", ex);
            }
            return ThumbPath;
        }

        static Size GetThumbnalSize(int actualWidth, int actualHeight, int BitMapSize)
        {
            Size size = new Size();
            if (actualWidth == actualHeight)
            {
                size.Width = size.Height = BitMapSize;
            }
            else if (actualWidth > actualHeight)
            {
                int nFactor = (int)Math.Ceiling((float)actualWidth / BitMapSize);

                size.Width = actualWidth / nFactor;
                size.Height = actualHeight / nFactor;
            }
            else
            {
                int nFactor = (int)Math.Ceiling((float)actualHeight / BitMapSize);

                size.Width = actualWidth / nFactor;
                size.Height = actualHeight / nFactor;
            }
            return size;
        }
        public static bool SaveImageThumb(string ImageFullPath, ref string ThumbPath, int ThumbSize = 250)
        {
            MagickImage imm = null;
            bool bThumbCreated = false;
            try
            {
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(ImageFullPath));
                ThumbPath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName_Thumb);
                using (imm = new MagickImage(ImageFullPath))
                {
                    /// Write image thumbnail with JPG format
                    /// 
                    Size thumbSize = GetThumbnalSize((int)imm.Width, (int)imm.Height, ThumbSize);
                    imm.Thumbnail((int)(uint)thumbSize.Width, (int)(uint)thumbSize.Height);
                    imm.Write(ThumbPath, MagickFormat.Jpg);
                }

                if (File.Exists(ThumbPath))
                    bThumbCreated = true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SystemScanWrapper:: ConvertHeicToJPG: ", ex);
            }
            finally
            {
                if (imm != null)
                    imm.TryDispose();
                imm = null;
            }

            return bThumbCreated;
        }

        public override bool ReadAllMessages(object client, object folder, ref List<csImageFileInfo> fileRecordsData0, Callback callback, int PauseCounter, string directoryPath)
        {
            CancellationTokenSource cts = null;
            //            try
            //            {

            //#if !SQLITE_NEEDED

            //                //if (cGlobalSettings.sqliteLogger == null)
            //                //{
            //                //    cGlobalSettings.sqliteLogger = new cSqliteLogger();
            //                //    cGlobalSettings.sqliteLogger.InitDB();
            //                //}

            //                //if (eFuncRetVal.SUCCESS != cGlobalSettings.sqliteLogger.FillTempTableWithScannedPath(cGlobalSettings.MediaFilePathToFileId.Select(p => p.Key)))
            //                //{
            //                //    cGlobalSettings.oLogger.WriteLogVerbose("Not able to insert temp data in a table.");
            //                //    return false;
            //                //}

            //                //cGlobalSettings.sqliteLogger.LoadSqliteToDictionary(false);
            //#endif

            //var groupSize = cGlobalSettings.dictFileRecordsData.GroupBy(p => p.Value.FileSize); // fileRecordsData.GroupBy(p => p.FileSize);

            var hashGroups = cGlobalSettings.listImageFileInfo.GroupBy(x => AppFunctions.CreateMD5Hash(x.Value.fileName));

           
            // var groupbySearchOptions = ValidateSearchOptions(cGlobalSettings.listImageFileInfo);
            //Dictionary<string, List<FileRecord>> dictDupsData = new Dictionary<string, List<FileRecord>>();

            int nTotalFilesCount = cGlobalSettings.listImageFileInfo.Count;
            int GroupNumber = 0;
            int nFilesProcessed = 0;
            int nTotalDupsFound = 0;


            List<csImageFileInfo> newFileRecordsData = new List<csImageFileInfo>();
            List<FileRecord> modifiedFileRecordsData = new List<FileRecord>();
            cts = new CancellationTokenSource();



            foreach (var group in hashGroups)
            {
                // Show the data till the scanning is stopped
                cGlobalSettings.oManualReset.WaitOne();
                if (cGlobalSettings.abortNow)
                {
                    OnScanCompletedProcess("Scan aborted.", "", true, 100);
                    break;
                }

                int nItems = group.Count();

                if (nItems == 1)
                {
                    nFilesProcessed += 1;
                    continue;
                }

                group.AsParallel()
                    .WithDegreeOfParallelism(3)
                    .WithCancellation(cts.Token)
                    .ForAll(p =>
                    //group.All(p =>
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                        {
                            OnScanCompletedProcess("Scan aborted", "", true, 100);

                            return;
                        }

                        nFilesProcessed += 1;

                            /// Hash already calculated
                          //  if (p.Value.bHashFoundInDB && !p.Value.bModified)
                            return;

                        //p.Value.FileHash = AppFunctions.GetMD5(p.Value.FilePath);

                            // generate thumnail cache, if file Updated then get new thumb also.
                             string ThumbPath = GetThumbnailImage(p.Value.filePath, false);
                        p.Value.ThumbnailPath = ThumbPath;

                        //if (p.Value.bModified)
                        {
                            //modifiedFileRecordsData.Add(p.Value);
                        }
                        //else
                        {
                            newFileRecordsData.Add(p.Value);
                        }

                        if (nFilesProcessed % 125 == 0)
                        {
                            int percentage1 = (nFilesProcessed * 100) / nTotalFilesCount;
                            OnScanProgressProcess(p.Value.filePath, percentage1, nTotalFilesCount, nFilesProcessed, nTotalDupsFound);
                        }
                            //return;
                        });


                //if (newFileRecordsData.Count >= nBatchSize)
                //{
                //    /// Insert  in DB 
                //    /// 
                //    cGlobalSettings.sqliteLogger.InsertNewRecordsToDB(newFileRecordsData);

                //    newFileRecordsData.Clear();
                //}

                //if (modifiedFileRecordsData.Count >= nBatchSize)
                //{
                //    /// Update data in DB
                //    /// 
                //    //cGlobalSettings.sqliteLogger.InsertNewRecordsToDB(modifiedFileRecordsData);
                //    cGlobalSettings.sqliteLogger.UpdateRecordsToDB(modifiedFileRecordsData);

                //    modifiedFileRecordsData.Clear();
                //}


                // Show the data till the scanning is stopped
                cGlobalSettings.oManualReset.WaitOne();
                if (cGlobalSettings.abortNow)
                {
                    OnScanCompletedProcess("Scan aborted", "", true, 100);
                    //break;
                    //return true;
                }

                //if (RegistryVariables.SameContent)
                //{
                //    /// Group files on bases of MD5 and assign Group number
                //    var dupsGroups = group.GroupBy(p => p.Value.hash).Where(p => !string.IsNullOrEmpty(p.Key));

                //    string strCurrentFile = "";
                //    foreach (var dupsGroup in dupsGroups)
                //    {
                //        int nDupsCount = dupsGroup.Count();

                //        if (nDupsCount == 1)
                //            continue;

                //        GroupNumber += 1;
                //        nTotalDupsFound += nDupsCount;
                //        dupsGroup.All(p =>
                //        {
                //            p.Value.GroupNumber = GroupNumber;
                //            strCurrentFile = p.Value.FilePath;
                //            return true;
                //        });

                //        //dictDupsData.Add(dupsGroup.Key, dupsGroup.ToList<FileRecord>());
                //        cGlobalSettings.TotalDuplicateGroups += 1;
                //        cGlobal.TotalDulicateFilesCount += nDupsCount;
                //        cGlobal.TotalDuplicateFileSize += dupsGroup.Select(p => p.Value.FileSize).Sum();
                //    }
                //    if (nFilesProcessed % 125 == 0)
                //    {
                //        int percentage = (nFilesProcessed * 100) / nTotalFilesCount;
                //        OnScanProgressProcess(strCurrentFile, percentage, nTotalFilesCount, nFilesProcessed, nTotalDupsFound);
                //    }
                //}
                else
                {

                    string strCurrentFile = "";
                    int nDupsCount = group.Count();
                    GroupNumber += 1;
                    nTotalDupsFound += nDupsCount;

                    group.All(p =>
                    {
                        p.Value.groupNumber = GroupNumber;
                        strCurrentFile = p.Value.filePath;
                        return true;
                    });

                    cGlobalSettings.TotalDuplicateGroups += 1;
                    cGlobal.TotalDulicateFilesCount += nDupsCount;
                    cGlobal.TotalDuplicateFileSize += group.Select(p => p.Value.fileSize).Sum();

                    if (nFilesProcessed % 125 == 0)
                    {
                        int percentage = (nFilesProcessed * 100) / nTotalFilesCount;
                        OnScanProgressProcess(strCurrentFile, percentage, nTotalFilesCount, nFilesProcessed, nTotalDupsFound);
                    }
                }
            }

            if (newFileRecordsData.Count > 0)
            {
                /// Insert  in DB 
                /// 
                //  cGlobalSettings.sqliteLogger.InsertNewRecordsToDB(newFileRecordsData);

                newFileRecordsData.Clear();
            }

            if (modifiedFileRecordsData.Count > 0)
            {
                /// Update data in DB
                /// 
                //  cGlobalSettings.sqliteLogger.UpdateRecordsToDB(modifiedFileRecordsData);

                modifiedFileRecordsData.Clear();
            }

            //  cGlobalSettings.dictDuplicateData = dictDupsData;
            OnScanCompletedProcess("Scan completed", "", true, 100);
            return true;
        }
            //catch (Exception ex)
            //{
            //    cGlobalSettings.oLogger.WriteLogException("SystemScanWrapper:: ReadAllMessages: ", ex);
            //}

            //return true;

        



        public override bool Scan(List<csScanPaths> inputToScan, object filters, SearchOption searchOption, ref List<csImageFileInfo> list, Callback callback, int PauseCounter)
        {
            OnScanStartProcess("Gettings files, please wait", 5, 0);

            if (objSystemScanHelper == null)
                objSystemScanHelper = new SystemScanHelper();
             
            List<string> searchPatterns = filters as List<string>;

            /// Init list
            /// 
            cGlobalSettings.listImageFileInfo = new ConcurrentDictionary<int, csImageFileInfo>();
          //  cGlobalSettings.MediaFilePathToFileId = new ConcurrentDictionary<string, int>();

            /// Search for files and add in list
            bool bAddingFiles = objSystemScanHelper.StartScan(inputToScan, searchPatterns, searchOption, ref list, callback, PauseCounter, OnScanProgressProcess);

            /// Adding files complete.
            /// Group by files size and start computing there hash values.
            /// 
            if (list.Count <= 0)
                return true;

            ConcurrentDictionary<int, csImageFileInfo> dictRecords = new ConcurrentDictionary<int, csImageFileInfo>();

            int nKey = 0;
            foreach (csImageFileInfo record in list)
            {
                nKey += 1;
               // record.Key = nKey;
                dictRecords.TryAdd(nKey, record);
            }

           var groupSize = list.GroupBy(p => p.fileSize);

            Dictionary<string, List<csImageFileInfo>> dictDupsData = new Dictionary<string, List<csImageFileInfo>>();

            int nTotalFilesCount = list.Count;
            int GroupNumber = 0;
            int nFilesProcessed = 0;
            int nTotalDupsFound = 0;
            foreach (var group in groupSize)
            {
                int nItems = group.Count();

                if (nItems == 1)
                {
                    group.All(p =>
                    {
                        p.groupNumber = 0;
                        return true;
                    });

                    nFilesProcessed += 1;
                    continue;
                }

                int progressCounter = 0;
                group.All(p =>
                {
                    p.fileID = AppFunctions.GetMD5(p.filePath);
                    nFilesProcessed += 1;

                    progressCounter++;
                    if (progressCounter % 125 == 0)
                    {
                        int percentage1 = (nFilesProcessed * 100) / nTotalFilesCount;
                        OnScanProgressProcess(p.filePath, percentage1, nTotalFilesCount, nFilesProcessed, nTotalDupsFound);
                    }
                    return true;
                });

                var dupsGroups = group.GroupBy(p => p.fileID).Where(p => !string.IsNullOrEmpty(p.Key));

            string strCurrentFile = "";
            foreach (var dupsGroup in dupsGroups)
            {
                int nDupsCount = dupsGroup.Count();

                if (nDupsCount == 1)
                    continue;

                GroupNumber += 1;
                nTotalDupsFound += nDupsCount;
                dupsGroup.All(p =>
                {
                    p.groupNumber = GroupNumber;
                    strCurrentFile = p.filePath;
                    return true;
                });

                dictDupsData.Add(dupsGroup.Key, dupsGroup.ToList<csImageFileInfo>());
            }

            if (nFilesProcessed % 125 == 0)
            {
                int percentage = (nFilesProcessed * 100) / list.Count;
                OnScanProgressProcess(strCurrentFile, percentage, list.Count, nFilesProcessed, nTotalDupsFound);
            }
        }

            return true;

        }

    }
}







