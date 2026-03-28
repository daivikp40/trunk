using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.App_code
{
    static class csOSProperties
    {
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        public static bool IsOS64Bit()
        {
            if (IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                {
                    return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)fnPtr, typeof(IsWow64ProcessDelegate));
                }
            }

            return null;
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

            if (fnDelegate == null)
            {
                return false;
            }

            bool isWow64;
            bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

            if (retVal == false)
            {
                return false;
            }

            return isWow64;
        }

        public static string GetOSFriendlyName()
        {
            try
            {
                var allValue = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>() select x.Properties).FirstOrDefault();

                var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
                return name != null ? name.ToString() : "Unknown";
            }
            catch
            {
                ;
            }
            return "Unknown";
        }


        public static Version GetOSVersion()
        {
            Version oVersion = null;

            System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem");
            try
            {
                foreach (var os in searcher.Get())
                {
                    oVersion = new Version(os["Version"].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return oVersion;
        }


        public enum eProductType
        {
            VER_NT_WORKSTATION = 1,
            VER_NT_SERVER = 3,
            VER_NT_DOMAIN_CONTROLLER = 2
        }

        public enum ePlatformId
        {
            VER_PLATFORM_WIN32s = 0,
            VER_PLATFORM_WIN32_WINDOWS = 1,
            VER_PLATFORM_WIN32_NT = 2,
        }

        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
        private const int PROCESSOR_ARCHITECTURE_IA64 = 6;
        private const int PROCESSOR_ARCHITECTURE_INTEL = 0;
        private const int VER_SUITE_WH_SERVER = 32768;

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }


        static eOSType _OSId = eOSType.None;
        public static eOSType OSId
        {
            get
            {
                if (_OSId == eOSType.None)
                {
                    GetOSId();
                }

                return _OSId;

            }
            set
            {
                _OSId = value;
            }
        }

        public enum eOSType
        {
            None = 0,
            Other = 1,
            WinXP = 5,
            Vista = 6,
            Win7 = 7,
            Win8 = 8,
            Win81 = 9,
            Win10 = 10
        }




        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx([In, Out] ref OSVERSIONINFOEX osVersionInfo);

        public static int GetOSId()
        {
            OSVERSIONINFOEX osvi = new OSVERSIONINFOEX();
            osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            int osid = -1;


            if (GetVersionEx(ref osvi))
            {

                //Version result = new Version(osvi.wServicePackMajor, osvi.wServicePackMinor);
                //return result;

                Version versionResult = GetOSVersion();


                SYSTEM_INFO siSysInfo = new SYSTEM_INFO();
                GetNativeSystemInfo(ref siSysInfo);


                short ProductType = osvi.wProductType;

                switch (osvi.dwPlatformId)
                {
                    //For the Windows Me/98/95.
                    case (int)ePlatformId.VER_PLATFORM_WIN32_WINDOWS:
                        {

                            //Windows 95
                            //if((versionResult.Major == 4) && (versionResult.Minor == 0))
                            //{
                            //	osid = 1;//windows 95
                            //	return osid;
                            //}

                            //Windows 98
                            if (versionResult.Major == 4 && versionResult.Minor == 10)
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
                            if (versionResult.Major == 4 && versionResult.Minor == 90)
                            {
                                osid = 2; //Microsoft Windows Millennium Edition
                                return osid;
                            }
                        }
                        break;

                    //For Microsoft Win32s
                    case (int)ePlatformId.VER_PLATFORM_WIN32s:
                        break;

                    //For the Windows NT
                    case (int)ePlatformId.VER_PLATFORM_WIN32_NT:
                    default:
                        {
                            //Windows NT 4.0
                            if ((versionResult.Major == 4) && (versionResult.Minor == 0))
                            {
                                osid = 10; //Windows NT 4.0
                                return osid;
                            }

                            //windows 2000 Itanium 64
                            if ((versionResult.Major == 5) && (versionResult.Minor == 0) &&
                                (IsOS64Bit() /*siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64*/))
                            {
                                osid = 17; //windows 2000 Itanium 64 
                                return osid;
                            }


                            //Windows 2000
                            if ((versionResult.Major == 5) && (versionResult.Minor == 0))
                            {
                                osid = 3; //Windows 2000
                                return osid;
                            }


                            //Windows Xp 32- bit
                            if (versionResult.Major == 5 && versionResult.Minor == 1)
                            {
                                OSId = eOSType.WinXP;
                                osid = 4;//Windows XP 
                                return osid;
                            }

                            //Windows XP Professional x64 Edition
                            if (versionResult.Major == 5 && versionResult.Minor == 2 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.WinXP;
                                osid = 7; //Windows XP Professional x64 Edition
                                return osid;
                            }

                            //Windows XP Professional IA64 Edition
                            if (versionResult.Major == 5 && versionResult.Minor == 2 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64)
                            {
                                OSId = eOSType.WinXP;
                                osid = 18; //Windows XP Professional IA64 Edition
                                return osid;
                            }

                            //For Win 2k3 server - AMD 64 bit
                            if (versionResult.Major == 5 && versionResult.Minor == 2 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 8;//Win 2k3 server- AMD 64 bit
                                return osid;
                            }

                            //For Win 2k3 server - IA 64 bit
                            if (versionResult.Major == 5 && versionResult.Minor == 2 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64)
                            {
                                osid = 19;//For Win 2k3 server - IA 64 bit
                                return osid;
                            }

                            //Microsoft Windows Server 2003
                            if (versionResult.Major == 5 && versionResult.Minor == 2)
                            {
                                osid = 5;   //Microsoft Windows Server 2003
                                return osid;
                            }


                            //Windows Vista 64 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 0 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.Vista;
                                osid = 9;//windows Vista AMD 64
                                return osid;
                            }

                            //windows Vista
                            if (versionResult.Major == 6 && versionResult.Minor == 0 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION)
                            {
                                OSId = eOSType.Vista;
                                osid = 6; //windows Vista
                                return osid;
                            }

                            //#ifdef _DEBUG
                            //Windows 7 64 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 1 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.Win7;
                                osid = 21;//windows 7 AMD 64
                                return osid;
                            }

                            //windows 7
                            if (versionResult.Major == 6 && versionResult.Minor == 1 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION)
                            {
                                OSId = eOSType.Win7;
                                osid = 12; //windows 7
                                return osid;
                            }

                            //Windows 8 64 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 2 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.Win8;
                                osid = 23;//windows 8 AMD 64
                                return osid;
                            }

                            //windows 8 32 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 2 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION)
                            {
                                OSId = eOSType.Win8;
                                osid = 22; //windows 8 32 bit
                                return osid;
                            }


                            //////////////////////////////////////////////////////////////////////////
                            /// Windows 8.1								6.3*				6					3					OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
                            ///
                            //Windows 8.1 64 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 3 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.Win8;
                                osid = 25;//windows 8.1 AMD 64
                                return osid;
                            }

                            //windows 8.1 32 bit
                            if (versionResult.Major == 6 && versionResult.Minor == 3 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION)
                            {
                                OSId = eOSType.Win8;
                                osid = 24; //windows 8.1 32 bit
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////
                            /// Windows 10 Insider Preview				10.0*				10					0					OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
                            ///

                            /// Windows 10 64 bit - Windows 10 Insider Preview
                            if (versionResult.Major == 10 && versionResult.Minor == 0 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                OSId = eOSType.Win10;
                                osid = 27;//windows 10 AMD 64
                                return osid;
                            }

                            /// Windows 10 32 bit - Windows 10 Insider Preview
                            if (versionResult.Major == 10 && versionResult.Minor == 0 &&
                                osvi.wProductType == (int)eProductType.VER_NT_WORKSTATION)
                            {
                                OSId = eOSType.Win10;
                                osid = 26; //windows 10 32 bit
                                return osid;
                            }
                            //////////////////////////////////////////////////////////////////////////

                            //#endif



                            //////////////////////////////////////////////////////////////////////////
                            /// Windows Server Technical Preview		10.0*				10					0					OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION

                            //Windows Server 2012 R2 server - 64 bit (AMd - 64)
                            if (versionResult.Major == 10 && versionResult.Minor == 0 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 35;//Windows Server Technical Preview - 64 bit(AMd - 64)
                                return osid;
                            }

                            if (versionResult.Major == 10 && versionResult.Minor == 0 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION)
                            {
                                osid = 34; //Windows Server Technical Preview
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////
                            //Windows Server 2012 R2 server
                            //Windows Server 2012 R2 server - 64 bit (AMd - 64)
                            if (versionResult.Major == 6 && versionResult.Minor == 3 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 33;//Windows Server 2012 R2 server - 64 bit(AMd - 64)
                                return osid;
                            }

                            if (versionResult.Major == 6 && versionResult.Minor == 3 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION)
                            {
                                osid = 32; //Windows Server 2012 R2 Server x86
                                return osid;
                            }
                            //////////////////////////////////////////////////////////////////////////

                            //Windows Server 2012 server - 64 bit (AMd - 64)
                            if (versionResult.Major == 6 && versionResult.Minor == 2 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 31;//Windows Server 2012 server - 64 bit(AMd - 64)
                                return osid;
                            }


                            //Windows Server 2012 server
                            if (versionResult.Major == 6 && versionResult.Minor == 2 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION)
                            {
                                osid = 30; //Windows Server 2012 Server x86
                                return osid;
                            }



                            //////////////////////////////////////////////////////////////////////////

                            //Windows 2008  R2 server - 64 bit (AMd - 64)
                            if (versionResult.Major == 6 && versionResult.Minor == 1 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 29;//Windows 2008 R2 server - 64 bit(AMd - 64)
                                return osid;
                            }


                            //Windows 2008  R2 server
                            if (versionResult.Major == 6 && versionResult.Minor == 1 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION)
                            {
                                osid = 28; //Windows 2008 R2 server
                                return osid;
                            }

                            //////////////////////////////////////////////////////////////////////////				


                            //Windows 2008 server - 64 bit (AMd - 64)
                            if (versionResult.Major == 6 && versionResult.Minor == 0 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)
                            {
                                osid = 16;//Windows 2008 server - 64 bit(AMd - 64)
                                return osid;
                            }

                            //Windows 2008 server - IA 64
                            if (versionResult.Major == 6 && versionResult.Minor == 0 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION &&
                                siSysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64)
                            {
                                osid = 20;//Windows 2008 server - IA 64
                                return osid;
                            }

                            //Windows 2008 server
                            if (versionResult.Major == 6 && versionResult.Minor == 0 &&
                                osvi.wProductType != (int)eProductType.VER_NT_WORKSTATION)
                            {
                                osid = 11; //Windows 2008 server
                                return osid;
                            }

                            //Windows home server
                            if (VER_SUITE_WH_SERVER == (osvi.wSuiteMask & VER_SUITE_WH_SERVER))
                            {
                                osid = 14; //Windows home server
                                return osid;
                            }
                        }
                        break;
                }
            }
            return osid;
        }


    }
}
