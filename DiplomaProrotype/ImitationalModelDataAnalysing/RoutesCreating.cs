using DiplomaProrotype;
using DiplomaProrotype.ObjectsManipulation;
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

        private List<Point> coordinates = MainWindow.stopTilesCoordinates;

        public Queue<Storyboard> stories = new Queue<Storyboard>();

        public List<DoubleAnimation> animations = new List<DoubleAnimation>();
        public List<AnimationClock> clocks = new List<AnimationClock>();


        private DoubleAnimation MoveFrom1To2X;
        private DoubleAnimation MoveFrom1To2Y;
        private DoubleAnimation MoveFrom2To3X;
        private DoubleAnimation MoveFrom2To3Y;
        private DoubleAnimation MoveFrom3To4X;
        private DoubleAnimation MoveFrom3To4Y;
        private DoubleAnimation MoveFrom4To5Y;
        private DoubleAnimation MoveFrom4To5X;


        public RoutesCreating() 
        {
            DataReading.Read();

            foreach (string operation in DataReading.routes)
            {
                if (operation.Contains("Переезд"))
                {
                    Relocate(operation);
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

            double YMargin = 20;
            double XMargin = 20;

            Vector movableTile = VisualTreeHelper.GetOffset(movableTiles[0]);
            Vector stopTile1 = VisualTreeHelper.GetOffset(stopTiles[0]);
            Vector stopTile2 = VisualTreeHelper.GetOffset(stopTiles[1]);
            Vector stopTile3 = VisualTreeHelper.GetOffset(stopTiles[2]);
            Vector stopTile4 = VisualTreeHelper.GetOffset(stopTiles[3]);
            Vector stopTile5 = VisualTreeHelper.GetOffset(stopTiles[4]);
            Vector stopTile6 = VisualTreeHelper.GetOffset(stopTiles[5]);
            Vector stopTile7 = VisualTreeHelper.GetOffset(stopTiles[6]);

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(stopTile1.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2));


            //for (int j = 0; j < 2; j++)
            //{
            //    for (int i = 1; i < coordinates.Count; i++)
            //    {
            //        pathFigure.Segments.Add(new LineSegment(new Point(coordinates[i - 1].X, coordinates[i - 1].Y), true));
            //        pathFigure.Segments.Add(new LineSegment(new Point(coordinates[i].X, coordinates[i - 1].Y), true));
            //    }
            //}

            //1-2

            MoveFrom1To2X = new DoubleAnimation();
            MoveFrom1To2X.From = stopTile1.X - XMargin;
            MoveFrom1To2X.To = stopTile2.X - XMargin;
            MoveFrom1To2X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom1To2X.AutoReverse = false;
            MoveFrom1To2X.RepeatBehavior = new RepeatBehavior(1);

            Storyboard.SetTarget(MoveFrom1To2X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom1To2X, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom1To2XClock = MoveFrom1To2X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom1To2X);

            MoveFrom1To2Y = new DoubleAnimation();
            MoveFrom1To2Y.From = stopTile1.Y - YMargin;
            MoveFrom1To2Y.To = stopTile2.Y - YMargin;
            MoveFrom1To2Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom1To2Y.AutoReverse = false;
            MoveFrom1To2Y.RepeatBehavior = new RepeatBehavior(2);

            Storyboard.SetTarget(MoveFrom1To2Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom1To2Y, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom1To2YClock = MoveFrom1To2Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom1To2Y);
            //Погрузка
            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = movableTiles[0].ResourceFigure1.Height;
            widthAnimation.To = movableTiles[0].ResourceFigure1.Height / 2;
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            widthAnimation.AutoReverse = false;
            widthAnimation.RepeatBehavior = new RepeatBehavior(3);


            AnimationClock widthanimationClock = widthAnimation.CreateClock() as AnimationClock;

            animations.Add(widthAnimation);

            //2-3
            MoveFrom2To3X = new DoubleAnimation();
            MoveFrom2To3X.From = stopTile2.X - XMargin;
            MoveFrom2To3X.To = stopTile3.X - XMargin;
            MoveFrom2To3X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom2To3X.AutoReverse = false;
            MoveFrom2To3X.RepeatBehavior = new RepeatBehavior(4);

            Storyboard.SetTarget(MoveFrom2To3X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom2To3X, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom2To3XClock = MoveFrom2To3X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom2To3X);

            MoveFrom2To3Y = new DoubleAnimation();
            MoveFrom2To3Y.From = stopTile2.Y - YMargin;
            MoveFrom2To3Y.To = stopTile3.Y - YMargin;
            MoveFrom2To3Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom2To3Y.AutoReverse = false;
            MoveFrom2To3Y.RepeatBehavior = new RepeatBehavior(5);

            Storyboard.SetTarget(MoveFrom2To3Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom2To3Y, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom2To3YClock = MoveFrom2To3Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom2To3Y);
            //3-4
            MoveFrom3To4X = new DoubleAnimation();
            MoveFrom3To4X.From = stopTile3.X - XMargin;
            MoveFrom3To4X.To = stopTile4.X - XMargin;
            MoveFrom3To4X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom3To4X.AutoReverse = false;
            MoveFrom3To4X.RepeatBehavior = new RepeatBehavior(6);

            Storyboard.SetTarget(MoveFrom3To4X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom3To4X, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom3To4XClock = MoveFrom3To4X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom3To4X);

            MoveFrom3To4Y = new DoubleAnimation();
            MoveFrom3To4Y.From = stopTile3.Y - YMargin;
            MoveFrom3To4Y.To = stopTile4.Y - YMargin;
            MoveFrom3To4Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom3To4Y.AutoReverse = false;
            MoveFrom3To4Y.RepeatBehavior = new RepeatBehavior(7);

            Storyboard.SetTarget(MoveFrom3To4Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom3To4Y, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom3To4YClock = MoveFrom3To4Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom3To4Y);

            //Разгрузка
            DoubleAnimation widthAnimation2 = new DoubleAnimation();
            widthAnimation2.From = movableTiles[0].ResourceFigure1.Width;
            widthAnimation2.To = movableTiles[0].ResourceFigure1.Width * 2;
            widthAnimation2.Duration = new Duration(TimeSpan.FromSeconds(2));
            widthAnimation2.AutoReverse = false;
            widthAnimation2.RepeatBehavior = new RepeatBehavior(8);

            AnimationClock widthAnimationClock2 = widthAnimation2.CreateClock();

            animations.Add(widthAnimation2);
            //4-1

            MoveFrom4To5Y = new DoubleAnimation();
            MoveFrom4To5Y.From = stopTile4.Y - YMargin;
            MoveFrom4To5Y.To = stopTile5.Y - YMargin;
            MoveFrom4To5Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom4To5Y.AutoReverse = false;
            MoveFrom4To5Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom4To5Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom4To5Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom4To1YClock = MoveFrom4To5Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom4To5Y);

            MoveFrom4To5X = new DoubleAnimation();
            MoveFrom4To5X.From = stopTile4.X - XMargin;
            MoveFrom4To5X.To = stopTile5.X - XMargin;
            MoveFrom4To5X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom4To5X.AutoReverse = false;
            MoveFrom4To5X.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(MoveFrom4To5X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom4To5X, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom4To5XClock = MoveFrom4To5X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom4To5X);

            clocks.Add(MoveFrom1To2XClock);
            clocks.Add(MoveFrom1To2YClock);
            clocks.Add(widthanimationClock);
            clocks.Add(MoveFrom2To3XClock);
            clocks.Add(MoveFrom2To3YClock);
            clocks.Add(MoveFrom3To4XClock);
            clocks.Add(MoveFrom3To4YClock);
            clocks.Add(widthAnimationClock2);
            clocks.Add(MoveFrom4To1YClock);
            clocks.Add(MoveFrom4To5XClock);
               
            Storyboard storyboard = new Storyboard();

            return storyboard;
        }

        public void Relocate(string operationName)
        {
            double YMargin = 20;
            double XMargin = 20;

            int startStopTileIndex = int.Parse(operationName[8].ToString()) - 1;
            int endStopTileIndex = int.Parse(operationName[10].ToString()) - 1;

            Vector start = VisualTreeHelper.GetOffset(stopTiles[(startStopTileIndex)]);
            Vector end = VisualTreeHelper.GetOffset(stopTiles[(endStopTileIndex)]);
            Vector lastInputTargetMargin = VisualTreeHelper.GetOffset(stopTiles[2]);

            Vector V1 = VisualTreeHelper.GetOffset(stopTiles[startStopTileIndex]);
            Point p1 = new Point(V1.X, V1.Y);


            Point startPoint = new Point(start.X, start.Y);
            Point endPoint = new Point(end.X, end.Y);

            DoubleAnimation MoveFromToX = new DoubleAnimation();
            MoveFromToX.From = startPoint.X - XMargin;
            MoveFromToX.To = endPoint.X - XMargin;
            MoveFromToX.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFromToX.AutoReverse = false;
            MoveFromToX.RepeatBehavior = new RepeatBehavior(4);

            Storyboard.SetTarget(MoveFromToX, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFromToX, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFromToXClock = MoveFromToX.CreateClock() as AnimationClock;



            DoubleAnimation MoveFromToY = new DoubleAnimation();
            MoveFromToY.From = startPoint.Y - YMargin;
            MoveFromToY.To = endPoint.Y - YMargin;
            MoveFromToY.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFromToY.AutoReverse = false;
            MoveFromToY.RepeatBehavior = new RepeatBehavior(5);

            Storyboard.SetTarget(MoveFromToY, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFromToY, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFromToYClock = MoveFromToY.CreateClock() as AnimationClock;
            
            //clocks.Add(MoveFromToXClock);
            //clocks.Add(MoveFromToYClock);

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
