using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine;

namespace DuplicatePhotosFixer.Sqlite
{
    
        /*
         * Tables to create
         * ExifInfo
         * HashTable
         * TempTable
         * 
         * Results
         * 
         * 
         * */

        public class SqliteEnums
        {
            public enum MediaDatabaseLogType
            {
                TempTable = 9,
                ExifInfo = 1,
                //HashTable = 2,
                HashTable_48 = 48,
                HashTable_56 = 56,
                HashTable_64 = 64,
                HashTable_72 = 72,
                HashTable_80 = 80,
                HashTable_88 = 88,
                HashTable_96 = 96,
                DupImageFileLog = 4,
                UpdateTable = 5,
            }


            public enum DupImageFileLogColumnsName
            {
                [Description("INTEGER NOT NULL UNIQUE")]//4
                Key,
                [Description("INTEGER NOT NULL UNIQUE")]//4
                ExifId,
                [Description("INTEGER")]//4
                GroupNumber,
                [Description("INTEGER")]//4
                IsMark,
                [Description("INTEGER")]//4
                IsLocked,
                [Description("INTEGER")]//4
                Match,
                /*[Description("INTEGER")]//4
                IsProtected,*/
                [Description("TEXT")]
                FolderPath,
            }

            // public enum DataType { String = 1, Number = 2, DateTime = 3, Long = 4, Binary = 5, Real = 6 };
            public enum ExifInfoColumnsNames
            {
                [Description("INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE")]//4
                Id,
                [Description("TEXT NOT NULL UNIQUE")]//1
                FilePath,
                [Description("INTEGER")]//4
                CaptureDate,
                [Description("INTEGER")]//4
                modDate,
                [Description("INTEGER")]//4
                FileSize,
                [Description("NUMERIC")]//2
                GpsLatitude,
                [Description("NUMERIC")]//2
                GpsLongitude,
                [Description("INTEGER")]//4
                PixelWidth,
                [Description("INTEGER")]//4
                PixelHeight,
                [Description("INTEGER")]//4
                DpiWidth,
                [Description("INTEGER")]//4
                DpiHeight,
                [Description("TEXT")]//1
                Orientation
            }

            public enum HashTableColumnsNames
            {
                [Description("INTEGER NOT NULL UNIQUE")]
                FileExifID,
                [Description("BLOB")]
                FileHash
            }

            public enum TempTableColumnsNames
            {
                [Description("TEXT NOT NULL UNIQUE")]
                FilePath
            }

            public enum UpdateTableColumnsNames
            {
                [Description("TEXT NOT NULL UNIQUE")]
                Key,
                [Description("INTEGER")]
                GroupNumber,
                [Description("INTEGER")]
                IsMark

            }
        }

        public partial class cSqliteLogger
        {


            static IntPtr WaitHandleFileProcessor = IntPtr.Zero;

            /// <summary>
            /// Sqlite writer
            /// </summary>
            public cSQliteFunction odfExifInfo = null;
            public cSQliteFunction odfHash = null;
            public cSQliteFunction odfTemp = null;
            public cSQliteFunction odfImageFileLog = null;
            public cSQliteFunction odfUpdateTable = null;

