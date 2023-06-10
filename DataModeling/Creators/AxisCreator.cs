using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;

namespace Charts.Creators
{
    public class AxisCreator
    {
        private readonly SKColor DefaultColor = SKColors.Black;
        private const int DefaultTextSize = 15;
        private const int MinLimit = 0;

        public Axis CreateXAxis()
        {
            return new Axis
            {
                MinLimit = MinLimit,
                MinStep = 1,
                NamePaint = new SolidColorPaint(DefaultColor),
                NameTextSize = DefaultTextSize,
                NamePadding = new Padding(0, 15),
            };
        }

        public Axis CreateYAxis(string name)
        {
            return new Axis
            {
                MinLimit = MinLimit,
                Name = name,
                NamePaint = new SolidColorPaint(DefaultColor),
                NameTextSize = 15,
            };
        }
    }
}
