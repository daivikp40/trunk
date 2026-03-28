using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DuplicatePhotosFixer
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int kb)
            {
                if (kb >= 1024)
                {
                    return $"{kb / 1024.0:F2} MB";
                }
                else
                {
                    return $"{kb} KB";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                if (strValue.EndsWith("MB", StringComparison.OrdinalIgnoreCase))
                {
                    if (double.TryParse(strValue.Replace("MB", "").Trim(), out double mb))
                    {
                        return (int)(mb * 1024);
                    }
                }
                else if (strValue.EndsWith("KB", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(strValue.Replace("KB", "").Trim(), out int kb))
                    {
                        return kb;
                    }
                }
            }
            return value;
        }
    }
}
