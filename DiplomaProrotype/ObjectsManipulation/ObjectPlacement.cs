using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Haley.Models;
using System.Windows.Input;
using DiplomaProrotype.Models;
using System.Security.AccessControl;

namespace DiplomaProrotype.ObjectsManipulation
{
    internal class ObjectPlacement
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<MovableTile> movableTiles = MainWindow.movableTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;
        static private List<Link> links = MainWindow.links;


        static public void ObjectMovingFromPanelOrCanvas(DragEventArgs e)
        {
            var resourceData = e.Data.GetData(typeof(ResourceTile)) as ResourceTile;
            var machineData = e.Data.GetData(typeof(MachineTile)) as MachineTile;
            var movableData = e.Data.GetData(typeof(MovableTile)) as MovableTile;
            var stopData = e.Data.GetData(typeof(StopTile)) as StopTile;

            Point dropPosition = e.GetPosition(mw.TargetCanvas);

            // Манипуляции с resourceData
            if (!(resourceData is null))
            {
                if (resourceData.Parent is StackPanel)
                {
                    ObjectPlacement.CreateResourceTile(dropPosition, MainWindow.resourceNearMachineIsEmpty);
                }

                if (resourceData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (resourceTiles.Contains(resourceData))
                    {
                        MainWindow.chosenOneObject = resourceData;

                        Canvas.SetLeft(resourceData, dropPosition.X - resourceData.Width / 2 - 7.5);
                        Canvas.SetTop(resourceData, dropPosition.Y - resourceData.Height / 2 + 7.5);

                        ResourceLinkMoving(dropPosition, resourceData);
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

                if (machineData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (machineTiles.Contains(machineData))
                    {
                        MainWindow.chosenOneObject = machineData;

                        Canvas.SetLeft(machineData, dropPosition.X - machineData.Width / 2 - 7.5);
                        Canvas.SetTop(machineData, dropPosition.Y - machineData.Height / 2 - 2.5);

                        MachineLinkMoving(dropPosition, machineData);
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

                if (movableData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (movableTiles.Contains(movableData))
                    {
                        MainWindow.chosenOneObject = movableData;

                        Canvas.SetLeft(movableData, dropPosition.X - movableData.Width / 2 - 10);
                        Canvas.SetTop(movableData, dropPosition.Y - movableData.Height / 2 - 2.5);
                    }
                }
            }

            // Манипуляции с stopData
            if (!(stopData is null))
            {
                if (stopData.Parent is StackPanel)
                {
                    ObjectPlacement.CreateStopTile(stopData, dropPosition);
                }

                if (stopData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (stopTiles.Contains(stopData))
                    {
                        MainWindow.chosenOneObject = stopData;

                        Canvas.SetLeft(stopData, dropPosition.X - stopData.Width / 2 - 10);
                        Canvas.SetTop(stopData, dropPosition.Y - stopData.Height / 2 + 2.5);
                    }
                }
            }
        }


        static private void ChooseOneObjectByClick(object sender, MouseButtonEventArgs e)
        {
            Type type = e.Source.GetType();

            if (type == typeof(ResourceTile))
            {
                MainWindow.chosenOneObject = (ResourceTile)e.Source;
            }
            if (type == typeof(MachineTile))
            {
                MainWindow.chosenOneObject = (MachineTile)e.Source;
            }
            if (type == typeof(MovableTile))
            {
                MainWindow.chosenOneObject = (MovableTile)e.Source;
            }
            if (type == typeof(StopTile))
            {
                MainWindow.chosenOneObject = (StopTile)e.Source;
            }
        }


        static public void ResourceLinkMoving(Point dropPosition, ResourceTile resourceData)
        {
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

        static public void MachineLinkMoving(Point dropPosition, MachineTile machineData)
        {
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


        static public void CreateResourceTile(Point dropPosition, bool resourceIsEmpty)
        {
            var resourceTile = new ResourceTile();

            CreateResourceContextMenu(resourceTile);
            MainWindow.resourceTiles.Add(resourceTile);

            MainWindow.matrixResourceMachine = (int[,]) ResizeArray(MainWindow.matrixResourceMachine, new int[] {machineTiles.Count, resourceTiles.Count});

            resourceTile.Text = "Задел";
            resourceTile.Id = MainWindow.resourceTiles.Count;

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

            resourceTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
            MainWindow.chosenOneObject = resourceTile;
        }

        static public void CreateMachineTile(MachineTile machineData, Point dropPosition)
        {
            var machineTile = new MachineTile();

            CreateMachineContextMenu(machineTile);
            MainWindow.machineTiles.Add(machineTile);

            MainWindow.matrixResourceMachine = (int[,]) ResizeArray(MainWindow.matrixResourceMachine, new int[] { machineTiles.Count, resourceTiles.Count });

            machineTile.Image = machineData.Image;
            machineTile.Text = machineData.Text;
            machineTile.Id = MainWindow.machineTiles.Count;

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

            machineTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
            MainWindow.chosenOneObject = machineTile;


            MainWindow.resourceNearMachineIsEmpty = true;

            dropPosition.X += 70;
            CreateResourceTile(dropPosition, MainWindow.resourceNearMachineIsEmpty);

            MainWindow.resourceNearMachineIsEmpty = false;
        }

        static public void CreateMovableTile(MovableTile movableData, Point dropPosition)
        {
            var movableTile = new MovableTile();

            CreateMovableContextMenu(movableTile);
            MainWindow.movableTiles.Add(movableTile);

            movableTile.Image = movableData.Image;
            movableTile.Text = movableData.Text;
            movableTile.Id = MainWindow.movableTiles.Count;

            Canvas.SetLeft(movableTile, dropPosition.X - movableTile.Width / 2 - 10);
            Canvas.SetTop(movableTile, dropPosition.Y - movableTile.Height / 2 - 22.5);

            movableTile.MovableImage.Margin = new Thickness(0, 25, 0, 0);
            movableTile.MovableText.Margin = new Thickness(0, 0, 0, 15);

            movableTile.MovableIndicator.Visibility = Visibility.Visible;
            movableTile.MovableId.Visibility = Visibility.Visible;
            movableTile.Height = 110;

            MainWindow.lastTileType = "movable";
            mw.TargetCanvas.Children.Add(movableTile);

            movableTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
            MainWindow.chosenOneObject = movableTile;
        }


        static public void CreateStopTile(StopTile stopData, Point dropPosition)
        {
            var stopTile = new StopTile();

            MainWindow.stopTiles.Add(stopTile);

            stopTile.Text = "Стоянка";
            stopTile.Id = MainWindow.stopTiles.Count;

            Canvas.SetLeft(stopTile, dropPosition.X - stopTile.Width / 2 - 10);
            Canvas.SetTop(stopTile, dropPosition.Y - stopTile.Height / 2 - 5);

            stopTile.StopText.Margin = new Thickness(0, 0, 0, 15);
            stopTile.StopId.Visibility = Visibility.Visible;
            stopTile.Height = 70;

            MainWindow.lastTileType = "stop";
            mw.TargetCanvas.Children.Add(stopTile);

            stopTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
            MainWindow.chosenOneObject = stopTile;
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


        public static Array ResizeArray(Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("Arr must have the same number of dimensions as there are elements in newSizes", "newSizes");

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }
    }
}
