using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer;

namespace MyLogger
{
    ///<summary>
    ///for manging error logging functionality
    ///</summary>
    public class cLogger : IDisposable
    {


        /// <summary>
        /// BackupgroundThread
        /// </summary>
        //public BackupgroundThread bkThread = null;


        // private string m_LogFileName;
        private string m_LogPath;
        private bool m_Logging;
        private string m_Area;
        private string m_ClassName;
        private string m_FunctionName;
        private string _sLogFilePathName;
        public string sLogFilePathName
        {
            get { return _sLogFilePathName; }
            set
            {
                _sLogFilePathName = value;
                sLogFilePathDirectory = Path.GetDirectoryName(_sLogFilePathName);
                mutex_id = mutex_id_initials + Path.GetFileNameWithoutExtension(value);
            }
        }

        static string mutex_id_initials = "Global\\cbackuplogmutex";
        static string mutex_id;

        private string sLogFilePathDirectory;
        private string sLogFileNameWithOutExtension { get; set; }
        private StreamWriter oStreamWriter;
        public bool bVerboseLoggingEnabled = true;

        ///<summary>
        ///get set LogFileName field
        ///</summary>
        //         public string LogFileName
        //         {
        //             get
        //             {
        //                 return m_LogFileName;
        //             }
        //             set
        //             {
        //                 m_LogFileName = value;
        //             }
        //         }

        ///<summary>
        ///get set LogPath field
        ///</summary>
        public string LogPath
        {
            get
            {
                return m_LogPath;
            }
            set
            {
                m_LogPath = value;
                if (m_LogPath[m_LogPath.Length - 1].ToString() != @"\") m_LogPath = m_LogPath + @"\";
            }
        }

        ///<summary>
        ///get set Logging field
        ///</summary>
        public bool Logging
        {
            get
            {
                return m_Logging;
            }
            set
            {
                m_Logging = value;
            }
        }

        ///<summary>
        ///get set Area field
        ///</summary>
        public string Area
        {
            get
            {
                return m_Area;
            }
            set
            {
                m_Area = value;
            }
        }

        ///<summary>
        ///get set ClassName field
        ///</summary>
        public string ClassName
        {
            get
            {
                return m_ClassName;
            }
            set
            {
                m_ClassName = value;
            }
        }

        ///<summary>
        ///get set FunctionName field
        ///</summary>
        public string FunctionName
        {
            get
            {
                return m_FunctionName;
            }
            set
            {
                m_FunctionName = value;
            }
        }


        ///<summary>
        ///constructor
        ///</summary>
        public cLogger(string LogPath, string LogFileName, string LogFileNameWithOutExtension)
        {
            try
            {
                //readerWriterLockSlim = new ReaderWriterLockSlim();
                //m_Logging = false;
                m_Logging = true;
                m_LogPath = LogPath;
                sLogFilePathName = PathEx.Combine(m_LogPath, LogFileName); //+ DateTime.Now.ToString("dd-MM-yyyy") + "_" + m_LogFileName;
                sLogFileNameWithOutExtension = LogFileNameWithOutExtension;
                if (!Directory.Exists(m_LogPath))
                {
                    Directory.CreateDirectory(m_LogPath);
                }
                if (!File.Exists(sLogFilePathName))
                {
                    FileStream fs = new FileStream(sLogFilePathName, FileMode.CreateNew, FileAccess.Write);
                    fs.Close();
                    fs.Dispose();
                }
                //FileInfo fi = new FileInfo(sLogFilePathName);
                //long fileLength = NativeMethodsExtensions.GetFileSize(sLogFilePathName);
                //if (fileLength/*fi.Length*/ > (64 * 1024 * 1024))
                //{
                //    FileStream fs = new FileStream(sLogFilePathName, FileMode.Open, FileAccess.ReadWrite);
                //    fs.Position = 48 * 1024 * 1024;
                //    fs.SetLength(16 * 1024 * 1024); //tocheck whether log is delted from top or bottom
                //    fs.Close();
                //    fs.Dispose();
                //}

                //bkThread = new BackupgroundThread(LogFileName);

                //m_LogFileName = cGlobal.LogFileName;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cLogger ::", ex);
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.Message);
#endif

                // throw;
            }
        }
        ~cLogger()
        {
            Dispose(false);
        }

