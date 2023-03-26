using DiplomaProrotype.Animations;
using DiplomaProrotype.CanvasManipulation;
using DiplomaProrotype.Models;

using Haley.Models;
using Haley.Services;
using Haley.Utils;
using Haley.WPF.Controls;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;
using MenuItem = System.Windows.Controls.MenuItem;

namespace DiplomaProrotype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public List<ResourceTile> resourceTiles = new List<ResourceTile>();
        static public List<MachineTile> machineTiles = new List<MachineTile>();
        static public List<MovableTile> movableTiles = new List<MovableTile>();
        static public List<Link> links = new List<Link>();

        public ResourceTile resourceTileFromContextMenu;
        public MachineTile machineTileFromContextMenu;
        public MovableTile movableTileFromContextMenu;

        static public int tilesCounter = 0;
        static public int linksCounter = 0;
        static public string lastTileType = "";
        static public string currentMode = "path";

        public bool resourceIsEmpty = false;




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


        #region Размещение объекта (создание + перемещение)
        private void TargetCanvas_Drop(object sender, DragEventArgs e)
        {
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;
            var machineData = e.Data.GetData(typeof(MachineTile)) as MachineTile;
            var movableData = e.Data.GetData(typeof(MovableTile)) as MovableTile;

            Point dropPosition = e.GetPosition(TargetCanvas);

            // Манипуляции с resourceData
            if (!(resourceData is null))
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

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].FirstTargetType == "resource" && links[i].FirstTargetListId == resourceTiles.IndexOf(resourceData))
                            {
                                double marginLeftFromObject = dropPosition.X + resourceData.Width / 2 - 20;
                                double marginTopFromObject = dropPosition.Y + 2.5;

                                links[i].LineInfo.X1 = marginLeftFromObject;
                                links[i].LineInfo.Y1 = marginTopFromObject;

                                links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].LastTargetType == "resource" && links[i].LastTargetListId == resourceTiles.IndexOf(resourceData))
                            {
                                double marginLeftFromObject = dropPosition.X - resourceData.Width / 2 + 20;
                                double marginTopFromObject = dropPosition.Y + 2.5;

                                links[i].LineInfo.X2 = marginLeftFromObject;
                                links[i].LineInfo.Y2 = marginTopFromObject;

                                //links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }
                    }
                }
            }

            // Манипуляции с machineData
            if (!(machineData is null))
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

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].FirstTargetType == "machine" && links[i].FirstTargetListId == machineTiles.IndexOf(machineData))
                            {
                                double marginLeftFromObject = dropPosition.X + machineData.Width / 2 - 20;
                                double marginTopFromObject = dropPosition.Y + 2.5;

                                links[i].LineInfo.X1 = marginLeftFromObject;
                                links[i].LineInfo.Y1 = marginTopFromObject;

                                links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].LastTargetType == "machine" && links[i].LastTargetListId == machineTiles.IndexOf(machineData))
                            {
                                double marginLeftFromObject = dropPosition.X - machineData.Width / 2 + 20;
                                double marginTopFromObject = dropPosition.Y + 2.5;

                                links[i].LineInfo.X2 = marginLeftFromObject;
                                links[i].LineInfo.Y2 = marginTopFromObject;

                                //links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }
                    }
                }
            }

            // Манипуляции с movableData
            if (!(movableData is null))
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


        #region Создание экземпляров объектов
        private void CreateResourceTile(Point dropPosition, bool resourceIsEmpty)
        {
            var resourceTile = new ResourceTile();
            resourceTile.Text = "Задел";
            resourceTile.Id = ++tilesCounter;

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
            machineTile.Image = machineData.Image;
            machineTile.Text = machineData.Text;
            machineTile.Id = ++tilesCounter;

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
            movableTile.Image = movableData.Image;
            movableTile.Text = movableData.Text;
            movableTile.Id = ++tilesCounter;

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
        private Link link;
        private Ellipse linkCircle;
        private Line linkLine;

        private Line routeLine;
        private bool routeIsDone = false;

        private Rectangle pathRectangle;

        private Point startPos;
        double lastX2 = 0, lastY2 = 0;

        private void TargetCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPos = e.GetPosition(TargetCanvas);

            if (currentMode == "link")
            {
                if (e.Source is ResourceTile || e.Source is MachineTile)
                {
                    ResourceTile target = e.Source as ResourceTile;
                    Vector targetMargin = VisualTreeHelper.GetOffset(target);                  

                    for (int i = 0; i < resourceTiles.Count; i++)
                    {                       
                        if (targetMargin == VisualTreeHelper.GetOffset(resourceTiles[i]))
                        {
                            link = new Link()
                            {
                                FirstTargetType = "resource",
                                FirstTargetListId = i
                            };
                        }
                    }

                    EnableObjectsOrNot.SetAllObjectsToUnenabled(); // Для прохождения связи сквозь объект

                    linkLine = new Line
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };

                    double marginLeftFromObject = targetMargin.X + target.Width / 2 - target.Margin.Left / 2 + 20; // 20 и 5 - отступы от центра
                    double marginTopFromObject = targetMargin.Y + target.Height / 2 - target.Margin.Top - 5;

                    linkCircle = new Ellipse
                    {
                        Fill = Brushes.White,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black,
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness(marginLeftFromObject - 5,  marginTopFromObject - 5, 0, 0)
                    };

                    linkLine.X2 = linkLine.X1 = marginLeftFromObject; // Точки конца там же, где и начала
                    linkLine.Y2 = linkLine.Y1 = marginTopFromObject;

                    TargetCanvas.Children.Add(linkLine);
                    TargetCanvas.Children.Add(linkCircle);
                }            
            }

            if (currentMode == "route")
            {
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
            }

            if (currentMode == "path")
            {
                pathRectangle = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                Canvas.SetLeft(pathRectangle, startPos.X);
                Canvas.SetTop(pathRectangle, startPos.Y);

                TargetCanvas.Children.Add(pathRectangle);
            }
        }

        private void TargetCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentMode == "link" && !(linkLine is null))
            {
                if (e.LeftButton == MouseButtonState.Released || link == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                linkLine.X2 = pos.X;
                linkLine.Y2 = pos.Y;
            }

            if (currentMode == "route")
            {
                if (e.LeftButton == MouseButtonState.Released || routeLine == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                routeLine.X2 = pos.X;
                routeLine.Y2 = pos.Y;
            }

            if (currentMode == "path")
            {
                if (e.LeftButton == MouseButtonState.Released || pathRectangle == null)
                    return;

                var pos = e.GetPosition(TargetCanvas);

                var x = Math.Min(pos.X, startPos.X);
                var y = Math.Min(pos.Y, startPos.Y);

                var w = Math.Max(pos.X, startPos.X) - x;
                var h = Math.Max(pos.Y, startPos.Y) - y;

                pathRectangle.Width = w;
                pathRectangle.Height = h;

                Canvas.SetLeft(pathRectangle, x);
                Canvas.SetTop(pathRectangle, y);
            }
        }

        private void TargetCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentMode == "link")
            {
                Point mousePos = e.MouseDevice.GetPosition(TargetCanvas);
                Vector targetMargin; //targetMargin - положение объекта на холсте

                if (machineTiles.Count != 0)
                {
                    for (int i = 0; i < machineTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(machineTiles[i]);

                        if (mousePos.X > targetMargin.X &&
                            mousePos.X < targetMargin.X + machineTiles[i].Width && 
                            mousePos.Y > targetMargin.Y &&                     
                            mousePos.Y < targetMargin.Y + machineTiles[i].Height)
                        {
                            link.LastTargetType = "machine";
                            link.LastTargetListId = i;
                            link.LinkId = ++linksCounter;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            links.Add(link);

                            double marginLeftFromObject = targetMargin.X + machineTiles[i].Width / 2 - machineTiles[i].Margin.Left / 2;
                            double marginTopFromObject = targetMargin.Y + machineTiles[i].Height / 2 - machineTiles[i].Margin.Top + 5; // 5 - отступ от центра

                            switch (machineTiles[i].MachineText.Text) // Отступы в зависимости от картинки, чтобы было красиво и касалось
                            {
                                case "Токарный": marginLeftFromObject -= 15; break;
                                case "Сварочный": marginLeftFromObject -= 20; break;
                                case "Фрезерный": marginLeftFromObject -= 17.5; break;
                            }

                            linkLine.X2 = marginLeftFromObject;
                            linkLine.Y2 = marginTopFromObject;

                            linkLine = null;

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }
            }

            if (currentMode == "path")
            {
                pathRectangle = null;
            }
        }

        private void TargetCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentMode == "route")
            {
                routeIsDone = true;
                routeLine = null;
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
            resourceTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void CreateMachineContextMenu(MachineTile machineTile)
        {
            var contextMenu = new ContextMenu();
            machineTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void CreateMovableContextMenu(MovableTile movableTile)
        {
            var contextMenu = new ContextMenu();
            movableTile.ContextMenu = contextMenu;
            DefaultContextMenu(contextMenu);
        }

        private void DefaultContextMenu(ContextMenu contextMenu)
        {
            var menuItemCheckLinks = new MenuItem();
            Image imageCheckLinks = new Image();
            imageCheckLinks.Source = new BitmapImage(new Uri("/Icons/x128/Molecular32.png", UriKind.Relative));
            menuItemCheckLinks.Icon = imageCheckLinks;
            menuItemCheckLinks.Click += new RoutedEventHandler(CMCheckLinks_Click);
            menuItemCheckLinks.Header = "Просмотреть связи";

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

            contextMenu.Items.Add(menuItemCheckLinks);
            contextMenu.Items.Add(menuItemAnima);
            //contextMenu.Items.Add(menuItemEdit);
            contextMenu.Items.Add(menuItemColor);
            contextMenu.Items.Add(menuItemDelete);
        }
        #endregion


        #region Методы контекстного меню
        private void GetTileFromContextMenu(object sender, RoutedEventArgs e) // Для считывания
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            
            resourceTileFromContextMenu = contextMenu.PlacementTarget as ResourceTile;
            machineTileFromContextMenu = contextMenu.PlacementTarget as MachineTile;
            movableTileFromContextMenu = contextMenu.PlacementTarget as MovableTile;
        }

        private void ResourceHeightAnimation(int from, int to, int time, Thickness inputMargin, Thickness outputMargin)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);
            resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);

            thicknessAnimation.From = inputMargin;
            thicknessAnimation.To = outputMargin;
            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
        }

        private void CMCheckLinks_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);
            int tileLinkAmount = 0;
            int row = 0;

            string[] tableRows = new string[links.Count + 3];
            tableRows[row++] = $"Выбранный (1-ый) объект: задел";
            tableRows[row++] = $"";
            tableRows[row++] = $"{"Номер связи"}\t | " +
                $"{"Номер 1-го"}\t | " +
                $"{"Тип 2-го"}\t | " +
                $"{"Номер 2-го"} ";

            if(resourceTiles.Contains(resourceTileFromContextMenu))
            {
                for (int i = 0; i < links.Count; i++)
                {
                    if (links[i].FirstTargetListId == resourceTiles.IndexOf(resourceTileFromContextMenu))
                    {
                        tableRows[row++] = $"{++tileLinkAmount}\t\t | " +
                            $"{resourceTiles[links[i].FirstTargetListId].ResourceId.Text}\t\t | " +
                            //$"{links[i].LastTargetType,-25} | " +
                            $"{"Станок"}\t\t | " +
                            $"{machineTiles[links[i].LastTargetListId].MachineId.Text}";
                    }
                }
                string table = string.Join('\n', tableRows);

                MessageBox.Show(table);
            }    
        }

        private void CMAnimate_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (machineTileFromContextMenu is null && movableTileFromContextMenu is null)
            {
                if (resourceTileFromContextMenu.ResourceFigure.Height == 45)
                {
                    ResourceHeightAnimation(45, 10, 3, new(0, 5, 0, 0), new(0, 40, 0, 0));
                }
                else
                {
                    ResourceHeightAnimation(10, 45, 3, new (0, 40, 0, 0), new (0, 5, 0, 0));
                }
            }

            if (resourceTileFromContextMenu is null && movableTileFromContextMenu is null)
            {
                machineTileFromContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");

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

            if (resourceTiles.Contains(resourceTileFromContextMenu))
            {
                resourceTileFromContextMenu.ResourceFigure.Fill = dialog.SelectedBrush;
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

            if (resourceTiles.Contains(resourceTileFromContextMenu))
            {
                TargetCanvas.Children.Remove(resourceTileFromContextMenu);
                resourceTiles.Remove(resourceTileFromContextMenu);
            }

            if (machineTiles.Contains(machineTileFromContextMenu))
            {
                TargetCanvas.Children.Remove(machineTileFromContextMenu);
                machineTiles.Remove(machineTileFromContextMenu);
            }

            if (movableTiles.Contains(movableTileFromContextMenu))
            {
                TargetCanvas.Children.Remove(movableTileFromContextMenu);
                movableTiles.Remove(movableTileFromContextMenu);
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
            machineTileFromContextMenu.MachineProgress.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            machineTileFromContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#DC143C");
        }
        #endregion


        #region Очистка холста
        private void ModeTile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            Cleanup.UndoLastElementPlacement();
        }

        private void ModeTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cleanup.CreateCleanupContextMenu();
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

        private void ColorBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PanelAnimation.ColorBorderAnimation();
        }
        #endregion

    }
}

