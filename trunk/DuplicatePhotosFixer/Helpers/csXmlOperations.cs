using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.ClassDictionary;
using DuplicatePhotosFixer.Engine.WinApiScan;
using DuplicatePhotosFixer.HelperClasses;
using static DuplicatePhotosFixer.cClientEnum;

namespace DuplicatePhotosFixer.Helpers
{
    class csXmlOperations
    {

        static string GetScanModeXMLFilePath(eScanMode scanMode)
        {
            string xmlFilePath = cGlobalSettings.xmlFilePath;


            switch (scanMode)
            {
                case eScanMode.FileSearch:
                    xmlFilePath = cGlobalSettings.xmlFilePath;
                    break;
                case eScanMode.GoogleDrive:
                    xmlFilePath = cGlobalSettings.xmlFilePath_GD;
                    break;
                case eScanMode.Dropbox:
                    xmlFilePath = cGlobalSettings.xmlFilePath_DB;
                    break;
               
            }


            return xmlFilePath;
        }



        public static void LoadScanPathsList()
        {
            try
            {
                string strXMLPath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

                if (File.Exists(strXMLPath))
                {
                    cGlobalSettings.listScanPaths = cSerializer.DeSerializeObject<List<csScanPaths>>(GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
                }

                if (cGlobalSettings.listScanPaths == null)
                    cGlobalSettings.listScanPaths = new List<csScanPaths>();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static bool SaveScanPathsList(List<csScanPaths> ListScanPaths)
        {
            return cSerializer.SerializeToXML(ListScanPaths, GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
        }


        public static void removeXmlElement(string pathSelected, eTypeOfInclusion TypeOfInclusion)
        {
            XDocument xdocument = null;
            IEnumerable<XElement> pathList = null;
            try
            {
                xdocument = XDocument.Load(cGlobalSettings.xmlFilePath);
                pathList = xdocument.Elements().ElementAt(0).Elements();
                foreach (var path in pathList)
                {
                    if (string.Compare(path.Element("ScanPath").Value.AddBackSlash().ToString().ToLower(), pathSelected.ToLower().AddBackSlash(), true) == 0 && string.Compare(path.Element("TypeOfInclusion").Value.ToString(), TypeOfInclusion.ToString(), true) == 0)
                    {
                        path.Remove();
                        break;
                    }
                }
                xdocument.Save(cGlobalSettings.xmlFilePath);
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csXmlOperation :: removeXmlElement ::", ex);
            }
            finally
            {
                if (xdocument != null) xdocument = null;
                if (pathList != null) pathList = null;
            }
        }
        //public static void LoadFileFormatsList()
        //{
        //    try
        //    {
        //        string strXMLPath = GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode);

        //        if (File.Exists(strXMLPath))
        //        {
        //            cGlobalSettings.listFileTypes = cSerializer.DeSerializeObject<List<csFileTypes>>(GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
        //        }

        //        if (cGlobalSettings.listFileTypes == null)
        //            cGlobalSettings.listFileTypes = new List<csFileTypes>();


        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //public static bool SaveFileExtensionsList(List<csFileTypes> ListFileFormatTypes)
        //{
        //    return cSerializer.SerializeToXML(ListFileFormatTypes, GetScanModeXMLFilePath(cGlobalSettings.CurrentScanMode));
        //}


    }
}