        public bool WriteLogVerbose(string format, params object[] args)
        {
            if (bVerboseLoggingEnabled)
            {
                try
                {
                    /*bkThread.InvokeInBackground(() => */
                    l_WriteLog(string.Format(format, args))/*)*/;
                    //return WriteLogVerbose(string.Format(format, args));
                }
                catch (System.Exception)
                {
                    ;
                }
                
            }
            return true;
        }

#if DEBUG
        const int MaxCount = 0;
#else
        const int MaxCount = 64;
#endif

        static int WriteCounter = 0;
        List<string> LogData = new List<string>();
        /// <summary>
        /// this is to write the verbose log of DLL
        /// </summary>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        public bool WriteLogVerbose(string sMessage, bool bEncypted=false, bool bForceWrite = true)
        {
            try
            {
                if (bVerboseLoggingEnabled)
                {

                    if (bForceWrite || (MaxCount <= 0) || (WriteCounter++ > MaxCount))
                    {
                        WriteCounter = 0;
                        if (LogData.Count > 1)
                        {
                            LogData.Add(sMessage);
                            LogData.ForEach(x => WriteLog(x, bEncypted));
                            LogData.Clear();
                        }
                        else
                        {
                            return WriteLog(sMessage, bEncypted);
                        }

                    }
                    else
                    {
                        LogData.Add(sMessage);
                    }
                }
            }
            catch (System.Exception ex)
            {
                
            }
           
            return true;
        }

        private static string WhoCalledMe()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(4);
                MethodBase methodBase = stackFrame.GetMethod();
                Console.WriteLine(" Parent Method Name {0} ", methodBase.Name);
                return string.Format("MethodName={0}; FileName={1};LineNumber={2};FileColumnNumber={3};", methodBase.Name, stackFrame.GetFileName(), stackFrame.GetFileLineNumber(), stackFrame.GetFileColumnNumber());
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }

