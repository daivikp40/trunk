using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DuplicatePhotosFixer.App_Code;
using HardwareHash;

namespace DuplicatePhotosFixer
{
    public class HardwareHash
    {

        /// <summary>
        /// WMI field values.
        /// </summary>
        public static readonly List<string> Win32_BIOS_OptionNames = new List<string> { ("Name"), ("Manufacturer"), ("SMBIOSBIOSVersion"), ("SerialNumber"), ("ReleaseDate") };
        public static readonly List<string> Win32_BaseBoard_OptionNames = new List<string> { ("Product"), ("Manufacturer"), ("SerialNumber"), ("Version") };
        public static readonly List<string> Win32_NetworkAdapter_OptionNames = new List<string> { ("macaddress") };
        public static readonly List<string> Win32_ComputerSystem_OptionNames = new List<string> { ("Model"), ("Manufacturer") };

        /// <summary>
        /// Queries to get values.
        /// </summary>
        public static readonly string query_BIOS = "Select Name, Manufacturer, SMBIOSBIOSVersion, SerialNumber, ReleaseDate from Win32_BIOS";
        public static readonly string query_BaseBoard = "Select Product, Manufacturer, SerialNumber, Version from Win32_BaseBoard";
        public static readonly string query_NetworkAdapter = "select name, macaddress from Win32_NetworkAdapter where netconnectionid<>NULL and macaddress<>NULL and Manufacturer <> 'Microsoft' AND NOT PNPDeviceID LIKE 'ROOT\\\\%'";
        public static readonly string query_ComputerSystem = "Select Model, Manufacturer from Win32_ComputerSystem";

        static string MachineUniqueId = "-1";


        /// <summary>
        /// Get final hardware hash values.
        /// </summary>
        /// <returns></returns>
        //        public static string GetHardwareHash()
        //        {
        //            List<string> HashList = null;
        //            try
        //            {

        //                if (MachineUniqueId != "-1")
        //                    return MachineUniqueId;

        //                string MachineUniqueIdInConfig = KeyValidator.GetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7);

        //                if (!string.IsNullOrEmpty(MachineUniqueIdInConfig))
        //                {
        //                    //cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in reg:", MachineUniqueIdInRegistry);
        //                    MachineUniqueId = MachineUniqueIdInConfig.ToString();
        //                    return MachineUniqueId;
        //                }

        //                long MachineUniqueIdInRegistry = Convert.ToInt64(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, -1));
        //                if (MachineUniqueIdInRegistry != -1)
        //                {
        //                    cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in reg:{0}", MachineUniqueIdInRegistry);
        //                    MachineUniqueId = MachineUniqueIdInRegistry.ToString();
        //                    KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);
        //                    return MachineUniqueId;
        //                }
        //                else
        //                {
        //                    cGlobalSettings.oLogger.WriteLogVerbose("GetOldHash|inside checking for old hash : ");
        //                    string sOldHash = GetOldHash();
        //                    if (!string.IsNullOrEmpty(sOldHash) && Int64.TryParse(sOldHash, out MachineUniqueIdInRegistry))
        //                    {
        //                        cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in old log file :{0}", MachineUniqueIdInRegistry);
        //                        MachineUniqueId = MachineUniqueIdInRegistry.ToString();
        //                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.String);
        //                        KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);
        //                        return MachineUniqueId;
        //                    }
        //                }


        //                cHardwareHash oHardwareHash = new cHardwareHash();
        //                MachineUniqueId = oHardwareHash.GetUserHardwareHash();

        //                cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.String);
        //                KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);

        //                return MachineUniqueId;
        //#if false



        //                string finalHash = string.Empty;

        //                HashList = new List<string>();

        //                List<string> Bios = cUtilityFunctions.ExecuteWmiQuery(query_BIOS, Win32_BIOS_OptionNames);
        //                List<string> BaseBoard = cUtilityFunctions.ExecuteWmiQuery(query_BaseBoard, Win32_BaseBoard_OptionNames);
        //                List<string> NetworkAdapter = cUtilityFunctions.ExecuteWmiQuery(query_NetworkAdapter, Win32_NetworkAdapter_OptionNames).Select(x => x.Replace(":", "")).ToList();
        //                List<string> ComputerSystem = cUtilityFunctions.ExecuteWmiQuery(query_ComputerSystem, Win32_ComputerSystem_OptionNames);
        //                List<string> OriginalMacAddresses = NetworkCards.GetOriginalMacAddresses();               

        //                /// get all values and add it to final list.
        //                HashList.AddRange(Bios);
        //                HashList.AddRange(BaseBoard);
        //                HashList.AddRange(NetworkAdapter);
        //                HashList.AddRange(ComputerSystem);
        //                HashList.AddRange(OriginalMacAddresses);

