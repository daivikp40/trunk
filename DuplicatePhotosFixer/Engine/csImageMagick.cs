using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicatePhotosFixer.HelperClasses;
using DuplicatePhotosFixer.Helpers;
using ImageMagick;

namespace DuplicatePhotosFixer.Engine
{
    public static class csImageMagick
    {
        //const string sSaveFolderPath = @"C:\Users\pranav.khuteta\AppData\Roaming\Systweak\Duplicate Photos Fixer Pro\thumbnails\";


        /// <summary>
        /// Convert heif to jpg and then create a thumbnail of BitmapSize size.
        /// Get MD5 of heic file path and save thumbnail with this <md5>.jpg 
        /// Get exif info from converted image and delete the file.
        /// Note: We save only thumbnail image (to show on ImageListView control) not converted image.
        /// 
        /// </summary>
        /// <param name="ImageFullPath"></param>
        /// <param name="imgID"></param>
        /// <param name="ThumbSize"></param>
        /// <returns></returns>




        static ulong maxThreads = (ulong)AppFunctions.GetMaxDegreeOfParallelism();

        public static string ConvertRAWToJPG(string ImageFullPath, int ImgId, ref string ThumbPath, bool bViaCmd = true, int ThumbSize = 250)
        {
            MagickImage imm = null;
            string strFullSaveFilePath = ImageFullPath;
            try
            {
                //RunAppRetryImageMagick(ImageFullPath, ImgId);
                //string strOriginalPathMD5 = AppFunctions.calcMD5();
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(ImageFullPath));
                string strFileName = string.Format("{0}.jpg", ImgId);
                strFullSaveFilePath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName);
                ThumbPath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName_Thumb);
                //ImageMagick.ResourceLimits.Thread = (ulong)AppFunctions.GetMaxDegreeOfParallelism();

