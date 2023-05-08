using DiplomaProrotype.Models;
using DiplomaProrotype.ObjectsManipulation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomaProrotype.CanvasManipulation
{
    internal class DrawLinkRoutePath
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;

        static private Link link;
        static private Ellipse linkCircle;
        static private Line linkLine;

        static private Line routeLine;
        static double lastX2 = 0, lastY2 = 0;

        static private Rectangle pathRectangle;

        static private Point startPos;

        static private double firstMarginLeft = 0;
        static private double firstMarginTop = 0;
        static private double lastMarginLeft = 0;
        static private double lastMarginTop = 0;

        static public void CreateLink(UserControl target)
        {
            Vector targetMargin = VisualTreeHelper.GetOffset(target);
            Type type = target.GetType();

            if (type == typeof(ResourceTile))
            {
                firstMarginLeft = targetMargin.X + target.Width / 2 - target.Margin.Left / 2 + 20; // 20 и 5 - отступы от центра
                firstMarginTop = targetMargin.Y + target.Height / 2 - target.Margin.Top - 5;

                for (int i = 0; i < resourceTiles.Count; i++)
                {
                    if (targetMargin == VisualTreeHelper.GetOffset(resourceTiles[i]))
                    {
                        link = new Link();
                        link.FirstTargetType = "resource";
                        link.FirstTargetListId = i;
                    }
                }
            }

            if (type == typeof(MachineTile))
            {
                firstMarginLeft = targetMargin.X + target.Width / 2 - target.Margin.Left / 2 + 30; // 30 и 5 - отступы от центра
                firstMarginTop = targetMargin.Y + ((MachineTile)target).MachineIndicator.Height + ((MachineTile)target).MachineImage.Height / 2 + 7.5;

                for (int i = 0; i < machineTiles.Count; i++)
                {
                    if (targetMargin == VisualTreeHelper.GetOffset(machineTiles[i]))
                    {
                        link = new Link();
                        link.FirstTargetType = "machine";
                        link.FirstTargetListId = i;
                    }
                }
            }

            if (type == typeof(StopTile))
            {
                firstMarginLeft = targetMargin.X + target.Width / 2 - target.Margin.Left / 2 + 20; // 20 и 5 - отступы от центра
                firstMarginTop = targetMargin.Y + target.Height / 2 - target.Margin.Top - 5;

                for (int i = 0; i < stopTiles.Count; i++)
                {
                    if (targetMargin == VisualTreeHelper.GetOffset(stopTiles[i]))
                    {
                        link = new Link();
                        if (stopTiles[i].StopText.Text == "Погрузка") link.FirstTargetType = "loading";
                        if (stopTiles[i].StopText.Text == "Разгрузка") link.FirstTargetType = "unloading";
                        link.FirstTargetListId = i;
                    }
                }
            }

            EnableObjectsOrNot.SetAllObjectsToUnenabled(); // Для прохождения связи сквозь объект

            linkLine = new Line();

            // Цвет линии связи
            if (type == typeof(ResourceTile))
            {
                if (((ResourceTile)target).ResourceFigure.Fill == Brushes.White)
                {
                    linkLine.Stroke = Brushes.Black;
                }
                else
                {
                    linkLine.Stroke = ((ResourceTile)target).ResourceFigure.Fill;
                }
            }

            if (type == typeof(MachineTile) || type == typeof(StopTile))
            {
                linkLine.Stroke = Brushes.Black;
            }         

            if (type == typeof(StopTile))
            {
                linkLine.StrokeDashArray = new DoubleCollection() { 1, 1 };
            }

            linkLine.StrokeThickness = 3;
            linkLine.X2 = linkLine.X1 = firstMarginLeft; // Точки конца там же, где и начала
            linkLine.Y2 = linkLine.Y1 = firstMarginTop;

            mw.TargetCanvas.Children.Add(linkLine);

            // Кружок начала линии
            linkCircle = new Ellipse
            {
                Fill = Brushes.White,
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                Width = 10,
                Height = 10,
                Margin = new Thickness(firstMarginLeft - 5, firstMarginTop - 5, 0, 0)
            };

            if (type == typeof(StopTile))
            {
                linkCircle.StrokeDashArray = new DoubleCollection() { 2, 1 };
            }

            mw.TargetCanvas.Children.Add(linkCircle);
        }

        static private void FinishLink(MouseButtonEventArgs e)
        {
            Point mousePos = e.MouseDevice.GetPosition(mw.TargetCanvas);
            Vector targetMargin;

            if (!(link is null) && link.FirstTargetType == "resource")
            {
                if (machineTiles.Count != 0) // Задел o-- станок
                {
                    for (int i = 0; i < machineTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(machineTiles[i]);

                        lastMarginLeft = targetMargin.X + machineTiles[i].Width / 2 - machineTiles[i].Margin.Left / 2;
                        lastMarginTop = targetMargin.Y + machineTiles[i].MachineIndicator.Height + machineTiles[i].MachineImage.Height / 2 + 7.5;

                        if (mousePos.X > targetMargin.X && mousePos.X < targetMargin.X + machineTiles[i].Width &&
                            mousePos.Y > targetMargin.Y && mousePos.Y < targetMargin.Y + machineTiles[i].Height)
                        {
                            MainWindow.linksResourceMachine.Add(link);

                            link.LastTargetType = "machine";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.linksResourceMachine.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

;                           switch (machineTiles[i].MachineText.Text) // Отступы в зависимости от картинки, чтобы было красиво и касалось
                            {
                                case "Токарный": lastMarginLeft -= 15; break;
                                case "Сварочный": lastMarginLeft -= 20; break;
                                case "Фрезерный": lastMarginLeft -= 17.5; break;
                            }

                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            MainWindow.matrixResourceMachine[0, link.FirstTargetListId + 1] = "Задел " + (link.FirstTargetListId + 1);
                            MainWindow.matrixResourceMachine[link.LastTargetListId + 1, 0] = "Станок " + machineTiles[i].MachineId.Text;
                            MainWindow.matrixResourceMachine[link.LastTargetListId + 1, link.FirstTargetListId + 1] = "-1";

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }

                if (stopTiles.Count != 0) // Задел o-- стоянка (Погрузка)
                {
                    for (int i = 0; i < stopTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(stopTiles[i]);

                        lastMarginLeft = targetMargin.X + stopTiles[i].Width / 2 - stopTiles[i].Margin.Left / 2 - 10;
                        lastMarginTop = targetMargin.Y + stopTiles[i].StopFigure.Height / 2 + 5;

                        if (mousePos.X > targetMargin.X && mousePos.X < targetMargin.X + stopTiles[i].Width &&
                            mousePos.Y > targetMargin.Y && mousePos.Y < targetMargin.Y + stopTiles[i].Height && stopTiles[i].StopText.Text == "Погрузка")
                        {
                            MainWindow.linksResourceMachine.Add(link); //

                            link.LastTargetType = "loading";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.linksResourceStop.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            linkLine.StrokeDashArray = new DoubleCollection() { 1, 1 };
                            linkCircle.StrokeDashArray = new DoubleCollection() { 2, 1 };
                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            MainWindow.matrixResourcePlaceStop[0, link.FirstTargetListId + 1] = "Задел " + (link.FirstTargetListId + 1);
                            MainWindow.matrixResourcePlaceStop[link.LastTargetListId + 1, 0] = "Погрузка " + stopTiles[i].StopId.Text;
                            MainWindow.matrixResourcePlaceStop[link.LastTargetListId + 1, link.FirstTargetListId + 1] = "-1";

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }
            }

            if (!(link is null) && link.FirstTargetType == "machine" )
            {
                if (resourceTiles.Count != 0)  // Станок o-- задел
                {
                    for (int i = 0; i < resourceTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(resourceTiles[i]);

                        lastMarginLeft = targetMargin.X + resourceTiles[i].Width / 2 - resourceTiles[i].Margin.Left / 2 - 10;
                        lastMarginTop = targetMargin.Y + resourceTiles[i].Height / 2 - resourceTiles[i].Margin.Top - 5; // 5 - отступ от центра

                        if (mousePos.X > targetMargin.X && mousePos.X < targetMargin.X + resourceTiles[i].Width &&
                            mousePos.Y > targetMargin.Y && mousePos.Y < targetMargin.Y + resourceTiles[i].Height)
                        {
                            MainWindow.linksResourceMachine.Add(link);

                            link.LastTargetType = "resource";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.linksResourceMachine.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            if (resourceTiles[i].ResourceFigure.Fill == Brushes.White) // Для раскрашивания связи от станка к ресурсу, если ресурс уже цветной
                            {
                                linkLine.Stroke = Brushes.Black;
                            }
                            else
                            {
                                linkLine.Stroke = resourceTiles[i].ResourceFigure.Fill;
                            }

                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            MainWindow.matrixResourceMachine[0, link.LastTargetListId + 1] = "Задел " + resourceTiles[i].ResourceId.Text;
                            MainWindow.matrixResourceMachine[link.FirstTargetListId + 1, 0] = "Станок " + (link.FirstTargetListId + 1);
                            MainWindow.matrixResourceMachine[link.FirstTargetListId + 1, link.LastTargetListId + 1] = "1";

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }
            }

            if (!(link is null) && link.FirstTargetType == "unloading")
            {
                if (resourceTiles.Count != 0)  // Стоянка o-- задел (Разгрузка)
                {
                    for (int i = 0; i < resourceTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(resourceTiles[i]);

                        lastMarginLeft = targetMargin.X + resourceTiles[i].Width / 2 - resourceTiles[i].Margin.Left / 2 - 10;
                        lastMarginTop = targetMargin.Y + resourceTiles[i].Height / 2 - resourceTiles[i].Margin.Top - 5; // 5 - отступ от центра

                        if (mousePos.X > targetMargin.X && mousePos.X < targetMargin.X + resourceTiles[i].Width &&
                            mousePos.Y > targetMargin.Y && mousePos.Y < targetMargin.Y + resourceTiles[i].Height)
                        {
                            MainWindow.linksResourceMachine.Add(link); //

                            link.LastTargetType = "resource";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.linksResourceMachine.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            if (resourceTiles[i].ResourceFigure.Fill == Brushes.White) // Для раскрашивания связи от станка к ресурсу, если ресурс уже цветной
                            {
                                linkLine.Stroke = Brushes.Black;
                            }
                            else
                            {
                                linkLine.Stroke = resourceTiles[i].ResourceFigure.Fill;
                            }

                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            MainWindow.matrixResourcePlaceStop[0, link.LastTargetListId + 1] = "Задел " + resourceTiles[i].ResourceId.Text;
                            MainWindow.matrixResourcePlaceStop[link.FirstTargetListId + 1, 0] = "Разгрузка " + (link.FirstTargetListId + 1);
                            MainWindow.matrixResourcePlaceStop[link.FirstTargetListId + 1, link.LastTargetListId + 1] = "1";

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }

                if (stopTiles.Count != 0) // Стоянка o-- стоянка
                {
                    for (int i = 0; i < stopTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(stopTiles[i]);

                        lastMarginLeft = targetMargin.X + stopTiles[i].Width / 2 - stopTiles[i].Margin.Left / 2 - 10;
                        lastMarginTop = targetMargin.Y + stopTiles[i].StopFigure.Height / 2 + 5;

                        if (mousePos.X > targetMargin.X && mousePos.X < targetMargin.X + stopTiles[i].Width &&
                            mousePos.Y > targetMargin.Y && mousePos.Y < targetMargin.Y + stopTiles[i].Height &&
                            stopTiles[link.FirstTargetListId].StopChain.Text == stopTiles[i].StopChain.Text &&
                            stopTiles[link.FirstTargetListId].StopText.Text == "Погрузка" && stopTiles[i].StopText.Text == "Разгрузка")
                        {
                            MainWindow.linksResourceMachine.Add(link); //

                            link.LastTargetType = "stop";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.linksResourceStop.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            linkLine.StrokeDashArray = new DoubleCollection() { 1, 1 };
                            linkCircle.StrokeDashArray = new DoubleCollection() { 2, 1 };
                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            //MainWindow.matrixResourceMachine[link.LastTargetListId, link.FirstTargetListId] = -1;

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }
            }
        }


        static public void DrawStart(MouseButtonEventArgs e)
        {
            startPos = e.GetPosition(mw.TargetCanvas);

            if (MainWindow.currentMode == "link")
            {
                if (e.Source is ResourceTile)
                {
                    ResourceTile target = e.Source as ResourceTile;
                    CreateLink(target);
                }

                if (e.Source is MachineTile)
                {
                    MachineTile target = e.Source as MachineTile;
                    CreateLink(target);
                }

                if (e.Source is StopTile)
                {
                    StopTile target = e.Source as StopTile;
                    CreateLink(target);
                }
            }

            if (MainWindow.currentMode == "route")
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

                routeLine.X1 = lastX2;
                routeLine.Y1 = lastY2;

                routeLine.X2 = routeLine.X1;
                routeLine.Y2 = routeLine.Y1;

                mw.TargetCanvas.Children.Add(routeLine);
            }

            if (MainWindow.currentMode == "path")
            {
                EnableObjectsOrNot.SetAllObjectsToUnenabled();

                pathRectangle = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                if (MainWindow.currentPathType == "discontinuous")
                {
                    pathRectangle.StrokeDashArray = new DoubleCollection() { 2, 1 };
                }

                Canvas.SetLeft(pathRectangle, startPos.X);
                Canvas.SetTop(pathRectangle, startPos.Y);

                mw.TargetCanvas.Children.Add(pathRectangle);
            }
        }

        static public void DrawInterim(MouseEventArgs e)
        {
            if (MainWindow.currentMode == "link" && !(linkLine is null))
            {
                if (e.LeftButton == MouseButtonState.Released || link == null)
                    return;

                var pos = e.GetPosition(mw.TargetCanvas);

                linkLine.X2 = pos.X;
                linkLine.Y2 = pos.Y;
            }

            if (MainWindow.currentMode == "route")
            {
                if (e.LeftButton == MouseButtonState.Released || routeLine == null)
                    return;

                var pos = e.GetPosition(mw.TargetCanvas);

                routeLine.X2 = pos.X;
                routeLine.Y2 = pos.Y;
            }

            if (MainWindow.currentMode == "path")
            {
                if (e.LeftButton == MouseButtonState.Released || pathRectangle == null)
                    return;

                var pos = e.GetPosition(mw.TargetCanvas);

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

        static public void DrawEndMouseLeft(MouseButtonEventArgs e)
        {
            if (MainWindow.currentMode == "link")
            {
                FinishLink(e);
            }

            if (MainWindow.currentMode == "path")
            {
                pathRectangle = null;

                EnableObjectsOrNot.SetAllObjectsToEnabled();
            }
        }

        static public void DrawEndMouseRight()
        {
            if (MainWindow.currentMode == "route")
            {
                routeLine = null;

                lastX2 = 0;
                lastY2 = 0;
            }
        }
    }
}
