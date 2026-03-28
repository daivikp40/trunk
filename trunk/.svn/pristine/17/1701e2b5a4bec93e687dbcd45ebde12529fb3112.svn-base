using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DuplicatePhotosFixer
{
    public class StringToStorageSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int unitIndex = (int)value;
            return unitIndex == 0 ? "KB" : "MB";


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string unitString = (string)value;
            return unitString == "KB" ? 0 : 1;
        }

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    int sizeInKB = (int)value;
        //    if (sizeInKB >= 1024)
        //    {
        //        double sizeInMB = sizeInKB / 1024.0;
        //        return $"{sizeInMB:F2} MB";
        //    }
        //    return $"{sizeInKB} KB";
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    string sizeString = (string)value;
        //    if (sizeString.EndsWith("MB"))
        //    {
        //        if (double.TryParse(sizeString.Replace(" MB", ""), out double sizeInMB))
        //        {
        //            return (int)(sizeInMB * 1024);
        //        }
        //    }
        //    else if (sizeString.EndsWith("KB"))
        //    {
        //        if (int.TryParse(sizeString.Replace(" KB", ""), out int sizeInKB))
        //        {
        //            return sizeInKB;
        //        }
        //    }
        //    return 0; // Default or error value
        //}
    }
}
