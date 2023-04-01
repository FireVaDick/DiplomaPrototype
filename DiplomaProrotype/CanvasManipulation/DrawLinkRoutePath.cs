using DiplomaProrotype.Models;
using DiplomaProrotype.ObjectsManipulation;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;

namespace DiplomaProrotype.CanvasManipulation
{
    internal class DrawLinkRoutePath
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;

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
            }

            if (type == typeof(MachineTile))
            {
                firstMarginLeft = targetMargin.X + target.Width / 2 - target.Margin.Left / 2 + 30; // 20 и 5 - отступы от центра
                firstMarginTop = targetMargin.Y + target.Height / 2 - target.Margin.Top + 5;
            }

            for (int i = 0; i < resourceTiles.Count; i++)
            {
                if (targetMargin == VisualTreeHelper.GetOffset(resourceTiles[i]))
                {
                    link = new Link();

                    if (type == typeof(ResourceTile))
                    {
                        link.FirstTargetType = "resource";
                    }

                    link.FirstTargetListId = i;
                }
            }

            for (int i = 0; i < machineTiles.Count; i++)
            {
                if (targetMargin == VisualTreeHelper.GetOffset(machineTiles[i]))
                {
                    link = new Link();

                    if (type == typeof(MachineTile))
                    {
                        link.FirstTargetType = "machine";
                    }

                    link.FirstTargetListId = i;
                }
            }

            EnableObjectsOrNot.SetAllObjectsToUnenabled(); // Для прохождения связи сквозь объект

            linkLine = new Line();

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

            if (type == typeof(MachineTile))
            {
                linkLine.Stroke = Brushes.Black;
            }
            linkLine.StrokeThickness = 3;

            linkCircle = new Ellipse
            {
                Fill = Brushes.White,
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                Width = 10,
                Height = 10,
                Margin = new Thickness(firstMarginLeft - 5, firstMarginTop - 5, 0, 0)
            };

            linkLine.X2 = linkLine.X1 = firstMarginLeft; // Точки конца там же, где и начала
            linkLine.Y2 = linkLine.Y1 = firstMarginTop;

            mw.TargetCanvas.Children.Add(linkLine);
            mw.TargetCanvas.Children.Add(linkCircle);
        }


        static private void FinishLink(MouseButtonEventArgs e)
        {
            Point mousePos = e.MouseDevice.GetPosition(mw.TargetCanvas);
            Vector targetMargin;

            if (link.FirstTargetType == "resource" && !(link is null))
            {
                if (machineTiles.Count != 0)
                {
                    for (int i = 0; i < machineTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(machineTiles[i]);

                        lastMarginLeft = targetMargin.X + machineTiles[i].Width / 2 - machineTiles[i].Margin.Left / 2;
                        lastMarginTop = targetMargin.Y + machineTiles[i].Height / 2 - machineTiles[i].Margin.Top + 5; // 5 - отступ от центра

                        if (mousePos.X > targetMargin.X &&
                            mousePos.X < targetMargin.X + machineTiles[i].Width &&
                            mousePos.Y > targetMargin.Y &&
                            mousePos.Y < targetMargin.Y + machineTiles[i].Height)
                        {
                            MainWindow.links.Add(link);

                            link.LastTargetType = "machine";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.links.Count;
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

                            MainWindow.matrixResourceMachine[link.LastTargetListId, link.FirstTargetListId] = -1;

                            EnableObjectsOrNot.SetAllObjectsToEnabled();
                        }
                    }
                }
            }

            if (link.FirstTargetType == "machine" && !(link is null))
            {
                if (resourceTiles.Count != 0)
                {
                    for (int i = 0; i < resourceTiles.Count; i++)
                    {
                        targetMargin = VisualTreeHelper.GetOffset(resourceTiles[i]);

                        lastMarginLeft = targetMargin.X + resourceTiles[i].Width / 2 - resourceTiles[i].Margin.Left / 2 - 10;
                        lastMarginTop = targetMargin.Y + resourceTiles[i].Height / 2 - resourceTiles[i].Margin.Top - 5; // 5 - отступ от центра

                        if (mousePos.X > targetMargin.X &&
                            mousePos.X < targetMargin.X + resourceTiles[i].Width &&
                            mousePos.Y > targetMargin.Y &&
                            mousePos.Y < targetMargin.Y + resourceTiles[i].Height)
                        {
                            MainWindow.links.Add(link);

                            link.LastTargetType = "resource";
                            link.LastTargetListId = i;
                            link.LinkId = MainWindow.links.Count;
                            link.LineInfo = linkLine;
                            link.CircleInfo = linkCircle;

                            linkLine.X2 = lastMarginLeft;
                            linkLine.Y2 = lastMarginTop;
                            linkLine = null;

                            MainWindow.matrixResourceMachine[link.FirstTargetListId, link.LastTargetListId] = 1;

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