        private static string GetStackTrace()
        {
            StringBuilder sb = new StringBuilder();
            // I WILL LOG THE EXCEPTION object "EX" here ! but ex.StackTrace is truncated!
            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);
                sb.AppendLine(sf.ToString().Replace("\r\n", "")/* + "Params:"+sf.GetMethod().GetParameters()*//*string.Format("MethodName={0}; FileName={1};LineNumber={2};FileColumnNumber={3};", sf.GetMethod(), sf.GetFileName(), sf.GetFileLineNumber(), sf.GetFileColumnNumber())*/);
            }

            return sb.ToString();
        }

        public bool WriteLogException(string message, System.Exception ex, bool bWriteFewDetailsOfException = false)
        {
            try
            {
#if DEBUG

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                if (bWriteFewDetailsOfException)
                {
                    return WriteLog(string.Format("Exception: Message:{0}\text:{1}, Exception Line number:{2}", message, ex.Message.ToString(), line/*, st*/));
                }
                else
                {
                    //WriteLog(string.Format("Exception: ex.StackTrace:{0}", ex.StackTrace));
                    //WriteLog("");
                    //WriteLog(string.Format("Exception: Environment.StackTrace:{0}", Environment.StackTrace));
                    //WriteLog("");
                    //WriteLog(string.Format("Exception: Full-StackTrace:{0}", GetStackTrace()));
                    //WriteLog("");

                    return WriteLog(string.Format("Exception: Message:{0}\text:{1}, Exception Line number:{2}, StackTrace:{3}", message, ex.ToString(), line, st));
                }
#else
                if(bWriteFewDetailsOfException)
                    return WriteLog(string.Format("Exception: Message:{0}\text:{1}", message, ex.Message));
                else
                    return WriteLog(string.Format("Exception: Message:{0}\text:{1}", message, ex.ToString()));

#endif
            }
            catch (System.Exception ex1)
            {
                //Trace.WriteLine(ex1.Message);
                return false;
            }

        }

        public bool WriteLog(string sMessage, bool bEncypted=false)
        {
            /*bkThread.InvokeInBackground(()=>*/
            l_WriteLog(sMessage,  bEncypted)/*)*/;

            return true;
        }

        public bool WriteLogWithNoBkThread(string sMessage)
        {
            l_WriteLog(sMessage);

            return true;
        }

        public string GetLogFileNewName()
        {
            if (string.IsNullOrEmpty(sLogFilePathDirectory) || string.IsNullOrEmpty(sLogFileNameWithOutExtension))
            {
                return string.Empty;
            }

            return PathEx.Combine(sLogFilePathDirectory, sLogFileNameWithOutExtension + DateTime.Now.ToString("_ddMMyyyy_hhmmss_fff") + ".txt");
        }

        public static string DateTimePattern = GetDateTimePattern();

        /// <summary>
        /// Reference:
        /// http://msdn.microsoft.com/en-us/library/bb882581(v=vs.110).aspx
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimePattern()
        {
            try
            {

                string LongTimePattern = DateTimeFormatInfo.CurrentInfo.LongTimePattern;
                LongTimePattern = Regex.Replace(LongTimePattern, "(:ss|:s)", "$1.fff");
                
                string DateTimePattern = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
                    + " "
                    + LongTimePattern;

                return DateTimePattern;
            }
            catch (System.Exception ex)
            {
                //cGlobalSettings.oLogger.WriteLogException("Logger::GetDateTimePattern", ex);

                return string.Empty;
            }            
            
        }

        public static ReaderWriterLockSlim readerWriterLogLockSlim = new ReaderWriterLockSlim();


        ///<summary>
        ///write log using default filepath name
        ///</summary>
        ///<param name="sMessage">message to be written</param>
        public bool l_WriteLog(string sMessage, bool bEncypted=false)
        {
            try
            {
                readerWriterLogLockSlim.EnterWriteLock();

#if DEBUG
                System.Diagnostics.Debug.WriteLine(string.Format("{0}|{1}", DateTime.Now.ToLocalTime(), sMessage));
#endif


                if (string.IsNullOrEmpty(sLogFilePathDirectory) || string.IsNullOrEmpty(sLogFilePathName))
                {
                    return false;
                }

                bool bResult = true;

                try
                {
                    if (!m_Logging) return true;


                    if (!Directory.Exists(sLogFilePathDirectory)) Directory.CreateDirectory(sLogFilePathDirectory);

                    if (File.Exists(sLogFilePathName))
                    {
                        long fileLength = NativeMethodsExtensions.GetFileSize(sLogFilePathName);
                        if (fileLength > (20 * 1024 * 1024)) // MAX SIZE OF A SINGLE LOG FILE
                        {
                            string sLogFilePathNewName = GetLogFileNewName();

                            // rename old file
                            File.Move(sLogFilePathName, sLogFilePathNewName);
                        }
                    }                    

                    if (string.IsNullOrEmpty(sLogFilePathName)) return false;                    

                    if (!File.Exists(sLogFilePathName))
                    {
                        

                        FileStream fs = new FileStream(sLogFilePathName, FileMode.CreateNew, FileAccess.Write);
                        fs.Close();
                        fs.Dispose();
                    }

                    

                    string content = string.Format("{0}|{2}", (string.IsNullOrEmpty(DateTimePattern) ? DateTime.Now.ToLocalTime().ToString() : DateTime.Now.ToLocalTime().ToString(DateTimePattern/*"dd-MM-yy hh:mm:ss:fff"*/)), "11111"/*cGlobalSettings.nUserMachineId*/, bEncypted ? ("<enc>" + EncryptString(sMessage) + "</enc>") : sMessage);
                    using (var mutex = new Mutex(false, mutex_id))
                    {
                        try
                        {
                            try
                            {
                                if (!mutex.WaitOne(TimeSpan.FromSeconds(30), false))
                                {
                                    //cGlobalSettings.oLogger.WriteLog("cBackupDataXMl::SaveToXML :mutex.WaitOne: ");
                                }
                            }
                            catch (AbandonedMutexException ex)
                            {
                                //cGlobalSettings.oLogger.WriteLogException("cBackupDataXMl::SaveToXML :AbandonedMutexException: ", ex);
                                // Log the fact the mutex was abandoned in another process, it will still get aquired
                            }

                            using (oStreamWriter = new StreamWriter(sLogFilePathName, true, Encoding.UTF8))
                            {
                                oStreamWriter.WriteLine(content);
                                oStreamWriter.Close();
                                oStreamWriter.Dispose();
                            }


                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }

                    //oStreamWriter = File.AppendText(sLogFilePathName);
                    //oStreamWriter.WriteLine(string.Format("{0}|{2}", DateTime.Now.ToLocalTime(), cGlobalSettings.nUserMachineId, bEncypted ? ( "<enc>" +  EncryptString(sMessage) +"</enc>") : sMessage));
                    //oStreamWriter.Close();


                }
                catch //(Exception oEx)
                {
                    bResult = false;
                    //throw;
                }
                return bResult;
            }
            finally
            {
                readerWriterLogLockSlim.SafeExitWriteLock();
            }
        }


        public string EncryptString(string Value)
        {
#if DEBUG
            return Value;
#endif
            StringBuilder res = new StringBuilder(1024);
            char[] ValueChar = Value.ToCharArray();
            for (int i = 0; i < ValueChar.Length; i++)
            {
                res.Append((char)(~(ValueChar[i])));
            }
            return res.ToString();
        }

        public string EncryptAnsiString(string Value)
        {

            StringBuilder res = new StringBuilder(1024);
            char[] ValueChar = Value.ToCharArray();
            for (int i = 0; i < ValueChar.Length; i++)
            {
                res.Append((char)(~(ValueChar[i])));
            }
            return res.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            //bkThread.Stop();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool bDisposing)
        {
            try
            {
                if (oStreamWriter != null)
                {
                    oStreamWriter.Dispose();
                    oStreamWriter = null;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }
        #endregion
    }


    public static class PathEx
    {

        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AddBackSlash(this string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path += Path.DirectorySeparatorChar.ToString();

            return path;
        }

        /// <summary>
        /// Add backupSlash in end
        /// 
        /// Path.AltDirectorySeparatorChar=/ 
        /// Path.DirectorySeparatorChar=\ 
        /// Path.PathSeparator=; 
        /// Path.VolumeSeparatorChar=:
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveEndBackSlashIfIsFirstChar(this string path)
        {
            if (path.StartsWith(Path.DirectorySeparatorChar.ToString()))
                path = path.Substring(Path.DirectorySeparatorChar.ToString().Length);

            return path;
        }

        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveEndBackSlash(this string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path = path.Remove(path.Length - 1);

            return path;
        }

        public static string RemoveEndVolumeSeparatorChar(this string path)
        {
            path = path.RemoveEndBackSlash();
            if (path.EndsWith(Path.VolumeSeparatorChar.ToString()))
                path = path.Remove(path.Length - 1);

            return path;
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path) ?? "";
        }

        /// <summary>
        /// return Path.Combine(path1.AddBackSlash(), "??" + path2).Replace("??", "");
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1.AddBackSlash(), path2);
        }


        public static string CombineAndRemoveEndVolumeSeparatorChar(string path1, string path2, bool bCallBase)
        {
            return bCallBase ? Path.Combine(path1.AddBackSlash(), path2) : Path.Combine(path1.AddBackSlash(), path2.RemoveEndVolumeSeparatorChar());
        }

        public static string CombineAndRemoveVolumeSeparatorChar(string path1, string path2, bool bCallBase)
        {
            return bCallBase ? Path.Combine(path1.AddBackSlash(), path2) : Path.Combine(path1.AddBackSlash(), path2.Replace(Path.VolumeSeparatorChar.ToString(), string.Empty).RemoveForwardBackSlashIfIsFirstChar().RemoveEndBackSlashIfIsFirstChar());
        }

        public static string CombinePaths(params object[] paths)
        {
            return Convert.ToString(paths.Aggregate((x, y) => System.IO.Path.Combine(x.ToString().AddBackSlash(), y.ToString())));
        }

        public static bool IsPathEquals(this string path1, string path2)
        {
            return (path1.AddBackSlash().ToLower() == path2.AddBackSlash().ToLower());
        }

        public static string RemoveStringIfStartsWith(this string str, string searchText)
        {
            if (str.StartsWith(searchText))
            {
                str = str.Remove(0, searchText.Length);
            }

            return str;
        }


        public static string CombineEx(string path1, string path2)
        {
            path1 = path1.TrimEnd('\\');
            path2 = path2.TrimStart('\\');
            return string.Format("{0}\\{1}", path1, path2);
        }

        


        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveForwardBackSlashIfIsFirstChar(this string path)
        {
            if (!path.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                path = path.Substring(Path.AltDirectorySeparatorChar.ToString().Length);

            return path;
        }

        public static void SafeExitWriteLock(this ReaderWriterLockSlim LockSlim)
        {
            if (LockSlim != null && LockSlim.IsWriteLockHeld) LockSlim.ExitWriteLock();
        }

        
    }
}

