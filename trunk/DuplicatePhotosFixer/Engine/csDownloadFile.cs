using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.Helpers;

namespace DuplicatePhotosFixer.Engine
{
    public class csDownloadFile
    {
        public int Retry = 1;


        public delegate void OnProgressProcess(int percentageCompleted, string message = "");
        public event OnProgressProcess OnProgress;


        public cClientEnum.eDownloadStatus DownloadFile(string urlToDownload, string FileName, string DownlaodedFolderPath, out string FullPath)
        {
            cClientEnum.eDownloadStatus status = cClientEnum.eDownloadStatus.DownloadSucessFull;

            HttpWebResponse webResponse = null;
            HttpWebRequest request = null;
            Stream responseStream = null;
            FileStream localFileStream = null;
            FullPath = string.Empty;
            try
            {
                if (OnProgress != null)
                    OnProgress(0, cResourceManager.LoadString("IDS_DOWNLOADING"));

                //check for disk space availability
                if (!cWin32APIs.IsMinDiskAvalaible(Directory.GetDirectoryRoot(cGlobalSettings.getAppDataApplicationPath())))
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Low disk space.");
                    status = cClientEnum.eDownloadStatus.DownloadFailed_LowDiskSpace;
                    return cClientEnum.eDownloadStatus.DownloadFailed_LowDiskSpace;
                }

                if (!AppFunctions.isInternetConnected())
                {
                    status = cClientEnum.eDownloadStatus.NoInternetConnection;
                    return status;
                }

                //
                // Is key is valid for this machineID and allow user to downloadDLL
                /*if (!KeyCheckEndPoint(string.Format("keyVerifyCheckURL", cGlobalSettings.cRegDetails.Key, cGlobalSettings.oArmadill.GetHardwareHash())))
                {
                    status = cClientEnum.eDownloadStatus.AccessDenied;
                    return status;
                }*/

                for (int retry = 0; retry < Retry; retry++)
                {
                    Int64 iRunningByteTotal = 0;

                    ///
                    /// Check is file is already downloaded/exist.
                    /// 
                    //if (File.Exists(Path.Combine(cGlobalSettings.GetDownloadPath(), Hash)))
                    //{
                    //    // Check is file has same SHA1 hash which is required to download.
                    //    bool isHashMatch = MatchHASH(Path.Combine(cGlobalSettings.GetDownloadPath(), Hash), Hash);
                    //    if (isHashMatch)
                    //    {
                    //        DownlaodedPath = Path.Combine(cGlobalSettings.GetDownloadPath(), Hash);
                    //        status = cClientEnum.eDownloadStatus.DownloadSucessFull;
                    //        break;
                    //    }
                    //}

                    try
                    { //try TLS 1.3 
                      //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00); // 0xc0 | 0x300 | 0xc00

                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                             | (SecurityProtocolType)3072
                                                             | (SecurityProtocolType)768
                                                             | SecurityProtocolType.Tls;
                    }
                    catch (NotSupportedException)
                    {
                        try
                        { //try TLS 1.2 
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
                                                                 | (SecurityProtocolType)768
                                                                 | SecurityProtocolType.Tls;
                        }
                        catch (NotSupportedException)
                        {
                            try
                            { //try TLS 1.1 
                                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768
                                                                     | SecurityProtocolType.Tls;
                            }
                            catch (NotSupportedException)
                            { //TLS 1.0 
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                            }
                        }
                    }


                    try
                    {
                        /// sample download URL: https://download.dll-files.com/client.php?hash=10051fd8dc7d5e42b9e3bbc997b70776&filename=CrySystem.dll&machineid=3814652201045951222&key=DG32M-NDRTW-P3BXP-VPEH7
                        request = (HttpWebRequest)HttpWebRequest.Create(urlToDownload);
                        request.ContentType = "application/json";
                        request.PreAuthenticate = true;
                        request.KeepAlive = false;
                        long TotalFileSize = 0;

                        using (webResponse = (HttpWebResponse)request.GetResponse())
                        {

                            TotalFileSize = webResponse.ContentLength;

                            using (responseStream = webResponse.GetResponseStream())
                            {
                                using (localFileStream = new FileStream(Path.Combine(DownlaodedFolderPath, FileName + ".partial"), FileMode.Create))
                                {
                                    bool allowToDown = true;
                                    if (allowToDown)
                                    {
                                        //Setups_status = StatusEnums.DownloadStatus.Downloading;
                                        int iByteSize;
                                        byte[] byteBuffer = new byte[8 * 1024];
                                        try
                                        {
                                            while ((iByteSize = responseStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                                            {
                                                if (cGlobalSettings.abortNow)
                                                {
                                                    request.Abort();
                                                    status = cClientEnum.eDownloadStatus.DownloadAborted;
                                                    break;
                                                }
                                                // write the bytes to the file system at the file path specified
                                                localFileStream.Write(byteBuffer, 0, iByteSize);
                                                iRunningByteTotal += iByteSize;

                                                float prog = (((float)iRunningByteTotal) * 100f) / ((float)TotalFileSize);
                                                if (OnProgress != null)
                                                    OnProgress((int)prog, null);
                                                //Trace.WriteLine(prog);

                                            }
                                        }
                                        catch
                                        {
                                            status = cClientEnum.eDownloadStatus.DownloadError;

                                            if (!AppFunctions.isInternetConnected())
                                            {
                                                status = cClientEnum.eDownloadStatus.NoInternetConnection;
                                                //return status;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //write(Path.Combine(Path.GetDirectoryName(saveTo), date + "SkippedSetups.txt"), urlToDownload + "\t::\tSize > " + ConfigurationManager.AppSettings["MaxSize"], true);
                                    }
                                    // clean up the file stream
                                    localFileStream.Flush();
                                    localFileStream.Close();
                                }
                                // close the connection to the remote server
                                //responseStream.Flush();
                                responseStream.Close();
                                responseStream.Dispose();
                            }
                            webResponse.Close();
                        }


                        /// --------------- Downloading complete --------------- 

                        if (status == cClientEnum.eDownloadStatus.DownloadSucessFull)
                        {

                            /// Match HASH 

                            // Update name, remove .partial extension from name.
                            string FullDownloadPath = Path.Combine(Path.GetDirectoryName(localFileStream.Name), Path.GetFileNameWithoutExtension(localFileStream.Name));
                            if (File.Exists(FullDownloadPath))
                            {
                                try
                                {
                                    File.Delete(FullDownloadPath);
                                }
                                catch (Exception ex)
                                {
                                    cGlobalSettings.oLogger.WriteLogException("Not able to delete file.", ex);
                                }
                            }
                            File.Move(localFileStream.Name, FullDownloadPath);

                            FullPath = FullDownloadPath;

                            status = cClientEnum.eDownloadStatus.DownloadSucessFull;
                        }
                    }
                    catch (WebException wex)
                    {
                        status = cClientEnum.eDownloadStatus.DownloadError;
                        WebExceptionStatus webStatus = wex.Status;
                        if (!AppFunctions.isInternetConnected())
                        {
                            status = cClientEnum.eDownloadStatus.NoInternetConnection;
                            //return status;
                        }
                        else if (webStatus == WebExceptionStatus.ProtocolError)
                        {
                            status = cClientEnum.eDownloadStatus.AccessDenied;
                            //return status;
                        }


                        cGlobalSettings.oLogger.WriteLogException("DownloadFile:: While downloading file: " + urlToDownload, wex);
                    }
                    catch (Exception ex)
                    {
                        status = cClientEnum.eDownloadStatus.DownloadError;

                        if (!AppFunctions.isInternetConnected())
                        {
                            status = cClientEnum.eDownloadStatus.NoInternetConnection;
                            //return status;
                        }

                        cGlobalSettings.oLogger.WriteLogException("DownloadFile:: While downloading file: " + urlToDownload, ex);
                    }

                    // Cases, when not to go for Retry...
                    if (status == cClientEnum.eDownloadStatus.DownloadSucessFull ||
                        status == cClientEnum.eDownloadStatus.AccessDenied ||
                        status == cClientEnum.eDownloadStatus.DownloadAborted)
                        break;
                }
            }
            catch (Exception ex)
            {
                status = cClientEnum.eDownloadStatus.DownloadError;
                cGlobalSettings.oLogger.WriteLogException("frmInstallDLL:: DownloadFile: ", ex);
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
                if (request != null) request = null;

                if (responseStream != null) responseStream.Close(); responseStream = null;

                if (localFileStream != null)
                {
                    localFileStream.Close();
                    localFileStream.Dispose();
                    localFileStream = null;
                }
            }
            return status;
        }
    }
}
