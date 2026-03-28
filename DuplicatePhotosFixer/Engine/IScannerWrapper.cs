using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using DuplicatePhotosFixer;
using DuplicatePhotosFixer.ClassDictionary;

namespace DuplicatePhotosFixer
{
    public delegate void OnScanStartProcess(string message, int percentageCompleted, int counter);
    public delegate void OnScanProgressProcess(cClientEnum.eScanPhase ScanPhase, string startMsg, int percentageCompleted, int TotalFiles, int TotalScanned, int TotalDuplicates, string AdditionalInfo = "", bool showScanBar = false);
    public delegate void OnScanCompletedProcess(string messageHeader, string messageDesc, bool bMoveToResultsTab, int percentageCompleted);
    public delegate void OnAuthSuccessProcessEvent();

    public delegate void Callback(ref List<csImageFileInfo> data, bool bInsertRemaining);

    public abstract class IScannerWrapper : IDisposable
    {


        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string redirectUri { get; set; }

        public bool bIsStop { get; set; }

        public int TotalItems { get; set; }

        //int Percentage { get; }

        //public int TotalItems { get; set; }
        public int TotalProcessed = 0;
        public int Percentage
        {
            get
            {
                return (int)(((float)TotalProcessed / (float)TotalItems) * 100);
            }
        }

        public abstract bool Scan
           (
           List<csScanPaths> inputToScan,
           object filters,
           System.IO.SearchOption searchOption,
           ref HashSet<csImageFileInfo> list,
           Callback callback,
           int PauseCounter
           );

        public abstract bool AuthCode(
            string clientId,
            string clientSecret,
            string redirectUri,
            Callback callback
            );

        public abstract bool Browse(out string[] SelectedPaths);


        public abstract bool ReadAllMessages(
            object client,
            object folder,
            ref HashSet<csImageFileInfo> emails,
            Callback callback,
            int PauseCounter,
            string directoryPath
            );

        public abstract bool ProcessMessage(
            object client,
            object folder,
            object mail,
            //bool bFetchUnReadMailsOnly,
            ref object record,
            //ref HashSet<object> emails,
            Callback callback,
            int PauseCounter,
            string directoryPath
            );

        public abstract bool Delete(
             object client,
              List<object> list
            //string fileId
            );

        public abstract string FormatProgressMessage(object folder);

        public event OnScanStartProcess OnScanStart;
        public event OnScanProgressProcess OnScanProgress;
        public event OnScanCompletedProcess OnScanCompleted;

        public event OnAuthSuccessProcessEvent OnAuthSuccess;

        public virtual void SendProgress(string message)
        {
            if (!string.IsNullOrEmpty(message)) cGlobalSettings.oLogger.WriteLogVerbose(message);
            OnScanStartProcess(message, Percentage, TotalProcessed);
        }

        protected virtual void OnScanStartProcess(string message, int percentageCompleted, int counter)
        {
            OnScanStart?.Invoke(message, percentageCompleted, counter);

        }


        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-raise-base-class-events-in-derived-classes
        /// </summary>
        /// <param name="message"></param>
        /// <param name="percentageCompleted"></param>
        /// <param name="counter"></param>
        protected virtual void OnScanProgressProcess(cClientEnum.eScanPhase ScanPhase, string startMsg, int percentageCompleted, int TotalFiles, int TotalScanned, int TotalDuplicates, string AdditionalInfo = "", bool showScanBar = false)
        {
            // Safely raise the event for all subscribers
            OnScanProgress?.Invoke(ScanPhase, startMsg, percentageCompleted, TotalFiles, TotalScanned, TotalDuplicates, AdditionalInfo = "", showScanBar = false);
        }

        protected virtual void OnScanCompletedProcess(string messageHeader, string messageDesc, bool bMoveToResultsTab, int percentageCompleted)
        {
            OnScanCompleted?.Invoke(messageHeader, messageDesc, bMoveToResultsTab, percentageCompleted);
        }


        protected virtual void OnAuthSuccessProcessEvent()
        {
            OnAuthSuccess?.Invoke();
        }

        public virtual void Dispose()
        {

        }
    }
}
