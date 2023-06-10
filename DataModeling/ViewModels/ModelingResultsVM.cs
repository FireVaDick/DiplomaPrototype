using Charts.Creators;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataModeling.DataSource;
using DataModeling.Models;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;

namespace DataModeling.ViewModels;

public partial class ModelingResultsVM : ObservableObject
{

    private const double TimeMinimum = 0.0;
    private const double TimeMaximum = 11.0;
    private const string MachinesLabel = "Выполнение работы";
    private const string OutputResourceLabel = "Наполнение задела";
    private const string InputResourcesLabel = "Истощение заделов";

    public Axis TimeAxis { get; set; }

    public Axis[] XAxes { get; set; }

    public Axis[] YAxesInputResources { get; set; }

    public Axis[] YAxesMachines { get; set; }

    public Axis[] YAxesOutputResource { get; set; }

    public RectangularSection[] Sections { get; set; }

    public Margin DrawMargin { get; set; }

    public DrawMarginFrame DrawMarginFrame { get; set; }

    [ObservableProperty]
    public ObservableCollection<IndustrialChain> chains;

    [ObservableProperty]
    public IndustrialChain selectedChain;

    [ObservableProperty]
    public IndustrialOperation selectedOperation;

    [ObservableProperty]
    public RectangularSection timeSection;

    [ObservableProperty]
    public double rangeSize;

    public double RangeMinimum { get; } = 1;

    public double RangeMaximum { get; } = TimeMaximum - TimeMinimum + 1;

    public ModelingResultsVM()
    {
        var dataSource = new MockDataSource();
        chains = dataSource.GetData();
        selectedChain = chains[0];
        selectedOperation = selectedChain.Operations[0];

        rangeSize = RangeMaximum;

        timeSection = new RectangularSection
        {
            Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(20)),
            IsVisible = false,
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Black.WithAlpha(45),
                PathEffect = new DashEffect(new float[] { 8, 6 }),
                StrokeThickness = 2,
            },
            Xi = 3,
            Xj = 7,
        };
        Sections = new RectangularSection[] { timeSection };

        var creator = new AxisCreator();
        TimeAxis = creator.CreateXAxis();
        TimeAxis.MinLimit = TimeMinimum;
        TimeAxis.MaxLimit = TimeMaximum;

        XAxes = new Axis[] { TimeAxis };
        YAxesInputResources = new Axis[] { creator.CreateYAxis(InputResourcesLabel) };
        YAxesMachines = new Axis[] { creator.CreateYAxis(MachinesLabel), };
        YAxesOutputResource = new Axis[] { creator.CreateYAxis(OutputResourceLabel), };

        DrawMargin = new Margin
        {
            Left = 70,
            Top = Margin.Auto,
            Right = Margin.Auto,
            Bottom = Margin.Auto,
        };
        DrawMarginFrame = new DrawMarginFrame()
        {
            Fill = new SolidColorPaint(SKColors.WhiteSmoke),
            Stroke = new SolidColorPaint(SKColors.LightGray, 2)
        };
    }

    #region Commands

    [RelayCommand]
    public void ChangeRange()
    {
        if (RangeSize < 0.0)
        {
            throw new ArgumentException($"[Ошибка]: Значение {nameof(RangeSize)} не может быть меньше или равно нулю.");
        }

        if (RangeSize == RangeMinimum)
        {
            return;
        }

        if (RangeSize == RangeMaximum)
        {
            TimeAxis.MinLimit = TimeMinimum;
            TimeAxis.MaxLimit = TimeMaximum;

            return;
        }

        var currentRangeSize = TimeAxis.MaxLimit - TimeAxis.MinLimit + 1;
        var rangeDifference = RangeSize - currentRangeSize;

        if (rangeDifference < 0.0 || rangeDifference + TimeAxis.MaxLimit < TimeMaximum)
        {
            TimeAxis.MaxLimit += rangeDifference;
        }
        else
        {
            var correctiveValue = RangeMaximum - TimeAxis.MaxLimit;

            TimeAxis.MaxLimit += correctiveValue;
            TimeAxis.MinLimit -= RangeSize - correctiveValue;
        }
    }

    [RelayCommand]
    public void NextOperation()
    {
        var operations = SelectedChain.Operations;
        var index = operations.IndexOf(SelectedOperation);

        if (index == operations.Count - 1)
        {
            return;
        }

        SelectedOperation = operations[index + 1];
    }

    [RelayCommand]
    public void PreviousOperation()
    {
        var operations = SelectedChain.Operations;
        var index = operations.IndexOf(SelectedOperation);

        if (index == 0)
        {
            return;
        }

        SelectedOperation = operations[index - 1];
    }

    [RelayCommand]
    public void ShiftRangeBackward()
    {
        if (TimeMinimum == TimeAxis.MinLimit)
        {
            return;
        }

        var rangeDecrement = RangeSize;

        if (TimeAxis.MinLimit < RangeSize)
        {
            rangeDecrement = (double)TimeAxis.MinLimit - TimeMinimum;
        }

        TimeAxis.MinLimit -= rangeDecrement;
        TimeAxis.MaxLimit -= rangeDecrement;
    }

    [RelayCommand]
    public void ShiftRangeForward()
    {
        if (TimeMaximum == TimeAxis.MaxLimit)
        {
            return;
        }

        var rangeIncrement = RangeSize;

        if (TimeAxis.MaxLimit + RangeSize > TimeMaximum)
        {
            rangeIncrement = TimeMaximum - (double)TimeAxis.MaxLimit;
        }

        TimeAxis.MinLimit += rangeIncrement;
        TimeAxis.MaxLimit += rangeIncrement;
    }

    [RelayCommand]
    public void ChangeSectionVisibility()
    {
        TimeSection.IsVisible = !TimeSection.IsVisible;
    }

    [RelayCommand]
    public void DisableScaling()
    {
        RangeSize = RangeMaximum;
        TimeAxis.MinLimit = TimeMinimum;
        TimeAxis.MaxLimit = TimeMaximum;
        SetLimitsNull(YAxesInputResources[0]);
        SetLimitsNull(YAxesMachines[0]);
        SetLimitsNull(YAxesOutputResource[0]);
    }

    [RelayCommand]
    public void SelectСhain(IndustrialChain chain)
    {
        SelectedChain = chain;
        SelectedOperation = SelectedChain.Operations[0];
    }

    [RelayCommand]
    public void SelectOperation(IndustrialOperation operation)
    {
        SelectedOperation = operation;
    }    

    [RelayCommand]
    public void SetSectionBounds()
    {
        throw new NotImplementedException();
    }

    #endregion

    private void SetLimitsNull(Axis axis)
    {
        axis.MinLimit = null;
        axis.MaxLimit = null;
    }

    private bool IsAxisBounds()
    {
        return TimeMinimum == TimeAxis.MinLimit && TimeMaximum == TimeAxis.MaxLimit;
    }

    private string FormatTime(double value)
    {
        var minutes = Math.Truncate(value);
        var seconds = Math.Truncate(Math.Round(value - minutes, 2) * 100);

        return string.Format("{0}:{1}", minutes, seconds);
    }
}