        //                /// get distinct values
        //                HashList.Sort();
        //                HashList = HashList.Distinct().ToList();
        //                HashList.Sort();                

        //                /// combine all strings.
        //                string finalStr = String.Join("", HashList.ToArray());

        //                /// get sha1
        //                finalHash = cUtilityFunctions.GetSha1HashFromString(finalStr);                

        //                /// get crc64
        //                MachineUniqueId = (long)CRC64CPP.Compute(0, Encoding.Unicode.GetBytes(finalHash.ToLower()));                

        //                cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.QWord);

        //                try
        //                {
        //                    /// log hash details
        //                    StringBuilder sb = new StringBuilder();
        //                    Bios.ForEach(x => sb.Append(string.Format("\r\nBios:{0}", x)));
        //                    BaseBoard.ForEach(x => sb.Append(string.Format("\r\nBaseBoard:{0}", x)));
        //                    NetworkAdapter.ForEach(x => sb.Append(string.Format("\r\nNetworkAdapter:{0}", x)));
        //                    OriginalMacAddresses.ForEach(x => sb.Append(string.Format("\r\nOriginalMacAddresses:{0}", x)));
        //                    ComputerSystem.ForEach(x => sb.Append(string.Format("\r\nComputerSystem:{0}", x)));                    
        //                    string HashPartStrings = String.Join("\t", HashList.ToArray());
        //                    sb.Append("\r\n"+HashPartStrings);
        //                    sb.Append(string.Format("PC Details:{0}\r\nCombined:{1}\r\nString1:{2}\r\nString2:{3}", HashPartStrings, finalStr, finalHash, MachineUniqueId));
        //                    cGlobalSettings.oLogger.WriteLogVerbose(sb.ToString());

        //                    sb.Remove(0, sb.Length - 1);
        //                    sb = null;
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    cGlobalSettings.oLogger.WriteLogException("GetUserHardwareHash-1", ex);
        //                }

        //#endif

        //                return MachineUniqueId;

        //            }
        //            catch (System.Exception ex)
        //            {
        //                cGlobalSettings.oLogger.WriteLogException("GetUserHardwareHash", ex);
        //            }
        //            finally
        //            {
        //                if (HashList != null) HashList.Clear(); HashList = null;
        //            }

        //            return MachineUniqueId;
        //        }


        public static string GetHardwareHash()
        {
            List<string> HashList = null;
            try
            {

                if (MachineUniqueId != "-1")
                    return MachineUniqueId;

                string MachineUniqueIdInConfig = KeyValidator.GetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7);

                if (!string.IsNullOrEmpty(MachineUniqueIdInConfig))
                {
                    //cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in reg:", MachineUniqueIdInRegistry);
                    MachineUniqueId = MachineUniqueIdInConfig.ToString();
                    return MachineUniqueId;
                }

                long MachineUniqueIdInRegistry = Convert.ToInt64(cGlobalSettings.GetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, -1));
                if (MachineUniqueIdInRegistry != -1)
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in reg:{0}", MachineUniqueIdInRegistry);
                    MachineUniqueId = MachineUniqueIdInRegistry.ToString();
                    KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);
                    return MachineUniqueId;
                }
                else
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("GetOldHash|inside checking for old hash : ");
                    string sOldHash = GetOldHash();
                    if (!string.IsNullOrEmpty(sOldHash) && Int64.TryParse(sOldHash, out MachineUniqueIdInRegistry))
                    {
                        cGlobalSettings.oLogger.WriteLogVerbose("GetUserHardwareHash|hash found in old log file :{0}", MachineUniqueIdInRegistry);
                        MachineUniqueId = MachineUniqueIdInRegistry.ToString();
                        cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.String);
                        KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);
                        return MachineUniqueId;
                    }
                }


                cHardwareHash oHardwareHash = new cHardwareHash();
                MachineUniqueId = oHardwareHash.GetUserHardwareHash();

                cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.String);
                KeyValidator.SetUserString(KeyValidator.USERSTRINGNUMBERS.USN_NOTUSED_7, MachineUniqueId);

                return MachineUniqueId;
