using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DuplicatePhotosFixer.CustomControl
{
    public class CircularProgressBar : Control
    {
        #region Enums

        public enum ProgressShape
        {
            Round,
            Flat
        }

        public enum TextMode
        {
            None,
            Value,
            Percentage,
            Custom
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(long), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(57L, FrameworkPropertyMetadataOptions.AffectsRender,
                    OnValueChanged));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(long), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(100L, FrameworkPropertyMetadataOptions.AffectsRender,
                    OnMaximumChanged));

        public static readonly DependencyProperty BarColor1Property =
            DependencyProperty.Register(nameof(BarColor1), typeof(Color), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(Colors.Orange, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BarColor2Property =
            DependencyProperty.Register(nameof(BarColor2), typeof(Color), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(Colors.Orange, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register(nameof(BarWidth), typeof(double), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(14.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(nameof(LineColor), typeof(Color), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(Colors.DimGray, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.Register(nameof(LineWidth), typeof(double), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ProgressShapeValueProperty =
            DependencyProperty.Register(nameof(ProgressShapeValue), typeof(ProgressShape), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(ProgressShape.Flat, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextModeValueProperty =
            DependencyProperty.Register(nameof(TextModeValue), typeof(TextMode), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(TextMode.Percentage, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CustomTextProperty =
            DependencyProperty.Register(nameof(CustomText), typeof(string), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        // NEW: Add SecondaryText property
        public static readonly DependencyProperty SecondaryTextProperty =
            DependencyProperty.Register(nameof(SecondaryText), typeof(string), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        // NEW: Add SecondaryFontSize property
        public static readonly DependencyProperty SecondaryFontSizeProperty =
            DependencyProperty.Register(nameof(SecondaryFontSize), typeof(double), typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(13.0, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region Properties

        public long Value
        {
            get => (long)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public long Maximum
        {
            get => (long)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public Color BarColor1
        {
            get => (Color)GetValue(BarColor1Property);
            set => SetValue(BarColor1Property, value);
        }

        public Color BarColor2
        {
            get => (Color)GetValue(BarColor2Property);
            set => SetValue(BarColor2Property, value);
        }

        public double BarWidth
        {
            get => (double)GetValue(BarWidthProperty);
            set => SetValue(BarWidthProperty, value);
        }

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public double LineWidth
        {
            get => (double)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        public ProgressShape ProgressShapeValue
        {
            get => (ProgressShape)GetValue(ProgressShapeValueProperty);
            set => SetValue(ProgressShapeValueProperty, value);
        }

        public TextMode TextModeValue
        {
            get => (TextMode)GetValue(TextModeValueProperty);
            set => SetValue(TextModeValueProperty, value);
        }

        public string CustomText
        {
            get => (string)GetValue(CustomTextProperty);
            set => SetValue(CustomTextProperty, value);
        }

        public string SecondaryText
        {
            get => (string)GetValue(SecondaryTextProperty);
            set => SetValue(SecondaryTextProperty, value);
        }

        public double SecondaryFontSize
        {
            get => (double)GetValue(SecondaryFontSizeProperty);
            set => SetValue(SecondaryFontSizeProperty, value);
        }

        #endregion

        #region Constructor

        static CircularProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularProgressBar),
                new FrameworkPropertyMetadata(typeof(CircularProgressBar)));
        }

        public CircularProgressBar()
        {
            MinWidth = 100;
            MinHeight = 100;
            Width = 130;
            Height = 130;
        }

        #endregion

        #region Property Changed Callbacks

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CircularProgressBar)d;
            long newValue = (long)e.NewValue;
            if (newValue > control.Maximum)
            {
                control.Value = control.Maximum;
            }
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CircularProgressBar)d;
            long newMaximum = (long)e.NewValue;
            if (newMaximum < 1)
            {
                control.Maximum = 1;
            }
        }

        #endregion

        #region Methods

        public void Increment(long val)
        {
            Value = Math.Min(Value + val, Maximum);
        }

        public void Decrement(long val)
        {
            Value = Math.Max(Value - val, 0);
        }

        #endregion

        #region Rendering

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            double size = Math.Min(ActualWidth, ActualHeight);
            Point center = new Point(size / 2, size / 2);
            double radius = (size - 48) / 2 + 6;

            // Draw background circle
            drawingContext.DrawEllipse(
                new SolidColorBrush(Background is SolidColorBrush bg ? bg.Color : Colors.Transparent),
                null,
                center,
                radius,
                radius);

            // Draw middle line circle
            if (LineWidth > 0)
            {
                drawingContext.DrawEllipse(
                    null,
                    new Pen(new SolidColorBrush(LineColor), LineWidth),
                    center,
                    radius,
                    radius);
            }

            // Draw progress arc
            if (Value > 0 && Maximum > 0)
            {
                double angle = (360.0 / Maximum) * Value;
                DrawProgressArc(drawingContext, center, radius, angle);
            }

            // Draw text (now supports two lines)
            DrawText(drawingContext, center, size);
        }

        private void DrawProgressArc(DrawingContext dc, Point center, double radius, double angle)
        {
            if (angle <= 0) return;

            // Cap angle at 359.9 to prevent full circle rendering issues
            if (angle >= 360)
                angle = 359.9;

            double startAngle = -90;
            double endAngle = startAngle + angle;

            Point startPoint = GetPointOnCircle(center, radius, startAngle);
            Point endPoint = GetPointOnCircle(center, radius, endAngle);

            bool isLargeArc = angle > 180;

            PathFigure pathFigure = new PathFigure { StartPoint = startPoint };
            pathFigure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = isLargeArc
            });

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            LinearGradientBrush brush = new LinearGradientBrush(BarColor1, BarColor2, 45);

            Pen pen = new Pen(brush, BarWidth);
            pen.StartLineCap = ProgressShapeValue == ProgressShape.Round ? PenLineCap.Round : PenLineCap.Flat;
            pen.EndLineCap = ProgressShapeValue == ProgressShape.Round ? PenLineCap.Round : PenLineCap.Flat;

            dc.DrawGeometry(null, pen, pathGeometry);
        }

        private Point GetPointOnCircle(Point center, double radius, double angleDegrees)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            return new Point(
                center.X + radius * Math.Cos(angleRadians),
                center.Y + radius * Math.Sin(angleRadians));
        }

        private void DrawText(DrawingContext dc, Point center, double size)
        {
            string displayText = GetDisplayText();
            string secondaryText = SecondaryText;

            if (string.IsNullOrEmpty(displayText) && string.IsNullOrEmpty(secondaryText))
                return;

#pragma warning disable CS0618 // Type or member is obsolete

            // Calculate total height for both texts
            double totalHeight = 0;
            FormattedText primaryFormatted = null;
            FormattedText secondaryFormatted = null;

            if (!string.IsNullOrEmpty(displayText))
            {
                primaryFormatted = new FormattedText(
                    displayText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    Foreground);
                totalHeight += primaryFormatted.Height;
            }

            if (!string.IsNullOrEmpty(secondaryText))
            {
                secondaryFormatted = new FormattedText(
                    secondaryText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    SecondaryFontSize,
                    Foreground);
                totalHeight += secondaryFormatted.Height;
            }

            // Start position (centered vertically for both texts)
            double startY = center.Y - totalHeight / 2;
            Color foregroundColor = (Foreground is SolidColorBrush brush) ? brush.Color : Colors.White;

            // Draw primary text
            if (primaryFormatted != null)
            {
                Point textPosition = new Point(
                    center.X - primaryFormatted.Width / 2,
                    startY);

                // Draw shadow
                FormattedText shadowText = new FormattedText(
                    displayText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    new SolidColorBrush(Color.FromArgb(100, foregroundColor.R, foregroundColor.G, foregroundColor.B)));

                dc.DrawText(shadowText, new Point(textPosition.X + 2, textPosition.Y + 2));
                dc.DrawText(primaryFormatted, textPosition);

                startY += primaryFormatted.Height;
            }

            // Draw secondary text
            if (secondaryFormatted != null)
            {
                Point textPosition = new Point(
                    center.X - secondaryFormatted.Width / 2,
                    startY);

                // Draw shadow
                FormattedText shadowText = new FormattedText(
                    secondaryText,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    SecondaryFontSize,
                    new SolidColorBrush(Color.FromArgb(100, foregroundColor.R, foregroundColor.G, foregroundColor.B)));

                dc.DrawText(shadowText, new Point(textPosition.X + 2, textPosition.Y + 2));
                dc.DrawText(secondaryFormatted, textPosition);
            }

#pragma warning restore CS0618 // Type or member is obsolete
        }

        private string GetDisplayText()
        {
            switch (TextModeValue)
            {
                case TextMode.None:
                    return string.Empty;
                case TextMode.Value:
                    return Value.ToString();
                case TextMode.Percentage:
                    return Maximum > 0 ? $"{(100 * Value) / Maximum}%" : "0%";
                case TextMode.Custom:
                    return CustomText;
                default:
                    return string.Empty;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double size = Math.Min(constraint.Width, constraint.Height);
            if (double.IsInfinity(size))
                size = 130;
            return new Size(size, size);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            double size = Math.Min(arrangeBounds.Width, arrangeBounds.Height);
            return new Size(size, size);
        }

        #endregion
    }
}