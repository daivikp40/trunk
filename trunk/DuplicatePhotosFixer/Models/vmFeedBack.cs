using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using DuplicatePhotosFixer.App_code;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Helpers;
using HardwareHash.Engine;
using static DuplicatePhotosFixer.Views.cNagDialogue;

namespace DuplicatePhotosFixer.Models
{
   public class vmFeedBack : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    
        public Window ctrlOwner = null;
        public List<cFeedBackList> lstFeedBack { get; set; }

        string _txtEmail { get; set; }
        public string txtEmail
        {
            get
            {
                return _txtEmail;
            }
            set
            {
                _txtEmail = value;
                OnPropertyChanged("txtEmail");
            }
        }

        string _txtBrief { get; set; }
        public string txtBrief
        {
            get
            {
                return _txtBrief;
            }
            set
            {
                _txtBrief = value;
                OnPropertyChanged("txtBrief");
            }
        }


        public class BIOSInfo
        {
            public string BIOS_Name { get; set; }
            public string BIOS_Manufacturer { get; set; }
            public string BIOS_SMBIOSBIOSVersion { get; set; }
            public string BIOS_SerialNo { get; set; }
            public string BIOS_ReleaseDate { get; set; }

        }

        public class BaseBoardInfo
        {
            public string BaseBoard_Manufacturer { get; set; }
            public string BaseBoard_Version { get; set; }
        }

        public class NetWorkInfo
        {
            public string Name { get; set; }
            public string MACAddress { get; set; }
        }

        public class SystemInfo
        {
            #region Constants

            //////////////////////////=== strings =============== ////////////////////
            const string SystemInfoStrings_STR_11132834 = "kernel32.dll";
            const string SystemInfoStrings_STR_11130542 = "GetNativeSystemInfo";
            const string SystemInfoStrings_STR_11121694 = "kernel32.dll";
            const string SystemInfoStrings_STR_11126919 = "GetNativeSystemInfo";
            const string SystemInfoStrings_STR_11134930 = "";
            //////////////////////////=== strings =============== ////////////////////
            #endregion

            #region Struct
            [StructLayout(LayoutKind.Sequential)]
            public struct OSVERSIONINFOEX
            {
                public uint dwOSVersionInfoSize;
                public uint dwMajorVersion;
                public uint dwMinorVersion;
                public uint dwBuildNumber;
                public uint dwPlatformId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string szCSDVersion;
                public ushort wServicePackMajor;
                public ushort wServicePackMinor;
                public SuiteMask wSuiteMask;
                public ProductType wProductType;
                public byte wReserved;
            }

            public enum ProductType : byte
            {
                VER_NT_DOMAIN_CONTROLLER = 0x0000002,
                VER_NT_SERVER = 0x0000003,
                VER_NT_WORKSTATION = 0x0000001,
            }

            [Flags]
            public enum SuiteMask : ushort
            {
                VER_SUITE_BACKOFFICE = 0x00000004,
                VER_SUITE_BLADE = 0x00000400,
                VER_SUITE_COMPUTE_SERVER = 0x00004000,
                VER_SUITE_DATACENTER = 0x00000080,
                VER_SUITE_ENTERPRISE = 0x00000002,
                VER_SUITE_EMBEDDEDNT = 0x00000040,
                VER_SUITE_PERSONAL = 0x00000200,
                VER_SUITE_SINGLEUSERTS = 0x00000100,
                VER_SUITE_SMALLBUSINESS = 0x00000001,
                VER_SUITE_SMALLBUSINESS_RESTRICTED = 0x00000020,
                VER_SUITE_STORAGE_SERVER = 0x00002000,
                VER_SUITE_TERMINAL = 0x00000010,
                VER_SUITE_WH_SERVER = 0x00008000,
            }

            public enum ProcessorArchitecture
            {
                X86 = 0,
                X64 = 9,
                @Arm = -1,
                Itanium = 6,
                Unknown = 0xFFFF,
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEM_INFO
            {
                public ProcessorArchitecture ProcessorArchitecture; // WORD
                public uint PageSize; // DWORD
                public IntPtr MinimumApplicationAddress; // (long)void*
                public IntPtr MaximumApplicationAddress; // (long)void*
                public IntPtr ActiveProcessorMask; // DWORD*
                public uint NumberOfProcessors; // DWORD (WTF)
                public uint ProcessorType; // DWORD
                public uint AllocationGranularity; // DWORD
                public ushort ProcessorLevel; // WORD
                public ushort ProcessorRevision; // WORD
            }
            #endregion

            #region WIN Api
            //https://www.pinvoke.net/default.aspx/kernel32.getsysteminfo
            [DllImport("kernel32.dll", SetLastError = false)]
            public static extern void GetSystemInfo(out SYSTEM_INFO Info);

            [DllImport("kernel32")]
            public static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);

            [DllImport("kernel32.dll")]
            public static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);
            #endregion


            string m_sdwBuildNumber = string.Empty;
            string m_sdwPlatformId = string.Empty;
            string m_swServicePackMajor = string.Empty;
            string m_swServicePackMinor = string.Empty;
            string m_swSuiteMask = string.Empty;
            string m_swProductType = string.Empty;
            string m_swReserved = string.Empty;

