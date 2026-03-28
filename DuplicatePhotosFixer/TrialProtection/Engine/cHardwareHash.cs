using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_code;

namespace DuplicatePhotosFixer.Trial_Protection.Engine
{
    public class cHardwareHash
    {

        public enum eClassName
        {
            eWin32_BIOS,
            eWin32_BaseBoard,
            eWin32_NetworkAdapter,
            eWin32_ComputerSystem,
            eWin32_PhysicalMemory,
            eWin32_OPTION_NAMES_SIZE
        };

        public static readonly string[] saClassNameStr =
        {
            "Win32_BIOS",
            "Win32_BaseBoard",
            "Win32_NetworkAdapter",
            "Win32_ComputerSystem",
            "Win32_PhysicalMemory"
        };

        public static readonly string[] Win32_ComputerSystem_OptionNames =
        {
            "Model",
            "Manufacturer"
        };

        public static readonly string[] Win32_BaseBoard_OptionNames =
        {
            "Product",
            "Manufacturer",
            "SerialNumber",
            "Version"
        };

        public static readonly string[] Win32_BIOS_OptionNames =
        {
            "Name",
            "Manufacturer",
            "SMBIOSBIOSVersion",
            "SerialNumber",
            "ReleaseDate"
        };

        public static readonly string[] Win32_NetworkAdapter_OptionNames =
        {
            "name",
            "macaddress"
        };

        public static readonly string[] Win32_NetworkAdapter_WhereOptionNames =
        {
            //_T("physicaladapter")
            "netconnectionid",
            "macaddress"
        };

        public static readonly string[] Win32_NetworkAdapter_WhereOptionValues =
        {
            //_T("true")
            "NULL",
            "NULL"
        };

        public const int VIRTUAL_ADDRESS_COUNT = 2;

        public static readonly string[] saVirtualMacAddress = {
            //_T("00038A000015"),		// we got this machine id more than 700 +  times, so assuming
            "005056C00001",
            "005056C00008"
        };

        /*
Class Name used to find the 
Unique hardware hash
*/
        const string HardwareHashStrings_STR_11141229 = "1111111111111111111111111111111111111111";
        const string HardwareHashStrings_STR_11116737 = "1111111111111111111111111111111111111111";

        public enum eWin32_BIOS_OptionNames
        {
            eBIOS_Name,
            eBIOS_Manufacturer,
            eBIOS_SMBIOSBIOSVersion,
            eBIOS_SerialNumber,
            eBIOS_ReleaseDate,
            eBIOS_OPTION_NAMES_SIZE
        };

        public enum eWin32_BaseBoard_OptionNames
        {
            eBaseBoard_Product,
            eBaseBoard_Manufacturer,
            eBaseBoard_SerialNumber,
            eBaseBoard_Version,
            eBaseBoard_OPTION_NAMES_SIZE
        };
        public enum eWin32_NetworkAdapter_OptionNames
        {
            eNetworkAdapter_macaddress,
            eNetworkAdapter_OPTION_NAMES_SIZE
        };

        public enum eWin32_ComputerSystem_OptionNames
        {
            eComputerSystem_Model,
            eComputerSystem_Manufacturer,
            eComputerSystem_OPTION_NAMES_SIZE
        };
        //////////////////////////////////////////////////////////////////////////
        // Get all WMI Information
        Dictionary<string, string> saBIOS_InfoKeyValuePair;						// contains information of User BIOS
        Dictionary<string, string> saBaseBoard_InfoKeyValuePair;					// contains information of User Base Board
        Dictionary<string, string> saComputerSystem_InfoKeyValuePair;				// contains information of User Computer System
        Dictionary<string, string> saPhysicalMemory_InfoKeyValuePair;				// contains information of User Computer System
        Dictionary<string, string> saNetworkAdapter_InfoKeyValuePair;				// contains information of User Network Adapters
        Dictionary<string, string> saWhereClauseKeyValuePair;                       // contains information of where clause values if any


        public string m_strHardwareHash { get; private set; }

        private readonly MACAddress m_macAddr;

        public cHardwareHash()
        {
            saBIOS_InfoKeyValuePair = new Dictionary<string, string>();
            saBaseBoard_InfoKeyValuePair = new Dictionary<string, string>();
            saComputerSystem_InfoKeyValuePair = new Dictionary<string, string>();
            saPhysicalMemory_InfoKeyValuePair = new Dictionary<string, string>();
            saNetworkAdapter_InfoKeyValuePair = new Dictionary<string, string>();
            saWhereClauseKeyValuePair = new Dictionary<string, string>();
            m_macAddr = new MACAddress();

        }

