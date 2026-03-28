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
    public class WMIFunctions
    {
        public static List<string> ExecuteWmiQuery(string query, List<string> fieldNames)
        {
            List<string> fieldValues = new List<string>();
            try
            {
                using (ManagementObjectSearcher moSearch = new ManagementObjectSearcher(query))
                {
                    using (ManagementObjectCollection moCollection = moSearch.Get())
                    {
                        foreach (ManagementObject mo in moCollection)
                        {
                            foreach (string field in fieldNames)
                            {
                                try
                                {
                                    string value = ReadWmiValue(mo, field, string.Empty).ToLower().Trim();//mo.Properties[field].Value.ToString().ToLower().Trim();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        fieldValues.Add(value);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    cGlobalSettings.oLogger.WriteLogException("ExecuteWmiQuery-1", ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ExecuteWmiQuery", ex);
            }

            return fieldValues;

        }


        /// <summary>
        /// Reads the key settings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ReadWmiValue<T>(ManagementObject mo, string key, T defaultReturnValue)
        {
            T retval = defaultReturnValue/*default(T)*/;
            try
            {
                if (mo == null)
                {
                    return defaultReturnValue;
                }

                object val = mo.Properties[key].Value;
                retval = (T)Convert.ChangeType(val, typeof(T));
            }
            catch (System.Exception ex)
            {
                //cGlobalSettings.oLogger.WriteLogException("NetworkCards::ReadWmiValue", ex);
            }

            return retval;
        }
    }
}
