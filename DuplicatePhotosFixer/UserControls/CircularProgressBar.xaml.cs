using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DuplicatePhotosFixer.UserControls
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar : UserControl
    {
        public CircularProgressBar()
        {
            InitializeComponent();
            UpdateArc();
        }

        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(100.0, OnValueChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(10.0));

        public static readonly DependencyProperty BackgroundStrokeThicknessProperty =
            DependencyProperty.Register("BackgroundStrokeThickness", typeof(double), typeof(CircularProgressBar),
                new PropertyMetadata(20.0));

        public static readonly DependencyProperty ForegroundStrokeProperty =
            DependencyProperty.Register("ForegroundStroke", typeof(Brush), typeof(CircularProgressBar),
                new PropertyMetadata(Brushes.DeepSkyBlue));

        public static readonly DependencyProperty BackgroundStrokeProperty =
            DependencyProperty.Register("BackgroundStroke", typeof(Brush), typeof(CircularProgressBar),
                new PropertyMetadata(Brushes.LightGray));

        #endregion

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }


        public double BackgroundStrokeThickness
        {
            get => (double)GetValue(BackgroundStrokeThicknessProperty);
            set => SetValue(BackgroundStrokeThicknessProperty, value);
        }

        public Brush ForegroundStroke
        {
            get => (Brush)GetValue(ForegroundStrokeProperty);
            set => SetValue(ForegroundStrokeProperty, value);
        }

        public Brush BackgroundStroke
        {
            get => (Brush)GetValue(BackgroundStrokeProperty);
            set => SetValue(BackgroundStrokeProperty, value);
        }

        public string ProgressText => $"{(int)(Value / Maximum * 100)}%";

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CircularProgressBar)d).UpdateArc();
        }

        private void UpdateArc()
        {
            // Don’t render until we have valid size
            if (ActualWidth <= 0 || ActualHeight <= 0)
                return;

            double percentage = Math.Max(0, Math.Min(1, Value / Maximum));
            double angle = 360 * percentage;

            double radius = (ActualWidth / 2) - ((BackgroundStrokeThickness) / 2);
            
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            Point startPoint = new Point(center.X, center.Y - radius);

            double endX = center.X + radius * Math.Sin(Math.PI * angle / 180);
            double endY = center.Y - radius * Math.Cos(Math.PI * angle / 180);
            Point endPoint = new Point(endX, endY);

            bool largeArc = angle > 180.0;

            PathFigure figure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = false
            };

            figure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                IsLargeArc = largeArc,
                SweepDirection = SweepDirection.Clockwise
            });

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            ProgressPath.Data = geometry;
            //((TextBlock)((Grid)Content).Children[2]).Text = ProgressText;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateArc();
        }
    }
}
