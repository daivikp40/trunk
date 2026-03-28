using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuplicatePhotosFixer.HelperClasses.MyLogger;

namespace DuplicatePhotosFixer.TrialProtection
{
    public static class Logger
    {
        static Logger()
        {
            InitLogger();
        }

        public static bool WriteLine(string format, params object[] args)
        {
            return oLogger.WriteLogVerbose(format, args);
        }


        public static bool WriteException(string message, System.Exception ex, bool bWriteFewDetailsOfException = false)
        {
            return oLogger.WriteLogException(message, ex, bWriteFewDetailsOfException);
        }

        public static string LogPath = /*Application.StartupPath;*/PathEx.Combine(getCommonAppDataApplicationPath()/*Application.StartupPath*/, @"oLogs");
        public static string LogFileName = string.Format(/*"log.txt"*/ "{0}_act_log.txt", System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
        public static cLogger oLogger = null;
        public static void InitLogger() { oLogger = new cLogger(LogPath, LogFileName, System.Reflection.Assembly.GetEntryAssembly().GetName().Name); }


        public static string LogPathCorrupt = /*Application.StartupPath;*/PathEx.Combine(getCommonAppDataApplicationPath()/*Application.StartupPath*/, @"oLogs");
        public static string LogFileNameCorrupt = string.Format(/*"log.txt"*/ "{0}_act_Corruptlog.txt", System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
        public static cLogger oLoggerCorrupt = null;
        public static void InitLoggerCorrupt() { oLoggerCorrupt = new cLogger(LogPathCorrupt, LogFileNameCorrupt, System.Reflection.Assembly.GetEntryAssembly().GetName().Name); }

        public static string LogPathOutOfMemory = /*Application.StartupPath;*/PathEx.Combine(getCommonAppDataApplicationPath()/*Application.StartupPath*/, @"oLogs");
        public static string LogFileNameOutOfMemory = string.Format(/*"log.txt"*/ "{0}_act_OutOfMemorylog.txt", System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
        public static cLogger oLoggerOutOfMemory = null;
        public static void InitLoggerOutOfMemory() { oLoggerOutOfMemory = new cLogger(LogPathOutOfMemory, LogFileNameOutOfMemory, System.Reflection.Assembly.GetEntryAssembly().GetName().Name); }

        public static string getCommonAppDataApplicationPath()
        {
            string sPath = Application.UserAppDataPath;
            sPath = sPath.Substring(0, sPath.LastIndexOf("\\"));
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            return sPath;
        }

    }
}
