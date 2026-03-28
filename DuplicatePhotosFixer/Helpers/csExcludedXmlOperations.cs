using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.HelperClasses;

namespace DuplicatePhotosFixer.Helpers
{
    class csExcludedXmlOperations
    {

        static string GetScanModeXMLFilePath(eScanMode scanMode)
        {
            string xmlFilePath = cGlobalSettings.xmlFilePath;
            switch (scanMode)
            {
                case eScanMode.FileSearch:
                    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath;
                    break;
                //case eScanMode.GoogleDrive:
                //    string UserID = AppFunctions.CreateMD5Hash(cGlobalSettings.currentGoogleDriveUserId);
                //    xmlFilePath = string.Format(cGlobalSettings.xmlExludedFoldersPath_GD, UserID);
                //    break;
                //case eScanMode.PicasaLibrary:
                //    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath_PL;
                //    break;
                //case eScanMode.Lightroom:
                //    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath_LR;
                //    break;
                //case eScanMode.Dropbox:
                //    string UserID_DB = AppFunctions.CreateMD5Hash(cGlobalSettings.currentDropboxUserId);
                //    xmlFilePath = string.Format(cGlobalSettings.xmlExludedFoldersPath_DB, UserID_DB);
                //    break;
                //case eScanMode.PhotoOrganizer:
                //    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath_PO;
                //    break;
                //case eScanMode.BoxCloud:
                //    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath_BX;
                //    break;
                //case eScanMode.AmazonS3:
                //    xmlFilePath = cGlobalSettings.xmlExludedFoldersPath_S3;
                //    break;
                //case eScanMode.DeleteEmptyFolder:
                //    break;
                default:
                    break;
            }
            return xmlFilePath;
        }

        public static void LoadExcludedFolderPathsList()
        {
            try
            {
                string strXMLPath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (File.Exists(strXMLPath))
                {
                    cGlobalSettings.listExcludedFolderPaths = cSerializer.DeSerializeObject<List<csExcludedPath>>(GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
                }

                if (cGlobalSettings.listExcludedFolderPaths == null)
                    cGlobalSettings.listExcludedFolderPaths = new List<csExcludedPath>();
              }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void addExcludedFolder(string folderToExclude)
        {
            XElement xEle = null;
            try
            {
                string xmlFilePath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (!File.Exists(xmlFilePath))
                {
                    LoadExcludedFolderPathsList();
                }
                xEle = XElement.Load(xmlFilePath);
                xEle.Add(new XElement("path",
                                               new XElement("excludedPath", folderToExclude)
                                           ));
                xEle.Save(xmlFilePath);

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("addExcludedFolder", ex);
            }
            finally
            {
                xEle = null;
            }
        }

        public static bool SaveExcludeFolderPathsList(List<csExcludedPath> ListExcludeFolderPaths)
        {
            return cSerializer.SerializeToXML(ListExcludeFolderPaths, GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
        }
    }
}
