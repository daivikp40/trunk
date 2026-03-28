using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using DuplicatePhotosFixer.ClassDictionary;
using System.Diagnostics;
using System.Threading;
//using System.Windows.Media
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;
using static DuplicatePhotosFixer.cClientEnum;
using DuplicatePhotosFixer.HelperClasses;
using DuplicatePhotosFixer.Helpers;

#region Help

/// Exif info helping source : 
/*-----------------------------------
 * 
https://msdn.microsoft.com/en-us/library/xddt0dz7.aspx
http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html
http://www.movable-type.co.uk/scripts/latlong.html
http://ptforum.photoolsweb.com/ubbthreads.php?ubb=download&Number=1024&filename=1024-2006_1011_093752.jpg

https://msdn.microsoft.com/en-us/library/ms534416(v=vs.85).aspx     //Property Item Descriptions
https://msdn.microsoft.com/en-us/library/ms534414(v=vs.85).aspx     //Image Property Tag Type Constants

 * http://weblogs.asp.net/bleroy/resizing-images-from-the-server-using-wpf-wic-instead-of-gdi
 * 
 * https://www.nuget.org/packages/ExifLib/1.6.2
 * http://www.codeproject.com/Articles/36342/ExifLib-A-Fast-Exif-Data-Extractor-for-NET-2-0
 * 
 * 
 * https://msdn.microsoft.com/en-us/library/ee719654(v=vs.85).aspx  //Extensions
 * ---------------------------------------*/

/*
         *  CGImageSourceRef src = CGImageSourceCreateWithURL((CFURLRef)[NSURL fileURLWithPath:imgPath], NULL);
    CFDictionaryRef options = (CFDictionaryRef)[[NSDictionary alloc] initWithObjectsAndKeys:
                                                (id)kCFBooleanTrue, (id)kCGImageSourceCreateThumbnailWithTransform,
                                                (id)kCFBooleanTrue, (id)kCGImageSourceCreateThumbnailFromImageIfAbsent,
                                                (id)[NSNumber numberWithDouble:size], (id)kCGImageSourceThumbnailMaxPixelSize,
                                                nil];
    CGImageRef thumbnail = CGImageSourceCreateThumbnailAtIndex(src, 0, options); // Create scaled image
    CFRelease(options);
    CFRelease(src);
    NSImage* img = [[NSImage alloc] initWithCGImage:thumbnail size:NSMakeSize(size, size)];
    [img autorelease];
    CGImageRelease(thumbnail);
    return img;
         * */
#endregion

namespace DuplicatePhotosFixer.Engine
{


    class ImageProcessor
    {

        public delegate void OnScanProgressProcess(cClientEnum.eScanPhase scanPhaseType, string message, int percentageCompleted, string counter, string subMessage);
        public event OnScanProgressProcess OnScanProgress;

        const float MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES = 0.85f;
        const float MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES_IN_EXACT_MATCH = 0.989f;
        const float MIN_VALUE_IF_NOT_MATCHED = 0.7f;
        uint totLength = (uint)(cGlobalSettings.ImageResize * cGlobalSettings.ImageResize);//reSizeVal here is governed by Preferences, default value = 64
        ulong maxScore = (ulong)((1.0f - MINIMUM_LEVEL_CONSIDERED_FOR_DUPLICATES/*0.85f*//*sliderInitialMatching.minValue*/) * (cGlobalSettings.ImageResize * cGlobalSettings.ImageResize));
        const int SQLITE_BATCH_SIZE = 1000;
        static bool bHelperDLLFound = false;
        //        public void ReadAllHashes()
        //        {
        //            CancellationTokenSource cts = null;
        //            int Counter = 0;
        //            int corruptImagesCount = 0;

        //            bool isExactMatch = !cGlobalSettings.isSimilarMatchChecked;

        //            try
        //            {
        //                bHelperDLLFound = false;
        //                string PathRawImageProcessFile = Path.Combine(Application.StartupPath, cGlobal.GetFileNameFromUrl(cGetString.GetMagickDwnUrl()));
        //                if (File.Exists(PathRawImageProcessFile))
        //                {
        //                    bHelperDLLFound = true;
        //                }

