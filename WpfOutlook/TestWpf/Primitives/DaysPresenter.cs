using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestWpf.Controls;

namespace TestWpf.Primitives
{
    public class DaysPresenter : Panel
    {
        private bool _visualChildrenGenerated;
        private UIElementCollection _visualChildren;

        static DaysPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DaysPresenter), new FrameworkPropertyMetadata(typeof(DaysPresenter)));
        }

        public CalendarViewPeriod Period { get; set; }

        public ListView ListView { get; set; }

        public CalendarView CalendarView { get; set; }

        protected override int VisualChildrenCount
        {
            get
            {
                if (_visualChildren == null)
                    return base.VisualChildrenCount;

                return _visualChildren.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if ((index < 0) || (index >= VisualChildrenCount))
                throw new ArgumentOutOfRangeException("index", index, "Index out of range");

            if (_visualChildren == null)
                return base.GetVisualChild(index);

            return _visualChildren[index];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in _visualChildren)
                element.Arrange(new Rect(new Point(0, 0), finalSize));

            return finalSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            GenerateVisualChildren();

            return constraint;
        }

        protected void GenerateVisualChildren()
        {
            if (_visualChildrenGenerated)
                return;

            if (_visualChildren == null)
                _visualChildren = CreateUIElementCollection(null);
            else
                _visualChildren.Clear();

            RangePanel panel = new RangePanel();

            Label labelDay = new Label();
            labelDay.FontSize = 15;
            labelDay.Content = $"{Period.BeginDate:ddd, MMM d, yyyy}";
            labelDay.HorizontalAlignment = HorizontalAlignment.Center;
            labelDay.VerticalAlignment = VerticalAlignment.Center;

            Border border = new Border() { BorderBrush = Brushes.Red, BorderThickness = new Thickness(2.0), CornerRadius = new CornerRadius(5, 5, 5, 5) };
            border.Child = panel;
            border.Child = labelDay;
            _visualChildren.Add(border);

            _visualChildrenGenerated = true;
        }
    }

}
