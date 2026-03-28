using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Trial_Protection.Engine
{
   public class UtilityFunctions
    {
        /// <summary>
        /// Returns the sha1 of string
        /// </summary>
        /// <param name="sString"></param>
        /// <returns></returns>
        public static string GetSha1HashFromString(string sString)
        {
            string sRet = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sString))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(sString);
                    using (var hashAlgorithm = SHA1.Create())
                    {
                        hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                        byte[] hash = hashAlgorithm.Hash;
                        sRet = BitConverter.ToString(hash);
                        sRet = sRet.Replace("-", string.Empty);
                        if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                    }

                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("HardwareHash|GetSha1HashFromString|Exception {0}", ex.Message));
            }

            return sRet;
        }
    }
}
