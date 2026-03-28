using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using DuplicatePhotoFixer;
using DuplicatePhotosFixer;
//using DuplicateFileFixer;

namespace HardwareHash
{
    public class CWmiHandler
    {
        const string HardwareHashStrings_STR_11119075 = "";
        const string HardwareHashStrings_STR_11140375 = ":";
        const string HardwareHashStrings_STR_11113684 = "select name, macaddress from Win32_NetworkAdapter where netconnectionid<>NULL and macaddress<>NULL and Manufacturer <> 'Microsoft' AND NOT PNPDeviceID LIKE 'ROOT\\\\%'";

        // member functions
        public static bool getValuesFromWMI(string classNameStr, Dictionary<string, string> saOptionNameValueData, Dictionary<string, string> saWhereClauseNameValueData)
        /*++
        Routine: 
        getValuesFromWMI

        Purpose: 
        Get WMI query results

        Returns: 
        0 if successful, else the error code
        --*/

        {
            ManagementObjectSearcher moSearch = null;
            ManagementObjectCollection moCollection = null;
            //ManagementObject mo = null;

            try
            {
                if (saOptionNameValueData.Count() <= 0) { return false; }





                string strOptionNames = string.Empty;
                foreach (KeyValuePair<string, string> dic in saOptionNameValueData)
                {

                    if (string.IsNullOrEmpty(dic.Key))
                        continue;

                    strOptionNames += dic.Key + ",";
                }

                strOptionNames = strOptionNames.Trim(new char[] { ',' });



                // create query
                string strQuery = string.Empty;
                //string HardwareHashStrings_STR_11113684 = "select name, macaddress from Win32_NetworkAdapter where netconnectionid<>NULL and macaddress<>NULL and Manufacturer <> 'Microsoft' AND NOT PNPDeviceID LIKE 'ROOT\\\\%'";
                strQuery = string.Format("Select {0} from {1}", strOptionNames, classNameStr);

                if (classNameStr.Equals(cHardwareHash.saClassNameStr[(int)cHardwareHash.eClassName.eWin32_NetworkAdapter], StringComparison.OrdinalIgnoreCase))
                {
                    strQuery = HardwareHashStrings_STR_11113684;

                    if (saOptionNameValueData.Count() > 0) { saOptionNameValueData.Clear(); }
                }




                moSearch = new ManagementObjectSearcher(strQuery);
                moCollection = moSearch.Get();

                {


                    if (classNameStr.Equals(cHardwareHash.saClassNameStr[(int)cHardwareHash.eClassName.eWin32_NetworkAdapter], StringComparison.OrdinalIgnoreCase) == true)
                    {

                        foreach (KeyValuePair<string, string> dic in saOptionNameValueData)
                        {
                            //Console.WriteLine($"Pair here: {bird.Key}, {bird.Value}");
                            Console.WriteLine(dic.Value);
                        }

                        string strNetworkAdapterName = string.Empty;
                        string strMacId = string.Empty;
                        string strRetVal = string.Empty;

                        foreach (ManagementObject item in moCollection)
                        {
                            strNetworkAdapterName = WMIFunctions.ReadWmiValue(item, cHardwareHash.Win32_NetworkAdapter_OptionNames[0], string.Empty);
                            strRetVal = WMIFunctions.ReadWmiValue(item, cHardwareHash.Win32_NetworkAdapter_OptionNames[1], string.Empty);
                            strMacId = strRetVal.Trim().Replace(HardwareHashStrings_STR_11140375, string.Empty);

                            saOptionNameValueData.Add(strNetworkAdapterName, strMacId);

                        }


                    }
                    else
                    {
                        string strRetVal = string.Empty;
                        foreach (ManagementObject item in moCollection)
                        {
                            //foreach (KeyValuePair<string, string> dic in saOptionNameValueData)
                            for (int i = 0; i < saOptionNameValueData.Count; i++)
                            {
                                var dic = saOptionNameValueData.ElementAt(i);
                                strRetVal = WMIFunctions.ReadWmiValue(item, dic.Key, string.Empty);
                                Console.WriteLine(dic.Value);

                                if (classNameStr.Equals(cHardwareHash.saClassNameStr[(int)cHardwareHash.eClassName.eWin32_PhysicalMemory], StringComparison.OrdinalIgnoreCase) == true)
                                {
                                    ulong nAddedTotalSize = Convert.ToUInt64(dic.Value);
                                    ulong nCurTotalSize = Convert.ToUInt64(strRetVal);
                                    nAddedTotalSize += nCurTotalSize;

                                    //							saOptionNameValueData[i].keyValue.Format(_T("%llu"), nAddedTotalSize);*/
                                    saOptionNameValueData[dic.Key] = nAddedTotalSize.ToString(); ;

                                }
                                else
                                {
                                    saOptionNameValueData[dic.Key] = strRetVal;
                                }
                            }
                        }

                    }



                }

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog("cWmiHandler.cs|getValuesFromWMI()");
            }
            finally
            {
                moSearch = null;
                moCollection = null;
            }
            return true;   // Program successfully completed.
        }
    }
}

