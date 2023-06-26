using LiveChartsCore;
using LiveChartsCore.Defaults;
using SkiaSharp;
using System.Collections.Generic;

namespace ModelingResults.Creators
{
    public interface ISeriesCreator
    {
        ISeries<ObservablePoint> Create(
            IEnumerable<ObservablePoint> values,
            string name);

        ISeries<ObservablePoint> Create(
            IEnumerable<ObservablePoint> values,
            string name,
            SKColor color,
            int width);
    }
}
