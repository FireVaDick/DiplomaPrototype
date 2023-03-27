using DiplomaProrotype.Animations;
using DiplomaProrotype.CanvasManipulation;
using DiplomaProrotype.ColorsManipulation;
using DiplomaProrotype.Models;
using DiplomaProrotype.ObjectsManipulation;
using DiplomaProrotype.Threads;
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

        static public ResourceTile resourceTileFromContextMenu;
        static public MachineTile machineTileFromContextMenu;
        static public MovableTile movableTileFromContextMenu;

        static public int tilesCounter = 0;
        static public int linksCounter = 0;
        static public string lastTileType = "";
        static public string currentMode = "path";

        static public bool resourceIsEmpty = false;


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
        #endregion

        //
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
                    ObjectPlacement.CreateResourceTile(dropPosition, resourceIsEmpty);
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
                                double firstMarginLeft = dropPosition.X + resourceData.Width / 2 - 20;
                                double firstMarginTop = dropPosition.Y + 2.5;

                                links[i].LineInfo.X1 = firstMarginLeft;
                                links[i].LineInfo.Y1 = firstMarginTop;

                                links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].LastTargetType == "resource" && links[i].LastTargetListId == resourceTiles.IndexOf(resourceData))
                            {
                                double lastMarginLeft = dropPosition.X - resourceData.Width / 2 + 25;
                                double lastMarginTop = dropPosition.Y + 2.5;

                                links[i].LineInfo.X2 = lastMarginLeft;
                                links[i].LineInfo.Y2 = lastMarginTop;

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
                    ObjectPlacement.CreateMachineTile(machineData, dropPosition);
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
                                double firstMarginLeft = dropPosition.X + machineData.Width / 2 - 10;
                                double firstMarginTop = dropPosition.Y + 2.5;

                                links[i].LineInfo.X1 = firstMarginLeft;
                                links[i].LineInfo.Y1 = firstMarginTop;

                                links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                            }
                        }

                        for (int i = 0; i < links.Count; i++)
                        {
                            if (links[i].LastTargetType == "machine" && links[i].LastTargetListId == machineTiles.IndexOf(machineData))
                            {
                                double lastMarginLeft = dropPosition.X - machineData.Width / 2;
                                double lastMarginTop = dropPosition.Y + 2.5;

                                switch (machineData.MachineText.Text)
                                {
                                    case "Токарный": lastMarginLeft += 18.5; break;
                                    case "Сварочный": lastMarginLeft += 16.5; break;
                                    case "Фрезерный": lastMarginLeft += 17.5; break;
                                }

                                links[i].LineInfo.X2 = lastMarginLeft;
                                links[i].LineInfo.Y2 = lastMarginTop;

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
                    ObjectPlacement.CreateMovableTile(movableData, dropPosition);
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


        #region Динамическая передача цвета последнему размещённому элементу
        private void ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorChooser.DynamicChooseColorFromPalette();
        }
        #endregion


        #region Очистка холста
        private void TargetCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.Z)
                {
                    Cleanup.UndoLastElementPlacement();
                }
            }
        }

        private void ModeTile_Undo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            Cleanup.UndoLastElementPlacement();
        }

        private void ModeTile_Erase_MouseDown(object sender, MouseButtonEventArgs e)
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