            const int VER_SUITE_WH_SERVER = 0x00008000;
            #region public function
            public bool GetOsVersonInfo(out uint dwMajorVersion, out uint dwMinorVersion, out uint producttype, out uint architecture_type,
                        out uint dwBuildNumber, bool b_callFnfillValues)
            {

                dwMajorVersion = 0;
                dwMinorVersion = 0;
                producttype = 0;
                architecture_type = 0;
                dwBuildNumber = 0;
                try
                {
                    OSVERSIONINFOEX osvi = new OSVERSIONINFOEX();
                    SYSTEM_INFO siSysInfo = new SYSTEM_INFO();
                    //PGNSI pGNSI;
                    //PGPI pGPI;

                    //ZeroMemory(&siSysInfo, sizeof(SYSTEM_INFO));
                    //ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));

                    osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(OSVERSIONINFOEX));
                    GetVersionEx(ref osvi);

                    //pGNSI = (PGNSI)GetProcAddress(
                    //    //			GetModuleHandle(TEXT("kernel32.dll")),"GetNativeSystemInfo");*/
                    //    GetModuleHandle(SystemInfoStrings_STR_11132834), SystemInfoStrings_STR_11130542);
                    //if (NULL != pGNSI)
                    //    pGNSI(&siSysInfo);
                    //else GetSystemInfo(out siSysInfo);
                    GetSystemInfo(out siSysInfo);



                    dwMajorVersion = osvi.dwMajorVersion;
                    dwMinorVersion = osvi.dwMinorVersion;
                    producttype = (uint)osvi.wProductType;
                    architecture_type = (uint)siSysInfo.ProcessorArchitecture;
                    dwBuildNumber = osvi.dwBuildNumber;

                    // Read os details
                    // ReadOsInfo(&osvi);

                    if (b_callFnfillValues)
                    {
                        FillValues(osvi);
                    }



                    return true;

                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("SystemInfo.cs|GetOsVersonInfo", ex);
                }

