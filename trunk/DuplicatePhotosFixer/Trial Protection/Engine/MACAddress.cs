using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using DuplicatePhotoFixer;
using DuplicatePhotosFixer;
//using DuplicateFileFixer;


namespace HardwareHash.Engine
{
    public class MACAddress
    {

        //////////////////////////=== strings =============== ////////////////////
        const string MacAddrStrings_STR_11132831 = "{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}";//CString( /* %02X%02X%02X%02X%02X%02X */ CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X"));
        const string MacAddrStrings_STR_11119793 = "";//CString( /* %2hx%2hx%2hx%2hx%2hx%2hx */ CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x") + CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x") + CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x") + CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x") + CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x") + CString(_T("%")) + _T("2") + CString(_T("h")) + _T("x"));
        const string MacAddrStrings_STR_11136598 = "";//CString( /* %02X%02X%02X%02X%02X%02X */ CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X"));
        const string MacAddrStrings_STR_11135623 = "000000000000";//CString( /* 000000000000 */ CString(_T("0")) + _T("0") + CString(_T("0")) + _T("0") + CString(_T("0")) + _T("0") + CString(_T("0")) + _T("0") + CString(_T("0")) + _T("0") + CString(_T("0")) + _T("0"));
        const string MacAddrStrings_STR_11124616 = "\r\n";//CString( /* \r\n */ CString(_T("\\")) + _T("r") + CString(_T("\\")) + _T("n"));
        const string MacAddrStrings_STR_11140345 = "";//CString( /* %02X%02X%02X%02X%02X%02X */ CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X") + CString(_T("%")) + _T("0") + CString(_T("2")) + _T("X"));
        const string MacAddrStrings_STR_11142078 = "\r\n";//CString( /* \r\n */ CString(_T("\\")) + _T("r") + CString(_T("\\")) + _T("n"));
        const string MacAddrStrings_STR_11113684 = "111111111111";//CString( /* 111111111111 */ CString(_T("1")) + _T("1") + CString(_T("1")) + _T("1") + CString(_T("1")) + _T("1") + CString(_T("1")) + _T("1") + CString(_T("1")) + _T("1") + CString(_T("1")) + _T("1"));
        //////////////////////////=== strings =============== ////////////////////

        public struct MyGuid
        {
            public int Data1;
            public short Data2;
            public short Data3;
            public byte[] Data4;

            public MyGuid(Guid g)
            {
                byte[] gBytes = g.ToByteArray();
                Data1 = BitConverter.ToInt32(gBytes, 0);
                Data2 = BitConverter.ToInt16(gBytes, 4);
                Data3 = BitConverter.ToInt16(gBytes, 6);
                Data4 = new Byte[8];
                Buffer.BlockCopy(gBytes, 8, Data4, 0, 8);
            }

            public Guid ToGuid()
            {
                return new Guid(Data1, Data2, Data3, Data4);

            }
        }



        /************************************************************************
List of virtual MAC Address 
Here we are not restricting these machine id from send, we 
are only searching in loop from all machine id's 
if it is present in that array, try to take any other machine id user 
have which can be considered as real
/************************************************************************/
        //        const int VIRTUAL_ADDRESS_COUNT = 2;
        //        readonly string[] saVirtualMacAddress = {
        //    //_T("00038A000015"),		// we got this machine id more than 700 +  times, so assuming
        //    "005056C00001",
        //    "005056C00008"
        //};

        const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        const int MAX_ADAPTER_NAME_LENGTH = 256;

        const int ERROR_SUCCESS = 0;
        const int ERROR_BUFFER_OVERFLOW = 111;
        const int MAX_PREFERRED_LENGTH = -1;

        #region WINApi

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct IP_ADDRESS_STRING
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string Address;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct IP_ADDR_STRING
        {
            public IntPtr Next;
            public IP_ADDRESS_STRING IpAddress;
            public IP_ADDRESS_STRING IpMask;
            public Int32 Context;
        }

