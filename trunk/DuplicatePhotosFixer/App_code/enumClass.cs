using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DuplicatePhotosFixer
{
    public enum eRcmdApp
    {
        TweakShot = 1,
        TweakPass = 2,
        SAV = 3,
        VPN = 4,
        VPN1 = 5,
        SAV1 = 6,
        SSU = 7,
        PR = 8,
        TSR = 9,
        APM = 10,
    }
    public enum eWatchTutorialType
    {
        NoWhere = 0,
        HelpeMenu = 1,
        AppFooter = 2,
        EveryWhere = 3
    }

    public enum eUserStatusType
    {
        Never = 0,
        RegisterUser = 1,
        TrialUser = 2,
        ExpiredUser = 3,
        Always = 4
    }

    public enum eFuncRetVal
    {
        SUCCESS = 0,
        FAIL = 1,
        OTHER,
        FILE_NOT_FOUND,
        CANCEL,
        NO_SPACE,
        PATH_NOT_FOUND,
    }

    public enum eScanMode
    {
        FileSearch = 0,
        GoogleDrive = 1,
        Dropbox = 2,
        PicasaLibrary = 3,
        Lightroom = 4,
        PhotoOrganizer = 5,
        BoxCloud = 6,
        AmazonS3 = 7,
        DeleteEmptyFolder = 10,
    }

    

    public enum eProgressType
    {
        STARTED,
        IN_PROGRESS,
        COMPLETED,
        COMPLETED_WITH_ERRORS,
        STOPPED, // due to any error
        CANCELED, // by user
    }

    public enum eOS
    {
        other = 0,
        XP = 4,
        XP64 = 5,
        Vista = 6,
        Win7 = 7,
        Win8 = 8,
        Win8_1 = 9,
        Win10 = 10,
        Win11 = 11
    }

    public static class enumClass
    {

        public enum eScanMode
        {
            FileSearch = 0,
            GoogleDrive = 1,
            DropBox = 2,
            PicasaLibrary = 3,
            LightRoomClassic = 4,
            PhotoOrganizer = 5,
            BoxCloud =6,
            AmazonS3 = 7


        }

        public enum eBtnVisableStatus
        {
            Active = 0,// Normal
            Inactive = 1,//Click
            MouseHover = 2,
            Disable = 3
        }

        public enum ePathType
        {
            file = 0,
            folder,
            library
        }


        public enum eMatchMethod
        {
            Exact,
            Similar
        }

     
        public enum eMatchingLevel
        {
            [Description("0.85")]
            _85 = 0,
            [Description("0.88")]
            _88 = 1,
            [Description("0.90")]
            _90 = 2,
            [Description("0.92")]
            _92 = 3,
            [Description("0.94")]
            _94 = 4,
            [Description("0.96")]
            _96 = 5,
            [Description("0.98")]
            _98 = 6,
            [Description("0.99")]
            _100 = 7
        }

        public enum eTimeInterval
        {
            [Description("DPF_SETTINGS_UC_24_HOURS")]
            _24_hours_and_less,
            [Description("DPF_SETTINGS_UC_12_HOURS")]
            _12_hours_and_less,
            [Description("DPF_SETTINGS_UC_6_HOURS")]
            _6_hours_and_less,
            [Description("DPF_SETTINGS_UC_3_HOURS")]
            _3_hours_and_less,
            [Description("DPF_SETTINGS_UC_2_HOURS")]
            _2_hours_and_less,
            [Description("DPF_SETTINGS_UC_1_HOURS")]
            _1_hours_and_less,
            [Description("DPF_SETTINGS_UC_30_MINUTES")]
            _30_minutes_and_less,
            [Description("DPF_SETTINGS_UC_10_MINUTES")]
            _10_minutes_and_less,
            [Description("DPF_SETTINGS_UC_5_MINUTES")]
            _5_minutes_and_less,
            [Description("DPF_SETTINGS_UC_3_MINUTES")]
            _3_minutes_and_less,
            [Description("DPF_SETTINGS_UC_2_MINUTES")]
            _2_minutes_and_less,
            [Description("DPF_SETTINGS_UC_1_MINUTES")]
            _1_minutes_and_less,
            [Description("DPF_SETTINGS_UC_30_SECONDS")]
            _30_seconds_and_less,
            [Description("DPF_SETTINGS_UC_10_SECONDS")]
            _10_seconds_and_less,
            [Description("DPF_SETTINGS_UC_5_SECONDS")]
            _5_seconds_and_less,
            [Description("DPF_SETTINGS_UC_3_SECONDS")]
            _3_seconds_and_less,
            [Description("DPF_SETTINGS_UC_2_SECONDS")]
            _2_seconds_and_less,
            [Description("DPF_SETTINGS_UC_1_SECONDS")]
            _1_second_and_less,
        }

        public enum eBitmapSize
        {
            [Description("48")]
            _48x48_pixels = 0,
            [Description("56")]
            _56x56_pixels,
            [Description("64")]
            _64x64_pixels,
            [Description("72")]
            _72x72_pixels,
            [Description("80")]
            _80x80_pixels,
            [Description("88")]
            _88x88_pixels,
            [Description("96")]
            _96x96_pixels
        }
        public enum eRegistryKeys
        {
            bNotShowDeleteConfirmation,
            bNotShowAutoMarkConfirmation,
            bNotShowBackConfirmation,
            priority,
            fileTypes,
            minSize,
            minSizeIn,
            FilesDeleted,
            isSimilar,
            matchingLevel,
            bitMapSizeValue,
            timeInterval,
            gpsValue,
            FreeAppTrialKey,
            ScanSystemStartup,
            ProductMessages,
            OtherProductMessages,
            DisableAutoUpdates,
            PeriodicMessages
        }

        public enum eGPS
        {
            [Description("DPF_SETTINGS_UC_1_METER")]
            _1_Meter = 0,
            [Description("DPF_SETTINGS_UC_2_METERS")]
            _2_Meters = 1,
            [Description("DPF_SETTINGS_UC_5_METERS")]
            _5_Meters = 2,
            [Description("DPF_SETTINGS_UC_10_METERS")]
            _10_Meters = 3,
            [Description("DPF_SETTINGS_UC_20_METERS")]
            _20_Meters = 4,
            [Description("DPF_SETTINGS_UC_50_METERS")]
            _50_Meters = 5,
            [Description("DPF_SETTINGS_UC_100_METERS")]
            _100_Meters = 6
        }


        public enum ePriority
        {
            Resolution = 0,
            ImageSize = 1,
            CaptureDate = 2,

            //CaptureDateNewest = CaptureDateEarlier,
            //CaptureDateNewest = 3,
            MetaData = 3,
            ImageDimension = 4,
            Folder = 5,
            FileFormat = 6,
            CreatedData = 7,
            ModifiedDate = 8,
            eDataGridViewFileFolderColumnNames,
        }

        public enum eREG_STATUS
        {
            TRIAL_VERSION = 0,
            REGISTERED = 1,
            REGISTERED_AND_IS_ABOUT_TOEXPIRE = 2,
            EXPIRED = 4,
            PIRATED = 8,
            VOLUME_COPY = 16,

        };
        public enum Client_Product_Version
        {
            //SYSTWEAK_ASO_V3 = 1,
            /*SYSTWEAK_REGCLEANPRO_V1				=	2,
            IYOGIMNT								=	3,
            //IYOGISPC								=	3,
            PCCLEANER_ULTRAPCCARE_3					=	4,
            AOL_AOLCOMPUTERCHECKUP_V4				=	11,
            AVANQUEST_FIXITPCOPTIMIZER_V10			=	12,
            DENFEL_OPTIMIZATIONSUITE_V1				=	13,
            IYOGI_PCOPTIMIZER_V3					=	14,
            MAXPC_MAXPC_V3							=	15,
            PCRENEW_PCPERFORMANCETOOLKIT_V3			=	16,
            PCRENEW_PCTUNEUP2011_V3					=	17,
            SOFTARAMA_CAPTAINOPTIMIZER_V1			=	18,
            SOFTARAMA_PRIVACYSENTINEL_V1			=	19,
            SOFTARAMA_REGISTRY_COMMANDER			=	20,
            SPEEDMYPC_SYSTEMOPTIMIZER_V1			=	21,
            WINZIP_WINZIPDRIVERUPDATER_V1			=	22,
            WINZIP_WINZIPSYSTEMUTILITIESSUITE_V1	=	23,
            UNKNOWN									=	24,*/
            S_ASP_V2 = 2374//17
        };


        public enum LanguageIdentifier
        {
            //[DescriptionAttribute("af")]
            Lang_Afrikaans = 1,
            //[DescriptionAttribute("sq")]
            Lang_Albanian = 2,
            //[DescriptionAttribute("ar-ae")]
            Lang_Arabic__U_A_E__ = 3,
            //[DescriptionAttribute("ar-bh")]
            Lang_Arabic__Bahrain_ = 4,
            //[DescriptionAttribute("ar-dz")]
            Lang_Arabic__Algeria_ = 5,
            //[DescriptionAttribute("ar-eg")]
            Lang_Arabic__Egypt_ = 6,
            //[DescriptionAttribute("ar-iq")]
            Lang_Arabic__Iraq_ = 7,
            //[DescriptionAttribute("ar-jo")]
            Lang_Arabic__Jordan_ = 8,
            //[DescriptionAttribute("ar-kw")]
            Lang_Arabic__Kuwait_ = 9,
            //[DescriptionAttribute("ar-lb")]
            Lang_Arabic__Lebanon_ = 10,
            //[DescriptionAttribute("ar-ly")]
            Lang_Arabic__Libya_ = 11,
            //[DescriptionAttribute("ar-ma")]
            Lang_Arabic__Morocco_ = 12,
            //[DescriptionAttribute("ar-om")]
            Lang_Arabic__Oman_ = 13,
            //[DescriptionAttribute("ar-qa")]
            Lang_Arabic__Qatar_ = 14,
            //[DescriptionAttribute("ar-sa")]
            Lang_Arabic__Saudi_Arabia_ = 15,
            //[DescriptionAttribute("ar-sy")]
            Lang_Arabic__Syria_ = 16,
            //[DescriptionAttribute("ar-tn")]
            Lang_Arabic__Tunisia_ = 17,
            //[DescriptionAttribute("ar-ye")]
            Lang_Arabic__Yemen_ = 18,
            //[DescriptionAttribute("eu")]
            Lang_Basque = 19,
            //[DescriptionAttribute("be")]
            Lang_Belarusian = 20,
            //[DescriptionAttribute("bg")]
            Lang_Bulgarian = 21,
            //[DescriptionAttribute("ca")]
            Lang_Catalan = 22,
            //[DescriptionAttribute("zh-cn")]
            Lang_Chinese__PRC_ = 23,
            //[DescriptionAttribute("zh-hk")]
            Lang_Chinese__Hong_Kong_SAR_ = 24,
            //[DescriptionAttribute("zh-sg")]
            Lang_Chinese__Singapore_ = 25,
            //[DescriptionAttribute("zh-tw")]
            Lang_Chinese__Taiwan_ = 26,
            //[DescriptionAttribute("hr")]
            Lang_Croatian = 27,
            //[DescriptionAttribute("cs")]
            Lang_Czech = 28,
            //[DescriptionAttribute("da")]
            Lang_Danish = 29,
            //[DescriptionAttribute("nl")]
            Lang_Dutch__Standard_ = 30,
            //[DescriptionAttribute("nl-be")]
            Lang_Dutch__Belgium_ = 31,
            //[DescriptionAttribute("en")]
            Lang_English__Caribbean_ = 32,
            //[DescriptionAttribute("en")]
            Lang_English = 33,
            //[DescriptionAttribute("en-au")]
            Lang_English__Australia_ = 34,
            //[DescriptionAttribute("en-bz")]
            Lang_English__Belize_ = 35,
            //[DescriptionAttribute("en-ca")]
            Lang_English__Canada_ = 36,
            //[DescriptionAttribute("en-gb")]
            Lang_English__United_Kingdom_ = 37,
            //[DescriptionAttribute("en-ie")]
            Lang_English__Ireland_ = 38,
            //[DescriptionAttribute("en-jm")]
            Lang_English__Jamaica_ = 39,
            //[DescriptionAttribute("en-nz")]
            Lang_English__New_Zealand_ = 40,
            //[DescriptionAttribute("en-tt")]
            Lang_English__Trinidad_ = 41,
            //[DescriptionAttribute("en-us")]
            Lang_English__United_States_ = 42,
            //[DescriptionAttribute("en-za")]
            Lang_English__South_Africa_ = 43,
            //[DescriptionAttribute("et")]
            Lang_Estonian = 44,
            //[DescriptionAttribute("fo")]
            Lang_Faeroese = 45,
            //[DescriptionAttribute("fa")]
            Lang_Farsi = 46,
            //[DescriptionAttribute("fi")]
            Lang_Finnish = 47,
            //[DescriptionAttribute("fr")]
            Lang_French__Standard_ = 48,
            //[DescriptionAttribute("fr-be")]
            Lang_French__Belgium_ = 49,
            //[DescriptionAttribute("fr-ca")]
            Lang_French__Canada_ = 50,
            //[DescriptionAttribute("fr-ch")]
            Lang_French__Switzerland_ = 51,
            //[DescriptionAttribute("fr-lu")]
            Lang_French__Luxembourg_ = 52,
            //[DescriptionAttribute("gd")]
            Lang_Gaelic__Scotland_ = 53,
            //[DescriptionAttribute("de")]
            Lang_German__Standard_ = 54,
            //[DescriptionAttribute("de-at")]
            Lang_German__Austria_ = 55,
            //[DescriptionAttribute("de-ch")]
            Lang_German__Switzerland_ = 56,
            //[DescriptionAttribute("de-li")]
            Lang_German__Liechtenstein_ = 57,
            //[DescriptionAttribute("de-lu")]
            Lang_German__Luxembourg_ = 58,
            //[DescriptionAttribute("el")]
            Lang_Greek = 59,
            //[DescriptionAttribute("he")]
            Lang_Hebrew = 60,
            //[DescriptionAttribute("hi")]
            Lang_Hindi = 61,
            //[DescriptionAttribute("hu")]
            Lang_Hungarian = 62,
            //[DescriptionAttribute("is")]
            Lang_Icelandic = 63,
            //[DescriptionAttribute("id")]
            Lang_Indonesian = 64,
            //[DescriptionAttribute("ga")]
            Lang_Irish = 65,
            //[DescriptionAttribute("it")]
            Lang_Italian__Standard_ = 66,
            //[DescriptionAttribute("it-ch")]
            Lang_Italian__Switzerland_ = 67,
            //[DescriptionAttribute("ja")]
            Lang_Japanese = 68,
            //[DescriptionAttribute("ko")]
            Lang_Korean = 69,
            //[DescriptionAttribute("ko")]
            Lang_Korean__Johab_ = 70,
            //[DescriptionAttribute("lv")]
            Lang_Latvian = 71,
            //[DescriptionAttribute("lt")]
            Lang_Lithuanian = 72,
            //[DescriptionAttribute("mk")]
            Lang_Macedonian__FYROM_ = 73,
            //[DescriptionAttribute("ms")]
            Lang_Malaysian = 74,
            //[DescriptionAttribute("mt")]
            Lang_Maltese = 75,
            //[DescriptionAttribute("no")]
            Lang_Norwegian__Nynorsk_ = 76,
            //[DescriptionAttribute("no")]
            Lang_Norwegian__Bokmal_ = 77,
            //[DescriptionAttribute("pl")]
            Lang_Polish = 78,
            //[DescriptionAttribute("pt")]
            Lang_Portuguese__Portugal_ = 79,
            //[DescriptionAttribute("pt-br")]
            Lang_Portuguese__Brazil_ = 80,
            //[DescriptionAttribute("rm")]
            Lang_Rhaeto_Romanic = 81,
            //[DescriptionAttribute("ro")]
            Lang_Romanian = 82,
            //[DescriptionAttribute("ro-mo")]
            Lang_Romanian__Republic_of_Moldova_ = 83,
            //[DescriptionAttribute("ru")]
            Lang_Russian = 84,
            //[DescriptionAttribute("ru-mo")]
            Lang_Russian__Republic_of_Moldova_ = 85,
            //[DescriptionAttribute("sz")]
            Lang_Sami__Lappish_ = 86,
            //[DescriptionAttribute("sr")]
            Lang_Serbian__Cyrillic_ = 87,
            //[DescriptionAttribute("sr")]
            Lang_Serbian__Latin_ = 88,
            //[DescriptionAttribute("sk")]
            Lang_Slovak = 89,
            //[DescriptionAttribute("sl")]
            Lang_Slovenian = 90,
            //[DescriptionAttribute("sb")]
            Lang_Sorbian = 91,
            //[DescriptionAttribute("es")]
            Lang_Spanish__Spain_ = 92,
            //[DescriptionAttribute("es-ar")]
            Lang_Spanish__Argentina_ = 93,
            //[DescriptionAttribute("es-bo")]
            Lang_Spanish__Bolivia_ = 94,
            //[DescriptionAttribute("es-cl")]
            Lang_Spanish__Chile_ = 95,
            //[DescriptionAttribute("es-co")]
            Lang_Spanish__Colombia_ = 96,
            //[DescriptionAttribute("es-cr")]
            Lang_Spanish__Costa_Rica_ = 97,
            //[DescriptionAttribute("es-do")]
            Lang_Spanish__Dominican_Republic_ = 98,
            //[DescriptionAttribute("es-ec")]
            Lang_Spanish__Ecuador_ = 99,
            //[DescriptionAttribute("es-gt")]
            Lang_Spanish__Guatemala_ = 100,
            //[DescriptionAttribute("es-hn")]
            Lang_Spanish__Honduras_ = 101,
            //[DescriptionAttribute("es-mx")]
            Lang_Spanish__Mexico_ = 102,
            //[DescriptionAttribute("es-ni")]
            Lang_Spanish__Nicaragua_ = 103,
            //[DescriptionAttribute("es-pa")]
            Lang_Spanish__Panama_ = 104,
            //[DescriptionAttribute("es-pe")]
            Lang_Spanish__Peru_ = 105,
            //[DescriptionAttribute("es-pr")]
            Lang_Spanish__Puerto_Rico_ = 106,
            //[DescriptionAttribute("es-py")]
            Lang_Spanish__Paraguay_ = 107,
            //[DescriptionAttribute("es-sv")]
            Lang_Spanish__El_Salvador_ = 108,
            //[DescriptionAttribute("es-uy")]
            Lang_Spanish__Uruguay_ = 109,
            //[DescriptionAttribute("es-ve")]
            Lang_Spanish__Venezuela_ = 110,
            //[DescriptionAttribute("sx")]
            Lang_Sutu = 111,
            //[DescriptionAttribute("sv")]
            Lang_Swedish = 112,
            //[DescriptionAttribute("sv-fi")]
            Lang_Swedish__Finland_ = 113,
            //[DescriptionAttribute("th")]
            Lang_Thai = 114,
            //[DescriptionAttribute("ts")]
            Lang_Tsonga = 115,
            //[DescriptionAttribute("tn")]
            Lang_Tswana = 116,
            //[DescriptionAttribute("tr")]
            Lang_Turkish = 117,
            //[DescriptionAttribute("uk")]
            Lang_Ukrainian = 118,
            //[DescriptionAttribute("ur")]
            Lang_Urdu = 119,
            //[DescriptionAttribute("ve")]
            Lang_Venda = 120,
            //[DescriptionAttribute("vi")]
            Lang_Vietnamese = 121,
            //[DescriptionAttribute("xh")]
            Lang_Xhosa = 122,
            //[DescriptionAttribute("ji")]
            Lang_Yiddish = 123,
            //[DescriptionAttribute("zu")]
            Lang_Zulu = 124,

        }
        public enum ACTIVATION_MESSAGES
        {
            /////////
            //THE THREE VALUES BELOW ARE SAME
            //BUT ARE USED JUST TO COVER SLOPPY PROGRAMMING 
            MSG_KEY_RESERVED = -1,
            MSG_KEY_EVERYTHING_OK = 0,
            MSG_KEY_GOOD_CONTINUE = 1,
            ////////////////////////////////

            MSG_KEY_PIRATED = 2, // Pirated key
            MSG_KEY_FALSE = 3, // Invalid key == > THIS IS THE DEFAULT MESSAGE NOW ON
            MSG_KEY_BLOCKED = 4, // key is blocked
            MSG_KEY_REFUND = 5, // Key refunded
            MSG_KEY_EXPIRED = 6, // Key expired        
            // MSG_KEY_LIMIT_EXCEEDED = 7 , // Key used more than limit
            MSG_KEY_LIMIT_EXCEEDED = 7 | 15, // Key used more than limit // added by gautam on 08-05-2015
            MSG_KEY_ALREADY_NOT_IN_DB = 8, // Key not generated by portal
            MSG_KEY_FOUND_FOR_OTHER_PRODUCT = 9, // Key belongs to other product


            // DeactivateReasons
            MSG_AUTO_EXPIRE_AFTER_TIME = 21,
            MSG_REFUND = 22,
            MSG_CHARGEBACK = 23,
            MSG_EXCEED_DOWNLOAD = 24,
            MSG_MISUSED = 25,


        };
        public enum eKeyActionToTake
        {

            /////////
            //THE THREE VALUES BELOW ARE SAME
            //BUT ARE USED JUST TO COVER SLOPPY PROGRAMMING 
            KEY_RESERVED = -1,
            KEY_EVERYTHING_OK = 0,
            KEY_GOOD_CONTINUE = 1,
            ////////////////////////////////


            KEY_BAD_EXPIRE_IT = 2,
            KEY_BAD_DO_NOTHING = 3,

            // show message cases
            KEY_BAD_ONLY_SHOW_MSG = 4,
            KEY_BAD_EXPIRE_IT_SHOW_MSG = 5,
            KEY_BAD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER = 6,

            // open browser cases
            KEY_BAD_ONLY_OPEN_BROWSER = 7,
            KEY_BAD_EXPIRE_IT_OPEN_BROWSER = 8,

            // fire and download exe WITH PARAMS AS PROVIDED BY WEBSERVICE
            KEY_BAD_DOWNLOAD_EXE_AND_EXEC_SILENT = 9,
            KEY_BAD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT = 10,

            // new key actions
            INSTALL_A_NEW_KEY = 11,
            SET_DEFAULT_KEY = 12,
            GET_AND_INSTALL_A_NEW_KEY_ONBASEOF_CURRENT_KEY = 13,
            // ....

            EXTEND_KEY_EXPIRY_DAYS = 14,

            KEY_GOOD_BUT_DO_NOT_ALLOW_REGISTRATION = 15, /* Key is bad do not allow registration with this key */


            // In case if key         
            KEY_GOOD_EXPIRE_IT = 16,

            // Campaign Cases        
            KEY_GOOD_ONLY_SHOW_MSG = 17,
            KEY_GOOD_EXPIRE_IT_SHOW_MSG = 18,
            KEY_GOOD_EXPIRE_IT_SHOW_MSG_OPEN_BROWSER = 19,

            // open browser cases        
            KEY_GOOD_ONLY_OPEN_BROWSER = 20,
            KEY_GOOD_EXPIRE_IT_OPEN_BROWSER = 21,

            // fire and download exe WITH PARAMS AS PROVIDED BY WEBSERVICE        
            KEY_GOOD_DOWNLOAD_EXE_AND_EXEC_SILENT = 22,
            KEY_GOOD_EXPIRE_IT_DOWNLOAD_EXE_AND_EXEC_SILENT = 23,

            // This could be done on base of case 11, 12, 13        
            KEY_BAD_MAKE_IT_GOOD = 24,

            // In case show campaign information to the users who are going to expire        
            KEY_GOOD_DO_WE_HAVE_TO_SHOW_REMINDER = 25,
        };


        public enum eVerificationTime
        {
            AppLaunch,
            Register

        }

        public enum ACTIVATION_STATUS
        {
            ONLINE_ACTIVATION_NEEDED = 1,								// is activation needed in current project
            ACTIVATION_TAKE_ACTION_ON_BASE_RETURNED_FLAGS = 1		// whether to take action returned by service in current project
        };

    }
}
