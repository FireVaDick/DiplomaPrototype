using DiplomaProrotype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiplomaPrototype.ImitationalModelDataAnalysing
{
    internal class RoutesCreating
    {
        private List<StopTile> stopTiles = MainWindow.stopTiles;
        private List<MovableTile> movableTiles = MainWindow.movableTiles;

        public Queue<Storyboard> stories = new Queue<Storyboard>();

        public RoutesCreating() 
        {
            DataReading.Read();

            foreach (string operation in DataReading.routes)
            {
                if (operation.Contains("Переезд"))
                {
                    stories.Enqueue(Relocate(operation));
                }
                else if (operation.Contains("Погрузка"))
                {
                    stories.Enqueue(Input(operation));
                }
                else if (operation.Contains("Разгрузка"))
                {
                    stories.Enqueue(Output(operation));
                }
                else
                {
                    return;
                }
            }

        }

        public Storyboard CreateStory()
        {
            Vector movableTile = VisualTreeHelper.GetOffset(movableTiles[0]);
            Vector stopTile1 = VisualTreeHelper.GetOffset(stopTiles[0]);
            Vector stopTile2 = VisualTreeHelper.GetOffset(stopTiles[1]);
            Vector stopTile3 = VisualTreeHelper.GetOffset(stopTiles[2]);
            Vector stopTile4 = VisualTreeHelper.GetOffset(stopTiles[3]);

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(movableTile.X, movableTile.Y);

            pathFigure.Segments.Add(new LineSegment(new Point(movableTile.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X, stopTile2.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X, stopTile2.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X, stopTile3.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile3.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile4.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X, stopTile2.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X, stopTile2.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X, stopTile3.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile3.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile4.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X, stopTile1.Y), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X, stopTile1.Y), true));

            pathGeometry.Figures.Add(pathFigure);

            MatrixAnimationUsingPath animation = new MatrixAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(35);


            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, movableTiles[0]);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(RenderTransform).(MatrixTransform.Matrix)"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        public Storyboard Relocate(string operationName)
        {
            int startStopTileIndex = int.Parse(operationName[8].ToString()) - 1;
            int endStopTileIndex = int.Parse(operationName[10].ToString()) - 1;

            Vector startTargetMargin = VisualTreeHelper.GetOffset(movableTiles[0]);
            Vector endTargetMargin = VisualTreeHelper.GetOffset(stopTiles[(endStopTileIndex)]);
            Vector lastInputTargetMargin = VisualTreeHelper.GetOffset(stopTiles[2]);

            Point startPoint = new Point(startTargetMargin.X, startTargetMargin.Y);
            Point endPoint = new Point(endTargetMargin.X, endTargetMargin.Y);

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;

            if ((int.Parse(operationName[10].ToString()) - 1) >= 4)
            {
                pathFigure.Segments.Add(new LineSegment(new Point(startPoint.X, lastInputTargetMargin.Y), true));
                pathFigure.Segments.Add(new LineSegment(new Point(endPoint.X, lastInputTargetMargin.Y), true));
                pathFigure.Segments.Add(new LineSegment(endPoint, true));
            }
            else
            {
                pathFigure.Segments.Add(new LineSegment(new Point(endPoint.X, startPoint.Y), true));
                pathFigure.Segments.Add(new LineSegment(endPoint, true));
            }

           
            pathGeometry.Figures.Add(pathFigure);

            // Создание DoubleAnimationUsingPath, содержащего анимацию движения объекта по траектории
            MatrixAnimationUsingPath animation = new MatrixAnimationUsingPath();


            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(35);

            // Создание Storyboard и добавление в него анимации
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, movableTiles[0]);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(RenderTransform).(MatrixTransform.Matrix)"));
            storyboard.Children.Add(animation);
             
            // Запуск анимации
            return storyboard;
        }

        public Storyboard Input(string operationName)
        {
            Vector startTargetMargin = VisualTreeHelper.GetOffset(stopTiles[(int.Parse(operationName[9].ToString()) - 1)]);
           
            Point startPoint = new Point(startTargetMargin.X, startTargetMargin.Y);
           
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;

            pathFigure.Segments.Add(new LineSegment(startPoint, true));

            pathGeometry.Figures.Add(pathFigure);

            // Создание DoubleAnimationUsingPath, содержащего анимацию движения объекта по траектории
            MatrixAnimationUsingPath animation = new MatrixAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(double.Parse(operationName[11..]));

            // Создание Storyboard и добавление в него анимации
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, movableTiles[0]);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation);

            // Запуск анимации
            return storyboard;
        }

        public Storyboard Output(string operationName)
        {
            Vector startTargetMargin = VisualTreeHelper.GetOffset(stopTiles[((3 + int.Parse(operationName[10].ToString())) - 1)]);

            Point startPoint = new Point(startTargetMargin.X, startTargetMargin.Y);

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = startPoint;

            pathFigure.Segments.Add(new LineSegment(startPoint, true));

            pathGeometry.Figures.Add(pathFigure);

            // Создание DoubleAnimationUsingPath, содержащего анимацию движения объекта по траектории
            MatrixAnimationUsingPath animation = new MatrixAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(double.Parse(operationName[12..]));

            // Создание Storyboard и добавление в него анимации
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, movableTiles[0]);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation);

            // Запуск анимации
            return storyboard;
        }
    }
}
