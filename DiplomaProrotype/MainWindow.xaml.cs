using Haley.Models;
using Haley.Services;
using Haley.Utils;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace DiplomaProrotype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        List<ResourceTile> resourceTiles = new List<ResourceTile>();
        List<MachineTile> machineTiles = new List<MachineTile>();
        List<MovableTile> movableTiles = new List<MovableTile>();
        //List<Rectangle> rectangles = new List<Rectangle>(); // Под вопросом?

        private ResourceTile resourceTileContextMenu;
        private MachineTile machineTileContextMenu;
        private MovableTile movableTileContextMenu;

        private int tilesCounter = 0;
        private string lastTileType = "";
        private string currentMode = "path";

        private bool resourceIsEmpty = false;
        private bool modeBorderOpen = true;
        private bool objectBorderOpen = true;
        private bool colorBorderOpen = false;



        public MainWindow()
        {
            InitializeComponent();

            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = 0;

            Loaded += Window_Loaded;  

            //AddModePanelTools();
            //AddObjectPanelTools();
            //ChooseIconsStyle();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(MyGrid).Add(new ResizableAdorner(TargetBorder));
        }



        #region Выбор режимов
        private void ModeTile_Move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMode = "move";

            ModeMove.Foreground = Brushes.Crimson;
            ModeLink.Foreground = Brushes.Black;
            ModeRoute.Foreground = Brushes.Black;
            ModePath.Foreground = Brushes.Black;
        }
        private void ModeTile_Link_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMode = "link";

            ModeMove.Foreground = Brushes.Black;
            ModeLink.Foreground = Brushes.Crimson;
            ModeRoute.Foreground = Brushes.Black;
            ModePath.Foreground = Brushes.Black;
        }
        private void ModeTile_Route_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMode = "route";

            ModeMove.Foreground = Brushes.Black;
            ModeLink.Foreground = Brushes.Black;
            ModeRoute.Foreground = Brushes.Crimson;
            ModePath.Foreground = Brushes.Black;
        }
        private void ModeTile_Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMode = "path";

            ModeMove.Foreground = Brushes.Black;
            ModeLink.Foreground = Brushes.Black;
            ModeRoute.Foreground = Brushes.Black;
            ModePath.Foreground = Brushes.Crimson;
        }
        #endregion


        #region Перемещение объекта
        private void TargetCanvas_Drop(object sender, DragEventArgs e)
        {
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;
            var machineData = e.Data.GetData(typeof(MachineTile)) as MachineTile;
            var movableData = e.Data.GetData(typeof(MovableTile)) as MovableTile;

            Point dropPosition = e.GetPosition(TargetCanvas);

            // Манипуляции с resourceData
            if (machineData is null && movableData is null)
            {
                if (resourceData.Parent is StackPanel)
                {
                    CreateResourceTile(dropPosition, resourceIsEmpty);
                }

                if (resourceData.Parent is Canvas && currentMode == "move")
                {
                    if (resourceTiles.Contains(resourceData))
                    {
                        Canvas.SetLeft(resourceData, dropPosition.X - resourceData.Width / 2 - 7.5);
                        Canvas.SetTop(resourceData, dropPosition.Y - resourceData.Height / 2 + 7.5);
                    }
                }
            }

            // Манипуляции с machineData
            if (resourceData is null && movableData is null)
            {
                if (machineData.Parent is StackPanel)
                {
                    CreateMachineTile(machineData, dropPosition);
                }

                if (machineData.Parent is Canvas && currentMode == "move")
                {
                    if (machineTiles.Contains(machineData))
                    {
                        Canvas.SetLeft(machineData, dropPosition.X - machineData.Width / 2 - 7.5);
                        Canvas.SetTop(machineData, dropPosition.Y - machineData.Height / 2 - 2.5);
                    }
                }
            }

            // Манипуляции с movableData
            if (resourceData is null && machineData is null)
            {
                if (movableData.Parent is StackPanel)
                {
                    CreateMovableTile(movableData, dropPosition);
                }

                if (movableData.Parent is Canvas && currentMode == "move")
                {
                    if (movableTiles.Contains(movableData))
                    {
                        Canvas.SetLeft(movableData, dropPosition.X - movableData.Width / 2 - 10);
                        Canvas.SetTop(movableData, dropPosition.Y - movableData.Height / 2 - 2.5);
                    }
                }
            }
        }

        private void TargetCanvas_DragOver(object sender, DragEventArgs e)
        { }

        private void ObjectPanel_Drop(object sender, DragEventArgs e)
        {
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;
            var machineData = e.Data.GetData(typeof(MachineTile)) as MachineTile;
            var movableData = e.Data.GetData(typeof(MovableTile)) as MovableTile;

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
        }
        #endregion


        #region Создание экземпляров
        private void CreateResourceTile(Point dropPosition, bool resourceIsEmpty)
        {
            var resourceTile = new ResourceTile();
            tilesCounter++;

            resourceTile.Text = "Задел";
            resourceTile.Id = tilesCounter;

            CreateResourceContextMenu(resourceTile);
            resourceTiles.Add(resourceTile);

            Canvas.SetLeft(resourceTile, dropPosition.X - resourceTile.Width / 2 - 7.5);
            Canvas.SetTop(resourceTile, dropPosition.Y - resourceTile.Height / 2);

            resourceTile.ResourceText.Margin = new Thickness(0, 0, 0, 15);
            resourceTile.ResourceId.Visibility = Visibility.Visible;
            resourceTile.Height = 85;

            if (resourceIsEmpty)
            {
                resourceTile.ResourceFigure.Height = 10;
                resourceTile.ResourceFigure.Margin = new Thickness(0, 40, 0, 0);
            }

            lastTileType = "resource";
            TargetCanvas.Children.Add(resourceTile);
        }

        private void CreateMachineTile(MachineTile machineData, Point dropPosition)
        {
            var machineTile = new MachineTile();
            tilesCounter++;

            machineTile.Image = machineData.Image;
            machineTile.Text = machineData.Text;
            machineTile.Id = tilesCounter;

            CreateMachineContextMenu(machineTile);
            machineTiles.Add(machineTile);

            Canvas.SetLeft(machineTile, dropPosition.X - machineTile.Width / 2 - 7.5);
            Canvas.SetTop(machineTile, dropPosition.Y - machineTile.Height / 2 - 25);

            machineTile.MachineImage.Margin = new Thickness(0, 25, 0, 0);
            machineTile.MachineProgress.Margin = new Thickness(10, 0, 10, 30);
            machineTile.MachineText.Margin = new Thickness(0, 0, 0, 15);

            machineTile.MachineIndicator.Visibility = Visibility.Visible;
            machineTile.MachineProgress.Visibility = Visibility.Visible;
            machineTile.MachineId.Visibility = Visibility.Visible;
            machineTile.Height = 115;

            lastTileType = "machine";
            TargetCanvas.Children.Add(machineTile);

            resourceIsEmpty = true;

            dropPosition.X += 70;
            CreateResourceTile(dropPosition, resourceIsEmpty);

            resourceIsEmpty = false;
        }

        private void CreateMovableTile(MovableTile movableData, Point dropPosition)
        {
            var movableTile = new MovableTile();
            tilesCounter++;

            movableTile.Image = movableData.Image;
            movableTile.Text = movableData.Text;
            movableTile.Id = tilesCounter;

            CreateMovableContextMenu(movableTile);
            movableTiles.Add(movableTile);

            Canvas.SetLeft(movableTile, dropPosition.X - movableTile.Width / 2 - 10);
            Canvas.SetTop(movableTile, dropPosition.Y - movableTile.Height / 2 - 22.5);

            movableTile.MovableImage.Margin = new Thickness(0, 25, 0, 0);
            movableTile.MovableText.Margin = new Thickness(0, 0, 0, 15);

            movableTile.MovableIndicator.Visibility = Visibility.Visible;
            movableTile.MovableId.Visibility = Visibility.Visible;
            movableTile.Height = 110;

            lastTileType = "movable";
            TargetCanvas.Children.Add(movableTile);
        }
        #endregion


        #region Рисование линий связи и путей-прямоугольников
        private Point startPoint;
        private Line link;
        private Line route;
        private Rectangle rect;
        double lastX2 = 0, lastY2 = 0;

        private void TargetCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(TargetCanvas);

            if (currentMode == "link")
            {
                link = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                Ellipse ellipse = new Ellipse
                {
                    Fill = Brushes.White,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black,
                    Width = 10,
                    Height = 10,
                    Margin = new Thickness(startPoint.X - 5, startPoint.Y - 5, 0, 0)
                };

                link.X1 = startPoint.X;
                link.Y1 = startPoint.Y;

                TargetCanvas.Children.Add(link);
                TargetCanvas.Children.Add(ellipse);
            }

            if (currentMode == "route")
            {
                if (!(route is null))
                {
                    lastX2 = route.X2;
                    lastY2 = route.Y2;
                }

                route = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                route.X1 = lastX2;
                route.Y1 = lastY2;

                if (lastX2 == 0 && lastY2 == 0)
                {
                    route.X1 = startPoint.X;
                    route.Y1 = startPoint.Y;
                }

                TargetCanvas.Children.Add(route);
            }

            if (currentMode == "path")
            {
                rect = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                Canvas.SetLeft(rect, startPoint.X);
                Canvas.SetTop(rect, startPoint.Y);

                TargetCanvas.Children.Add(rect);
            }
        }

        private void TargetCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentMode == "link")
            {
                if (e.LeftButton == MouseButtonState.Released || link == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                link.X2 = pos.X;
                link.Y2 = pos.Y;
            }

            if (currentMode == "route")
            {
                if (e.LeftButton == MouseButtonState.Released || route == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                route.X2 = pos.X;
                route.Y2 = pos.Y;
            }

            if (currentMode == "path")
            {
                if (e.LeftButton == MouseButtonState.Released || rect == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                rect.Width = w;
                rect.Height = h;

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
            }
        }

        private void TargetCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentMode == "link")
            {
                link = null;
            }

            if (currentMode == "path")
            {
                rect = null;
            }
        }

        private void TargetCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentMode == "route")
            {
                route = null;
            }
        }
        #endregion


        #region Динамическая передача цвета последнему размещённому элементу
        private void ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lastTileType == "resource")
            {
                resourceTiles[resourceTiles.Count - 1].ResourceFigure.Fill = ColorPalette.SelectedBrush;
            }

            /*if (lastTileType == "machine")
            {
                machineTiles[machineTiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }*/

            /*if (lastTileType == "movable")
            {
                movableTiles[movableTiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }*/
        }
        #endregion


        #region Контекстное меню
        private void CreateResourceContextMenu(ResourceTile resourceTile)
        {
            var contextMenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            resourceTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void CreateMachineContextMenu(MachineTile machineTile)
        {
            var contextMenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            machineTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void CreateMovableContextMenu(MovableTile movableTile)
        {
            var contextMenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            movableTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void DefaultContextMenu(ContextMenu contextMenu)
        {
            var menuItemAnima = new MenuItem();
            Image imageAnima = new Image();
            imageAnima.Source = new BitmapImage(new Uri("/Icons/x128/Animate32.png", UriKind.Relative));
            menuItemAnima.Icon = imageAnima;
            menuItemAnima.Click += new RoutedEventHandler(CMAnimate_Click);
            menuItemAnima.Header = "Анимировать";

            var menuItemEdit = new MenuItem();
            Image imageEdit = new Image();
            imageEdit.Source = new BitmapImage(new Uri("/Icons/x128/Pencil32.png", UriKind.Relative));
            menuItemEdit.Icon = imageEdit;
            menuItemEdit.Header = "Редактировать";

            var menuItemColor = new MenuItem();
            Image imageColor = new Image();
            imageColor.Source = new BitmapImage(new Uri("/Icons/x128/Brush32.png", UriKind.Relative));
            menuItemColor.Icon = imageColor;
            menuItemColor.Click += new RoutedEventHandler(CMChangeColor_Click);
            menuItemColor.Header = "Выбрать цвет";

            var menuItemDelete = new MenuItem();
            Image imageDelete = new Image();
            imageDelete.Source = new BitmapImage(new Uri("/Icons/x128/Delete32.png", UriKind.Relative));
            menuItemDelete.Icon = imageDelete;
            menuItemDelete.Click += new RoutedEventHandler(CMDelete_Click);
            menuItemDelete.Header = "Удалить";

            /*var menuItemEditSomething = new MenuItem();
            menuItemEditSomething.Header = "Настроить что-то";
            menuItemEdit.Items.Add(menuItemEditSomething);*/

            contextMenu.Items.Add(menuItemAnima);
            //contextMenu.Items.Add(menuItemEdit);
            contextMenu.Items.Add(menuItemColor);
            contextMenu.Items.Add(menuItemDelete);
        }
        #endregion


        #region Методы контекстного меню
        private void GetTileFromContextMenu(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            
            resourceTileContextMenu = contextMenu.PlacementTarget as ResourceTile;
            machineTileContextMenu = contextMenu.PlacementTarget as MachineTile;
            movableTileContextMenu = contextMenu.PlacementTarget as MovableTile;
        }

        private void ResourceHeightAnimation(int from, int to, int time, Thickness inputMargin, Thickness outputMargin)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);
            resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);

            thicknessAnimation.From = inputMargin;
            thicknessAnimation.To = outputMargin;
            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
        }



        private void CMAnimate_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (machineTileContextMenu is null && movableTileContextMenu is null)
            {
                if (resourceTileContextMenu.ResourceFigure.Height == 45)
                {
                    ResourceHeightAnimation(45, 10, 3, new(0, 5, 0, 0), new(0, 40, 0, 0));
                }
                else
                {
                    ResourceHeightAnimation(10, 45, 3, new (0, 40, 0, 0), new (0, 5, 0, 0));
                }
            }

            if (resourceTileContextMenu is null && movableTileContextMenu is null)
            {
                machineTileContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                worker.RunWorkerAsync();
            }

            /*if (resourceTileContextMenu is null && machineTileContextMenu is null)
            {
                movableTileContextMenu сделать анимацию движения
            }*/
        }

        private void CMCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CMChangeColor_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            var dialog = new ColorPickerDialog();
            dialog.ShowDialog();

            if (resourceTiles.Contains(resourceTileContextMenu))
            {
                resourceTileContextMenu.ResourceFigure.Fill = dialog.SelectedBrush;
            }

            /*if (machineTiles.Contains(machineTileContextMenu))
            {
                machineTileContextMenu.Background = dialog.SelectedBrush;
            }*/

            /*if (mmovableTiles.Contains(movableTileContextMenu))
            {
                mmovableTileContextMenu.Background = dialog.SelectedBrush;
            }*/
        }

        private void CMDelete_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (resourceTiles.Contains(resourceTileContextMenu))
            {
                TargetCanvas.Children.Remove(resourceTileContextMenu);
                resourceTiles.Remove(resourceTileContextMenu);
            }

            if (machineTiles.Contains(machineTileContextMenu))
            {
                TargetCanvas.Children.Remove(machineTileContextMenu);
                machineTiles.Remove(machineTileContextMenu);
            }

            if (movableTiles.Contains(movableTileContextMenu))
            {
                TargetCanvas.Children.Remove(movableTileContextMenu);
                movableTiles.Remove(movableTileContextMenu);
            }
        }
        #endregion


        #region Создание дополнительного потока BackgroundWorker
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; ++i)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            machineTileContextMenu.MachineProgress.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            machineTileContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#DC143C");
        }
        #endregion


        #region Очистка холста
        private void ModeTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Type type = TargetCanvas.Children[^1].GetType();

            if (TargetCanvas.Children.Count!= 0)
            {
                if (type == typeof(Ellipse))
                {
                    TargetCanvas.Children.RemoveAt(TargetCanvas.Children.Count - 1);
                }

                TargetCanvas.Children.RemoveAt(TargetCanvas.Children.Count - 1);
            }
        }

        private void ModeTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = new ContextMenu();

            Eraser.ContextMenu = contextMenu;

            var menuItemDeleteObjects = new MenuItem();
            menuItemDeleteObjects.Click += new RoutedEventHandler(CMDeleteObjects_Click);
            menuItemDeleteObjects.Header = "Удалить только объекты";

            var menuItemDeleteAll = new MenuItem();
            menuItemDeleteAll.Click += new RoutedEventHandler(CMDeleteAll_Click);
            menuItemDeleteAll.Header = "Удалить всё";

            contextMenu.Items.Add(menuItemDeleteObjects);
            contextMenu.Items.Add(menuItemDeleteAll);
        }

        private void CMDeleteObjects_Click(object sender, RoutedEventArgs e)
        {
            tilesCounter = 0;

            for (int i = 0; i < resourceTiles.Count;)
            {
                TargetCanvas.Children.Remove(resourceTiles[i]);
                resourceTiles.Remove(resourceTiles[i]);
            }

            for (int i = 0; i < machineTiles.Count;)
            {
                TargetCanvas.Children.Remove(machineTiles[i]);
                machineTiles.Remove(machineTiles[i]);
            }

            for (int i = 0; i < movableTiles.Count;)
            {
                TargetCanvas.Children.Remove(movableTiles[i]);
                movableTiles.Remove(movableTiles[i]);
            }
        }

        private void CMDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            tilesCounter = 0;

            for (int i = 0; i < resourceTiles.Count;)
            {
                resourceTiles.Remove(resourceTiles[i]);
            }

            for (int i = 0; i < machineTiles.Count;)
            {
                machineTiles.Remove(machineTiles[i]);
            }

            for (int i = 0; i < movableTiles.Count;)
            {
                movableTiles.Remove(movableTiles[i]);
            }

            TargetCanvas.Children.Clear();
        }
        #endregion


        #region Анимации сокрытия панелей
        private void CreateAnimationWidth(Border border, double endValue, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = border.ActualWidth;
            animation.To = endValue;
            animation.Duration = TimeSpan.FromSeconds(duration);
            border.BeginAnimation(Button.WidthProperty, animation);
        }

        private void CreateAnimationHeight(Border border, double endValue, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = border.ActualHeight;
            animation.To = endValue;
            animation.Duration = TimeSpan.FromSeconds(duration);
            border.BeginAnimation(Button.HeightProperty, animation);
        }

        private void ModeBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (modeBorderOpen)
            {
                CreateAnimationWidth(ModeBorder, 20, 0.3);
                modeBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(ModeBorder, 100, 0.3);
                modeBorderOpen = true;
            }
        }

        private void ObjectBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (objectBorderOpen)
            {
                CreateAnimationHeight(ObjectBorder, 20, 0.3);
                objectBorderOpen = false;
            }
            else
            {
                CreateAnimationHeight(ObjectBorder, 100, 0.3);
                objectBorderOpen = true;
            }
        }


        private void ColorBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (colorBorderOpen)
            {
                CreateAnimationWidth(ColorBorder, 35.5, 0.7);
                colorBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(ColorBorder, 350, 0.7);
                colorBorderOpen = true;
            }
        }
        #endregion


        // Не реализовано
        #region Динамическое создание инструментов панелей
        /*private void AddModetPanelTools()
        {
            modeTileArray[0].Text = "Перемещение";
            modeTileArray[1].Text = "Связи";
            modeTileArray[2].Text = "Маршруты";
            modeTileArray[3].Text = "Холсты";

            foreach (ModeTile tile in modeTileArray)
            {
                ModePanel.Children.Add(tile);
            }
        }

        private void AddObjectPanelTools()
        {
            objectTileArray[0].Text = "Работник";
            objectTileArray[1].Text = "Золото";
            objectTileArray[2].Text = "Алмаз";
            objectTileArray[3].Text = "Станок";
            objectTileArray[4].Text = "Механизм";
            objectTileArray[5].Text = "Манипулятор";
            objectTileArray[6].Text = "Тележка";
            objectTileArray[7].Text = "Погрузчик";

            foreach (ObjectTile tile in objectTileArray)
            {
                ObjectPanel.Children.Add(tile);
            }
        }*/
        #endregion

    }
}










