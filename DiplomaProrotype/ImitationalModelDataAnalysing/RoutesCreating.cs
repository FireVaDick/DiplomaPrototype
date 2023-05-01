using DiplomaProrotype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiplomaPrototype.ImitationalModelDataAnalysing
{
    internal class RoutesCreating
    {
        private List<StopTile> stopTiles = MainWindow.stopTiles;
        private List<MovableTile> movableTiles = MainWindow.movableTiles;

        public RoutesCreating() 
        {
            DataReading.Read();
            string[] operations = DataReading.routes.Split('\n');

            //foreach (string operation in operations)
            //{
            //    if(operation.Contains("Переезд 1-2"))
            //    {

            //    }
            //    else if (operation.Contains("Погрузка"))
            //    {

            //    }
            //    else if (operation.Contains("Разгрузка"))
            //    {

            //    }
            //    else
            //    {

            //    }
            //}

        }

        public PathGeometry Relocate(string operationName) 
        {

            Point startPoint = new Point(stopTiles[((int)operationName[9] + 1)].X, stopTiles[((int)operationName[9] + 1)]);
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0, 0);
            pathFigure.Segments.Add(new LineSegment(new Point(100, 0), true));
            pathFigure.Segments.Add(new LineSegment(new Point(100, 100), true));
            pathFigure.Segments.Add(new LineSegment(new Point(0, 100), true));
            pathGeometry.Figures.Add(pathFigure);

            // Создание DoubleAnimationUsingPath, содержащего анимацию движения объекта по траектории
            DoubleAnimationUsingPath animation = new DoubleAnimationUsingPath();
            animation.PathGeometry = pathGeometry;
            animation.Duration = TimeSpan.FromSeconds(5);
            animation.Source = PathAnimationSource.X;

            // Создание Storyboard и добавление в него анимации
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(animation, yourObject);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(animation);

            // Запуск анимации
            storyboard.Begin();
        }
    }
}
