using DataModeling.Creators;
using DataModeling.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using ModelingResults.Creators;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace DataModeling.DataSource
{
    public class MockDataSource : IDataSource
    {
        private const int DefaultWidth = 2;

        private const string InputResource = "Входной задел";
        private const string Machine = "Работа станка";
        private const string OutputResource = "Выходной задел";

        private readonly SKColor InputChartColor = new SKColor(189, 183, 107);
        private readonly SKColor MachineChartColor = new SKColor(0, 100, 0);
        private readonly SKColor OutputChartColor = new SKColor(189, 183, 107);

        private readonly ISeriesCreator seriesCreator = new SeriesCreator();

        public ObservableCollection<IndustrialChain> GetData()
        {
            var inputSeries = new ISeries[]
            {
                seriesCreator.Create(
                    new ObservableCollection<ObservablePoint>()
                    {
                        new ObservablePoint(1, 5),
                        new ObservablePoint(3, 5),
                        new ObservablePoint(3, 4),
                        new ObservablePoint(5, 4),
                        new ObservablePoint(5, 3),
                        new ObservablePoint(7, 3),
                        new ObservablePoint(7, 2),
                        new ObservablePoint(9, 2),
                        new ObservablePoint(9, 1),
                        new ObservablePoint(11, 1),
                        new ObservablePoint(11, 0),
                    }, InputResource, InputChartColor, DefaultWidth),
            };
            var machineSeries = new ISeries[]
            {
                seriesCreator.Create(new ObservableCollection<ObservablePoint>()
                    {
                        new ObservablePoint(1, 0),
                        new ObservablePoint(3, 1),
                        new ObservablePoint(3, 0),
                        new ObservablePoint(5, 1),
                        new ObservablePoint(5, 0),
                        new ObservablePoint(7, 1),
                        new ObservablePoint(7, 0),
                        new ObservablePoint(9, 1),
                        new ObservablePoint(9, 0),
                        new ObservablePoint(11, 1),
                        new ObservablePoint(11, 0),
                    }, Machine, MachineChartColor, DefaultWidth),
            };
            var outputSeries = new ISeries[] 
            {
                seriesCreator.Create(new ObservablePoint[] {
                        new ObservablePoint(1, 1),
                        new ObservablePoint(3, 1),
                        new ObservablePoint(3, 2),
                        new ObservablePoint(5, 2),
                        new ObservablePoint(5, 3),
                        new ObservablePoint(7, 3),
                        new ObservablePoint(7, 4),
                        new ObservablePoint(9, 4),
                        new ObservablePoint(9, 5),
                        new ObservablePoint(11, 5),
                    }, OutputResource, OutputChartColor, DefaultWidth),
            };
            var inputSeries1 = new ISeries[]
            {
                seriesCreator.Create(
                    new ObservableCollection<ObservablePoint>()
                    {
                        new ObservablePoint(1, 5),
                        new ObservablePoint(3, 5),
                        new ObservablePoint(3, 4),
                        new ObservablePoint(5, 4),
                        new ObservablePoint(5, 3),
                        new ObservablePoint(7, 3),
                        new ObservablePoint(7, 2),
                        new ObservablePoint(9, 2),
                        new ObservablePoint(9, 1),
                        new ObservablePoint(11, 1),
                        new ObservablePoint(11, 0),
                    }, InputResource, SKColors.Red, DefaultWidth),
            };
            var machineSeries1 = new ISeries[]
            {
                seriesCreator.Create(new ObservableCollection<ObservablePoint>()
                    {
                        new ObservablePoint(1, 0),
                        new ObservablePoint(3, 1),
                        new ObservablePoint(3, 0),
                        new ObservablePoint(5, 1),
                        new ObservablePoint(5, 0),
                        new ObservablePoint(7, 1),
                        new ObservablePoint(7, 0),
                        new ObservablePoint(9, 1),
                        new ObservablePoint(9, 0),
                        new ObservablePoint(11, 1),
                        new ObservablePoint(11, 0),
                    }, Machine, SKColors.BlueViolet, DefaultWidth),
            };
            var outputSeries1 = new ISeries[] {
                seriesCreator.Create(new ObservablePoint[] {
                        new ObservablePoint(1, 1),
                        new ObservablePoint(3, 1),
                        new ObservablePoint(3, 2),
                        new ObservablePoint(5, 2),
                        new ObservablePoint(5, 3),
                        new ObservablePoint(7, 3),
                        new ObservablePoint(7, 4),
                        new ObservablePoint(9, 4),
                        new ObservablePoint(9, 5),
                        new ObservablePoint(11, 5),
                    }, OutputResource, SKColors.DarkSlateBlue, DefaultWidth),
            };

            return new ObservableCollection<IndustrialChain>()
            {
                new IndustrialChain(1, "Тестовая цепочка 1", new ObservableCollection<IndustrialOperation>()
                {
                    new IndustrialOperation(1, 1, "Операция 1.1", inputSeries, machineSeries, outputSeries),
                    new IndustrialOperation(1, 2, "Операция 1.2", outputSeries, machineSeries, inputSeries),
                }),
                new IndustrialChain(2, "Тестовая цепочка 2", new ObservableCollection<IndustrialOperation>()
                {
                    new IndustrialOperation(2, 1, "Операция 2.1", outputSeries1, machineSeries1, inputSeries1),
                    new IndustrialOperation(2, 2, "Операция 2.2", inputSeries1, machineSeries1, outputSeries1),
                })
            };
        }
    }
}
