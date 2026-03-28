using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.App_code
{

    public static class Constants
    {
        #region thread synchronization
        public static ManualResetEvent oManualReset = new ManualResetEvent(true);
        public static bool m_pStopEvent = false;
        #endregion

        #region  Enums
        /* these are the usual params to add in to the URL, for tracking */
        public enum enum_append_params
        {
            utm_source = 0,
            utm_campaign = 1,
            utm_medium = 2,
            affiliateid = 3,
            utm_content = 4,
            utm_term = 5,
            IsRegistered = 6,
            regkey = 7,
            numberOfdays = 8,
            ProductVersion = 9,
            lang = 10,
            loc = 11,
            mac = 12,
        }
        #endregion

        #region Constants & ReadOnly fields
        public const string G_COMPANY_NAME_ABV = "Systweak";
        //public const string G_COMPANY_NAME_ABV = "Jawego Partners LLC";
        public const string G_PRODUCT_ABB_NAME = "DPFW";
        //public const string G_PRODUCT_ABB_NAME = "du_plus";


        /************************************************************************
    List of virtual MAC Address 
    Here we are not restricting these machine id from send, we 
    are only searching in loop from all machine id's 
    if it is present in that array, try to take any other machine id user 
    have which can be considered as real
    /************************************************************************/
        public const int VIRTUAL_ADDRESS_COUNT = 2;

        public static readonly string[] saVirtualMacAddress = {
            //_T("00038A000015"),		// we got this machine id more than 700 +  times, so assuming
            "005056C00001",
            "005056C00008"
        };




        public static readonly string[] enum_append_paramsNamesDefault =
        {
        "utm_source",		/*utm_source		= 0, */
        "utm_campaign",		/*utm_campaign	= 1, */
        "utm_medium",		/*utm_medium		= 2, */
        "affiliateid",		/*affiliateid		= 3, */
        "utm_content",		/*utm_content		= 4, */
        "utm_term",		/*utm_term		= 5, */
        "IR",		/*IsRegistered	= 6, */
        "RK",		/*regkey			= 7,*/
        "ND",		/*numberOfdays	= 8,*/
        "ProductVersion",		/*ProductVersion	= 9,*/
        "lang",		/*lang			= 10,*/
        "loc",		/*loc				= 11,  application location from where it is called */
        "mac",		/*mac				= 12, user machine id */
        };

        public static readonly string[] enum_append_paramsNames =
        {
            "<utm_src>",	/*utm_source		= 0, */
	        "<utm_cpgn>",	/*utm_campaign	= 1, */
	        "<utm_med>",	/*utm_medium		= 2, */
	        "<affid>",	/*affiliateid		= 3, */
	        "<utm_con>",	/*utm_content		= 4, */
	        "<utm_term>",	/*utm_term		= 5, */
	        "<ir>",	/*IsRegistered	= 6, */
	        "<rk>",	/*regkey			= 7,*/
	        "<nd>",	/*numberOfdays	= 8,*/
	        "<prodver>",	/*ProductVersion	= 9,*/
	        "<lang>",	/*lang			= 10,*/
	        "<loc>",	/*loc				= 11,  application location from where it is called */
	        "<mac>",	/*mac				= 12, user machine id */
        };

        //const CString BASE_FORMAT_URL_FOR_PARMETERES = _T("utm_source=<utm_source>&utm_campaign=<utm_campaign>&utm_medium=<utm_medium>&affiliateid=<affiliateid>");
        public readonly static string BASE_FORMAT_URL_FOR_PARMETERES =
            "utm_source=<utm_src>&utm_campaign=<utm_cpgn>&utm_medium=<utm_med>&utm_content=<utm_con>&affiliateid=<affid>&utm_term=<utm_term>&ir=<ir>&RK=<rk>&ND=<nd>&ProductVersion=<prodver>&lang=lang>&loc=<apploc>";





        public const int _MAX_PATH = 260; /* max. length of full pathname */
        public const int DIGCF_ALLCLASSES = (0x00000004);
        public const int DIGCF_PRESENT = (0x00000002);

        public const int MAX_DEV_LEN = 1000;//The return value of maximum length

        public const int SPDRP_DEVICEDESC = (0x00000000);// DeviceDesc (R/W)
        public const int SPDRP_HARDWAREID = (0x00000001);// HardwareID (R/W)
        public const int SPDRP_COMPATIBLEIDS = (0x00000002);// CompatibleIDs (R/W)
        public const int SPDRP_UNUSED0 = (0x00000003);// unused
        public const int SPDRP_SERVICE = (0x00000004);// Service (R/W)
        public const int SPDRP_UNUSED1 = (0x00000005);// unused
        public const int SPDRP_UNUSED2 = (0x00000006);// unused
        public const int SPDRP_CLASS = (0x00000007);// Class (R--tied to ClassGUID)
        public const int SPDRP_CLASSGUID = (0x00000008);// ClassGUID (R/W)
        public const int SPDRP_DRIVER = (0x00000009);// Driver (R/W)
        public const int SPDRP_CONFIGFLAGS = (0x0000000A);// ConfigFlags (R/W)
        public const int SPDRP_MFG = (0x0000000B);// Mfg (R/W)
        public const int SPDRP_FRIENDLYNAME = (0x0000000C);// FriendlyName (R/W)
        public const int SPDRP_LOCATION_INFORMATION = (0x0000000D);// LocationInformation (R/W)
        public const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = (0x0000000E);// PhysicalDeviceObjectName (R)
        public const int SPDRP_CAPABILITIES = (0x0000000F);// Capabilities (R)
        public const int SPDRP_UI_NUMBER = (0x00000010);// UiNumber (R)
        public const int SPDRP_UPPERFILTERS = (0x00000011);// UpperFilters (R/W)
        public const int SPDRP_LOWERFILTERS = (0x00000012);// LowerFilters (R/W)
        public const int SPDRP_BUSTYPEGUID = (0x00000013);// BusTypeGUID (R)
        public const int SPDRP_LEGACYBUSTYPE = (0x00000014);// LegacyBusType (R)
        public const int SPDRP_BUSNUMBER = (0x00000015);// BusNumber (R)
        public const int SPDRP_ENUMERATOR_NAME = (0x00000016);// Enumerator Name (R)
        public const int SPDRP_SECURITY = (0x00000017);// Security (R/W, binary form)
        public const int SPDRP_SECURITY_SDS = (0x00000018);// Security=(W, SDS form)
        public const int SPDRP_DEVTYPE = (0x00000019);// Device Type (R/W)
        public const int SPDRP_EXCLUSIVE = (0x0000001A);// Device is exclusive-access (R/W)
        public const int SPDRP_CHARACTERISTICS = (0x0000001B);// Device Characteristics (R/W)
        public const int SPDRP_ADDRESS = (0x0000001C);// Device Address (R)
        public const int SPDRP_UI_NUMBER_DESC_FORMAT = (0X0000001D);// UiNumberDescFormat (R/W)
        public const int SPDRP_DEVICE_POWER_DATA = (0x0000001E);// Device Power Data (R)
        public const int SPDRP_REMOVAL_POLICY = (0x0000001F);// Removal Policy (R)
        public const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = (0x00000020);// Hardware Removal Policy (R)
        public const int SPDRP_REMOVAL_POLICY_OVERRIDE = (0x00000021);// Removal Policy Override (RW)
        public const int SPDRP_INSTALL_STATE = (0x00000022);// Device Install State (R)
        public const int SPDRP_LOCATION_PATHS = (0x00000023);// Device Location Paths (R)
        public const int SPDRP_BASE_CONTAINERID = (0x00000024);// Base ContainerID (R)

        public const int SPDRP_MAXIMUM_PROPERTY = (0x00000025);// Upper bound on ordinals

        public static volatile int m_iCancel_Func;
        public static volatile int m_hStopEvent;
        /////////////////////////////////////////////////////////////////////
        //Log Severity Level
        public const int LOG_SEVERITY_DEBUG = 0x01;
        public const int LOG_SEVERITY_INFO = 0x02;
        public const int LOG_SEVERITY_WARNING = 0x03;
        public const int LOG_SEVERITY_ERROR = 0x04;
        public const int LOG_SEVERITY_FATAL = 0x05;

        //////////////////////////////////////////////////////////////////////////
        //Message handling

        public const int NEED_RESTART = 0x06;
        public const int NOT_UNINSTALL = 0x07;
        public const int ACCESS_DENIED = 0x08;
        public const int INVALID_NAME = 0x09;
        public const int SERVICE_EXISTS = 0x10;
        public const int DUP_NAME = 0x11;



        /////////////////////////////////////////////////////////////////////
        //Returned Identifiers
        public const int CANCEL_SCAN = 0x12;
        public const int CANCEL_BACKUP = 0x13;
        public const int CANCEL_RESTORE = 0x14;
        public const int CANCEL_DELETE_OLD_DRIVERS = 0x15;
        public const int CANCEL_UNINSTALL = 0x16;
        public const int CANCEL_UNKNOWN_SCAN = 0x17;
        public const int CANCEL_UPDATE_DRIVERS = 0x18;
        public const int CANCEL_BACKUP_NON_PNP = 0x19;
        public const int SUCCESS = 0x20;
        public const int FAIL = 0x21;
        public const int INCORRECT_PARAM = 0x22;
        public const int DEFAULT = 0x23;
        public const int OTHER_DRIVER_INSTALLATION_IN_PROGRESS = 0x28;

        public const int NEED_DOWNLOAD = 0x24;
        public const int CANCEL_XML_UPLOAD = 0x25;
        public const int NETCONNECTION_FAIL = 0x26;
        public const int CANCEL_INSTALL = 0x27;


        //--------------------------------------------------------------
        // Configuration Manager return status codes
        //--------------------------------------------------------------

        public const int CR_SUCCESS = 0x00000000;
        public const int CR_DEFAULT = 0x00000001;
        public const int CR_OUT_OF_MEMORY = 0x00000002;
        public const int CR_INVALID_POINTER = 0x00000003;
        public const int CR_INVALID_FLAG = 0x00000004;
        public const int CR_INVALID_DEVNODE = 0x00000005;
        public const int CR_INVALID_DEVINST = CR_INVALID_DEVNODE;
        public const int CR_INVALID_RES_DES = 0x00000006;
        public const int CR_INVALID_LOG_CONF = 0x00000007;
        public const int CR_INVALID_ARBITRATOR = 0x00000008;
        public const int CR_INVALID_NODELIST = 0x00000009;
        public const int CR_DEVNODE_HAS_REQS = 0x0000000A;
        public const int CR_DEVINST_HAS_REQS = CR_DEVNODE_HAS_REQS;
        public const int CR_INVALID_RESOURCEID = 0x0000000B;
        public const int CR_DLVXD_NOT_FOUND = 0x0000000C;   // WIN 95 ONLY
        public const int CR_NO_SUCH_DEVNODE = 0x0000000D;
        public const int CR_NO_SUCH_DEVINST = CR_NO_SUCH_DEVNODE;
        public const int CR_NO_MORE_LOG_CONF = 0x0000000E;
        public const int CR_NO_MORE_RES_DES = 0x0000000F;
        public const int CR_ALREADY_SUCH_DEVNODE = 0x00000010;
        public const int CR_ALREADY_SUCH_DEVINST = CR_ALREADY_SUCH_DEVNODE;
        public const int CR_INVALID_RANGE_LIST = 0x00000011;
        public const int CR_INVALID_RANGE = 0x00000012;
        public const int CR_FAILURE = 0x00000013;
        public const int CR_NO_SUCH_LOGICAL_DEV = 0x00000014;
        public const int CR_CREATE_BLOCKED = 0x00000015;
        public const int CR_NOT_SYSTEM_VM = 0x00000016;   // WIN 95 ONLY
        public const int CR_REMOVE_VETOED = 0x00000017;
        public const int CR_APM_VETOED = 0x00000018;
        public const int CR_INVALID_LOAD_TYPE = 0x00000019;
        public const int CR_BUFFER_SMALL = 0x0000001A;
        public const int CR_NO_ARBITRATOR = 0x0000001B;
        public const int CR_NO_REGISTRY_HANDLE = 0x0000001C;
        public const int CR_REGISTRY_ERROR = 0x0000001D;
        public const int CR_INVALID_DEVICE_ID = 0x0000001E;
        public const int CR_INVALID_DATA = 0x0000001F;
        public const int CR_INVALID_API = 0x00000020;
        public const int CR_DEVLOADER_NOT_READY = 0x00000021;
        public const int CR_NEED_RESTART = 0x00000022;
        public const int CR_NO_MORE_HW_PROFILES = 0x00000023;
        public const int CR_DEVICE_NOT_THERE = 0x00000024;
        public const int CR_NO_SUCH_VALUE = 0x00000025;
        public const int CR_WRONG_TYPE = 0x00000026;
        public const int CR_INVALID_PRIORITY = 0x00000027;
        public const int CR_NOT_DISABLEABLE = 0x00000028;
        public const int CR_FREE_RESOURCES = 0x00000029;
        public const int CR_QUERY_VETOED = 0x0000002A;
        public const int CR_CANT_SHARE_IRQ = 0x0000002B;
        public const int CR_NO_DEPENDENT = 0x0000002C;
        public const int CR_SAME_RESOURCES = 0x0000002D;
        public const int CR_NO_SUCH_REGISTRY_KEY = 0x0000002E;
        public const int CR_INVALID_MACHINENAME = 0x0000002F;   // NT ONLY
        public const int CR_REMOTE_COMM_FAILURE = 0x00000030;   // NT ONLY
        public const int CR_MACHINE_UNAVAILABLE = 0x00000031;   // NT ONLY
        public const int CR_NO_CM_SERVICES = 0x00000032;   // NT ONLY
        public const int CR_ACCESS_DENIED = 0x00000033;   // NT ONLY
        public const int CR_CALL_NOT_IMPLEMENTED = 0x00000034;
        public const int CR_INVALID_PROPERTY = 0x00000035;
        public const int CR_DEVICE_INTERFACE_ACTIVE = 0x00000036;
        public const int CR_NO_SUCH_DEVICE_INTERFACE = 0x00000037;
        public const int CR_INVALID_REFERENCE_STRING = 0x00000038;
        public const int CR_INVALID_CONFLICT_LIST = 0x00000039;
        public const int CR_INVALID_INDEX = 0x0000003A;
        public const int CR_INVALID_STRUCTURE_SIZE = 0x0000003B;
        public const int NUM_CR_RESULTS = 0x0000003C;


        /////////////////////////////////////////////////////////////////////
        ////////////////Directory Constants/////////////////////////////////
        public const int CSIDL_DESKTOP = 0x0000;        // <desktop>
        public const int CSIDL_INTERNET = 0x0001;        // Internet Explorer (icon on desktop)
        public const int CSIDL_PROGRAMS = 0x0002;        // Start Menu\Programs
        public const int CSIDL_CONTROLS = 0x0003;        // My Computer\Control Panel
        public const int CSIDL_PRINTERS = 0x0004;        // My Computer\Printers
        public const int CSIDL_PERSONAL = 0x0005;        // My Documents
        public const int CSIDL_FAVORITES = 0x0006;        // <user name>\Favorites
        public const int CSIDL_STARTUP = 0x0007;        // Start Menu\Programs\Startup
        public const int CSIDL_RECENT = 0x0008;        // <user name>\Recent
        public const int CSIDL_SENDTO = 0x0009;        // <user name>\SendTo
        public const int CSIDL_BITBUCKET = 0x000a;        // <desktop>\Recycle Bin
        public const int CSIDL_STARTMENU = 0x000b;        // <user name>\Start Menu
        public const int CSIDL_MYDOCUMENTS = CSIDL_PERSONAL; //  Personal was just a silly name for My Documents
        public const int CSIDL_MYMUSIC = 0x000d;        // "My Music" folder
        public const int CSIDL_MYVIDEO = 0x000e;        // "My Videos" folder
        public const int CSIDL_DESKTOPDIRECTORY = 0x0010;        // <user name>\Desktop
        public const int CSIDL_DRIVES = 0x0011;        // My Computer
        public const int CSIDL_NETWORK = 0x0012;        // Network Neighborhood (My Network Places)
        public const int CSIDL_NETHOOD = 0x0013;        // <user name>\nethood
        public const int CSIDL_FONTS = 0x0014;        // windows\fonts
        public const int CSIDL_TEMPLATES = 0x0015;
        public const int CSIDL_COMMON_STARTMENU = 0x0016;        // All Users\Start Menu
        public const int CSIDL_COMMON_PROGRAMS = 0X0017;        // All Users\Start Menu\Programs
        public const int CSIDL_COMMON_STARTUP = 0x0018;        // All Users\Startup
        public const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;        // All Users\Desktop
        public const int CSIDL_APPDATA = 0x001a;        // <user name>\Application Data
        public const int CSIDL_PRINTHOOD = 0x001b;        // <user name>\PrintHood


        public const int CSIDL_LOCAL_APPDATA = 0x001c;        // <user name>\Local Settings\Applicaiton Data (non roaming)


        public const int CSIDL_ALTSTARTUP = 0x001d;        // non localized startup
        public const int CSIDL_COMMON_ALTSTARTUP = 0x001e;        // non localized common startup
        public const int CSIDL_COMMON_FAVORITES = 0x001f;


        public const int CSIDL_INTERNET_CACHE = 0x0020;
        public const int CSIDL_COOKIES = 0x0021;
        public const int CSIDL_HISTORY = 0x0022;
        public const int CSIDL_COMMON_APPDATA = 0x0023;        // All Users\Application Data
        public const int CSIDL_WINDOWS = 0x0024;        // GetWindowsDirectory()
        public const int CSIDL_SYSTEM = 0x0025;        // GetSystemDirectory()
        public const int CSIDL_PROGRAM_FILES = 0x0026;        // C:\Program Files
        public const int CSIDL_MYPICTURES = 0x0027;        // C:\Program Files\My Pictures


        public const int CSIDL_PROFILE = 0x0028;        // USERPROFILE
        public const int CSIDL_SYSTEMX86 = 0x0029;        // x86 system directory on RISC
        public const int CSIDL_PROGRAM_FILESX86 = 0x002a;        // x86 C:\Program Files on RISC


        public const int CSIDL_PROGRAM_FILES_COMMON = 0x002b;        // C:\Program Files\Common


        public const int CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c;        // x86 Program Files\Common on RISC
        public const int CSIDL_COMMON_TEMPLATES = 0x002d;        // All Users\Templates


        public const int CSIDL_COMMON_DOCUMENTS = 0x002e;        // All Users\Documents
        public const int CSIDL_COMMON_ADMINTOOLS = 0x002f;        // All Users\Start Menu\Programs\Administrative Tools
        public const int CSIDL_ADMINTOOLS = 0x0030;        // <user name>\Start Menu\Programs\Administrative Tools


        public const int CSIDL_CONNECTIONS = 0x0031;        // Network and Dial-up Connections
        public const int CSIDL_COMMON_MUSIC = 0x0035;        // All Users\My Music
        public const int CSIDL_COMMON_PICTURES = 0x0036;        // All Users\My Pictures
        public const int CSIDL_COMMON_VIDEO = 0x0037;        // All Users\My Video
        public const int CSIDL_RESOURCES = 0x0038;        // Resource Direcotry


        public const int CSIDL_RESOURCES_LOCALIZED = 0x0039;        // Localized Resource Direcotry


        public const int CSIDL_COMMON_OEM_LINKS = 0x003a;        // Links to All Users OEM specific apps
        public const int CSIDL_CDBURN_AREA = 0x003b;        // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
                                                            // unused                               0x003c
        public const int CSIDL_COMPUTERSNEARME = 0x003d;        // Computers Near Me (computered from Workgroup membership)



        /////////////////////////////////////////////////////////////
        //Defined in WinNt.h
        public const int PROCESSOR_ARCHITECTURE_INTEL = 0;
        public const int PROCESSOR_ARCHITECTURE_MIPS = 1;
        public const int PROCESSOR_ARCHITECTURE_ALPHA = 2;
        public const int PROCESSOR_ARCHITECTURE_PPC = 3;
        public const int PROCESSOR_ARCHITECTURE_SHX = 4;
        public const int PROCESSOR_ARCHITECTURE_ARM = 5;
        public const int PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const int PROCESSOR_ARCHITECTURE_ALPHA64 = 7;
        public const int PROCESSOR_ARCHITECTURE_MSIL = 8;
        public const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const int PROCESSOR_ARCHITECTURE_IA32_ON_WIN64 = 10;

        public const int PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;


        //
        // RtlVerifyVersionInfo() os product type values
        //

        public const int VER_NT_WORKSTATION = 0x0000001;
        public const int VER_NT_DOMAIN_CONTROLLER = 0x0000002;
        public const int VER_NT_SERVER = 0x0000003;
        //
        // dwPlatformId defines:
        //

        public const int VER_PLATFORM_WIN32s = 0;
        public const int VER_PLATFORM_WIN32_WINDOWS = 1;
        public const int VER_PLATFORM_WIN32_NT = 2;

        public const uint APPLICATION_ERROR_MASK = 0x20000000;
        public const uint ERROR_SEVERITY_SUCCESS = 0x00000000;
        public const uint ERROR_SEVERITY_INFORMATIONAL = 0x40000000;
        public const uint ERROR_SEVERITY_WARNING = 0x80000000;
        public const uint ERROR_SEVERITY_ERROR = 0xC0000000;
        public const uint ERROR_FILE_EXISTS = (uint)unchecked(80L);
        ////////////////////////////////////////////////////88464
        public static string ADU_KEY = cGlobalSettings.cRegDetails.Key;
        public static string REGISTERED_EMAIL = cResourceManager.LoadString("IDS_SUPPORT_ADDRESS");




        // SFI_xxx: specific parts of information from a file's version info block.
        // Indices correspond to m_lpszStringFileInfos[] string array.
        // See GetVersionInfo member function for usage.
        //*************************************************************************
        public const int SFI_COMPANYNAME = 0;
        public const int SFI_FILEDESCRIPTION = 1;
        public const int SFI_FILEVERSION = 2;
        public const int SFI_INTERNALNAME = 3;
        public const int SFI_LEGALCOPYRIGHT = 4;
        public const int SFI_ORIGINALFILENAME = 5;
        public const int SFI_PRODUCTNAME = 6;
        public const int SFI_PRODUCTVERSION = 7;
        public const int SFI_COMMENTS = 8;
        public const int SFI_LEGALTRADEMARKS = 9;
        public const int SFI_PRIVATEBUILD = 10;
        public const int SFI_SPECIALBUILD = 11;
        public const int SFI_FIRST = SFI_COMPANYNAME;
        public const int SFI_LAST = SFI_SPECIALBUILD;



        public const string BACKUP_FOLDER = "Backup\\";
        public const string DOWNLOAD_FOLDER = "Download\\";

        public const string REGSTR_VAL_SUPPORT_URL = AppConstants.HelpURL;
        public const string REGSTR_VAL_HELP_URL_DE = AppConstants.ContactURL;
        public const string REGSTR_VAL_HELP_URL_INT = AppConstants.ContactURL;
        public const string STAppStrings_STR_11122821 = "BaseFormatURLreg";
        public const string STAppStrings_STR_11142922 = "BaseFormatURLreg";
        public const string STAppStrings_STR_11114861 = "BaseFormatURLunreg";
        public const string STAppStrings_STR_11141591 = "BaseFormatURLunreg";

        public static string REGSTR_BASEKEY = cGlobalSettings.VersionIndependentRegKey.Substring(cGlobalSettings.VersionIndependentRegKey.IndexOf("\\"));
        public const string REGSTR_VAL_INFPATH = "InfPath";
        #endregion



        #region static fields

        /*public static string m_strUTM_SOURCE = "googleadw";
        public static string m_strUTM_CAMPAIGN = "adw_dpf_installer";
        public static string m_strUTM_MEDIUM = "";
        public static string m_strAFFILIATEID = "";
        public static string m_strUTM_CONTENT = "";
        public static string m_strBaseFormatURLunreg = "";
        public static string m_strBaseFormatURLreg = "";
        public static string m_strASOBUILDFOR = "googleadw";
        public static string m_strASOCampaignID = "adw_dpf_installer";
        public static string m_strSupportUrl = "";
        public static string m_strHelpUrlDE = "";
        public static string m_strHelpUrlINT = "";*/

        ///Global values to evaluate the state/////////////
        ///////////////////////////////////////////////////
        //public static DriverScanUpdate.FUNCT_IDENTIFER m_Func_Identifier;
        //public static DriverScanUpdate m_DrvScanUpdtCustom;
        //public static DriverScanner m_DllMainClass;
        //public static CDrvExclusion m_drvExclusion;
        //public static UpdateServiceHelper m_UpdateService;
        //public static Dictionary<string, string> myDownloadmap = new Dictionary<string, string>();
        //public static Dictionary<string, string> mapDownloadDistinctURL = new Dictionary<string, string>();
        public static int m_nErrorCount = 0;

        ////////////////////////////////////////////////////


        //public static string strTempPath { get; set; }
        //public static bool IsBackupAll { get; set; }
        //CString strMainAppLoc;										//contains the Backup Location	

        /// <summary>
        /// Backup location
        /// </summary>


        //public static bool IsInstallClick { get; set; }









        public static string m_hardwareHash;
        #endregion

        #region constructor
        /*static Constants()
        {
            //m_DrvScanUpdtCustom = new DriverScanUpdate();
            //m_DllMainClass = new DriverScanner();
            //m_drvExclusion = new CDrvExclusion();
            //m_UpdateService = new UpdateServiceHelper();
            //strTempPath = CreateTempFolder(AppConstants.ApplicationName);
            //strMainAppLoc = CreateAppDataFolder(AppConstants.ApplicationName);
            //strBkpLoc = CreateBackupLocation();
            //IsBackupAll = true;
            GetAffiliateBuildInfo();
            //strDownloadLoc = Path.Combine(strMainAppLoc, "Download");
        }
        #endregion

        #region Public methods




        //This function will read the affiliate build information from registry
        public static void GetAffiliateBuildInfo()
        {

            //m_strUTM_SOURCE = string.Empty;
            //m_strUTM_CAMPAIGN = string.Empty;
            //m_strUTM_MEDIUM = string.Empty;
            //m_strAFFILIATEID = string.Empty;
            //m_strUTM_CONTENT = string.Empty;
            //m_strBaseFormatURLunreg = string.Empty;
            //m_strBaseFormatURLreg = string.Empty;
            return;
            RegistryHelper reg = null;
            try
            {

                reg = new RegistryHelper();
                reg.Permission = cRegistry.KEY_READ;
                string Path = REGSTR_BASEKEY;
                //_ASSERTE(Path.GetLength()); 

                if (reg.Open(cRegistry.ROOT_KEY.HKEY_LOCAL_MACHINE, Path))
                {

                    // changed to utm_source and utm_campain as suggested by CG sir, as these values are not written by adu
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_source]/ *_T("ASOBUILDFOR")* /))
                    {
                        m_strASOBUILDFOR = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_source]/ *_T("ASOBUILDFOR")* /, m_strASOBUILDFOR);
                        if (m_strASOBUILDFOR.Length > 0)
                        {
                            m_strASOBUILDFOR = m_strASOBUILDFOR.ToLower().Trim();
                            //m_strASOBUILDFOR.Trim();
                        }
                    }

                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_campaign]/ *_T("ASO3CAM")* /))
                    {
                        m_strASOCampaignID = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_campaign]/ *_T("ASO3CAM")* /, m_strASOCampaignID);
                        if (m_strASOCampaignID.Length > 0)
                        {
                            m_strASOCampaignID = m_strASOCampaignID.ToLower().Trim();
                            //m_strASOCampaignID.Trim();
                        }
                    }


                    // utm_source
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_source]))
                    {
                        m_strUTM_SOURCE = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_source], m_strUTM_SOURCE);
                        if (m_strUTM_SOURCE.Length > 0)
                        {
                            m_strUTM_SOURCE = m_strUTM_SOURCE.ToLower().Trim();
                            //m_strUTM_SOURCE.Trim();
                        }
                    }

                    // utm_campaign
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_campaign]))
                    {
                        m_strUTM_CAMPAIGN = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_campaign], m_strUTM_CAMPAIGN);
                        if (m_strUTM_CAMPAIGN.Length > 0)
                        {
                            m_strUTM_CAMPAIGN = m_strUTM_SOURCE.ToLower().Trim();
                            //m_strUTM_CAMPAIGN.Trim();
                        }
                    }

                    // utm_medium
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_medium]))
                    {
                        m_strUTM_MEDIUM = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_medium], m_strUTM_MEDIUM);
                        if (m_strUTM_MEDIUM.Length > 0)
                        {
                            m_strUTM_MEDIUM = m_strUTM_SOURCE.ToLower().Trim();
                            //m_strUTM_MEDIUM.Trim();
                        }
                    }

                    // affiliateid
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.affiliateid]))
                    {
                        m_strAFFILIATEID = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.affiliateid], m_strAFFILIATEID);
                        if (m_strAFFILIATEID.Length > 0)
                        {
                            m_strAFFILIATEID = m_strUTM_SOURCE.ToLower().Trim();
                            //m_strAFFILIATEID.Trim();
                        }
                    }

                    // utm_content
                    if (reg.VerifyValue(enum_append_paramsNamesDefault[(int)enum_append_params.utm_content]))
                    {
                        m_strUTM_CONTENT = string.Empty;
                        reg.ReadString(enum_append_paramsNamesDefault[(int)enum_append_params.utm_content], m_strUTM_CONTENT);
                        if (m_strUTM_CONTENT.Length > 0)
                        {
                            m_strUTM_CONTENT = m_strUTM_SOURCE.ToLower().Trim();
                            //m_strUTM_CONTENT.Trim();
                        }
                    }

                    // Base format url unregistered version
                    //		if(reg.VerifyValue(_T("BaseFormatURLunreg")))* /
                    if (reg.VerifyValue(STAppStrings_STR_11141591))
                    {
                        m_strBaseFormatURLunreg = string.Empty;
                        //			reg.ReadString(_T("BaseFormatURLunreg"), m_strBaseFormatURLunreg);* /
                        reg.ReadString(STAppStrings_STR_11114861, m_strBaseFormatURLunreg);
                        if (m_strBaseFormatURLunreg.Length > 0)
                        {
                            m_strBaseFormatURLunreg = m_strBaseFormatURLunreg.ToLower().Trim();
                            //m_strBaseFormatURLunreg.Trim();
                        }
                    }


                    // Base format url registered version
                    //		if(reg.VerifyValue(_T("BaseFormatURLreg")))* /
                    if (reg.VerifyValue(STAppStrings_STR_11142922))
                    {
                        m_strBaseFormatURLreg = string.Empty;
                        //			reg.ReadString(_T("BaseFormatURLreg"), m_strBaseFormatURLreg);* /
                        reg.ReadString(STAppStrings_STR_11122821, m_strBaseFormatURLreg);
                        if (m_strBaseFormatURLreg.Length > 0)
                        {
                            m_strBaseFormatURLreg = m_strBaseFormatURLreg.ToLower().Trim();
                            //m_strBaseFormatURLreg.Trim();
                        }
                    }


                    // Base format url registered version
                    if (reg.VerifyValue(REGSTR_VAL_HELP_URL_INT))
                    {
                        m_strHelpUrlINT = string.Empty;
                        reg.ReadString(REGSTR_VAL_HELP_URL_INT, m_strHelpUrlINT);
                        if (m_strHelpUrlINT.Length > 0)
                        {
                            m_strHelpUrlINT = m_strHelpUrlINT.ToLower().Trim();
                            //m_strHelpUrlINT.Trim();
                        }
                    }

                    // Base format url registered version
                    if (reg.VerifyValue(REGSTR_VAL_HELP_URL_DE))
                    {
                        m_strHelpUrlDE = string.Empty;
                        reg.ReadString(REGSTR_VAL_HELP_URL_DE, m_strHelpUrlDE);
                        if (m_strHelpUrlDE.Length > 0)
                        {
                            m_strHelpUrlDE = m_strHelpUrlDE.ToLower().Trim();
                            //m_strHelpUrlDE.Trim();
                        }
                    }


                    // Base format url registered version
                    if (reg.VerifyValue(REGSTR_VAL_SUPPORT_URL))
                    {
                        m_strSupportUrl = string.Empty;
                        reg.ReadString(REGSTR_VAL_SUPPORT_URL, m_strSupportUrl);
                        if (m_strSupportUrl.Length > 0)
                        {
                            m_strSupportUrl = m_strSupportUrl.ToLower().Trim();
                            //m_strSupportUrl.Trim();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetAffiliateBuildInfo()", ex);
            }


            return;
        }

    */

        #endregion



    }
}
