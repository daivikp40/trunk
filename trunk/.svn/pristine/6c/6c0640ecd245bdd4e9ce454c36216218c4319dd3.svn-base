using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DuplicatePhotosFixer.App_code;
using DuplicatePhotosFixer.Helper;
using DuplicatePhotosFixer.Helpers;
using DuplicatePhotosFixer.Views;
using static DuplicatePhotosFixer.cClientEnum;
using static DuplicatePhotosFixer.Views.cNagDialogue;
using Control = System.Windows.Controls.Control;

namespace DuplicatePhotosFixer.Models
{
  public class vmActionCenter : INotifyPropertyChanged
    {
        private bool bHomeTracking = false;
        private bool bActionCenter = false;

        private readonly Dispatcher dispatcher;

        public BindingList<Product> lstProducts { get; set; }

        #region properties





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

        public vmActionCenter()
        {
            UpdateData();
            dispatcher = Dispatcher.CurrentDispatcher;
        }



        public void UpdateData()
        {
            try
            {


                if (App.oMainReference == null) 
                    return;
                UtilityKitNew.LoadProducts();
                if (UtilityKitNew.lstProducts != null)
                {
                    lstProducts = new BindingList<Product>(UtilityKitNew.lstProducts);

                }

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog("vmActionCenter :: UpdateData()|error : " + ex.Message);
            }
        }



        public void ActivateNow()
        {
            try
            {

                //if (cGlobal.IS_LB_BUILD == 0)
                //    cGlobal.BuyNow(System.Windows.Forms.eDiscountOffers.None, System.Windows.Forms.eUTM_BTN_TYPE.HomeUpgrade.ToString());

                cGlobal.NagParentStr = cTrackingParameters.eProductName.ActivateNowTab.ToString();
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_ActivateNow.ToString(), ""), cTrackingParameters.eProductName.MainWindow.ToString(), true, bHomeTracking, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonsecondary);
                if (App.oMainReference.frmRegistration == null)
                    App.oMainReference.frmRegistration = new frmRegistrationDPF();

                App.oMainReference.Progress_Show(App.oMainReference.frmRegistration, App.oMainReference, true);
                App.oMainReference.Progress_Close(App.oMainReference.frmRegistration);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("vmActionCenter |btnUpgradeNow_Click", ex.Message));
            }
        }

        public void InstallProduct(object sender)
        {
            try
            {
                Control ctrl = (Control)sender;
                if (ctrl.Tag == null)
                    return;
                Product oData = (Product)ctrl.Tag;

                if (oData.nInstallSatus == (int)Helper.eInstallSatus.Installed)
                {

                    Process.Start(oData.strCompletePath, string.Empty/*, "/autoscan"/*"-autorun"/*"-rem"*/);
                    App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_LaunchNow.ToString(), oData.ProductName.Replace(" ", "")), cTrackingParameters.eProductName.ActionCenterWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bActionCenter, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonprimary);
                    return;
                }

                cGlobal.frmNagReturnCode = FORM_RETURN_CODES.DO_NOTHING;

                App.oMainReference.ShowInstallUtility(oData);

                switch (cGlobal.frmNagReturnCode)
                {
                    case FORM_RETURN_CODES.InstallUtility:
                        {
                            // Start download and install process
                            new Thread(() =>
                            {
                                try
                                {
                                    dispatcher.Invoke(new Action(() =>
                                    {
                                        oData.nInstallSatus = (int)Helper.eInstallSatus.Installing;
                                        oData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Installing.ToString().ToUpper());

                                        lstProducts.ResetBindings();
                                    }));
                                    Thread.Sleep(100);
                                    cClientEnum.eDownloadStatus downloadStatus;
                                    cGlobalSettings.DownloadNInstallRecommandedSetup(oData.ProductName, oData.DownloadLink, false, out downloadStatus);

                                    dispatcher.Invoke(new Action(() =>
                                    {
                                        oData.strCompletePath = oData.strCompletePath;

                                        if (downloadStatus == cClientEnum.eDownloadStatus.DownloadSucessFull && System.IO.File.Exists(oData.strCompletePath))
                                        {
                                            oData.nInstallSatus = (int)Helper.eInstallSatus.Installed;
                                            oData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Installed.ToString().ToUpper());
                                        }
                                        else
                                        {
                                            if (downloadStatus == cClientEnum.eDownloadStatus.NoInternetConnection)
                                            {
                                                App.oMainReference.showMessage(cResourceManager.LoadString("IDS_ERROR_CONNECTING_INTERNET"), "");
                                            }

                                            oData.nInstallSatus = (int)Helper.eInstallSatus.Not_Installed;
                                            oData.strInstallSatus = cResourceManager.LoadString("IDS_INSTALLSATUS_" + Helper.eInstallSatus.Not_Installed.ToString().ToUpper());
                                        }
                                        UpdateData();
                                    }));
                                }
                                catch (Exception ex)
                                {

                                }
                            }).Start();
                        }
                        App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_InstallNow.ToString(), oData.ProductName.Replace(" ", "")), cTrackingParameters.eProductName.ActionCenterWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bActionCenter, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonprimary);
                        break;
                    case FORM_RETURN_CODES.DO_NOTHING:
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog("vmActionCenter | InstallProduct | error:" + ex.Message);
            }
        }


        public void UpgradeNow()
        { 
            
            try
                {


                
                    //if (cGlobalSettings.oScanSettings.LanguageCode.ToLower() == "en" && cGlobal.setupType == eSetupType.ADG_CAPMPAIGN_PADDLE)
                    //{
                    //    App.oMainReference.ShowUtilityOffer();
                    //}
                    //else
                    //{
                    //    bool isNewPricePlanNagShown = App.oMainReference.DisplayPricePlanNag();

                    //    if (!isNewPricePlanNagShown)
                    //    {
                    //        WindowsFormsSynchronizationContext.Current.Post(_ =>
                    //        {
                    //            App.oMainReference.DisplayEnterKeyDialog();
                    //        }, null);
                    //        System.Threading.Thread.Sleep(20);
                    //        cGlobal.BuyNow(eDiscountOffers.None, eUTM_BTN_TYPE.HomeUpgrade.ToString());
                    //    }
                    //}
               
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("UpgradeNow", ex);
            }
        }

    }
}

    