        //                cts = new CancellationTokenSource();
        //                var groups = cGlobalSettings.listImageFileInfo.Where(x =>
        //                    {
        //                        if (cGlobalSettings.CurrentScanMode == eScanMode.GoogleDrive ||
        //                        cGlobalSettings.CurrentScanMode == eScanMode.Dropbox || cGlobalSettings.CurrentScanMode == eScanMode.BoxCloud || cGlobalSettings.CurrentScanMode == eScanMode.AmazonS3)
        //                        {
        //                            if (x.Value.ThumbnailPath == null)
        //                                return false;
        //                        }

        //                        //bool returnValue = false;
        //                        if (x.Value.bHashFoundInDB)
        //                        {
        //                            Counter++;
        //                            return !x.Value.bHashFoundInDB;
        //                        }
        //                        if (isExactMatch)
        //                        {
        //                            if (x.Value.calcHashForExactMatch)
        //                                return true;
        //                            else
        //                                return false;
        //                        }
        //                        return true;
        //                    }).ToDictionary(x => x.Key, x => x.Value).Split(SQLITE_BATCH_SIZE);

        //                string strSearchingForduplicates = cResourceManager.LoadString("DPF_PROCESSOR_PROGRESS_SEARCHING_DUPLICATE_TEXT");
        //                //OnScanProgress(strSearchingForduplicates, 5, "", "");
        //                //OnScanProgress(cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_DUPLICATES"), 100 * i / TotalGroupsCount, string.Format(cResourceManager.LoadString("DPF_PROCESSOR_PROCESSED"), totalPhotosProcessed += x.Count()), cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));
        //                groups.ForEach(y =>
        //                    {
        //                        cGlobalSettings.oManualReset.WaitOne();
        //                        if (cGlobalSettings.abortNow)
        //                            return;
        //                        /// here we have processed the 1K bulk
        //                        //cGlobalSettings.listImageFileInfo/*.OrderByDescending(x => x.Value.filePath)*/
        //                        y.AsParallel()
        //                        .WithDegreeOfParallelism(AppFunctions.GetMaxDegreeOfParallelism())
        //                        .WithCancellation(cts.Token)
        //                        .ForAll(x =>
        //                        {
        //                            try
        //                            {
        //                                cGlobalSettings.oManualReset.WaitOne();
        //                                if (cGlobalSettings.abortNow)
        //                                    return;

        //                                Counter++;

        //                                if (Counter % 9 == 0)
        //                                {
        //                                    System.Threading.Thread.Sleep(0);
        //                                    int percentage = ((100 * Counter) / cGlobalSettings.listImageFileInfo.Count);

        //                                    OnScanProgress(cClientEnum.eScanPhase.Read, strSearchingForduplicates,
        //                                        percentage, Counter + "",
        //                                        cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));
        //                                }

        //                                /// if hash already found, just return
        //                                if (x.Value.bHashFoundInDB)
        //                                    return;

        //                                byte[] bools = null;
        //                                List<byte[]> arrOrientations = new List<byte[]>();
        //                                bools = GetImageHash(x.Key, x.Value.filePath, 4, 4, ref arrOrientations);

        //                                if (bools == null)
        //                                {
        //                                    corruptImagesCount++;
        //                                    return;
        //                                }
        //                                Func<byte[]> __getimagehash = () =>
        //                                {

        //                                    /// http://stackoverflow.com/questions/713057/convert-bool-to-byte-c-sharp
        //                                    // pack (in this case, using the first bool as the lsb - if you want
        //                                    // the first bool as the msb, reverse things ;-p)
        //                                    int bytes = (bools.Length + 7) / 8;
        //                                    byte[] arr = new byte[bytes];
        //                                    int bitIndex = 0, byteIndex = 0;
        //                                    for (int i = 0; i < bools.Length; i++)
        //                                    {
        //                                        if (bools[i] == 1)
        //                                        {
        //                                            //x.Value.hash[byteIndex] |= (byte)(((byte)1) << bitIndex);
        //                                            arr[byteIndex] |= (byte)(1 << (7 - bitIndex));
        //                                        }
        //                                        bitIndex++;
        //                                        if (bitIndex == 8)
        //                                        {
        //                                            bitIndex = 0;
        //                                            byteIndex++;
        //                                        }
        //                                    }

        //                                    return arr;
        //                                };

