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

        static public int linksCounter = 0;
        static public string lastTileType = "";
        static public string currentMode = "move";
        static public bool resourceNearMachineIsEmpty = false;


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


        #region Просмотр всех связей
        private void ModeTile_CheckAllLinks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
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

