using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.Helpers;
using Microsoft.VisualBasic.Devices;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class cSystemInfo
    {
        public class cSystemInfoObjects
        {
            public string OSManufacturer { get; set; }
            public string Manufacturer { get; set; }
            public Image ManufacturerLogo { get; set; }
            public eManufacturer eManufacturerID { get; set; }
            public string Model { get; set; }
            public string OSFriendlyName { get; set; }
            public string Caption { get; set; }
            public string ComputerName { get; set; }
            public string ProcessorName { get; set; }
            public long Memory_RAM { get; set; }
            public string strMemory_RAM { get; set; }
            public string GraphicsCard { get; set; }
            public System.Windows.Media.Imaging.BitmapImage OSImage { get; set; }
            public DateTime PCRestartTime { get; set; }
            public bool isvm { get; set; }
        }

        public enum eManufacturer
        {
            UNKNOWN = 0,
            ACER,
            ALIENWARE,
            AMD,
            ASROCK,
            ASUS,
            ASUSTek,
            ATI,
            BENQ,
            BIOSTAR,
            BOARDMARKER,
            CASPER,
            CLEVO,
            COMMON,
            COMPAL,
            COMPAQ,
            DELL,
            DIGIBOARD,
            DIGIBRAS,
            DIGILITE,
            ECS,
            EPSON,
            E_MACHINES,
            FOXCONN,
            FUJITSU,
            GATEWAY,
            GIGABYTE,
            HCL,
            HP,
            IBALL,
            IBM,
            INTEL,
            INTELBRAS,
            ITAUTEC,
            KOBIAN,
            LENOVO,
            LG_ELECTRONICS,
            LITEON,
            LOGIN_INFORMATICA,
            MATSUSHITA,
            MEDION,
            MICROSOFT,
            MOUSE_COMPUTER,
            MSI,
            NEC,
            NVIDIA,
            OEM,
            ONKYO,
            PACKARD_BELL,
            PANASONIC,
            PCCHIPS,
            PEGATRON,
            PHILCO,
            POSITIVO,
            QUANTA,
            SAMSUNG,
            SONY,
            STI,
            TOSHIBA,
            UNITCOM,
            VAIO,
            //WIN10,
            //WIN7,
            //WIN8.1,
            //WIN8,
            WINVISTA,
            WIPRO,
            WISTRON,
            ZOTAC
        }

        public static cSystemInfoObjects GetSystemInfo()
        {
            cSystemInfoObjects oSysInfo = null;
            try
            {
                oSysInfo = new cSystemInfoObjects();
                //System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");

                //initialize the searcher with the query it is supposed to execute
                var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get().Cast<ManagementObject>().First();
                /*oSysInfo.Manufacturer = Convert.ToString(searcher["Manufacturer"]);
*/
                //oSysInfo.Model = Convert.ToString(searcher["Model"]);
                oSysInfo.ComputerName = Convert.ToString(searcher["Caption"]);
                oSysInfo.OSManufacturer = Convert.ToString(searcher["Manufacturer"]);

                //string strManufacturer = Convert.ToString(searcher["Manufacturer"]);






                //string ComputerName = Convert.ToString(searcher["TotalPhysicalMemory"]);

                // Searcher base board...
                var searcherBB = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_BaseBoard").Get().Cast<ManagementObject>().First(); ;

                oSysInfo.Manufacturer = Convert.ToString(searcherBB["Manufacturer"]);
                oSysInfo.Model = Convert.ToString(searcherBB["Model"]);
                if (string.IsNullOrEmpty(oSysInfo.Model))
                {
                    oSysInfo.Model = Convert.ToString(searcher["Model"]);

                    if (string.Compare(oSysInfo.Model, "System Product Name", true) == 0)
                    {
                        oSysInfo.Model = cResourceManager.LoadString("IDS_NO_DETAILS_AVAILABLE");
                    }
                }

                if (!string.IsNullOrEmpty(oSysInfo.Model))
                {
                    if (oSysInfo.Model.ToLower().Contains("virtual") ||
                        oSysInfo.Manufacturer.ToLower().Contains("vmware") ||
                        oSysInfo.Model == "VirtualBox")
                    {
                        oSysInfo.isvm = true;
                    }
                }

                /*ManagementObjectCollection information = searcherBB.Get();
                foreach (ManagementObject obj in information)
                {
                    foreach (PropertyData data in obj.Properties)
                    {
                        //System.Diagnostics.Trace.WriteLine(string.Format("{0} = {1}", data.Name, data.Value));
                        Console.WriteLine("{0} = {1}", data.Name, data.Value);
                    }
                    Console.WriteLine();
                }*/

                var cpu = new ManagementObjectSearcher("select Name from Win32_Processor").Get().Cast<ManagementObject>().First();
                oSysInfo.ProcessorName = Convert.ToString(cpu["Name"]);

                /*var win_os = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().First();
                oSysInfo.Caption = Convert.ToString(win_os["Caption"]);*/
                oSysInfo.Caption = GetOSName();

                /// Get Manufacturer logo, in case if Manufacturer logo not present then try to get ProcessorName Manufacturer logo.
                /// 
                eManufacturer eManufacturerID;
                oSysInfo.ManufacturerLogo = GetLogo(oSysInfo.Manufacturer, oSysInfo.ProcessorName, out eManufacturerID);
                oSysInfo.eManufacturerID = eManufacturerID;

                ComputerInfo computerInfo = new ComputerInfo();
                oSysInfo.OSFriendlyName = computerInfo.OSFullName;


                oSysInfo.Memory_RAM = (long)computerInfo.TotalPhysicalMemory;

                oSysInfo.strMemory_RAM = AppFunctions.getSizeFormat(computerInfo.TotalPhysicalMemory);

                var displaySearcher = new ManagementObjectSearcher("SELECT Description FROM Win32_DisplayConfiguration").Get().Cast<ManagementObject>().First();
                oSysInfo.GraphicsCard = Convert.ToString(displaySearcher["Description"]);

                oSysInfo.OSImage = GetOSImage();

                oSysInfo.PCRestartTime = GetLastRestartTime();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucSystemInfo:: GetSystemInfo: ", ex);
            }
            return oSysInfo;
        }

        static bool IsVirtualMachine()
        {
            bool isVM = false;
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                {
                    using (var items = searcher.Get())
                    {
                        foreach (var item in items)
                        {
                            string manufacturer = item["Manufacturer"].ToString().ToLower();
                            if (item["Model"].ToString().ToLower().Contains("VIRTUAL")
                                || manufacturer.Contains("vmware")
                                || item["Model"].ToString() == "VirtualBox")
                            {
                                isVM = true;
                                break;
                            }
                        }
                    }
                }
                isVM = false;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("IsVirtualMachine:: ", ex);
            }
            return isVM;
        }

        static DateTime GetLastRestartTime()
        {
            DateTime dtBootTime = new DateTime();
            try
            {
                SelectQuery query = new SelectQuery(@"SELECT LastBootUpTime FROM Win32_OperatingSystem WHERE Primary='true'");

                // create a new management object searcher and pass it
                // the select query
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                // get the datetime value and set the local boot
                // time variable to contain that value
                foreach (ManagementObject mo in searcher.Get())
                {
                    dtBootTime =
                        ManagementDateTimeConverter.ToDateTime(
                            mo.Properties["LastBootUpTime"].Value.ToString());

                    // display the start time and date
                    /*txtDate.Text = dtBootTime.ToLongDateString();
                    txtTime.Text = dtBootTime.ToLongTimeString();*/
                }

                return dtBootTime;
            }
            catch (Exception ex)
            {
                return dtBootTime;
            }
        }

        static Image GetLogo(string Manufacturer, string ProcessorName, out eManufacturer LogoID)
        {
            Image imgLogo = null;
            LogoID = eManufacturer.UNKNOWN;
            Dictionary<int, string> mydic = new Dictionary<int, string>();
            try
            {
                mydic = new Dictionary<int, string>();
                foreach (eManufacturer foo in Enum.GetValues(typeof(eManufacturer)))
                {
                    mydic.Add((int)foo, foo.ToString().ToLower());
                }

                KeyValuePair<int, string> result = mydic.Where(p => ProcessorName.ToLower().Contains(p.Value)).SingleOrDefault();

                eManufacturer eCompany;
                if (string.IsNullOrEmpty(result.Value))
                {
                    KeyValuePair<int, string> resultTry = mydic.Where(p => Manufacturer.ToLower().StartsWith(p.Value)).FirstOrDefault();

                    eCompany = EnumUtils.ParseEnum<eManufacturer>(resultTry.Value);
                }
                else
                {
                    eCompany = EnumUtils.ParseEnum<eManufacturer>(result.Value);
                }
                LogoID = eCompany;
                switch (eCompany)
                {
                    /*case eManufacturer.ACER:
                        imgLogo = Properties.Resources.ACER_LOGO;
                        break;
                    case eManufacturer.ALIENWARE:
                        imgLogo = Properties.Resources.ALIENWARE_LOGO;
                        break;
                    case eManufacturer.AMD:
                        imgLogo = Properties.Resources.AMD_LOGO;
                        break;
                    case eManufacturer.ASROCK:
                        imgLogo = Properties.Resources.ASROCK_LOGO;
                        break;
                    case eManufacturer.ASUS:
                    case eManufacturer.ASUSTek:
                        imgLogo = Properties.Resources.ASUS_LOGO;
                        break;
                    case eManufacturer.ATI:
                        imgLogo = Properties.Resources.ATI_LOGO;
                        break;
                    case eManufacturer.BENQ:
                        imgLogo = Properties.Resources.BENQ_LOGO;
                        break;
                    case eManufacturer.BIOSTAR:
                        imgLogo = Properties.Resources.BIOSTAR_LOGO;
                        break;
                    case eManufacturer.BOARDMARKER:
                        imgLogo = Properties.Resources.BOARDMARKER_LOGO;
                        break;
                    case eManufacturer.CASPER:
                        imgLogo = Properties.Resources.CASPER_LOGO;
                        break;
                    case eManufacturer.CLEVO:
                        imgLogo = Properties.Resources.CLEVO_LOGO;
                        break;
                    case eManufacturer.COMMON:
                        imgLogo = Properties.Resources.COMMON_LOGO;
                        break;
                    case eManufacturer.COMPAL:
                        imgLogo = Properties.Resources.COMPAL_LOGO;
                        break;
                    case eManufacturer.COMPAQ:
                        imgLogo = Properties.Resources.COMPAQ_LOGO;
                        break;
                    case eManufacturer.DELL:
                        imgLogo = Properties.Resources.DELL_LOGO;
                        break;
                    case eManufacturer.DIGIBOARD:
                        imgLogo = Properties.Resources.DIGIBOARD_LOGO;
                        break;
                    case eManufacturer.DIGIBRAS:
                        imgLogo = Properties.Resources.DIGIBRAS_LOGO;
                        break;
                    case eManufacturer.DIGILITE:
                        imgLogo = Properties.Resources.DIGILITE_LOGO;
                        break;
                    case eManufacturer.ECS:
                        imgLogo = Properties.Resources.ECS_LOGO;
                        break;
                    case eManufacturer.EPSON:
                        imgLogo = Properties.Resources.EPSON_LOGO;
                        break;
                    case eManufacturer.E_MACHINES:
                        imgLogo = Properties.Resources.E_MACHINES_LOGO;
                        break;
                    case eManufacturer.FOXCONN:
                        imgLogo = Properties.Resources.FOXCONN_LOGO;
                        break;
                    case eManufacturer.FUJITSU:
                        imgLogo = Properties.Resources.FUJITSU_LOGO;
                        break;
                    case eManufacturer.GATEWAY:
                        imgLogo = Properties.Resources.GATEWAY_LOGO;
                        break;
                    case eManufacturer.GIGABYTE:
                        imgLogo = Properties.Resources.GIGABYTE_LOGO;
                        break;
                    case eManufacturer.HCL:
                        imgLogo = Properties.Resources.HCL_LOGO;
                        break;
                    case eManufacturer.HP:
                        imgLogo = Properties.Resources.HP_LOGO;
                        break;
                    case eManufacturer.IBALL:
                        imgLogo = Properties.Resources.IBALL_LOGO;
                        break;
                    case eManufacturer.IBM:
                        imgLogo = Properties.Resources.IBM_LOGO;
                        break;
                    case eManufacturer.INTELBRAS:
                        imgLogo = Properties.Resources.INTELBRAS_LOGO;
                        break;
                    case eManufacturer.INTEL:
                        imgLogo = Properties.Resources.INTEL_LOGO;
                        break;
                    case eManufacturer.ITAUTEC:
                        imgLogo = Properties.Resources.ITAUTEC_LOGO;
                        break;
                    case eManufacturer.KOBIAN:
                        imgLogo = Properties.Resources.KOBIAN_LOGO;
                        break;
                    case eManufacturer.LENOVO:
                        imgLogo = Properties.Resources.LENOVO_LOGO;
                        break;
                    case eManufacturer.LG_ELECTRONICS:
                        imgLogo = Properties.Resources.LG_ELECTRONICS_LOGO;
                        break;
                    case eManufacturer.LITEON:
                        imgLogo = Properties.Resources.LITEON_LOGO;
                        break;
                    case eManufacturer.LOGIN_INFORMATICA:
                        imgLogo = Properties.Resources.LOGIN_INFORMATICA_LOGO;
                        break;
                    case eManufacturer.MATSUSHITA:
                        imgLogo = Properties.Resources.MATSUSHITA_LOGO;
                        break;
                    case eManufacturer.MEDION:
                        imgLogo = Properties.Resources.MEDION_LOGO;
                        break;
                    case eManufacturer.MICROSOFT:
                        imgLogo = Properties.Resources.MICROSOFT_LOGO;
                        break;
                    case eManufacturer.MOUSE_COMPUTER:
                        imgLogo = Properties.Resources.MOUSE_COMPUTER_LOGO;
                        break;
                    case eManufacturer.MSI:
                        imgLogo = Properties.Resources.MSI_LOGO;
                        break;
                    case eManufacturer.NEC:
                        imgLogo = Properties.Resources.NEC_LOGO;
                        break;
                    case eManufacturer.NVIDIA:
                        imgLogo = Properties.Resources.NVIDIA_LOGO;
                        break;
                    case eManufacturer.OEM:
                        imgLogo = Properties.Resources.OEM_LOGO;
                        break;
                    case eManufacturer.ONKYO:
                        imgLogo = Properties.Resources.ONKYO_LOGO;
                        break;
                    case eManufacturer.PACKARD_BELL:
                        imgLogo = Properties.Resources.PACKARD_BELL_LOGO;
                        break;
                    case eManufacturer.PANASONIC:
                        imgLogo = Properties.Resources.PANASONIC_LOGO;
                        break;
                    case eManufacturer.PCCHIPS:
                        imgLogo = Properties.Resources.PCCHIPS_LOGO;
                        break;
                    case eManufacturer.PEGATRON:
                        imgLogo = Properties.Resources.PEGATRON_LOGO;
                        break;
                    case eManufacturer.PHILCO:
                        imgLogo = Properties.Resources.PHILCO_LOGO;
                        break;
                    case eManufacturer.POSITIVO:
                        imgLogo = Properties.Resources.POSITIVO_LOGO;
                        break;
                    case eManufacturer.QUANTA:
                        imgLogo = Properties.Resources.QUANTA_LOGO;
                        break;
                    case eManufacturer.SAMSUNG:
                        imgLogo = Properties.Resources.SAMSUNG_LOGO;
                        break;
                    case eManufacturer.SONY:
                        imgLogo = Properties.Resources.SONY_LOGO;
                        break;
                    case eManufacturer.STI:
                        imgLogo = Properties.Resources.STI_LOGO;
                        break;
                    case eManufacturer.TOSHIBA:
                        imgLogo = Properties.Resources.TOSHIBA_LOGO;
                        break;
                    case eManufacturer.UNITCOM:
                        imgLogo = Properties.Resources.UNITCOM_LOGO;
                        break;
                    case eManufacturer.VAIO:
                        imgLogo = Properties.Resources.VAIO_LOGO;
                        break;
                    case eManufacturer.WINVISTA:
                        imgLogo = Properties.Resources.WINVISTA_LOGO;
                        break;
                    case eManufacturer.WIPRO:
                        imgLogo = Properties.Resources.WIPRO_LOGO;
                        break;
                    case eManufacturer.WISTRON:
                        imgLogo = Properties.Resources.WISTRON_LOGO;
                        break;
                    case eManufacturer.ZOTAC:
                        imgLogo = Properties.Resources.ZOTAC_LOGO;
                        break;*/
                    default:
                        imgLogo = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cSystemInfo:: GetLogo: ", ex);
            }
            finally
            {
                if (mydic != null) mydic.Clear(); mydic = null;
            }
            return imgLogo;
        }

        static System.Windows.Media.Imaging.BitmapImage GetOSImage()
        {
            System.Windows.Media.Imaging.BitmapImage imgLogo = null;

            /*switch (cGlobalSettings.systemOS)
            {
                case eOS.other:
                case eOS.XP:
                case eOS.XP64:
                    break;
                case eOS.Vista:
                    imgLogo = cImageResources.imgOSWinVista;
                    break;
                case eOS.Win7:
                    imgLogo = cImageResources.imgOSWin7;
                    break;
                case eOS.Win8:
                    imgLogo = cImageResources.imgOSWin8;
                    break;
                case eOS.Win8_1:
                    imgLogo = cImageResources.imgOSWin81;
                    break;
                case eOS.Win10:
                    imgLogo = cImageResources.imgOSWin10;
                    break;
                default:
                    break;
            }*/

            return imgLogo;
        }

        static string GetOSName()
        {
            string OSName = "Other";
            switch (cGlobalSettings.systemOS)
            {

                case eOS.XP:
                case eOS.XP64:
                    OSName = "Windows XP";
                    break;
                case eOS.Vista:
                    OSName = "Windows Vista";
                    break;
                case eOS.Win7:
                    OSName = "Windows 7";
                    break;
                case eOS.Win8:
                    OSName = "Windows 8";
                    break;
                case eOS.Win8_1:
                    OSName = "Windows 8.1";
                    break;
                case eOS.Win10:
                    OSName = "Windows 10";
                    break;
                case eOS.Win11:
                    OSName = "Windows 11";
                    break;
                case eOS.other:
                default:
                    break;
            }

            return OSName;
        }


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

    }
}
