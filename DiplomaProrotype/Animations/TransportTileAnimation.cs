using DiplomaProrotype;
using DiplomaPrototype.ImitationalModelDataAnalysing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DiplomaPrototype.Animations
{
    internal class TransportTileAnimation
    {
        private Storyboard storyboard; // список Storyboard

        private static MovableTile movableTile;

        private int currentStoryboardIndex = 0; // индекс текущего Storyboard

        public TransportTileAnimation(Storyboard storyboard, MovableTile nmovableTile, List<AnimationClock> clocks, List<DoubleAnimation> animations)
        {
            this.storyboard = storyboard;
            movableTile = nmovableTile;
            //StartNextStoryboard();
            movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[0]);


            clocks[0].Completed += (s, e) =>
            {             
                movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[1]);
            };
            clocks[1].Completed += (s, e) =>
            {
                movableTile.ResourceFigure1.ApplyAnimationClock(Button.HeightProperty, clocks[2]);
            };
            clocks[2].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[3]);
            };
            clocks[3].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[4]);
            };
            clocks[4].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[5]);
            };
            clocks[5].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[6]);
            };
            clocks[6].Completed += (s, e) =>
            {
                movableTile.ResourceFigure1.ApplyAnimationClock(Button.HeightProperty, clocks[7]);
            };
            clocks[7].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[8]);
            };
            clocks[8].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[9]);
            };
        }

        private void Storyboard_Completed(object sender, System.EventArgs e)
        {
            //StartNextStoryboard();
        }

        private void StartNextStoryboard()
        {
            BackgroundWorker worker = new BackgroundWorker();


            storyboard.Begin();


            Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) =>
            {
                storyboard.Pause();
                //MovableHeightAnimation(6, 25, 3, -15, -35);                      

            });

            // Продолжение выполнения Storyboard через 10 секунд
            Task.Delay(TimeSpan.FromSeconds(7)).ContinueWith((t) =>
            {
                storyboard.Resume();
            });


            Task.Delay(TimeSpan.FromSeconds(14.75)).ContinueWith((t) =>
            {
                storyboard.Pause();
                //ResourceAnimation.ResourceOnMovableHeightAnimation(25, 6, 3, -35, -15);
            });

            // Продолжение выполнения Storyboard через 10 секунд
            Task.Delay(TimeSpan.FromSeconds(20)).ContinueWith((t) =>
            {
                storyboard.Resume();
            });

            Task.Delay(TimeSpan.FromSeconds(29.75)).ContinueWith((t) =>
            {
                storyboard.Pause();
            });

            // Продолжение выполнения Storyboard через 10 секунд
            Task.Delay(TimeSpan.FromSeconds(35)).ContinueWith((t) =>
            {
                storyboard.Resume();
            });

            Task.Delay(TimeSpan.FromSeconds(42.75)).ContinueWith((t) =>
            {
                storyboard.Pause();
            });

            // Продолжение выполнения Storyboard через 10 секунд
            Task.Delay(TimeSpan.FromSeconds(46)).ContinueWith((t) =>
            {
                storyboard.Resume();
            });

            //var timer = new System.Timers.Timer(2000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
        }


        static public void MovableHeightAnimation(int from, int to, int time, int inputTop, int outputTop)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);

            if (movableTile != null)
            {
                movableTile.ResourceFigure1.BeginAnimation(Button.HeightProperty, doubleAnimation);
                movableTile.ResourceFigure2.BeginAnimation(Button.HeightProperty, doubleAnimation);
                movableTile.ResourceFigure3.BeginAnimation(Button.HeightProperty, doubleAnimation);
            }

            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            if (movableTile != null)
            {
                thicknessAnimation.From = new Thickness(0, inputTop, 10, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, 10, 0);
                movableTile.ResourceFigure1.BeginAnimation(Button.MarginProperty, thicknessAnimation);

                thicknessAnimation.From = new Thickness(0, inputTop, -12, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, -12, 0);
                movableTile.ResourceFigure2.BeginAnimation(Button.MarginProperty, thicknessAnimation);

                thicknessAnimation.From = new Thickness(0, inputTop, -34, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, -34, 0);
                movableTile.ResourceFigure3.BeginAnimation(Button.MarginProperty, thicknessAnimation);
            }
        }

        //private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    // остановить Storyboard
        //    if (storyboard != null)
        //    {
        //        storyboard.Pause();
        //        // запустить таймер через 10 секунд для возобновления выполнения Storyboard
        //        var resumeTimer = new System.Timers.Timer(5000);
        //        resumeTimer.Elapsed += ResumeTimer_Elapsed;
        //        resumeTimer.Start();
        //    }
        //}

        //private void ResumeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    // возобновить выполнение Storyboard
        //    if (storyboard != null)
        //    {
        //        storyboard.Resume();

        //            var timer = new System.Timers.Timer(4000);
        //            timer.Elapsed += Timer2_Elapsed;
        //            timer.Start();
        //        }
        //}

        //    private void Timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //    {
        //        if (storyboard != null)
        //        {
        //            storyboard.Pause();
        //            // запустить таймер через 10 секунд для возобновления выполнения Storyboard
        //            var resumeTimer = new System.Timers.Timer(5000);
        //            resumeTimer.Elapsed += ResumeTimer2_Elapsed;
        //            resumeTimer.Start();
        //        }
        //    }

        //    private void ResumeTimer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //    {
        //        // возобновить выполнение Storyboard
        //        if (storyboard != null)
        //        {
        //            storyboard.Resume();
        //        }
        //    }
    }
}
