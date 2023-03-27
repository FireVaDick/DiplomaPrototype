using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DiplomaProrotype.ObjectsManipulation
{
    internal class ObjectPlacement
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;


        static public void CreateResourceTile(Point dropPosition, bool resourceIsEmpty)
        {
            var resourceTile = new ResourceTile();
            resourceTile.Text = "Задел";
            resourceTile.Id = ++MainWindow.tilesCounter;

            CreateResourceContextMenu(resourceTile);
            MainWindow.resourceTiles.Add(resourceTile);

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

            MainWindow.lastTileType = "resource";
            mw.TargetCanvas.Children.Add(resourceTile);
        }

        static public void CreateMachineTile(MachineTile machineData, Point dropPosition)
        {
            var machineTile = new MachineTile();
            machineTile.Image = machineData.Image;
            machineTile.Text = machineData.Text;
            machineTile.Id = ++MainWindow.tilesCounter;

            CreateMachineContextMenu(machineTile);
            MainWindow.machineTiles.Add(machineTile);

            Canvas.SetLeft(machineTile, dropPosition.X - machineTile.Width / 2 - 7.5);
            Canvas.SetTop(machineTile, dropPosition.Y - machineTile.Height / 2 - 25);

            machineTile.MachineImage.Margin = new Thickness(0, 25, 0, 0);
            machineTile.MachineProgress.Margin = new Thickness(10, 0, 10, 30);
            machineTile.MachineText.Margin = new Thickness(0, 0, 0, 15);

            machineTile.MachineIndicator.Visibility = Visibility.Visible;
            machineTile.MachineProgress.Visibility = Visibility.Visible;
            machineTile.MachineId.Visibility = Visibility.Visible;
            machineTile.Height = 115;

            MainWindow.lastTileType = "machine";
            mw.TargetCanvas.Children.Add(machineTile);

            MainWindow.resourceIsEmpty = true;

            dropPosition.X += 70;
            CreateResourceTile(dropPosition, MainWindow.resourceIsEmpty);

            MainWindow.resourceIsEmpty = false;
        }

        static public void CreateMovableTile(MovableTile movableData, Point dropPosition)
        {
            var movableTile = new MovableTile();
            movableTile.Image = movableData.Image;
            movableTile.Text = movableData.Text;
            movableTile.Id = ++MainWindow.tilesCounter;

            CreateMovableContextMenu(movableTile);
            MainWindow.movableTiles.Add(movableTile);

            Canvas.SetLeft(movableTile, dropPosition.X - movableTile.Width / 2 - 10);
            Canvas.SetTop(movableTile, dropPosition.Y - movableTile.Height / 2 - 22.5);

            movableTile.MovableImage.Margin = new Thickness(0, 25, 0, 0);
            movableTile.MovableText.Margin = new Thickness(0, 0, 0, 15);

            movableTile.MovableIndicator.Visibility = Visibility.Visible;
            movableTile.MovableId.Visibility = Visibility.Visible;
            movableTile.Height = 110;

            MainWindow.lastTileType = "movable";
            mw.TargetCanvas.Children.Add(movableTile);
        }

        static private void CreateResourceContextMenu(ResourceTile resourceTile)
        {
            var contextMenu = new ContextMenu();
            resourceTile.ContextMenu = contextMenu;
            ObjectContextMenu.CreateDefaultContextMenu(contextMenu);
        }

        static private void CreateMachineContextMenu(MachineTile machineTile)
        {
            var contextMenu = new ContextMenu();
            machineTile.ContextMenu = contextMenu;
            ObjectContextMenu.CreateDefaultContextMenu(contextMenu);
        }

        static private void CreateMovableContextMenu(MovableTile movableTile)
        {
            var contextMenu = new ContextMenu();
            movableTile.ContextMenu = contextMenu;
            ObjectContextMenu.CreateDefaultContextMenu(contextMenu);
        }
    }
}