        public bool IsVirtualNetworkAddress(string csNetworkAddress)
        {

            bool b_isVirtualAddress = false;
            try
            {


                string sNetworkAddress = csNetworkAddress.Trim();
                for (int j = 0; j < VIRTUAL_ADDRESS_COUNT; j++)
                {
                    if (saVirtualMacAddress[j].Equals(sNetworkAddress, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        b_isVirtualAddress = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("cHardwareHash.cs|IsVirtualNetworkAddress()", ex);
            }

            return b_isVirtualAddress;
        }


        public string GetUserHardwareHash()
        /*++

        

        Purpose: 
        Unique Hash has been calculated on base of:
        All User machine Ids + BaseBoard Details 


        Returns: 
        hash
        --*/
        {


            if (!string.IsNullOrEmpty(m_strHardwareHash))
            {
                return m_strHardwareHash;
            }

            string strFinalHash = HardwareHashStrings_STR_11141229;
            strFinalHash = strFinalHash.Trim().ToUpper();


            try
            {

                //const int retval = !FAILED(theApp.hResCoInitializeEx);//CoInitializeExWMI();

                // removing this check, as now we have the option try to get results from m_macAddr.MAC_GetAllAddresses()
                /*if (retval == 1)
                {
                    return strFinalHash;
                }*/

                ClearAllArr();
                //InitHashObject();

                // get BIOS information
                WMI_GetBIOSInfo();

                // get Base board information
                WMI_GetBaseBoardInfo();

                // AfxMessageBox(_T("network adapters"));
                // get mac address info
                WMI_GetNetworkAdapterInfo();

                // Get Model Details
                WMI_GetComputerSystemInfo();


                // Get RAM Details
                //WMI_GetPhysicalMemoryInfo();


                //CoUninitializeWMI();





                // list is used, because it provides feature of unique and sort
                List<string> HashList = new List<string>();

                //HashList.clear();


                //InitHashObject();

                // use all the BIOS Key Value Pair
                HashArr_AddArr(saBIOS_InfoKeyValuePair, HashList);

                // use all the BaseBoard Key Value Pair
                HashArr_AddArr(saBaseBoard_InfoKeyValuePair, HashList);

                // use all the BaseBoard Key Value Pair
                HashArr_AddArr(saComputerSystem_InfoKeyValuePair, HashList);

                //omitting the mac address for unique hash purpose
                //if we are able to get the mac address through WMI 
                //if (saNetworkAdapter_InfoKeyValuePair.Count() > 0)
                //{
                //    HashArr_AddArr(saNetworkAdapter_InfoKeyValuePair, HashList, true);

                //}
                //else
                {
                    // get all MAC Address
                    m_macAddr.MAC_GetAllAddresses();

                    // use all the MAC Address	
                    for (int i = 0; i < m_macAddr.m_MAC_ArrStr.Count(); i++)
                    {
                        HashArr_AddItem(m_macAddr.m_MAC_ArrStr[i].Trim(), HashList, true);
                    }

                }

                // sort the array, so the problem of order with mac obtained will be fixed.		
                // first sort it, so the unique list feature will work
                HashList.Sort();
                HashList = HashList.Distinct().ToList();
                HashList.Sort();

                /// combine all strings.
                string finalStr = String.Join("", HashList.ToArray());

                /// get sha1
                string finalHash = UtilityFunctions.GetSha1HashFromString(finalStr);

                /// get crc64
                long MachineUniqueId = (long)CRC64CPP.Compute(0, Encoding.Unicode.GetBytes(finalHash.ToLower()));
                strFinalHash = MachineUniqueId.ToString();


            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|GetUserHardwareHash()", ex);
            }

            if (string.IsNullOrEmpty(strFinalHash))
            {
                strFinalHash = HardwareHashStrings_STR_11116737;
            }

            m_strHardwareHash = strFinalHash;
            return strFinalHash;
        }

        private void HashArr_AddArr(Dictionary<string, string> saKeyValuePair, List<string> hashList, bool b_isNetworkAdapterInfo = false)
        {
            //// use all the BIOS Key Value Pair
            try
            {
                foreach (KeyValuePair<string, string> dic in saKeyValuePair)
                {
                    HashArr_AddItem(dic.Value.Trim(), hashList, b_isNetworkAdapterInfo);
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|HashArr_AddArr()", ex);
            }
        }

        private void HashArr_AddItem(string csData, List<string> HashList, bool b_isNetworkAdapterInfo)
        {
            try
            {
                string sData = csData.Trim();
                if (string.IsNullOrEmpty(sData)) { return; }

                // check if is virtual address								
                if (b_isNetworkAdapterInfo)
                {
                    if (IsVirtualNetworkAddress(sData)) { return; }
                }

                sData = sData.ToLower().Trim();

                // add item to list
                HashList.Add(sData);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|HashArr_AddItem()", ex);
            }

        }


        private void WMI_GetComputerSystemInfo()
        {
            try
            {

                if (saWhereClauseKeyValuePair.Count() > 0)
                {
                    saWhereClauseKeyValuePair.Clear();
                }
                WMI_GetInfoBase(eClassName.eWin32_ComputerSystem, saComputerSystem_InfoKeyValuePair, saWhereClauseKeyValuePair, Win32_ComputerSystem_OptionNames, (int)eWin32_ComputerSystem_OptionNames.eComputerSystem_OPTION_NAMES_SIZE);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|WMI_GetComputerSystemInfo()", ex);
            }
        }

        private void WMI_GetNetworkAdapterInfo()
        {
            try
            {


                if (saWhereClauseKeyValuePair.Count() > 0)
                {
                    saWhereClauseKeyValuePair.Clear();
                }

                for (int i = 0; i < 2; i++)
                {
                    string sOptionName = Win32_NetworkAdapter_WhereOptionNames[i].Trim();
                    string sOptionValue = Win32_NetworkAdapter_WhereOptionValues[i].Trim();
                    saWhereClauseKeyValuePair.Add(sOptionName, sOptionValue);
                }

                WMI_GetInfoBase(eClassName.eWin32_NetworkAdapter, saNetworkAdapter_InfoKeyValuePair, saWhereClauseKeyValuePair, Win32_NetworkAdapter_OptionNames, (int)eWin32_NetworkAdapter_OptionNames.eNetworkAdapter_OPTION_NAMES_SIZE);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|WMI_GetNetworkAdapterInfo()", ex);
            }
        }

        private void WMI_GetBaseBoardInfo()
        {
            try
            {

                if (saWhereClauseKeyValuePair.Count() > 0)
                {
                    saWhereClauseKeyValuePair.Clear();
                }
                WMI_GetInfoBase(eClassName.eWin32_BaseBoard, saBaseBoard_InfoKeyValuePair, saWhereClauseKeyValuePair, Win32_BaseBoard_OptionNames, (int)eWin32_BaseBoard_OptionNames.eBaseBoard_OPTION_NAMES_SIZE);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|WMI_GetBaseBoardInfo()", ex);
            }
        }

        private void WMI_GetBIOSInfo()
        {
            try
            {
                if (saWhereClauseKeyValuePair.Count() > 0)
                {
                    saWhereClauseKeyValuePair.Clear();
                }

                WMI_GetInfoBase(eClassName.eWin32_BIOS, saBIOS_InfoKeyValuePair, saWhereClauseKeyValuePair, Win32_BIOS_OptionNames, (int)eWin32_BIOS_OptionNames.eBIOS_OPTION_NAMES_SIZE);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|WMI_GetBIOSInfo()", ex);
            }
        }

        private void WMI_GetInfoBase(eClassName enumClassName, Dictionary<string, string> saKeyValuePair, Dictionary<string, string> saWhereKeyValuePair, string[] saOptionNames, int arrlen)
        {

            try
            {
                if (saKeyValuePair.Count() > 0)
                {
                    saKeyValuePair.Clear();
                }


                for (int i = 0; i < arrlen; i++)
                {
                    string sOptionName = saOptionNames[i].Trim();
                    saKeyValuePair.Add(sOptionName, string.Empty);
                }


                CWmiHandler.getValuesFromWMI(saClassNameStr[(int)enumClassName], saKeyValuePair, saWhereKeyValuePair);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|WMI_GetInfoBase()", ex);
            }
        }



        private void ClearAllArr()
        {
            try
            {


                if (saBIOS_InfoKeyValuePair != null && saBIOS_InfoKeyValuePair.Count > 0)
                    saBIOS_InfoKeyValuePair.Clear();

                if (saBaseBoard_InfoKeyValuePair != null && saBaseBoard_InfoKeyValuePair.Count > 0)
                    saBaseBoard_InfoKeyValuePair.Clear();

                if (saComputerSystem_InfoKeyValuePair != null && saComputerSystem_InfoKeyValuePair.Count > 0)
                    saComputerSystem_InfoKeyValuePair.Clear();

                if (saPhysicalMemory_InfoKeyValuePair != null && saPhysicalMemory_InfoKeyValuePair.Count > 0)
                    saPhysicalMemory_InfoKeyValuePair.Clear();

                if (saNetworkAdapter_InfoKeyValuePair != null && saNetworkAdapter_InfoKeyValuePair.Count > 0)
                    saNetworkAdapter_InfoKeyValuePair.Clear();

                if (saWhereClauseKeyValuePair != null && saWhereClauseKeyValuePair.Count > 0)
                    saWhereClauseKeyValuePair.Clear();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("HardwareHash.cs|ClearAllArr()", ex);
            }
        }
    }
}
