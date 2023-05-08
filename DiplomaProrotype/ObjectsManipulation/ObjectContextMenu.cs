using DiplomaProrotype.Animations;
using DiplomaProrotype.Models;
using DiplomaProrotype.Threads;
using DiplomaPrototype;
using Haley.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace DiplomaProrotype.ObjectsManipulation
{
    internal class ObjectContextMenu
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<MovableTile> movableTiles = MainWindow.movableTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;
        static private List<Link> linksResourceMachine = MainWindow.linksResourceMachine;

        static private ResourceTile resourceTileFromContextMenu = MainWindow.resourceTileFromContextMenu;
        static private MachineTile machineTileFromContextMenu = MainWindow.machineTileFromContextMenu;
        static private MovableTile movableTileFromContextMenu = MainWindow.movableTileFromContextMenu;
        static private StopTile stopTileFromContextMenu = MainWindow.stopTileFromContextMenu;


        static public void CreateDefaultContextMenu(ContextMenu contextMenu)
        {
            var menuItemCheckLinks = new MenuItem();
            Image imageCheckLinks = new Image();
            imageCheckLinks.Source = new BitmapImage(new Uri("/Icons/x128/Molecular32.png", UriKind.Relative));
            menuItemCheckLinks.Icon = imageCheckLinks;
            menuItemCheckLinks.Click += new RoutedEventHandler(CMCheckLinks_Click);
            menuItemCheckLinks.Header = "Просмотреть связи";

            var menuItemAnimate = new MenuItem();
            Image imageAnimate = new Image();
            imageAnimate.Source = new BitmapImage(new Uri("/Icons/x128/Animate32.png", UriKind.Relative));
            menuItemAnimate.Icon = imageAnimate;
            menuItemAnimate.Click += new RoutedEventHandler(CMAnimate_Click);
            menuItemAnimate.Header = "Анимировать";

            var menuItemEdit = new MenuItem();
            Image imageEdit = new Image();
            imageEdit.Source = new BitmapImage(new Uri("/Icons/x128/Pencil32.png", UriKind.Relative));
            menuItemEdit.Icon = imageEdit;
            menuItemEdit.Click += new RoutedEventHandler(CMEdit_Click);
            menuItemEdit.Header = "Редактировать";

            var menuItemChangeColor = new MenuItem();
            Image imageChangeColor = new Image();
            imageChangeColor.Source = new BitmapImage(new Uri("/Icons/x128/Brush32.png", UriKind.Relative));
            menuItemChangeColor.Icon = imageChangeColor;
            menuItemChangeColor.Click += new RoutedEventHandler(CMChangeColor_Click);
            menuItemChangeColor.Header = "Выбрать цвет";

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
            contextMenu.Items.Add(menuItemAnimate);
            contextMenu.Items.Add(menuItemEdit);
            contextMenu.Items.Add(menuItemChangeColor);
            contextMenu.Items.Add(menuItemDelete);
        }


        static private void GetTileFromContextMenu(object sender, RoutedEventArgs e) // Для считывания
        {
            var menuItem = sender as MenuItem;
            while (menuItem.Parent is MenuItem)
            {
                menuItem = (MenuItem)menuItem.Parent;
            }
            var contextMenu = menuItem.Parent as ContextMenu;

            resourceTileFromContextMenu = MainWindow.resourceTileFromContextMenu = contextMenu.PlacementTarget as ResourceTile;
            machineTileFromContextMenu = MainWindow.machineTileFromContextMenu = contextMenu.PlacementTarget as MachineTile;
            movableTileFromContextMenu = MainWindow.movableTileFromContextMenu = contextMenu.PlacementTarget as MovableTile;
            stopTileFromContextMenu = MainWindow.stopTileFromContextMenu = contextMenu.PlacementTarget as StopTile;
        }


        static private void CMCheckLinks_Click(object sender, RoutedEventArgs e) //!!!!!!!!!!!!!!!!!!!!!
        {
            GetTileFromContextMenu(sender, e);
            int tileLinkAmount = 0;
            int row = 4;

            if (resourceTiles.Contains(resourceTileFromContextMenu))
            {
                string[] tableRows = new string[linksResourceMachine.Count + 4];
                tableRows[0] = $"Тип первого объекта: " + $"{resourceTileFromContextMenu.ResourceText.Text}";
                tableRows[1] = $"Номер первого объекта: " + $"{resourceTileFromContextMenu.ResourceId.Text}";

                for (int i = 0; i < linksResourceMachine.Count; i++)
                {
                    if (linksResourceMachine[i].FirstTargetListId == resourceTiles.IndexOf(resourceTileFromContextMenu))
                    {
                        tableRows[2] = $"";
                        tableRows[3] = $"{"Номер связи"}\t | " +
                        $"{"Тип второго"}\t | " +
                        $"{"Номер второго"} ";

                        tableRows[row++] = $"{++tileLinkAmount}\t\t | " +
                            $"{machineTiles[linksResourceMachine[i].LastTargetListId].MachineText.Text}\t | " +
                            $"{machineTiles[linksResourceMachine[i].LastTargetListId].MachineId.Text}";
                    }
                }
                string table = string.Join('\n', tableRows);

                MessageBox.Show(table);
            }

            if (machineTiles.Contains(machineTileFromContextMenu))
            {
                string[] tableRows = new string[linksResourceMachine.Count + 4];
                tableRows[0] = $"Тип первого объекта: " + $"{machineTileFromContextMenu.MachineText.Text}";
                tableRows[1] = $"Номер первого объекта: " + $"{machineTileFromContextMenu.MachineId.Text}";

                for (int i = 0; i < linksResourceMachine.Count; i++)
                {
                    if (linksResourceMachine[i].FirstTargetListId == machineTiles.IndexOf(machineTileFromContextMenu))
                    {
                        tableRows[2] = $"";
                        tableRows[3] = $"{"Номер связи"}\t | " +
                        $"{"Тип второго"}\t | " +
                        $"{"Номер второго"} ";

                        tableRows[row++] = $"{++tileLinkAmount}\t\t | " +
                            $"{resourceTiles[linksResourceMachine[i].LastTargetListId].ResourceText.Text}\t\t | " +
                            $"{resourceTiles[linksResourceMachine[i].LastTargetListId].ResourceId.Text}";
                    }
                }
                string table = string.Join('\n', tableRows);

                MessageBox.Show(table);
            }
        }

        static private void CMAnimate_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (!(resourceTileFromContextMenu is null))
            {
                if (resourceTileFromContextMenu.ResourceFigure.Height == 45)
                {
                    ResourceAnimation.ResourceHeightAnimation(45, 10, 3, new(0, 5, 0, 0), new(0, 40, 0, 0));
                }
                else
                {
                    ResourceAnimation.ResourceHeightAnimation(10, 45, 3, new(0, 40, 0, 0), new(0, 5, 0, 0));
                }
            }

            if (!(machineTileFromContextMenu is null))
            {
                machineTileFromContextMenu.MachineIndicator.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += BackgroundWorkerThread.worker_DoWork;
                worker.ProgressChanged += BackgroundWorkerThread.worker_ProgressChanged;
                worker.RunWorkerCompleted += BackgroundWorkerThread.worker_RunWorkerCompleted;

                worker.RunWorkerAsync();
            }

            if (!(movableTileFromContextMenu is null))
            {
                if (movableTileFromContextMenu.ResourceFigure1.Height == 25)
                {
                    ResourceAnimation.ResourceOnMovableHeightAnimation(25, 6, 3, -35, -15);
                }
                else
                {
                    ResourceAnimation.ResourceOnMovableHeightAnimation(6, 25, 3, -15, -35);
                }
            }
        }

        static private void CMEdit_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (stopTiles.Contains(stopTileFromContextMenu))
            {
                SelectWindow.CreateStopSelectWindow();
                if (SelectWindow.currentNumber > 0 && SelectWindow.currentWord != "")
                {
                    if (stopTileFromContextMenu.Text == "Погрузка") MainWindow.amountLoading--;
                    if (stopTileFromContextMenu.Text == "Разгрузка") MainWindow.amountUnloading--;

                    stopTileFromContextMenu.Chain = SelectWindow.currentNumber;
                    stopTileFromContextMenu.Text = SelectWindow.currentWord;

                    if (stopTileFromContextMenu.Text == "Погрузка") MainWindow.amountLoading++;
                    if (stopTileFromContextMenu.Text == "Разгрузка") MainWindow.amountUnloading++;
                }
            }
        }

        static private void CMChangeColor_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            var dialog = new ColorPickerDialog();
            dialog.ShowDialog();

            if (resourceTiles.Contains(resourceTileFromContextMenu))
            {
                resourceTileFromContextMenu.ResourceFigure.Fill = dialog.SelectedBrush;

                for (int i = 0; i < linksResourceMachine.Count; i++)
                {
                    if (linksResourceMachine[i].FirstTargetType == "resource" && linksResourceMachine[i].FirstTargetListId == resourceTiles.IndexOf(resourceTileFromContextMenu))
                    {
                        linksResourceMachine[i].LineInfo.Stroke = dialog.SelectedBrush;
                    }

                    if (linksResourceMachine[i].LastTargetType == "resource" && linksResourceMachine[i].LastTargetListId == resourceTiles.IndexOf(resourceTileFromContextMenu))
                    {
                        linksResourceMachine[i].LineInfo.Stroke = dialog.SelectedBrush;
                    }
                }
            }
        }

        static private void CMDelete_Click(object sender, RoutedEventArgs e)
        {
            GetTileFromContextMenu(sender, e);

            if (resourceTiles.Contains(resourceTileFromContextMenu))
            {
                mw.TargetCanvas.Children.Remove(resourceTileFromContextMenu);
                resourceTiles.Remove(resourceTileFromContextMenu);
            }

            if (machineTiles.Contains(machineTileFromContextMenu))
            {
                mw.TargetCanvas.Children.Remove(machineTileFromContextMenu);
                machineTiles.Remove(machineTileFromContextMenu);
            }

            if (movableTiles.Contains(movableTileFromContextMenu))
            {
                mw.TargetCanvas.Children.Remove(movableTileFromContextMenu);
                movableTiles.Remove(movableTileFromContextMenu);
            }

            if (stopTiles.Contains(stopTileFromContextMenu))
            {
                mw.TargetCanvas.Children.Remove(stopTileFromContextMenu);
                stopTiles.Remove(stopTileFromContextMenu);
            }

            MainWindow.matrixResourceMachine = ObjectPlacement.ResizeArray(MainWindow.matrixResourceMachine, machineTiles.Count + 1, resourceTiles.Count + 1);
            MainWindow.matrixResourcePlaceStop = ObjectPlacement.ResizeArray(MainWindow.matrixResourcePlaceStop, stopTiles.Count + 1, resourceTiles.Count + MainWindow.amountPlaces + 1);
            
        }
    }
}
