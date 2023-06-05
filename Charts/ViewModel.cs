using Charts.Creators;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Charts
{
    // Sample
    public partial class ViewModel : ObservableObject
    {
        private const int DefaultWidth = 2;

        private readonly SKColor InputChartColor = new SKColor(189, 183, 107);
        private readonly SKColor MachineChartColor = new SKColor(0, 100, 0);
        private readonly SKColor OutputChartColor = new SKColor(189, 183, 107);

        private const string DefaultMachine = "Степень выполнения, %";
        private const string DefaultResource = "Объём задела, шт";
        private const string InputResource = "Входной задел";
        private const string Machine = "Работа станка";
        private const string OutputResource = "Выходной задел";

        #region Charts values
        public ISeries[] InputResourceCollection { get; set; }

        public ISeries[] MachineCollection { get; set; }

        public ISeries[] OutputResourceCollection { get; set; }
        #endregion

        #region Axises
        public Axis[] TimeXAxis { get; set; }

        public Axis[] InputResourceYAxis { get; set; }

        public Axis[] OutputResourceYAxis { get; set; }

        public Axis[] MachineYAxis { get; set; }
        #endregion

        public Margin DrawMargin { get; set; }

        public DrawMarginFrame DrawMarginFrame { get; set; }

        public ViewModel()
        {
            var creator = new AxisCreator();

            var inputResourceValues = new ObservablePoint[] {
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
            };
            var outputResourceValues = new ObservablePoint[] {
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
            };
            var machineValues = new ObservablePoint[] {
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
            };

            #region Значения графиков
            InputResourceCollection = new ISeries[]
            {
                new LineSeries<ObservablePoint> {
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0,
                    Name = InputResource,
                    Stroke = new SolidColorPaint(InputChartColor, DefaultWidth),
                    Values = inputResourceValues,
                },
            };
            MachineCollection = new ISeries[]
            {
                new LineSeries<ObservablePoint> {
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0,
                    Name = Machine,
                    Values = machineValues,
                    Stroke = new SolidColorPaint(MachineChartColor, DefaultWidth),
                },
            };
            OutputResourceCollection = new ISeries[] {
                new LineSeries<ObservablePoint> {
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0,
                    Name = OutputResource,
                    Stroke = new SolidColorPaint(OutputChartColor, DefaultWidth),
                    Values = outputResourceValues,
                }
            };
            #endregion

            #region Оси
            TimeXAxis = new Axis[] { creator.CreateXAxis() };
            InputResourceYAxis = new Axis[] { creator.CreateYAxis(DefaultResource) };
            MachineYAxis = new Axis[] { creator.CreateYAxis(DefaultMachine), };
            OutputResourceYAxis = new Axis[] { creator.CreateYAxis(DefaultResource), };
            #endregion

            DrawMargin = new Margin
            {
                Left = 70,
                Top = Margin.Auto,
                Right = Margin.Auto,
                Bottom = Margin.Auto
            };

            DrawMarginFrame = new DrawMarginFrame()
            {
                Fill = new SolidColorPaint(SKColors.DarkGray),
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.DarkGray,
                    StrokeThickness = 1
                }
            };
        }
    }
}