        /// <summary>
        /// Lmcons.h
        /// #define NET_API_STATUS DWORD
        /// </summary>
        public enum NET_API_STATUS : uint
        {
            NERR_Success = 0,
            /// <summary>
            /// This computer name is invalid.
            /// </summary>
            NERR_InvalidComputer = 2351,
            /// <summary>
            /// This operation is only allowed on the primary domain controller of the domain.
            /// </summary>
            NERR_NotPrimary = 2226,
            /// <summary>
            /// This operation is not allowed on this special group.
            /// </summary>
            NERR_SpeGroupOp = 2234,
            /// <summary>
            /// This operation is not allowed on the last administrative account.
            /// </summary>
            NERR_LastAdmin = 2452,
            /// <summary>
            /// The password parameter is invalid.
            /// </summary>
            NERR_BadPassword = 2203,
            /// <summary>
            /// The password does not meet the password policy requirements. 
            /// Check the minimum password length, password complexity and password history requirements.
            /// </summary>
            NERR_PasswordTooShort = 2245,
            /// <summary>
            /// The user name could not be found.
            /// </summary>
            NERR_UserNotFound = 2221,
            ERROR_ACCESS_DENIED = 5,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_NAME = 123,
            ERROR_INVALID_LEVEL = 124,
            ERROR_MORE_DATA = 234,
            ERROR_SESSION_CREDENTIAL_CONFLICT = 1219,
            /// <summary>
            /// The RPC server is not available. This error is returned if a remote computer was specified in
            /// the lpServer parameter and the RPC server is not available.
            /// </summary>
            RPC_S_SERVER_UNAVAILABLE = 2147944122, // 0x800706BA
            /// <summary>
            /// Remote calls are not allowed for this process. This error is returned if a remote computer was 
            /// specified in the lpServer parameter and remote calls are not allowed for this process.
            /// </summary>
            RPC_E_REMOTE_DISABLED = 2147549468 // 0x8001011C
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct IP_ADAPTER_INFO
        {
            public IntPtr Next;
            public Int32 ComboIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_NAME_LENGTH + 4)]
            public string AdapterName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
            public string AdapterDescription;
            public UInt32 AddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ADAPTER_ADDRESS_LENGTH)]
            public byte[] Address;
            public Int32 Index;
            public UInt32 Type;
            public UInt32 DhcpEnabled;
            public IntPtr CurrentIpAddress;
            public IP_ADDR_STRING IpAddressList;
            public IP_ADDR_STRING GatewayList;
            public IP_ADDR_STRING DhcpServer;
            public bool HaveWins;
            public IP_ADDR_STRING PrimaryWinsServer;
            public IP_ADDR_STRING SecondaryWinsServer;
            public Int32 LeaseObtained;
            public Int32 LeaseExpires;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WKSTA_TRANSPORT_INFO_0
        {
            public int wkti0_quality_of_service;
            public int wkti0_number_of_vcs;
            //public IntPtr wkti0_transport_name;
            //public IntPtr wkti0_transport_address;
            public string wkti0_transport_name;
            public string wkti0_transport_address;
            public int wkti0_wan_ish;
        }




        [DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int NetWkstaTransportEnum(
               string servername,
               int level,
               out IntPtr bufptr,
               int prefmaxlen,
               out int entriesread,
               out int totalentries,
               ref int resume_handle);



        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);

        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref Int64 pBufOutLen);

        [DllImport("Netapi32.dll", SetLastError = true)]
        static extern int NetApiBufferFree(IntPtr Buffer);

        #endregion


        // public   
        public List<string> m_MAC_ArrStr { get; set; }
        public MACAddress()
        {
            m_MAC_ArrStr = new List<string>();
        }


        // Fetches the MAC address and prints it
        public string CGGetMACaddress()
        {
            string randomnumber = string.Empty;
            string strmcAddress = string.Empty;
            byte[] MACData = new byte[6];

            try
            {


                Guid uuid = Guid.Empty;
                UuidCreateSequential(out uuid);				// Ask OS to create UUID
                MyGuid guid = new MyGuid(uuid);

                for (int i = 2; i < 8; i++)
                {
                    // Bytes 2 through 7 inclusive are MAC address
                    MACData[i - 2] = guid.Data4[i];//uuid.Data4[i];
                }


                //	strmcAddress.Format(_T("%02X%02X%02X%02X%02X%02X"), MACData[0], MACData[1], MACData[2], MACData[3], MACData[4], MACData[5]);*/
                strmcAddress = string.Format(MacAddrStrings_STR_11132831, MACData[0], MACData[1], MACData[2], MACData[3], MACData[4], MACData[5]);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|CGGetMACaddress()", ex);
            }
            return strmcAddress;

        }




        // Fetches the MAC address and prints it
        public string CGGetMACaddress1(ref string macAddress)
        {

            //WKSTA_TRANSPORT_INFO_0 pwkti; // Allocate data structure
            // for NetBIOS
            int dwEntriesRead;
            int dwTotalEntries;
            int resumeHandle = 0;
            IntPtr pbBuffer = IntPtr.Zero;
            NET_API_STATUS dwStatus;
            string strmcAddress = string.Empty;

            Type tWui1 = typeof(WKSTA_TRANSPORT_INFO_0);
            int nStructSize = Marshal.SizeOf(tWui1);
            WKSTA_TRANSPORT_INFO_0 wui1;


            try
            {
                // Get MAC address via NetBIOS's enumerate function
                dwStatus = (NET_API_STATUS)NetWkstaTransportEnum(
                   null,                 // [in]  server name
                   0,                    // [in]  data structure to return
                   out pbBuffer,            // [out] pointer to buffer
                   MAX_PREFERRED_LENGTH, // [in]  maximum length
                   out dwEntriesRead,       // [out] counter of elements
                                            //       actually enumerated
                   out dwTotalEntries,      // [out] total number of elements
                                            //       that could be enumerated
                   ref resumeHandle);                // [in/out] resume handle



                if (dwStatus == NET_API_STATUS.NERR_Success)
                {
                    IntPtr currentPointer = pbBuffer;

                    for (uint i = 0; i < dwEntriesRead; i++)  // first address is
                    // 00000000, skip it
                    {

                        wui1 = (WKSTA_TRANSPORT_INFO_0)Marshal.PtrToStructure(currentPointer, tWui1);



                        strmcAddress = wui1.wkti0_transport_address;


                        if (string.Equals(strmcAddress, MacAddrStrings_STR_11135623, StringComparison.OrdinalIgnoreCase) != true)
                        {
                            //			macAddress += strmcAddress + _T("\r\n");*/
                            macAddress += strmcAddress + MacAddrStrings_STR_11124616;
                            MAC_AddToArr(strmcAddress);
                        }
                        currentPointer = (IntPtr)((int)currentPointer + nStructSize);
                    }



                    return strmcAddress;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|CGGetMACaddress1()", ex);
            }
            finally
            {
                // Release pbBuffer allocated by above function
                dwStatus = (NET_API_STATUS)NetApiBufferFree(pbBuffer);
            }

            return string.Empty;
        }


        // Fetches the MAC address and prints it
        public string CGGetMACaddress2(ref string macAddress)
        {

            string strmcAddress = string.Empty;

            ///////////////////Fair Code//////////////
            //IP_ADAPTER_INFO[] AdapterInfo = new IP_ADAPTER_INFO[16];//[16];       // Allocate information
            //int NumOfParams = AdapterInfo.Length;
            //// for up to 16 NICs
            //int size = Marshal.SizeOf(AdapterInfo[0]);  // Save memory size of buffer
            //IntPtr mem =  Marshal.AllocHGlobal(size * AdapterInfo.Length);
            //for (int i = 0; i < AdapterInfo.Length; ++i)
            //{
            //    Marshal.StructureToPtr(AdapterInfo[i], mem, false);
            //    mem = new IntPtr((long)mem + size);
            //}
            //////////////////////////////////////////
            IntPtr pbBuffer = IntPtr.Zero;
            try
            {
                #region Get AdapterInfo
                long dwBufLen = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));
                pbBuffer = Marshal.AllocHGlobal(new IntPtr(dwBufLen));

                int status = GetAdaptersInfo(      // Call GetAdapterInfo
                    pbBuffer,                 // [out] buffer to receive data
                    ref dwBufLen);                  // [in] size of receive data buffer
                #endregion

                #region Check for buffer overflow
                // valid, no buffer overflow
                if (status == ERROR_BUFFER_OVERFLOW) // ERROR_BUFFER_OVERFLOW == 111
                {
                    // Buffer was too small, reallocate the correct size for the buffer.
                    pbBuffer = Marshal.ReAllocHGlobal(pbBuffer, new IntPtr(dwBufLen));

                    status = GetAdaptersInfo(pbBuffer, ref dwBufLen);
                }
                #endregion

                #region iterate to find MAC Address
                if (status == ERROR_SUCCESS)
                {
                    IntPtr currentPointer = pbBuffer;

                    do
                    {
                        IP_ADAPTER_INFO currentEntry = (IP_ADAPTER_INFO)Marshal.PtrToStructure(currentPointer, typeof(IP_ADAPTER_INFO));

                        // MAC Address
                        strmcAddress = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}", currentEntry.Address[0], currentEntry.Address[1], currentEntry.Address[2], currentEntry.Address[3], currentEntry.Address[4], currentEntry.Address[5]);
#if DEBUG

                        cGlobalSettings.oLogger.WriteLog("Mac Address is " + strmcAddress);
                        cGlobalSettings.oLogger.WriteLog("\n index " + currentEntry.Index.ToString());
#endif
                        macAddress += strmcAddress + MacAddrStrings_STR_11142078;
                        MAC_AddToArr(strmcAddress);

                        currentPointer = currentEntry.Next;

                    } while (currentPointer != IntPtr.Zero);

                }
                #endregion

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|CGGetMACaddress2()", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(pbBuffer);
            }


            // Terminate if last adapter
            return strmcAddress;
        }

        //////////////////////////////////////////////////////////////////////////

        public void MAC_ClearArr()
        {
            if (m_MAC_ArrStr.Count > 0)
            {
                m_MAC_ArrStr.Clear();
            }
        }

        public void MAC_AddToArr(string sMACAddress)
        {
            try
            {
                sMACAddress = sMACAddress.Trim();
                if (string.IsNullOrEmpty(sMACAddress)) { return; }
                if (!MAC_IsElementAlreadyInArr(sMACAddress))
                {
                    m_MAC_ArrStr.Add(sMACAddress.Trim());
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|MAC_AddToArr()", ex);
            }
        }
        public bool MAC_IsElementAlreadyInArr(string sMACAddress)
        {
            try
            {
                for (int i = 0; i < m_MAC_ArrStr.Count; i++)
                {
                    if (string.Equals(m_MAC_ArrStr[i], sMACAddress, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|MAC_IsElementAlreadyInArr()", ex);
            }

            return false;
        }

        //////////////////////////////////////////////////////////////////////////

        public void MAC_GetAllAddresses()
        {
            try
            {


                MAC_ClearArr();
                string sTemp = string.Empty, m_strMACAddress = string.Empty;
                CGGetMACaddress2(ref sTemp);
                CGGetMACaddress1(ref sTemp);
                CGGetMACaddress();


                for (int i = 0; i < m_MAC_ArrStr.Count(); i++)
                {
                    string macAddressStr = m_MAC_ArrStr[i].Trim();
                    bool b_isVirtualAddress = false;
                    for (int j = 0; j < cHardwareHash.VIRTUAL_ADDRESS_COUNT; j++)
                    {
                        if (string.Equals(cHardwareHash.saVirtualMacAddress[j], macAddressStr, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            b_isVirtualAddress = true;
                            break;
                        }
                    }
                    // if it is not a virtual address
                    if (!b_isVirtualAddress)
                    {
                        m_strMACAddress = macAddressStr;
                        break;
                    }
                }

                // if no real mac address found and not able to found 
                // correct one, assign the first value if present.
                if (string.IsNullOrEmpty(m_strMACAddress) && m_MAC_ArrStr.Count() > 0)
                {
                    m_strMACAddress = (m_MAC_ArrStr[0]).Trim();
                }

                // still not get any data
                if (string.IsNullOrEmpty(m_strMACAddress))
                {
                    //		m_strMACAddress = _T("111111111111");*/
                    m_strMACAddress = MacAddrStrings_STR_11113684;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("MACAddress.cs|MAC_GetAllAddresses()", ex);
            }


        }

        //////////////////////////////////////////////////////////////////////////
    }
}
