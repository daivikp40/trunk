using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DuplicatePhotosFixer.HelperClasses.MyLogger;

namespace DuplicatePhotosFixer.Sqlite
{
    public class cSQliteFunction
    {

        public enum DataType { String = 1, Number = 2, DateTime = 3, Long = 4, Binary = 5, Real = 6 };
        public SQLiteConnection dbconn = null;// new SQLiteConnection();
        SQLiteCommand comm = null;
        SQLiteCommand syncSQLiteCommand = null;
        public DataSet ds;
        SQLiteDataAdapter ada = null;
        public UInt32 uiMinId = 100000;
        private string m_UserName;
        private string m_Password;
        private bool m_IsCompressed;

        private bool m_CreateNew;


        //for updation from dataset

        public SQLiteConnection oConnection = null/*new SQLiteConnection()*/;
        public SQLiteCommand oCommand = null;
        public SQLiteCommandBuilder oCommandBuilder = null;
        public SQLiteDataAdapter oAdatpter = null;
        public DataSet oDsUpdate = null;
        public DataSet oDsUpdateRecords = null;
        public bool bUseEncryption = false;
        EventWaitHandle waitHandle = null;


        //CUtils oUtils = new CUtils();
        public cSQliteFunction(string handleName, int randomNo)
        {
            //cHardwareHash oHash = null;
            try
            {
                //string val = cHardwareHashFromCPP.GetHardwareHash();
                //oHash = new cHardwareHash();
                m_UserName = "admin";
                m_Password = "admin"/*HardwareHash.GetHardwareHash().ToString()*/;//oHash.GetHardwareHash() /*+ "11"*/; //"asp123cmdscan"; //"6248555299900054506";//
                //cGlobalSettings.oLogger.WriteLogVerbose(/*"cSQliteFunction:db:acc:"*/"sdbacc:" + m_Password);
                m_IsCompressed = false;
                m_CreateNew = true;

                string connectionString = "";


                dbconn = new SQLiteConnection(connectionString);
                comm = new SQLiteCommand();
                syncSQLiteCommand = new SQLiteCommand();
                ada = new SQLiteDataAdapter();
                oConnection = new SQLiteConnection();
                //
                // TODO: Add constructor logic here
                //

                waitHandle = new EventWaitHandle(true, EventResetMode.ManualReset, handleName + randomNo.ToString());
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cSQliteFunction()", ex);
            }
            finally
            {
                //oHash = null;
            }
        }


        public cSQliteFunction(string connectionString)
        {

            dbconn = new SQLiteConnection(connectionString);

        }
        public string DatabaseConnectionString
        {
            get
            {
                return dbconn.ConnectionString;
            }
            set
            {
                dbconn.ConnectionString = (value);
            }
        }
        /* public string UserName
         {
             get { return m_UserName; }
             set { m_UserName = value; }
         }

         public string Password
         {
             get { return m_Password; }
             set { m_Password = value; }
         }*/
        public SQLiteConnection DatabaseConnection
        {
            get { return dbconn; }
            set { dbconn = value; }
        }
        public bool IsCompressed
        {
            get { return m_IsCompressed; }
            set { m_IsCompressed = value; }
        }
        public bool CreateNew
        {
            get { return m_CreateNew; }
            set { m_CreateNew = value; }
        }

        #region CreateConnection Object
        public void CreateConnection(string databaseName, bool IsCompressed, bool CreateNew)
        {
            CreateConnection(cGlobalSettings.GetDBPath(), databaseName, IsCompressed, CreateNew, m_UserName, m_Password);

        }
        public void CreateConnection(string DatabasePath, string databaseName, bool IsCompressed, bool CreateNew)
        {
            CreateConnection(DatabasePath, databaseName, IsCompressed, CreateNew, m_UserName, m_Password);

        }
        public void CreateConnection(string DatabasePath, string databaseName)
        {
            CreateConnection(DatabasePath, databaseName, m_IsCompressed, m_CreateNew, m_UserName, m_Password);

        }
        public void CreateConnection(string DatabaseFullPath)
        {

            string sDbPath, sDbName;
            sDbPath = sDbName = string.Empty;

            if (DatabaseFullPath.Contains("\\"))
            {
                sDbPath = DatabaseFullPath.Substring(0, DatabaseFullPath.LastIndexOf("\\"));
                sDbName = DatabaseFullPath.Substring(DatabaseFullPath.LastIndexOf("\\") + 1);
            }
            else
            {
                sDbPath = cGlobalSettings.GetDBPath();
                sDbName = DatabaseFullPath;
            }
            CreateConnection(sDbPath, sDbName, m_IsCompressed, m_CreateNew, m_UserName, m_Password);


        }
        public void CreateConnection(string DatabasePath, string databaseName, bool IsCompressed, bool CreateNew, string sUserName, string sPassword)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                //data encryption increasing database size by 20 mb so removed
                string sConn = string.Empty;