        //                                if (cGlobalSettings.listImageFileInfo[x.Key].byteHash == null)
        //                                {
        //                                    cGlobalSettings.listImageFileInfo[x.Key].byteHash = __getimagehash(); // arr;
        //#if ROTATE_FLIPPED_CHECK

        //                                    for (int i = 0; i < arrOrientations.Count; i++)
        //                                    {
        //                                        bools = arrOrientations[i];
        //                                        arrOrientations[i] = __getimagehash();
        //                                    }

        //                                    cGlobalSettings.listImageFileInfo[x.Key].byteHashArr = arrOrientations;
        //#endif
        //                                }
        //#if false
        //                        //try
        //                        //{
        //                        //    string basepath = @"C:\Users\sudhir.sharma\Desktop\hash check\hashes";
        //                        //    File.WriteAllLines(PathEx.Combine(basepath, Path.GetFileNameWithoutExtension(x.Value.filePath) + "_bits.txt"), bools.Select(z => z.ToString()).ToArray());
        //                        //    File.WriteAllLines(PathEx.Combine(basepath, Path.GetFileNameWithoutExtension(x.Value.filePath) + "_bytes.txt"), arr.Select(z => z.ToString()).ToArray());
        //                        //    File.WriteAllLines(PathEx.Combine(basepath, Path.GetFileNameWithoutExtension(x.Value.filePath) + "_long.txt"), x.Value.hash.Select(z => z.ToString()).ToArray());
        //                        //}
        //                        //catch (System.Exception ex)
        //                        //{

        //                        //}
        //#endif
        //                            }
        //                            catch (System.Exception ex)
        //                            {
        //                                cGlobalSettings.oLogger.WriteLogException("ReadAllHashes", ex);
        //                            }
        //                        });
        //#if SQLITE_NEEDED
        //                        //oSqlite.insertValuesToDB(y.Keys.ToList(), dbResult);


        //                        cGlobalSettings.oManualReset.WaitOne();

        //                        if (cGlobalSettings.abortNow)
        //                            return;
        //                        // Update Exif-Info info values and Delete Hash Table Values
        //                        UpdateRecordsToDB(y.Where(z => z.Value.bFileModified).Select(z => z.Key));

        //                        // Insert values
        //                        InsertNewRecordsToDB(y.Where(z => !z.Value.bHashFoundInDB).Select(z => z.Key));
        //#endif

        //                        cGlobalSettings.oManualReset.WaitOne();
        //                        if (cGlobalSettings.abortNow)
        //                            return;
        //                        /// insert to sq-lite
        //                        /// calculate long hash for these 1K items
        //                        ConvertByteArrayToLongArrayForBunch(y.Keys.ToList());
        //                    });
        //            }
        //            catch (Exception ex)
        //            {
        //                cGlobalSettings.oLogger.WriteLogException("ReadAllHashes", ex);
        //            }
        //            finally
        //            {
        //                try
        //                {
        //                    cGlobalSettings.oLogger.WriteLogVerbose("\tTotal corrupt images count:{0}", corruptImagesCount, false, true);

        //                    OnScanProgress(cClientEnum.eScanPhase.Read, cResourceManager.LoadString("DPF_PROCESSOR_PROGRESS_SEARCHING_DUPLICATE_TEXT"), 100, Counter + "", cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG"));
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    cGlobalSettings.oLogger.WriteLogException("ReadAllHashes-OnScanProgress", ex);
        //                }

        //                cts = null;
        //            }
        //        }

        //void UpdateRecordsToDB(IEnumerable<int> fileIdsToUpdateInDb)
        //{
        //    try
        //    {
        //        foreach (int i in fileIdsToUpdateInDb)
        //        {

