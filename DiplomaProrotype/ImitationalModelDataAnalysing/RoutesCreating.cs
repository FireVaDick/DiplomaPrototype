using DiplomaProrotype;
using System;
using System.Collections.Generic;
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
        private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;

        private List<Point> coordinates = MainWindow.stopTilesCoordinates;

        public Queue<Storyboard> stories = new Queue<Storyboard>();

        public List<DoubleAnimation> animations = new List<DoubleAnimation>();
        public List<AnimationClock> clocks = new List<AnimationClock>();
        public List<AnimationClock> clocks2 = new List<AnimationClock>();

        public List<List<AnimationClock>> complexesClocks = new List<List<AnimationClock>>();


        private DoubleAnimation MoveFrom1To2X;
        private DoubleAnimation MoveFrom1To2Y;
        private DoubleAnimation MoveFrom1To3X;
        private DoubleAnimation MoveFrom1To3Y;
        private DoubleAnimation MoveFrom2To3X;
        private DoubleAnimation MoveFrom2To3Y;
        private DoubleAnimation MoveFrom3To4X;
        private DoubleAnimation MoveFrom3To4Y;
        private DoubleAnimation MoveFrom4To5Y;
        private DoubleAnimation MoveFrom4To5X;
        private DoubleAnimation MoveFrom4To6Y;
        private DoubleAnimation MoveFrom4To6X;
        private DoubleAnimation MoveFrom5To6Y;
        private DoubleAnimation MoveFrom5To6X;
        private DoubleAnimation MoveFrom6To2Y;
        private DoubleAnimation MoveFrom6To2X;
        private DoubleAnimation MoveFrom6To3Y;
        private DoubleAnimation MoveFrom6To3X;

      


        public RoutesCreating()
        {
            complexesClocks.Add(new List<AnimationClock>());
            complexesClocks.Add(new List<AnimationClock>());
            complexesClocks.Add(new List<AnimationClock>());
            complexesClocks.Add(new List<AnimationClock>());
            complexesClocks.Add(new List<AnimationClock>());

            DataReading.Read();
            

            InitializeRoutes();

            foreach (string operation in DataReading.routes)
            {

                if (operation.Contains("Переезд"))
                {
                    Relocate(operation);
                }
                else if (operation.Contains("Погрузка"))
                {
                    Input(operation);
                }
                else if (operation.Contains("Разгрузка"))
                {
                    Output(operation);
                }
            }

        }

        public void InitializeRoutes()
        {
            double YMargin = 20;
            double XMargin = 20;

            AnimationClock MoveFrom1To2XClock = null;
            AnimationClock MoveFrom1To2YClock = null;
            AnimationClock MoveFrom1To3XClock = null;
            AnimationClock MoveFrom1To3YClock = null;
            AnimationClock MoveFrom2To3XClock = null;
            AnimationClock MoveFrom2To3YClock = null;
            AnimationClock MoveFrom3To4XClock = null;
            AnimationClock MoveFrom3To4YClock = null;
            AnimationClock MoveFrom4To5YClock = null;
            AnimationClock MoveFrom4To5XClock = null;
            AnimationClock MoveFrom4To6YClock = null;
            AnimationClock MoveFrom4To6XClock = null;
            AnimationClock MoveFrom5To6YClock = null;
            AnimationClock MoveFrom5To6XClock = null;
            AnimationClock MoveFrom6To2YClock = null;
            AnimationClock MoveFrom6To2XClock = null;
            AnimationClock MoveFrom6To3YClock = null;
            AnimationClock MoveFrom6To3XClock = null;
            AnimationClock resource1InputClock = null;
            AnimationClock resource1OutputClock = null;

            AnimationClock resource2InputClock = null;
            AnimationClock resource2OutputClock = null;

            Vector movableTile = VisualTreeHelper.GetOffset(movableTiles[0]);
            Vector stopTile1 = VisualTreeHelper.GetOffset(stopTiles[0]);
            Vector stopTile2 = VisualTreeHelper.GetOffset(stopTiles[1]);
            Vector stopTile3 = VisualTreeHelper.GetOffset(stopTiles[2]);
            Vector stopTile4 = VisualTreeHelper.GetOffset(stopTiles[3]);
            Vector stopTile5 = VisualTreeHelper.GetOffset(stopTiles[4]);
            Vector stopTile6 = VisualTreeHelper.GetOffset(stopTiles[5]);

            foreach (string operation in DataReading.routes)
            {
                if (operation.Contains("Переезд"))
                {
                    int startIndex = int.Parse(operation[10].ToString()) - 1;
                    int endIndex = int.Parse(operation[12].ToString()) - 1;
                    double durationTime = double.Parse(operation[14..].ToString()) / 2;

                    if (startIndex == 0)
                    {
                        if(endIndex == 1)
                        {
                            //1-2

                            MoveFrom1To2X = new DoubleAnimation();
                            MoveFrom1To2X.From = stopTile1.X - XMargin;
                            MoveFrom1To2X.To = stopTile2.X - XMargin;
                            MoveFrom1To2X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom1To2X.AutoReverse = false;
                            MoveFrom1To2X.RepeatBehavior = new RepeatBehavior(1);

                            Storyboard.SetTarget(MoveFrom1To2X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom1To2X, new PropertyPath(Canvas.LeftProperty));

                            MoveFrom1To2XClock = MoveFrom1To2X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom1To2X);

                            MoveFrom1To2Y = new DoubleAnimation();
                            MoveFrom1To2Y.From = stopTile1.Y - YMargin;
                            MoveFrom1To2Y.To = stopTile2.Y - YMargin;
                            MoveFrom1To2Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom1To2Y.AutoReverse = false;
                            MoveFrom1To2Y.RepeatBehavior = new RepeatBehavior(2);

                            Storyboard.SetTarget(MoveFrom1To2Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom1To2Y, new PropertyPath(Canvas.TopProperty));

                             MoveFrom1To2YClock = MoveFrom1To2Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom1To2Y);
                        }
                        else if(endIndex == 2)
                        {
                            //1-3
                            MoveFrom1To3X = new DoubleAnimation();
                            MoveFrom1To3X.From = stopTile1.X - XMargin;
                            MoveFrom1To3X.To = stopTile3.X - XMargin;
                            MoveFrom1To3X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom1To3X.AutoReverse = false;
                            MoveFrom1To3X.RepeatBehavior = new RepeatBehavior(1);

                            Storyboard.SetTarget(MoveFrom1To3X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom1To3X, new PropertyPath(Canvas.LeftProperty));

                             MoveFrom1To3XClock = MoveFrom1To3X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom1To3X);

                            MoveFrom1To3Y = new DoubleAnimation();
                            MoveFrom1To3Y.From = stopTile1.Y - YMargin;
                            MoveFrom1To3Y.To = stopTile3.Y - YMargin;
                            MoveFrom1To3Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom1To3Y.AutoReverse = false;
                            MoveFrom1To3Y.RepeatBehavior = new RepeatBehavior(2);

                            Storyboard.SetTarget(MoveFrom1To3Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom1To3Y, new PropertyPath(Canvas.TopProperty));

                             MoveFrom1To3YClock = MoveFrom1To3Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom1To3Y);
                        }
                    }
                    else if(startIndex == 1)
                    {

                        //2-3
                        MoveFrom2To3X = new DoubleAnimation();
                        MoveFrom2To3X.From = stopTile2.X - XMargin;
                        MoveFrom2To3X.To = stopTile3.X - XMargin;
                        MoveFrom2To3X.Duration = new Duration(TimeSpan.FromSeconds(durationTime/2));
                        MoveFrom2To3X.AutoReverse = false;
                        MoveFrom2To3X.RepeatBehavior = new RepeatBehavior(4);

                        Storyboard.SetTarget(MoveFrom2To3X, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom2To3X, new PropertyPath(Canvas.LeftProperty));

                         MoveFrom2To3XClock = MoveFrom2To3X.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom2To3X);

                        MoveFrom2To3Y = new DoubleAnimation();
                        MoveFrom2To3Y.From = stopTile2.Y - YMargin;
                        MoveFrom2To3Y.To = stopTile3.Y - YMargin;
                        MoveFrom2To3Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime/2));
                        MoveFrom2To3Y.AutoReverse = false;
                        MoveFrom2To3Y.RepeatBehavior = new RepeatBehavior(5);

                        Storyboard.SetTarget(MoveFrom2To3Y, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom2To3Y, new PropertyPath(Canvas.TopProperty));

                         MoveFrom2To3YClock = MoveFrom2To3Y.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom2To3Y);

                        //3-4
                        MoveFrom3To4X = new DoubleAnimation();
                        MoveFrom3To4X.From = stopTile3.X - XMargin;
                        MoveFrom3To4X.To = stopTile4.X - XMargin;
                        MoveFrom3To4X.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom3To4X.AutoReverse = false;
                        MoveFrom3To4X.RepeatBehavior = new RepeatBehavior(6);

                        Storyboard.SetTarget(MoveFrom3To4X, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom3To4X, new PropertyPath(Canvas.LeftProperty));

                        MoveFrom3To4XClock = MoveFrom3To4X.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom3To4X);

                        MoveFrom3To4Y = new DoubleAnimation();
                        MoveFrom3To4Y.From = stopTile3.Y - YMargin;
                        MoveFrom3To4Y.To = stopTile4.Y - YMargin;
                        MoveFrom3To4Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom3To4Y.AutoReverse = false;
                        MoveFrom3To4Y.RepeatBehavior = new RepeatBehavior(7);

                        Storyboard.SetTarget(MoveFrom3To4Y, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom3To4Y, new PropertyPath(Canvas.TopProperty));

                        MoveFrom3To4YClock = MoveFrom3To4Y.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom3To4Y);
                    }
                    else if (startIndex == 2)
                    {
                        //3-4
                        MoveFrom3To4X = new DoubleAnimation();
                        MoveFrom3To4X.From = stopTile3.X - XMargin;
                        MoveFrom3To4X.To = stopTile4.X - XMargin;
                        MoveFrom3To4X.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom3To4X.AutoReverse = false;
                        MoveFrom3To4X.RepeatBehavior = new RepeatBehavior(6);

                        Storyboard.SetTarget(MoveFrom3To4X, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom3To4X, new PropertyPath(Canvas.LeftProperty));

                         MoveFrom3To4XClock = MoveFrom3To4X.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom3To4X);

                        MoveFrom3To4Y = new DoubleAnimation();
                        MoveFrom3To4Y.From = stopTile3.Y - YMargin;
                        MoveFrom3To4Y.To = stopTile4.Y - YMargin;
                        MoveFrom3To4Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom3To4Y.AutoReverse = false;
                        MoveFrom3To4Y.RepeatBehavior = new RepeatBehavior(7);

                        Storyboard.SetTarget(MoveFrom3To4Y, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom3To4Y, new PropertyPath(Canvas.TopProperty));

                         MoveFrom3To4YClock = MoveFrom3To4Y.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom3To4Y);

                        //4-5

                        MoveFrom4To5Y = new DoubleAnimation();
                        MoveFrom4To5Y.From = stopTile4.Y - YMargin;
                        MoveFrom4To5Y.To = stopTile5.Y - YMargin;
                        MoveFrom4To5Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom4To5Y.AutoReverse = false;
                        MoveFrom4To5Y.RepeatBehavior = new RepeatBehavior(9);

                        Storyboard.SetTarget(MoveFrom4To5Y, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom4To5Y, new PropertyPath(Canvas.LeftProperty));

                        MoveFrom4To5YClock = MoveFrom4To5Y.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom4To5Y);

                        MoveFrom4To5X = new DoubleAnimation();
                        MoveFrom4To5X.From = stopTile4.X - XMargin;
                        MoveFrom4To5X.To = stopTile5.X - XMargin;
                        MoveFrom4To5X.Duration = new Duration(TimeSpan.FromSeconds(durationTime / 2));
                        MoveFrom4To5X.AutoReverse = false;
                        MoveFrom4To5X.RepeatBehavior = new RepeatBehavior(10);

                        Storyboard.SetTarget(MoveFrom4To5X, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom4To5X, new PropertyPath(Canvas.TopProperty));

                        MoveFrom4To5XClock = MoveFrom4To5X.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom4To5X);
                    }
                    else if (startIndex == 3)
                    {
                        if (endIndex == 4)
                        {
                            //4-5

                            MoveFrom4To5Y = new DoubleAnimation();
                            MoveFrom4To5Y.From = stopTile4.Y - YMargin;
                            MoveFrom4To5Y.To = stopTile5.Y - YMargin;
                            MoveFrom4To5Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom4To5Y.AutoReverse = false;
                            MoveFrom4To5Y.RepeatBehavior = new RepeatBehavior(9);

                            Storyboard.SetTarget(MoveFrom4To5Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom4To5Y, new PropertyPath(Canvas.LeftProperty));

                             MoveFrom4To5YClock = MoveFrom4To5Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom4To5Y);

                            MoveFrom4To5X = new DoubleAnimation();
                            MoveFrom4To5X.From = stopTile4.X - XMargin;
                            MoveFrom4To5X.To = stopTile5.X - XMargin;
                            MoveFrom4To5X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom4To5X.AutoReverse = false;
                            MoveFrom4To5X.RepeatBehavior = new RepeatBehavior(10);

                            Storyboard.SetTarget(MoveFrom4To5X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom4To5X, new PropertyPath(Canvas.TopProperty));

                             MoveFrom4To5XClock = MoveFrom4To5X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom4To5X);

                        }
                        else if (endIndex == 5)
                        {
                            //4-6

                            MoveFrom4To6Y = new DoubleAnimation();
                            MoveFrom4To6Y.From = stopTile4.Y - YMargin;
                            MoveFrom4To6Y.To = stopTile6.Y - YMargin;
                            MoveFrom4To6Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom4To6Y.AutoReverse = false;
                            MoveFrom4To6Y.RepeatBehavior = new RepeatBehavior(9);

                            Storyboard.SetTarget(MoveFrom4To6Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom4To6Y, new PropertyPath(Canvas.LeftProperty));

                             MoveFrom4To6YClock = MoveFrom4To6Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom4To6Y);

                            MoveFrom4To6X = new DoubleAnimation();
                            MoveFrom4To6X.From = stopTile4.X - XMargin;
                            MoveFrom4To6X.To = stopTile5.X - XMargin;
                            MoveFrom4To6X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom4To6X.AutoReverse = false;
                            MoveFrom4To6X.RepeatBehavior = new RepeatBehavior(10);

                            Storyboard.SetTarget(MoveFrom4To6X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom4To6X, new PropertyPath(Canvas.TopProperty));

                             MoveFrom4To6XClock = MoveFrom4To6X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom4To6X);
                        }
                    }
                    else if (startIndex == 4)
                    {
                        //5-6

                        MoveFrom5To6Y = new DoubleAnimation();
                        MoveFrom5To6Y.From = stopTile5.Y - YMargin;
                        MoveFrom5To6Y.To = stopTile6.Y - YMargin;
                        MoveFrom5To6Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                        MoveFrom5To6Y.AutoReverse = false;
                        MoveFrom5To6Y.RepeatBehavior = new RepeatBehavior(9);

                        Storyboard.SetTarget(MoveFrom5To6Y, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom5To6Y, new PropertyPath(Canvas.LeftProperty));

                         MoveFrom5To6YClock = MoveFrom5To6Y.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom5To6Y);

                        MoveFrom5To6X = new DoubleAnimation();
                        MoveFrom5To6X.From = stopTile5.X - XMargin;
                        MoveFrom5To6X.To = stopTile6.X - XMargin;
                        MoveFrom5To6X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                        MoveFrom5To6X.AutoReverse = false;
                        MoveFrom5To6X.RepeatBehavior = new RepeatBehavior(10);

                        Storyboard.SetTarget(MoveFrom5To6X, movableTiles[0]);
                        Storyboard.SetTargetProperty(MoveFrom5To6X, new PropertyPath(Canvas.TopProperty));

                         MoveFrom5To6XClock = MoveFrom5To6X.CreateClock() as AnimationClock;

                        animations.Add(MoveFrom5To6X);
                    }
                    else if (startIndex == 5)
                    {
                        if (endIndex == 1)
                        {
                            //6-2

                            MoveFrom6To2Y = new DoubleAnimation();
                            MoveFrom6To2Y.From = stopTile6.Y - YMargin;
                            MoveFrom6To2Y.To = stopTile2.Y - YMargin;
                            MoveFrom6To2Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom6To2Y.AutoReverse = false;
                            MoveFrom6To2Y.RepeatBehavior = new RepeatBehavior(9);

                            Storyboard.SetTarget(MoveFrom6To2Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom6To2Y, new PropertyPath(Canvas.LeftProperty));

                             MoveFrom6To2YClock = MoveFrom6To2Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom5To6Y);

                            MoveFrom6To2X = new DoubleAnimation();
                            MoveFrom6To2X.From = stopTile6.X - XMargin;
                            MoveFrom6To2X.To = stopTile2.X - XMargin;
                            MoveFrom6To2X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom6To2X.AutoReverse = false;
                            MoveFrom6To2X.RepeatBehavior = new RepeatBehavior(10);

                            Storyboard.SetTarget(MoveFrom6To2X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom6To2X, new PropertyPath(Canvas.TopProperty));

                             MoveFrom6To2XClock = MoveFrom6To2X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom6To2X);
                        }
                        else if (endIndex == 2)
                        {
                            //6-3

                            MoveFrom6To3Y = new DoubleAnimation();
                            MoveFrom6To3Y.From = stopTile6.Y - YMargin;
                            MoveFrom6To3Y.To = stopTile3.Y - YMargin;
                            MoveFrom6To3Y.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom6To3Y.AutoReverse = false;
                            MoveFrom6To3Y.RepeatBehavior = new RepeatBehavior(9);

                            Storyboard.SetTarget(MoveFrom6To3Y, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom6To3Y, new PropertyPath(Canvas.LeftProperty));

                             MoveFrom6To3YClock = MoveFrom6To3Y.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom5To6Y);

                            MoveFrom6To3X = new DoubleAnimation();
                            MoveFrom6To3X.From = stopTile6.X - XMargin;
                            MoveFrom6To3X.To = stopTile3.X - XMargin;
                            MoveFrom6To3X.Duration = new Duration(TimeSpan.FromSeconds(durationTime));
                            MoveFrom6To3X.AutoReverse = false;
                            MoveFrom6To3X.RepeatBehavior = new RepeatBehavior(10);

                            Storyboard.SetTarget(MoveFrom6To3X, movableTiles[0]);
                            Storyboard.SetTargetProperty(MoveFrom6To3X, new PropertyPath(Canvas.TopProperty));

                             MoveFrom6To3XClock = MoveFrom6To3X.CreateClock() as AnimationClock;

                            animations.Add(MoveFrom6To2X);
                        }
                    }
                }
                else if (operation.Contains("Погрузка"))
                {
                    int inputIndex = int.Parse(operation[11].ToString());
                    if (inputIndex == 1)
                    {
                        //Погрузка 1
                        ColorAnimation resource1Input = new ColorAnimation();
                        resource1Input.From = Colors.White;
                        resource1Input.To = ((SolidColorBrush)resourceTiles[1].ResourceFigure.Fill).Color;
                        resource1Input.Duration = TimeSpan.FromSeconds(2);

                        resource1InputClock = resource1Input.CreateClock();

                    }
                    else if(inputIndex == 2)
                    {
                        ColorAnimation resource2Input = new ColorAnimation();
                        resource2Input.From = Colors.White;
                        resource2Input.To = ((SolidColorBrush)resourceTiles[3].ResourceFigure.Fill).Color;
                        resource2Input.Duration = TimeSpan.FromSeconds(2);

                        resource2InputClock = resource2Input.CreateClock();
                    }
                }
                else if (operation.Contains("Разгрузка"))
                {
                    int outputIndex = int.Parse(operation[12].ToString());
                    //Разгрузка
                    if(outputIndex == 1)
                    {
                        ColorAnimation resource1Output = new ColorAnimation();
                        resource1Output.From = ((SolidColorBrush)resourceTiles[1].ResourceFigure.Fill).Color;
                        resource1Output.To = Colors.White;
                        resource1Output.Duration = new Duration(TimeSpan.FromSeconds(2));

                        resource1OutputClock = resource1Output.CreateClock();
                    }
                    else if(outputIndex == 2)
                    {
                        ColorAnimation resource2Output = new ColorAnimation();
                        resource2Output.From = ((SolidColorBrush)resourceTiles[1].ResourceFigure.Fill).Color;
                        resource2Output.To = Colors.White;
                        resource2Output.Duration = new Duration(TimeSpan.FromSeconds(2));

                        resource1OutputClock = resource2Output.CreateClock();
                    }
                  
                }
              
            }
            clocks.Add(MoveFrom1To2XClock);
            clocks.Add(MoveFrom1To2YClock);
            clocks.Add(MoveFrom1To3XClock);
            clocks.Add(MoveFrom1To3YClock);
            clocks.Add(MoveFrom2To3XClock);
            clocks.Add(MoveFrom2To3YClock);
            clocks.Add(MoveFrom3To4XClock);
            clocks.Add(MoveFrom3To4YClock);
            clocks.Add(MoveFrom4To5XClock);
            clocks.Add(MoveFrom4To5YClock);
            clocks.Add(MoveFrom4To6XClock);
            clocks.Add(MoveFrom4To6YClock);
            clocks.Add(MoveFrom5To6XClock);
            clocks.Add(MoveFrom5To6YClock);
            clocks.Add(MoveFrom6To2XClock);
            clocks.Add(MoveFrom6To2YClock);
            clocks.Add(MoveFrom6To3XClock);
            clocks.Add(MoveFrom6To3YClock);
            clocks.Add(resource1InputClock);
            clocks.Add(resource1OutputClock);
            clocks.Add(resource2InputClock);
            clocks.Add(resource2OutputClock);
        }

        #region метод-заглушка,создающий пути сейчас
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

            //1-2

            MoveFrom1To2X = new DoubleAnimation();
            MoveFrom1To2X.From = stopTile1.X - XMargin;
            MoveFrom1To2X.To = stopTile2.X - XMargin;
            MoveFrom1To2X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom1To2X.AutoReverse = false;
            MoveFrom1To2X.RepeatBehavior = new RepeatBehavior(1);

            Storyboard.SetTarget(MoveFrom1To2X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom1To2X, new PropertyPath(Canvas.TopProperty));

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

            //1-3
            MoveFrom1To3X = new DoubleAnimation();
            MoveFrom1To3X.From = stopTile1.X - XMargin;
            MoveFrom1To3X.To = stopTile3.X - XMargin;
            MoveFrom1To3X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom1To3X.AutoReverse = false;
            MoveFrom1To3X.RepeatBehavior = new RepeatBehavior(1);

            Storyboard.SetTarget(MoveFrom1To3X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom1To3X, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom1To3XClock = MoveFrom1To3X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom1To3X);

            MoveFrom1To3Y = new DoubleAnimation();
            MoveFrom1To3Y.From = stopTile1.Y - YMargin;
            MoveFrom1To3Y.To = stopTile3.Y - YMargin;
            MoveFrom1To3Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom1To3Y.AutoReverse = false;
            MoveFrom1To3Y.RepeatBehavior = new RepeatBehavior(2);

            Storyboard.SetTarget(MoveFrom1To3Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom1To3Y, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom1To3YClock = MoveFrom1To3Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom1To3Y);

            //Погрузка
            DoubleAnimation resource1Input = new DoubleAnimation();
            resource1Input.From = movableTiles[0].ResourceFigure1.Height;
            resource1Input.To = movableTiles[0].ResourceFigure1.Height / 2;
            resource1Input.Duration = new Duration(TimeSpan.FromSeconds(2));
            resource1Input.AutoReverse = false;
            resource1Input.RepeatBehavior = new RepeatBehavior(3);


            AnimationClock resource1InputClock = resource1Input.CreateClock() as AnimationClock;

            animations.Add(resource1Input);

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
            DoubleAnimation resource1Output = new DoubleAnimation();
            resource1Output.From = movableTiles[0].ResourceFigure1.Width;
            resource1Output.To = movableTiles[0].ResourceFigure1.Width * 2;
            resource1Output.Duration = new Duration(TimeSpan.FromSeconds(2));
            resource1Output.AutoReverse = false;
            resource1Output.RepeatBehavior = new RepeatBehavior(8);

            AnimationClock resource1OutputClock = resource1Output.CreateClock();

            animations.Add(resource1Output);
            //4-5

            MoveFrom4To5Y = new DoubleAnimation();
            MoveFrom4To5Y.From = stopTile4.Y - YMargin;
            MoveFrom4To5Y.To = stopTile5.Y - YMargin;
            MoveFrom4To5Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom4To5Y.AutoReverse = false;
            MoveFrom4To5Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom4To5Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom4To5Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom4To5YClock = MoveFrom4To5Y.CreateClock() as AnimationClock;

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

            //4-6

            MoveFrom4To6Y = new DoubleAnimation();
            MoveFrom4To6Y.From = stopTile4.Y - YMargin;
            MoveFrom4To6Y.To = stopTile6.Y - YMargin;
            MoveFrom4To6Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom4To6Y.AutoReverse = false;
            MoveFrom4To6Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom4To6Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom4To6Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom4To6YClock = MoveFrom4To6Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom4To6Y);

            MoveFrom4To6X = new DoubleAnimation();
            MoveFrom4To6X.From = stopTile4.X - XMargin;
            MoveFrom4To6X.To = stopTile5.X - XMargin;
            MoveFrom4To6X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom4To6X.AutoReverse = false;
            MoveFrom4To6X.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(MoveFrom4To6X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom4To6X, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom4To6XClock = MoveFrom4To6X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom4To6X);

            //5-6

            MoveFrom5To6Y = new DoubleAnimation();
            MoveFrom5To6Y.From = stopTile5.Y - YMargin;
            MoveFrom5To6Y.To = stopTile6.Y - YMargin;
            MoveFrom5To6Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom5To6Y.AutoReverse = false;
            MoveFrom5To6Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom5To6Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom5To6Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom5To6YClock = MoveFrom5To6Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom5To6Y);

            MoveFrom5To6X = new DoubleAnimation();
            MoveFrom5To6X.From = stopTile5.X - XMargin;
            MoveFrom5To6X.To = stopTile6.X - XMargin;
            MoveFrom5To6X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom5To6X.AutoReverse = false;
            MoveFrom5To6X.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(MoveFrom5To6X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom5To6X, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom5To6XClock = MoveFrom5To6X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom5To6X);

            //6-2

            MoveFrom6To2Y = new DoubleAnimation();
            MoveFrom6To2Y.From = stopTile6.Y - YMargin;
            MoveFrom6To2Y.To = stopTile2.Y - YMargin;
            MoveFrom6To2Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom6To2Y.AutoReverse = false;
            MoveFrom6To2Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom6To2Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom6To2Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom6To2YClock = MoveFrom6To2Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom5To6Y);

            MoveFrom6To2X = new DoubleAnimation();
            MoveFrom6To2X.From = stopTile6.X - XMargin;
            MoveFrom6To2X.To = stopTile2.X - XMargin;
            MoveFrom6To2X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom6To2X.AutoReverse = false;
            MoveFrom6To2X.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(MoveFrom6To2X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom6To2X, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom6To2XClock = MoveFrom6To2X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom6To2X);

            //6-3

            MoveFrom6To3Y = new DoubleAnimation();
            MoveFrom6To3Y.From = stopTile6.Y - YMargin;
            MoveFrom6To3Y.To = stopTile3.Y - YMargin;
            MoveFrom6To3Y.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom6To3Y.AutoReverse = false;
            MoveFrom6To3Y.RepeatBehavior = new RepeatBehavior(9);

            Storyboard.SetTarget(MoveFrom6To3Y, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom6To3Y, new PropertyPath(Canvas.LeftProperty));

            AnimationClock MoveFrom6To3YClock = MoveFrom6To3Y.CreateClock() as AnimationClock;

            animations.Add(MoveFrom5To6Y);

            MoveFrom6To3X = new DoubleAnimation();
            MoveFrom6To3X.From = stopTile6.X - XMargin;
            MoveFrom6To3X.To = stopTile3.X - XMargin;
            MoveFrom6To3X.Duration = new Duration(TimeSpan.FromSeconds(2));
            MoveFrom6To3X.AutoReverse = false;
            MoveFrom6To3X.RepeatBehavior = new RepeatBehavior(10);

            Storyboard.SetTarget(MoveFrom6To3X, movableTiles[0]);
            Storyboard.SetTargetProperty(MoveFrom6To3X, new PropertyPath(Canvas.TopProperty));

            AnimationClock MoveFrom6To3XClock = MoveFrom6To3X.CreateClock() as AnimationClock;

            animations.Add(MoveFrom6To2X);


            clocks.Clear();

            clocks.Add(MoveFrom1To2XClock);
            clocks.Add(MoveFrom1To2YClock);
            clocks.Add(MoveFrom1To3XClock);
            clocks.Add(MoveFrom1To3YClock);
            clocks.Add(MoveFrom2To3XClock);
            clocks.Add(MoveFrom2To3YClock);
            clocks.Add(MoveFrom3To4XClock);
            clocks.Add(MoveFrom3To4YClock);
            clocks.Add(MoveFrom4To5XClock);
            clocks.Add(MoveFrom4To5YClock);
            clocks.Add(MoveFrom4To6XClock);
            clocks.Add(MoveFrom4To6YClock);
            clocks.Add(MoveFrom5To6XClock);
            clocks.Add(MoveFrom5To6YClock);
            clocks.Add(MoveFrom6To2XClock);
            clocks.Add(MoveFrom6To2YClock);
            clocks.Add(MoveFrom6To3XClock);
            clocks.Add(MoveFrom6To3YClock);
            clocks.Add(resource1InputClock);
            clocks.Add(resource1OutputClock);
            //clocks.Add(resource2InputClock);
            //clocks.Add(resource2OutputClock);

            clocks2.Add(MoveFrom1To2XClock);
            clocks2.Add(MoveFrom1To2YClock);
            clocks2.Add(MoveFrom2To3XClock);
            clocks2.Add(MoveFrom2To3YClock);
            clocks2.Add(MoveFrom3To4XClock);
            clocks2.Add(MoveFrom3To4YClock);
            clocks2.Add(MoveFrom4To5XClock);
            clocks2.Add(MoveFrom4To5YClock);


            Storyboard storyboard = new Storyboard();

            return storyboard;
        }
        #endregion

        public void Relocate(string operationName)
        {

            int movableTileIndex = int.Parse(operationName[0].ToString()) - 1;
            int startStopTileIndex = int.Parse(operationName[10].ToString()) - 1;
            int endStopTileIndex = int.Parse(operationName[12].ToString()) - 1;



            if (startStopTileIndex == 0)
            {
                if (endStopTileIndex == 1)
                {
                    complexesClocks[movableTileIndex].Add(clocks[0]);
                    complexesClocks[movableTileIndex].Add(clocks[1]);
                }
                else if (endStopTileIndex == 2)
                {
                    complexesClocks[movableTileIndex].Add(clocks[2]);
                    complexesClocks[movableTileIndex].Add(clocks[3]);
                }
            }
            else if (startStopTileIndex == 1)
            {
                    complexesClocks[movableTileIndex].Add(clocks[4]);
                    complexesClocks[movableTileIndex].Add(clocks[5]);
                    complexesClocks[movableTileIndex].Add(clocks[6]);
                    complexesClocks[movableTileIndex].Add(clocks[7]);
            }
            else if (startStopTileIndex == 2)
            {
                complexesClocks[movableTileIndex].Add(clocks[6]);
                complexesClocks[movableTileIndex].Add(clocks[7]);
                complexesClocks[movableTileIndex].Add(clocks[8]);
                complexesClocks[movableTileIndex].Add(clocks[9]);
            }
            else if (startStopTileIndex == 3)
            {
                if (endStopTileIndex == 4)
                {
                    complexesClocks[movableTileIndex].Add(clocks[8]);
                    complexesClocks[movableTileIndex].Add(clocks[9]);
                }
                else if (endStopTileIndex == 5)
                {
                    complexesClocks[movableTileIndex].Add(clocks[10]);
                    complexesClocks[movableTileIndex].Add(clocks[11]);
                }
            }
            else if (startStopTileIndex == 4)
            {
                    complexesClocks[movableTileIndex].Add(clocks[12]);
                    complexesClocks[movableTileIndex].Add(clocks[13]);   
            }
            else if (startStopTileIndex == 5)
            {
                if (endStopTileIndex == 1)
                {
                    complexesClocks[movableTileIndex].Add(clocks[14]);
                    complexesClocks[movableTileIndex].Add(clocks[15]);
                }
                else if (endStopTileIndex == 2)
                {
                    complexesClocks[movableTileIndex].Add(clocks[16]);
                    complexesClocks[movableTileIndex].Add(clocks[17]);
                }
            }

            //Vector start = VisualTreeHelper.GetOffset(stopTiles[(startStopTileIndex)]);
            //Vector end = VisualTreeHelper.GetOffset(stopTiles[(endStopTileIndex)]);
            //Vector lastInputTargetMargin = VisualTreeHelper.GetOffset(stopTiles[2]);


            //Point startPoint = new Point(start.X, start.Y);
            //Point endPoint = new Point(end.X, end.Y);

            //DoubleAnimation MoveFromToX = new DoubleAnimation();
            //MoveFromToX.From = startPoint.X - XMargin;
            //MoveFromToX.To = endPoint.X - XMargin;
            //MoveFromToX.Duration = new Duration(TimeSpan.FromSeconds(2));
            //MoveFromToX.AutoReverse = false;
            //MoveFromToX.RepeatBehavior = new RepeatBehavior(4);

            //Storyboard.SetTarget(MoveFromToX, movableTiles[0]);
            //Storyboard.SetTargetProperty(MoveFromToX, new PropertyPath(Canvas.LeftProperty));

            //AnimationClock MoveFromToXClock = MoveFromToX.CreateClock() as AnimationClock;



            //DoubleAnimation MoveFromToY = new DoubleAnimation();
            //MoveFromToY.From = startPoint.Y - YMargin;
            //MoveFromToY.To = endPoint.Y - YMargin;
            //MoveFromToY.Duration = new Duration(TimeSpan.FromSeconds(2));
            //MoveFromToY.AutoReverse = false;
            //MoveFromToY.RepeatBehavior = new RepeatBehavior(5);

            //Storyboard.SetTarget(MoveFromToY, movableTiles[0]);
            //Storyboard.SetTargetProperty(MoveFromToY, new PropertyPath(Canvas.TopProperty));

            //AnimationClock MoveFromToYClock = MoveFromToY.CreateClock() as AnimationClock;

            //clocks.Add(MoveFromToXClock);
            //clocks.Add(MoveFromToYClock);

        }

        public void Input(string operationName)
        {
            int movableTileIndex = int.Parse(operationName[0].ToString()) - 1;
            int inputOperationIndex = int.Parse(operationName[11].ToString());

            if(inputOperationIndex == 1)
            {
                complexesClocks[movableTileIndex].Add(clocks[18]);
            }
            else  if (inputOperationIndex == 2)
            {
                complexesClocks[movableTileIndex].Add(clocks[20]);
            }
           
        }

        public void Output(string operationName)
        {
            int movableTileIndex = int.Parse(operationName[0].ToString()) - 1;
            int outputOperationIndex = int.Parse(operationName[12].ToString());

            if (outputOperationIndex == 1)
            {
                complexesClocks[movableTileIndex].Add(clocks[19]);
            }
            else if (outputOperationIndex == 2)
            {
                complexesClocks[movableTileIndex].Add(clocks[21]);
            }
        }
    }
}