#if false
                


                string finalHash = string.Empty;

                HashList = new List<string>();

                List<string> Bios = cUtilityFunctions.ExecuteWmiQuery(query_BIOS, Win32_BIOS_OptionNames);
                List<string> BaseBoard = cUtilityFunctions.ExecuteWmiQuery(query_BaseBoard, Win32_BaseBoard_OptionNames);
                List<string> NetworkAdapter = cUtilityFunctions.ExecuteWmiQuery(query_NetworkAdapter, Win32_NetworkAdapter_OptionNames).Select(x => x.Replace(":", "")).ToList();
                List<string> ComputerSystem = cUtilityFunctions.ExecuteWmiQuery(query_ComputerSystem, Win32_ComputerSystem_OptionNames);
                List<string> OriginalMacAddresses = NetworkCards.GetOriginalMacAddresses();               

                /// get all values and add it to final list.
                HashList.AddRange(Bios);
                HashList.AddRange(BaseBoard);
                HashList.AddRange(NetworkAdapter);
                HashList.AddRange(ComputerSystem);
                HashList.AddRange(OriginalMacAddresses);

                /// get distinct values
                HashList.Sort();
                HashList = HashList.Distinct().ToList();
                HashList.Sort();                

                /// combine all strings.
                string finalStr = String.Join("", HashList.ToArray());

                /// get sha1
                finalHash = cUtilityFunctions.GetSha1HashFromString(finalStr);                

                /// get crc64
                MachineUniqueId = (long)CRC64CPP.Compute(0, Encoding.Unicode.GetBytes(finalHash.ToLower()));                

                cGlobalSettings.SetCommonAppDataRootRegistryValue(cRegistrySettings.REGSTR_VALUE_MACHINE_UNIQUE_ID, MachineUniqueId, Microsoft.Win32.RegistryValueKind.QWord);

                try
                {
                    /// log hash details
                    StringBuilder sb = new StringBuilder();
                    Bios.ForEach(x => sb.Append(string.Format("\r\nBios:{0}", x)));
                    BaseBoard.ForEach(x => sb.Append(string.Format("\r\nBaseBoard:{0}", x)));
                    NetworkAdapter.ForEach(x => sb.Append(string.Format("\r\nNetworkAdapter:{0}", x)));
                    OriginalMacAddresses.ForEach(x => sb.Append(string.Format("\r\nOriginalMacAddresses:{0}", x)));
                    ComputerSystem.ForEach(x => sb.Append(string.Format("\r\nComputerSystem:{0}", x)));                    
                    string HashPartStrings = String.Join("\t", HashList.ToArray());
                    sb.Append("\r\n"+HashPartStrings);
                    sb.Append(string.Format("PC Details:{0}\r\nCombined:{1}\r\nString1:{2}\r\nString2:{3}", HashPartStrings, finalStr, finalHash, MachineUniqueId));
                    cGlobalSettings.oLogger.WriteLogVerbose(sb.ToString());

                    sb.Remove(0, sb.Length - 1);
                    sb = null;
                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("GetUserHardwareHash-1", ex);
                }
                  
#endif

                return MachineUniqueId;

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetUserHardwareHash", ex);
            }
            finally
            {
                if (HashList != null) HashList.Clear(); HashList = null;
            }

            return MachineUniqueId;
        }

        public static string GetOldHash()
        {
            string m_Password = string.Empty;
            try
            {
                m_Password = string.Empty;
                List<string> cmd = Environment.GetCommandLineArgs().Select(x => x.ToLower()).ToList();
                cGlobalSettings.oLogger.WriteLogVerbose("GetOldHash|inside searching for oldhash envvars : " + string.Join("|", cmd.ToArray()));
                if (cmd.Contains(cGlobalSettings.ASP_CMD_PARAMS_SILENT_SCAN) || cmd.Contains(cGlobalSettings.ASP_CMD_PARAMS_FIRSTSCAN) || cmd.Contains(cGlobalSettings.ASP_CMD_PARAMS_INSTALL))
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("GetOldHash|inside searching for oldhash");

                    StreamReader oSr = new StreamReader(Path.Combine(cGlobalSettings.LogPath, "rightbackup.txt"));
                    string sAlldata = oSr.ReadToEnd();
                    oSr.Close();
                    oSr.Dispose();
                    string sPattern1 = "(.+?)cSQliteFunction:db:password:(?<pwd>.+?)\r\n";
                    string sPattern2 = "(.+?)cSQliteFunction:db:acc:(?<pwd>.+?)\r\n";
                    string sPattern3 = "(.+?)sdbacc:(?<pwd>.+?)\r\n";
                    Match oMatch = Regex.Match(sAlldata, sPattern1, RegexOptions.Compiled);
                    m_Password = oMatch.Groups["pwd"].Value;
                    if (string.IsNullOrEmpty(m_Password))
                    {
                        oMatch = Regex.Match(sAlldata, sPattern2, RegexOptions.Compiled);
                        m_Password = oMatch.Groups["pwd"].Value;
                    }
                    if (string.IsNullOrEmpty(m_Password))
                    {
                        oMatch = Regex.Match(sAlldata, sPattern3, RegexOptions.Compiled);
                        m_Password = oMatch.Groups["pwd"].Value;
                    }

                }

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetOldHash", ex);
            }
            return m_Password;
            // cSQliteFunction:db:password:-5428567495413647493
        }




    }
}