        //            Double CaptureDateSeconds = 0;
        //            if (cGlobalSettings.listImageFileInfo[i].exifInfo.CaptureDate == DateTime.MinValue || cGlobalSettings.listImageFileInfo[i].exifInfo.CaptureDate == DateTime.MaxValue)
        //            {
        //                CaptureDateSeconds = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //            }
        //            else
        //                CaptureDateSeconds = cGlobalSettings.listImageFileInfo[i].exifInfo.CaptureDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        //            Double ModifiedDateSeconds = 0;
        //            if (cGlobalSettings.listImageFileInfo[i].modDate == DateTime.MinValue || cGlobalSettings.listImageFileInfo[i].modDate == DateTime.MaxValue)
        //            {
        //                ModifiedDateSeconds = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //            }
        //            else
        //                ModifiedDateSeconds = cGlobalSettings.listImageFileInfo[i].modDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        //            cGlobalSettings.sqliteLogger.odfExifInfo.updateData(SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString(),
        //                /*FieldName*/   new string[] { SqliteEnums.ExifInfoColumnsNames.CaptureDate.ToString(), SqliteEnums.ExifInfoColumnsNames.DpiHeight.ToString(), SqliteEnums.ExifInfoColumnsNames.DpiWidth.ToString(), SqliteEnums.ExifInfoColumnsNames.FileSize.ToString(), SqliteEnums.ExifInfoColumnsNames.GpsLatitude.ToString(), SqliteEnums.ExifInfoColumnsNames.GpsLongitude.ToString(), SqliteEnums.ExifInfoColumnsNames.modDate.ToString(), SqliteEnums.ExifInfoColumnsNames.Orientation.ToString(), SqliteEnums.ExifInfoColumnsNames.PixelHeight.ToString(), SqliteEnums.ExifInfoColumnsNames.PixelWidth.ToString() },
        //                new byte[] { 4, 4, 4, 4, 2, 2, 4, 1, 4, 4 },
        //                new object[]{
        //                    CaptureDateSeconds/*cGlobalSettings.listImageFileInfo[i].exifInfo.CaptureDate*/
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.VerticalResolution
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.HorizontalResolution
        //                    ,cGlobalSettings.listImageFileInfo[i].fileSize
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.latitudeDegree
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.longitudeDegree
        //                    ,ModifiedDateSeconds/*cGlobalSettings.listImageFileInfo[i].fileInfo.LastWriteTime*/
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.Orientation
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.ImageDimension.Height
        //                    ,cGlobalSettings.listImageFileInfo[i].exifInfo.ImageDimension.Width
        //                },
        //                string.Format(" {0}={1} ", SqliteEnums.ExifInfoColumnsNames.Id.ToString(), cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId),
        //                new string[] { SqliteEnums.ExifInfoColumnsNames.Id.ToString() },
        //                new byte[] { 4 },
        //                new object[] { cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId });


        //            // =============== Delete record from Hash table ===============
        //            string tableName = "";
        //            if (cGlobalSettings.isSimilarMatchChecked && Enum.IsDefined(typeof(SqliteEnums.MediaDatabaseLogType), cGlobalSettings.ImageResize))
        //            {
        //                tableName = ((SqliteEnums.MediaDatabaseLogType)cGlobalSettings.ImageResize).ToString();
        //            }
        //            else
        //            {
        //                tableName = SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString();
        //            }
        //            cGlobalSettings.sqliteLogger.odfExifInfo.deleteData(tableName, string.Format(" {0}={1} ", SqliteEnums.HashTableColumnsNames.FileExifID.ToString(), cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId), new string[] { SqliteEnums.HashTableColumnsNames.FileExifID.ToString() },
        //                new byte[] { 4 },
        //                new string[] { cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId.ToString() });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("ImageProcessor :: UpdateRecordsToDB : ", ex);
        //    }
        //}

        //void InsertNewRecordsToDB(IEnumerable<int> fileIdsNotFoundInDb)
        //{
        //    try
        //    {
        //        /// insert these to db
        //        cGlobalSettings.sqliteLogger.FillBlankDataSetForTableExifInfo();

        //        foreach (int i in fileIdsNotFoundInDb)
        //        {
        //            if (cGlobalSettings.listImageFileInfo[i].exifInfo != null && cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId < 0)
        //            {
        //                DataRow dr = cGlobalSettings.sqliteLogger.odfExifInfo.oDsUpdate.Tables[0].NewRow();
        //                ConvertImageFileInfoToDataRow(cGlobalSettings.listImageFileInfo[i], ref dr);
        //                cGlobalSettings.sqliteLogger.odfExifInfo.oDsUpdate.Tables[0].Rows.Add(dr);
        //            }
        //        }

        //        cGlobalSettings.sqliteLogger.odfExifInfo.UpdateViaDataSet("ReadAllHashes", true, true);

