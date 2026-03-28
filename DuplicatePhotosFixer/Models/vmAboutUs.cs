using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.Helper;

namespace DuplicatePhotosFixer.Models
{
    public class vmAboutUs : INotifyPropertyChanged
    {

        bool bAboutUsNagTrack = false;


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public vmAboutUs()
        {

        }

        public void EulaClick()
        {
            try
            {
                string strUrl = cGlobal.GetParams(cGlobalSettings.LinkEULA);
                Process.Start(strUrl);
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_Eula.ToString(), ""), cTrackingParameters.eProductName.AboutUsWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bAboutUsNagTrack, 0, (int)cTrackingParameters.eReminderEventType.other);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmAboutUs :: EulaClick : ", ex);
            }
        }

        public void PrivacyPolicyClick()
        {
            try
            {
                string strUrl = cGlobal.GetParams(cGlobalSettings.LinkPP);
                Process.Start(strUrl);
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_PrivacyPolicy.ToString(), ""), cTrackingParameters.eProductName.AboutUsWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bAboutUsNagTrack, 0, (int)cTrackingParameters.eReminderEventType.other);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmAboutUs :: PrivacyPolicyClick : ", ex);
            }
        }

       
        public void UninstallInstructionsClick()
        {
            try
            {
                string strUrl = cGlobal.GetParams(cGlobalSettings.LinkUninstall);
                Process.Start(strUrl);
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_UninstallInstructions.ToString(), ""), cTrackingParameters.eProductName.AboutUsWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bAboutUsNagTrack, 0, (int)cTrackingParameters.eReminderEventType.other);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmAboutUs :: UninstallInstructionsClick : ", ex);
            }
        }

        public void SystweakLinkClick()
        {
            try
            {
                string strUrl = cGlobal.GetParams(cGlobalSettings.SystweakWebURL);
                Process.Start(strUrl);
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_UninstallInstructions.ToString(), ""), cTrackingParameters.eProductName.AboutUsWindow.ToString(), cGlobalSettings.bEnableGATrackingVerbose, bAboutUsNagTrack, 0, (int)cTrackingParameters.eReminderEventType.other);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmAboutUs :: SystweakLinkClick : ", ex);
            }
        }

        public void SupportEmailSystem()
        {
            try
            {
                Process.Start("mailto:" + cResourceManager.LoadString("IDS_SUPPORT_ADDRESS"));
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmAboutUs :: SupportEmailSystem : ", ex);
            }
        }

        public void Activate()
        {
            try
            {
                cGlobal.NagParentStr = cTrackingParameters.eProductName.AboutUsWindow.ToString();
                App.oMainReference.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.BTN_ActivateNow.ToString(), ""), cTrackingParameters.eProductName.AboutUsWindow.ToString(), true, bAboutUsNagTrack, 0, (int)cTrackingParameters.eReminderEventType.actionbuttonprimary);

                //if (cGlobal.IS_LB_BUILD == 0)
                //    cGlobal.BuyNow(System.Windows.Forms.eDiscountOffers.None, System.Windows.Forms.eUTM_BTN_TYPE.AboutUpgrade.ToString());

                //if (App.oMainFormReference.oActivation == null)
                App.oMainReference.frmRegistration = new Views.frmRegistrationDPF();

                App.oMainReference.Progress_Show(App.oMainReference.frmRegistration, App.oMainReference, true);
                App.oMainReference.Progress_Close(App.oMainReference.frmAboutUs);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("frmAboutUs_btnActivate_Click:: ", ex);
            }
        }
    }
}
