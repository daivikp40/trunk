using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine.ScanModes.FileSystem;
using DuplicatePhotosFixer.Models;
using DuplicatePhotosFixer.Views;
using DuplicatePhotosFixer.UserControls;
using static DuplicatePhotosFixer.cClientEnum;
using Application = System.Windows.Forms.Application;
using Control = System.Windows.Forms.Control;
using DuplicatePhotosFixer.Nags;
using static DuplicatePhotosFixer.Views.cNagDialogue;
using WebBrowser = System.Windows.Forms.WebBrowser;
using System.Configuration;
using DuplicatePhotosFixer.App_code;
using System.IO;

namespace DuplicatePhotosFixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //properties 

        public bool UiWait = false;
        public MainWindow()
        {

            App.oMainReference = this;
            InitializeComponent();

            oSelect = new ucHomeFooter();
            oHome = new ucHomeMain();
            oHomeComapriosn = new ucHomeComparisonMethod();
            objHomeComparisonModel = new vmHomeComparisonMethod();
          
           

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        public static Views.frmExitNag ofrmExitNag = null;
        public static frmSettings oSettingsfrm = null;
        public static Views.frmSelectionAssistant selectionAssistantfrm = null;
        public static UserControls.ucClearCache clearCachefrm = null;
        public frmRegistrationDPF frmRegistration = null;
        public  Views.frmAboutUs frmAboutUs = null;
        public static UserControls.ucActionCentre ucActionCentre = null;
        //public  UserControls.ucHomeMain oSelectFolder = null;
        public ucHomeFooter oSelect = null;
        public  UserControls.ucHomeMain oHome = null;
        public List<string> AutoMarkPreferences = null;
        public frmFeedBack ofrmFeedBack = null;
        public  ucHomeComparisonMethod oHomeComapriosn = null;

        public static frmAfterPhotosFound ofrmAfterScan = null;

        public  UserControls.ucResultView oResult = null;
        public frmAfterScan oFormAfterScanComplete = null;
       

        public frmProcessing fProgress = null;
        public frmTaskbarNotification oTaskbarNotice = null;
        NotifyIcon notifyMainApp = null;
        public frmYTTutorial objYTTutorial = null;
        //public frmSettings ofrmSettings = null;
        //public frmRegistrationDPF objActivate = null;

        //Settings UserControsl

        public ucGeneral  oGeneralSettings = null;
        public ucSettingsFileFormat oFileFormat = null;
        public ucExcludeSettings oExcludedFormat = null;
        public ucExtraSettings oExtraSettings = null;
        public ucSettingsMove oMoveSettings = null;
        public ucNagSettings oNagSettings = null;
        public ucSettingsFileFormat oFileFormatSettings = null;
        public ucHomeFooter oFooter = null;

        // View Models
        public vmGeneralSettings objGeneral = null;
        public  vmFileFormatSettings objFileFormat = null;
        public vmExcludedFolderSettings objExcluded = null;
        
        public vmMoveSettings objMove = null;
        public vmNagThemeSettings objNag = null;
        public vmFeedBack objFeedBack = null;
        public vmActionCenter objActionCenter = null;
        public ucHomeViewModel objHomeViewModel = null;
        public vmHomeComparisonMethod objHomeComparisonModel = null;
        public vmHomeFooter objHomeFooter = null;
        public vmAboutUs objAboutUsViewModel = null;

        public vmScanProgress objScanProgressViewModel = null;
        public vmAfterScan objAfterScanViewModel = null;

        public bool bIsMaximized = false;

        public List<string> ExcludedFolderList = new List<string>();

        public int servicePack = 0;

        public void Home_startCompare()
        {

        }

        public void ShowResults()
        {
            try
            {
                objHome.Visibility = Visibility.Collapsed;
                App.oMainReference.ShowAfterScanNag();
                objResult.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: LoadResults: ", ex);
            }
        }

        public void ShowHome()
        {
            try
            {
                objHome.Visibility = Visibility.Visible;
                objResult.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: LoadResults: ", ex);
            }
        }

        public void ShowExitNag()
        {
            try
            {
               
                if (ofrmExitNag == null)
                {
                    ofrmExitNag = new Views.frmExitNag();
                }
                Progress_Show(ofrmExitNag, this);
                Progress_Close(ofrmExitNag, this);

            }
            catch(Exception ex)
            {

            }
        }

        public void ShowAssistantSelection()
        {
            try
            {

                if (selectionAssistantfrm == null)
                {
                    selectionAssistantfrm = new Views.frmSelectionAssistant();
                }
                Progress_Show(selectionAssistantfrm, this);
                Progress_Close(selectionAssistantfrm, this);


            }
            catch (Exception ex)
            {

            }
        }
        public void ShowClearCaheDialog()
        {
            try
            {
                if (clearCachefrm == null)
                {
                    clearCachefrm = new UserControls.ucClearCache();
                }
                Progress_Show(clearCachefrm, this);
                Progress_Close(clearCachefrm, this);

            }
            catch(Exception ex)
            {

            }
        }

        public void ShowAfterScanNag()
        {
            try
            {
                if (oFormAfterScanComplete == null)
                {
                    oFormAfterScanComplete = new frmAfterScan();
                }
                Progress_Show(oFormAfterScanComplete, this);
                Progress_Close(oFormAfterScanComplete, this);
            }
            catch (Exception ex)
            {

               
            }
        }

        //public static ReaderWriterLockSlim ShowSelectionAssistantLock = new ReaderWriterLockSlim();
        public void ShowSettings(eSettingsTab SettingsTabSelect)
        {
            try
            {
                //ShowSelectionAssistantLock.EnterWriteLock();
                /*if (preferencesform == null || preferencesform.IsDisposed)
                {
                    preferencesform = new frmPreferences();
                }

                preferencesform.StartPosition = FormStartPosition.CenterParent;
                Progress_Show(preferencesform, true);*/
                if (oSettingsfrm == null || !oSettingsfrm.IsLoaded)
                {
                    oSettingsfrm = new frmSettings();
                    
                }

                oSettingsfrm.SettingsTabSelect = SettingsTabSelect;
                oSettingsfrm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Progress_Show(oSettingsfrm, this);
                Progress_Close(oSettingsfrm, this);

                //preferencesform.ShowDialog(this);

                switch (cGlobal.frmNagReturnCode)
                {
                    case FORM_RETURN_CODES.RESET_DEFAULT:
                        {
                            if (oSettingsfrm.ResetDefaultSettings())
                            {
                                App.oMainReference.showMessage(cResourceManager.LoadString("IDS_DEFAULT_SETTINGS_APPLICATED"), "");
                            }
                        }
                        break;
                    case FORM_RETURN_CODES.DO_NOTHING:
                    default:
                        break;
                }

                /// Update UI settings
                /// 
                if (objHome != null)
                    objHome.UpdateData();

                cGlobal.frmNagReturnCode = FORM_RETURN_CODES.DO_NOTHING;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ShowSelectionAssistant", ex);
            }
            finally
            {
                //ShowSelectionAssistantLock.SafeExitWriteLock();
            }
        }

        public void ShowRegistration()
        {
            try
            {
                if (frmRegistration == null)
                {
                    frmRegistration = new Views.frmRegistrationDPF();
                }
                Progress_Show(frmRegistration, this);
                Progress_Close(frmRegistration, this);


            }
            catch (Exception ex)
            {

                
            }
        }

        public void ShowAboutUs()
        {
            try
            {
                if (frmAboutUs == null)
                {
                    frmAboutUs = new Views.frmAboutUs();
                }
                Progress_Show(frmAboutUs, this);
                Progress_Close(frmAboutUs, this);

            }
            catch (Exception ex)
            {

                
            }
        }

        public void ShowFeedBack()
        {
            try
            {
                if (ofrmFeedBack == null)
                {
                    ofrmFeedBack = new frmFeedBack ();
                }
                Progress_Show(ofrmFeedBack, this);
                Progress_Close(ofrmFeedBack, this);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ShowActionCenter()
        {
            try
            {
                objHome.Visibility = Visibility.Collapsed;
                objActionCentre.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: LoadResults: ", ex);
            }
        }

        public void ShowHome2()
        {
            try
            {
                objHome.Visibility = Visibility.Visible;
                objActionCentre.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: LoadResults: ", ex);
            }
        }

        public void ShowSaveSettings()
        {
            try
            {
                if (oSettingsfrm != null)
                {
                    oSettingsfrm.ShowSettingsApplied();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow :: ShowSaveSettings : ", ex);
            }
        }
       
        public void Progress_Show(Window oWindow, Window owner = null, bool showAsDialog = true)
        {
            try
            {
                if (oWindow == null)
                {
                    return;
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    //Window wndParent = owner;
                    try
                    {
                        if (owner == null)
                        {
                            owner = this;
                        }
                        //wndParent = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

                        owner.Opacity = .3;
                        owner.Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 15 };

                        oWindow.ShowInTaskbar = false;
                        oWindow.Owner = owner;
                        oWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        //if (this.WindowState != WindowState.Maximized)
                        {
                            oWindow.Left = this.Left + (this.ActualWidth - oWindow.ActualWidth) / 2;
                            oWindow.Top = this.Top + (this.ActualHeight - oWindow.ActualHeight) / 2;
                        }
                        if (showAsDialog)
                            oWindow.ShowDialog();
                        else
                        {
                            oWindow.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.Message);
                        //Log.Error("MainWindow \t mnuAbout_Click \t {error}", ex);
                    }
                    finally
                    {
                        if (owner != null)
                        {
                            if (showAsDialog && !(owner == this && IsOtherWindowsOpen()))
                            {
                                owner.Opacity = 1;
                                owner.Effect = null;
                                owner.Activate();
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("Progress_Show:: ", ex);
            }
        }

        public void Progress_Close(Window oWindow, Window owner = null)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Window wndParent = owner;
                    try
                    {
                        if (wndParent == null)
                        {
                            wndParent = System.Windows.Application.Current.MainWindow;
                        }
                        if (wndParent != null)
                        {
                            //if (!WindowHelper.IsOtherWindowsOpen())
                            {
                                wndParent.Opacity = 1;
                                wndParent.Effect = null;
                            }
                        }

                        if (oWindow.Visibility != Visibility.Visible)
                            return;

                        if (!oWindow.IsVisible)
                        {
                            return;
                        }

                        //wndParent = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); 
                        if (oWindow == null || !oWindow.IsVisible)
                            return;

                        oWindow.Hide();

                    }
                    catch (Exception ex)
                    {
                        //Log.Error("MainWindow \t mnuAbout_Click \t {error}", ex); 
                    }

                }));

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("Progress_Close:: ", ex);
            }
        }
        public bool IsOtherWindowsOpen(Window wndToIgnore = null)
        {
            bool isOpen = false;
            try
            {
                foreach (Window item in App.Current.Windows)
                {
                    if (item != this && item.Visibility == Visibility.Visible)
                    {
                        if (wndToIgnore != null && item == wndToIgnore)
                        {
                            continue;
                        }

                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|CloseIfOtherWindowsOpen()|Error:{1}", this.Name, ex.Message));
            }

            return isOpen;
        }

        public void showMessage(string heading, string message)
        {
            try
            {
                cGlobalSettings.oLogger.WriteLog(heading);
                this.UIThread(() =>
                {
                    showMessageEx(this, "", heading + "\r\n" + message);
                
                });

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: showMessage:", ex);
            }
        }

        public DialogResult showYesNoMessage(string message)
        {
            DialogResult result = System.Windows.Forms.DialogResult.No;
            try
            {

                //cGlobalSettings.oLogger.WriteLog();
                this.UIThread(() =>
                {
                    //result = MessageBoxEx.Show(this, message, cGlobalSettings.GetProductNameFromDesc(), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    result = showMessageEx(this, cGlobalSettings.GetProductNameFromDesc(), message, MessageBoxButtons.YesNo);

                });
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmMian:: showMessage:", ex);
            }
            return result;
        }

        public DialogResult showMessageEx(Window owner, string Caption, string Message, MessageBoxButtons msgBoxButtons = MessageBoxButtons.OK)
        {
            try
            {
                cGlobalSettings.custMsgBox = null;

                cGlobalSettings.custMsgBox = new frmThemeMessageBox();
                cGlobalSettings.custMsgBox.ShowInTaskbar = false;
                cGlobalSettings.custMsgBox.msgBoxButtons = msgBoxButtons;
                cGlobalSettings.custMsgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                cGlobalSettings.custMsgBox.ShowMsg(owner, Caption, Message);

                cGlobalSettings.custMsgBox.ShowDialog();

                return cGlobalSettings.custMsgBox.result;

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: showMessage:", ex);
            }

            return cGlobalSettings.custMsgBox.result;

        }

        public void footer_showScanDuplicateButton(int statusID, bool visibleScanButton, bool enableScanButton)
        {
            try
            {
                if (oFooter != null)
                    oFooter.showScanDuplicateButton(statusID, visibleScanButton, enableScanButton);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("footer_showScanDuplicateButton:: ", ex);
            }
        }

        public void ucResult_showDuplicateStatusMessage()
        {
            //oResult.showDuplicateStatusMessage(); ///have to add fn in ucResult
        }

        public void CloseApp()
        {
            try
            {
                if (notifyMainApp != null)
                { notifyMainApp.Dispose(); }
                notifyMainApp = null;

                this.Close();
                Application.Exit();
                Environment.Exit(0);
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("MainWindow|Close |{0}", ex.Message));
            }
        }
        public void ForceApplicationExit()
        {
            try
            {

                cGlobalSettings.FORCE_APP_CLOSE = true;

                /*this.Close();*/
                CloseApp();

            }
            catch (System.Exception ex)
            {

            }
        }
        public bool bSetminimizedToTray = true;
        public bool minimizedToTray = false;

        public void ShowWindow()
        {
            try
            {
                bool isReady = true;
                //webMainWindow.UIThread(() => isReady = webMainWindow.ReadyState == WebBrowserReadyState.Complete);
                if (!isReady)
                {

                    return;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|ShowWindow()|Error:{1}", this.Name, ex.Message));
            }
        }

        public void ShowFormAfterScanMark()
        {
            try
            {
                frmAfterPhotosFound objAfterScanMark = new frmAfterPhotosFound();

                objAfterScanMark.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                Progress_Show(objAfterScanMark,null, true);

                Progress_Close(objAfterScanMark);

                switch (cGlobal.frmNagReturnCode)
                {
                    case FORM_RETURN_CODES.AUTO_MARK:
                        {
                            if (cGlobalSettings.listImageFileInfo == null || cGlobalSettings.listImageFileInfo.Count < 1)
                                return;

                          //  Program.oMainReference.oResult.btn_autoMark_Click(this, null); //Click auto mark button and update listImageFileInfo
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmMain:: ShowFormGuide: ", ex);
            }
        }

        #region Tracking pixel fire
        //internal void FireEventTrackingPixelInThread(string format, params object[] args)
        //{
        //    try
        //    {
        //        //return;
        //        /// change the site id to track it
        //        ThreadPool.QueueUserWorkItem(_ => FireEventTrackingPixel(format, args));
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("frmMain::FireEventTrackingPixelInThread", ex);
        //    }
        //}

        //private void FireEventTrackingPixel(string format, params object[] args)
        //{
        //    try
        //    {
        //        string eventLabel = string.Format(format, args);

        //        FireEventTrackingPixel(eventLabel, cGlobal.strEventCategory);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("frmMain::FireEventTrackingPixel-format", ex);
        //    }

        //}

        //public void FireEventTrackingPixelInThread(string eventLabel, string eventCategory = "rbwinapp", string eventAction = "btnclick", int eventValue = 1)
        //{
        //    ThreadPool.QueueUserWorkItem(_=> FireEventTrackingPixel(eventLabel));
        //}


        /// <summary>
        /// will fire the Google analytics url to track the application clicks
        /// </summary>
        /// <param name="surl"></param>
        /// <param name="sSize"></param>

        public static void UiInvoke(System.Action a)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(a);
            }
            catch (Exception oEx)
            {
                //oLogger.WriteLog("UiInvoke()", oEx.Message);
            }
        }
        private void FireEventTrackingPixel(string eventLabel, string eventCategory = "", string eventAction = "btnclick", int eventValue = 1)
        {
            try
            {
                UiInvoke(() =>
                {
                    System.Windows.Forms.WebBrowser oBrowser = null;

                    try
                    {
                        cGlobalSettings.oLogger.WriteLogVerbose("EventTrackingPixel:called ");
                        if (string.IsNullOrEmpty(eventCategory))
                        {
                            eventCategory = cGlobal.strEventCategory;
                        }
                        string sUrl = cGlobal.ReturnFormattedPixelURL(eventLabel, eventCategory, eventAction, eventValue);

                        cGlobalSettings.oLogger.WriteLogVerbose("EventTrackingPixel:called with URL: {0} ", sUrl);

                        oBrowser = new System.Windows.Forms.WebBrowser();
                        oBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(oBrowser_DocumentCompleted);

                        /*if (oBrowser.ActiveXInstance != null)
                        {
                            try
                            {
                                cGlobalSettings.oLogger.WriteLogVerbose("FireEventTrackingPixel:: Browser has ActiveXInstance!!");
                                var oActiveX = (SHDocVw.WebBrowser)oBrowser.ActiveXInstance;
                                oActiveX.FileDownload += OActiveX_FileDownload;
                            }
                            catch (Exception ex)
                            {
                                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|LoadUrlForTrackingSilently()|download event exception:{1}", this, ex.Message));
                            }
                        }*/

                        oBrowser.ScriptErrorsSuppressed = true;
                        oBrowser.Navigate(sUrl);
                        // oUtils.WriteToFile(sUrl); 

                    }
                    catch (System.Exception ex)
                    {
                        cGlobalSettings.oLogger.WriteLogException("frmMain::FireEventTrackingPixel-1", ex);
                    }
                    finally
                    {
                        oBrowser = null;
                    }

                    cGlobalSettings.oLogger.WriteLogVerbose("EventTrackingPixel:exit ");
                });
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|FireEventTrackingPixel()|Error:{1}", this.Name, ex.Message));
            }
        }


        /// <summary>
        /// the document completed event fired multiple times for a page, so delaying the form close for 10 sec, so page will load completely.        
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bCallBrowserSet"></param>
        //public void SilentUrlfire(string url/*, bool bCallBrowserSet = false*/)
        //{
        //    try
        //    {
        //        bool bDisposeTimerFired = false;
        //        this.BeginInvoke(new System.Action(() =>
        //        {
        //            /*if (bCallBrowserSet) */
        //            cGlobal.settingWebBrowser();

        //            System.Windows.Forms.WebBrowser oBrowser2 = new System.Windows.Forms.WebBrowser();
        //            oBrowser2.DocumentCompleted += (sender, e) =>
        //            {
        //                try
        //                {
        //                    Trace.WriteLine("Url - fired");

        //                    if (e.Url != null)
        //                        Trace.WriteLine("Url - fired - " + e.Url.AbsolutePath);

        //                    if (!bDisposeTimerFired)
        //                    {
        //                        bDisposeTimerFired = true;

        //                        try
        //                        {
        //                            System.Threading.Timer timer = null;
        //                            timer = new System.Threading.Timer((obj) =>
        //                            {
        //                                this.BeginInvoke(new System.Action(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        (sender as System.Windows.Forms.WebBrowser).Dispose();
        //                                        sender = null;
        //                                    }
        //                                    catch (Exception)
        //                                    {
        //                                        ;
        //                                    }

        //                                }));

        //                                timer.Dispose();

        //                            }, null, 10000, System.Threading.Timeout.Infinite);
        //                        }
        //                        catch (Exception)
        //                        {
        //                            ;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    cGlobalSettings.oLogger.WriteLog(string.Format("{0}|WebBrowser1_DocumentCompleted|exception:{1}", this, ex.Message));
        //                }
        //            };

        //            oBrowser2.ScriptErrorsSuppressed = true;
        //            oBrowser2.Navigate(url);
        //        }));

        //        //System.Threading.Thread.Sleep(500); // (1000); 
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLog(string.Format("{0}|SilentUrlfire()|exception:{1}", this, ex.Message));
        //    }
        //}

        //public void LoadUrlForTrackingSilently(string url)
        //{
        //    try
        //    {
        //        //enable BHO logs only when scanned for the first time 
        //        WebBrowser oBrowser = new WebBrowser();
        //        oBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(oBrowser_DocumentCompleted);
        //        oBrowser.ScriptErrorsSuppressed = true;
        //        oBrowser.Navigate(url);
        //        System.Threading.Thread.Sleep(500); // (1000); 
        //    }
        //    catch (System.Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("frmMain:: LoadUrlForTrackingSilently: ", ex);
        //        //oUtils.WriteToFile(string.Format("{0}|LoadUrlForTrackingSilently()|exception:{1}", this, ex.Message));
        //    }
        //}

        //internal void FireKeyStatus(string sKey, string sEmail, string utmContent, string HardwareHash)
        //{
        //    try
        //    {
        //        Version osVersion = csOSProperties.GetOSVersion();
        //        string url = string.Format(cGetString.GetKeyStatusString() +
        //        "key={0}" +
        //        "&email={1}" +
        //        "&utm_source={2}" +
        //        "&utm_campaign={3}" +
        //        "&utm_content={4}" +
        //        "&utm_medium={5}" +
        //        "&xat={6}" +
        //        "&affiliate={7}" +
        //        "&machineid={8}" +
        //        "&osid={9}" +
        //        "&osver={10}" +
        //        "&osarc={11}" +
        //        "&productid={12}" +
        //        "&langid={13}" +
        //        "&langcode={14}",
        //            sKey, sEmail, cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN, utmContent,
        //            cGlobal.UTM_MEDIUM, cGlobal.X_AT, cGlobal.UTM_AFFILIATE, HardwareHash,
        //            csOSProperties.GetOSId(), string.Format("{0}.{1}", osVersion.Major, osVersion.Minor), csOSProperties.IsOS64Bit() ? "64" : "32",
        //            cGlobalSettings.ProductId, (int)cGlobalSettings.oScanSettings.CurrentLangSettings.LangIdentifier, cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv);

        //        cGlobalSettings.oLogger.WriteLog("Send status: Url" + url);

        //        LoadUrlForTrackingSilently(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("FireTrackingLink", ex);
        //    }
        //}


        #endregion


        #region utilities

        public bool CheckTweakPassCompatible()
        {
            bool IsCompatible = true;

            try
            {
                eOS CurrentOS = cGlobalSettings.systemOS;

                if (CurrentOS == eOS.XP || CurrentOS == eOS.XP64 || CurrentOS == eOS.Vista || CurrentOS == eOS.Win7)
                {
                    if (!string.IsNullOrEmpty(Environment.OSVersion.ServicePack))
                    { servicePack = Convert.ToInt16(Environment.OSVersion.ServicePack); }

                    if (servicePack <= 1)
                        IsCompatible = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return IsCompatible;
        }
        public bool CheckSAVCompatible()
        {
            bool IsCompatible = true;
            try
            {
                eOS CurrentOS = cGlobalSettings.systemOS;
                if (CurrentOS == eOS.XP || CurrentOS == eOS.XP64 || CurrentOS == eOS.Vista)
                    IsCompatible = false;

                if (CurrentOS == eOS.Win7)
                {
                    Version osVer = cGlobalSettings.GetOSVersion();
                    if (osVer.Major >= 6 && osVer.Minor >= 1 && osVer.Build >= 7601)
                        IsCompatible = true;
                    else
                        IsCompatible = false;

                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CheckSAVCompatible()", ex);
            }

            return IsCompatible;
        }

        public bool CheckSSUCompatible()
        {
            bool IsCompatible = true;
            try
            {
                eOS CurrentOS = cGlobalSettings.systemOS;

                if (CurrentOS == eOS.XP || CurrentOS == eOS.XP64 || CurrentOS == eOS.Vista)
                    IsCompatible = false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CheckSSUCompatible()", ex);
            }

            return IsCompatible;
        }



        public bool CheckVPNCompatible()
        {
            bool IsCompatible = true;
            try
            {
                eOS CurrentOS = cGlobalSettings.systemOS;

                if (CurrentOS == eOS.XP || CurrentOS == eOS.XP64 || CurrentOS == eOS.Vista)
                    IsCompatible = false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CheckVPNCompatible()", ex);
            }

            return IsCompatible;
        }
        public bool CheckTweakShotCompatible()
        {
            bool IsCompatible = true;
            try
            {

                switch (cGlobalSettings.systemOS)
                {
                    case eOS.XP:
                    case eOS.XP64:
                    case eOS.Vista:
                        IsCompatible = false;
                        break;
                }
                /*
                  string stros = GetOSInfo();

                if (stros == "XP" || stros == "XP64" || stros == "Vista")
                    IsCompatible = false;
                    */
            }
            catch (Exception ex)
            {
                IsCompatible = false;
                cGlobalSettings.oLogger.WriteLogException("CheckTweakShotCompatible:: ", ex);
            }
            return IsCompatible;
        }

        public void ShowInstallUtilityBuyNow(ProductBuyNow RecommendedItemsData, bool IsInstalled)
        {
            if (RecommendedItemsData == null)
                return;

            //frmInstallutilitiesBuyNow oInstallUtilities = new frmInstallutilitiesBuyNow(RecommendedItemsData, IsInstalled);

            //Progress_Show(oInstallUtilities, this, true);
            //Progress_Close(oInstallUtilities, this);

        }

        public void ShowInstallUtility(Product RecommendedItemsData)
        {
            if (RecommendedItemsData == null)
                return;

            frmInstallUtilities oInstallUtilities = new frmInstallUtilities(RecommendedItemsData);

            Progress_Show(oInstallUtilities, this, true);
            Progress_Close(oInstallUtilities, this);

        }


        private string _countryCode;
        private double _totalAmount;
        public void ShowUtilityOffer()
        {

            if (!cWin32APIs.IsInternetConnectedEx())
            {
                DialogResult response = showYesNoMessage(string.Format(cResourceManager.LoadString("IDS_INTERNET_ERROR"), cResourceManager.LoadString("IDS_DU_APP_NAME")));
                if (response == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        cGlobal.BuyNow(eDiscountOffers.None, eUTM_BTN_TYPE.RP_BTN_UpgradeNow.ToString());
                        //System.Diagnostics.Process.Start(cGlobalSettings.DPFWebURL);
                    }
                    catch (Exception ex)
                    {
                        cGlobalSettings.oLogger.WriteLogException("ShowUtilityOffer() :: buyNow_Yes :", ex);
                    }
                }
                return;
            }
            _countryCode = "";
            //try
            //{
            //    if (oBestValue == null || oBestValue.IsDisposed)
            //    {
            //        oBestValue = new frmBestValue() { StartPosition = FormStartPosition.CenterParent };
            //        oBestValue.Onsubmitvalues += new frmBestValue.SubmitValues(oBestValue_Onsubmitvalues);
            //    }
            //    //cGlobal.BuyNow(eDiscountOffers.None, eUTM_BTN_TYPE.HelpMenuEnterRegistrationKey);
            //    //ShowChildWindow(Program.oMainReference.oBestValue, true);

            //    Progress_Show(oBestValue, true);

            //    //if (!string.IsNullOrEmpty(_countryCode))
            //    //{
            //    //    paddle = new frmPaddle() { StartPosition = FormStartPosition.CenterParent, VendorID = cGlobalSettings.PaddleVender_ID, ProductID = cGlobalSettings.PaddleProductID, AppName = cGlobalSettings.GetProductName() };
            //    //    //ShowChildWindow(Program.oMainReference.paddle, true);
            //    //    Progress_Show(paddle, true);
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    cGlobalSettings.oLogger.WriteLogException(" frmMain :: DisplayBestValueDialog : ", ex);
            //}
            //oRegistration.ShowDialog(this);
        }




        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        public void LaunchAndRefresh()
        {
            if (minimizedToTray)
            {
                try
                {
                    Program.bSilentScan = false;
                    this.Opacity = 1;

                    // CloseIfOtherWindowsOpen(null);
                    //  
                    bSetminimizedToTray = false;
                    ShowWindow();


                    foreach (Form oFrm in Application.OpenForms)
                    {
                        if (!oFrm.IsDisposed && oFrm != null)
                        {

                            if (oFrm.Visible)
                            {
                                if (oFrm.Name == this.Name)
                                {
                                    oFrm.WindowState = bIsMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
                                    continue;
                                }

                                if (!oFrm.IsDisposed && oFrm != null)
                                {
                                    //if (oFrm is frmShowTransDialog && !cGlobalSettings.b_SCAN_STARTED)
                                    //{
                                    //    oFrm.Hide();
                                    //    continue;
                                    //}

                                    if (oFrm.WindowState == FormWindowState.Minimized)
                                    {
                                        // oFrm.ShowInTaskbar = false
                                        oFrm.WindowState = FormWindowState.Normal;
                                    }
                                    else if (oFrm is frmProgress)
                                    {

                                        //var oProgress = oFrm as frmProgress;
                                        //oProgress.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(513, 190);
                                        oFrm.Size = new System.Drawing.Size(513, 229);

                                        //oFrm.Size = new System.Drawing.Size(0, 0);
                                        //oFrm.Size = new Size(513, 196);
                                        //513, 229
                                        //oFrm.Hide();
                                    }

                                }
                            }

                        }
                    }



                    if (!cGlobalSettings.isAppBusy())//if scan is not started, prompt the user that scan was scheduled at this time, whether he/she wants to scan or continue with the present operation
                    {

                        //ShowScreen((int)sSelectedMainTab, (int)sSelectedMainTab);
                    }
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    minimizedToTray = false;
                    bSetminimizedToTray = true;
                }

            }
            else
            {
                ShowWindow();
            }
            Application.DoEvents();
        }
        internal void DisplayTaskbarNotification()
        {
            try
            {
                if (cGlobalSettings.TotalDuplicatePhotos <= 0 /*|| cGlobalSettings.TotalDuplicatePhotosSize <= 0*/)
                    return;

                if (oTaskbarNotice == null )
                {
                    oTaskbarNotice = new frmTaskbarNotification();
                }

                oTaskbarNotice.SetTotalDuplicates = cGlobalSettings.TotalDuplicatePhotos;
                oTaskbarNotice.SetTotalScace = cGlobalSettings.TotalDuplicatePhotosSize;

                oTaskbarNotice.Show();


                int nCount = (int)cGlobalSettings.GetCommonAppDataRegistryValue(cGlobalSettings.strTrayNagCount, 0);

                oTaskbarNotice.nCounterNagShownTillNow = nCount + 1;
                cGlobalSettings.SetCommonAppDataRegistryValue(cGlobalSettings.strTrayNagCount, oTaskbarNotice.nCounterNagShownTillNow, Microsoft.Win32.RegistryValueKind.DWord);
                //App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.SystemTrayNoticeShown.ToString() + oTaskbarNotice.nCounterNagShownTillNow, eOfferName.DPF_NOTICE.ToString()));
                //FireEventTrackingPixelInThread("{0}", eUTM_BTN_TYPE.SystemTrayNoticeShown.ToString() + oTaskbarNotice.nCounterNagShownTillNow);
            }
            catch (Exception ex)
            {

            }
        }

        public void StartYTTutorial(string sUrl)
        {
            try
            {
                if (objYTTutorial == null)
                {
                    objYTTutorial = new frmYTTutorial(sUrl);
                    objYTTutorial.Show();
                }

                //objYTTutorial.BringToFront();

                Progress_Show(objYTTutorial,null, true);
                Progress_Close(objYTTutorial);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("StartYTTutorial:: ", ex);
                objYTTutorial = null;
            }
        }

        public void LoadUrlForTrackingSilently(string url)
        {
            try
            {
                //enable BHO logs only when scanned for the first time 
                WebBrowser oBrowser = new WebBrowser();
                oBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(oBrowser_DocumentCompleted);
                oBrowser.ScriptErrorsSuppressed = true;
                oBrowser.Navigate(url);
                System.Threading.Thread.Sleep(500); // (1000); 
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmMain:: LoadUrlForTrackingSilently: ", ex);
                //oUtils.WriteToFile(string.Format("{0}|LoadUrlForTrackingSilently()|exception:{1}", this, ex.Message));
            }
        }

        void oBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {

                (sender as WebBrowser).Dispose();
                sender = null;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("{0}|oBrowser_DocumentCompleted()|exception:{1}", this, ex.Message));
            }
        }

        internal string GetSaveThumbnailImage(csImageFileInfo keyItem)
        {
            string thumbPath = "";

            try
            {

                switch (cGlobalSettings.CurrentScanMode)
                {
                    case eScanMode.FileSearch:
                    case eScanMode.Lightroom:
                    case eScanMode.PicasaLibrary:
                      //  thumbPath = csImageMagick.GetThumbnailImage(keyItem.filePath);
                        break;
                    case eScanMode.GoogleDrive:
                       // thumbPath = AppFunctions.DownloadCloudThumnail(keyItem.ThumbnailPath, keyItem.md5);
                        break;
                    case eScanMode.Dropbox:
                        {
                            string strThumbPath = keyItem.ThumbnailPath;
                            if (string.IsNullOrEmpty(strThumbPath) || !File.Exists(keyItem.ThumbnailPath))
                            {
                                thumbPath = "";
                            }
                            else
                                thumbPath = strThumbPath;

                        }
                        break;
                    case eScanMode.BoxCloud:
                        {
                            if (File.Exists(keyItem.ThumbnailPath))
                            {
                                thumbPath = keyItem.ThumbnailPath;
                            }


                        }
                        break;

                    case eScanMode.AmazonS3:
                        {
                            if (File.Exists(keyItem.ThumbnailPath))
                            {
                                thumbPath = keyItem.ThumbnailPath;
                            }


                        }
                        break;

                    case eScanMode.DeleteEmptyFolder:
                        break;
                }

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: GetSaveThumbnailImage: ", ex);
            }


            return thumbPath;
        }


        #region Tracking pixel fire

        internal void FireKeyStatus(string sKey, string sEmail, string utmContent, string HardwareHash)
        {
            try
            {
                Version osVersion = csOSProperties.GetOSVersion();
                string url = string.Format(cGetString.GetKeyStatusString() +
                "key={0}" +
                "&email={1}" +
                "&utm_source={2}" +
                "&utm_campaign={3}" +
                "&utm_content={4}" +
                "&utm_medium={5}" +
                "&xat={6}" +
                "&affiliate={7}" +
                "&machineid={8}" +
                "&osid={9}" +
                "&osver={10}" +
                "&osarc={11}" +
                "&productid={12}" +
                "&langid={13}" +
                "&langcode={14}",
                    sKey, sEmail, cGlobal.UTM_SOURCE, cGlobal.UTM_CAMPAIGN, utmContent,
                    cGlobal.UTM_MEDIUM, cGlobal.X_AT, cGlobal.UTM_AFFILIATE, HardwareHash,
                    csOSProperties.GetOSId(), string.Format("{0}.{1}", osVersion.Major, osVersion.Minor), csOSProperties.IsOS64Bit() ? "64" : "32",
                    cGlobalSettings.ProductId, (int)cGlobalSettings.oScanSettings.CurrentLangSettings.LangIdentifier, cGlobalSettings.oScanSettings.CurrentLangSettings.NameAbbv);

                cGlobalSettings.oLogger.WriteLog("Send status: Url" + url);

                LoadUrlForTrackingSilently(url);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FireTrackingLink", ex);
            }
        }

        internal void FireEventTrackingPixelInThread(string format, params object[] args)
        {
            try
            {
                //return;
                /// change the site id to track it
                ThreadPool.QueueUserWorkItem(_ => FireEventTrackingPixel(format, args));
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow::FireEventTrackingPixelInThread", ex);
            }
        }

        private void FireEventTrackingPixel(string format, params object[] args)
        {
            try
            {
                string eventLabel = string.Format(format, args);

                FireEventTrackingPixel(eventLabel, cGlobal.strEventCategory);
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow::FireEventTrackingPixel-format", ex);
            }

        }


        public void TrackEvent(string param1, string param2, bool bFireGA, bool bFireNagTracking = true, int reminderoffertype = -1, int remindereventtype = -1, string urlForTrackingSilently = "", string extraParameter = "")
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        if (bFireNagTracking)LoadUrlForTrackingSilently((!string.IsNullOrEmpty(urlForTrackingSilently)) ? urlForTrackingSilently : cGlobalSettings.CombineUrlAppTracking(Convert.ToString(ConfigurationManager.AppSettings["NAG_TRACKING"]), param1, param2, reminderoffertype, remindereventtype, extraParameter));//"RegistrationNag", "Click_BuyNow")); 

                        //MS20072018 Google Analytics Tracking Added. 
                        if (bFireGA) FireEventTrackingPixelInThread("{0}-{1}", param2, param1);//cEnums.eUTM_BTN_TYPE.BtnBuyNowFromRegisterTab.ToString(), string.Empty); 
                    }
                    catch (Exception ex)
                    {
                        cGlobalSettings.oLogger.WriteLogException("Th-TrackEvent:: ", ex);
                    }

                }).Start();


            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("TrackEvent:: ", ex);
            }
        }


        #endregion


        #region Scann wrapper

        public void InitScanner()
        {
            try
            {
                switch (cGlobalSettings.CurrentScanMode)
                {
                    case eScanMode.FileSearch:
                        {
                            cGlobalSettings.objScanner = new SystemScanWrapper();
                        }
                        break;
                    case eScanMode.GoogleDrive:
                        break;
                    case eScanMode.Dropbox:
                        break;
                    case eScanMode.BoxCloud:
                        break;
                    case eScanMode.PicasaLibrary:
                        break;
                    case eScanMode.Lightroom:
                        break;
                    default:
                        break;
                }

                cGlobalSettings.objScanner.OnScanStart += ObjScanner_OnScanStart;
                cGlobalSettings.objScanner.OnScanProgress += ObjScanner_OnScanProgress;
                cGlobalSettings.objScanner.OnScanCompleted += ObjScanner_OnScanCompleted;
            }
            catch (Exception ex)
            {

            }
        }

        private void ObjScanner_OnScanStart(string message, int percentageCompleted, int counter)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        if (fProgress == null)
                        {
                            fProgress = new frmProcessing();
                            fProgress.Show();

                        }
                        fProgress.OnStart();

                    }
                    catch (Exception ex)
                    {
                        cGlobalSettings.oLogger.WriteLogException("MainWindow:: ObjScanner_OnScanStart: ", ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MainWindow:: ObjScanner_OnScanStart: ", ex);
            }
        }

        private void ObjScanner_OnScanProgress(cClientEnum.eScanPhase ScanPhase, string startMsg, int percentageCompleted, int TotalFiles, int TotalScanned, int TotalDuplicates, string AdditionalInfo = "", bool showScanBar = false)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (fProgress != null && fProgress.IsVisible)
                    {
                        fProgress.OnProgress(ScanPhase, startMsg, percentageCompleted, TotalFiles, TotalScanned, TotalDuplicates);
                    }
                }));
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ObjScanner_OnScanProgress: ", ex);
            }
        }

        private void ObjScanner_OnScanCompleted(string messageHeader, string messageDesc, bool bMoveToResultsTab, int percentageCompleted)
        {
            
            this.Dispatcher.Invoke(() =>
            {
                if (cGlobalSettings.listImageFileInfo == null || cGlobalSettings.listImageFileInfo.Count <= 0)
                {
                    // Now it's safe to show message dialogs
                    if (cGlobalSettings.isSimilarMatchChecked)
                        App.oMainReference.showMessage(cResourceManager.LoadString("DPF_SCAN_PHOTOS_NO_SIMILAR_PHOTOS_FOUND"), cResourceManager.LoadString("DPF_SCAN_PHOTOS_HINT_SIMILAR_PHOTOS"));
                    else
                        App.oMainReference.showMessage(cResourceManager.LoadString("DPF_SCAN_PHOTOS_NO_DUPLICATE_PHOTOS_FOUND"), cResourceManager.LoadString("DPF_SCAN_PHOTOS_HINT_DUPLICATE_PHOTOS"));

                    // Close the progress window when no duplicates found
                    if (fProgress != null)
                    {
                        fProgress.Close();
                        fProgress = null;
                    }
                }
                else
                {
                    // Handle the case when duplicates are found
                    if (objResult == null)
                    {
                        objResult = new ucResultView();
                    }

                    if (fProgress != null)
                    {
                        fProgress.OnComplete();
                        fProgress.Close();
                        fProgress = null;
                    }

                    // Add any other UI updates here
                }
            });
        }
           
        public void StartScanTh()
        {
            try
            {

                ShowFormProcessing();

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("", ex);
            }
        }

        public void StartScan()
        {
            try
            {
                HashSet<csImageFileInfo> lstScanFiles = new HashSet<csImageFileInfo>();
                cGlobalSettings.objScanner.Scan(cGlobalSettings.listScanPaths/*.Where(p => p.IsSelected && p.TypeOfInclusion == eTypeOfInclusion.Included)*/, new List<string> { "*.*" }, System.IO.SearchOption.AllDirectories, ref lstScanFiles, null, -1);

                // No files found to compare.
                if (lstScanFiles == null)
                    return;

                cGlobalSettings.objScanner.ReadAllMessages(null, null, ref lstScanFiles, null, -1, "");

                object imgObject = null;
                cGlobalSettings.objScanner.ProcessMessage(null, null, null, ref imgObject, null, -1, "");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("StartScanTh:: ", ex);
            }
        }

        #endregion

        public void ApplySkin(cClientEnum.eThemeMode theme, bool bChanged)
        {
            try
            {
                App.ApplySkin(theme);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ApplySkin:: ", ex);
            }
        }

        public void ShowFormProcessing()
        {
            try
            {
                fProgress = new frmProcessing();
                fProgress.WindowStartupLocation = WindowStartupLocation.CenterOwner;


                Progress_Show(fProgress, this);


            }
            catch (Exception ex)
            {

            }
        }

       

    }
}
