using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DuplicatePhotosFixer.CustomControl
{
   public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        public override void BeginInit()
        {
            base.BeginInit();
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource SelectedImage
        {
            get { return (ImageSource)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }
        public static readonly DependencyProperty SelectedImageProperty =
            DependencyProperty.Register("SelectedImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public int SelectedRotateAngle
        {
            get { return (int)GetValue(SelectedRotationProperty); }
            set { SetValue(SelectedRotationProperty, value); }
        }
        public static readonly DependencyProperty SelectedRotationProperty =
            DependencyProperty.Register("SelectedRotateAngle", typeof(int), typeof(ImageButton), new PropertyMetadata(null));

        public int HoverRotateAngle
        {
            get { return (int)GetValue(HoverRotationProperty); }
            set { SetValue(HoverRotationProperty, value); }
        }
        public static readonly DependencyProperty HoverRotationProperty =
            DependencyProperty.Register("HoverRotateAngle", typeof(int), typeof(ImageButton), new PropertyMetadata(null));

        public int RotateAngle
        {
            get { return (int)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("RotateAngle", typeof(int), typeof(ImageButton), new PropertyMetadata(null));


        public bool IsFocused
        {
            get { return (bool)GetValue(SelectedButtonProperty); }
            set { SetValue(SelectedButtonProperty, value); }
        }
        public static readonly DependencyProperty SelectedButtonProperty =
            DependencyProperty.Register("IsFocused", typeof(bool), typeof(ImageButton), new PropertyMetadata(null));

        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("Text", typeof(int), typeof(ImageButton), new PropertyMetadata(null));


    }
}
