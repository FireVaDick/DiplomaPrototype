using DiplomaProrotype.Models;
using DiplomaPrototype;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace DiplomaProrotype.ObjectsManipulation
{
    internal class ObjectPlacement
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<MovableTile> movableTiles = MainWindow.movableTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;
        static private List<Link> linksResourceMachine = MainWindow.linksResourceMachine;


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
                    CreateResourceTile(dropPosition, MainWindow.resourceNearMachineIsEmpty);
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
                    CreateMachineTile(machineData, dropPosition);
                }

                if (machineData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (machineTiles.Contains(machineData))
                    {
                        MainWindow.chosenOneObject = machineData;

                        Canvas.SetLeft(machineData, dropPosition.X - machineData.Width / 2 - 7.5);
                        Canvas.SetTop(machineData, dropPosition.Y - machineData.MachineIndicator.Height - machineData.MachineImage.Height + 5);

                        dropPosition.Y -= 5;

                        MachineLinkMoving(dropPosition, machineData);
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

                if (movableData.Parent is Canvas && MainWindow.currentMode == "move")
                {
                    if (movableTiles.Contains(movableData))
                    {
                        if (movableData.Amount == 1)
                        {
                            MainWindow.chosenOneObject = movableData;

                            Canvas.SetLeft(movableData, dropPosition.X - movableData.Width / 2 - 10);
                            Canvas.SetTop(movableData, dropPosition.Y - movableData.Height / 2 - 2.5);
                        }

                        if (movableData.Amount > 1)
                        {
                            movableData.Amount--;
                            CreateMovableTile(movableData, dropPosition);
                        }                    
                    }
                }
            }

            // Манипуляции с stopData
            if (!(stopData is null))
            {
                if (stopData.Parent is StackPanel)
                {
                    CreateStopTile(stopData, dropPosition);
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
            for (int i = 0; i < linksResourceMachine.Count; i++)
            {
                if (linksResourceMachine[i].FirstTargetType == "resource" && linksResourceMachine[i].FirstTargetListId == resourceTiles.IndexOf(resourceData))
                {
                    double firstMarginLeft = dropPosition.X + resourceData.Width / 2 - 20;
                    double firstMarginTop = dropPosition.Y + 2.5;

                    linksResourceMachine[i].LineInfo.X1 = firstMarginLeft;
                    linksResourceMachine[i].LineInfo.Y1 = firstMarginTop;

                    linksResourceMachine[i].CircleInfo.Margin = new Thickness(linksResourceMachine[i].LineInfo.X1 - 5, linksResourceMachine[i].LineInfo.Y1 - 5, 0, 0);
                }
            }

            for (int i = 0; i < linksResourceMachine.Count; i++)
            {
                if (linksResourceMachine[i].LastTargetType == "resource" && linksResourceMachine[i].LastTargetListId == resourceTiles.IndexOf(resourceData))
                {
                    double lastMarginLeft = dropPosition.X - resourceData.Width / 2 + 25;
                    double lastMarginTop = dropPosition.Y + 2.5;

                    linksResourceMachine[i].LineInfo.X2 = lastMarginLeft;
                    linksResourceMachine[i].LineInfo.Y2 = lastMarginTop;

                    //links[i].CircleInfo.Margin = new Thickness(links[i].LineInfo.X1 - 5, links[i].LineInfo.Y1 - 5, 0, 0);
                }
            }
        }

        static public void MachineLinkMoving(Point dropPosition, MachineTile machineData)
        {
            for (int i = 0; i < linksResourceMachine.Count; i++)
            {
                if (linksResourceMachine[i].FirstTargetType == "machine" && linksResourceMachine[i].FirstTargetListId == machineTiles.IndexOf(machineData))
                {
                    double firstMarginLeft = dropPosition.X + machineData.Width / 2 - 10;
                    double firstMarginTop = dropPosition.Y + 2.5;

                    linksResourceMachine[i].LineInfo.X1 = firstMarginLeft;
                    linksResourceMachine[i].LineInfo.Y1 = firstMarginTop;

                    linksResourceMachine[i].CircleInfo.Margin = new Thickness(linksResourceMachine[i].LineInfo.X1 - 5, linksResourceMachine[i].LineInfo.Y1 - 5, 0, 0);
                }
            }

            for (int i = 0; i < linksResourceMachine.Count; i++)
            {
                if (linksResourceMachine[i].LastTargetType == "machine" && linksResourceMachine[i].LastTargetListId == machineTiles.IndexOf(machineData))
                {
                    double lastMarginLeft = dropPosition.X - machineData.Width / 2;
                    double lastMarginTop = dropPosition.Y + 2.5;

                    switch (machineData.MachineText.Text)
                    {
                        case "Токарный": lastMarginLeft += 18.5; break;
                        case "Сварочный": lastMarginLeft += 16.5; break;
                        case "Фрезерный": lastMarginLeft += 17.5; break;
                    }

                    linksResourceMachine[i].LineInfo.X2 = lastMarginLeft;
                    linksResourceMachine[i].LineInfo.Y2 = lastMarginTop;

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
            Canvas.SetTop(machineTile, dropPosition.Y - machineData.MachineIndicator.Height - machineData.MachineImage.Height + 5);

            SelectWindow.CreateMachineSelectWindow();

            machineTile.Processes = SelectWindow.currentNumber;
            if (machineTile.Processes > 0 && machineTile.Processes <= 5)
            {
                machineTile.MachineProgress5.Margin = new Thickness(10, 0, 10, 35);
                machineTile.MachineProgress5.Visibility = Visibility.Visible;

                if (machineTile.Processes > 1)
                {
                    machineTile.MachineProgress4.Margin = new Thickness(10, 0, 10, 45);
                    machineTile.MachineProgress4.Visibility = Visibility.Visible;
                }
                if (machineTile.Processes > 2)
                {
                    machineTile.MachineProgress3.Margin = new Thickness(10, 0, 10, 55);
                    machineTile.MachineProgress3.Visibility = Visibility.Visible;
                }
                if (machineTile.Processes > 3)
                {
                    machineTile.MachineProgress2.Margin = new Thickness(10, 0, 10, 65);
                    machineTile.MachineProgress2.Visibility = Visibility.Visible;
                }
                if (machineTile.Processes > 4)
                {
                    machineTile.MachineProgress1.Margin = new Thickness(10, 0, 10, 75);
                    machineTile.MachineProgress1.Visibility = Visibility.Visible;
                }

                machineTile.Height = 110 + SelectWindow.currentNumber * 10;

                machineTile.MachineImage.Margin = new Thickness(0, 25, 0, 0);
                machineTile.MachineText.Margin = new Thickness(0, 0, 0, 15);

                machineTile.MachineIndicator.Visibility = Visibility.Visible;
                machineTile.MachineId.Visibility = Visibility.Visible;

                MainWindow.lastTileType = "machine";
                mw.TargetCanvas.Children.Add(machineTile);

                machineTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
                MainWindow.chosenOneObject = machineTile;


                MainWindow.resourceNearMachineIsEmpty = true;

                dropPosition.X += 70;
                CreateResourceTile(dropPosition, MainWindow.resourceNearMachineIsEmpty);

                MainWindow.resourceNearMachineIsEmpty = false;
            }            
        }

        static public void CreateMovableTile(MovableTile movableData, Point dropPosition)
        {            
            var movableTile = new MovableTile();

            if (movableData.Amount == 0) // Создание из панели
            {
                SelectWindow.CreateMovableSelectWindow();
                movableTile.Amount = SelectWindow.currentNumber;
                movableTile.Places = SelectWindow.currentPlaces;
            }
            else // Перемещение одной из группы, где > 1
            {
                movableTile.Amount = 1;
                movableTile.Places = movableData.Places;
            }
           
            if (movableTile.Amount > 0 && movableTile.Places > 0 && movableTile.Places <= 3)
            {
                CreateMovableContextMenu(movableTile);
                MainWindow.movableTiles.Add(movableTile);

                movableTile.Image = movableData.Image;
                movableTile.Text = movableData.Text;
                movableTile.Id = MainWindow.movableTiles.Count;

                Canvas.SetLeft(movableTile, dropPosition.X - movableTile.Width / 2 - 10);
                Canvas.SetTop(movableTile, dropPosition.Y - movableTile.Height / 2 - 22.5);

                if (movableTile.Places > 0 && movableTile.Places <= 3)
                {
                    movableTile.ResourceFigure1.Margin = new Thickness(0, -5, 10, 0);
                    movableTile.ResourceFigure1.Visibility = Visibility.Visible;

                    if (movableTile.Places > 1)
                    {
                        movableTile.ResourceFigure2.Margin = new Thickness(0, -5, -12, 0);
                        movableTile.ResourceFigure2.Visibility = Visibility.Visible;
                    }

                    if (movableTile.Places > 2)
                    {
                        movableTile.ResourceFigure3.Margin = new Thickness(0, -5, -34, 0);
                        movableTile.ResourceFigure3.Visibility = Visibility.Visible;
                    }
                }

                movableTile.MovableImage.Margin = new Thickness(0, 25, 0, 0);
                movableTile.MovableText.Margin = new Thickness(0, 0, 0, 15);

                movableTile.MovableIndicator.Visibility = Visibility.Visible;
                movableTile.MovableAmount.Visibility = Visibility.Visible;
                movableTile.MovableId.Visibility = Visibility.Visible;
                movableTile.Height = 105;

                MainWindow.lastTileType = "movable";
                mw.TargetCanvas.Children.Add(movableTile);

                movableTile.MouseLeftButtonUp += new MouseButtonEventHandler(ChooseOneObjectByClick);
                MainWindow.chosenOneObject = movableTile;
            }         
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
