using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace DuplicatePhotosFixer.CustomControl
{
    public class UnitUpDown : IntegerUpDown
    {
        static UnitUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnitUpDown), new FrameworkPropertyMetadata(typeof(UnitUpDown)));
        }

        protected override void OnValueChanged(int? oldValue, int? newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateUnitDisplay();
        }

        private void UpdateUnitDisplay()
        {
            if (Value == 0)
            {
                Text = "KB";
            }
            else if (Value == 1)
            {
                Text = "MB";
            }
        }

        protected override void OnIncrement()
        {
            Value = 1; // Set to MB
            UpdateUnitDisplay();
        }

        protected override void OnDecrement()
        {
            Value = 0; // Set to KB
            UpdateUnitDisplay();
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == System.Windows.Input.Key.Up)
            {
                OnIncrement();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Down)
            {
                OnDecrement();
                e.Handled = true;
            }
        }
    }
}
