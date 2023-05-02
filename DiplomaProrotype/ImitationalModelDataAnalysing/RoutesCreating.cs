using DiplomaProrotype;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Storyboard> stories = new List<Storyboard>();

        public RoutesCreating() 
        {
            DataReading.Read();

            foreach (string operation in DataReading.routes)
            {
                if (operation.Contains("Переезд"))
                {
                    stories.Add(Relocate(operation));
                }
                else if (operation.Contains("Погрузка"))
                {
                    stories.Add(Input(operation));
                }
                else if (operation.Contains("Разгрузка"))
                {
                    stories.Add(Output(operation));
                }
                else
                {
                    return;
                }
            }

        }

        public Storyboard Relocate(string operationName)
        {
            int startStopTileIndex = int.Parse(operationName[8].ToString()) - 1;
            int endStopTileIndex = int.Parse(operationName[10].ToString()) - 1;
            char sym = operationName[8];

            Vector startTargetMargin = VisualTreeHelper.GetOffset(stopTiles[(startStopTileIndex)]);
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
            DoubleAnimationUsingPath animation = new DoubleAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(Convert.ToDouble(operationName[12..]));
            animation.Source = PathAnimationSource.X;

            // Создание Storyboard и добавление в него анимации
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, movableTiles[0]);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
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
            DoubleAnimationUsingPath animation = new DoubleAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(double.Parse(operationName[11..]));
            animation.Source = PathAnimationSource.X;

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
            DoubleAnimationUsingPath animation = new DoubleAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(double.Parse(operationName[12..]));
            animation.Source = PathAnimationSource.X;

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
