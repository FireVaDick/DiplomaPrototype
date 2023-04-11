using Haley.Models;
using Haley.Services;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        List<ObjectTile> objectTiles = new List<ObjectTile>();
        List<ResourceTile> resourceTiles = new List<ResourceTile>();
        ModeTile[] modeTileArray = new ModeTile[4];
        ObjectTile[] objectTileArray = new ObjectTile[6];

        private bool materialIconStyle = true;
        private int tilesCounter = 0;
        private string lastTileType = "";

        private bool modeBorderOpen = true;
        private bool objectBorderOpen = true;
        private bool colorBorderOpen = false;


        //поля для анимации
        private Line routeLine;
        private bool routeIsDone = false;
        private Point startPos;
        double lastX2 = 0, lastY2 = 0;

        List<Line> animationLineList = new List<Line>();
        //
        private ObjectTile objectTileContextMenu;
        private ResourceTile resourceTileContextMenu;

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

            coordinates.Add(new Point(0, 0));
        }


        #region Перемещение объекта
        private void TargetCanvas_Drop(object sender, DragEventArgs e)
        {
            var objectData = e.Data.GetData(typeof(ObjectTile)) as ObjectTile;
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;

            Point dropPosition = e.GetPosition(TargetCanvas);

            // Манипуляции с objectData
            if (resourceData is null) 
            {
                if (objectData.Parent is StackPanel)
                {
                    CreateObjectTile(objectData, dropPosition);
                }

                if (objectData.Parent is Canvas)
                {
                    if (objectTiles.Contains(objectData))
                    {
                        Canvas.SetLeft(objectData, dropPosition.X - objectData.Width / 2 - 7.5);
                        Canvas.SetTop(objectData, dropPosition.Y - objectData.Height / 2 - 2.5);
                    }
                }
            }

            // Манипуляции с resourceData
            else if (objectData is null)
            {
                if (resourceData.Parent is StackPanel)
                {
                    CreateResourceTile(dropPosition);
                }

                if (resourceData.Parent is Canvas)
                {
                    if (resourceTiles.Contains(resourceData))
                    {
                        Canvas.SetLeft(resourceData, dropPosition.X - resourceData.Width / 2 - 7.5);
                        Canvas.SetTop(resourceData, dropPosition.Y - resourceData.Height / 2 + 7.5);
                    }
                }
            }
        }

        private void TargetCanvas_DragOver(object sender, DragEventArgs e)
        { }

        private void ObjectPanel_Drop(object sender, DragEventArgs e)
        {
            var objectData = e.Data.GetData(typeof(ObjectTile)) as ObjectTile;
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;

            if (!(objectData is null) && objectData.Parent is StackPanel == false)
            {
                ((Canvas)(objectData.Parent)).Children.Remove(objectData);
            }

            if (!(resourceData is null) && resourceData.Parent is StackPanel == false)
            {
                ((Canvas)(resourceData.Parent)).Children.Remove(resourceData);
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

        #endregion


        #region Создание экземпляров
        private void CreateObjectTile(ObjectTile objectData, Point dropPosition)
        {
            var objectTile = new ObjectTile();
            tilesCounter++;

            objectTile.Image = objectData.Image;
            objectTile.Text = objectData.Text;
            objectTile.Id = tilesCounter;

            CreateObjectContextMenu(objectTile);
            objectTiles.Add(objectTile);

            Canvas.SetLeft(objectTile, dropPosition.X - objectTile.Width / 2 - 7.5);
            Canvas.SetTop(objectTile, dropPosition.Y - objectTile.Height / 2 - 25);

          //при добавлении объекта на холст к нему будет рисоваться линия и его координаты добавляются в список для маршрута
            
            Line routeLine = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

            routeLine.X1 = coordinates[coordinates.Count - 1].X;
            routeLine.Y1 = coordinates[coordinates.Count - 1].Y;
            routeLine.X2 = dropPosition.X;
            routeLine.Y2 = dropPosition.Y;

            coordinates.Add(dropPosition);
           
            TargetCanvas.Children.Add(routeLine);
            animationLineList.Add(routeLine);
            //

            objectTile.ObjectImage.Margin = new Thickness(0, 25, 0, 0);          
            objectTile.ObjectProgress.Margin = new Thickness(10, 0, 10, 30);
            objectTile.ObjectText.Margin = new Thickness(0, 0, 0, 15);

            objectTile.ObjectIndicator.Visibility = Visibility.Visible;
            objectTile.ObjectProgress.Visibility = Visibility.Visible;
            objectTile.ObjectId.Visibility = Visibility.Visible;
            objectTile.Height = 115;

            lastTileType = "object";
            TargetCanvas.Children.Add(objectTile);
            
            if (objectTile.Text != "Сотрудник" && objectTile.Text != "Тележка" && objectTile.Text != "Погрузчик") // Добавление задела после каждого станка
            {
                dropPosition.X += 70;
                CreateResourceTile(dropPosition);
            }
        }

        private void CreateResourceTile(Point dropPosition)
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

            lastTileType = "resource";
            TargetCanvas.Children.Add(resourceTile);
        }
        #endregion 


        #region Динамическая передача цвета последнему размещённому элементу
        private void ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*if (lastTileType == "object")
            {
                objectTiles[objectTiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }*/

            if (lastTileType == "resource")
            {
                resourceTiles[resourceTiles.Count - 1].ResourceFigure.Fill = ColorPalette.SelectedBrush;
            }
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


        #region Очистка всего холста
        private void ModeTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tilesCounter = 0;

            for(int i = 0; i < objectTiles.Count;)
            {           
                TargetCanvas.Children.Remove(objectTiles[i]);
                objectTiles.Remove(objectTiles[i]);
            }

            for (int i = 0; i < resourceTiles.Count;)
            {
                TargetCanvas.Children.Remove(resourceTiles[i]);
                resourceTiles.Remove(resourceTiles[i]);
            }
        }
        #endregion


        #region Контекстное меню
        private void CreateObjectContextMenu(ObjectTile objectTile)
        {
            var contextmenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            objectTile.ContextMenu = contextmenu;

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

            var menuItemEditSomething = new MenuItem();
            menuItemEditSomething.Header = "Настроить что-то";
            menuItemEdit.Items.Add(menuItemEditSomething);

            contextmenu.Items.Add(menuItemAnima);
            contextmenu.Items.Add(menuItemEdit);
            contextmenu.Items.Add(menuItemColor);
            contextmenu.Items.Add(menuItemDelete);
        }

        private void CreateResourceContextMenu(ResourceTile resourceTile)
        {
            var contextmenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            resourceTile.ContextMenu = contextmenu;

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

            var menuItemEditSomething = new MenuItem();
            menuItemEditSomething.Header = "Настроить что-то";
            menuItemEdit.Items.Add(menuItemEditSomething);

            contextmenu.Items.Add(menuItemAnima);
            contextmenu.Items.Add(menuItemEdit);
            contextmenu.Items.Add(menuItemColor);
            contextmenu.Items.Add(menuItemDelete);
        }
        #endregion


        #region Методы контекстного меню


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
            objectTileContextMenu.ObjectProgress.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            objectTileContextMenu.ObjectIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#DC143C");
        }

        private void CMAnimate_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            objectTileContextMenu = contextMenu.PlacementTarget as ObjectTile;
            resourceTileContextMenu = contextMenu.PlacementTarget as ResourceTile;

            if (resourceTileContextMenu is null)
            {
                objectTileContextMenu.ObjectIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                worker.RunWorkerAsync();
            }

            if (objectTileContextMenu is null)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

                if (resourceTileContextMenu.ResourceFigure.Height == 45)
                {
                    
                    doubleAnimation.From = 45;
                    doubleAnimation.To = 10;
                    doubleAnimation.Duration = TimeSpan.FromSeconds(3);
                    resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);

                    thicknessAnimation.From = new Thickness(0, 5, 0, 0);
                    thicknessAnimation.To = new Thickness(0, 40, 0, 0);
                    thicknessAnimation.Duration = TimeSpan.FromSeconds(3);
                    resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
                }
                else
                {
                    doubleAnimation.From = 10;
                    doubleAnimation.To = 45;
                    doubleAnimation.Duration = TimeSpan.FromSeconds(3);
                    resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);
                   
                    thicknessAnimation.From = new Thickness(0, 40, 0, 0);
                    thicknessAnimation.To = new Thickness(0, 5, 0, 0);
                    thicknessAnimation.Duration = TimeSpan.FromSeconds(3);
                    resourceTileContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
                }
            }
        }

        private void CMCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CMChangeColor_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            objectTileContextMenu = contextMenu.PlacementTarget as ObjectTile;
            resourceTileContextMenu = contextMenu.PlacementTarget as ResourceTile;

            var dialog = new ColorPickerDialog();
            dialog.ShowDialog();

            /*if (objectTiles.Contains(objectTileContextMenu))
            {
                objectTileContextMenu.Background = dialog.SelectedBrush;
            }*/

            if (resourceTiles.Contains(resourceTileContextMenu))
            {
                resourceTileContextMenu.ResourceFigure.Fill = dialog.SelectedBrush;
            }
        }

        private void CMDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            objectTileContextMenu = contextMenu.PlacementTarget as ObjectTile;
            resourceTileContextMenu = contextMenu.PlacementTarget as ResourceTile;

            if (objectTiles.Contains(objectTileContextMenu))
            {
                TargetCanvas.Children.Remove(objectTileContextMenu);
                objectTiles.Remove(objectTileContextMenu);
            }

            if (resourceTiles.Contains(resourceTileContextMenu))
            {
                TargetCanvas.Children.Remove(resourceTileContextMenu);
                resourceTiles.Remove(resourceTileContextMenu);
            }
        }
        #endregion


        // Не реализовано
        #region Динамическое создание инструментов панелей
        private void AddModetPanelTools()
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
        }
        #endregion

        #region Анимация объекта на холсте

        private List<Point> coordinates = new List<Point>();
        public void Animation()
        {
            List<Point> points = coordinates; //Список координат по которым будет двигаться объект


            PathFigure pFigure = new PathFigure(); //Создание фигуры,описывающей движение анимации
            pFigure.StartPoint = new Point(35, 0); //Стартовая точка анимации

            for (int i = 0; i < points.Count; i++)
            {

                int X = (int)points[i].X;
                int Y = (int)points[i].Y;
                LineSegment lineSegment = new() //Создание линии с координатами конечной точки
                {
                    Point = new Point(X, Y)
                };
                pFigure.Segments.Add(lineSegment); //Добавление линии к фигуре,описывающей движение
            }


            TargetPathGeometry.Figures.Clear(); //Очистка массива фигур на случай предыдущих данных

            TargetPathGeometry.Figures.Add(pFigure); //Добавление фигуры в качестве пути движения

            TargetPathGeometry.Figures.Clear();

            TargetPathGeometry.Figures.Add(pFigure);
            TargetStoryboard.Begin(this);

        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPos = e.GetPosition(TargetCanvas);

            if (!(routeLine is null))
            {
                lastX2 = routeLine.X2;
                lastY2 = routeLine.Y2;
            }

            routeLine = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };

            if (lastX2 == 0 && lastY2 == 0)
            {
                lastX2 = startPos.X;
                lastY2 = startPos.Y;
            }

            if (routeIsDone == false)
            {
                routeLine.X1 = lastX2;
                routeLine.Y1 = lastY2;
            }

            routeLine.X2 = routeLine.X1;
            routeLine.Y2 = routeLine.Y1;

            routeIsDone = false;
            TargetCanvas.Children.Add(routeLine);

            Point point = e.GetPosition(TargetCanvas);
            coordinates.Add(point);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (routeLine == null)
                return;

            var pos = e.GetPosition(TargetCanvas);

            routeLine.X2 = pos.X;
            routeLine.Y2 = pos.Y;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            routeIsDone = true;
            routeLine = null;

            // Здесь происходит завершение работы и возвращение массива координат
            coordinates.Add(e.GetPosition(TargetCanvas));

            Animation();

        }
        #endregion
    }
}













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

