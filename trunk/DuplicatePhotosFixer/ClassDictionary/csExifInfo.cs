using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class csExifInfo
    {
        public int FileExifId = -1;
        public int Rating { get; set; }

        public string Keywords { get; set; }

        public string Orientation { get; set; }


        public string Exposure { get; set; }

        public string FocalLength { get; set; }

        public string ISOSpeedRating { get; set; }

        public string ExposureBias { get; set; }

        public string ExposureMode { get; set; }

        public string ExposureProgram { get; set; }

        public string MeteringMode { get; set; }

        public string LightSource { get; set; }

        public string SensingMethod { get; set; }

        public string ScenceCaptureMethod { get; set; }

        public string CameraMaker { get; set; }

        public string CameraModel { get; set; }

        public string CameraFirmware { get; set; }


        public string ColorSpace { get; set; }

        public string ColorModel { get; set; }

        public string ProfileName { get; set; }


        public string Altitude { get; set; }

        public string Latitiude { get; set; }

        public string Longitude { get; set; }

        public string LatitudeRef { get; set; }

        public string LongitudeRef { get; set; }

        public float HorizontalResolution { get; set; }

        public float VerticalResolution { get; set; }

        public DateTime CaptureDate { get; set; }

        public Size ImageDimension { get; set; }



        public string EditingSoftware { get; set; }
        public long FileSize { get; set; }

        //------------------------------------

        public DateTime dateCreated { get; set; }

        public DateTime dateModified { get; set; }

        public double latitudeDegree { get; set; }

        public double longitudeDegree { get; set; }

        public double latitudeRadian { get; set; }

        public double longitudeRadian { get; set; }

        public int PropertyCount { get; set; }

        public int BitDepth { get; set; }

        //public DateTime dateCreated;

#if false
         public csExifInfo(string ImagePath)
        {
            DateTime datePictureTaken;
            string LatitudeRef = string.Empty;
            double[] Latitude;
            string LongitudeRef = string.Empty;
            double[] Longitude = null;
            ExifReader reader = null;
            try
            {
                using (reader = new ExifReader(ImagePath))
                {
                    // ----------- Get photo dateTime  -----------
                    try
                    {
                        // Get date 
                        reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken);
                        
                        // Set property
                        dateCreated = datePictureTaken;
                    }
                    catch
                    {
                        ;// Property not found, Do nothing
                    }
                    // --------------------------------------------

                    // ----------- Get GSP values -----------
                    try
                    {
                        // Default values
                        latitudeDegree = 360;
                        latitudeRadian = 360;
                        longitudeDegree = 360;
                        longitudeRadian = 360;

                        // Get latitude
                        reader.GetTagValue<string>(ExifTags.GPSLatitudeRef, out LatitudeRef);
                        reader.GetTagValue<double[]>(ExifTags.GPSLatitude, out Latitude);
                        // Set property
                        latitudeDegree = ConvertDegreeAngleToDouble(Latitude[0], Latitude[1], Latitude[2], LatitudeRef); //  Latitude in degree
                        latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree); // Latitude in radian
                        // Get longitude 
                        reader.GetTagValue<double[]>(ExifTags.GPSLongitude, out Longitude);
                        reader.GetTagValue<string>(ExifTags.GPSLongitudeRef, out LongitudeRef);
                        // Set property
                        longitudeDegree = ConvertDegreeAngleToDouble(Longitude[0], Longitude[1], Longitude[2], LongitudeRef); // Longitude in degree
                        longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree); // Longitude in radian
                    }
                    catch
                    {
                        ;// Property not found, do nothing
                    }
                    //--------------------------------------------
                }
            }
            catch
            {
                // Not a valid jpeg file.

                dateCreated = DateTime.MinValue;
                latitudeDegree = 360;
                latitudeRadian = 360;
                longitudeDegree = 360;
                longitudeRadian = 360;
            }
            finally
            {
                Latitude = null;
                Longitude = null;

                if (reader!=null) reader.Dispose(); reader = null;
            }
        }