                if (bUseEncryption) sConn = string.Format("Data Source={0};Version=3;New={1};Compress={2};synchronous=ON;username={3};password={4};auto_vacuum=FULL;", PathEx.Combine(DatabasePath, databaseName), CreateNew, IsCompressed, sUserName, sPassword);
                else sConn = string.Format("Data Source={0};Version=3;New={1};Compress={2};synchronous=ON;auto_vacuum=FULL;", PathEx.Combine(DatabasePath, databaseName), CreateNew, IsCompressed, sUserName, sPassword);

                if (dbconn == null)
                {
                    dbconn = new SQLiteConnection(sConn);

                }
                else
                {
                    if (dbconn.State == ConnectionState.Open)
                    {
                        dbconn.Close();

                    }

                    dbconn.ConnectionString = sConn;
                }

                oConnection = new System.Data.SQLite.SQLiteConnection(sConn);
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        }

        #endregion


        #region CreateConnectionString
        public string CreateConnectionString(string databaseName, bool IsCompressed, bool CreateNew)
        {
            return CreateConnectionString(cGlobalSettings.GetDBPath(), databaseName, IsCompressed, CreateNew, m_UserName, m_Password);

        }
        public string CreateConnectionString(string DatabasePath, string databaseName, bool IsCompressed, bool CreateNew)
        {
            return CreateConnectionString(DatabasePath, databaseName, IsCompressed, CreateNew, m_UserName, m_Password);

        }
        public string CreateConnectionString(string DatabasePath, string databaseName)
        {
            return CreateConnectionString(DatabasePath, databaseName, m_IsCompressed, m_CreateNew, m_UserName, m_Password);

        }
        public string CreateConnectionString(string DatabaseFullPath)
        {
            string sDbPath, sDbName;
            sDbPath = sDbName = string.Empty;

            if (DatabaseFullPath.Contains("\\"))
            {
                sDbPath = DatabaseFullPath.Substring(0, DatabaseFullPath.LastIndexOf("\\"));
                sDbName = DatabaseFullPath.Substring(DatabaseFullPath.LastIndexOf("\\") + 1);
            }
            else
            {
                sDbPath = cGlobalSettings.GetDBPath()/*cGlobal.DATABASE_PATH*/;
                sDbName = DatabaseFullPath;
            }
            return CreateConnectionString(sDbPath, sDbName, m_IsCompressed, m_CreateNew, m_UserName, m_Password);

        }
        public string CreateConnectionString(string DatabasePath, string databaseName, bool IsCompressed, bool CreateNew, string sUserName, string sPassword)
        {
            return string.Format("Data Source={0};Version=3;New={1};Compress={2};synchronous=OFF;"/*username={3};password={4}*/, PathEx.Combine(DatabasePath, databaseName), CreateNew, IsCompressed/*, sUserName, sPassword*/);

        }
        #endregion


