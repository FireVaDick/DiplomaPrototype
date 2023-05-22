using Bytescout.Spreadsheet;
using DiplomaProrotype.Animations;
using DiplomaProrotype.CanvasManipulation;
using DiplomaProrotype.ColorsManipulation;
using DiplomaProrotype.Models;
using DiplomaProrotype.ObjectsManipulation;
using DiplomaPrototype;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomaProrotype
{
    public partial class MainWindow : Window
    {
        static public MatrixWindow matrixWindow;
        static public SelectWindow selectWindow;       

        static public List<ResourceTile> resourceTiles = new List<ResourceTile>();
        static public List<MachineTile> machineTiles = new List<MachineTile>();
        static public List<MovableTile> movableTiles = new List<MovableTile>();
        static public List<StopTile> stopTiles = new List<StopTile>();
        static public List<Rectangle> mainStopPlaces = new List<Rectangle>();
        static public List<Link> links = new List<Link>();

        static public ResourceTile resourceTileFromContextMenu;
        static public MachineTile machineTileFromContextMenu;
        static public MovableTile movableTileFromContextMenu;
        static public StopTile stopTileFromContextMenu;

        static public string[,] matrixResourceMachine = new string[1, 1];
        static public string[,] matrixResourceStop = new string[1, 1];
        static public string[,] matrixChainStop = new string[0, 0];
        static public int[,] matrixParticipation = new int[0, 0];
        static public List<int> vectorChain = new List<int>();
        static public List<int> vectorSignal = new List<int>();
        static public List<int> vectorUnoccupation = new List<int>();

        static public UserControl chosenOneObject;
        static public string lastTileType = "";
        static public string currentMode = "move";
        static public string currentPathType = "solid";
        static public bool resourceNearMachineIsEmpty = false;

        static public int amountLoading = 0;
        static public int amountUnloading = 0;

        Vector targetMargin;

        public MainWindow()
        {
            InitializeComponent();

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = 0;

            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(MyGrid).Add(new ResizableCanvas(TargetBorder));          
        }


        #region Выбор режимов
        private void ModeTile_Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModeChooser.ChooseMoveMode();
        }
        private void ModeTile_Link_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModeChooser.ChooseLinkMode();
        }
        private void ModeTile_Route_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModeChooser.ChooseRouteMode();
        }
        private void ModeTile_Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModeChooser.ChoosePathMode();
        }

        private void CMChooseSolidPath_Click(object sender, RoutedEventArgs e)
        {
            currentPathType = "solid";
            ModeChooser.ChoosePathMode();
        }
        private void CMChooseDiscontinuousPath_Click(object sender, RoutedEventArgs e)
        {
            currentPathType = "discontinuous";
            ModeChooser.ChoosePathMode();
        }
        #endregion


        #region Размещение объекта (создание + перемещение)
        private void TargetCanvas_Drop(object sender, DragEventArgs e)
        {
            ObjectPlacement.ObjectMovingFromPanelOrCanvas(e);
        }

        private void TargetCanvas_DragOver(object sender, DragEventArgs e)
        {}

        private void ObjectPanel_Drop(object sender, DragEventArgs e)
        {
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;
            var machineData = e.Data.GetData(typeof(MachineTile)) as MachineTile;
            var movableData = e.Data.GetData(typeof(MovableTile)) as MovableTile;
            var stopData = e.Data.GetData(typeof(StopTile)) as StopTile;

            if (!(resourceData is null) && resourceData.Parent is StackPanel == false)
            {
                ((Canvas)(resourceData.Parent)).Children.Remove(resourceData);
            }

            if (!(machineData is null) && machineData.Parent is StackPanel == false)
            {
                ((Canvas)(machineData.Parent)).Children.Remove(machineData);
            }

            if (!(movableData is null) && movableData.Parent is StackPanel == false)
            {
                ((Canvas)(movableData.Parent)).Children.Remove(movableData);
            }

            if (!(stopData is null) && stopData.Parent is StackPanel == false)
            {
                ((Canvas)(stopData.Parent)).Children.Remove(stopData);
            }
        }
        #endregion


        #region Рисование связей, маршрутов, путей
        private void TargetCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawLinkRoutePath.DrawStart(e);
        }

        private void TargetCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            DrawLinkRoutePath.DrawInterim(e);
        }

        private void TargetCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DrawLinkRoutePath.DrawEndMouseLeft(e);
        }

        private void TargetCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DrawLinkRoutePath.DrawEndMouseRight();
        }     
        #endregion


        #region Анимации сокрытия панелей
        private void ModeBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.ModeBorderAnimation();
        }

        private void ObjectBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.ObjectBorderAnimation();
        }

        private void MatrixBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.MatrixBorderAnimation();
        }

        private void ClearBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.ClearBorderAnimation();
        }

        private void ColorBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.ColorBorderAnimation();
        }
        #endregion


        #region Динамическая передача цвета последнему размещённому элементу
        private void ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorChooser.DynamicChooseColorFromPalette();
        }
        #endregion


        #region Просмотр всех связей
        private void ModeTile_CheckAllLinks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MatrixWindow.CreateMatrixWindow(); 
        }
        #endregion


        #region Запись в Excel
        private void WriteToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            Spreadsheet document = new Spreadsheet();
            Worksheet sheet = document.Workbook.Worksheets.Add("writeExcel");

            for (int i = 0; i < machineTiles.Count + 1; i++)
            {
                for (int j = 0; j < resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1);

                    if (matrixResourceMachine[i, j] == null && i > 0 && j > 0)
                        sheet.Cell(currentCell).Value = 0;
                    else
                        sheet.Cell(currentCell).Value = matrixResourceMachine[i, j];

                }
            }

            for (int i = 0; i < stopTiles.Count + 1; i++)
            {
                for (int j = 0; j < resourceTiles.Count + 1; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1 + matrixResourceMachine.GetLength(0));

                    if (matrixResourceStop[i, j] == null && i > 0 && j > 0)
                        sheet.Cell(currentCell).Value = 0;
                    else
                        sheet.Cell(currentCell).Value = matrixResourceStop[i, j];

                }

                for (int j = 0; j < vectorChain.Count; j++)
                {
                    var currentColumn = Convert.ToString(Convert.ToChar(65 + j + matrixResourceStop.GetLength(1)));
                    var currentCell = string.Format("{0}{1}", currentColumn, i + 1 + matrixResourceMachine.GetLength(0));

                    if (matrixChainStop[i, j] == null && i > 0 && j >= 0)
                        sheet.Cell(currentCell).Value = 0;
                    else
                        sheet.Cell(currentCell).Value = matrixChainStop[i, j];

                }
            }

            if (File.Exists(@"..\\Result.xlsx"))
            {
                File.Delete(@"..\\Result.xlsx");
            }

            document.SaveAs(@"..\\Result.xlsx");
            document.Close();

            //Process.Start("excel.exe", @"..\\Result");
        }
        #endregion


        #region Очистка холста
        private void ModeTile_Erase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cleanup.CreateCleanupContextMenu();
        }
        #endregion


        #region Масштабирование
        private void SliderAllInterface_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (var c in TargetCanvas.Children.OfType<UserControl>())
            {
                c.RenderTransform = new ScaleTransform(SliderAllInterface.Value, SliderAllInterface.Value);
            }

            foreach (var c in TargetCanvas.Children.OfType<Line>())
            {
                c.RenderTransform = new ScaleTransform(SliderAllInterface.Value, SliderAllInterface.Value);
            }

            foreach (var c in TargetCanvas.Children.OfType<Ellipse>())
            {
                c.RenderTransform = new ScaleTransform(SliderAllInterface.Value, SliderAllInterface.Value);
            }
        }
        #endregion


        #region Горячие клавиши
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Z:
                        Cleanup.CMDeleteLastElement_Click(sender, e); break;

                    case Key.S:
                        PanelAnimation.ShowAllBordersAnimation(); break;

                    case Key.H:
                        PanelAnimation.HideAllBordersAnimation(); break;
                }
            }
            else 
                switch (e.Key)
                {
                    case Key.Escape:
                        Close(); break;

                    case Key.Tab:
                        MatrixWindow.CreateMatrixWindow(); break;

                    case Key.D1:
                        ModeChooser.ChooseMoveMode(); break;

                    case Key.D2:
                        ModeChooser.ChooseLinkMode(); break;

                    case Key.D3:
                        ModeChooser.ChooseRouteMode(); break;

                    case Key.D4:
                        ModeChooser.ChoosePathMode(); break;

                    case Key.Up:
                        targetMargin = VisualTreeHelper.GetOffset(chosenOneObject);
                        Canvas.SetTop(chosenOneObject, --targetMargin.Y - 10);
                        CheckLinkPlacement(); break;

                    case Key.Down:
                        targetMargin = VisualTreeHelper.GetOffset(chosenOneObject);
                        Canvas.SetTop(chosenOneObject, ++targetMargin.Y - 10);
                        CheckLinkPlacement(); break;

                    case Key.Left:
                        targetMargin = VisualTreeHelper.GetOffset(chosenOneObject);
                        Canvas.SetLeft(chosenOneObject, --targetMargin.X - 10); 
                        CheckLinkPlacement(); break;

                    case Key.Right:
                        targetMargin = VisualTreeHelper.GetOffset(chosenOneObject);
                        Canvas.SetLeft(chosenOneObject, ++targetMargin.X - 10);
                        CheckLinkPlacement(); break;
                }
        }

        private void CheckLinkPlacement()
        {
            Type type = chosenOneObject.GetType();

            if (type == typeof(ResourceTile))
            {
                targetMargin.X += chosenOneObject.Width / 2 - 2.5;
                targetMargin.Y += 22.5 + 2.5; // 22.5 = ((ResourceTile)chosenOneObject).ResourceFigure.Height / 2
                ObjectPlacement.ResourceLinkMoving((Point)targetMargin, (ResourceTile)chosenOneObject);
            }

            if (type == typeof(MachineTile))
            {
                targetMargin.X += chosenOneObject.Width / 2 - 2.5;
                targetMargin.Y += ((MachineTile)chosenOneObject).MachineIndicator.Height + ((MachineTile)chosenOneObject).MachineImage.Height / 2 + 5;
                ObjectPlacement.MachineLinkMoving((Point)targetMargin, (MachineTile)chosenOneObject);
            }

            if (type == typeof(StopTile))
            {
                targetMargin.X += chosenOneObject.Width / 2 - 8.5;
                targetMargin.Y += ((StopTile)chosenOneObject).StopFigure.Height / 2 + 2.5;
                ObjectPlacement.StopLinkMoving((Point)targetMargin, (StopTile)chosenOneObject);
            }
        }


        #endregion
    }
}

