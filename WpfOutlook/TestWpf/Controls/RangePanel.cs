using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace TestWpf.Controls
{
    public class RangePanel : Panel
    {
        public static DependencyProperty MinimumHeightProperty = DependencyProperty.Register("MinimumHeight", typeof(double), typeof(RangePanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty MaximumHeightProperty = DependencyProperty.Register("MaximumHeight", typeof(double), typeof(RangePanel), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static DependencyProperty StartProperty = DependencyProperty.RegisterAttached("Start", typeof(double), typeof(UIElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty FinishProperty = DependencyProperty.RegisterAttached("Finish", typeof(double), typeof(UIElement), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static DependencyProperty DayOfYearProperty = DependencyProperty.RegisterAttached("DayOfYear", typeof(int), typeof(UIElement), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static void SetDayOfYear(UIElement element, int value)
        {
            element.SetValue(DayOfYearProperty, value);
        }
        public static int GetDayOfYear(UIElement element)
        {
            return (int) element.GetValue(DayOfYearProperty);
        }
        public static void SetStart(UIElement element, double value)
        {
            element.SetValue(StartProperty, value);
        }
        public static double GetStart(UIElement element)
        {
            return (double)element.GetValue(StartProperty);
        }
        public static void SetFinish(UIElement element, double value)
        {
            element.SetValue(FinishProperty, value);
        }
        public static double GetFinish(UIElement element)
        {
            return (double)element.GetValue(FinishProperty);
        }

        public double MaximumHeight
        {
            get { return (double)this.GetValue(MaximumHeightProperty); }
            set { this.SetValue(MaximumHeightProperty, value); }
        }
        public double MinimumHeight
        {
            get { return (double)this.GetValue(MinimumHeightProperty); }
            set { this.SetValue(MinimumHeightProperty, value); }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double containerRangeHeigth = (this.MaximumHeight - this.MinimumHeight);

            List<UIElement> uiAll = new List<UIElement>();
            List<UIElement> uiOverlapping = new List<UIElement>();

            foreach (UIElement item in this.Children)
            {
                uiAll.Add(item);
            }

            for (int i = 0; i < uiAll.Count; i++)
            {
                double begin = (double)uiAll.ElementAt(i).GetValue(StartProperty);
                double end = (double)uiAll.ElementAt(i).GetValue(FinishProperty);
                int dayOfYear = (int)uiAll.ElementAt(i).GetValue(DayOfYearProperty);

                var forOverlap = uiAll.Where(s => (double)s.GetValue(FinishProperty) > begin 
                && (double)s.GetValue(StartProperty) < end
                && (int)s.GetValue(DayOfYearProperty) == dayOfYear).ToList();

                foreach (var item in forOverlap)
                {
                    if (!uiOverlapping.Contains(item) && forOverlap.Count > 1)
                    {
                        uiOverlapping.Add(item);
                    }
                }
            }

            Size widthOverlap = new Size();
            widthOverlap.Width = finalSize.Width / uiOverlapping.Count;
            Point locationX = new Point();
            locationX.X = 0;
            foreach (UIElement element in this.Children)
            {
                if (uiOverlapping.Contains(element))
                {
                    double begin = (double)element.GetValue(StartProperty);
                    double end = (double)element.GetValue(FinishProperty);
                    double elementRange = end - begin;

                    Size size = new Size();
                    size.Width = widthOverlap.Width; // property for overlapped appointment
                    size.Height = elementRange / containerRangeHeigth * finalSize.Height;

                    Point location = new Point();
                    location.X = locationX.X; // property for overlapped appointment
                    location.Y = (begin - MinimumHeight) / containerRangeHeigth * finalSize.Height;

                    element.Arrange(new Rect(location, size));

                    widthOverlap.Width = finalSize.Width / uiOverlapping.Count;
                    locationX.X = locationX.X + finalSize.Width / uiOverlapping.Count;
                }
                else
                {
                    double begin = (double)element.GetValue(StartProperty);
                    double end = (double)element.GetValue(FinishProperty);
                    double elementRange = end - begin;

                    Size size = new Size();
                    size.Width = finalSize.Width;
                    size.Height = elementRange / containerRangeHeigth * finalSize.Height;

                    Point location = new Point();
                    location.X = 0;
                    location.Y = (begin - MinimumHeight) / containerRangeHeigth * finalSize.Height;

                    element.Arrange(new Rect(location, size));
                }
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