                return false;

            }

            public int GetOsVersion(out int dwMajorVersion, out int dwMinorVersion, out int producttype, out int architecture_type, bool b_callFnfillValues = false)
            {
                int osid = -1;

                //            ZeroMemory(@os,SizeOf(OSVERSIONINFOEXW));
                // os.dwOSVersionInfoSize:=SizeOf(OSVERSIONINFOEXW);

                // RtlGetVersion(@os);
                //ShowMessage('Major : ' + IntToStr(Os.dwMajorVersion))
                //ShowMessage('Major : ' + IntToStr(Os.dwMinorVersion))


                OSVERSIONINFOEX osvi = new OSVERSIONINFOEX();
                SYSTEM_INFO siSysInfo = new SYSTEM_INFO();
                //PGNSI pGNSI;
                //PGPI pGPI;

                //ZeroMemory(&siSysInfo, sizeof(SYSTEM_INFO));
                //ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));

                osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(OSVERSIONINFOEX));
                GetVersionEx(ref osvi);
                Console.WriteLine(Environment.OSVersion.Version.Major);

                dwMajorVersion = (int)osvi.dwMajorVersion;
                dwMinorVersion = (int)osvi.dwMinorVersion;
                producttype = (int)osvi.wProductType;
                architecture_type = 0;


                //    pGNSI = (PGNSI) GetProcAddress(
                ////		GetModuleHandle(TEXT("kernel32.dll")),"GetNativeSystemInfo");*/
                //        GetModuleHandle(SystemInfoStrings_STR_11121694),SystemInfoStrings_STR_11126919);
                //    if(NULL != pGNSI)
                //        pGNSI(&siSysInfo);
                //    else 
                GetSystemInfo(out siSysInfo);

                //architecture_type = (int)siSysInfo.ProcessorArchitecture;

                if (Utility.SysIsRunningInWin64())
                {
                    //architecture_type = 9;
                    //there is a bug in api if we run the application in X86 in 64 bit environment
                    // it return the process Architecture 32 bit
                    siSysInfo.ProcessorArchitecture = (ProcessorArchitecture)9;
                    architecture_type = 9;
                }

                // Read os details
                // ReadOsInfo(&osvi);

                if (b_callFnfillValues)
                {
                    FillValues(osvi);
                }


                switch (osvi.dwPlatformId)
                {
                    //For the Windows Me/98/95.
                    case Constants.VER_PLATFORM_WIN32_WINDOWS:
                        {

                            //Windows 95
                            //if((osvi.dwMajorVersion == 4) && (osvi.dwMinorVersion == 0))
                            //{
                            //	osid = 1;//windows 95
                            //	return osid;
                            //}

                            //Windows 98
                            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 10)
                            {
                                if (osvi.szCSDVersion[1] == 'A')
                                {
                                    osid = 1; //Windows 98 SE
                                }
                                else
                                {
                                    osid = 1; // Windows 98	
                                }

                                return osid;
                            }

                            //Windows Me
                            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 90)
                            {
                                osid = 2; //Microsoft Windows Millennium Edition
                                return osid;
                            }
                        }
                        break;

                    //For the Windows NT
                    case Constants.VER_PLATFORM_WIN32_NT:
                        {

                            //Windows NT 4.0
                            if ((osvi.dwMajorVersion == 4) && (osvi.dwMinorVersion == 0))
                            {
                                osid = 10; //Windows NT 4.0
                                return osid;
                            }

                            //windows 2000 Itanium 64
                            if ((osvi.dwMajorVersion == 5) && (osvi.dwMinorVersion == 0) &&
                                (siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_IA64))
                            {
                                osid = 17; //windows 2000 Itanium 64 
                                return osid;
                            }


                            //Windows 2000
                            if ((osvi.dwMajorVersion == 5) && (osvi.dwMinorVersion == 0))
                            {
                                osid = 3; //Windows 2000
                                return osid;
                            }


                            //Windows Xp 32- bit
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 1)
                            {
                                osid = 4;//Windows XP 
                                return osid;
                            }

                            //Windows XP Professional x64 Edition
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 7; //Windows XP Professional x64 Edition
                                return osid;
                            }

                            //Windows XP Professional IA64 Edition
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_IA64)
                            {
                                osid = 18; //Windows XP Professional IA64 Edition
                                return osid;
                            }

                            //For Win 2k3 server - AMD 64 bit
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 8;//Win 2k3 server- AMD 64 bit
                                return osid;
                            }

                            //For Win 2k3 server - IA 64 bit
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_IA64)
                            {
                                osid = 19;//For Win 2k3 server - IA 64 bit
                                return osid;
                            }

                            //Microsoft Windows Server 2003
                            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2)
                            {
                                osid = 5;   //Microsoft Windows Server 2003
                                return osid;
                            }


                            //Windows Vista 64 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 9;//windows Vista AMD 64
                                return osid;
                            }

                            //windows Vista
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 6; //windows Vista
                                return osid;
                            }

                            //#ifdef _DEBUG
                            //Windows 7 64 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 21;//windows 7 AMD 64
                                return osid;
                            }

                            //windows 7
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 12; //windows 7
                                return osid;
                            }

                            //Windows 8 64 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 23;//windows 8 AMD 64
                                return osid;
                            }

                            //windows 8 32 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 22; //windows 8 32 bit
                                return osid;
                            }


                            //////////////////////////////////////////////////////////////////////////
                            /// Windows 8.1								6.3*				6					3					OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
                            ///
                            //Windows 8.1 64 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 3 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 25;//windows 8.1 AMD 64
                                return osid;
                            }

                            //windows 8.1 32 bit
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 3 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 24; //windows 8.1 32 bit
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////
                            /// Windows 10 Insider Preview				10.0*				10					0					OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
                            ///

                            /// Windows 10 64 bit - Windows 10 Insider Preview
                            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 27;//windows 10 AMD 64
                                return osid;
                            }

                            /// Windows 10 32 bit - Windows 10 Insider Preview
                            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType == (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 26; //windows 10 32 bit
                                return osid;
                            }
                            //////////////////////////////////////////////////////////////////////////

                            //#endif



                            //////////////////////////////////////////////////////////////////////////
                            /// Windows Server Technical Preview		10.0*				10					0					OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION

                            //Windows Server 2012 R2 server - 64 bit (AMd - 64)
                            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 35;//Windows Server Technical Preview - 64 bit(AMd - 64)
                                return osid;
                            }

                            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 34; //Windows Server Technical Preview
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////
                            //Windows Server 2012 R2 server
                            //Windows Server 2012 R2 server - 64 bit (AMd - 64)
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 3 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 33;//Windows Server 2012 R2 server - 64 bit(AMd - 64)
                                return osid;
                            }

                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 3 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 32; //Windows Server 2012 R2 Server x86
                                return osid;
                            }
                            //////////////////////////////////////////////////////////////////////////

                            //Windows Server 2012 server - 64 bit (AMd - 64)
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 31;//Windows Server 2012 server - 64 bit(AMd - 64)
                                return osid;
                            }


                            //Windows Server 2012 server
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 2 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 30; //Windows Server 2012 Server x86
                                return osid;
                            }



                            //////////////////////////////////////////////////////////////////////////

                            //Windows 2008  R2 server - 64 bit (AMd - 64)
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 29;//Windows 2008 R2 server - 64 bit(AMd - 64)
                                return osid;
                            }


                            //Windows 2008  R2 server
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 28; //Windows 2008 R2 server
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////				


                            //Windows 2008 server - 64 bit (AMd - 64)
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 16;//Windows 2008 server - 64 bit(AMd - 64)
                                return osid;
                            }

                            //Windows 2008 server - IA 64
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION &&
                                siSysInfo.ProcessorArchitecture == (ProcessorArchitecture)Constants.PROCESSOR_ARCHITECTURE_IA64)
                            {
                                osid = 20;//Windows 2008 server - IA 64
                                return osid;
                            }

                            //Windows 2008 server
                            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0 &&
                                osvi.wProductType != (ProductType)Constants.VER_NT_WORKSTATION)
                            {
                                osid = 11; //Windows 2008 server
                                return osid;
                            }

                            //Windows home server
                            if ((SuiteMask)VER_SUITE_WH_SERVER == (osvi.wSuiteMask & (SuiteMask)VER_SUITE_WH_SERVER))
                            {
                                osid = 14; //Windows home server
                                return osid;
                            }
                        }
                        break;

                    //For Microsoft Win32s
                    case Constants.VER_PLATFORM_WIN32s:
                        break;
                }
                return osid;
            }

            private void FillValues(OSVERSIONINFOEX osvi)
            {
                m_sdwBuildNumber = osvi.dwBuildNumber.ToString();
                m_sdwPlatformId = osvi.dwPlatformId.ToString();
                m_swServicePackMajor = osvi.wServicePackMajor.ToString();
                m_swServicePackMinor = osvi.wServicePackMinor.ToString();
                m_swSuiteMask = osvi.wSuiteMask.ToString();
                m_swProductType = osvi.wProductType.ToString();
                m_swReserved = osvi.wReserved.ToString();
            }

            public string GetLocalComputerName()
            {
                return Environment.MachineName;
            }

            public int GetDbOsIdInt()
            {
                SystemInfo sysInfo = null;
                try
                {
                    sysInfo = new SystemInfo();
                    int dwMajorVersion, dwMinorVersion, producttype, architecture_type;
                    dwMajorVersion = dwMinorVersion = producttype = architecture_type = 0;
                    return sysInfo.GetOsVersion(out dwMajorVersion, out dwMinorVersion, out producttype, out architecture_type, false);
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("SystemInfo.cs|GetDbOsIdInt()", ex);
                }
                return 0;
            }

            public int GetOsVersion()
            {
                //SystemInfo sysInfo;
                int osid = 0;
                try
                {
                    //sysInfo = new SystemInfo();
                    int dwMajorVersion, dwMinorVersion, producttype, architecture_type;
                    dwMajorVersion = dwMinorVersion = producttype = architecture_type = 0;
                    osid = GetOsVersion(out dwMajorVersion, out dwMinorVersion, out producttype, out architecture_type, false);
                }
                catch (Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("SystemInfo.cs|GetOsVersion()", ex);
                }
                return osid;
            }
            #endregion

        }
        public vmFeedBack()
        {
            Init();
        }


        void Init()
        {
            try
            {
                FillFeedBackList();
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("vmFeedBack:: Init: ", ex);
            }
        }

        void FillFeedBackList()
        {
            try
            {
                txtEmail = "";
                txtBrief = "";
                lstFeedBack = new List<cFeedBackList>();
                lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_SELECT_PRBTYPE") });
                if (cGlobalSettings.oScanSettings.LanguageCode.ToLower() == "en")
                {
                    lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_INSTALL_COMP") });
                    lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_PRB_DOWNLOADING") });
                    lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_PRB_INSTALLING") });
                }
                lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_SELECT_PRBTYPE") });
                lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_PRB_REGISTRING_PRODUCT") });
                lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_PRB_UNDERSTAND") });
                lstFeedBack.Add(new cFeedBackList() { FeedBackType = cResourceManager.LoadString("PKS_GENERAL_FEEDBACK") });

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHomeViewModel:: FillScanModes: ", ex);
            }
        }


        public void SubmitFeedBack()
        {
            try
            {
                //App.mainWindow.TrackEvent(string.Format("{0}{1}", cTrackingParameters.eUTM_BTN_TYPE.Btn_Submit.ToString(), ""), cTrackingParameters.eProductName.FeedbackWindow.ToString(), true, true, "", cTrackingParameters.eReminderEventType.Action1, cTrackingParameters.eReminderOfferType.AppPurchaseOffer);
                string sReportvalue = string.Empty;
                if (ValidateFeedBack())
                {
                    sReportvalue = CreateReport();

                    ///// Will have to add ServiceRefernce here/////////////////////
                   
                    //using (ServiceReference1.MiscServiceSoapClient objUpdateService = new ServiceReference1.MiscServiceSoapClient())
                    //{
                    //    string response = objUpdateService.UploadFeedback(sReportvalue, Get_XmlFileName(false, false, string.Empty));
                    //    if (response == "OK")
                    //    {
                    //        App.oMainReference.showMessageEx(App.oMainReference.ofrmFeedBack, "", cResourceManager.LoadString("PKS_THANK_FEEDBACK"));
                    //        //MessageBox.Show(this, cResourceManager.LoadString("PKS_THANK_FEEDBACK"));
                           
                    //    }
                    //}
                }
                App.oMainReference.LoadUrlForTrackingSilently(cGlobal.CombineUrlADUTracking(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ADU_TRACKING"]), eUTM_BTN_TYPE.BtnSubmit.ToString(), eOfferName.FEEDBACK.ToString()));
                App.oMainReference.FireEventTrackingPixelInThread("{0}-{1}", eOfferName.FEEDBACK.ToString(), eUTM_BTN_TYPE.BtnSubmit.ToString());

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("SubmitFeedBack:: ", ex);
                App.oMainReference.showMessageEx(App.oMainReference.ofrmFeedBack, "", cResourceManager.LoadString("IDS_ERROR_CONNECTING_INTERNET"));
                MessageBox.Show(App.oMainReference.ofrmFeedBack, cResourceManager.LoadString("IDS_ERROR_CONNECTING_INTERNET"));
            }
        }




        private bool ValidateFeedBack()
        {
            bool rval = true;
            string sMsg = string.Empty;
            if (string.IsNullOrEmpty(txtEmail))
            {

                //App.mainWindow.ShowMsg(this, cResourceManager.LoadString("PKS_IDEMAIL_NOTEMPTY"), MessageBoxButton.OK, MessageBoxImage.Warning);
                App.oMainReference.showMessageEx(App.oMainReference.ofrmFeedBack, "", cResourceManager.LoadString("PKS_IDEMAIL_NOTEMPTY"));


                //MessageBox.Show(this, cResourceManager.LoadString("PKS_IDEMAIL_NOTEMPTY"));
                //txtEmail.Focus();
                rval = false;
                return rval;
            }
            if (!AppFunctions.IsValidEmailAddress(txtEmail))
            {
                sMsg = cResourceManager.LoadString("PKS_EMAIL_ADDRESS_INVALID").Replace("\\n", Environment.NewLine);

                //App.mainWindow.ShowCustomMessageBox(this, sMsg);
                App.oMainReference.showMessageEx(App.oMainReference.ofrmFeedBack, "", sMsg);
                //MessageBox.Show(this, sMsg);

                //txtEmail.Focus();
                rval = false;
                return rval;
            }

            //if (cboFeedBacktype.SelectedIndex == 0)
            //{
            //    //App.mainWindow.ShowCustomMessageBox(this, cResourceManager.LoadString("PKS_FEEDBACK_TYPE"));
            //    App.oMainReference.showMessageEx(App.oMainReference.ovmFeedBack, "", cResourceManager.LoadString("PKS_FEEDBACK_TYPE"));
            //    //MessageBox.Show(this, cResourceManager.LoadString("PKS_FEEDBACK_TYPE"));
            //    cboFeedBacktype.Focus();
            //    rval = false;
            //    return rval;
            //}

            if (string.IsNullOrEmpty(txtBrief))
            {
                //App.mainWindow.ShowCustomMessageBox(this, cResourceManager.LoadString("PKS_FEEDBACK_NOEMPTY"));
                App.oMainReference.showMessageEx(App.oMainReference.ofrmFeedBack, "", cResourceManager.LoadString("PKS_FEEDBACK_NOEMPTY"));
                //MessageBox.Show(this, cResourceManager.LoadString("PKS_FEEDBACK_NOEMPTY"));

               // txtBrief.Focus();
                rval = false;
                return rval;
            }
            return rval;
        }

        public string Get_XmlFileName(bool b_isAutomaticGenerated, bool b_isUsingSimpleStr, string strCustomName)
        {
            /*string strNewXmlNam = _T("DriverUpdater-Feedback.xml");/ *strXmlName* /;*/
            string strAppNameWithoutSpace = GetApplicationDbIdentifierStr();
            strAppNameWithoutSpace.Replace((" "), (""));
            string strNewXmlNam = strAppNameWithoutSpace + (b_isAutomaticGenerated ? ("Automated") : (" ")) + (b_isUsingSimpleStr ? ("SimpleStr") : (" ")) + ("-Feedback.xml");/*strXmlName*/;

            //if (!strCustomName.IsEmpty())
            if (!string.IsNullOrEmpty(strCustomName.Trim()))
            {
                strNewXmlNam = strAppNameWithoutSpace + strCustomName + (b_isAutomaticGenerated ? ("Automated") : (" ")) + (b_isUsingSimpleStr ? ("SimpleStr") : (" ")) + ("-Feedback.xml");/*strXmlName*/;
            }

            if (!string.IsNullOrEmpty(strNewXmlNam))
                strNewXmlNam = strNewXmlNam.Split('.')[0];

            DateTime dt = DateTime.Now;

            string strmin;
            strmin = dt.ToString("yyyyMMdd HH:mm:ss");
            //Add time to xml
            strNewXmlNam = strNewXmlNam + strmin;



            //Add random key
            Random rand = new Random(1000);
            int num = rand.Next(1000, 1000000);


            strNewXmlNam = strNewXmlNam + " " + Convert.ToString(num);

            //Add .xml extension
            strNewXmlNam = strNewXmlNam + (".xml");

            return strNewXmlNam;

        }

        public string GetApplicationDbIdentifierStr()
        {
            return "duplicatephotosfixerprowindows";
        }
        private string CreateReport(string xmlPath = "")
        {
            string sReportvalue = string.Empty;
            XmlTextWriter writer = null;
            StringBuilder builder = null;
            try
            {
                if (!string.IsNullOrEmpty(xmlPath))
                {
                    writer = new XmlTextWriter(xmlPath, System.Text.Encoding.UTF8);
                }
                else
                {
                    //Create xml in memory without the need of path
                    builder = new StringBuilder();
                    writer = new XmlTextWriter(new StringWriterWithEncoding(builder, Encoding.UTF8));
                }

                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;

                writer.WriteStartElement("sf_feedback"); //sf_feedback tag start 

                FillFeedBack(writer);
                FillSysInfo(writer);
                GetOtherInfo(writer);
                OSInfoToXML(writer);
                dbrefToXML(writer);
                KeyToXML(writer);
                HardwareInfoToXML(writer);
                UserHashToXML(writer);
                campaign_infoToXML(writer);
                AppInfo(writer);
                writer.WriteEndElement(); //sf_feedback tag end 
                writer.Close();

                sReportvalue = builder.ToString();
                sReportvalue = AppFunctions.RemoveXmlDefinition(sReportvalue);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|CreateReport()", ex);
            }
            return sReportvalue;
        }
        private void AppInfo(XmlTextWriter writer)
        {
            try
            {
                if (cGlobalSettings.oScanSettings != null)
                {
                    //cGlobalSettings.oScanSettings.CurrentLangSettings.
                    writer.WriteStartElement("app_info"); //app_info tag start

                    writer.WriteStartElement("cur_lang_id"); //cur_lang_id tag start
                    writer.WriteString(cGlobalSettings.oScanSettings.LanguageID.ToString());
                    writer.WriteEndElement(); //cur_lang_id tag End

                    writer.WriteStartElement("cur_lang_code"); //cur_lang_code tag start
                    writer.WriteString(cGlobalSettings.oScanSettings.LanguageCode);
                    writer.WriteEndElement(); //cur_lang_code tag End

                    writer.WriteStartElement("cur_lang_relpath"); //cur_lang_relpath tag start CurrentLangSettings
                    writer.WriteString(cGlobalSettings.oScanSettings.CurrentLangSettings.IniName);
                    writer.WriteEndElement(); //cur_lang_relpath tag End

                    writer.WriteStartElement("cur_lang_locale"); //cur_lang_locale tag start
                    writer.WriteString(cGlobalSettings.oScanSettings.CurrentLangSettings.Name);
                    writer.WriteEndElement(); //cur_lang_locale tag End

                    writer.WriteStartElement("cur_lang_vslangid"); //cur_lang_vslangid tag start
                    writer.WriteString(((int)cGlobalSettings.oScanSettings.CurrentLangSettings.LangIdentifier).ToString());
                    writer.WriteEndElement(); //cur_lang_vslangid tag End

                    writer.WriteStartElement("app_ver"); //app_ver tag start
                    writer.WriteString(cGlobalSettings.GetApplicationVersion());
                    writer.WriteEndElement(); //app_ver tag End

                    writer.WriteStartElement("productid"); //productid tag start
                    writer.WriteString(cGlobalSettings.ProductId.ToString());
                    writer.WriteEndElement(); //productid tag End

                    writer.WriteEndElement(); //app_info tag End
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|AppInfo()", ex);
            }
        }

        void campaign_infoToXML(XmlTextWriter writer)
        {
            try
            {
                writer.WriteStartElement("campaign_info"); //campaign_info tag start

                writer.WriteStartElement("ASOBUILDFOR"); //bfor tag start
                writer.WriteString(string.Empty);
                writer.WriteEndElement(); //bfor tag End

                writer.WriteStartElement("ASOCampaignID"); //campid tag start
                writer.WriteString(string.Empty);
                writer.WriteEndElement(); //campid tag End

                writer.WriteStartElement("utm_source"); //us tag start
                writer.WriteString(cGlobal.UTM_SOURCE);
                writer.WriteEndElement(); //us tag End

                writer.WriteStartElement("utm_campaign"); //us tag start
                writer.WriteString(cGlobal.UTM_CAMPAIGN);
                writer.WriteEndElement(); //uc tag End

                writer.WriteStartElement("utm_medium"); //us tag start
                writer.WriteString(cGlobal.UTM_MEDIUM);
                writer.WriteEndElement(); //um tag End

                writer.WriteStartElement("affiliateid"); //affiliateid tag start
                writer.WriteString(cGlobal.UTM_AFFILIATE);
                writer.WriteEndElement(); //aff tag End

                writer.WriteStartElement("utm_content"); //us tag start
                writer.WriteString("DPFW_Feedback");
                writer.WriteEndElement(); //uc tag End

                writer.WriteStartElement("URL_BUYNOW"); //us tag start
                //writer.WriteString("http://www.systweak.com/adu/price.asp?langid=en&amp;utm_source=googleadw&amp;utm_campaign=adw_dpf_installer&amp;utm_medium=newbuild&amp;affiliateid=");
  //              writer.WriteString(System.Web.HttpUtility.HtmlEncode(cGlobal.BUY_NOW_URL));
                writer.WriteEndElement(); //URL_BUYNOW tag End

                writer.WriteEndElement(); //campaign_info tag End
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|campaign_infoToXML()", ex);
            }
        }

        void UserHashToXML(XmlTextWriter writer)
        {
            try
            {
                writer.WriteStartElement("user_hash");

                writer.WriteStartElement("user_hash_data");
                writer.WriteString(string.Empty);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|UserHashToXML()", ex);
            }

        }

        void HardwareInfoToXML(XmlTextWriter writer)
        {
            BIOSInfo oBIOSInfo = null;
            BaseBoardInfo oBBInfo = null;
            NetWorkInfo oNetworkInfo = null;
            MACAddress oMacAdd = null;

            try
            {
                oBIOSInfo = GetBIOSInfo();

                #region BI info
                if (oBIOSInfo != null)
                {
                    writer.WriteStartElement("bios_info");

                    writer.WriteStartElement("bios_info_Name");
                    writer.WriteString(oBIOSInfo.BIOS_Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("bios_info_Manufacturer");
                    writer.WriteString(oBIOSInfo.BIOS_Manufacturer);
                    writer.WriteEndElement();

                    writer.WriteStartElement("bios_info_SMBIOSBIOSVersion");
                    writer.WriteString(oBIOSInfo.BIOS_SMBIOSBIOSVersion);
                    writer.WriteEndElement();

                    writer.WriteStartElement("bios_info_SerialNumber");
                    writer.WriteString(oBIOSInfo.BIOS_SerialNo);
                    writer.WriteEndElement();

                    writer.WriteStartElement("bios_info_ReleaseDate");
                    writer.WriteString(oBIOSInfo.BIOS_ReleaseDate);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                #endregion

                oBBInfo = GetBaseBoardInfo();
                #region BaseBoard Info
                writer.WriteStartElement("baseboard_info");

                writer.WriteStartElement("baseboard_info_Manufacturer");
                writer.WriteString(oBBInfo.BaseBoard_Manufacturer);
                writer.WriteEndElement();

                writer.WriteStartElement("baseboard_info_Version");
                writer.WriteString(oBBInfo.BaseBoard_Version);
                writer.WriteEndElement();

                writer.WriteEndElement();

                #endregion


                #region MAC Info
                // if we are able to get the mac address through wmi 
                oNetworkInfo = GetNetWorkInfo();
                if (oNetworkInfo != null)
                {

                    writer.WriteStartElement("mac_info");

                    writer.WriteStartElement("mac_address_id_0_name");
                    writer.WriteString(oNetworkInfo.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("mac_address_id_0");
                    writer.WriteString(oNetworkInfo.MACAddress);
                    writer.WriteEndElement();
                }
                else
                {

                    oMacAdd = new MACAddress();
                    oMacAdd.MAC_GetAllAddresses();
                    if (oMacAdd.m_MAC_ArrStr.Count > 0)
                    {
                        writer.WriteStartElement("mac_info");
                        //remove virtual id
                        List<string> result = oMacAdd.m_MAC_ArrStr.Except(Constants.saVirtualMacAddress).ToList();
                        int i = 0;
                        foreach (var item in result)
                        {

                            writer.WriteStartElement(string.Format("mac_address_id_{0}", i));
                            writer.WriteString(oNetworkInfo.MACAddress);
                            writer.WriteEndElement();
                            i++;
                        }

                    }


                }
                //now add virtual id in xml
                for (int i = 0; i < Constants.saVirtualMacAddress.Length; i++)
                {
                    writer.WriteStartElement(string.Format("mac_address_virtual_id_{0}", i));
                    writer.WriteString(Constants.saVirtualMacAddress[i]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                #endregion
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|HardwareInfoToXML()", ex);
            }
            finally
            {
                oBIOSInfo = null;
                oBBInfo = null;
                oNetworkInfo = null;
                oMacAdd = null;
            }

        }

        void KeyToXML(XmlTextWriter writer)
        {//UserKey()
            try
            {
                writer.WriteStartElement("key"); //os tag start

                writer.WriteStartElement("keyvalue"); //osid tag start
                writer.WriteString("");
                writer.WriteEndElement(); //osid tag End

                writer.WriteStartElement("machineid"); //osid tag start
                writer.WriteString(Convert.ToString(HardwareHash.GetHardwareHash()));
                writer.WriteEndElement(); //maj tag End

                writer.WriteStartElement("registered_email_id"); //min tag start
                writer.WriteString(Constants.REGISTERED_EMAIL);
                writer.WriteEndElement(); //min tag End                

                writer.WriteEndElement(); //os tag End
            }
            catch (Exception ex)
            {

                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|KeyToXML()", ex);
            }
        }

        void dbrefToXML(XmlTextWriter writer)
        {
            try
            {
                string sdbinitials = GetApplicationDbIdentifierStr();
                writer.WriteStartElement("dbref");
                writer.WriteStartElement("dbinitials");
                writer.WriteString(sdbinitials);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|dbrefToXML()", ex);
            }
        }

        void OSInfoToXML(XmlTextWriter writer)
        {
            SystemInfo sysInfo = null;
            int majorVersion = 0;
            int minorVersion = 0;
            int productType = 0;
            int arcType = 0;
            try
            {
                sysInfo = new SystemInfo();
                int osID = sysInfo.GetOsVersion(out majorVersion, out minorVersion, out productType, out arcType);

                writer.WriteStartElement("os"); //os tag start

                writer.WriteStartElement("osid"); //osid tag start
                writer.WriteString(osID.ToString());
                writer.WriteEndElement(); //osid tag End

                writer.WriteStartElement("maj"); //osid tag start
                writer.WriteString(majorVersion.ToString());
                writer.WriteEndElement(); //maj tag End

                writer.WriteStartElement("min"); //min tag start
                writer.WriteString(minorVersion.ToString());
                writer.WriteEndElement(); //min tag End

                writer.WriteStartElement("pt"); //pt tag start
                writer.WriteString(productType.ToString());
                writer.WriteEndElement(); //pt tag End

                writer.WriteStartElement("arc"); //arc tag start
                writer.WriteString(arcType.ToString());
                writer.WriteEndElement(); //arc tag End

                writer.WriteEndElement(); //os tag End
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("vmFeedBack|OSInfoToXML()", ex);
            }
            finally
            {
                sysInfo = null;
            }


        }
        private void FillFeedBack(XmlTextWriter writer)
        {
            writer.WriteStartElement("feedback"); //feedback tag start

            writer.WriteStartElement("email"); //email tag start
            writer.WriteString(txtEmail);
            writer.WriteEndElement(); //email tag End

            writer.WriteStartElement("feedback_type"); //feedback_type tag start
 //           writer.WriteString(cboFeedBacktype.Text);
            writer.WriteEndElement(); //feedback_type tag End

            writer.WriteStartElement("content"); //content tag start
            writer.WriteString(txtBrief);
            writer.WriteEndElement(); //content tag End

            writer.WriteEndElement(); //feedback tag End
        }

        private void FillSysInfo(XmlTextWriter writer)
        {
            SystemInfo sysInfo = null;
            int majorVersion = 0;
            int minorVersion = 0;
            int productType = 0;
            int arcType = 0;

            sysInfo = new SystemInfo();
            int osID = sysInfo.GetOsVersion(out majorVersion, out minorVersion, out productType, out arcType);

            writer.WriteStartElement("sys_info"); //sys_info tag start

            writer.WriteStartElement("os_major_version"); //os_major_version tag start
            writer.WriteString(majorVersion.ToString());
            writer.WriteEndElement(); //os_major_version tag End

            writer.WriteStartElement("os_minor_version"); //os_major_version tag start
            writer.WriteString(minorVersion.ToString());
            writer.WriteEndElement(); //os_major_version tag End

            writer.WriteStartElement("os_product_type"); //os_major_version tag start
            writer.WriteString(productType.ToString());
            writer.WriteEndElement(); //os_major_version tag End

            writer.WriteStartElement("os_architecture_type"); //os_major_version tag start
            writer.WriteString(arcType.ToString());
            writer.WriteEndElement(); //os_major_version tag End

            GetRAMDetails(writer);
            GetHDDetails(writer);
            writer.WriteEndElement(); //sys_info tag End
        }

        private void GetRAMDetails(XmlTextWriter writer)
        {
            try
            {

                Utility.MEMORYSTATUSEX oMEMORYSTATUSEX = new Utility.MEMORYSTATUSEX();
                oMEMORYSTATUSEX.dwLength = (uint)Marshal.SizeOf(oMEMORYSTATUSEX);
                Utility.GlobalMemoryStatusEx(oMEMORYSTATUSEX);

                ulong ullTotalPhys = oMEMORYSTATUSEX.ullTotalPhys;
                ulong ullAvailPhys = oMEMORYSTATUSEX.ullAvailPhys;
                ulong ullTotalPageFile = oMEMORYSTATUSEX.ullTotalPageFile;
                ulong ullAvailPageFile = oMEMORYSTATUSEX.ullAvailPageFile;

                writer.WriteStartElement("memory_info"); //memory_info tag start
                try
                {
                    writer.WriteStartElement("total_phy"); //total_phy tag start
                    writer.WriteString(ullTotalPhys.ToString());
                    writer.WriteEndElement(); //total_phy tag End

                    writer.WriteStartElement("avail_phy"); //avail_phy tag start
                    writer.WriteString(ullAvailPhys.ToString());
                    writer.WriteEndElement(); //avail_phy tag End

                    writer.WriteStartElement("total_page_file"); //total_page_file tag start
                    writer.WriteString(ullTotalPageFile.ToString());
                    writer.WriteEndElement(); //total_page_file tag End

                    writer.WriteStartElement("avail_page_file"); //avail_page_file tag start
                    writer.WriteString(ullAvailPageFile.ToString());
                    writer.WriteEndElement(); //avail_page_file tag End
                }
                catch (Exception ex)
                {
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //memory_info tag End
            }
            catch (Exception ex)
            {

            }
        }


        const int _MAX_PATH = 260;
        void GetHDDetails(XmlTextWriter writer)
        {
            const int size = 512;
            char[] buffer = new char[size];

            uint drives = Utility.GetLogicalDriveStrings(size, buffer);

            ulong lTotalbytesALL = 0;
            ulong lFreebytesALL = 0;
            ulong lUsedbytesALL = 0;
            int nPercent = 0;
            string sSystemDrive = string.Empty;

            if (true/*!bAllDrives*/) //for system drive only...
            {
                StringBuilder szSysDir = new StringBuilder(_MAX_PATH);
                //		TCHAR	szSysDir[_MAX_PATH] = DlgNagDriverFeedbackStrings_STR_11124235;
                Utility.GetSystemDirectory(szSysDir, _MAX_PATH);

                //TCHAR drive[5], dir[1024], fname[256], ext[256];
                //_wsplitpath(szSysDir, drive, dir, fname, ext);
                string drive = System.IO.Path.GetPathRoot(szSysDir.ToString());
                sSystemDrive = drive;

                //strHDDetails.Format(_T("System Drive::%s"), sSystemDrive);
                //		xmlMarkup.AddChildElem(_T("system_driver"), sSystemDrive);*/
                //xmlMarkup.AddChildElem(DlgNagDriverFeedbackStrings_STR_11127986, sSystemDrive);

                writer.WriteStartElement("system_driver");
                writer.WriteString(sSystemDrive);
                writer.WriteEndElement();

            }

            foreach (DriveInfo objDrive in DriveInfo.GetDrives())
            {
                //<drive_info>
                //<drive_name>C:\</drive_name>
                //<total_size_in_bytes>249532772352</total_size_in_bytes>
                //<free_space_in_bytes>24692252672</free_space_in_bytes>
                //<used_space_in_bytes>224840519680</used_space_in_bytes>
                //</drive_info>

                string drive_name;
                string total_size_in_bytes;
                string free_space_in_bytes;
                string used_space_in_bytes;

                try
                {
                    drive_name = objDrive.Name;
                    total_size_in_bytes = Convert.ToString(objDrive.TotalSize);
                    free_space_in_bytes = Convert.ToString(objDrive.TotalFreeSpace);
                    used_space_in_bytes = Convert.ToString((objDrive.TotalSize - objDrive.TotalFreeSpace));
                }
                catch (Exception ex)
                {
                    continue;
                }

                try
                {
                    writer.WriteStartElement("drive_info");
                    if (objDrive.DriveType == DriveType.Fixed || objDrive.DriveType == DriveType.Removable)
                    {
                        writer.WriteStartElement("drive_name");
                        writer.WriteString(drive_name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("total_size_in_bytes");
                        writer.WriteString(total_size_in_bytes);
                        writer.WriteEndElement();

                        writer.WriteStartElement("free_space_in_bytes");
                        writer.WriteString(free_space_in_bytes);
                        writer.WriteEndElement();

                        writer.WriteStartElement("used_space_in_bytes");
                        writer.WriteString(used_space_in_bytes);
                        writer.WriteEndElement();
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //end drive_info

            }
        }

        private void GetOtherInfo(XmlTextWriter writer)
        {

            writer.WriteStartElement("prog_info"); //prog_info tag start

            writer.WriteStartElement("prog_ver"); //prog_ver tag start
            try
            {
                writer.WriteString(cGlobalSettings.GetApplicationVersion());
            }
            catch { }
            writer.WriteEndElement(); //prog_ver tag End
            writer.WriteEndElement(); //end prog_info

#if SendInFeedback_sysname
            writer.WriteStartElement("system_name"); //system_name tag start
            writer.WriteStartElement("sysname"); //feedback_type tag start
            try
            {
                writer.WriteString(Environment.MachineName);
            }
            catch { }
            writer.WriteEndElement(); //sysname tag End
            writer.WriteEndElement(); //system_name tag End
#endif


        }


        private BIOSInfo GetBIOSInfo()
        {
            BIOSInfo oBIOSInfo = null;
            ManagementObjectSearcher searcher = null;
            try
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

                oBIOSInfo = new BIOSInfo();
                foreach (ManagementObject queryObj in searcher.Get())
                {

                    oBIOSInfo.BIOS_Name = queryObj["Name"].ToString();
                    oBIOSInfo.BIOS_SMBIOSBIOSVersion = queryObj["SMBIOSBIOSVersion"].ToString();
                    oBIOSInfo.BIOS_Manufacturer = queryObj["Manufacturer"].ToString();
                    oBIOSInfo.BIOS_SerialNo = queryObj["SerialNumber"].ToString();
                    oBIOSInfo.BIOS_ReleaseDate = queryObj["ReleaseDate"].ToString();

                }
                return oBIOSInfo;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DriverScanUpdate.cs|GetBIOSInfo()", ex);
            }
            finally
            {
                searcher = null;
            }
            return oBIOSInfo;
        }

        private NetWorkInfo GetNetWorkInfo()
        {
            string scope = "root\\CIMV2";
            string queryString = @"SELECT name, macaddress FROM Win32_NetworkAdapter
            WHERE netconnectionid <> NULL AND macaddress <> NULL AND Manufacturer <> 'Microsoft' AND NOT PNPDeviceID LIKE 'ROOT\\%'";
            ManagementObjectSearcher searcher = null;
            NetWorkInfo ninfo = null;

            try
            {
                ninfo = new NetWorkInfo();
                searcher = new ManagementObjectSearcher(scope, queryString);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    ninfo.Name = queryObj["Name"].ToString();
                    ninfo.MACAddress = queryObj["MACAddress"].ToString().Replace(":", string.Empty);
                }
                return ninfo;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DriverScanUpdate.cs|GetNetWorkInfo()", ex);
            }
            finally
            {
                searcher = null;
                // ninfo = null;
            }
            return ninfo;
        }

        private BaseBoardInfo GetBaseBoardInfo()
        {
            BaseBoardInfo oBBInfo = null;
            ManagementObjectSearcher searcher = null;

            try
            {
                searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

                oBBInfo = new BaseBoardInfo();
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    oBBInfo.BaseBoard_Manufacturer = queryObj["Manufacturer"].ToString();
                    oBBInfo.BaseBoard_Version = queryObj["Version"].ToString();
                }
                return oBBInfo;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DriverScanUpdate.cs|GetBaseBoardInfo()", ex);
            }
            finally
            {
                searcher = null;
            }
            return oBBInfo;
        }

    }

}

    
