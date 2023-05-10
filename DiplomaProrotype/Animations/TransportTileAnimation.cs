using DiplomaProrotype;
using DiplomaProrotype.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiplomaPrototype.Animations
{
    internal class TransportTileAnimation
    {
        private Storyboard storyboard; // список Storyboard

        private MovableTile movableTile;

        private int currentStoryboardIndex = 0; // индекс текущего Storyboard

        public TransportTileAnimation(Storyboard storyboard, MovableTile movableTile)
        {
            this.storyboard = storyboard;
            StartNextStoryboard();
        }       

        private void Storyboard_Completed(object sender, System.EventArgs e)
        {
            //StartNextStoryboard();
        }

        private void StartNextStoryboard()
        {
            storyboard.Begin();


            Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) =>
            {
                storyboard.Pause();
                ResourceAnimation.ResourceOnMovableHeightAnimation(6, 25, 3, -15, -35);
            });

            // Продолжение выполнения Storyboard через 10 секунд
            Task.Delay(TimeSpan.FromSeconds(7)).ContinueWith((t) =>
            {
                storyboard.Resume();
            });


            Task.Delay(TimeSpan.FromSeconds(14.75)).ContinueWith((t) =>
            {
                storyboard.Pause();
                ResourceAnimation.ResourceOnMovableHeightAnimation(25, 6, 3, -35, -15);
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
