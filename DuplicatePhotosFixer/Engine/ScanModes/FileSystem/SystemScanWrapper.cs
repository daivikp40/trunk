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
using System.Drawing;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.Engine.ScanModes.FileSystem
{

    class SystemScanWrapper : IScannerWrapper
    {
        SystemScanHelper objSystemScanHelper = null;
        string lastFolderPath = string.Empty;

        const float MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES = 0.85f;
        const float MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES_IN_EXACT_MATCH = 0.989f;
        const float MIN_VALUE_IF_NOT_MATCHED = 0.7f;
        const int PartsSize = 4;
        const int SQLITE_BATCH_SIZE = 1000;
        const int nBatchSize = 1000;

        uint totLength = (uint)(cGlobalSettings.ImageResize * cGlobalSettings.ImageResize);
        ulong maxScore = (ulong)((1.0f - MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES) * (cGlobalSettings.ImageResize * cGlobalSettings.ImageResize));

        static bool bHelperDLLFound = false;

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

        public static string GetThumbnailImage(string SourcePath, bool bCreateThumbIfNotFound = false)
        {
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

        static System.Drawing.Size GetThumbnalSize(int actualWidth, int actualHeight, int BitMapSize)
        {
            System.Drawing.Size size = new System.Drawing.Size();
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
                    System.Drawing.Size thumbSize = GetThumbnalSize((int)imm.Width, (int)imm.Height, ThumbSize);
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

        public void InitAll()
        {
            try
            {
                cGlobal.TotalDulicateFilesCount = 0;
                cGlobal.TotalDuplicateFileSize = 0;

                /// Init list
                cGlobalSettings.listImageFileInfo = new ConcurrentDictionary<int, csImageFileInfo>();
                cGlobalSettings.MediaFilePathToFileId = new ConcurrentDictionary<string, int>();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SystemScanWrapper:: InitAll: ", ex);
            }
        }

        public override bool Scan(List<csScanPaths> inputToScan, object filters, SearchOption searchOption, ref HashSet<csImageFileInfo> list, Callback callback, int PauseCounter)
        {
            cGlobalSettings.abortNow = false;
            try
            {
                InitAll();

                OnScanStartProcess("Gettings files, please wait", 5, 0);

                if (objSystemScanHelper == null)
                    objSystemScanHelper = new SystemScanHelper();

                List<string> searchPatterns = filters as List<string>;

                /// Search for files and add in list
                bool bAddingFiles = objSystemScanHelper.StartScan(inputToScan, searchPatterns, searchOption, ref list, callback, PauseCounter, OnScanProgressProcess);

                /// Adding files complete.
                /// Group by files size and start computing there hash values.
                /// 
                if (list.Count <= 0)
                    return true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SystemScanWrapper:: Scan: ", ex);
            }
            return true;
        }

        public override bool ReadAllMessages(
            object client,
            object folder,
            ref HashSet<csImageFileInfo> imageRecordsData,
            Callback callback,
            int PauseCounter,
            string directoryPath)
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = AppFunctions.GetMaxDegreeOfParallelism(),
            };

            CancellationTokenSource cts = null;

            try
            {
                bool isExactMatch = !cGlobalSettings.isSimilarMatchChecked;

#if !SQLITE_NEEDED
                if (cGlobalSettings.sqliteLogger == null)
                {
                    cGlobalSettings.sqliteLogger = new cSqliteLogger();
                    cGlobalSettings.sqliteLogger.InitDB();
                }

                if (eFuncRetVal.SUCCESS != cGlobalSettings.sqliteLogger.FillTempTableWithScannedPath(cGlobalSettings.listImageFileInfo.Select(p => p.Value.filePath)))
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Not able to insert temp data in a table.");
                    return false;
                }
                cGlobalSettings.sqliteLogger.LoadSqliteToDictionary(false);
#endif

                var imagesToProcess = cGlobalSettings.listImageFileInfo.Where(x =>
                {
                    if (x.Value.bHashFoundInDB)
                        return false;

                    if (isExactMatch)
                    {
                        if (x.Value.calcHashForExactMatch)
                            return true;
                        else
                            return false;
                    }
                    return true;
                }).ToDictionary(x => x.Key, x => x.Value);


                int nTotalFilesCount = imagesToProcess.Count;
                int nFilesProcessed = 0;
                int nCorruptImagesCount = 0;
                int GroupNumber = 0;
                int nTotalDupsFound = 0;

                List<csImageFileInfo> newHashRecordsData = new List<csImageFileInfo>();
                List<csImageFileInfo> modifiedHashRecordsData = new List<csImageFileInfo>();
                List<csImageFileInfo> duplicateHashRecordsData = new List<csImageFileInfo>();

                string strSearchingForDuplicates = cResourceManager.LoadString("DPF_PROCESSOR_PROGRESS_SEARCHING_DUPLICATE_TEXT");

                var groupedBatches = imagesToProcess.Split(SQLITE_BATCH_SIZE);
                cts = new CancellationTokenSource();

                // PHASE 2: Calculate hashes for all images (5-50%)
                foreach (var batch in groupedBatches)
                {
                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                    {
                        OnScanCompletedProcess("Scan aborted.", "", true, 100);
                        return true;
                    }


                    // Process batch in parallel
                    Parallel.ForEach(batch, parallelOptions, x =>
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                        {
                            OnScanCompletedProcess("Scan aborted", "", true, 100);
                            return;
                        }

                        Interlocked.Increment(ref nFilesProcessed);

                        // Hash already calculated
                        if (x.Value.bHashFoundInDB && !x.Value.bFileModified)
                            return;

                        try
                        {
                            // Validate key before calling GetImageHash
                            if (x.Key < 0)
                            {
                                cGlobalSettings.oLogger?.WriteLogVerbose($"Skipping image with invalid key {x.Key}: {x.Value.filePath}");
                                return;
                            }

                            byte[] bools = null;
                                List<byte[]> arrOrientations = new List<byte[]>();
                                bools = ImageProcessor.GetImageHash(x.Key, x.Value.filePath, PartsSize, PartsSize, ref arrOrientations);

                                if (bools == null)
                                {
                                    Interlocked.Increment(ref nCorruptImagesCount);
                                    cGlobalSettings.oLogger?.WriteLogVerbose($"Corrupt image detected: {x.Value.filePath}");
                                    return;
                                }

                                Func<byte[]> __getimagehash = () =>
                                {
                                    int bytes = (bools.Length + 7) / 8;
                                    byte[] arr = new byte[bytes];
                                    int bitIndex = 0, byteIndex = 0;
                                    for (int i = 0; i < bools.Length; i++)
                                    {
                                        if (bools[i] == 1)
                                        {
                                            arr[byteIndex] |= (byte)(1 << (7 - bitIndex));
                                        }
                                        bitIndex++;
                                        if (bitIndex == 8)
                                        {
                                            bitIndex = 0;
                                            byteIndex++;
                                        }
                                    }
                                    return arr;
                                };

                                if (cGlobalSettings.listImageFileInfo[x.Key].byteHash == null)
                                {
                                    cGlobalSettings.listImageFileInfo[x.Key].byteHash = __getimagehash();
                                    cGlobalSettings.oLogger?.WriteLogVerbose($"Generated byteHash for key {x.Key}: {x.Value.filePath}");

#if ROTATE_FLIPPED_CHECK
                                    for (int i = 0; i < arrOrientations.Count; i++)
                                    {
                                        bools = arrOrientations[i];
                                        arrOrientations[i] = __getimagehash();
                                    }
                                    cGlobalSettings.listImageFileInfo[x.Key].byteHashArr = arrOrientations;
#endif
                                }
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger?.WriteLogException($"ReadAllMessages - Hash Calculation for {x.Value.filePath}", ex);
                            }

                            // MODIFIED: Progress reporting for Phase 2 (5-50%)
                            if (nFilesProcessed % 125 == 0)
                            {
                                // Calculate percentage from 5% to 50% (45% range)
                                int percentage = 5 + ((nFilesProcessed * 45) / nTotalFilesCount);
                                percentage = Math.Min(50, percentage);  // Cap at 50%

                                cGlobalSettings.oLogger?.WriteLogVerbose($"[Callback] Calling OnScanProgressProcess - Phase: Read, Percentage: {percentage}");
                                OnScanProgressProcess(
                                    cClientEnum.eScanPhase.Read,
                                    x.Value.filePath,
                                    percentage,
                                    nTotalFilesCount,
                                    nFilesProcessed,
                                    0,  // No duplicates found yet
                                    cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));
                            }
                        });

                    if (newHashRecordsData.Count >= nBatchSize)
                    {
                      cGlobalSettings.sqliteLogger.InsertNewRecordsToDB(newHashRecordsData);
                        newHashRecordsData.Clear();
                    }

                    if (modifiedHashRecordsData.Count >= nBatchSize)
                    {
                      cGlobalSettings.sqliteLogger.UpdateRecordsToDB(modifiedHashRecordsData);
                        modifiedHashRecordsData.Clear();
                    }

                    cGlobalSettings.oManualReset.WaitOne();
                    if (cGlobalSettings.abortNow)
                    {
                        OnScanCompletedProcess("Scan aborted", "", true, 100);
                        return true;
                    }

                    var validKeys = batch.Where(x => x.Key >= 0 && x.Value.byteHash != null).Select(x => x.Key);

                    if (validKeys != null)
                    {
                        ImageProcessor.ConvertByteArrayToLongArrayForBunch(validKeys);
                    }

                    int hashesConverted = batch.Count(x => x.Value.hash != null);
                    cGlobalSettings.oLogger?.WriteLogVerbose($"After conversion: {hashesConverted} images have long hash arrays");
                }

                if (newHashRecordsData.Count > 0)
                {
               cGlobalSettings.sqliteLogger.InsertNewRecordsToDB(newHashRecordsData);
                  newHashRecordsData.Clear();
                }

                if (modifiedHashRecordsData.Count > 0)
                {
                   cGlobalSettings.sqliteLogger.UpdateRecordsToDB(modifiedHashRecordsData);
                    modifiedHashRecordsData.Clear();
                }

                // ADDED: Mark Phase 2 complete at 50%
                OnScanProgressProcess(
                    cClientEnum.eScanPhase.Read,
                    "Hash calculation completed",
                    50,  // Phase 2 ends at 50%
                    nTotalFilesCount,
                    nTotalFilesCount,
                    0,
                    "");

                cGlobalSettings.oLogger?.WriteLogVerbose("Phase 2 (Read) completed at 50%");

                cGlobalSettings.oManualReset.WaitOne();
                if (cGlobalSettings.abortNow)
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("\tScanning aborted");
                    return false;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ReadAllMessages: ", ex);
            }

            return true;
        }

        public override bool ProcessMessage(object client, object folder, object mail, ref object record, Callback callback, int PauseCounter, string directoryPath)
        {
            bool bSuccess = false;
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("[ProcessMessage] Starting comparison phase");

                if (cGlobalSettings.objMainScanHelper != null)
                {
                    switch (cGlobalSettings.MatchSettingsData.MatchMethod)
                    {
                        case enumClass.eMatchMethod.Exact:
                            {
                                cGlobalSettings.oLogger?.WriteLogVerbose("[ProcessMessage] Using Exact Match mode");
                                cGlobalSettings.objMainScanHelper.compareForExactPhotos(OnScanProgressProcess);

                                // After exact match, still need to compare hashes
                                cGlobalSettings.objMainScanHelper.compareForSimilarPhotos(OnScanProgressProcess);
                            }
                            break;

                        case enumClass.eMatchMethod.Similar:
                            {
                                cGlobalSettings.oLogger?.WriteLogVerbose("[ProcessMessage] Using Similar Match mode");
                                cGlobalSettings.objMainScanHelper.compareForSimilarPhotos(OnScanProgressProcess);
                            }
                            break;
                    }

                    // Run diagnostics
                    cGlobalSettings.objMainScanHelper.DiagnosticCheckDuplicates();

                    OnScanCompletedProcess("", "", true, -1);
                    bSuccess = true;
                }
                else
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("[ProcessMessage] ERROR: objMainScanHelper is null!");
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("ProcessMessage", ex);
            }
            return bSuccess;
        }


        //public override bool ProcessMessage(object client, object folder, object mail, ref object record, Callback callback, int PauseCounter, string directoryPath)
        //{
        //    bool bSuccess = false;
        //    try
        //    {
        //        // Make sure this only runs ONCE
        //        if (cGlobalSettings.objMainScanHelper != null)
        //        {
        //            switch (cGlobalSettings.MatchSettingsData.MatchMethod)
        //            {
        //                case enumClass.eMatchMethod.Exact:
        //                    {
        //                        cGlobalSettings.objMainScanHelper.compareForExactPhotos(OnScanProgressProcess);

        //                    }
        //                    break;
        //                case enumClass.eMatchMethod.Similar:
        //                    {
        //                        // This should only be called ONCE
        //                        cGlobalSettings.objMainScanHelper.compareForSimilarPhotos(OnScanProgressProcess);
        //                    }
        //                    break;
        //            }

        //            OnScanCompletedProcess("", "", true, -1);  // Signal completion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger?.WriteLogException("ProcessMessage", ex);
        //    }
        //    return bSuccess;
        //}

    }
}