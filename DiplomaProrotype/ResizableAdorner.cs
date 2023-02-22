using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomaProrotype
{
    internal class ResizableAdorner : Adorner
    {
        VisualCollection AdornerVisuals;
        Thumb thumb1, thumb2;
        Rectangle Rec;

        public ResizableAdorner(UIElement adornedElement) : base(adornedElement)
        {
            AdornerVisuals = new VisualCollection(this);
            thumb1 = new Thumb() { Background = Brushes.Black, Height = 8, Width = 8 };
            thumb2 = new Thumb() { Background = Brushes.Black, Height = 8, Width = 8 };
            Rec = new Rectangle() { Stroke = Brushes.Black, StrokeThickness = 1.5, StrokeDashArray = { 3, 2 } };


            thumb1.DragDelta += Thumb1_DragDelta;
            thumb2.DragDelta += Thumb2_DragDelta;

            AdornerVisuals.Add(Rec);
            AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);
        }

        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;
            ele.Height = ele.Height - e.VerticalChange < 0 ? 0 : ele.Height - e.VerticalChange;
            ele.Width = ele.Width - e.HorizontalChange < 0 ? 0 : ele.Width - e.HorizontalChange;
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;
            ele.Height = ele.Height + e.VerticalChange < 0 ? 0 : ele.Height + e.VerticalChange ;
            ele.Width = ele.Width + e.HorizontalChange < 0 ? 0 : ele.Width + e.HorizontalChange;
        }


        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Rec.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));
            //thumb1.Arrange(new Rect(-5, -5, 10, 10));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5 - 100, 10, 10));

            return base.ArrangeOverride(finalSize);
        }

    }
}
