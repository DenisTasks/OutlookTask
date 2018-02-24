using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestWpf.Controls
{
    public class RangePanel : Panel
    {
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangePanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangePanel), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RangePanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static DependencyProperty BeginDateProperty = DependencyProperty.RegisterAttached("BeginDate", typeof(double), typeof(UIElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty EndDateProperty = DependencyProperty.RegisterAttached("EndDate", typeof(double), typeof(UIElement), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static void SetBegin(UIElement element, double value)
        {
            element.SetValue(BeginDateProperty, value);
        }

        public static double GetBegin(UIElement element)
        {
            return (double)element.GetValue(BeginDateProperty);
        }

        public static void SetEnd(UIElement element, double value)
        {
            element.SetValue(EndDateProperty, value);
        }

        public static double GetEnd(UIElement element)
        {
            return (double)element.GetValue(EndDateProperty);
        }

        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double containerRange = (this.Maximum - this.Minimum);

            foreach (UIElement element in this.Children)
            {
                double begin = (double)element.GetValue(RangePanel.BeginDateProperty);
                double end = (double)element.GetValue(RangePanel.EndDateProperty);
                double elementRange = end - begin;

                Size size = new Size();
                size.Width = (Orientation == Orientation.Vertical) ? finalSize.Width : elementRange / containerRange * finalSize.Width;
                size.Height = (Orientation == Orientation.Vertical) ? elementRange / containerRange * finalSize.Height : finalSize.Height;

                Point location = new Point();
                location.X = (Orientation == Orientation.Vertical) ? 0 : (begin - this.Minimum) / containerRange * finalSize.Width;
                location.Y = (Orientation == Orientation.Vertical) ? (begin - this.Minimum) / containerRange * finalSize.Height : 0;

                element.Arrange(new Rect(location, size));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement element in this.Children)
            {
                element.Measure(availableSize);
            }
            return availableSize;
        }
    }
}