        //        cGlobalSettings.sqliteLogger.FillTempTableWithScannedPath(cGlobalSettings.listImageFileInfo.Select(x => x.Value.filePath.ToLower()));

        //        /// get all file ExifInfo ids
        //        cGlobalSettings.sqliteLogger.LoadSqliteToDictionary(true);

        //        cGlobalSettings.sqliteLogger.FillBlankDataSetForTableHash();
        //        foreach (int i in fileIdsNotFoundInDb)
        //        {
        //            if (cGlobalSettings.listImageFileInfo[i].exifInfo != null)
        //            {
        //                DataRow dr = cGlobalSettings.sqliteLogger.odfHash.oDsUpdate.Tables[0].NewRow();

        //                dr[SqliteEnums.HashTableColumnsNames.FileExifID.ToString()] = cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId;

        //                //if (cGlobalSettings.isSimilarMatchChecked)
        //                {
        //                    dr[SqliteEnums.HashTableColumnsNames.FileHash.ToString()] = cGlobalSettings.listImageFileInfo[i].byteHashArr.SelectMany(a => a).ToArray();
        //                }
        //                /*else
        //                {
        //                    dr[SqliteEnums.HashTableColumnsNames.FileHash.ToString()] = Encoding.UTF8.GetBytes(cGlobalSettings.listImageFileInfo[i].md5);
        //                }*/
        //                cGlobalSettings.sqliteLogger.odfHash.oDsUpdate.Tables[0].Rows.Add(dr);
        //            }
        //        }

        //        cGlobalSettings.sqliteLogger.odfHash.UpdateViaDataSet("ReadAllHashes", true, true);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("InsertNewRecordsToDB ::", ex);
        //    }
        //}

        //void ConvertImageFileInfoToDataRow(csImageFileInfo imageInfo, ref DataRow dr)
        //{
        //    try
        //    {
        //        dr[SqliteEnums.ExifInfoColumnsNames.FilePath.ToString()] = imageInfo.filePath.ToLower();

        //        if (imageInfo.exifInfo == null)
        //        {
        //            return;
        //        }

        //        Double totalCaptureDateSeconds = 0;
        //        if (imageInfo.exifInfo.CaptureDate == DateTime.MinValue || imageInfo.exifInfo.CaptureDate == DateTime.MaxValue)
        //        {
        //            totalCaptureDateSeconds = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //        }
        //        else
        //            totalCaptureDateSeconds = imageInfo.exifInfo.CaptureDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        //        Double totalModifiedDateSeconds = 0;
        //        if (imageInfo.modDate == DateTime.MinValue || imageInfo.modDate == DateTime.MaxValue)
        //        {
        //            totalModifiedDateSeconds = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //        }
        //        else
        //            totalModifiedDateSeconds = imageInfo.modDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        //        dr[SqliteEnums.ExifInfoColumnsNames.CaptureDate.ToString()] = totalCaptureDateSeconds;
        //        dr[SqliteEnums.ExifInfoColumnsNames.modDate.ToString()] = totalModifiedDateSeconds;
        //        dr[SqliteEnums.ExifInfoColumnsNames.GpsLatitude.ToString()] = imageInfo.exifInfo.latitudeDegree;
        //        dr[SqliteEnums.ExifInfoColumnsNames.GpsLongitude.ToString()] = imageInfo.exifInfo.longitudeDegree;
        //        dr[SqliteEnums.ExifInfoColumnsNames.PixelWidth.ToString()] = imageInfo.exifInfo.ImageDimension.Width;
        //        dr[SqliteEnums.ExifInfoColumnsNames.PixelHeight.ToString()] = imageInfo.exifInfo.ImageDimension.Height;
        //        dr[SqliteEnums.ExifInfoColumnsNames.DpiWidth.ToString()] = imageInfo.exifInfo.HorizontalResolution;
        //        dr[SqliteEnums.ExifInfoColumnsNames.DpiHeight.ToString()] = imageInfo.exifInfo.VerticalResolution;
        //        dr[SqliteEnums.ExifInfoColumnsNames.Orientation.ToString()] = imageInfo.exifInfo.Orientation;
        //        dr[SqliteEnums.ExifInfoColumnsNames.FileSize.ToString()] = imageInfo.fileSize;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("ConvertImageFileInfoToDataRow", ex);
        //    }
        //}


