using Haley.Services;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace DiplomaProrotype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ObjectTile> tiles = new List<ObjectTile>();
        ModeTile[] modeTileArray = new ModeTile[4];
        ObjectTile[] objectTileArray = new ObjectTile[7];

        private bool materialIconStyle = true;
        private int tilesCounter = 0;

        private bool modeBorderOpen = true;
        private bool objectBorderOpen = true;
        private bool colorBorderOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += Window_Loaded;  

            //AddModePanelTools();
            //AddObjectPanelTools();
            //ChooseIconsStyle();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(MyGrid).Add(new ResizableAdorner(TargetBorder));
        }


        #region Перемещение объекта
        private void TargetCanvas_Drop(object sender, DragEventArgs e)
        {
            var objectData = e.Data.GetData(typeof(ObjectTile)) as ObjectTile;

            Point dropPosition = e.GetPosition(TargetCanvas);

            if (objectData.Parent is StackPanel) // Возможная проверка objectData is UIElement element
            {
                CreateObjectTile(objectData, dropPosition);
            }

            if (objectData.Parent is Canvas)
            {
                if (tiles.Contains(objectData))
                {
                    Canvas.SetLeft(objectData, dropPosition.X - objectData.Width / 2 - 5);
                    Canvas.SetTop(objectData, dropPosition.Y - objectData.Height / 2 + 5);
                }
            }
        }

        private void TargetCanvas_DragOver(object sender, DragEventArgs e)
        { }

        private void ObjectPanel_Drop(object sender, DragEventArgs e)
        {
            var objectData = e.Data.GetData(typeof(ObjectTile)) as ObjectTile;

            if (objectData.Parent is StackPanel == false)
            {
                ((Canvas)(objectData.Parent)).Children.Remove(objectData);
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


        #region Создание экземпляра объекта
        private void CreateObjectTile(ObjectTile objectData, Point dropPosition)
        {
            var objectTile = new ObjectTile();
            tilesCounter++;

            objectTile.Image = objectData.Image;
            objectTile.Text = objectData.Text;
            objectTile.Id = tilesCounter;

            // Добавление событий
            // displayTile.MouseRightButtonDown += new MouseButtonEventHandler(DeleteDisplayTile_MouseRightButtonDown);
            CreateContextMenu(objectTile);

            tiles.Add(objectTile);

            Canvas.SetLeft(objectTile, dropPosition.X - objectTile.Width / 2 - 5);
            Canvas.SetTop(objectTile, dropPosition.Y - objectTile.Height / 2 + 5);

            objectTile.ObjectText.Margin = new Thickness(0, 0, 0, 15);
            objectTile.ObjectId.Visibility = Visibility.Visible;
            objectTile.Height = 85;
            

            TargetCanvas.Children.Add(objectTile);
        }
        #endregion 


        #region Динамическая передача цвета последнему размещённому элементу
        private void ColorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tiles.Count != 0)
            {
                tiles[tiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }

        }
        #endregion


        #region Анимации сокрытия
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

            for(int i = 0; i < tiles.Count;)
            {           
                TargetCanvas.Children.Remove(tiles[i]);
                tiles.Remove(tiles[i]);
            }
        }
        #endregion


        #region Контекстное меню
        private void CreateContextMenu(ObjectTile objectTile)
        {
            var contextmenu = new ContextMenu();
            var sep = new Separator(); // Разделитель

            objectTile.ContextMenu = contextmenu;

            var menuItemEdit = new MenuItem();
            Image imageEdit = new Image();
            imageEdit.Source = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-pencil-64.png", UriKind.Relative));
            menuItemEdit.Icon = imageEdit;
            menuItemEdit.Header = "Редактировать";

            var menuItemColor = new MenuItem();
            Image imageColor = new Image();
            imageColor.Source = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-paint-brush-64.png", UriKind.Relative));
            menuItemColor.Icon = imageColor;
            menuItemColor.Click += new RoutedEventHandler(CMChangeColor_Click);
            menuItemColor.Header = "Выбрать цвет";

            var menuItemDelete = new MenuItem();
            Image imageDelete = new Image();
            imageDelete.Source = new BitmapImage(new Uri("/Icons/BlackOutline/icons8-delete-64.png", UriKind.Relative));
            menuItemDelete.Icon = imageDelete;
            menuItemDelete.Click += new RoutedEventHandler(CMDelete_Click);
            menuItemDelete.Header = "Удалить";

            var menuItemEditSomething = new MenuItem();
            menuItemEditSomething.Header = "Настроить что-то";
            menuItemEdit.Items.Add(menuItemEditSomething);

            contextmenu.Items.Add(menuItemEdit);
            contextmenu.Items.Add(menuItemColor);
            contextmenu.Items.Add(menuItemDelete);
        }
        #endregion 


        // Реализовано не до конца
        #region Методы контекстного меню
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
            var objectTile = contextMenu.PlacementTarget as ObjectTile;

            var dialog = new ColorPickerDialog();
            dialog.ShowDialog();
            objectTile.Background = dialog.SelectedBrush;
        }

        private void CMDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;
            var objectTile = contextMenu.PlacementTarget as ObjectTile;

            if (tiles.Contains(objectTile))
            {
                TargetCanvas.Children.Remove(objectTile);
                tiles.Remove(objectTile);
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


        // Не реализовано
        #region Выбор стиля иконок
        private void ChooseIconsStyle()
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
        }



        #endregion
    }
}










/*private void DeleteDisplayTile_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
{
    var displayTile = sender as ObjectTile;

    if (tiles.Contains(displayTile))
    {
        tiles.Remove(displayTile);
        TargetCanvas.Children.Remove(displayTile);
    }
}*/


