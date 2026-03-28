using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DuplicatePhotosFixer.Helpers;

namespace DuplicatePhotosFixer.App_code
{
   public class csExcludedFolders
    {
        public static bool createExcludedXmLDocumet()
        {
            bool isCreated = false;
            try
            {
                string xmlFilePath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (!File.Exists(xmlFilePath))
                {
                    var xEle = new XElement("pathList", null);
                    xEle.Save(xmlFilePath);
                }
                isCreated = true;
            }
            catch
            {

            }
            return isCreated;
        }

        public static void addExcludedFolder(string folderToExclude)
        {
            XElement xEle = null;
            try
            {
                string xmlFilePath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (!File.Exists(xmlFilePath))
                {
                    createExcludedXmLDocumet();
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

        public static void removExcludedPathXmlElement(string xmlExludedFoldersPath)
        {
            XDocument xdocument = null;
            IEnumerable<XElement> pathList = null;
            try
            {
                string xmlFilePath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                xdocument = XDocument.Load(xmlFilePath);
                pathList = xdocument.Elements().ElementAt(0).Elements();
                foreach (var path in pathList)
                {
                    if (path.Element("excludedPath").Value == xmlExludedFoldersPath)
                    {
                        path.Remove();
                        break;
                    }
                }

                xdocument.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csExcludedFolders :: removExcludedPathXmlElement :", ex);
            }
            finally
            {
                if (xdocument != null) xdocument = null;
                if (pathList != null) pathList = null;
            }
        }

        public static void xmlToExcludedPathList()
        {
            XDocument xdocument = null;
            IEnumerable<XElement> scanPaths = null;
            try
            {
                string xmlFilePath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (File.Exists(xmlFilePath))
                {
                    xdocument = XDocument.Load(xmlFilePath);
                    scanPaths = xdocument.Elements().ElementAt(0).Elements();
                    foreach (var scanPath in scanPaths)
                    {
                        string excludedPath = scanPath.Element("excludedPath").Value;

                        // Add path to List N DataGrid
                       // Program.oMainReference.preferencesform_addExcludFolderPath(excludedPath);
                        //Program.oMainReference.Home_addSelectedPath(isSelected, strPath, (enumClass.ePathType)Enum.Parse(typeof(enumClass.ePathType), pathType, true), false);
                    }
                }
                else
                {
                    //create a file with default exclusion folders
                    foreach (string defaultExcludedFolder in cGlobalSettings.listDefaultExcludePaths)
                    {
                        addExcludedFolder(defaultExcludedFolder);
                       // Program.oMainReference.preferencesform_addExcludFolderPath(defaultExcludedFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("xmlToExcludedPathList", ex);
            }
            finally
            {
                if (xdocument != null) xdocument = null;
                if (scanPaths != null) scanPaths = null;
            }
        }


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
    }
}