        public void FillBlankDataSet(string sQuery)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }



                oDsUpdate = new DataSet();
                oDsUpdateRecords = new DataSet();
                oCommand = new SQLiteCommand(sQuery, oConnection);
                oAdatpter = new SQLiteDataAdapter(oCommand);
                oCommandBuilder = new SQLiteCommandBuilder(oAdatpter);
                oAdatpter.Fill(oDsUpdate);
                oDsUpdateRecords = oDsUpdate.Copy();
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cSqLiteFunction|FillBlankDataSet()|exception:", ex);
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        }



        public bool UpdateViaDataSet(string calledFrom, bool bClearTableRows = false, bool bUseLock = true)
        {
            bool bRes = false;
            try
            {
                if (bUseLock) sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                if (oDsUpdate == null) return false;
                if (oDsUpdate.Tables[0].Rows.Count == 0) return false;

                //SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(oAdatpter);
                //oAdatpter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                //SQLiteCommand cmdInsert = new SQLiteCommand(commandBuilder.GetInsertCommand().CommandText, oConnection);
                //cmdInsert.CommandText += ";Select SCOPE_IDENTITY() as id";


                oConnection.Open();
                using (SQLiteTransaction oTran = oConnection.BeginTransaction())
                {
                    //oAdatpter.InsertCommand = cmdInsert;
                    //oAdatpter.InsertCommand.UpdatedRowSource = UpdateRowSource.Both;
                    //oAdatpter.AcceptChangesDuringUpdate = true;
                    oAdatpter.Update(oDsUpdate.Tables[0]);
                    oTran.Commit();
                    oDsUpdate.Dispose();
                    //oDsUpdate.AcceptChanges();

                    bRes = true;
                }
                oConnection.Close();

                if (bClearTableRows)
                {
                    oDsUpdate.Tables[0].Rows.Clear();
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(calledFrom + " cSqLiteFunction|UpdateViaDataSet()|exception: ", ex);
            }
            finally
            {
                try
                {
                    if (oConnection != null) oConnection.Close();
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("UpdateViaDataSet ::", ex);
                }


                if (waitHandle != null) waitHandle.Set();

                if (bUseLock) sqlLiteLock.SafeExitWriteLock();
            }
            return bRes;
        }

        public DataSet SelectData(string sSqlQuery)
        {
            return SelectData(sSqlQuery, null);
        }
        public static ReaderWriterLockSlim sqlLiteLock = new ReaderWriterLockSlim();
        public DataSet SelectData(string sSqlQuery, string sTableName)
        {



            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                ds = new DataSet();

                if (string.IsNullOrEmpty(dbconn.ConnectionString)) return ds;

                ds.Clear();
                SQLiteDataAdapter ada = new SQLiteDataAdapter();
                comm = new SQLiteCommand(sSqlQuery, dbconn);

                ada = new SQLiteDataAdapter(comm);
                if (string.IsNullOrEmpty(sTableName))
                {
                    ada.Fill(ds);
                }
                else
                {
                    ada.Fill(ds, sTableName);
                }

                comm.Dispose();
                return ds;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("cSqliteFunction|SelectData({0}):error:", sSqlQuery, ex), ex);
                if (!File.Exists(cGlobalSettings.GetDBPath()))
                {
                    cWin32APIs.PostMessage((IntPtr)cWin32APIs.HWND_BROADCAST, cWin32APIs.WM_CALLSIGNOUT, IntPtr.Zero, IntPtr.Zero);
                }
                return ds;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        }

        public SQLiteDataReader SelectDataReader(string sSqlQuery)
        {
            SQLiteDataReader oReader = null;
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                comm = new SQLiteCommand(sSqlQuery, dbconn);
                if (dbconn.State == ConnectionState.Closed)
                {
                    dbconn.Open();
                }
                oReader = comm.ExecuteReader();
                return oReader;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("cSqliteFunction|SelectDataReader({0}):error:", sSqlQuery, ex), ex);
                return oReader;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        }

        public DataSet SelectData(string sSqlQuery, string sWhere, string[] sFieldName, byte[] byFieldType, string[] sFieldValue)
        {
            ds = new DataSet();
            ds.Clear();
            string sSql = "";

            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                if (sFieldName.Length == byFieldType.Length && byFieldType.Length == sFieldValue.Length)
                {
                    sSql = sSqlQuery + " where " + sWhere;
                    comm = new SQLiteCommand(sSql, dbconn);

                    for (byte byFieldCount = 0; byFieldCount < sFieldValue.Length; byFieldCount++)
                    {

                        if (byFieldType[byFieldCount] == (byte)DataType.String)//varchar
                        {
                            comm.Parameters.Add("@" + sFieldName[byFieldCount], (DbType)SqlDbType.VarChar, 4000).Value = Convert.ToString(sFieldValue[byFieldCount]);
                        }
                        else if (byFieldType[byFieldCount] == (byte)DataType.Number)//int
                        {
                            comm.Parameters.Add("@" + sFieldName[byFieldCount], (DbType)SqlDbType.Int).Value = Convert.ToInt32(sFieldValue[byFieldCount]);
                        }
                        else if (byFieldType[byFieldCount] == (byte)DataType.DateTime)// datetime
                        {
                            comm.Parameters.Add("@" + sFieldName[byFieldCount], (DbType)SqlDbType.DateTime).Value = Convert.ToDateTime(sFieldValue[byFieldCount]);
                        }
                    }

                    ada = new SQLiteDataAdapter(comm);
                    ada.Fill(ds);
                    return ds;

                }
                else
                {
                    return ds;
                }
            }
            catch (Exception ex)
            {


                cGlobalSettings.oLogger.WriteLogException(string.Format("cSqliteFunction|SelectData({0},{1}):error:", sSqlQuery, sWhere, ex), ex);

                if (!File.Exists(cGlobalSettings.GetDBPath()))
                {
                    cWin32APIs.PostMessage((IntPtr)cWin32APIs.HWND_BROADCAST, cWin32APIs.WM_CALLSIGNOUT, IntPtr.Zero, IntPtr.Zero);
                }

                return ds;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        }

        public UInt32 getCount(string sTableName, string sFieldName)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                ds = new DataSet();

                string sSql = "";
                UInt32 uiGetCount = 0;
                sSql = "SELECT Count(" + sFieldName + ") as Cnt from " + sTableName;

                SQLiteCommand comm = new SQLiteCommand(sSql, dbconn);
                SQLiteDataAdapter ada = new SQLiteDataAdapter(comm);
                ada.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    uiGetCount = Convert.ToUInt16(ds.Tables[0].Rows[0]["Cnt"]);
                }

                ds.Clear();

                return uiGetCount;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        } // end function getcount

        // getCount with where clause
        public UInt32 getCount(string sTableName, string sFieldName, string sWhere)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sSql = "";
                UInt32 uiCount = 0;
                sSql = "SELECT Count(" + sFieldName + ") as Cnt from " + sTableName;

                if (sWhere != string.Empty)
                {
                    sSql = sSql + " where " + sWhere;
                }

                comm = new SQLiteCommand(sSql, dbconn);

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                uiCount = Convert.ToUInt32(comm.ExecuteScalar());
                dbconn.Close();

                return uiCount;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        } // end function getcount with where clause

        public UInt32 getScalar(string Command)
        {
            UInt32 uiMax = 0;
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                comm = new SQLiteCommand(Command, dbconn);

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                uiMax = Convert.ToUInt32(comm.ExecuteScalar());
                dbconn.Close();
                comm.Dispose();
                return uiMax;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("getScalar({0})|error:", Command, ex.Message), ex);
                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }

                return uiMax;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }


        }// end getMax function
        public string getScalarString(string Command)
        {
            string Res = string.Empty;
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                comm = new SQLiteCommand(Command, dbconn);

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                Res = Convert.ToString(comm.ExecuteScalar());
                dbconn.Close();
                comm.Dispose();
                return Res;
            }
            catch (Exception ex)
            {

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                cGlobalSettings.oLogger.WriteLogException(string.Format("getScalar({0})|error:", Command, ex.Message), ex);
                return Res;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }


        }// end getMax function


        public T getScalarT<T>(string Command)
        {
            T oRetVal = default(T);
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                comm = new SQLiteCommand(Command, dbconn);

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                oRetVal = (T)Convert.ChangeType(comm.ExecuteScalar(), typeof(T));
                //Res = Convert.ToString(comm.ExecuteScalar());
                dbconn.Close();
                comm.Dispose();
                return oRetVal;
            }
            catch (Exception ex)
            {

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                cGlobalSettings.oLogger.WriteLogException(string.Format("getScalar({0})|error:", Command, ex.Message), ex);
                return oRetVal;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }


        }


        //Function to get max of the field specified in table
        public UInt32 getMax(string sTableName, string sFieldName)
        {
            UInt32 uiMax = 0;
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sSql;

                sSql = "SELECT IFNULL(Max(" + sFieldName + ") ,0) as Mx from " + sTableName;

                comm = new SQLiteCommand(sSql, dbconn);

                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                uiMax = Convert.ToUInt32(comm.ExecuteScalar());
                dbconn.Close();
                return uiMax;
            }
            catch (Exception ex)
            {
                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                cGlobalSettings.oLogger.WriteLogException(string.Format("getMax({0})|error:", sTableName, ex.Message), ex);
                return uiMax;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }


        }// end getMax function

        //Function to get max of the field specified in table
        public UInt32 getMax(string sTableName, string sFieldName, string sWhere) //get max with where clause
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sSql;
                UInt32 uiMax = 0;
                sSql = "SELECT IFNULL(Max(" + sFieldName + "),0) as Mx from " + sTableName;
                if (sWhere != "")
                {
                    sSql = sSql + " where " + sWhere;
                }

                comm = new SQLiteCommand(sSql, dbconn);
                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
                dbconn.Open();
                uiMax = Convert.ToUInt32(comm.ExecuteScalar());
                dbconn.Close();
                return uiMax;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        }// end getMax function with where clause



        public UInt32 getSpywareMax(string sTableName, string sFieldName) //get max with where clause
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                ds = new DataSet();
                string sSql;
                UInt32 uiMax = 0;
                sSql = "SELECT Max(" + sFieldName + ") as Mx from " + sTableName;


                comm = new SQLiteCommand(sSql, dbconn);
                ada = new SQLiteDataAdapter(comm);
                ada.Fill(ds);

                if (ds.Tables[0].Rows[0]["Mx"] != System.DBNull.Value)
                {
                    uiMax = Convert.ToUInt32(ds.Tables[0].Rows[0]["Mx"]);
                    if (uiMax < uiMinId)
                    {
                        uiMax = uiMinId;
                    }
                }
                else
                {
                    uiMax = 200000;
                }

                ds.Clear();
                return uiMax;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        }// end getMax function with where clause


        public UInt32 insertData(string sTableName, string[] sFieldName, byte[] sFieldType, string[] sFieldValue)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sSql = "";
                string values = "";
                string fNames = "";
                for (int byFieldCount = 0; byFieldCount < sFieldValue.Length; byFieldCount++)
                {
                    if (byFieldCount != sFieldValue.Length - 1)
                    {
                        fNames += Convert.ToString(sFieldName[byFieldCount]) + ",";
                        values += "@" + Convert.ToString(sFieldName[byFieldCount]) + ",";
                    }
                    else
                    {
                        fNames += Convert.ToString(sFieldName[byFieldCount]);
                        values += "@" + Convert.ToString(sFieldName[byFieldCount]);
                    }
                }

                if (values != "" && values != null && sTableName != "")
                {
                    sSql = "insert into " + sTableName + " (" + fNames + ") values (" + values + ")";
                    comm = dbconn.CreateCommand();
                    comm.CommandText = sSql;

                    for (byte byFieldCount = 0; byFieldCount < sFieldValue.Length; byFieldCount++)
                    {
                        if (Convert.ToString(sFieldType[byFieldCount]) == "1")//varchar
                        {

                            comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.VarChar), 5000).Value = Convert.ToString(sFieldValue[byFieldCount]);
                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "2")//int
                        {
                            if (sFieldValue[byFieldCount].ToString() != string.Empty)
                            {
                                if (sFieldValue[byFieldCount].ToString().ToLower() == "true")
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 1;
                                }
                                else if (sFieldValue[byFieldCount].ToString().ToLower() == "false")
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 0;
                                }
                                else
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = Convert.ToInt32(sFieldValue[byFieldCount]);
                                }
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = System.DBNull.Value;
                            }
                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "3")// datetime
                        {
                            if (string.IsNullOrEmpty(sFieldValue[byFieldCount]))
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.DateTime)).Value = System.DBNull.Value;
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.DateTime)).Value = Convert.ToDateTime(sFieldValue[byFieldCount]);
                            }

                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "4")//int64
                        {
                            if (sFieldValue[byFieldCount].ToString() != string.Empty)
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.BigInt)).Value = Convert.ToInt64(sFieldValue[byFieldCount]);
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.BigInt)).Value = System.DBNull.Value;
                            }
                        }




                    }

                    if (dbconn.State == ConnectionState.Open)
                    {
                        dbconn.Close();
                    }

                    dbconn.Open();

                    int result = 0;
                    //comm.CommandTimeout = 600;
                    result = comm.ExecuteNonQuery();
                    dbconn.Close();
                    comm.Dispose();
                    return (UInt32)result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("insertData({0})|error:", sTableName, ex.ToString()), ex);
                dbconn.Close();
                if (comm != null) comm.Dispose();

                if (!File.Exists(cGlobalSettings.GetDBPath()))
                {
                    cWin32APIs.PostMessage((IntPtr)cWin32APIs.HWND_BROADCAST, cWin32APIs.WM_CALLSIGNOUT, IntPtr.Zero, IntPtr.Zero);
                }
                return 0;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        }


        public void insertData(string sTableName, string[] sFieldName, byte[] sFieldType, object[] sFieldValue, string sMasterid, ref uint SpyId, bool getMaxId)
        {
            string sSql = "";
            string values = "";
            string fNames = "";
            ds = new DataSet();
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                if (getMaxId)
                {
                    fNames = sMasterid + ",";
                    values = "@@" + sMasterid + ",";
                }

                for (int byFieldCount = 0; byFieldCount < sFieldValue.Length; byFieldCount++)
                {

                    if (byFieldCount != sFieldValue.Length - 1)
                    {
                        fNames += Convert.ToString(sFieldName[byFieldCount]) + ",";
                        values += "@" + Convert.ToString(sFieldName[byFieldCount]) + ",";
                    }
                    else
                    {

                        fNames += Convert.ToString(sFieldName[byFieldCount]);// +"," + sMasterid;
                        values += "@" + Convert.ToString(sFieldName[byFieldCount]);// +",@" + sMasterid;

                    }
                }
                if (values != "" && values != null && sTableName != "")
                {

                    if (getMaxId)
                        sSql = "declare @@" + sMasterid + " int;select @@" + sMasterid + " =isnull(max(" + sMasterid + "),0)+ 1 from " + sTableName + ";";

                    sSql += "insert into "
                        //  + sTableName + " (" + fNames + ") OUTPUT Inserted." + sMasterid + " values (" + values + ")";
                        + sTableName + " (" + fNames + ") values (" + values + "); SELECT last_insert_rowid()";
                    comm = dbconn.CreateCommand();
                    comm.CommandText = sSql;
                    comm.CommandTimeout = 0;
                    for (byte byFieldCount = 0; byFieldCount < sFieldValue.Length; byFieldCount++)
                    {

                        if (Convert.ToString(sFieldType[byFieldCount]) == "1")//varchar
                        {
                            comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.VarChar), 5000).Value = Convert.ToString(sFieldValue[byFieldCount]);
                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "2")//int
                        {
                            if (sFieldValue[byFieldCount].ToString() != string.Empty)
                            {
                                if (sFieldValue[byFieldCount].ToString().ToLower() == "true")
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 1;
                                }
                                else if (sFieldValue[byFieldCount].ToString().ToLower() == "false")
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 0;
                                }
                                else
                                {
                                    comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = Convert.ToInt32(sFieldValue[byFieldCount]);
                                }
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Int)).Value = System.DBNull.Value;
                            }
                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "3")// datetime
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(sFieldValue[byFieldCount])))
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.DateTime)).Value = System.DBNull.Value;
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.DateTime)).Value = Convert.ToDateTime(sFieldValue[byFieldCount]);
                            }

                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "4")//int64
                        {
                            if (sFieldValue[byFieldCount].ToString() != string.Empty)
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.BigInt)).Value = Convert.ToInt64(sFieldValue[byFieldCount]);
                            }
                            else
                            {
                                comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.BigInt)).Value = System.DBNull.Value;
                            }
                        }
                        else if (Convert.ToString(sFieldType[byFieldCount]) == "5")//blob
                        {
                            comm.Parameters.Add("@" + Convert.ToString(sFieldName[byFieldCount]), (DbType)(SqlDbType.Binary)).Value = (byte[])(sFieldValue[byFieldCount]);
                        }


                    }
                    //  uint maxid = getMax(sTableName, sMasterid) + 1;
                    // comm.Parameters.Add("@" + sMasterid, SqlDbType.Int).Value = Convert.ToInt32(maxid);

                    if (dbconn.State != ConnectionState.Closed)
                    {
                        dbconn.Close();
                    }
                    dbconn.Open();

                    // int result = 0;
                    //result = comm.ExecuteNonQuery();
                    ada = new SQLiteDataAdapter(comm);
                    ada.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // if (SpyId == 1)// if function is called for retrieving Main SpywareMasterId 1 means spyware_master table call
                        {
                            SpyId = Convert.ToUInt32(ds.Tables[0].Rows[0][0]);
                        }
                    }
                    ds.Dispose();

                }
                else
                {
                    //return 0;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("insertData with output ({0})|error:", sTableName, ex.ToString()), ex);
                dbconn.Close();
                if (comm != null) comm.Dispose();

                if (!File.Exists(cGlobalSettings.GetDBPath()))
                {
                    cWin32APIs.PostMessage((IntPtr)cWin32APIs.HWND_BROADCAST, cWin32APIs.WM_CALLSIGNOUT, IntPtr.Zero, IntPtr.Zero);
                }
                //  return 0;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }

        }


        public Boolean doesTableExist(String tableName)
        {
            bool bRes = false;
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sDbFilePath = dbconn.ConnectionString.ToLower();
                if (sDbFilePath.IndexOf("data source=") != -1)
                {
                    sDbFilePath = sDbFilePath.Substring(sDbFilePath.IndexOf("data source=") + "data source=".Length);
                    sDbFilePath = sDbFilePath.Substring(0, sDbFilePath.IndexOf(";"));
                    if (!File.Exists(sDbFilePath)) return false;
                }

                // if (!File.Exists(dbconn.DataSource)) return false;
                using (SQLiteCommand cmd = new SQLiteCommand(dbconn))
                {
                    if (dbconn.State == ConnectionState.Open)
                    {
                        dbconn.Close();
                    }

                    dbconn.Open();
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE lower(name)='" + tableName.ToLower() + "'";
                    SQLiteDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        bRes = true;
                    }
                    else
                    {
                        bRes = false;
                    }
                    rdr.Dispose();
                    dbconn.Close();

                    cmd.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException(string.Format("doesTableExist({0})|error:", tableName, ex.Message), ex);
                bRes = false;
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
            return bRes;
        }



        public UInt32 updateData(string sTableName, string[] aFieldNames, byte[] aFieldTypes, object[] aFieldValues, string sWhere, string[] aWhereFieldNames, byte[] aWhereFieldTypes, object[] aWhereFieldValues)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }


                string sSql = "";
                string sUpdateFieldNames = "";

                for (byte byFieldCount = 0; byFieldCount < aFieldValues.Length; byFieldCount++)
                {
                    if (byFieldCount != aFieldValues.Length - 1)
                    {
                        sUpdateFieldNames += aFieldNames[byFieldCount] + " = @" + aFieldNames[byFieldCount] + ",";
                    }
                    else
                    {
                        sUpdateFieldNames += aFieldNames[byFieldCount] + " = @" + aFieldNames[byFieldCount];

                    }
                }
                sSql = "UPDATE " + sTableName + " set " + sUpdateFieldNames + " where " + sWhere;
                comm = dbconn.CreateCommand();
                comm.CommandText = sSql;

                // UPDATE query updatable value parameters added
                for (byte byFieldCount = 0; byFieldCount < aFieldValues.Length; byFieldCount++)
                {

                    if (aFieldTypes[byFieldCount] == (byte)DataType.String)
                    {
                        comm.Parameters.Add("@" + aFieldNames[byFieldCount], (DbType)SqlDbType.VarChar).Value = Convert.ToString(aFieldValues[byFieldCount]);

                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.Number)
                    {
                        if (aFieldValues[byFieldCount].ToString().ToLower() == "true")
                        {
                            comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 1;
                        }
                        else if (aFieldValues[byFieldCount].ToString().ToLower() == "false")
                        {
                            comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)(SqlDbType.Int)).Value = 0;
                        }
                        else
                        {
                            comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)(SqlDbType.Int)).Value = Convert.ToInt32(aFieldValues[byFieldCount]);
                        }

                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.DateTime)
                    {
                        comm.Parameters.Add("@" + aFieldNames[byFieldCount], (DbType)SqlDbType.DateTime).Value = Convert.ToDateTime(aFieldValues[byFieldCount]);

                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.Long)
                    {
                        comm.Parameters.Add("@" + aFieldNames[byFieldCount], (DbType)SqlDbType.BigInt).Value = Convert.ToInt64(aFieldValues[byFieldCount]);

                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.Binary)
                    {
                        comm.Parameters.Add("@" + aFieldNames[byFieldCount], (DbType)SqlDbType.Binary).Value = (byte[])(aFieldValues[byFieldCount]);

                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.Real)
                    {
                        comm.Parameters.Add("@" + aFieldNames[byFieldCount], (DbType)SqlDbType.Real).Value = Convert.ToDouble(aFieldValues[byFieldCount]);

                    }
                }

                // UPDATE query where parameters added
                for (byte byFieldCount = 0; byFieldCount < aWhereFieldValues.Length; byFieldCount++)
                {

                    if (aWhereFieldTypes[byFieldCount] == (byte)DataType.String)
                    {
                        comm.Parameters.Add("@" + aWhereFieldNames[byFieldCount], (DbType)SqlDbType.VarChar).Value = aWhereFieldValues[byFieldCount];

                    }
                    else if (aWhereFieldTypes[byFieldCount] == (byte)DataType.Number)
                    {
                        comm.Parameters.Add("@" + aWhereFieldNames[byFieldCount], (DbType)(SqlDbType.Int)).Value = aWhereFieldValues[byFieldCount];

                    }
                    else if (aWhereFieldTypes[byFieldCount] == (byte)DataType.DateTime)
                    {
                        comm.Parameters.Add("@" + aWhereFieldNames[byFieldCount], (DbType)SqlDbType.DateTime).Value = aWhereFieldValues[byFieldCount];

                    }
                    else if (aWhereFieldTypes[byFieldCount] == (byte)DataType.Long)
                    {
                        comm.Parameters.Add("@" + aWhereFieldNames[byFieldCount], (DbType)SqlDbType.BigInt).Value = Convert.ToInt64(aWhereFieldValues[byFieldCount]);

                    }
                    else if (aWhereFieldTypes[byFieldCount] == (byte)DataType.Real)
                    {
                        comm.Parameters.Add("@" + aWhereFieldNames[byFieldCount], (DbType)SqlDbType.Real).Value = Convert.ToDouble(aWhereFieldValues[byFieldCount]);

                    }
                }

                if (sTableName != "")
                {
                    if (dbconn.State != ConnectionState.Closed)
                    {
                        dbconn.Close();
                    }
                    dbconn.Open();
                    int result = 0;
                    result = comm.ExecuteNonQuery();
                    dbconn.Close();

                    return (UInt32)result;

                }
                else
                {
                    return 0;
                }
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        }

        public UInt32 deleteData(string sTableName, string sWhere, string[] aFieldNames, byte[] aFieldTypes, string[] aFieldValues)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                string sSql = "";
                sSql = "DELETE from " + sTableName + " where " + sWhere;
                comm = dbconn.CreateCommand();
                comm.CommandText = sSql;
                for (byte byFieldCount = 0; byFieldCount < aFieldValues.Length; byFieldCount++)
                {
                    if (aFieldTypes[byFieldCount] == (byte)DataType.String)//varchar
                    {
                        comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)SqlDbType.VarChar, 4000).Value = Convert.ToString(aFieldValues[byFieldCount]);
                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.Number)//int
                    {
                        comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)SqlDbType.Int).Value = Convert.ToInt32(aFieldValues[byFieldCount]);
                    }
                    else if (aFieldTypes[byFieldCount] == (byte)DataType.DateTime)// datetime
                    {
                        comm.Parameters.Add("@" + Convert.ToString(aFieldNames[byFieldCount]), (DbType)SqlDbType.DateTime).Value = Convert.ToDateTime(aFieldValues[byFieldCount]);
                    }
                }

                if (sTableName != "")
                {
                    if (dbconn.State != ConnectionState.Closed)
                    {
                        dbconn.Close();
                    }
                    dbconn.Open();
                    int result = 0;
                    result = comm.ExecuteNonQuery();
                    dbconn.Close();
                    return (UInt32)result;
                }
                else
                {
                    return 0;
                }
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        } // End of Function
        ////////

        public UInt32 ExecuteQuery(string sQuery)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                if (sQuery != "")
                {
                    comm = dbconn.CreateCommand();
                    comm.CommandText = sQuery;
                    //comm.CommandTimeout = 0;
                    if (dbconn.State != ConnectionState.Closed)
                    {
                        dbconn.Close();
                    }
                    dbconn.Open();
                    int result = 0;
                    result = comm.ExecuteNonQuery();
                    dbconn.Close();
                    comm.Dispose();
                    return (UInt32)result;
                }
                else
                {
                    return 0;
                }
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        } // End of Function

        public UInt32 DropTable(string tableName)
        {
            try
            {
                sqlLiteLock.EnterWriteLock();

                if (waitHandle != null)
                {
                    waitHandle.WaitOne(/*10000*/); // wait for 10 seconds
                    waitHandle.Reset();
                }

                if (tableName != "")
                {
                    comm = dbconn.CreateCommand();
                    comm.CommandText = "DROP TABLE if exists '" + tableName + "';";
                    //comm.CommandTimeout = 0;
                    if (dbconn.State != ConnectionState.Closed)
                    {
                        dbconn.Close();
                    }
                    dbconn.Open();
                    int result = 0;
                    result = comm.ExecuteNonQuery();
                    dbconn.Close();
                    comm.Dispose();
                    return (UInt32)result;
                }
                else
                {
                    return 0;
                }
            }
            finally
            {
                if (waitHandle != null) waitHandle.Set();

                sqlLiteLock.SafeExitWriteLock();
            }
        } // End of Function


        // Returns date time in format YYYY-MM-DD HH:MM:SS 
        public DateTime getDateTime()
        {
            ds = SelectData("SELECT Convert(varchar,getdate(), 120) As Date");
            return Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"]);
        }

        // Returns date time in SPECIFIED format
        public DateTime getDateTime(byte byDateFormat)
        {
            try
            {
                ds = SelectData("SELECT Convert(varchar,getdate(), " + byDateFormat + ") As Date");
                return Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"]);
            }
            catch (SQLiteException)
            {
                //oErrorHandle.ShowDatabaseMessageBox(ErrorHandle.DatabaseOperation.InvalidDateFormat);
                return DateTime.Now;
            }
        }


        public void CloseDbConnection()
        {
            try
            {
                if (dbconn != null)
                {
                    dbconn.Close();
                    dbconn.Dispose();
                }

                if (oConnection != null)
                {
                    oConnection.Close();
                    oConnection.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CloseDbConnection", ex);
            }
            finally
            {
                dbconn = null;
                oConnection = null;
            }

        }


    }

    public struct SQLiteDbType
    {
        public const string INTEGER = " INTEGER ";
        public const string NUMERIC = " REAL ";
        public const string BOOLEAN = " BOOLEAN ";
        public const string DATE = " DATE ";
        public const string TEXT = " TEXT ";
    }
}
