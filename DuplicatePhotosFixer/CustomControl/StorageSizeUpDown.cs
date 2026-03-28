using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace DuplicatePhotosFixer.CustomControl
{
    public class StorageSizeUpDown : IntegerUpDown
    {
        static StorageSizeUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StorageSizeUpDown), new FrameworkPropertyMetadata(typeof(StorageSizeUpDown)));
        }

        public StorageSizeUpDown()
        {
            this.ValueChanged += StorageSizeUpDown_ValueChanged;
        }

        private void StorageSizeUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.Value >= 1024)
            {
                this.Text = $"{this.Value / 1024} MB";
            }
            else
            {
                this.Text = $"{this.Value} KB";
            }
        }

        protected override void OnValueChanged(int? oldValue, int? newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            this.Text = ConvertValueToString(newValue);
        }

        private string ConvertValueToString(int? value)
        {
            if (value.HasValue)
            {
                if (value.Value >= 1024)
                {
                    return $"{value.Value / 1024} MB";
                }
                else
                {
                    return $"{value.Value} KB";
                }
            }
            return "0 KB";
        }
    }
}