public static class TimeSpanExtensionMethods
{
    public static string GetTimeSpanEx(this DateTime dtStart)
    {
        TimeSpan duration = DateTime.Now.Subtract(dtStart);
        return string.Format("{0}:{1}:{2}:{3}", duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds);
    }

    public static string GetTimeSpanEx(this Stopwatch stopWatch)
    {
        TimeSpan duration = stopWatch.Elapsed;
        return string.Format("{0}:{1}:{2}:{3}", duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds);
    }

    public static string GetTimeSpanWithDays(this Stopwatch stopWatch)
    {
        TimeSpan duration = stopWatch.Elapsed;
        return string.Format("\tDays:\t{0}, \tTime:\t{1}:{2}:{3}:{4}", duration.Days, duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds);
    }
}


public static partial class NativeMethodsExtensions
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SP_DEVINFO_DATA
    {
        public uint cbSize;
        public Guid classGuid;
        public uint devInst;
        public IntPtr reserved;
    }

    /// <summary>
    /// The device type icon information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class SP_CLASSIMAGELIST_DATA
    {
        public int cbSize;
        public string ImageList;
        public ulong Reserved;
    }
    /// <summary>
    /// Calculates the file size using windows API
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static long GetFileSize(string path)
    {
        WIN32_FILE_ATTRIBUTE_DATA lpFileInformation;
        GetFileAttributesExW(path, GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out lpFileInformation);
        return (long)((lpFileInformation.nFileSizeHigh << 0x20) | (lpFileInformation.nFileSizeLow & 0xffffffffL));
    }



    public enum GET_FILEEX_INFO_LEVELS : uint
    {
        GetFileExInfoStandard = 0,
        GetFileExMaxInfoLevel = 1
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WIN32_FILE_ATTRIBUTE_DATA
    {
        public FileAttributes dwFileAttributes;
        internal System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        internal System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool GetFileAttributesExW(string lpFileName, [In, MarshalAs(UnmanagedType.U4)] GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);


}