        /// <summary>
        /// convert byte array to long array for comparison
        /// after sqlite db insertion
        /// </summary>
        /// <param name="bunchOfDict"></param>
        /// 

        public static void SaveImage(Image img, string filePath /*, string identifier*/)
        {
            try
            {
                string strPath = filePath; // GetThumbSavePath(filePath, identifier);

                if (File.Exists(strPath))
                    File.Delete(strPath);
                if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(strPath));

                img.Save(strPath, ImageFormat.Jpeg);
            }
            catch (System.Exception) { }
        }

        public void compareForExactPhotos()
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

        public static void ConvertByteArrayToLongArrayForBunch(IEnumerable<int> bunchOfDict)
        {
            CancellationTokenSource cts = null;
            try
            {
                cts = new CancellationTokenSource();
                bunchOfDict
                    .AsParallel()
                    .WithDegreeOfParallelism(1)
                    .WithCancellation(cts.Token)
                    .ForAll(x =>
                    {
                        cGlobalSettings.oManualReset.WaitOne();
                        if (cGlobalSettings.abortNow)
                            return;

                        if (cGlobalSettings.listImageFileInfo[x].byteHash == null)
                            return;

                        byte[] arr = cGlobalSettings.listImageFileInfo[x].byteHash;

                        ConvertByteArrayToLongArray(x, ref arr);
#if ROTATE_FLIPPED_CHECK

                        if (cGlobalSettings.listImageFileInfo[x].byteHashArr!=null && cGlobalSettings.listImageFileInfo[x].byteHashArr.Count>0)
                        {
                            for(int i=0; i< cGlobalSettings.listImageFileInfo[x].byteHashArr.Count; i++)
                            {
                                ConvertByteArrayToLongArray(x, i, cGlobalSettings.listImageFileInfo[x].byteHashArr[i]);
                            }
                        }
#endif

                    });
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ImageProcessor :: startCalcLongHash : ", ex);
            }
        }


        public static void ConvertByteArrayToLongArray(int index, ref byte[] arr)
        {
            try
            {
                //const int BytesInInt32 = 4;
                const int BytesInInt64 = 8;
                int length1 = (arr.Length + 7) / BytesInInt64;
                cGlobalSettings.listImageFileInfo[index].hash = new ulong[length1];

                for (int i = 0, pos = 0; i < length1; i++, pos += BytesInInt64)
                {
                    cGlobalSettings.listImageFileInfo[index].hash[i] = BitConverter.ToUInt64(arr, pos);
                }

                cGlobalSettings.listImageFileInfo[index].byteHash = null;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("Converting Byte to Int32/64", ex);
            }
        }
        // ROTATE_FLIPPED_CHECK

        public static void ConvertByteArrayToLongArray(int index, int arrIndex, byte[] arr)
        {
            try
            {
                //const int BytesInInt32 = 4;
                const int BytesInInt64 = 8;
                int length1 = (arr.Length + 7) / BytesInInt64;
                if (cGlobalSettings.listImageFileInfo[index].arrHash == null)
                    cGlobalSettings.listImageFileInfo[index].arrHash = new List<ulong[]>();
                cGlobalSettings.listImageFileInfo[index].arrHash.Add(new ulong[length1]);

                for (int i = 0, pos = 0; i < length1; i++, pos += BytesInInt64)
                {
                    cGlobalSettings.listImageFileInfo[index].arrHash[arrIndex][i] = BitConverter.ToUInt64(arr, pos);
                }

                if (cGlobalSettings.listImageFileInfo[index].byteHashArr != null && cGlobalSettings.listImageFileInfo[index].byteHashArr.Count <= arrIndex)
                    cGlobalSettings.listImageFileInfo[index].byteHashArr[arrIndex] = null;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("Converting Byte to Int32/64", ex);
            }
        }

