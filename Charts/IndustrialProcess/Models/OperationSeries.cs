using LiveChartsCore.Defaults;
using SkiaSharp;
using System.Collections.Generic;

namespace Charts.Models
{
    public class OperationSeries
    {
        private readonly SKColor DefaultColor = new SKColor(0, 100, 0);
        private const int DefaultWidth = 2;

        public int Width { get; }

        public SKColor Color { get; }

        public List<ObservablePoint> Series { get; } = new List<ObservablePoint>();

        public OperationSeries() 
        {
            Color = DefaultColor;
            Width = DefaultWidth;
        }

        public OperationSeries(SKColor color, int thickness, List<ObservablePoint> points)
        {
            Color = color;
            Width = thickness;
            Series = points;
        }
    }
}