/* private void MyGrid_DragOver(object sender, DragEventArgs e) // Перемещение холста
{
    Point dropPosition = e.GetPosition(MyGrid);

    TargetBorder.Margin = new Thickness(dropPosition.X - TargetBorder.Width, 
        dropPosition.Y - TargetBorder.Height, 0, 0);
}

private void TargetCanvas_MouseMove(object sender, MouseEventArgs e)
{
    base.OnMouseMove(e);
    if (e.LeftButton == MouseButtonState.Pressed)
    {
        DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
    }
} */


/*private void ChooseIconsStyle()
{
    if (materialIconStyle)
    {

        materialIconStyle = false;
    }
    else
    {
        modeTileArray[0].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-hand-64.png", UriKind.Relative));
        modeTileArray[1].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-advance-64.png", UriKind.Relative));
        modeTileArray[2].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-squiggly-arrow-64.png", UriKind.Relative));
        modeTileArray[3].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-drag-64.png", UriKind.Relative));

        objectTileArray[0].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-worker-64.png", UriKind.Relative));
        objectTileArray[1].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-gold-bars-64.png", UriKind.Relative));
        objectTileArray[2].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-jewel64.png", UriKind.Relative));
        objectTileArray[3].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-hammer-and-anvil-64.png", UriKind.Relative));
        objectTileArray[4].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-gear-64.png", UriKind.Relative));
        objectTileArray[5].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-robot-64.png", UriKind.Relative));
        objectTileArray[6].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-mine-cart-64.png", UriKind.Relative));
        objectTileArray[7].Image = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-forl-lift-64.png", UriKind.Relative));

        materialIconStyle = true;
    }
}*/