#endif


        public static int resolutionVertical011A = int.Parse("011A", System.Globalization.NumberStyles.HexNumber);

        public static int resolutionHorizontal011B = int.Parse("011B", System.Globalization.NumberStyles.HexNumber);

        public static int rating4746 = int.Parse("4746", System.Globalization.NumberStyles.HexNumber);

        public static int keywords4746 = int.Parse("9286", System.Globalization.NumberStyles.HexNumber);

        public static int orientation0112 = int.Parse("0112", System.Globalization.NumberStyles.HexNumber);

        public static int exposure0112 = int.Parse("829A", System.Globalization.NumberStyles.HexNumber);

        public static int focalLength920A = int.Parse("920A", System.Globalization.NumberStyles.HexNumber);

        public static int ISOSpeedRating8827 = int.Parse("8827", System.Globalization.NumberStyles.HexNumber);

        public static int ExposureBias9204 = int.Parse("9204", System.Globalization.NumberStyles.HexNumber);

        public static int ExposureMode9204 = int.Parse("A402", System.Globalization.NumberStyles.HexNumber);

        public static int ExposureProg9204 = int.Parse("8822", System.Globalization.NumberStyles.HexNumber);

        public static int meteringMode9204 = int.Parse("9207", System.Globalization.NumberStyles.HexNumber);

        public static int lightSource9209 = int.Parse("9209", System.Globalization.NumberStyles.HexNumber);

        public static int sensingMethod9209 = int.Parse("A217", System.Globalization.NumberStyles.HexNumber);

        public static int scenceCaptureMethodA406 = int.Parse("A406", System.Globalization.NumberStyles.HexNumber);

        public static int cameraMaker010F = int.Parse("010F", System.Globalization.NumberStyles.HexNumber);

        public static int cameraModel0110 = int.Parse("0110", System.Globalization.NumberStyles.HexNumber);

        public static int bitDepth5015 = int.Parse("5015", System.Globalization.NumberStyles.HexNumber);

        public static int altitude0006 = int.Parse("0006", System.Globalization.NumberStyles.HexNumber);

        public static int colorSpaceA001 = int.Parse("A001", System.Globalization.NumberStyles.HexNumber);
        public static int CaptureDate9003 = int.Parse("9003", System.Globalization.NumberStyles.HexNumber);

        /*public static int kuchHaiPataNahiParKaamAayega005 = int.Parse("005", System.Globalization.NumberStyles.HexNumber);
        public static int kuchHaiPataNahiParKaamAayega006 = int.Parse("0006", System.Globalization.NumberStyles.HexNumber);*/

        public static int latitudeRef0001 = int.Parse("0001", System.Globalization.NumberStyles.HexNumber);
        public static int latitude0002 = int.Parse("0002", System.Globalization.NumberStyles.HexNumber);

        public static int longitudeRef0003 = int.Parse("0003", System.Globalization.NumberStyles.HexNumber);
        public static int longitude0004 = int.Parse("0004", System.Globalization.NumberStyles.HexNumber);

        public static int editingSoftware0131 = int.Parse("0131", System.Globalization.NumberStyles.HexNumber);

        //Image img = null;
        //PropertyItem[] exifInfo = null;
        private static Regex r = new Regex(":");
        public csExifInfo()
        {

        }
        //public csExifInfo(int key, PropertyItem[] exifInfo, bool calcAllExif = false)
        //public csExifInfo(int key, Image img, bool calcAllExif = false)
        //{
        //    try
        //    {
        //        if (img == null)
        //            return;


        //        /*img = image;*/
        //        PropertyCount = 0;
        //        DateTime temp;
        //        PropertyItem[] exifInfo = img.PropertyItems;

        //        /*if(cGlobalSettings.listImageFileInfo.ContainsKey(key))
        //        {
        //            exifHorizontalResolution();

        //            exifCaptureDate();

        //            exifImageDimension();

                    
        //        }*/


        //        /*exifInfo.All(p =>
        //        {
        //            try
        //            {

        //                if (p.Value == null)
        //                    return true;

        //                cGlobalSettings.oLogger1.WriteLogVerbose(Encoding.ASCII.GetString(p.Value).Replace("\0", ""));
        //                return true;

        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //            return true;
        //        });*/




        //        #region --- Horizontal Resolution ---

        //        /*var _verticalResolution = exifInfo.Where(x => x.Id == resolutionVertical011A).SingleOrDefault();
        //        if (_verticalResolution != null)
        //        {
        //            VerticalResolution = BitConverter.ToInt32(_verticalResolution.Value, 0);
        //            PropertyCount++;
        //        }*/
        //        HorizontalResolution = img.HorizontalResolution;

        //        #endregion

        //        #region --- Vertical Resolution ---
        //        /*var _horizontalResolution = exifInfo.Where(x => x.Id == resolutionHorizontal011B).SingleOrDefault();
        //        if (_horizontalResolution != null)
        //        {
        //            HorizontalResolution = BitConverter.ToInt32(_horizontalResolution.Value, 0);
        //            PropertyCount++;
        //        } */
        //        VerticalResolution = img.VerticalResolution;
        //        #endregion

        //        #region --- CaptureDate ---
        //        CaptureDate = DateTime.MaxValue;

        //        //var _dateTaken = exifInfo.Where(x => x.Id == 36867).SingleOrDefault();
        //        /*var _dateTaken = exifInfo.Where(x => x.Id == 306).SingleOrDefault();
        //        if (_dateTaken==null)*/
        //        var _dateTaken = exifInfo.Where(x => x.Id == 36867).SingleOrDefault();
        //        if (_dateTaken == null)
        //            _dateTaken = exifInfo.Where(x => x.Id == 36868).SingleOrDefault();
        //        if (_dateTaken != null)
        //        {
        //            string dateTaken = r.Replace(Encoding.UTF8.GetString(_dateTaken.Value), "-", 2);
        //            bool success = DateTime.TryParse(dateTaken, out temp);
        //            if (success)
        //            {
        //                CaptureDate = temp;
        //                PropertyCount++;
        //            }
        //        }
        //        else
        //        {

        //           // ReadMetaData(cGlobalSettings.listImageFileInfo[key].filePath);

        //            /*
        //            Thread staThread = new Thread(new ParameterizedThreadStart(GetImageCaptureDate));
        //            staThread.SetApartmentState(ApartmentState.STA);
        //            staThread.Start(cGlobalSettings.listImageFileInfo[key].filePath);
        //            staThread.Join();
        //            */
        //            //CaptureDate = GetImageCaptureDate(cGlobalSettings.listImageFileInfo[key].filePath);
        //        }

        //        /*if (img.PropertyIdList.Any(x => x == 36867))
        //        {
        //            PropertyItem propItem = img.GetPropertyItem(36867);
        //            string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
        //            bool success = DateTime.TryParse(dateTaken, out temp);
        //            if (success)
        //            {
        //                CaptureDate = temp;
        //                PropertyCount++;
        //            }
        //        }*/
        //        #endregion

        //        #region --- Image Dimension N FileSize ----

        //        /*var _width = exifInfo.Where(x => x.Id == 256).SingleOrDefault();
        //        var _height = exifInfo.Where(x => x.Id == 257).SingleOrDefault();

        //        if (_width != null && _height != null)
        //        {
        //            int width=  0;
        //            int height= 0;
        //            if (_width.Value.Count() > 2)
        //            {
        //                width = BitConverter.ToInt32(_width.Value, 0);
        //                height = BitConverter.ToInt32(_height.Value, 0);
        //            }
        //            else
        //            {
        //                width = BitConverter.ToInt16(_width.Value, 0);
        //                height = BitConverter.ToInt16(_height.Value, 0);
        //            }
        //            ImageDimension = new Size(width, height);
        //            PropertyCount++;
        //            FileSize = width * height;
        //        }*/
        //        ImageDimension = img.Size;
        //        PropertyCount++;
        //        FileSize = img.Width * img.Height;
        //        #endregion

        //        #region--- Orientation ---
        //        var _orientation = exifInfo.Where(x => x.Id == orientation0112).SingleOrDefault();
        //        if (_orientation != null)
        //        {
        //            Orientation = _orientation.Value[0] + "";
        //            PropertyCount++;
        //            /*switch (Orientation)
        //            {
        //                case "1": // landscape, do nothing
        //                    Orientation = Orientation + " (Normal)";
        //                    break;

        //                case "8": // rotated 90 right
        //                    // de-rotate:
        //                    Orientation = Orientation + " (Rotate 270°)";
        //                    break;

        //                case "3": // bottoms up
        //                    Orientation = Orientation + " (Rotate 180°)";
        //                    break;

        //                case "6": // rotated 90 left
        //                    Orientation = Orientation + " (Rotate 90°)";
        //                    break;
        //            }*/
        //        }
        //        #endregion

        //        #region --- Bit Depth ----
        //        //bitDepth5015
        //        try
        //        {
        //            ///https://msdn.microsoft.com/en-us/library/system.drawing.imaging.pixelformat.aspx
        //            ///
        //            ///http://stackoverflow.com/questions/1126967/how-to-get-bitsperpixel-from-a-bitmap

        //            BitDepth = Image.GetPixelFormatSize(img.PixelFormat);

        //            /*string _bitDepth = img.PixelFormat.ToString();
        //            int asfsd = _bitDepth.IndexOf("Format") > -1 ? 6 : -1;
        //            int bitDepthInt;
        //            if (int.TryParse(_bitDepth.Substring(asfsd, _bitDepth.IndexOf("bpp") - 6), out bitDepthInt))
        //            {
        //                BitDepth = bitDepthInt;
        //            }*/
        //        }
        //        catch (Exception ex)
        //        {
        //            cGlobalSettings.oLogger.WriteLogException("csExifInfo:: BitDepth", ex);
        //        }

        //        #endregion

        //        #region --- get Created Date ---
        //        DateTime dtCreated = DateTime.MaxValue;
        //        try
        //        {
        //            dateCreated = cGlobalSettings.listImageFileInfo[key].createDate;// fileInfo.CreationTime;
        //        }
        //        catch
        //        {
        //            dateCreated = dtCreated; // Properties not contains information
        //        }
        //        #endregion

        //        #region --- get Modified Date ---
        //        DateTime dtModified = DateTime.MaxValue;
        //        try
        //        {
        //            dateModified = cGlobalSettings.listImageFileInfo[key].modDate;// fileInfo.LastWriteTime;
        //        }
        //        catch
        //        {
        //            dateCreated = dtCreated; // Properties not contains information
        //        }
        //        #endregion


        //        #region--- Get GPS Longitude and Latitude ---
        //        try
        //        {
        //            eFuncRetVal propertyStatus = eFuncRetVal.FAIL;

        //            latitudeDegree = 360;
        //            latitudeRadian = 360;
        //            longitudeDegree = 360;
        //            longitudeRadian = 360;

        //            string latiRef = string.Empty;
        //            var _latiRef = exifInfo.Where(x => x.Id == latitudeRef0001).SingleOrDefault();
        //            if (_latiRef != null)
        //            {
        //                propertyStatus = eFuncRetVal.SUCCESS;
        //                latiRef = Encoding.ASCII.GetString(_latiRef.Value).Replace("\0", "");
        //            }

        //            if (propertyStatus == eFuncRetVal.SUCCESS)
        //            {
        //                var lati = exifInfo.Where(x => x.Id == latitude0002).SingleOrDefault().Value;

        //                uint nLatiDegree = BitConverter.ToUInt32(new byte[] { lati[0], lati[1], lati[2], lati[3] }, 0);
        //                uint dLatiDegree = BitConverter.ToUInt32(new byte[] { lati[4], lati[5], lati[6], lati[7] }, 0);

        //                uint nLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[8], lati[9], lati[10], lati[11] }, 0);
        //                uint dLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[12], lati[13], lati[14], lati[15] }, 0);

        //                uint nLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[16], lati[17], lati[18], lati[19] }, 0);
        //                uint dLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[20], lati[21], lati[22], lati[23] }, 0);

        //                latitudeDegree = ConvertDegreeAngleToDouble(nLatiDegree / dLatiDegree, nLatiMinutes / dLatiMinutes, nLatiSeconds / dLatiSeconds, latiRef);
        //                if (Math.Abs(latitudeDegree) > 90)
        //                {
        //                    latitudeDegree = 360;
        //                    propertyStatus = eFuncRetVal.FAIL;
        //                    //return;
        //                }
        //                latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree);
        //                if (Math.Abs(latitudeRadian) > 90)
        //                {
        //                    latitudeDegree = 360;
        //                    propertyStatus = eFuncRetVal.FAIL;
        //                    //return;
        //                }


        //                if (propertyStatus == eFuncRetVal.SUCCESS)
        //                {

        //                    string longRef = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == longitudeRef0003).SingleOrDefault().Value).Replace("\0", "");
        //                    var longi = exifInfo.Where(x => x.Id == longitude0004).SingleOrDefault().Value;

        //                    uint nLongiDegree = BitConverter.ToUInt32(new byte[] { longi[0], longi[1], longi[2], longi[3] }, 0);
        //                    uint dLongiDegree = BitConverter.ToUInt32(new byte[] { longi[4], longi[5], longi[6], longi[7] }, 0);

        //                    uint nLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[8], longi[9], longi[10], longi[11] }, 0);
        //                    uint dLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[12], longi[13], longi[14], longi[15] }, 0);

        //                    uint nLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[16], longi[17], longi[18], longi[19] }, 0);
        //                    uint dLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[20], longi[21], longi[22], longi[23] }, 0);

        //                    longitudeDegree = ConvertDegreeAngleToDouble(nLongiDegree / dLongiDegree, nLongiMinutes / dLongiMinutes, nLongiSeconds / dLongiSeconds, longRef);
        //                    if (Math.Abs(latitudeRadian) > 180)
        //                    {
        //                        longitudeDegree = 360;
        //                        propertyStatus = eFuncRetVal.FAIL;
        //                    }
        //                    longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree);
        //                    if (Math.Abs(longitudeRadian) > 180)
        //                    {
        //                        longitudeDegree = 360;
        //                        propertyStatus = eFuncRetVal.FAIL;
        //                    }

        //                    if (propertyStatus == eFuncRetVal.SUCCESS)
        //                    {
        //                        LatitudeRef = latiRef;
        //                        LongitudeRef = longRef;

        //                        Latitiude = string.Format("{0}° {1}' {2}\" {3}", (float)nLatiDegree / dLatiDegree, (float)nLatiMinutes / dLatiMinutes, (float)nLatiSeconds / dLatiSeconds, latiRef.ToUpper());
        //                        PropertyCount++;
        //                        Longitude = string.Format("{0}° {1}' {2}\" {3}", (float)nLongiDegree / dLongiDegree, (float)nLongiMinutes / dLongiMinutes, (float)nLongiSeconds / dLongiSeconds, longRef.ToUpper());
        //                    }
        //                }
        //                PropertyCount++;
        //            }
        //        }
        //        catch
        //        {
        //            ; // Properties not contains information
        //        }
        //        #endregion


        //        if (!calcAllExif)
        //            return;

        //        #region ---- Rating ----
        //        /*if ( img.PropertyItems.Where(p => p.Id == 18246).Count()>0)
        //        {
        //            PropertyItem propRating = img.GetPropertyItem(18246);
        //            Rating = BitConverter.ToInt16(propRating.Value, 0).ToString();
        //        }*/

        //        var _rating = exifInfo.Where(x => x.Id == rating4746).SingleOrDefault();
        //        if (_rating != null)
        //        {
        //            Rating = _rating.Value[0];
        //            PropertyCount++;
        //        }

        //        #endregion

        //        #region--- keywords ----
        //        var _keywords = exifInfo.Where(x => x.Id == keywords4746).SingleOrDefault();
        //        if (_keywords != null)
        //        {
        //            Keywords = Encoding.ASCII.GetString(_keywords.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        #endregion

        //        // -------------------- Digital Zoom Ratio --------------------

        //        #region--- Editing Software ---
        //        var editSw = exifInfo.Where(x => x.Id == editingSoftware0131).SingleOrDefault();
        //        if (editSw != null)
        //        {
        //            EditingSoftware = Encoding.ASCII.GetString(editSw.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region--- Exposure ---
        //        var _exposure = exifInfo.Where(x => x.Id == exposure0112).SingleOrDefault();
        //        if (_exposure != null)
        //        {
        //            PropertyCount++;
        //            String pv0 = BitConverter.ToInt16(_exposure.Value, 0).ToString();
        //            String pv1 = BitConverter.ToInt16(_exposure.Value, 4).ToString();
        //            // Turn 10/2500 into 1/250
        //            if (pv0.EndsWith("0") && pv1.EndsWith("0"))
        //            {
        //                pv0 = pv0.Substring(0, pv0.Length - 1);
        //                pv1 = pv1.Substring(0, pv1.Length - 1);
        //            }
        //            //Turn 8/1 into 8
        //            if (pv1 == "1")
        //            { Exposure = pv0; }
        //            else { Exposure = pv0 + "/" + pv1; }
        //        }

        //        #endregion

        //        #region --- FocalLength ---

        //        var _focalLength = exifInfo.Where(x => x.Id == focalLength920A).SingleOrDefault();
        //        if (_focalLength != null)
        //        {
        //            FocalLength = BitConverter.ToInt16(_focalLength.Value, 0).ToString();
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region --- Exposure Bias ---
        //        var _exposureBias = exifInfo.Where(x => x.Id == ExposureBias9204).SingleOrDefault();

        //        if (_exposureBias != null)
        //        {
        //            float pvf = (float)(BitConverter.ToInt16(_exposureBias.Value, 0)) / 2;
        //            ExposureBias = pvf.ToString();
        //            if (!ExposureBias.StartsWith("-")) ExposureBias = "+" + ExposureBias;
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region --- Exposure Mode ---

        //        var _ExposureMode = exifInfo.Where(x => x.Id == ExposureMode9204).SingleOrDefault();
        //        if (_ExposureMode != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_ExposureMode.Value, 0).ToString();
        //            if (pv0 == "0") ExposureMode = "Auto Exposure";
        //            if (pv0 == "1") ExposureMode = "Manual Exposure";
        //            if (pv0 == "2") ExposureMode = "Auto Bracket";
        //            if (pv0 == "3") ExposureMode = "Reserved";
        //            ExposureMode = pv0;
        //            PropertyCount++;
        //        }
        //        #endregion


        //        #region --- Exposure Mode ---
        //        var _ExposureProg = exifInfo.Where(x => x.Id == ExposureProg9204).SingleOrDefault();
        //        if (_ExposureProg != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_ExposureProg.Value, 0).ToString();
        //            if (pv0 == "0") ExposureProgram = "Not Defined";
        //            if (pv0 == "1") ExposureProgram = "Manual";
        //            if (pv0 == "2") ExposureProgram = "Program";
        //            if (pv0 == "3") ExposureProgram = "Aperture Priority";
        //            if (pv0 == "4") ExposureProgram = "Shutter Priority";
        //            if (pv0 == "5") ExposureProgram = "Creative Program";
        //            if (pv0 == "6") ExposureProgram = "Action Program";
        //            if (pv0 == "7") ExposureProgram = "Portrait Mode";
        //            if (pv0 == "8") ExposureProgram = "Landscape Mode";
        //            ExposureProgram = pv0;
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region --- Metering Mode ---
        //        var _MeteringMode = exifInfo.Where(x => x.Id == meteringMode9204).SingleOrDefault();

        //        if (_MeteringMode != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_MeteringMode.Value, 0).ToString();
        //            if (pv0 == "1") MeteringMode = "Average Metering";
        //            if (pv0 == "2") MeteringMode = "Center Weighted Average Metering";
        //            if (pv0 == "3") MeteringMode = "Spot Metering";
        //            if (pv0 == "4") MeteringMode = "MultiSpot Metering";
        //            if (pv0 == "5") MeteringMode = "Matrix Metering";
        //            if (pv0 == "6") MeteringMode = "Partial Metering";

        //            MeteringMode = "Unknown";
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region Light Source

        //        var _LightSource = exifInfo.Where(x => x.Id == lightSource9209).SingleOrDefault();
        //        if (_LightSource != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_LightSource.Value, 0).ToString();
        //            if (pv0 == "0") LightSource = "No Flash";
        //            if (pv0 == "1") LightSource = "Flash";
        //            PropertyCount++;
        //        }
        //        #endregion


        //        #region Sensing Method

        //        var _sensingMethod = exifInfo.Where(x => x.Id == sensingMethod9209).SingleOrDefault();
        //        if (_sensingMethod != null)
        //        {
        //            string pv0 = BitConverter.ToInt16(_sensingMethod.Value, 0).ToString();

        //            if (pv0 == "1") SensingMethod = "not defined";
        //            if (pv0 == "2") SensingMethod = "one-chip color area sensor";
        //            if (pv0 == "3") SensingMethod = "two-chip color area sensor";
        //            if (pv0 == "4") SensingMethod = "three-chip color area sensor";
        //            if (pv0 == "5") SensingMethod = "color sequential area sensor";
        //            if (pv0 == "7") SensingMethod = "tri-linear sensor";
        //            if (pv0 == "8") SensingMethod = "color sequential linear sensor";
        //            else SensingMethod = "reserved";
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region Scence Capture Method

        //        var _scenceCaptureMethod = exifInfo.Where(x => x.Id == scenceCaptureMethodA406).SingleOrDefault();
        //        if (_scenceCaptureMethod != null)
        //        {
        //            string pv0 = BitConverter.ToInt16(_scenceCaptureMethod.Value, 0).ToString();

        //            if (pv0 == "0") ScenceCaptureMethod = "Standard";
        //            if (pv0 == "1") ScenceCaptureMethod = "Landscape";
        //            if (pv0 == "2") ScenceCaptureMethod = "Portrait";
        //            if (pv0 == "3") ScenceCaptureMethod = "Night scene";
        //            else ScenceCaptureMethod = "Standard";
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region Camera Maker
        //        var _cameraMaker = exifInfo.Where(x => x.Id == cameraMaker010F).SingleOrDefault();
        //        if (_cameraMaker != null)
        //        {
        //            CameraMaker = Encoding.ASCII.GetString(_cameraMaker.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }

        //        #endregion

        //        #region Camera Model
        //        var _cameraModel = exifInfo.Where(x => x.Id == cameraModel0110).SingleOrDefault();

        //        if (_cameraModel != null)
        //        {
        //            CameraModel = Encoding.ASCII.GetString(_cameraModel.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }

        //        #endregion

        //        #region ISO Speed Rating
        //        var _ISOSpeedRating = exifInfo.Where(x => x.Id == ISOSpeedRating8827).SingleOrDefault();
        //        if (_ISOSpeedRating != null)
        //        {
        //            ISOSpeedRating = BitConverter.ToInt16(_ISOSpeedRating.Value, 0).ToString();
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region  Camera Firmware
        //        var _cameraFirmware = exifInfo.Where(x => x.Id == 305).SingleOrDefault();
        //        if (_cameraFirmware != null)
        //        {
        //            CameraFirmware = Encoding.ASCII.GetString(_cameraFirmware.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region --- Color Space / Color Model ---
        //        var _colorSpace = exifInfo.Where(x => x.Id == colorSpaceA001).FirstOrDefault();
        //        if (_colorSpace != null)
        //        {

        //            ColorSpace = BitConverter.ToInt16(_colorSpace.Value, 0).ToString();
        //            if (ColorSpace == "1")
        //                ColorModel = "RGB";

        //            else
        //                ColorModel = "other";
        //            PropertyCount++;
        //        }
        //        #endregion

        //        #region --- Profile Name ---
        //        var _profileName = exifInfo.Where(x => x.Id == 34675).FirstOrDefault();
        //        if (_profileName != null)
        //        {
        //            if (_profileName.Value != null)
        //            {
        //                ProfileName = Encoding.ASCII.GetString(_profileName.Value).Replace("\0", "");
        //                PropertyCount++;
        //            }
        //        }
        //        #endregion

        //        #region Altitude
        //        var _altitude = exifInfo.Where(x => x.Id == altitude0006).SingleOrDefault();
        //        if (_altitude != null)
        //        {
        //            var alt = _altitude.Value;
        //            byte[] num = { (byte)alt[0], (byte)alt[1], (byte)alt[2], (byte)alt[3] };

        //            byte[] den = { (byte)alt[4], (byte)alt[5], (byte)alt[6], (byte)alt[7] };


        //            uint numo = BitConverter.ToUInt32(num, 0);

        //            uint deno = BitConverter.ToUInt32(den, 0);

        //            Altitude = ((float)numo / deno).ToString("0.00");
        //            PropertyCount++;
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("csExifInfo :: ", ex);
        //    }
        //}

        //public csExifInfo(int key, Google.Apis.Drive.v3.Data.File.ImageMediaMetadataData img, bool calcAllExif)
        //{
        //    try
        //    {
        //        if (img == null)
        //            return;


        //        /*img = image;*/
        //        PropertyCount = 0;
        //        DateTime temp;
        //        //PropertyItem[] exifInfo = img.PropertyItems;

        //        /*if(cGlobalSettings.listImageFileInfo.ContainsKey(key))
        //        {
        //            exifHorizontalResolution();

        //            exifCaptureDate();

        //            exifImageDimension();

                    
        //        }*/

        //        FileExifId = key;


        //        #region --- Horizontal Resolution ---

        //        HorizontalResolution = 0; // HorizontalResolution;

        //        #endregion

        //        #region --- Vertical Resolution ---

        //        VerticalResolution = 0;
        //        #endregion

        //        #region --- CaptureDate ---
        //        CaptureDate = DateTime.MaxValue;

        //        if (DateTime.TryParseExact(img.Time, new string[] { "yyyy:MM:dd HH:mm:ss", "yyyy:MM:DD H:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
        //        {
        //            CaptureDate = temp;
        //        }
        //        else if (DateTime.TryParse(img.Time, out temp))
        //        {
        //            CaptureDate = temp;
        //        }

        //        #endregion

        //        #region --- Image Dimension N FileSize ----

        //        ImageDimension = new Size(img.Width.HasValue ? img.Width.Value : 0, img.Height.HasValue ? img.Height.Value : 0);
        //        PropertyCount++;
        //        //FileSize = (img.Width.HasValue ? img.Width.Value : 0) * (img.Height.HasValue ? img.Height.Value : 0);

        //        #endregion

        //        #region--- Orientation ---
        //        Orientation = Convert.ToString(img.Rotation.HasValue ? img.Rotation.Value : 0); // exifInfo.Where(x => x.Id == orientation0112).SingleOrDefault();

        //        PropertyCount++;
        //        #endregion

        //        #region --- Bit Depth ----
        //        //bitDepth5015
        //        try
        //        {
        //            ///https://msdn.microsoft.com/en-us/library/system.drawing.imaging.pixelformat.aspx
        //            ///
        //            ///http://stackoverflow.com/questions/1126967/how-to-get-bitsperpixel-from-a-bitmap

        //            BitDepth = 0;

        //            /*string _bitDepth = img.PixelFormat.ToString();
        //            int asfsd = _bitDepth.IndexOf("Format") > -1 ? 6 : -1;
        //            int bitDepthInt;
        //            if (int.TryParse(_bitDepth.Substring(asfsd, _bitDepth.IndexOf("bpp") - 6), out bitDepthInt))
        //            {
        //                BitDepth = bitDepthInt;
        //            }*/
        //        }
        //        catch (Exception ex)
        //        {
        //            cGlobalSettings.oLogger.WriteLogException("csExifInfo:: BitDepth", ex);
        //        }

        //        #endregion

        //        #region --- get Created Date ---
        //        DateTime dtCreated = DateTime.MaxValue;
        //        try
        //        {
        //            dateCreated = cGlobalSettings.listImageFileInfo[key].createDate;
        //        }
        //        catch
        //        {
        //            dateCreated = dtCreated; // Properties not contains information
        //        }
        //        #endregion

        //        #region --- get Modified Date ---
        //        DateTime dtModified = DateTime.MaxValue;
        //        try
        //        {
        //            dateModified = cGlobalSettings.listImageFileInfo[key].modDate;
        //        }
        //        catch
        //        {
        //            dateCreated = dtCreated; // Properties not contains information
        //        }
        //        #endregion


        //        #region--- Get GPS Longitude and Latitude ---
        //        try
        //        {

        //            latitudeDegree = 360;
        //            latitudeRadian = 360;
        //            longitudeDegree = 360;
        //            longitudeRadian = 360;

        //            if (img.Location != null)
        //            {
        //                Latitiude = Convert.ToString(img.Location.Latitude.HasValue ? img.Location.Latitude.Value : 0);
        //                latitudeDegree = img.Location.Latitude.HasValue ? img.Location.Latitude.Value : 0;
        //                latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree);

        //                //Altitude = Convert.ToString(img.Location.Altitude.HasValue ? img.Location.Altitude.Value : 0);

        //                Longitude = Convert.ToString(img.Location.Longitude.HasValue ? img.Location.Longitude.Value : 0);
        //                longitudeDegree = img.Location.Longitude.HasValue ? img.Location.Longitude.Value : 0;
        //                longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree);
        //                PropertyCount++;
        //            }
        //            /*
                    
        //            eFuncRetVal propertyStatus = eFuncRetVal.FAIL;

        //            latitudeDegree = 360;
        //            latitudeRadian = 360;
        //            longitudeDegree = 360;
        //            longitudeRadian = 360;
        //            string latiRef = string.Empty;
        //            var _latiRef = exifInfo.Where(x => x.Id == latitudeRef0001).SingleOrDefault();
        //            if (_latiRef != null)
        //            {
        //                propertyStatus = eFuncRetVal.SUCCESS;
        //                latiRef = Encoding.ASCII.GetString(_latiRef.Value).Replace("\0", "");
        //            }

        //            if (propertyStatus == eFuncRetVal.SUCCESS)
        //            {
        //                var lati = exifInfo.Where(x => x.Id == latitude0002).SingleOrDefault().Value;

        //                uint nLatiDegree = BitConverter.ToUInt32(new byte[] { lati[0], lati[1], lati[2], lati[3] }, 0);
        //                uint dLatiDegree = BitConverter.ToUInt32(new byte[] { lati[4], lati[5], lati[6], lati[7] }, 0);

        //                uint nLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[8], lati[9], lati[10], lati[11] }, 0);
        //                uint dLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[12], lati[13], lati[14], lati[15] }, 0);

        //                uint nLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[16], lati[17], lati[18], lati[19] }, 0);
        //                uint dLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[20], lati[21], lati[22], lati[23] }, 0);

        //                latitudeDegree = ConvertDegreeAngleToDouble(nLatiDegree / dLatiDegree, nLatiMinutes / dLatiMinutes, nLatiSeconds / dLatiSeconds, latiRef);
        //                if (Math.Abs(latitudeDegree) > 90)
        //                {
        //                    latitudeDegree = 360;
        //                    propertyStatus = eFuncRetVal.FAIL;
        //                    //return;
        //                }
        //                latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree);
        //                if (Math.Abs(latitudeRadian) > 90)
        //                {
        //                    latitudeDegree = 360;
        //                    propertyStatus = eFuncRetVal.FAIL;
        //                    //return;
        //                }


        //                if (propertyStatus == eFuncRetVal.SUCCESS)
        //                {

        //                    string longRef = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == longitudeRef0003).SingleOrDefault().Value).Replace("\0", "");
        //                    var longi = exifInfo.Where(x => x.Id == longitude0004).SingleOrDefault().Value;

        //                    uint nLongiDegree = BitConverter.ToUInt32(new byte[] { longi[0], longi[1], longi[2], longi[3] }, 0);
        //                    uint dLongiDegree = BitConverter.ToUInt32(new byte[] { longi[4], longi[5], longi[6], longi[7] }, 0);

        //                    uint nLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[8], longi[9], longi[10], longi[11] }, 0);
        //                    uint dLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[12], longi[13], longi[14], longi[15] }, 0);

        //                    uint nLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[16], longi[17], longi[18], longi[19] }, 0);
        //                    uint dLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[20], longi[21], longi[22], longi[23] }, 0);

        //                    longitudeDegree = ConvertDegreeAngleToDouble(nLongiDegree / dLongiDegree, nLongiMinutes / dLongiMinutes, nLongiSeconds / dLongiSeconds, longRef);
        //                    if (Math.Abs(latitudeRadian) > 180)
        //                    {
        //                        longitudeDegree = 360;
        //                        propertyStatus = eFuncRetVal.FAIL;
        //                    }
        //                    longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree);
        //                    if (Math.Abs(longitudeRadian) > 180)
        //                    {
        //                        longitudeDegree = 360;
        //                        propertyStatus = eFuncRetVal.FAIL;
        //                    }

        //                    if (propertyStatus == eFuncRetVal.SUCCESS)
        //                    {
        //                        LatitudeRef = latiRef;
        //                        LongitudeRef = longRef;

        //                        Latitiude = string.Format("{0}° {1}' {2}\" {3}", (float)nLatiDegree / dLatiDegree, (float)nLatiMinutes / dLatiMinutes, (float)nLatiSeconds / dLatiSeconds, latiRef.ToUpper());
        //                        PropertyCount++;
        //                        Longitude = string.Format("{0}° {1}' {2}\" {3}", (float)nLongiDegree / dLongiDegree, (float)nLongiMinutes / dLongiMinutes, (float)nLongiSeconds / dLongiSeconds, longRef.ToUpper());
        //                    }
        //                }
        //                PropertyCount++;
        //            }
        //            */
        //        }
        //        catch
        //        {
        //            ; // Properties not contains information
        //        }
        //        #endregion


        //        if (!calcAllExif)
        //            return;

        //        #region ---- Rating ----
        //        /*
        //        var _rating = exifInfo.Where(x => x.Id == rating4746).SingleOrDefault();
        //        if (_rating != null)
        //        {
        //            Rating = _rating.Value[0];
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region--- keywords ----

        //        /*var _keywords = exifInfo.Where(x => x.Id == keywords4746).SingleOrDefault();
        //        if (_keywords != null)
        //        {
        //            Keywords = Encoding.ASCII.GetString(_keywords.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        // -------------------- Digital Zoom Ratio --------------------

        //        #region--- Editing Software ---
        //        /*
        //        var editSw = exifInfo.Where(x => x.Id == editingSoftware0131).SingleOrDefault();
        //        if (editSw != null)
        //        {
        //            EditingSoftware = Encoding.ASCII.GetString(editSw.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region--- Exposure ---
        //        if (img.ExposureTime.HasValue)
        //        {
        //            Exposure = Convert.ToString(img.ExposureTime.Value);
        //            PropertyCount++;
        //        }
        //        /*
        //        var _exposure = exifInfo.Where(x => x.Id == exposure0112).SingleOrDefault();
        //        if (_exposure != null)
        //        {
        //            PropertyCount++;
        //            String pv0 = BitConverter.ToInt16(_exposure.Value, 0).ToString();
        //            String pv1 = BitConverter.ToInt16(_exposure.Value, 4).ToString();
        //            // Turn 10/2500 into 1/250
        //            if (pv0.EndsWith("0") && pv1.EndsWith("0"))
        //            {
        //                pv0 = pv0.Substring(0, pv0.Length - 1);
        //                pv1 = pv1.Substring(0, pv1.Length - 1);
        //            }
        //            //Turn 8/1 into 8
        //            if (pv1 == "1")
        //            { Exposure = pv0; }
        //            else { Exposure = pv0 + "/" + pv1; }
        //        }
        //        */
        //        #endregion

        //        #region --- FocalLength ---
        //        if (img.FocalLength.HasValue)
        //        {
        //            FocalLength = Convert.ToString(img.FocalLength.Value);
        //            PropertyCount++;
        //        }

        //        /*
        //        var _focalLength = exifInfo.Where(x => x.Id == focalLength920A).SingleOrDefault();
        //        if (_focalLength != null)
        //        {
        //            FocalLength = BitConverter.ToInt16(_focalLength.Value, 0).ToString();
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region --- Exposure Bias ---

        //        if (img.ExposureBias.HasValue)
        //        {
        //            ExposureBias = Convert.ToString(img.ExposureBias.Value);
        //            PropertyCount++;
        //        }
        //        /*
        //        var _exposureBias = exifInfo.Where(x => x.Id == ExposureBias9204).SingleOrDefault();

        //        if (_exposureBias != null)
        //        {
        //            float pvf = (float)(BitConverter.ToInt16(_exposureBias.Value, 0)) / 2;
        //            ExposureBias = pvf.ToString();
        //            if (!ExposureBias.StartsWith("-")) ExposureBias = "+" + ExposureBias;
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region --- Exposure Mode ---
        //        if (!string.IsNullOrEmpty(img.ExposureMode))
        //        {
        //            ExposureMode = img.ExposureMode;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _ExposureMode = exifInfo.Where(x => x.Id == ExposureMode9204).SingleOrDefault();
        //        if (_ExposureMode != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_ExposureMode.Value, 0).ToString();
        //            if (pv0 == "0") ExposureMode = "Auto Exposure";
        //            if (pv0 == "1") ExposureMode = "Manual Exposure";
        //            if (pv0 == "2") ExposureMode = "Auto Bracket";
        //            if (pv0 == "3") ExposureMode = "Reserved";
        //            ExposureMode = pv0;
        //            PropertyCount++;
        //        }*/
        //        #endregion


        //        #region --- Exposure Mode ---
        //        /*
        //        var _ExposureProg = exifInfo.Where(x => x.Id == ExposureProg9204).SingleOrDefault();
        //        if (_ExposureProg != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_ExposureProg.Value, 0).ToString();
        //            if (pv0 == "0") ExposureProgram = "Not Defined";
        //            if (pv0 == "1") ExposureProgram = "Manual";
        //            if (pv0 == "2") ExposureProgram = "Program";
        //            if (pv0 == "3") ExposureProgram = "Aperture Priority";
        //            if (pv0 == "4") ExposureProgram = "Shutter Priority";
        //            if (pv0 == "5") ExposureProgram = "Creative Program";
        //            if (pv0 == "6") ExposureProgram = "Action Program";
        //            if (pv0 == "7") ExposureProgram = "Portrait Mode";
        //            if (pv0 == "8") ExposureProgram = "Landscape Mode";
        //            ExposureProgram = pv0;
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region --- Metering Mode ---
        //        if (string.IsNullOrEmpty(img.MeteringMode))
        //        {
        //            MeteringMode = img.MeteringMode;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _MeteringMode = exifInfo.Where(x => x.Id == meteringMode9204).SingleOrDefault();

        //        if (_MeteringMode != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_MeteringMode.Value, 0).ToString();
        //            if (pv0 == "1") MeteringMode = "Average Metering";
        //            if (pv0 == "2") MeteringMode = "Center Weighted Average Metering";
        //            if (pv0 == "3") MeteringMode = "Spot Metering";
        //            if (pv0 == "4") MeteringMode = "MultiSpot Metering";
        //            if (pv0 == "5") MeteringMode = "Matrix Metering";
        //            if (pv0 == "6") MeteringMode = "Partial Metering";

        //            MeteringMode = "Unknown";
        //            PropertyCount++;
        //        }*/
        //        #endregion

        //        #region Light Source
        //        if (img.FlashUsed.HasValue)
        //        {
        //            if (img.FlashUsed.Value)
        //            {
        //                LightSource = "Flash";
        //            }
        //            else
        //            {
        //                LightSource = "No Flash";
        //            }
        //            PropertyCount++;
        //        }
        //        /*
        //        var _LightSource = exifInfo.Where(x => x.Id == lightSource9209).SingleOrDefault();
        //        if (_LightSource != null)
        //        {
        //            String pv0 = BitConverter.ToInt16(_LightSource.Value, 0).ToString();
        //            if (pv0 == "0") LightSource = "No Flash";
        //            if (pv0 == "1") LightSource = "Flash";
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion


        //        #region Sensing Method
        //        if (!string.IsNullOrEmpty(img.Sensor))
        //        {
        //            SensingMethod = img.Sensor;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _sensingMethod = exifInfo.Where(x => x.Id == sensingMethod9209).SingleOrDefault();
        //        if (_sensingMethod != null)
        //        {
        //            string pv0 = BitConverter.ToInt16(_sensingMethod.Value, 0).ToString();

        //            if (pv0 == "1") SensingMethod = "not defined";
        //            if (pv0 == "2") SensingMethod = "one-chip color area sensor";
        //            if (pv0 == "3") SensingMethod = "two-chip color area sensor";
        //            if (pv0 == "4") SensingMethod = "three-chip color area sensor";
        //            if (pv0 == "5") SensingMethod = "color sequential area sensor";
        //            if (pv0 == "7") SensingMethod = "tri-linear sensor";
        //            if (pv0 == "8") SensingMethod = "color sequential linear sensor";
        //            else SensingMethod = "reserved";
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region Scence Capture Method
        //        /*
        //        var _scenceCaptureMethod = exifInfo.Where(x => x.Id == scenceCaptureMethodA406).SingleOrDefault();
        //        if (_scenceCaptureMethod != null)
        //        {
        //            string pv0 = BitConverter.ToInt16(_scenceCaptureMethod.Value, 0).ToString();

        //            if (pv0 == "0") ScenceCaptureMethod = "Standard";
        //            if (pv0 == "1") ScenceCaptureMethod = "Landscape";
        //            if (pv0 == "2") ScenceCaptureMethod = "Portrait";
        //            if (pv0 == "3") ScenceCaptureMethod = "Night scene";
        //            else ScenceCaptureMethod = "Standard";
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region Camera Maker
        //        if (!string.IsNullOrEmpty(img.CameraMake))
        //        {
        //            CameraMaker = img.CameraMake;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _cameraMaker = exifInfo.Where(x => x.Id == cameraMaker010F).SingleOrDefault();
        //        if (_cameraMaker != null)
        //        {
        //            CameraMaker = Encoding.ASCII.GetString(_cameraMaker.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region Camera Model

        //        if (!string.IsNullOrEmpty(img.CameraModel))
        //        {
        //            CameraModel = img.CameraModel;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _cameraModel = exifInfo.Where(x => x.Id == cameraModel0110).SingleOrDefault();

        //        if (_cameraModel != null)
        //        {
        //            CameraModel = Encoding.ASCII.GetString(_cameraModel.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region ISO Speed Rating
        //        if (img.IsoSpeed.HasValue)
        //        {
        //            ISOSpeedRating = Convert.ToString(img.IsoSpeed.Value);
        //            PropertyCount++;
        //        }
        //        /*
        //        var _ISOSpeedRating = exifInfo.Where(x => x.Id == ISOSpeedRating8827).SingleOrDefault();
        //        if (_ISOSpeedRating != null)
        //        {
        //            ISOSpeedRating = BitConverter.ToInt16(_ISOSpeedRating.Value, 0).ToString();
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region  Camera Firmware
        //        /*
        //        var _cameraFirmware = exifInfo.Where(x => x.Id == 305).SingleOrDefault();
        //        if (_cameraFirmware != null)
        //        {
        //            CameraFirmware = Encoding.ASCII.GetString(_cameraFirmware.Value).Replace("\0", "");
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region --- Color Space / Color Model ---
        //        if (!string.IsNullOrEmpty(img.ColorSpace))
        //        {
        //            ColorSpace = img.ColorSpace;
        //            PropertyCount++;
        //        }
        //        /*
        //        var _colorSpace = exifInfo.Where(x => x.Id == colorSpaceA001).FirstOrDefault();
        //        if (_colorSpace != null)
        //        {

        //            ColorSpace = BitConverter.ToInt16(_colorSpace.Value, 0).ToString();
        //            if (ColorSpace == "1")
        //                ColorModel = "RGB";

        //            else
        //                ColorModel = "other";
        //            PropertyCount++;
        //        }
        //        */
        //        #endregion

        //        #region --- Profile Name ---
        //        /*
        //        var _profileName = exifInfo.Where(x => x.Id == 34675).FirstOrDefault();
        //        if (_profileName != null)
        //        {
        //            if (_profileName.Value != null)
        //            {
        //                ProfileName = Encoding.ASCII.GetString(_profileName.Value).Replace("\0", "");
        //                PropertyCount++;
        //            }
        //        }
        //        */
        //        #endregion

        //        #region Altitude

        //        if (img.Location != null && img.Location.Altitude.HasValue)
        //        {
        //            Altitude = Convert.ToString(img.Location.Altitude.Value);
        //            PropertyCount++;
        //        }

        //        /*var _altitude = exifInfo.Where(x => x.Id == altitude0006).SingleOrDefault();
        //        if (_altitude != null)
        //        {
        //            var alt = _altitude.Value;
        //            byte[] num = { (byte)alt[0], (byte)alt[1], (byte)alt[2], (byte)alt[3] };

        //            byte[] den = { (byte)alt[4], (byte)alt[5], (byte)alt[6], (byte)alt[7] };


        //            uint numo = BitConverter.ToUInt32(num, 0);

        //            uint deno = BitConverter.ToUInt32(den, 0);

        //            Altitude = ((float)numo / deno).ToString("0.00");
        //            PropertyCount++;
        //        }*/
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("csExifInfo :: ", ex);
        //    }
        //}

        /*

                public void exifHorizontalResolution()
                {
                    #region --- Horizontal/Vertical Resolution ---
                    HorizontalResolution = img.HorizontalResolution;
                    PropertyCount++;
                    VerticalResolution = img.VerticalResolution;
                    PropertyCount++;
                    #endregion
                }

                public void exifCaptureDate()
                {
                    #region --- CaptureDate ---
                    DateTime temp;
                    CaptureDate = DateTime.MaxValue;
                    if (img.PropertyIdList.Any(x => x == 36867))
                    {
                        PropertyItem propItem = img.GetPropertyItem(36867);
                        string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        bool success = DateTime.TryParse(dateTaken, out temp);
                        if (success)
                        {
                            CaptureDate = temp;
                            PropertyCount++;
                        }
                    }
                    #endregion
                }

                public void exifImageDimension()
                {
                    #region --- Image Size ----
                    ImageDimension = img.Size;
                    PropertyCount++;
                    FileSize = img.Width * img.Height;
                    #endregion
                }

                public void exifRating()
                {
                    #region ---- Rating ----
                    / *if ( img.PropertyItems.Where(p => p.Id == 18246).Count()>0)
                        {
                            PropertyItem propRating = img.GetPropertyItem(18246);
                            Rating = BitConverter.ToInt16(propRating.Value, 0).ToString();
                        }* /

                    var _rating = exifInfo.Where(x => x.Id == rating4746).SingleOrDefault();
                    if (_rating != null)
                    {
                        Rating = _rating.Value[0];
                        PropertyCount++;
                    }

                    #endregion
                }


                public void exifKeywords()
                {
                    #region--- keywords ----
                    var _keywords = exifInfo.Where(x => x.Id == keywords4746).SingleOrDefault();
                    if (_keywords != null)
                    {
                        Keywords = Encoding.ASCII.GetString(_keywords.Value).Replace("\0", "");
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifOrientation()
                {
                    #region--- Orientation ---
                    var _orientation = exifInfo.Where(x => x.Id == orientation0112).SingleOrDefault();
                    if (_orientation != null)
                    {
                        Orientation = _orientation.Value[0] + "";
                        PropertyCount++;
                        switch (Orientation)
                        {
                            case "1": // landscape, do nothing
                                Orientation = Orientation + " (Normal)";
                                break;

                            case "8": // rotated 90 right
                                // de-rotate:
                                Orientation = Orientation + " (Rotate 270°)";
                                break;

                            case "3": // bottoms up
                                Orientation = Orientation + " (Rotate 180°)";
                                break;

                            case "6": // rotated 90 left
                                Orientation = Orientation + " (Rotate 90°)";
                                break;
                        }
                    }
                    #endregion
                }

                // -------------------- Digital Zoom Ratio --------------------

                public void exifdateCreated()
                {
                    #region --- get Capture time ---
                    DateTime dtCreated = DateTime.MaxValue;
                    dateCreated = dtCreated;
                    try
                    {
                        var captureDate = exifInfo.Where(x => x.Id == CaptureDate9003).SingleOrDefault();

                        if (captureDate != null)
                        {
                            string dt = Encoding.ASCII.GetString(captureDate.Value);

                            if (DateTime.TryParseExact(dt.Replace("\0", ""), "yyyy:MM:dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dtCreated))
                            {
                                dateCreated = dtCreated;
                                PropertyCount++;
                            }
                        }
                    }
                    catch
                    {
                        ; // Properties not contains information
                    }
                    #endregion
                }

                public void exifEditingSoftware()
                {


                    #region--- Editing Software ---
                    var editSw = exifInfo.Where(x => x.Id == editingSoftware0131).SingleOrDefault();
                    if (editSw != null)
                    {
                        EditingSoftware = Encoding.ASCII.GetString(editSw.Value).Replace("\0", "");
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifExposure()
                {



                    #region--- Exposure ---
                    var _exposure = exifInfo.Where(x => x.Id == exposure0112).SingleOrDefault();
                    if (_exposure != null)
                    {
                        PropertyCount++;
                        String pv0 = BitConverter.ToInt16(_exposure.Value, 0).ToString();
                        String pv1 = BitConverter.ToInt16(_exposure.Value, 4).ToString();
                        // Turn 10/2500 into 1/250
                        if (pv0.EndsWith("0") && pv1.EndsWith("0"))
                        {
                            pv0 = pv0.Substring(0, pv0.Length - 1);
                            pv1 = pv1.Substring(0, pv1.Length - 1);
                        }
                        //Turn 8/1 into 8
                        if (pv1 == "1")
                        { Exposure = pv0; }
                        else { Exposure = pv0 + "/" + pv1; }
                    }

                    #endregion
                }

                public void exifFocalLength()
                {



                    #region --- FocalLength ---

                    var _focalLength = exifInfo.Where(x => x.Id == focalLength920A).SingleOrDefault();
                    if (_focalLength != null)
                    {
                        FocalLength = BitConverter.ToInt16(_focalLength.Value, 0).ToString();
                        PropertyCount++;
                    }
                    #endregion
                }


                public void exifExposureBias()
                {
        s

                    #region --- Exposure Bias ---
                    var _exposureBias = exifInfo.Where(x => x.Id == ExposureBias9204).SingleOrDefault();

                    if (_exposureBias != null)
                    {
                        float pvf = (float)(BitConverter.ToInt16(_exposureBias.Value, 0)) / 2;
                        ExposureBias = pvf.ToString();
                        if (!ExposureBias.StartsWith("-")) ExposureBias = "+" + ExposureBias;
                        PropertyCount++;
                    }
                    #endregion
                }


                public void exifExposureMode()
                {


                    #region --- Exposure Mode ---

                    var _ExposureMode = exifInfo.Where(x => x.Id == ExposureMode9204).SingleOrDefault();
                    if (_ExposureMode != null)
                    {
                        String pv0 = BitConverter.ToInt16(_ExposureMode.Value, 0).ToString();
                        if (pv0 == "0") ExposureMode = "Auto Exposure";
                        if (pv0 == "1") ExposureMode = "Manual Exposure";
                        if (pv0 == "2") ExposureMode = "Auto Bracket";
                        if (pv0 == "3") ExposureMode = "Reserved";
                        ExposureMode = pv0;
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifExposureProgram()
                {
                    #region --- Exposure Mode ---
                    var _ExposureProg = exifInfo.Where(x => x.Id == ExposureProg9204).SingleOrDefault();
                    if (_ExposureProg != null)
                    {
                        String pv0 = BitConverter.ToInt16(_ExposureProg.Value, 0).ToString();
                        if (pv0 == "0") ExposureProgram = "Not Defined";
                        if (pv0 == "1") ExposureProgram = "Manual";
                        if (pv0 == "2") ExposureProgram = "Program";
                        if (pv0 == "3") ExposureProgram = "Aperture Priority";
                        if (pv0 == "4") ExposureProgram = "Shutter Priority";
                        if (pv0 == "5") ExposureProgram = "Creative Program";
                        if (pv0 == "6") ExposureProgram = "Action Program";
                        if (pv0 == "7") ExposureProgram = "Portrait Mode";
                        if (pv0 == "8") ExposureProgram = "Landscape Mode";
                        ExposureProgram = pv0;
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifMeteringMode()
                {
                    #region --- Metering Mode ---
                    var _MeteringMode = exifInfo.Where(x => x.Id == meteringMode9204).SingleOrDefault();

                    if (_MeteringMode != null)
                    {
                        String pv0 = BitConverter.ToInt16(_MeteringMode.Value, 0).ToString();
                        if (pv0 == "1") MeteringMode = "Average Metering";
                        if (pv0 == "2") MeteringMode = "Center Weighted Average Metering";
                        if (pv0 == "3") MeteringMode = "Spot Metering";
                        if (pv0 == "4") MeteringMode = "MultiSpot Metering";
                        if (pv0 == "5") MeteringMode = "Matrix Metering";
                        if (pv0 == "6") MeteringMode = "Partial Metering";

                        MeteringMode = "Unknown";
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifLightSource()
                {
                    #region Light Source

                    var _LightSource = exifInfo.Where(x => x.Id == lightSource9209).SingleOrDefault();
                    if (_LightSource != null)
                    {
                        String pv0 = BitConverter.ToInt16(_LightSource.Value, 0).ToString();
                        if (pv0 == "0") LightSource = "No Flash";
                        if (pv0 == "1") LightSource = "Flash";
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifSensingMethod()
                {
                    #region Sensing Method

                    var _sensingMethod = exifInfo.Where(x => x.Id == sensingMethod9209).SingleOrDefault();
                    if (_sensingMethod != null)
                    {
                        string pv0 = BitConverter.ToInt16(_sensingMethod.Value, 0).ToString();

                        if (pv0 == "1") SensingMethod = "not defined";
                        if (pv0 == "2") SensingMethod = "one-chip color area sensor";
                        if (pv0 == "3") SensingMethod = "two-chip color area sensor";
                        if (pv0 == "4") SensingMethod = "three-chip color area sensor";
                        if (pv0 == "5") SensingMethod = "color sequential area sensor";
                        if (pv0 == "7") SensingMethod = "tri-linear sensor";
                        if (pv0 == "8") SensingMethod = "color sequential linear sensor";
                        else SensingMethod = "reserved";
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifScenceCaptureMethod()
                {
                    #region Scence Capture Method

                    var _scenceCaptureMethod = exifInfo.Where(x => x.Id == scenceCaptureMethodA406).SingleOrDefault();
                    if (_scenceCaptureMethod != null)
                    {
                        string pv0 = BitConverter.ToInt16(_scenceCaptureMethod.Value, 0).ToString();

                        if (pv0 == "0") ScenceCaptureMethod = "Standard";
                        if (pv0 == "1") ScenceCaptureMethod = "Landscape";
                        if (pv0 == "2") ScenceCaptureMethod = "Portrait";
                        if (pv0 == "3") ScenceCaptureMethod = "Night scene";
                        else ScenceCaptureMethod = "Standard";
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifCameraMaker()
                {
                    #region Camera Maker
                    var _cameraMaker = exifInfo.Where(x => x.Id == cameraMaker010F).SingleOrDefault();
                    if (_cameraMaker != null)
                    {
                        CameraMaker = Encoding.ASCII.GetString(_cameraMaker.Value).Replace("\0", "");
                        PropertyCount++;
                    }

                    #endregion
                }

                public void exifCameraModel()
                {
                    #region Camera Model
                    var _cameraModel = exifInfo.Where(x => x.Id == cameraModel0110).SingleOrDefault();

                    if (_cameraModel != null)
                    {
                        CameraModel = Encoding.ASCII.GetString(_cameraModel.Value).Replace("\0", "");
                        PropertyCount++;
                    }

                    #endregion
                }

                public void exifISOSpeedRating()
                {
                    #region ISO Speed Rating
                    var _ISOSpeedRating = exifInfo.Where(x => x.Id == ISOSpeedRating8827).SingleOrDefault();
                    if (_ISOSpeedRating != null)
                    {
                        ISOSpeedRating = BitConverter.ToInt16(_ISOSpeedRating.Value, 0).ToString();
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifCameraFirmware()
                {
                    #region  Camera Firmware
                    var _cameraFirmware = exifInfo.Where(x => x.Id == 305).SingleOrDefault();
                    if (_cameraFirmware != null)
                    {
                        CameraFirmware = Encoding.ASCII.GetString(_cameraFirmware.Value).Replace("\0", "");
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifColorModel()
                {
                    #region --- Color Space / Color Model ---
                    var _colorSpace = exifInfo.Where(x => x.Id == colorSpaceA001).FirstOrDefault();
                    if (_colorSpace != null)
                    {

                        ColorSpace = BitConverter.ToInt16(_colorSpace.Value, 0).ToString();
                        if (ColorSpace == "1")
                            ColorModel = "RGB";

                        else
                            ColorModel = "other";
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifProfileName()
                {
                    #region --- Profile Name ---
                    var _profileName = exifInfo.Where(x => x.Id == 34675).FirstOrDefault();
                    if (_profileName != null)
                    {
                        if (_profileName.Value != null)
                        {
                            ProfileName = Encoding.ASCII.GetString(_profileName.Value).Replace("\0", "");
                            PropertyCount++;
                        }
                    }
                    #endregion
                }

                public void exifAltitude()
                {
                    #region Altitude
                    var _altitude = exifInfo.Where(x => x.Id == altitude0006).SingleOrDefault();
                    if (_altitude != null)
                    {
                        var alt = _altitude.Value;
                        byte[] num = { (byte)alt[0], (byte)alt[1], (byte)alt[2], (byte)alt[3] };

                        byte[] den = { (byte)alt[4], (byte)alt[5], (byte)alt[6], (byte)alt[7] };


                        uint numo = BitConverter.ToUInt32(num, 0);

                        uint deno = BitConverter.ToUInt32(den, 0);

                        Altitude = ((float)numo / deno).ToString("0.00");
                        PropertyCount++;
                    }
                    #endregion
                }

                public void exifGPS()
                {
                    #region--- Get GPS Longitude and Latitude ---
                    try
                    {
                        latitudeDegree = 360;
                        latitudeRadian = 360;
                        longitudeDegree = 360;
                        longitudeRadian = 360;

                        string latiRef = string.Empty;
                        var _latiRef = exifInfo.Where(x => x.Id == latitudeRef0001).SingleOrDefault();
                        if (_latiRef != null)
                        {
                            latiRef = Encoding.ASCII.GetString(_latiRef.Value).Replace("\0", "");
                        }
                        else
                            return;


                        var lati = exifInfo.Where(x => x.Id == latitude0002).SingleOrDefault().Value;

                        uint nLatiDegree = BitConverter.ToUInt32(new byte[] { lati[0], lati[1], lati[2], lati[3] }, 0);
                        uint dLatiDegree = BitConverter.ToUInt32(new byte[] { lati[4], lati[5], lati[6], lati[7] }, 0);

                        uint nLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[8], lati[9], lati[10], lati[11] }, 0);
                        uint dLatiMinutes = BitConverter.ToUInt32(new byte[] { lati[12], lati[13], lati[14], lati[15] }, 0);

                        uint nLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[16], lati[17], lati[18], lati[19] }, 0);
                        uint dLatiSeconds = BitConverter.ToUInt32(new byte[] { lati[20], lati[21], lati[22], lati[23] }, 0);

                        latitudeDegree = ConvertDegreeAngleToDouble(nLatiDegree / dLatiDegree, nLatiMinutes / dLatiMinutes, nLatiSeconds / dLatiSeconds, latiRef);

                        if (Math.Abs(latitudeDegree) > 90)
                        {
                            latitudeDegree = 360;
                            return;
                        }
                        latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree);
                        if (Math.Abs(latitudeRadian) > 90)
                        {
                            latitudeDegree = 360;
                            return;
                        }

                        string longRef = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == longitudeRef0003).SingleOrDefault().Value).Replace("\0", "");
                        var longi = exifInfo.Where(x => x.Id == longitude0004).SingleOrDefault().Value;

                        uint nLongiDegree = BitConverter.ToUInt32(new byte[] { longi[0], longi[1], longi[2], longi[3] }, 0);
                        uint dLongiDegree = BitConverter.ToUInt32(new byte[] { longi[4], longi[5], longi[6], longi[7] }, 0);

                        uint nLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[8], longi[9], longi[10], longi[11] }, 0);
                        uint dLongiMinutes = BitConverter.ToUInt32(new byte[] { longi[12], longi[13], longi[14], longi[15] }, 0);

                        uint nLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[16], longi[17], longi[18], longi[19] }, 0);
                        uint dLongiSeconds = BitConverter.ToUInt32(new byte[] { longi[20], longi[21], longi[22], longi[23] }, 0);

                        longitudeDegree = ConvertDegreeAngleToDouble(nLongiDegree / dLongiDegree, nLongiMinutes / dLongiMinutes, nLongiSeconds / dLongiSeconds, longRef);
                        if (Math.Abs(latitudeRadian) > 180)
                        {
                            longitudeDegree = 360;
                            return;
                        }
                        longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree);
                        if (Math.Abs(longitudeRadian) > 180)
                        {
                            longitudeDegree = 360;
                            return;
                        }

                        LatitudeRef = latiRef;
                        LongitudeRef = longRef;

                        Latitiude = string.Format("{0}° {1}' {2}\" {3}", (float)nLatiDegree / dLatiDegree, (float)nLatiMinutes / dLatiMinutes, (float)nLatiSeconds / dLatiSeconds, latiRef.ToUpper());
                        PropertyCount++;
                        Longitude = string.Format("{0}° {1}' {2}\" {3}", (float)nLongiDegree / dLongiDegree, (float)nLongiMinutes / dLongiMinutes, (float)nLongiSeconds / dLongiSeconds, longRef.ToUpper());
                        PropertyCount++;
                    }
                    catch
                    {
                        ; // Properties not contains information
                    }
                    finally
                    {
                        exifInfo = null;
                    }
                    #endregion
                }
        */





