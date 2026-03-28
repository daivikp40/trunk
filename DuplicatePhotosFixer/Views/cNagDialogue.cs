using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Views
{
    public class cNagDialogue
    {
        public enum FORM_RETURN_CODES
        {
            DO_NOTHING,
            UPGRADE_NOW,
            START_CLEAN,
            SHOW_ENTER_KEY_DIALOG,
            SHOW_ENTER_FREE_KEY_DIALOG,
            AUTO_MARK,
            RESET_DEFAULT,
            SHOW_SETTINGS,
            REQUEST_FREE_LICENSE_KEY,
            NO_THANKS,
            EULA,
            PRIVACY_POLICY,
            GET_RB,
            UPGRADE_NOW_RP,
            UPDATE_PROFILE_RP,
            QUIT_WINDOW,
            SHOW_REMOVE_DUPLICATE_GUIDE,
            CLOSE_TO_TRAY,
            UPGRADE_NOW_N_SHOW_ENTER_KEY_DIALOG,
            YES_PROCEED,
            START_SCAN,
            InstallUtility,
            GetAuthorization, // get authorization

        }


        public enum eOfferName
        {
            DPF_ADU_Offer,
            DPF_RP_Offer1,
            DPF_RP_Offer2,
            DPF_50_Offer,
            DPF_NOTICE,

            DPF_DFF_OFFER,
            DPF_ONEDAY_TRIAL, //VG261218
            FEEDBACK,
            AfterScanMark,
            AutoMarkProgress,

        }


        public enum eDiscountOffers
        {
            None,
            Percent_20 = 20,
            Percent_35 = 35,
            Percent_50 = 50,
            Percent_70 = 70,
        }

        public enum eUTM_BTN_TYPE
        {
            None = 0,
            HomeUpgrade = 1,
            TrialNagRemove15Upgrade = 2,
            TrialNagUpgrade = 3,
            EnterKeyDialog = 4,
            RegistrationNagUpgrade = 5,
            HelpMenuEnterRegistrationKey = 6,
            HelpMenuAboutRegisterNow = 7,
            RegistrationNagRegisterNow,

            MessageTheamUpgradeNow = 8,
            NewTheam1EmailNagUpgrade = 9,
            NewTheam2EmailNagUpgrade = 10,
            NewTheamUpdadeNow = 11,
            VistaTheamEmailNagUpdade = 12,
            VistaTheamNagUpdadeNow = 13,

            UpdateInstalled_new = 14,
            AfterUpdateOffer_AlreadyInstalled = 15,
            AfterUpdateOfferShown = 16,
            AfterUpdateOfferShow_close,
            AfterUpdateOfferShow_quite_popup,
            AfterUpdateOfferShow_close_popup,
            AfterUpdateOfferShow_download_popup,
            //AfterUpdateOfferShow_download,
            AfterUpdateOfferShow_tryitnow,
            AfterUpdateOffer_PrivacyPolicy,
            AfterUpdateOffer_EULA,

            USIndependenceDay,
            USOfferShown,
            USOfferClose,

            //RegistrationNagRegisterNow=6,

            NagRPRecommandationNew,
            RP_BTN_AlreadyUpgraded,
            RP_BTN_UpgradeNow,
            RP_BTN_CLOSE,

            TrialNagRemove15Upgrade_Shown, // Trial nag with free 15 photo delete button showing.
            TrialNagUpgrade_Shown, // Trial nag without free no. of photo delete button showing.
            TrialNag_RemoveFirstFreeTrial_SHOWN, // Trial nag with free first time free clean button shown.

            TrialNagUpgrade50Disc, // Quit window claim offer button.
            TrialNagUpgrade_Close, // Trail window close button [X].
            TrialNag_Remove15, // Trail window remove free photos button.
            TrialNag_RemoveFirstFreeTrial, // Trail window remove free one time free clean button.
            QuitWindow_Show, // Quit window shown.
            QuitWindow_Close, // Quit window close button [X].
            QuitWindow_NoThanks, // Quit window No thanks link.
            QuitWindow_SkipNCLose,// Quit window Skip & CLose link.
            QuitWindow_Submit, // Quit window Submit button click.

            NotifyMainApp_Exit, // When application exit from notification bar.

            ShowQuickGuide, // User choose to Show Quick Guide, help to guide user how to auto mark and remove duplicates.

            TrialNagUpgradeExit,
            TrialNagRemove15UpgradeExit,
            TrialNagUpgradeExit_Close,

            DeleteMarkedLinkClick,

            TrialNagNewRemoveFreePhotos,
            TrialNagNewUpgradeNowBeforeFreeClean,
            TrialNagNewUpgradeNowAfterFreeClean,

            TrialNagNewCloseBeforeFreeClean, // Close button [X] click
            TrialNagNewExitBeforeFreeClean, // Exit link click
            TrialNagNewCloseAfterFreeClean, // Close button [X] click
            TrialNagNewExitAfterFreeClean, // Exit link click

            TrialNagNewEnterKeyDialogBeforeFreeClean,
            TrialNagNewEnterKeyDialogAfterFreeClean,

            CloseNagNewClose, // Close nag [X] button click
            CloseNagNewExit, // Close nag exit link click
            CloseNagNewUpgradeNow,
            CloseNagNewEnterKeyDialog,

            SystemTrayNoticeShown,
            SystemTrayNoticeClose,
            SystemTrayNoticeUpgradeNow,
            SystemTrayNoticeEnterKeyDialog,

            ExitNagClam50Offer,
            ExitNagQuit,
            ExitNagClose,

            SystemTrayOfferShown,
            SystemTrayOfferClam50Offer,
            SystemTrayOfferClose,


            /// <summary>
            /// Tracking for new 50% offer image slider nag.
            /// </summary>
            SystemTrayOfferShownSliderExact,
            SysTrayClaimOffer50SliderExact,
            SystemTrayOfferCloseSliderExact,
            SystemTrayOfferShownSliderSimilar,
            SysTrayClaimOffer50SliderSimilar,
            SystemTrayOfferCloseSliderSimilar,

            AfterUpdateADUOfferShown,
            AfterUpdateADUOfferShown_new_fifteen,
            AfterUpdateADUOfferShow_close_fifteen,
            AfterUpdateADUOffer_PrivacyPolicy_new_fifteen,
            AfterUpdateADUOffer_EULA_new_fifteen,
            AfterUpdateADUOffer_OnEulaClick,
            AfterUpdateADUOffer_OnPrivacyClick,
            AfterUpdateADUOffer_AlreadyInstalled_new_fifteen,
            AfterUpdateADUOfferShow_tryitnow_new_fifteen,
            AfterUpdateADUOfferOnbtnDownloadNow,
            AfterUpdateADUOfferOnbtnClose,


            AfterUpdate50OfferShown,
            AfterUpdate50OfferClaimButton,
            AfterUpdate50OfferClaimLink,
            AfterUpdate50OfferClose,


            SystemTrayOfferFixFilesShown,
            SystemTrayOfferFixFilesTryNow,
            SystemTrayOfferFixFilesClose,


            //VG180517 - as per discussion with Madhusudan updating name of buttons required for local tracking
            OfferAlreadyInstalled,
            OfferShown,
            OnbtnClose,
            OnbtnDownloadNow,
            OnEulaClick,
            OnPrivacyClick,

            //VG261218
            StartFreeTrialOneDayNagClose,
            StartFreeTrialOneDayNagNext,
            StartFreeTrialOneDayNagTrail,

            FreeTrial24ExpireShow,
            FreeTrial24ExpireClose,
            FreeTrial24ExpireRegisterNow,

            OnNextAfter24TrialClean,

            BtnDeleteFooter,
            BtnDeleteResultHeadLink,
            BtnNagCooldownLimit,
            BtnNagCooldownUpgradeNow,
            BtnNagCooldownRemoveLimit,
            BtnDetailsViewPro,

            BtnFooterWatchTut,
            MenuGoToChannel,
            FrmWatchVideo,
            BtnClose,
            BtnSubmit,
            BtnCancel,
            Shown,
            BtnMarkManually,
            BtnAutoMark

        }

       

        public enum eSetupType
        {
            REGULAR = 1, // Site build no path is added by default in scan list.
            THEME_VISTA = 2,
            THEME_NEW_1 = 3,
            THEME_NEW_2 = 4,
            THEME_MESSAGE = 5,
            NL_ONE_TIME_EMAIL_REGISTER = 6,///Add some new updates of SetupType = 9. From type 9 remove quick guide and feedback form.

            SCAN_DEFAULT = 7, // Add OS drive by default and 
            SCAN_ALL = 8, // Add all drives in scan list and start scan.
            ADD_All = 9, // Add all drives in scan list
            NL_EQUIVALENT = 10, /// Same as NL build except functionality of One time free key as in NL build

            ADG_CAPMPAIGN = 11, /// For goggle Ad word's campaign
            ADG_CAPMPAIGN_PADDLE = 12, /// For goggle Ad word's campaign

           
            ADG_CAPMPAIGN_24_HOUR = 3, /// For goggle Ad word's campaign with 24 hour
           

            //REGULAR_OFFER_UPDATE = 11, // This is a regular build, only difference is REGULAR type build read utm_source and utm_campaign from Params while this from its registry.
        }
    }
}