            public eErrorCodes InitDB()
            {

                try
                {
                    //LockAudioFilesDB.EnterWriteLock();

                    try
                    {
                        // delete files if want
                    }
                    catch (System.Exception ex)
                    {
                        cGlobalSettings.oLogger.WriteLogException("AudioFingerprintIndexer::InitDB::FileDelete", ex);
                    }

                    odfExifInfo = new cSQliteFunction("odfExifInfo", 45 + cGlobalSettings.GlobalLockIdentifier);
                    odfExifInfo.CreateConnection(cGlobalSettings.GetPhotosDBPath());

                    odfHash = new cSQliteFunction("odfExifInfo", 45 + cGlobalSettings.GlobalLockIdentifier);
                    odfHash.CreateConnection(cGlobalSettings.GetPhotosDBPath());

                    odfTemp = new cSQliteFunction("odfExifInfo", 45 + cGlobalSettings.GlobalLockIdentifier);
                    odfTemp.CreateConnection(cGlobalSettings.GetPhotosDBPath());

                    odfImageFileLog = new cSQliteFunction("odfImageFileLog", 45 + cGlobalSettings.GlobalLockIdentifier);
                    odfImageFileLog.CreateConnection(cGlobalSettings.GetPhotosDBPath());

                    odfUpdateTable = new cSQliteFunction("odfUpdateTable", 45 + cGlobalSettings.GlobalLockIdentifier);
                    odfUpdateTable.CreateConnection(cGlobalSettings.GetPhotosDBPath());


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.ExifInfoColumnsNames)).Cast<SqliteEnums.ExifInfoColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString()));

                    /*odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable.ToString()));*/

                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_48.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_56.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_64.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_72.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_88.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.HashTableColumnsNames)).Cast<SqliteEnums.HashTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.HashTable_96.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.TempTableColumnsNames)).Cast<SqliteEnums.TempTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.TempTable.ToString()));


                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.DupImageFileLogColumnsName)).Cast<SqliteEnums.DupImageFileLogColumnsName>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.DupImageFileLog.ToString()));


                    odfUpdateTable.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.UpdateTableColumnsNames)).Cast<SqliteEnums.UpdateTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.UpdateTable.ToString()));

                    BuildIndex(true);

                    return eErrorCodes.SUCCESS;
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cSqliteLogger::InitDB", ex);
                    return eErrorCodes.CLIENT_FAILED;
                }
                finally
                {
                    //LockAudioFilesDB.SafeExitWriteLock();
                }


            }



            public void BuildIndex(bool bOmitExtraInitializations = false)
            {
                try
                {
                    ////LockAudioFilesDB.EnterWriteLock();

                    //cGlobalSettings.oLogger.WriteLogVerbose("\tInside BuildIndex");

                    if (!bOmitExtraInitializations) odfExifInfo.ExecuteQuery("VACUUM;");
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_id]           ON [{0}] ([{1}] ASC);", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString(), SqliteEnums.ExifInfoColumnsNames.Id));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_filepath]     ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString(), SqliteEnums.ExifInfoColumnsNames.FilePath));

                    //odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_48.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_56.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_64.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_72.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_88.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_fileExifID]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.HashTable_96.ToString(), SqliteEnums.HashTableColumnsNames.FileExifID));
                    odfExifInfo.ExecuteQuery(string.Format("CREATE UNIQUE INDEX if not exists [idx_{0}_key]   ON [{0}] ([{1}]  ASC)", SqliteEnums.MediaDatabaseLogType.DupImageFileLog.ToString(), SqliteEnums.DupImageFileLogColumnsName.Key));


                    if (!bOmitExtraInitializations)
                    {
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_id];", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_filepath];", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString()));
                        //odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable.ToString()));

                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_48.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_56.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_64.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_72.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_88.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_fileExifID];", SqliteEnums.MediaDatabaseLogType.HashTable_96.ToString()));
                        odfExifInfo.ExecuteQuery(string.Format("REINDEX main.[idx_{0}_key];", SqliteEnums.MediaDatabaseLogType.DupImageFileLog.ToString()));
                    }

                    odfExifInfo.FillBlankDataSet(string.Format("select * from {0} where {1}=0", SqliteEnums.MediaDatabaseLogType.ExifInfo.ToString(), SqliteEnums.ExifInfoColumnsNames.Id.ToString()));
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cSqliteLogger::BuildIndex", ex);
                }
                finally
                {
                    ////LockAudioFilesDB.SafeExitWriteLock();
                }

            }

            public void TruncateTables()
            {
                try
                {

                    //LockAudioFilesDB.EnterWriteLock();
#if false
                    odfExifInfo.ExecuteQuery(string.Format("drop table {0}; VACUUM;", SQLiteEnums.MediaDatabaseLogType.AudioFiles.ToString())); //VACUUM;
#endif
                    //oDbfAudioFingerprintIndexer.ExecuteQuery(string.Format("drop table {0}; VACUUM;", SQLiteEnums.MediaDatabaseLogType.FpIndex.ToString())); //VACUUM;

                    //oDbfAudioFingerprintIndexer1.ExecuteQuery(string.Format("drop table {0}; VACUUM;", SQLiteEnums.MediaDatabaseLogType.FpIndex1.ToString())); //VACUUM;
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("cSqliteLogger::TruncateTables", ex);
                }
                finally
                {
                    //LockAudioFilesDB.SafeExitWriteLock();

                    // in finally so lock is freed
                    InitDB();
                }
            }


            #region StartOperation
            public eFuncRetVal FillTempTableWithScannedPath(IEnumerable<string> FilePathOfImagesScanned)
            {
                eFuncRetVal sqliteStatus = eFuncRetVal.FAIL;
                SQLiteConnection con = null;

                try
                {
                    odfExifInfo.DropTable(SqliteEnums.MediaDatabaseLogType.TempTable.ToString()); // Drop temp table if exist               

                    // Create table 
                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.TempTableColumnsNames)).Cast<SqliteEnums.TempTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.TempTable.ToString()));

                    odfExifInfo.FillBlankDataSet(string.Format("Select * from {0} where {1} = '-1'", SqliteEnums.MediaDatabaseLogType.TempTable.ToString(), SqliteEnums.TempTableColumnsNames.FilePath));

                    FilePathOfImagesScanned.All(x =>
                    {
                        DataRow dr = odfExifInfo.oDsUpdate.Tables[0].NewRow();
                        dr[0] = x;
                        odfExifInfo.oDsUpdate.Tables[0].Rows.Add(dr);
                        return true;
                    });

                    odfExifInfo.UpdateViaDataSet("fillTempTable", true, true);

                    sqliteStatus = eFuncRetVal.SUCCESS;

                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("fillTempTable::", ex);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                        con.Dispose();
                    }
                    con = null;

                    //if (dt != null) dt.Dispose(); dt = null;

                }
                return sqliteStatus;
            }


            public eFuncRetVal UpdateTableWithGroupNumber(/*IEnumerable<string> FilePathOfImagesScanned*/)
            {
                eFuncRetVal sqliteStatus = eFuncRetVal.FAIL;
                SQLiteConnection con = null;

                try
                {
                    odfExifInfo.DropTable(SqliteEnums.MediaDatabaseLogType.UpdateTable.ToString()); // Drop temp table if exist               

                    // Create table 
                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.UpdateTableColumnsNames)).Cast<SqliteEnums.UpdateTableColumnsNames>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.UpdateTable.ToString()));

                    odfExifInfo.FillBlankDataSet(string.Format("Select * from {0} where {1} = '-1'", SqliteEnums.MediaDatabaseLogType.UpdateTable.ToString(), SqliteEnums.UpdateTableColumnsNames.Key));

                    cGlobalSettings.listImageFileInfo.Where(p => p.Value.groupNumber > 0).All(x =>
                    {
                        DataRow dr = odfExifInfo.oDsUpdate.Tables[0].NewRow();
                        dr[0] = x.Key;
                        dr[1] = x.Value.groupNumber;
                        dr[2] = x.Value.isMark ? 1 : 0;
                        odfExifInfo.oDsUpdate.Tables[0].Rows.Add(dr);
                        return true;
                    });

                    odfExifInfo.UpdateViaDataSet("UpdateTable", true, true);

                    sqliteStatus = eFuncRetVal.SUCCESS;

                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("fillTempTable::", ex);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                        con.Dispose();
                    }
                    con = null;

                    //if (dt != null) dt.Dispose(); dt = null;

                }
                return sqliteStatus;
            }

            public string ReturnTableNameOnBaseOfCurrentAlgo()
            {
                if (cGlobalSettings.isSimilarMatchChecked && Enum.IsDefined(typeof(SqliteEnums.MediaDatabaseLogType), cGlobalSettings.ImageResize))
                {
                    return ((SqliteEnums.MediaDatabaseLogType)cGlobalSettings.ImageResize).ToString();
                }
                else
                {
                    return SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString();
                }

            }

            public void FillBlankDataSetForTableExifInfo()
            {
                odfExifInfo.FillBlankDataSet(string.Format("select * from {0} where {1} = -1", SqliteEnums.MediaDatabaseLogType.ExifInfo, SqliteEnums.ExifInfoColumnsNames.Id));
            }

            public void FillBlankDataSetForTableHash()
            {
                odfHash.FillBlankDataSet(string.Format("select * from {0} where {1} = -1", ReturnTableNameOnBaseOfCurrentAlgo(), SqliteEnums.HashTableColumnsNames.FileExifID));
            }

            public void FillBlankDataSetForTableImageFileLog()
            {
                odfImageFileLog.FillBlankDataSet(string.Format("select * from {0} where {1} = -1", SqliteEnums.MediaDatabaseLogType.DupImageFileLog, SqliteEnums.DupImageFileLogColumnsName.Key));
            }

            public DataSet GetRecordsDataWhichAlreadyExistsInDB()
            {
                DataSet ds = null;
                try
                {
                    string TableName = ReturnTableNameOnBaseOfCurrentAlgo();
                    string strQuery = string.Format(
                            @"select ei.*,h1.fileHash from exifInfo ei 
                            inner join tempTable t1 on t1.filepath = ei.filepath 
                            left join {0} h1 on ei.id= h1.fileexifid",
                        TableName);

                    ds = odfExifInfo.SelectData(strQuery);

                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("fillTempTable::", ex);
                    ds = null;
                }
                finally
                {
                }

                return ds;
            }


            public void LoadSqliteToDictionary(bool bGetIdsOnly)
            {
                Stopwatch watch = null;
                SQLiteDataReader oDataReader = null;
                try
                {
                    if (cGlobalSettings.sqliteLogger == null)
                    {
                        cGlobalSettings.sqliteLogger = new cSqliteLogger();
                        cGlobalSettings.sqliteLogger.InitDB();
                    }
                    watch = new Stopwatch();

                    string tableName = "";
                    if (cGlobalSettings.isSimilarMatchChecked && Enum.IsDefined(typeof(SqliteEnums.MediaDatabaseLogType), cGlobalSettings.ImageResize))
                    {
                        tableName = ((SqliteEnums.MediaDatabaseLogType)cGlobalSettings.ImageResize).ToString();
                    }
                    else // If exact match
                    {
                        tableName = SqliteEnums.MediaDatabaseLogType.HashTable_80.ToString();
                    }
                    string query = string.Format(@"select {0},h1.fileHash from exifInfo ei 
                            inner join tempTable t1 on t1.filepath = ei.filepath 
                            left join {1} h1 on ei.id= h1.fileexifid",
                                String.Join(",", Enum.GetValues(typeof(SqliteEnums.ExifInfoColumnsNames)).Cast<SqliteEnums.ExifInfoColumnsNames>().Select(x => "ei." + x.ToString()).ToArray()) + "",
                                                                            tableName);


                    oDataReader = cGlobalSettings.sqliteLogger.odfHash.SelectDataReader(query);


                    //cGlobalSettings.oLogger.WriteLogVerbose("\tDB loaded:\t{0}", watch.GetTimeSpanEx());
                    watch.Reset();
                    watch.Start();

                    int counter = 0;
                    while (oDataReader.Read())
                    {

                        string filePath = oDataReader.GetString((int)SqliteEnums.ExifInfoColumnsNames.FilePath);
                        if (!cGlobalSettings.MediaFilePathToFileId.ContainsKey(filePath))
                            continue;

                        int key = cGlobalSettings.MediaFilePathToFileId[filePath];




                        /// assign file path
                        cGlobalSettings.listImageFileInfo[key].filePath = filePath;

                        if (bGetIdsOnly)
                        {
                            cGlobalSettings.listImageFileInfo[key].exifInfo.FileExifId = oDataReader.GetInt32((int)SqliteEnums.ExifInfoColumnsNames.Id);

                            /*cGlobalSettings.listImageFileInfo[key].exifInfo = new csExifInfo
                            {
                                FileExifId = oDataReader.GetInt32((int)SqliteEnums.ExifInfoColumnsNames.Id)
                            };*/
                        }
                        else
                        {
                            if (cGlobalSettings.listImageFileInfo[key].exifInfo == null ||
                                cGlobalSettings.CurrentScanMode != eScanMode.GoogleDrive ||
                                cGlobalSettings.CurrentScanMode != eScanMode.Dropbox)
                            {
                                cGlobalSettings.listImageFileInfo[key].exifInfo = new csExifInfo();
                            }
                            cGlobalSettings.listImageFileInfo[key].exifInfo.FileExifId = oDataReader.GetInt32((int)SqliteEnums.ExifInfoColumnsNames.Id); // Get Id from Exif Table 

                            //-------- Check if file is modified with same name by user -----------------
                            if (cGlobalSettings.listImageFileInfo[key].fileSize != oDataReader.GetInt64((int)SqliteEnums.ExifInfoColumnsNames.FileSize))
                            {
                                cGlobalSettings.listImageFileInfo[key].bFileModified = true;
                                continue;
                            }
                            // Format our new DateTime object to start at the UNIX Epoch
                            System.DateTime modDateTime = new System.DateTime(1970, 1, 1);
                            Int64 totalModTicks = oDataReader.GetInt64((int)SqliteEnums.ExifInfoColumnsNames.modDate);
                            modDateTime = (modDateTime.AddSeconds(totalModTicks - 1).Date) == DateTime.MaxValue.Date ? DateTime.MaxValue : modDateTime.AddSeconds(totalModTicks);

                            if (cGlobalSettings.listImageFileInfo[key].modDate.Date != modDateTime.Date)
                            {
                                cGlobalSettings.listImageFileInfo[key].bFileModified = true;
                                continue;
                            }
                            /*
                            * select ei.Id,ei.FilePath,ei.CaptureDate,ei.FileSize,ei.GpsLatitude,ei.GpsLongitude,ei.PixelWidth,ei.PixelHeight,ei.DpiWidth,ei.DpiHeight,ei.Orientation,h1.fileHash from exifInfo ei 
                                       inner join tempTable t1 on t1.filepath = ei.filepath 
                                       left join HashTable_64 h1 on ei.id= h1.fileexifid
                            * 
                            * */



                            cGlobalSettings.listImageFileInfo[key].exifInfo.FileSize = oDataReader.GetInt64((int)SqliteEnums.ExifInfoColumnsNames.FileSize);
                            cGlobalSettings.listImageFileInfo[key].exifInfo.latitudeDegree = oDataReader.GetDouble((int)SqliteEnums.ExifInfoColumnsNames.GpsLatitude);
                            cGlobalSettings.listImageFileInfo[key].exifInfo.longitudeDegree = oDataReader.GetDouble((int)SqliteEnums.ExifInfoColumnsNames.GpsLongitude);
                            cGlobalSettings.listImageFileInfo[key].exifInfo.Orientation = Convert.ToString(oDataReader.GetValue((int)SqliteEnums.ExifInfoColumnsNames.Orientation));
                            cGlobalSettings.listImageFileInfo[key].exifInfo.ImageDimension = new System.Drawing.Size(oDataReader.GetInt32((int)SqliteEnums.ExifInfoColumnsNames.PixelWidth), oDataReader.GetInt32((int)SqliteEnums.ExifInfoColumnsNames.PixelHeight));
                            cGlobalSettings.listImageFileInfo[key].exifInfo.HorizontalResolution = oDataReader.GetFloat((int)SqliteEnums.ExifInfoColumnsNames.DpiWidth);
                            cGlobalSettings.listImageFileInfo[key].exifInfo.VerticalResolution = oDataReader.GetFloat((int)SqliteEnums.ExifInfoColumnsNames.DpiHeight);

                            //read height and width for detail view of result
                            cGlobalSettings.listImageFileInfo[key].Height = cGlobalSettings.listImageFileInfo[key].exifInfo.ImageDimension.Height;
                            cGlobalSettings.listImageFileInfo[key].Width = cGlobalSettings.listImageFileInfo[key].exifInfo.ImageDimension.Width;


                            // Format our new DateTime object to start at the UNIX Epoch
                            System.DateTime dateTime = new System.DateTime(1970, 1, 1);
                            Int64 totalTicks = oDataReader.GetInt64((int)SqliteEnums.ExifInfoColumnsNames.CaptureDate);
                            dateTime = (dateTime.AddSeconds(totalTicks - 1).Date) == DateTime.MaxValue.Date ? DateTime.MaxValue : dateTime.AddSeconds(totalTicks);

                            cGlobalSettings.listImageFileInfo[key].exifInfo.CaptureDate = dateTime;

                            object objHash = oDataReader["fileHash"];

                            // Check is Hash present for selected BitRate 
                            if (objHash != null)
                            {
                                if (objHash.GetType() != typeof(System.DBNull))
                                {
                                    //if (cGlobalSettings.isSimilarMatchChecked)
                                    {
                                        byte[] byteHash = (byte[])objHash;
                                        if (!GetBytesList(key, byteHash))
                                            ImageProcessor.ConvertByteArrayToLongArray(key, ref byteHash);
                                        byteHash = null;
                                    }
                                    /*else
                                        cGlobalSettings.listImageFileInfo[key].md5 = Encoding.UTF8.GetString((byte[])objHash);*/

                                    cGlobalSettings.listImageFileInfo[key].bHashFoundInDB = true;
                                }
                            }
                        }
                    }

                    oDataReader.Close();
                    oDataReader.Dispose();

                    counter++;
                    if (counter > 10000)
                    {
                        GC.Collect();
                        counter = 0;
                    }
                    //cGlobalSettings.oLogger.WriteLogVerbose("\tDataset to dict converted:\t{0}", watch.GetTimeSpanEx());
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("LoadSqliteToDictionary", ex);
                }
                finally
                {
                    if (oDataReader != null)
                    {
                        oDataReader = null;
                    }

                    GC.Collect();
                }
            }

            bool GetBytesList(int key, byte[] objRawHash)
            {
                bool bSuccesfullyConverted = false;
                try
                {
                    int singleArrLength = objRawHash.Length;
                    if (cGlobalSettings.ImageResize * cGlobalSettings.ImageResize == objRawHash.Length)
                    {
                        singleArrLength = (objRawHash.Length / 8); // 512;
                    }
                    //check that the byte is the correct length
                    if (objRawHash.Length % singleArrLength != 0)
                        return bSuccesfullyConverted;
                    int totalHashes = objRawHash.Length / singleArrLength;
                    for (int i = 0; i < totalHashes; i++)
                    {
                        byte[] single = objRawHash.Take(singleArrLength).ToArray(); // take the first 18
                        ImageProcessor.ConvertByteArrayToLongArray(key, i, single);
                        if (i == 0)
                        {
                            ImageProcessor.ConvertByteArrayToLongArray(key, ref single);
                        }
                        objRawHash = objRawHash.Skip(singleArrLength).ToArray(); // remove the first 18 from the collection
                                                                                 //Yield DeconvertOne(single);
                    }

                    bSuccesfullyConverted = true;
                    /*object[] objList = Convert.ToString(objRawHash).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (objList == null || objList.Length == 0)
                        return;

                    for(int i=0; i< objList.Length; i++)
                    {
                        byte[] byteHash = (byte[])objList[i];
                        ImageProcessor.ConvertByteArrayToLongArray(key,i, byteHash);
                        byteHash = null;
                    }*/

                }
                catch (Exception ex)
                {

                }
                return bSuccesfullyConverted;
            }

            void ConvertImageFileInfoToDataRow(csImageFileInfo imageInfo, ref DataRow dr)
            {
                try
                {
                    dr[SqliteEnums.DupImageFileLogColumnsName.Key.ToString()] = imageInfo.key;
                    dr[SqliteEnums.DupImageFileLogColumnsName.ExifId.ToString()] = imageInfo.exifInfo.FileExifId;
                    dr[SqliteEnums.DupImageFileLogColumnsName.GroupNumber.ToString()] = imageInfo.groupNumber;
                    dr[SqliteEnums.DupImageFileLogColumnsName.IsMark.ToString()] = imageInfo.isMark ? 1 : 0;
                    dr[SqliteEnums.DupImageFileLogColumnsName.IsLocked.ToString()] = imageInfo.isLocked ? 1 : 0;
                    //dr[SqliteEnums.DupImageFileLogColumnsName.IsProtected.ToString()] = imageInfo.isLocked ? 1 : 0;
                    dr[SqliteEnums.DupImageFileLogColumnsName.Match.ToString()] = imageInfo.match;
                    dr[SqliteEnums.DupImageFileLogColumnsName.FolderPath.ToString()] = imageInfo.folderPath;

                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("ConvertImageFileInfoToDataRow", ex);
                }
            }

            public void InsertImageFileLog(IEnumerable<int> keyToInsert)
            {
                try
                {

                    odfImageFileLog.DropTable(SqliteEnums.MediaDatabaseLogType.DupImageFileLog.ToString()); // Drop temp table if exist               

                    // Create table 
                    odfExifInfo.ExecuteQuery(string.Format("CREATE TABLE IF NOT EXISTS [{0}] (" +
                            String.Join(",", Enum.GetValues(typeof(SqliteEnums.DupImageFileLogColumnsName)).Cast<SqliteEnums.DupImageFileLogColumnsName>().Select(x => x + " " + x.stringValueOfDescAttr()).ToArray()) + ");",
                            SqliteEnums.MediaDatabaseLogType.DupImageFileLog.ToString()));

                    FillBlankDataSetForTableImageFileLog();

                    foreach (int i in keyToInsert)
                    {
                        // Get exifID for path whose id is not able to  get
                        if (cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId < 1)
                        {
                            string query = string.Format("select {0} from {1} where {2}='{3}'", SqliteEnums.ExifInfoColumnsNames.Id, SqliteEnums.MediaDatabaseLogType.ExifInfo, SqliteEnums.ExifInfoColumnsNames.FilePath, cGlobalSettings.listImageFileInfo[i].filePath);

                            SQLiteDataReader reader = null;
                            try
                            {
                                reader = odfImageFileLog.SelectDataReader(query);
                                if (reader.Read())
                                {
                                    cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId = Convert.ToInt32(reader[0].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLogException("InsertImageFileLog:: ReadExifInfoFromDB:", ex);
                            }
                            finally
                            {
                                if (reader != null) reader.Close(); reader = null;
                            }

                        }
                        DataRow dr = cGlobalSettings.sqliteLogger.odfImageFileLog.oDsUpdate.Tables[0].NewRow();
                        ConvertImageFileInfoToDataRow(cGlobalSettings.listImageFileInfo[i], ref dr);
                        cGlobalSettings.sqliteLogger.odfImageFileLog.oDsUpdate.Tables[0].Rows.Add(dr);
                    }

                    cGlobalSettings.sqliteLogger.odfImageFileLog.UpdateViaDataSet("ReadAllHashes", true, true);

                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("InsertImageFileLog:: ", ex);
                }
            }

            void InsertNewRecordsToDB(IEnumerable<int> fileIdsNotFoundInDb)
            {
                try
                {
                    /// insert these to db
                    cGlobalSettings.sqliteLogger.FillBlankDataSetForTableExifInfo();

                    foreach (int i in fileIdsNotFoundInDb)
                    {
                        if (cGlobalSettings.listImageFileInfo[i].exifInfo != null && cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId < 0)
                        {
                            DataRow dr = cGlobalSettings.sqliteLogger.odfExifInfo.oDsUpdate.Tables[0].NewRow();
                            ConvertImageFileInfoToDataRow(cGlobalSettings.listImageFileInfo[i], ref dr);
                            cGlobalSettings.sqliteLogger.odfExifInfo.oDsUpdate.Tables[0].Rows.Add(dr);
                        }
                    }

                    cGlobalSettings.sqliteLogger.odfExifInfo.UpdateViaDataSet("ReadAllHashes", true, true);

                    cGlobalSettings.sqliteLogger.FillTempTableWithScannedPath(cGlobalSettings.listImageFileInfo.Select(x => x.Value.filePath.ToLower()));

                    /// get all file ExifInfo ids
                    cGlobalSettings.sqliteLogger.LoadSqliteToDictionary(true);

                    cGlobalSettings.sqliteLogger.FillBlankDataSetForTableHash();
                    foreach (int i in fileIdsNotFoundInDb)
                    {
                        if (cGlobalSettings.listImageFileInfo[i].exifInfo != null)
                        {
                            DataRow dr = cGlobalSettings.sqliteLogger.odfHash.oDsUpdate.Tables[0].NewRow();

                            dr[SqliteEnums.HashTableColumnsNames.FileExifID.ToString()] = cGlobalSettings.listImageFileInfo[i].exifInfo.FileExifId;

                            //if (cGlobalSettings.isSimilarMatchChecked)
                            {
                                dr[SqliteEnums.HashTableColumnsNames.FileHash.ToString()] = cGlobalSettings.listImageFileInfo[i].byteHash;
                            }
                            /*else
                            {
                                dr[SqliteEnums.HashTableColumnsNames.FileHash.ToString()] = Encoding.UTF8.GetBytes(cGlobalSettings.listImageFileInfo[i].md5);
                            }*/
                            cGlobalSettings.sqliteLogger.odfHash.oDsUpdate.Tables[0].Rows.Add(dr);
                        }
                    }

                    cGlobalSettings.sqliteLogger.odfHash.UpdateViaDataSet("ReadAllHashes", true, true);

                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("InsertNewRecordsToDB ::", ex);
                }
            }

            #endregion



            #region AudioFiles




            public DataRow GetNewBlankRow(SqliteEnums.MediaDatabaseLogType tableName, string sColName)
            {
                odfExifInfo.FillBlankDataSet(string.Format("select * from {0} where {1}=0", tableName, sColName));
                return odfExifInfo.oDsUpdate.Tables[0].NewRow();
            }

            #endregion



        }




    }

