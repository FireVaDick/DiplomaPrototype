using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ModelingResults.Creators;
using SkiaSharp;
using System.Collections.Generic;

namespace DataModeling.Creators
{
    public class SeriesCreator : ISeriesCreator
    {
        private const int DefaultWidth = 2;

        private readonly SKColor DefaultColor = new SKColor(0, 100, 0);

        public ISeries<ObservablePoint> Create(
            IEnumerable<ObservablePoint> values,
            string name)
        {
            var series = CreateLineSeries(DefaultColor);
            series.Values = values;
            series.Name = name;

            return series;
        }

        public ISeries<ObservablePoint> Create(
            IEnumerable<ObservablePoint> values,
            string name,
            SKColor color,
            int width)
        {
            var series = CreateLineSeries(color, width);
            series.Values = values;
            series.Name = name;

            return series;
        }

        private LineSeries<ObservablePoint> CreateLineSeries(SKColor color, int width = DefaultWidth)
        {
            return new LineSeries<ObservablePoint>
            {
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 0,
                Stroke = new SolidColorPaint(color, width),
            };
        }
    }
}
