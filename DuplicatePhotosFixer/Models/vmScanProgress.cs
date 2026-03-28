using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading; // ADD THIS
using DuplicatePhotosFixer.Engine;

namespace DuplicatePhotosFixer.Models
{
    public class vmScanProgress : INotifyPropertyChanged // ADD INotifyPropertyChanged
    {
        #region properties

        private int _initProgressPercentage { get; set; }
        public int initProgressPercentage
        {
            get { return _initProgressPercentage; }
            set
            {
                _initProgressPercentage = value;
                OnPropertyChanged("initProgressPercentage");
               
                OnPropertyChanged("ProgressText");    
                OnPropertyChanged("ProgressValue");
            }
        }

        


public string initProgressPercentageDisplay
{
    get { return _initProgressPercentage + "%"; }
}

private string _txtFilesProcessedCount { get; set; }
        public string txtFilesProcessedCount
        {
            get { return _txtFilesProcessedCount; }
            set
            {
                _txtFilesProcessedCount = value;
                OnPropertyChanged("txtFilesProcessedCount");
            }
        }

        private string _txtFilesDuplicateCount { get; set; }
        public string txtFilesDuplicateCount
        {
            get { return _txtFilesDuplicateCount; }
            set
            {
                _txtFilesDuplicateCount = value;
                OnPropertyChanged("txtFilesDuplicateCount");
            }
        }

        private string _txtCurrentFile { get; set; }
        public string txtCurrentFile
        {
            get { return _txtCurrentFile; }
            set
            {
                _txtCurrentFile = value;
                OnPropertyChanged("txtCurrentFile");
            }
        }

        private string _txtHeader { get; set; }
        public string txtHeader
        {
            get { return _txtHeader; }
            set
            {
                _txtHeader = value;
                OnPropertyChanged("txtHeader");
            }
        }

        private string _txtSubHeader { get; set; }
        public string txtSubHeader
        {
            get { return _txtSubHeader; }
            set
            {
                _txtSubHeader = value;
                OnPropertyChanged("txtSubHeader");
            }
        }


        private string _ScanType { get; set; }
        public string ScanType
        {
            get { return _ScanType; }
            set
            {
                _ScanType = value;
                OnPropertyChanged("ScanType");
            }
        }
        private string _txtFilesProcessedLabel { get; set; }
        public string txtFilesProcessedLabel
        {
            get { return _txtFilesProcessedLabel; }
            set
            {
                _txtFilesProcessedLabel = value;
                OnPropertyChanged("txtFilesProcessedLabel");
            }
        }

        private string _txtFilesDuplicateLabel { get; set; }
        public string txtFilesDuplicateLabel
        {
            get { return _txtFilesDuplicateLabel; }
            set
            {
                _txtFilesDuplicateLabel = value;
                OnPropertyChanged("txtFilesDuplicateLabel");
            }
        }
        //ScanType

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public vmScanProgress()
        {
            Init();
        }

        void Init()
        {
            //Initialize with default values
           initProgressPercentage = 0 ;
            txtFilesProcessedCount = "0";
            txtFilesDuplicateCount = "0";
            txtCurrentFile = "";
            txtFilesProcessedLabel = "Remaining Photos";  // Default
            txtFilesDuplicateLabel = "Photos Compared";   // Default
        }

        public void OnScanStart()
        {
            try
            {
                cGlobal.TotalDulicateFilesCount = 0;
                cGlobal.TotalDuplicateFileSize = 0;
                cGlobal.TotalFileSelectedToDelete = 0;
                cGlobal.TotalSpaceSelectedToSave = 0;

                initProgressPercentage = 5;
                txtFilesProcessedCount = "0";
                txtFilesDuplicateCount = "0";
                txtCurrentFile = "";

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmScanProgress :: OnScanStart : ", ex);
            }
        }

