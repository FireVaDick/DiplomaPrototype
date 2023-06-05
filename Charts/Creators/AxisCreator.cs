using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;

namespace Charts.Creators
{
    public class AxisCreator
    {
        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_crosshair = new(255, 171, 145);

        private readonly SKColor DefaultColor = SKColors.Black;
        private const int DefaultTextSize = 15;

        public Axis CreateXAxis()
        {
            return new Axis
            {
                NamePaint = new SolidColorPaint(DefaultColor),
                NameTextSize = DefaultTextSize,
                NamePadding = new Padding(0, 15),
                //Padding = new Padding(5, 15, 5, 5),
                //LabelsPaint = new SolidColorPaint(s_gray),
                //SeparatorsPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1,
                //    PathEffect = new DashEffect(new float[] { 3, 3 })
                //},
                //SubseparatorsPaint = new SolidColorPaint
                //{
                //    Color = s_gray2,
                //    StrokeThickness = 0.5f
                //},
                //ZeroPaint = new SolidColorPaint
                //{
                //    Color = s_gray1,
                //    StrokeThickness = 2
                //},
                //TicksPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1.5f
                //},
                //SubticksPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1
                //},
                //CrosshairPaint = new SolidColorPaint
                //{
                //    Color = s_crosshair,
                //    StrokeThickness = 3
                //},
                //CrosshairLabelsPaint = new SolidColorPaint
                //{
                //    Color = DefaultColor,
                //    SKFontStyle = new SKFontStyle(SKFontStyleWeight.SemiBold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
                //},
                //CrosshairLabelsBackground = s_crosshair.AsLvcColor(),
                //CrosshairPadding = new Padding(10, 20, 10, 10)
            };
        }

        public Axis CreateYAxis(string name)
        {
            return new Axis
            {
                Name = name,
                NamePaint = new SolidColorPaint(DefaultColor),
                NameTextSize = 15,
                //NamePadding = new Padding(5, 15),
                //Padding = new Padding(5, 0, 15, 0),
                //LabelsPaint = new SolidColorPaint(s_gray),
                //SeparatorsPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1,
                //    PathEffect = new DashEffect(new float[] { 3, 3 })
                //},
                //SubseparatorsPaint = new SolidColorPaint
                //{
                //    Color = s_gray2,
                //    StrokeThickness = 0.5f
                //},
                //ZeroPaint = new SolidColorPaint
                //{
                //    Color = s_gray1,
                //    StrokeThickness = 2
                //},
                //TicksPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1.5f
                //},
                //SubticksPaint = new SolidColorPaint
                //{
                //    Color = s_gray,
                //    StrokeThickness = 1
                //},
                //CrosshairPaint = new SolidColorPaint
                //{
                //    Color = s_crosshair,
                //    StrokeThickness = 3
                //},
                //CrosshairLabelsPaint = new SolidColorPaint
                //{
                //    Color = SKColors.Black,
                //    SKFontStyle = new SKFontStyle(SKFontStyleWeight.SemiBold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
                //},
                //CrosshairLabelsBackground = s_crosshair.AsLvcColor(),
                //CrosshairPadding = new Padding(10, 10, 30, 10)
            };
        }
    }
}