                if (bViaCmd)
                {
                    RunAppRetryImageMagick(ImageFullPath, ImgId);
                }
                else
                {
                    using (imm = new MagickImage(ImageFullPath))
                    {
                        /// Write image with JPG format
                        /// 
                        imm.Write(strFullSaveFilePath, MagickFormat.Jpg);

                        //Size thumbSize = GetThumbnalSize(imm.Width, imm.Height, ThumbSize);
                        //imm.Thumbnail(thumbSize.Width, thumbSize.Height);
                        //imm.Write(ThumbPath, MagickFormat.Jpg);


                        //imm.Resize(thumbSize.Width, thumbSize.Height);

                        /// Save thumbnail
                        /// 
                        //imm.Thumbnail(thumbSize.Width, thumbSize.Height);

                        /// Create thumbnail with path MD5 name and delete save file.
                        /// 
                    }
                }
            }
            catch (MagickException mex)
            {
                /*if (mex.Message.Contains(@"ReadDNGImage/467") &&!bViaCmd && !Program.bCreateThumb)
                {
                    Thread.Sleep(0);
                    //GC.Collect();
                    RunAppRetryImageMagick(ImageFullPath, ImgId);
                }*/
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: ConvertHeicToJPG: ", ex);
            }
            finally
            {
                if (imm != null)
                    imm.TryDispose();
                imm = null;
            }

            return strFullSaveFilePath;
        }

        public static bool SaveImageThumb(string ImageFullPath, ref string ThumbPath, int ThumbSize = 250)
        {
            MagickImage imm = null;
            bool bThumbCreated = false;
            try
            {
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(ImageFullPath));
                ThumbPath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName_Thumb);
                using (imm = new MagickImage(ImageFullPath))
                {
                    /// Write image thumbnail with JPG format
                    /// 
                    Size thumbSize = GetThumbnalSize((int)imm.Width, (int)imm.Height, ThumbSize);
                    imm.Thumbnail((int)(uint)thumbSize.Width, (int)(uint)thumbSize.Height);
                    imm.Write(ThumbPath, MagickFormat.Jpg);
                }

                if (File.Exists(ThumbPath))
                    bThumbCreated = true;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: ConvertHeicToJPG: ", ex);
            }
            finally
            {
                if (imm != null)
                    imm.TryDispose();
                imm = null;
            }

            return bThumbCreated;
        }

        static void RunAppRetryImageMagick(string ImageFullPath, int ImgId)
        {
            try
            {
                string args = string.Format("{0}{1} imgid={2} imgpath=\"{3}\"", "", cGlobalSettings.CMD_PARAMS_CREATETHUMB, ImgId, ImageFullPath);

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = Application.ExecutablePath;
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DPFHelper.exe");
                process.StartInfo.Arguments = args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("RunAppRetryImageMagick:: ", ex);
            }
        }

        /*
        public static string ConvertHeicToJPG(FileStream stream, string ImageFullPath, int ImgId, int ThumbSize, ref string ThumbPath)
        {
            MagickImage imm = null;
            string strFullSaveFilePath = ImageFullPath;
            try
            {
                //string strOriginalPathMD5 = AppFunctions.calcMD5();
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(ImageFullPath));
                string strFileName = string.Format("{0}.jpg", ImgId);
                strFullSaveFilePath = Path.Combine(strThumbSavePath, strFileName);
                ThumbPath = Path.Combine(strThumbSavePath, strFileName_Thumb);
                imm = new MagickImage(stream);

                /// Write image with JPG format
                /// 
                imm.Write(strFullSaveFilePath, MagickFormat.Jpg);


                Size thumbSize = GetThumbnalSize(imm.Width, imm.Height, ThumbSize);
                imm.Thumbnail(thumbSize.Width, thumbSize.Height);
                imm.Write(ThumbPath, MagickFormat.Jpg);
                //imm.Resize(thumbSize.Width, thumbSize.Height);

                /// Save thumbnail
                /// 
                //imm.Thumbnail(thumbSize.Width, thumbSize.Height);

                /// Create thumbnail with path MG5 name and delete save file.
                /// 
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: ConvertHeicToJPG: ", ex);
            }
            finally
            {
                if (imm != null)
                    imm.TryDispose();
                imm = null;
            }

            return strFullSaveFilePath;
        }

        */
        /*
        internal void ConvertHeicToJPG(FileStream fileStream, string imgID, int BitmapSize)
        {
            MagickImage imm = null;
            try
            {
                string strFileName = string.Format("{0}_{1}.jpg", imgID, BitmapSize);
                string strFullSaveFilePath = Path.Combine(sSaveFolderPath, imgID);
                imm = new MagickImage(fileStream, MagickFormat.Jpg);
                
                Size thumbSize = GetThumbnalSize(imm.Width, imm.Height, BitmapSize);
                imm.Thumbnail(thumbSize.Width, thumbSize.Height);
                imm.Write(strFullSaveFilePath);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: ConvertHeicToJPG: ", ex);
            }
            finally
            {
                if (imm != null)
                    imm.TryDispose();
                imm = null;
            }
        }
        */
        static Size GetThumbnalSize(int actualWidth, int actualHeight, int BitMapSize)
        {
            Size size = new Size();
            if (actualWidth == actualHeight)
            {
                size.Width = size.Height = BitMapSize;
            }
            else if (actualWidth > actualHeight)
            {
                int nFactor = (int)Math.Ceiling((float)actualWidth / BitMapSize);

                size.Width = actualWidth / nFactor;
                size.Height = actualHeight / nFactor;
            }
            else
            {
                int nFactor = (int)Math.Ceiling((float)actualHeight / BitMapSize);

                size.Width = actualWidth / nFactor;
                size.Height = actualHeight / nFactor;
            }
            return size;
        }

        public static string GetThumbnailImage(string SourcePath, bool bCreateThumbIfNotFound = false)
        {
            //return null;
            string ThumbPath = null;

            try
            {
                string strFileName_Thumb = string.Format("{0}.jpg", AppFunctions.CreateMD5Hash(SourcePath));
                ThumbPath = Path.Combine(cGlobalSettings.strThumbSaveFolderPath, strFileName_Thumb);

                if (File.Exists(ThumbPath))
                {
                    return ThumbPath;
                }
                else
                {
                    bool bNew = false;
                    var extFound = cGlobalSettings.listFileType.Where(p => p.Key > -1 && p.Value.ImageFileExtension.IndexOf(Path.GetExtension(SourcePath), StringComparison.OrdinalIgnoreCase) >= 0);

                    if (extFound != null)
                    {
                        bNew = extFound.FirstOrDefault().Value.IsNewLibSupportedExt;
                    }

                    if (!bNew)
                        return null;

                    if (bCreateThumbIfNotFound && SaveImageThumb(SourcePath, ref ThumbPath))
                    {
                        return ThumbPath;
                    }
                    else
                    {
                        ThumbPath = null;
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csImageMagick:: GetThumbnailImage: ", ex);
            }
            return ThumbPath;
        }
    }
}
