using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Charts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ScalerMinimum = 1.0;
        private const double ScalerMaximum = 480.0;
        private const double RangeMinimum = 1.0;
        private const double RangeMaximum = 480.0;
        private const double DefaultRangeValue = 480.0;
        private readonly DoubleCollection Ticks = new DoubleCollection { 5, 10, 15, 20, 24, 30, 40, 48, 60, 80, 120, 160, 240, 480 };

        //private readonly Axis TimeAxis;

        public MainWindow()
        {
            InitializeComponent();

            #region Default values
            ChartScaler.Minimum = ScalerMinimum;
            ChartScaler.Maximum = ScalerMaximum;
            ChartScaler.Value = DefaultRangeValue;

            RangeSizeSlider.Minimum = RangeMinimum;
            RangeSizeSlider.Maximum = RangeMaximum;
            RangeSizeSlider.Value = DefaultRangeValue;
            RangeSizeSlider.Ticks = Ticks;
            RangeSizeSlider.IsSnapToTickEnabled = true;
            #endregion

            #region StatusBar filling
            StatusBarChain.Content = "Изготовление двигателя";
            StatusBarOperation.Content = "Изготовление поршней";
            StatusBarRangeStart.Content = FormatTime(0.0);
            #endregion
        }

        private void RangeSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StatusBarRangeSize == null)
            {
                return;
            }

            if (ChartScaler.Track != null)
            {
                ChartScaler.Track.Thumb.MinHeight = e.NewValue;
            }

            StatusBarRangeSize.Content = e.NewValue;
            CalculateScalerMaximum(e.NewValue);
        }

        private void CalculateScalerMaximum(double newValue)
        {
            var newPointsCount = Math.Truncate(ScalerMaximum / newValue);

            if (ChartScaler.Value != 0)
            {
                var value = ScalerMaximum * ChartScaler.Maximum / ChartScaler.Value;
                ChartScaler.Value = Math.Truncate(newPointsCount * value / ScalerMaximum);
            }

            ChartScaler.Maximum = newPointsCount;
        }

        private string FormatTime(double value)
        {
            var minutes = Math.Truncate(value);
            var seconds = Math.Truncate(Math.Round(value - minutes, 2) * 100);

            return string.Format("{0}:{1}", minutes, seconds);
        }
    }
}