        public static byte[] GetImageHash(int keyNode, string originalImage, int rows, int columns, ref List<byte[]> arrOrientations)
        {
            byte[] bools = null;
            //List<byte[]> boolsArr = null;
            try
            {
                arrOrientations = new List<byte[]>();
                if (cGlobalSettings.CurrentScanMode == eScanMode.GoogleDrive ||
                    cGlobalSettings.CurrentScanMode == eScanMode.BoxCloud ||
                    cGlobalSettings.CurrentScanMode == eScanMode.Dropbox || cGlobalSettings.CurrentScanMode == eScanMode.AmazonS3)
                {
                    originalImage = App.oMainReference.GetSaveThumbnailImage(cGlobalSettings.listImageFileInfo[keyNode]);
                    bools = _GetImageHash(keyNode, originalImage, rows, columns, false, ref arrOrientations);
                    return bools;
                }

                /// If file is of RAW format then first try to convert the image and get hash value
                //if (!cGlobalSettings.listImageFileInfo[keyNode].bRAWFormat)
                bools = _GetImageHash(keyNode, originalImage, rows, columns, false, ref arrOrientations);

                if (bools == null && bHelperDLLFound &&
                    (cGlobalSettings.CurrentScanMode == eScanMode.FileSearch || cGlobalSettings.CurrentScanMode == eScanMode.PicasaLibrary || cGlobalSettings.CurrentScanMode == eScanMode.Lightroom))
                {
                    Thread.Sleep(0);
                    // try with new library
                    string strThumbPath = "";
                    string originalImageConverted = csImageMagick.ConvertRAWToJPG(originalImage, keyNode, ref strThumbPath);
                    if (!File.Exists(originalImageConverted))
                    {
                        /// Not able to convert image via ImageMagick, so return
                        return bools;
                    }
                    cGlobalSettings.listImageFileInfo[keyNode].bRAWFormat = true;
                    cGlobalSettings.listImageFileInfo[keyNode].ThumbnailPath = strThumbPath;
                    bools = _GetImageHash(keyNode, originalImageConverted, rows, columns, true, ref arrOrientations);

                    if (!(string.Compare(originalImage, originalImageConverted) == 0))
                    {
                        // Delete converted image and save only thumbnail image in app data folder
                        File.Delete(originalImageConverted);
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetImageHashExtended:: ", ex);
            }
            return bools;
        }


        static byte[] _GetImageHash(
    int keyNode,
    string originalImage,
    int rows,
    int columns,
    bool isConvertedImage,
    ref List<byte[]> arrOrientations)
        {
            FileStream stream = null;
            Image image = null;
            Bitmap reSizedImage = null;
            List<int> arrColors = null;
            int[] means = null;
            byte[] hash = null;

            const UseEmbeddedThumbnails useEmbeddedThumbnails = UseEmbeddedThumbnails.Never;
            const bool useExifOrientation = true, useWIC = true;

            try
            {
                // Validate global state
                if (cGlobalSettings.listImageFileInfo == null ||
                    keyNode < 0 ||
                    keyNode >= cGlobalSettings.listImageFileInfo.Count ||
                    cGlobalSettings.listImageFileInfo[keyNode] == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(keyNode), $"Invalid keyNode {keyNode} for listImageFileInfo.");
                }

                if (string.IsNullOrEmpty(originalImage) || !File.Exists(originalImage))
                {
                    throw new FileNotFoundException($"Image file not found: {originalImage}");
                }

                using (stream = File.OpenRead(originalImage))
                using (image = Image.FromStream(stream, false, false))
                {
                    if (image == null)
                        throw new Exception("Failed to load image from stream.");

                    // Thumbnail size
                    Size imgSize = ImageFunctions.GetThumbnailSize(image, cGlobalSettings.ImageResize);

                    // Extract thumbnail safely
                    reSizedImage = ThumbnailExtractor.FromFile(
                        originalImage,
                        new Size(cGlobalSettings.ImageResize, cGlobalSettings.ImageResize),
                        useEmbeddedThumbnails,
                        useExifOrientation,
                        useWIC) as Bitmap;

                    if (reSizedImage == null)
                        throw new Exception("ThumbnailExtractor.FromFile returned null.");

                    reSizedImage = ImageFunctions.GetThumbnailImageHighQuality(reSizedImage, imgSize.Width, imgSize.Height);
                    if (reSizedImage == null)
                        throw new Exception("Failed to create high-quality thumbnail.");

                    reSizedImage = ImageFunctions.MakeGrayscale3(reSizedImage);
                    if (reSizedImage == null)
                        throw new Exception("Failed to grayscale thumbnail.");

                    reSizedImage = ImageFunctions.GetThumbnailImageHighQuality(reSizedImage, cGlobalSettings.ImageResize, cGlobalSettings.ImageResize);
                    if (reSizedImage == null)
                        throw new Exception("Failed to resize grayscale image.");

                    // Handle thumbnail saving
                    string thumbnailPath;
                    Size imgSizeThumb = ImageFunctions.GetThumbnailSize(image, 256);

                    if (isConvertedImage)
                    {
                        // Ensure path exists
                        thumbnailPath = cGlobalSettings.listImageFileInfo[keyNode].ThumbnailPath;
                        if (string.IsNullOrEmpty(thumbnailPath))
                        {
                            thumbnailPath = Path.Combine(
                                cGlobalSettings.strThumbSaveFolderPathTemp,
                                $"{AppFunctions.CreateMD5Hash(originalImage)}.jpg"
                            );
                        }
                    }
                    else
                    {
                        thumbnailPath = Path.Combine(
                            cGlobalSettings.strThumbSaveFolderPathTemp,
                            $"{AppFunctions.CreateMD5Hash(originalImage)}.jpg"
                        );

                        SaveImage(
                            ImageFunctions.GetThumbnailImageHighQuality((Bitmap)image, imgSizeThumb.Width, imgSizeThumb.Height),
                            thumbnailPath
                        );
                    }

                    // Ensure exifInfo exists
                    var info = cGlobalSettings.listImageFileInfo[keyNode];
                    if (info.exifInfo == null)
                    {
                        info.exifInfo = new csExifInfo();
                    }
                    info.Height = info.exifInfo.ImageDimension.Height;
                    info.Width = info.exifInfo.ImageDimension.Width;

                    // Hash generator
                    Func<byte[]> __getimagehash = () =>
                    {
                        int m_width = cGlobalSettings.ImageResize;
                        int m_height = cGlobalSettings.ImageResize;
                        arrColors = new List<int>();
                        means = new int[rows * columns];
                        int indexMean = 0;
                        int pHeight = m_height / rows;
                        int pWidth = m_width / columns;

                        // Partition and compute means
                        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < columns; colIndex++)
                            {
                                int countTotal = 0, indexCount = 0;
                                int stHeight = (pHeight * rowIndex);
                                int enHeight = (pHeight * (rowIndex + 1)) - 1;
                                int stWidth = (pWidth * colIndex);
                                int enWidth = (pWidth * (colIndex + 1)) - 1;

                                for (int y = stHeight; y <= enHeight; y++)
                                {
                                    for (int x = stWidth; x <= enWidth; x++)
                                    {
                                        indexCount++;
                                        Color originalColor = reSizedImage.GetPixel(x, y);
                                        int greyScaleColorAverage = (originalColor.R + originalColor.G + originalColor.B) / 3;
                                        arrColors.Add(greyScaleColorAverage);
                                        countTotal += greyScaleColorAverage;
                                    }
                                }
                                int mean = countTotal / indexCount;
                                means[indexMean++] = mean;
                            }
                        }

                        hash = new byte[cGlobalSettings.ImageResize * cGlobalSettings.ImageResize];
                        int index = 0;
                        indexMean = 0;

                        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < columns; colIndex++)
                            {
                                int stHeight = (pHeight * rowIndex);
                                int enHeight = (pHeight * (rowIndex + 1)) - 1;
                                int stWidth = (pWidth * colIndex);
                                int enWidth = (pWidth * (colIndex + 1)) - 1;

                                int mean = means[indexMean++];
                                for (int y = stHeight; y <= enHeight; y++)
                                {
                                    for (int x = stWidth; x <= enWidth; x++)
                                    {
                                        int val = arrColors[index];
                                        hash[index++] = (val > mean) ? (byte)1 : (byte)0;
                                    }
                                }
                            }
                        }
                        return hash;
                    };

                    // Calculate orientation hash
                    arrOrientations.Add(__getimagehash());
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException($"GetImageHash failed for: {originalImage}", ex);
                return null;
            }
            finally
            {
                stream?.Dispose();
                image?.Dispose();
                reSizedImage?.Dispose();
                arrColors?.Clear();
                means = null;
            }

            return hash;
        }


    }
}