#if false
        public csExifInfo(string dt, string latiRef, byte[] lati, string longRef, byte[] longi)
        {
            try
            {

                // get Capture time
                DateTime dtCreated = DateTime.MaxValue;
                dateCreated = dtCreated;
                try
                {
                    //dt = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == int.Parse("9003", System.Globalization.NumberStyles.HexNumber)).SingleOrDefault().Value);
                    if (DateTime.TryParseExact(dt.Replace("\0", ""), "yyyy:MM:dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dtCreated))
                    {
                        dateCreated = dtCreated;
                    }
                }
                catch
                {
                    ; // Properties not contains information
                }


                // Get GPS 
                try
                {
                    latitudeDegree = 360;
                    latitudeRadian = 360;
                    longitudeDegree = 360;
                    longitudeRadian = 360;

                    //string latiRef = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == int.Parse("0001", System.Globalization.NumberStyles.HexNumber)).SingleOrDefault().Value).Replace("\0", "");
                    //var lati = exifInfo.Where(x => x.Id == int.Parse("0002", System.Globalization.NumberStyles.HexNumber)).SingleOrDefault().Value;

                    latitudeDegree = ConvertDegreeAngleToDouble(lati[0] / lati[4], lati[8] / lati[12], lati[16] / lati[20], latiRef.Replace("\0", ""));

                    if (Math.Abs(latitudeDegree) > 90)
                    {
                        latitudeDegree = 360;
                        return;
                    }
                    latitudeRadian = AppFunctions.degreeToRadian(latitudeDegree);
                    if (Math.Abs(latitudeRadian) > 90)
                    {
                        latitudeDegree = 360;
                        return;
                    }

                    //string longRef = Encoding.ASCII.GetString(exifInfo.Where(x => x.Id == int.Parse("0003", System.Globalization.NumberStyles.HexNumber)).SingleOrDefault().Value).Replace("\0", "");
                    //var longi = exifInfo.Where(x => x.Id == int.Parse("0004", System.Globalization.NumberStyles.HexNumber)).SingleOrDefault().Value;

                    longitudeDegree = ConvertDegreeAngleToDouble(longi[0] / longi[4], longi[8] / longi[12], longi[16] / longi[20], longRef);
                    if (Math.Abs(latitudeRadian) > 180)
                    {
                        longitudeDegree = 360;
                        return;
                    }
                    longitudeRadian = AppFunctions.degreeToRadian(longitudeDegree);
                    if (Math.Abs(longitudeRadian) > 180)
                    {
                        longitudeDegree = 360;
                        return;
                    }
                }
                catch
                {
                    ; // Properties not contains information
                }
                finally
                {
                    //exifInfo = null;
                }
            }
            catch
            {

            }
        }
#endif

        public static double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds, string latLongRef)
        {
            double result = ConvertDegreeAngleToDouble(degrees, minutes, seconds);
            if (latLongRef.ToUpper() == "S" || latLongRef.ToUpper() == "W")
            {
                result *= -1;
            }
            return result;
        }

        public static double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }


        void TestMethod(string filePath, ref DateTime dtTaken)
        {

        }

        //void ReadMetaData(string fileName)
        //{
        //    IEnumerable<MetadataExtractor.Directory> directories = null;
        //    try
        //    {
        //        if (!File.Exists(fileName))
        //            return;
        //        directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(fileName);
        //        /*foreach (var directory in directories)
        //            foreach (var tag in directory.Tags)
        //                Trace.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
        //                */
        //        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        //        try
        //        {
        //            var dateTime = subIfdDirectory?.Parent.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

        //            if (dateTime != null)
        //            {
        //                //2018:02:08 11:36:04
        //                DateTime dtTaken = DateTime.ParseExact(dateTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        //                CaptureDate = dtTaken;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        if (CaptureDate.Date == DateTime.MinValue.Date || CaptureDate.Date == DateTime.MaxValue.Date)
        //        {
        //            try
        //            {
        //                var dateTime = subIfdDirectory?.Parent.GetDescription(ExifDirectoryBase.TagDateTime);
        //                if (dateTime != null)
        //                {
        //                    //2018:02:08 11:36:04
        //                    DateTime dtTaken = DateTime.ParseExact(dateTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        //                    CaptureDate = dtTaken;
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }


        //        if (CaptureDate.Date == DateTime.MinValue.Date || CaptureDate.Date == DateTime.MaxValue.Date)
        //        {
        //            try
        //            {
        //                var dateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
        //                if (dateTime != null)
        //                {
        //                    //2018:02:08 11:36:04
        //                    DateTime dtTaken = DateTime.ParseExact(dateTime, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        //                    CaptureDate = dtTaken;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                cGlobalSettings.oLogger.WriteLogException("ReadMetaData:: While parse Capture Date: ", ex);
        //            }
        //        }
        //        subIfdDirectory = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        cGlobalSettings.oLogger.WriteLogException("ReadMetaData:: ", ex);
        //    }
        //    finally
        //    {
        //        if (directories != null) directories.TryDispose();
        //        directories = null;
        //    }
        //}

        /*
        void GetImageCaptureDate(object objPath)
        {
            DateTime dtTaken = DateTime.MinValue;
            List<string> arrHeaders = null;
            Shell32.Shell shell = null;
            Shell32.Folder shellFolder;
            try
            {
                string filePath = Convert.ToString(objPath);
                arrHeaders = new List<string>();
                shell = new Shell32.Shell();
                string value = "";
                string dirName = Path.GetDirectoryName(filePath);
                shellFolder = shell.NameSpace(dirName);
                string fileName = Path.GetFileName(filePath);
                Shell32.FolderItem folderitem = shellFolder.ParseName(fileName);
                if (folderitem != null)
                {
                    for (int i = 0; i < short.MaxValue; i++)
                    {
                        //Get the property name for property index i
                        string property = shellFolder.GetDetailsOf(null, i);

                        //Will be empty when all possible properties has been looped through, break out of loop
                        if (String.IsNullOrEmpty(property)) break;

                        //Skip to next property if this is not the specified property
                        if (property != "Date taken") continue;

                        //Read value of property
                        value = shellFolder.GetDetailsOf(folderitem, i);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(value))
                {
                    bool success = DateTime.TryParse(value, out dtTaken);
                    if(success)
                    {
                        CaptureDate = dtTaken;
                    }
                    System.Threading.Thread threadForCulture = new System.Threading.Thread(delegate () { });
                    string format = threadForCulture.CurrentCulture.DateTimeFormat.ShortDatePattern;


                    string strFormatDate = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    string strFormatTime = CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern;
                    dtTaken = DateTime.Parse(value, CultureInfo.CurrentCulture);
                    if (!DateTime.TryParseExact(value, strFormatDate, CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out dtTaken))
                    {
                        DateTime.TryParse(value, out dtTaken);
                    }
                }
                CaptureDate = dtTaken;
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("csExifInfo:: GetImageCaptureDate:: ", ex);
            }
            finally
            {
                if (arrHeaders != null) arrHeaders.Clear();
                arrHeaders = null;
                shellFolder = null;
                if (shell != null) shell = null;
            }
        }

        */
    }
}
