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


        private DoubleAnimation myDoubleAnimation;
        private DoubleAnimation myDoubleAnimation2;
        private DoubleAnimation myDoubleAnimation3;
        private DoubleAnimation myDoubleAnimation4;
        private DoubleAnimation myDoubleAnimation5;
        private DoubleAnimation myDoubleAnimation6;
        private DoubleAnimation myDoubleAnimation7;
        private DoubleAnimation myDoubleAnimation8;


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

            myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = stopTile1.X - (movableTiles[0].Width - 50);
            myDoubleAnimation.To = stopTile2.X - (movableTiles[0].Width - 50);
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation.AutoReverse = false;
            myDoubleAnimation.RepeatBehavior = new RepeatBehavior(1);

            Storyboard.SetTarget(myDoubleAnimation, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Canvas.LeftProperty));

            AnimationClock animationClock = myDoubleAnimation.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation);

            myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation2.From = stopTile1.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation2.To = stopTile2.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation2.AutoReverse = false;
            myDoubleAnimation2.RepeatBehavior = new RepeatBehavior(2);

            Storyboard.SetTarget(myDoubleAnimation2, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath(Canvas.TopProperty));

            AnimationClock animationClock2 = myDoubleAnimation2.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation2);
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
            myDoubleAnimation3 = new DoubleAnimation();
            myDoubleAnimation3.From = stopTile2.X - (movableTiles[0].Width - 50);
            myDoubleAnimation3.To = stopTile3.X - (movableTiles[0].Width - 50);
            myDoubleAnimation3.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation3.AutoReverse = false;
            myDoubleAnimation3.RepeatBehavior = new RepeatBehavior(4);

            Storyboard.SetTarget(myDoubleAnimation3, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation3, new PropertyPath(Canvas.LeftProperty));

            AnimationClock animationClock3 = myDoubleAnimation3.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation3);

            myDoubleAnimation4 = new DoubleAnimation();
            myDoubleAnimation4.From = stopTile2.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation4.To = stopTile3.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation4.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation4.AutoReverse = false;
            myDoubleAnimation4.RepeatBehavior = new RepeatBehavior(5);

            Storyboard.SetTarget(myDoubleAnimation4, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation4, new PropertyPath(Canvas.TopProperty));

            AnimationClock animationClock4 = myDoubleAnimation4.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation4);
            //3-4
            myDoubleAnimation5 = new DoubleAnimation();
            myDoubleAnimation5.From = stopTile3.X - (movableTiles[0].Width - 50);
            myDoubleAnimation5.To = stopTile4.X - (movableTiles[0].Width - 50);
            myDoubleAnimation5.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation5.AutoReverse = false;
            myDoubleAnimation5.RepeatBehavior = new RepeatBehavior(6);

            Storyboard.SetTarget(myDoubleAnimation5, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation5, new PropertyPath(Canvas.LeftProperty));

            AnimationClock animationClock5 = myDoubleAnimation5.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation5);

            myDoubleAnimation6 = new DoubleAnimation();
            myDoubleAnimation6.From = stopTile3.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation6.To = stopTile4.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation6.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation6.AutoReverse = false;
            myDoubleAnimation6.RepeatBehavior = new RepeatBehavior(7);

            Storyboard.SetTarget(myDoubleAnimation6, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation6, new PropertyPath(Canvas.TopProperty));

            AnimationClock animationClock6 = myDoubleAnimation6.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation6);

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

            myDoubleAnimation7 = new DoubleAnimation();
            myDoubleAnimation7.From = stopTile4.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation7.To = stopTile1.Y - (movableTiles[0].Height / 2);
            myDoubleAnimation7.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation7.AutoReverse = false;
            myDoubleAnimation7.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(myDoubleAnimation7, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation7, new PropertyPath(Canvas.LeftProperty));

            AnimationClock animationClock7 = myDoubleAnimation7.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation7);

            myDoubleAnimation8 = new DoubleAnimation();
            myDoubleAnimation8.From = stopTile4.X - (movableTiles[0].Width - 50);
            myDoubleAnimation8.To = stopTile1.X - (movableTiles[0].Width - 50);
            myDoubleAnimation8.Duration = new Duration(TimeSpan.FromSeconds(2));
            myDoubleAnimation8.AutoReverse = false;
            myDoubleAnimation8.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(myDoubleAnimation8, movableTiles[0]);
            Storyboard.SetTargetProperty(myDoubleAnimation8, new PropertyPath(Canvas.TopProperty));

            AnimationClock animationClock8 = myDoubleAnimation8.CreateClock() as AnimationClock;

            animations.Add(myDoubleAnimation8);

            clocks.Add(animationClock);
            clocks.Add(animationClock2);
            clocks.Add(widthanimationClock);
            clocks.Add(animationClock3);
            clocks.Add(animationClock4);
            clocks.Add(animationClock5);
            clocks.Add(animationClock6);
            clocks.Add(widthAnimationClock2);
            clocks.Add(animationClock7);
            clocks.Add(animationClock8);
    

            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X - (movableTiles[0].Width - 50), stopTile2.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X - (movableTiles[0].Width - 50), stopTile2.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X - (movableTiles[0].Width - 50), stopTile3.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile3.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile4.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile2.X - (movableTiles[0].Width - 50), stopTile2.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X - (movableTiles[0].Width - 50), stopTile2.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile3.X - (movableTiles[0].Width - 50), stopTile3.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile3.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile4.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile4.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
            pathFigure.Segments.Add(new LineSegment(new Point(stopTile1.X - (movableTiles[0].Width - 50), stopTile1.Y - (movableTiles[0].Height / 2)), true));
                
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