        // Make this thread-safe by dispatching to UI thread
        public void OnScanProgress(cClientEnum.eScanPhase ScanPhase, string strMsg, int percentageCompleted, int nTotalFiles, int nTotalProcess, int nTotalDups)
        {
            try
            {
                SetStrings(ScanPhase);

                if (!string.IsNullOrEmpty(strMsg))
                {
                    txtCurrentFile = strMsg;
                }

                // Update displayed counts based on phase
                if (ScanPhase == cClientEnum.eScanPhase.Search)
                {
                    // Phase 1 (0-5%): Adding photos

                    txtFilesProcessedCount = nTotalFiles.ToString();


                    cGlobalSettings.oLogger?.WriteLogVerbose($"[Phase 1] Files: {nTotalProcess}, Progress: {percentageCompleted}%");
                }
                else if (ScanPhase == cClientEnum.eScanPhase.Read)
                {
                    // Phase 2 (5-50%): Calculating hashes
                    
                    txtFilesProcessedCount = string.Format("{0}/{1}", nTotalProcess, nTotalFiles);
         

                    cGlobalSettings.oLogger?.WriteLogVerbose($"[Phase 2] Processed: {nTotalProcess}/{nTotalFiles}, Progress: {percentageCompleted}%");
                }
                else if (ScanPhase == cClientEnum.eScanPhase.Compare)
                {
                    // Phase 3 (50-100%): Comparing hashes
                   
                    txtFilesDuplicateCount = nTotalProcess.ToString(); 
                   
                    // Photos compared
                   

                    cGlobalSettings.oLogger?.WriteLogVerbose($"[Phase 3] Compared: {nTotalProcess}, Duplicates: {nTotalDups}, Progress: {percentageCompleted}%");
                }
                else
                {
                    if (nTotalProcess > 0)
                        txtFilesProcessedCount = nTotalProcess.ToString();
                    if (nTotalDups >= 0)
                        txtFilesDuplicateCount = nTotalDups.ToString();
                }

                if (percentageCompleted > initProgressPercentage && percentageCompleted <= 100)
                {
                    initProgressPercentage = percentageCompleted;
                    
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmScanProgress :: OnScanProgress : ", ex);
            }
        }

        cClientEnum.eScanPhase LastPhase = cClientEnum.eScanPhase.Other;
        void SetStrings(cClientEnum.eScanPhase ScanPhase)
        {
            if (LastPhase == ScanPhase)
                return;

            switch (ScanPhase)
            {
                case cClientEnum.eScanPhase.Search:
                    ScanType = cResourceManager.LoadString("DPF_ADDING");
                    txtHeader = cResourceManager.LoadString("DPF_SCAN_PHOTOS_ADDING_PHOTOS");
                    txtSubHeader = cResourceManager.LoadString("DPF_SCAN_PHOTOS_PLEASE_BE_PATIENT_MSG");

                    // Update labels for Phase 1
                    txtFilesProcessedLabel = cResourceManager.LoadString("DPF_SCAN_PHOTOS_PHOTOS_ADDED") ;
                    txtFilesDuplicateLabel = cResourceManager.LoadString("DPF_PHOTOS_COMPARED");
                    break;
                case cClientEnum.eScanPhase.Read:
                    {
                        ScanType = cResourceManager.LoadString("IDS_SCANNING");
                        txtHeader = cResourceManager.LoadString("DPF_PROCESSOR_PROGRESS_SEARCHING_DUPLICATE_TEXT");
                        txtSubHeader = cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PLEASE_BE_PATIENT_MSG");
                        // Update labels for Phase 2
                        txtFilesProcessedLabel = cResourceManager.LoadString("DPF_PROCESSED");
                        txtFilesDuplicateLabel = cResourceManager.LoadString("DPF_PHOTOS_COMPARED");
                    }
                    break;
                case cClientEnum.eScanPhase.Compare:
                    
                    ScanType = cResourceManager.LoadString("DPF_COMPARING");
                    txtHeader = cResourceManager.LoadString("DPF_PROCESSOR_COMPARING_DUPLICATES");
                    txtSubHeader = cResourceManager.LoadString("DFP_PROCESSOR_SCAN_PATIENT_WHILE_COMPARING_MSG");


                    // Update labels for Phase 3
                    txtFilesProcessedLabel = cResourceManager.LoadString("DPF_PHOTOS_FOUND");
                    txtFilesDuplicateLabel = cResourceManager.LoadString("DPF_PHOTOS_COMPARED");
                    break;
                case cClientEnum.eScanPhase.ComponentDownload:
                    break;
                case cClientEnum.eScanPhase.Delete:
                    break;
                case cClientEnum.eScanPhase.Other:
                    break;
                default:
                    break;
            }

            LastPhase = ScanPhase;
        }

        public void UpdateProgress(cClientEnum.eScanPhase ScanPhase, string strMsg, int percentageCompleted, int nTotalFiles, int nTotalProcess, int nTotalDups)
        {





            if (!string.IsNullOrEmpty(strMsg))
            {
                txtCurrentFile = strMsg;
            }

            if (nTotalProcess > 0)
            {
                txtFilesProcessedCount = nTotalProcess.ToString();
            }

            if (nTotalDups > 0)
            {
                txtFilesDuplicateCount = nTotalDups.ToString();
            }

            if (percentageCompleted > initProgressPercentage && percentageCompleted <= 100)
            {
                initProgressPercentage = percentageCompleted;
            }
        }

      
        public void OnManualScanStop()
        {
            try
            {
                if (initProgressPercentage >= 99)
                    return;

                Close_Process();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmScanProgress :: OnManualScanStop : ", ex);
            }
        }

        void Close_Process()
        {
            try
            {
                cGlobalSettings.oManualReset.Reset();
                DialogResult result = DialogResult.No;

                if (cGlobalSettings.b_COMPONENT_INSTALL_STARTED)
                    App.oMainReference.showMessage(cResourceManager.LoadString("IDS_WIAT_INSTALL_COMPONENT"), "");
                else
                    result = App.oMainReference.showYesNoMessage(cResourceManager.LoadString("IDS_STOP_SCAN_DES"));

                cGlobalSettings.oManualReset.Set();

                if (result != DialogResult.Yes)
                {
                    return;
                }
                cGlobalSettings.abortNow = true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmScanProgress :: Close_Process : ", ex);
            }
        }
    }
}
