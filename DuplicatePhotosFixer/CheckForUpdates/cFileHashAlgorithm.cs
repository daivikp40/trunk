using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.CheckForUpdates
{
    public class cFileHashAlgorithm : IDisposable
    {
        protected HashAlgorithm hashAlgorithm;

        protected int bufferSize = 4096;

        public cFileHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }
        public cFileHashAlgorithm()
        {
        }

        public string ComputeHash(Stream stream)
        {
            string sRet = string.Empty;
            try
            {
                byte[] hash = null;
                int _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing 

                byte[] readAheadBuffer, buffer;
                int readAheadBytesRead, bytesRead;
                long size, totalBytesRead = 0;

                size = stream.Length;
                readAheadBuffer = new byte[_bufferSize];
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

                do
                {
                    bytesRead = readAheadBytesRead;
                    buffer = readAheadBuffer;

                    readAheadBuffer = new byte[_bufferSize];
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    if (readAheadBytesRead == 0)
                        hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                    else
                        hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                } while (readAheadBytesRead != 0);
                hash = hashAlgorithm.Hash;
                sRet = BitConverter.ToString(hash);
                sRet = sRet.Replace("-", string.Empty);
                if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|ComputeHash|Exception {0}", ex.Message));
            }
            return sRet;
        }

        public byte[] ComputeHash(Stream stream, long nStartPos, long nBytesToRead)
        {
            byte[] hash = null;
            try
            {
                int _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing 

                byte[] readAheadBuffer, buffer;
                int readAheadBytesRead, bytesRead;
                long totalBytesRead = 0;
                long nBytesRemaining = nBytesToRead;
                if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                readAheadBuffer = new byte[_bufferSize];
                if (nStartPos > 0) stream.Seek(nStartPos, SeekOrigin.Begin);//advance the position in stream
                readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                totalBytesRead += readAheadBytesRead;

                do
                {
                    bytesRead = readAheadBytesRead;
                    buffer = readAheadBuffer;
                    nBytesRemaining = nBytesToRead - totalBytesRead;
                    _bufferSize = bufferSize;
                    if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                    readAheadBuffer = new byte[_bufferSize];
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    if ((readAheadBytesRead == 0) || (nBytesRemaining == 0)) hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                    else hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                    if (nBytesRemaining == 0) break;
                } while (readAheadBytesRead != 0);
                hash = hashAlgorithm.Hash;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|ComputeHash|Exception {0}", ex.Message));
            }
            return hash;
        }

        public bool UpdateHash(string sFilePath, long nStartPos, long nBytesToRead)
        {
            //byte[] hash = null;        
            try
            {
                using (var stream = File.Open(sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing 

                    byte[] readAheadBuffer, buffer;
                    int readAheadBytesRead, bytesRead;
                    long totalBytesRead = 0;
                    long nBytesRemaining = nBytesToRead;
                    if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                    readAheadBuffer = new byte[_bufferSize];
                    if (nStartPos > 0) stream.Seek(nStartPos, SeekOrigin.Begin);//advance the position in stream
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    do
                    {
                        bytesRead = readAheadBytesRead;
                        buffer = readAheadBuffer;
                        nBytesRemaining = nBytesToRead - totalBytesRead;
                        _bufferSize = bufferSize;
                        if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                        readAheadBuffer = new byte[_bufferSize];
                        readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                        totalBytesRead += readAheadBytesRead;

                        //if ((readAheadBytesRead == 0) || (nBytesRemaining == 0)) hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                        //else 
                        hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                        if (nBytesRemaining == 0) break;
                    } while (readAheadBytesRead != 0);


                    stream.Close();
                    stream.Dispose();
                }

                //hash = hashAlgorithm.Hash;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|ComputeHash|Exception {0}", ex.Message));

                return false;
            }
            return true;
            //return hash;
        }

        public string FinalHash(string sFilePath, long nStartPos, long nBytesToRead)
        {
            byte[] hash = null;
            string sRet = string.Empty;
            try
            {
                using (var stream = File.Open(sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    int _bufferSize = bufferSize; // this makes it impossible to change the buffer size while computing 

                    byte[] readAheadBuffer, buffer;
                    int readAheadBytesRead, bytesRead;
                    long totalBytesRead = 0;
                    long nBytesRemaining = nBytesToRead;
                    if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                    readAheadBuffer = new byte[_bufferSize];
                    if (nStartPos > 0) stream.Seek(nStartPos, SeekOrigin.Begin);//advance the position in stream
                    readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;

                    do
                    {
                        bytesRead = readAheadBytesRead;
                        buffer = readAheadBuffer;
                        nBytesRemaining = nBytesToRead - totalBytesRead;
                        _bufferSize = bufferSize;
                        if (nBytesToRead < bufferSize) _bufferSize = (int)nBytesRemaining;
                        readAheadBuffer = new byte[_bufferSize];
                        readAheadBytesRead = stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                        totalBytesRead += readAheadBytesRead;

                        if ((readAheadBytesRead == 0) || (nBytesRemaining == 0)) hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                        else hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                        if (nBytesRemaining == 0) break;
                    } while (readAheadBytesRead != 0);
                    hash = hashAlgorithm.Hash;

                    stream.Close();
                    stream.Dispose();
                }

                if (hash != null)
                {
                    sRet = BitConverter.ToString(hash);
                    sRet = sRet.Replace("-", string.Empty);
                    if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                }

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|ComputeHash|Exception {0}", ex.Message));
            }
            return sRet;
        }

        public string GetMd5Hash(string sFilePath, ref long nFileLength)
        {
            string sRet = string.Empty;
            FileStream oFileStream = null;
            try
            {
                if (!string.IsNullOrEmpty(sFilePath) && File.Exists(sFilePath))
                {

                    oFileStream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (var md5 = new MD5CryptoServiceProvider())
                    {
                        byte[] result = md5.ComputeHash(oFileStream);

                        nFileLength = oFileStream.Length;
                        oFileStream.Close();
                        StringBuilder hashString = new StringBuilder(33);

                        for (int i = 0; i < result.Length; i++)
                        {
                            hashString.Append(Convert.ToString(result[i], 16).PadLeft(2, '0'));
                        }
                        sRet = hashString.ToString();
                        result = null;
                    }


                    //md5 = null;

                    //oFileStream = File.Open(sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    //nFileLength = oFileStream.Length;
                    //hashAlgorithm = MD5.Create();
                    //sRet = ComputeHash(oFileStream);
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5Hash|Exception {0}", ex.Message));
            }
            finally
            {
                if (oFileStream != null)
                {
                    oFileStream.Close();
                    oFileStream = null;
                }
            }
            return sRet;
        }

        public string GetMd5Hash(string sFilePath)
        {
            string sRet = string.Empty;
            FileStream oFileStream = null;
            try
            {
                if (!string.IsNullOrEmpty(sFilePath) && File.Exists(sFilePath))
                {
                    oFileStream = File.Open(sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    hashAlgorithm = MD5.Create();
                    sRet = ComputeHash(oFileStream);
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5Hash|Exception {0}", ex.Message));
            }
            finally
            {
                if (oFileStream != null)
                {
                    oFileStream.Close();
                    oFileStream = null;
                }
            }
            return sRet;
        }


        internal string getSha128(string sFilepath)
        {

            try
            {

                FileStream file = new FileStream(sFilepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                // This is one implementation of the abstract class MD5.
                SHA1 md5 = new SHA1CryptoServiceProvider();

                byte[] result = md5.ComputeHash(file);
                file.Close();
                StringBuilder hashString = new StringBuilder(33);

                for (int i = 0; i < result.Length; i++)
                {
                    hashString.Append(Convert.ToString(result[i], 16).PadLeft(2, '0'));
                }

                return hashString.ToString();
            }

            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|getSha128|Exception {0}", ex.Message));
                return ex.ToString();
            }
        }


        public string GetMd5HashFromString(string sString)
        {
            string sRet = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sString))
                {

                    byte[] buffer = Encoding.Unicode.GetBytes(sString);
#if DEBUG
                    //byte[] buffer = Encoding.Unicode.GetBytes(sString);
#elif release_test
                byte[] buffer = Encoding.Unicode.GetBytes(sString);
#elif releasebeta
            byte[] buffer = (cGlobalSettings.IsBetaBuild()) ? Encoding.ASCII.GetBytes(sString)  : Encoding.Unicode.GetBytes(sString);
#elif release
                byte[] buffer = Encoding.Unicode.GetBytes(sString);
#endif

                    //#if release
                    //                byte[] buffer = (cGlobalSettings.IsBetaBuild()) ? Encoding.ASCII.GetBytes(sString)  : Encoding.Unicode.GetBytes(sString);
                    //#else
                    //                byte[] buffer = Encoding.Unicode.GetBytes(sString);
                    //               // byte[] buffer = Encoding.ASCII.GetBytes(sString);
                    //#endif
                    hashAlgorithm = MD5.Create();
                    hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                    byte[] hash = hashAlgorithm.Hash;
                    sRet = BitConverter.ToString(hash);
                    sRet = sRet.Replace("-", string.Empty);
                    if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5HashFromString|Exception {0}", ex.Message));
            }

            return sRet;
        }


        public string GetMd5HashFromStringUni(string sString)
        {
            string sRet = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sString))
                {

#if !release
                    byte[] buffer = Encoding.Unicode.GetBytes(sString);
#else
                byte[] buffer = Encoding.ASCII.GetBytes(sString);
#endif
                    hashAlgorithm = MD5.Create();
                    hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                    byte[] hash = hashAlgorithm.Hash;
                    sRet = BitConverter.ToString(hash);
                    sRet = sRet.Replace("-", string.Empty);
                    if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5HashFromString|Exception {0}", ex.Message));
            }

            return sRet;
        }


        public string GetMd5HashFromByteArray(byte[] buffer)
        {
            string sRet = string.Empty;
            try
            {
                if (buffer != null && buffer.Length > 0)
                {
                    hashAlgorithm = MD5.Create();
                    hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                    byte[] hash = hashAlgorithm.Hash;
                    sRet = BitConverter.ToString(hash);
                    sRet = sRet.Replace("-", string.Empty);
                    if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5HashFromString|Exception {0}", ex.Message));
            }

            return sRet;
        }

        public string GetSha1HashFromString(string sString)
        {
            string sRet = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sString))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(sString);
                    hashAlgorithm = SHA1.Create();
                    hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
                    byte[] hash = hashAlgorithm.Hash;
                    sRet = BitConverter.ToString(hash);
                    sRet = sRet.Replace("-", string.Empty);
                    if (!string.IsNullOrEmpty(sRet)) sRet = sRet.ToLower();
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLog(string.Format("cFileHashAlgorithm|GetMd5HashFromString|Exception {0}", ex.Message));
            }

            return sRet;
        }

        public void Dispose()
        {
            if (hashAlgorithm != null)
            {
                hashAlgorithm.Clear();
                hashAlgorithm = null;
            }
        }
    }
}
