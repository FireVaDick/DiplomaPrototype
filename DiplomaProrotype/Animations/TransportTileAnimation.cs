using DiplomaProrotype;
using DiplomaPrototype.ImitationalModelDataAnalysing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DiplomaPrototype.Animations
{
    internal class TransportTileAnimation
    {
        private Storyboard storyboard; // список Storyboard

        private static MovableTile movableTile;

        private int currentStoryboardIndex = 0; // индекс текущего Storyboard

        private List<AnimationClock> nclocks;

        public TransportTileAnimation(Storyboard storyboard, MovableTile nmovableTile, List<AnimationClock> clocks, List<DoubleAnimation> animations, List<List<AnimationClock>> AnimationClocks)
        {
            this.storyboard = storyboard;
            movableTile = nmovableTile;

            List<string> animationNames = new List<string>();
            animationNames.Add("Переезд");
            animationNames.Add("Переезд");
            animationNames.Add("Погрузка");
            nclocks = clocks;
            //animationNames.Add("Погрузка");

            AnimateTile(movableTile, AnimationClocks[0], animationNames);
            //AnimateTile(MainWindow.movableTiles[1], AnimationClocks[1], animationNames);
            //StartNextStoryboard();
            //movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[0]);


            //clocks[0].Completed += (s, e) =>
            //{             
            //    movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[1]);
            //};
            //clocks[1].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Button.HeightProperty, clocks[4]);
            //};
            //clocks[2].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[5]);
            //};
            //clocks[3].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[4]);
            //};
            //clocks[4].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[5]);
            //};
            //clocks[5].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[6]);
            //};
            //clocks[6].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Button.HeightProperty, clocks[7]);
            //};
            //clocks[7].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)clocks[8]);
            //};
            //clocks[8].Completed += (s, e) =>
            //{
            //    movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)clocks[9]);
            //};
        }

        public void AnimateTile(MovableTile movableTile,List<AnimationClock> animationClocks, List<string> operations)
        {

            movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)animationClocks[0]);

            animationClocks[0].Completed += (s, e) =>
            {
                movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)animationClocks[1]);
            };
            animationClocks[1].Completed += (s, e) =>
            {
                Brush resourceFigureCopy = movableTile.ResourceFigure1.Fill.Clone();
                movableTile.ResourceFigure1.Fill = resourceFigureCopy;
                movableTile.ResourceFigure1.Fill.ApplyAnimationClock(SolidColorBrush.ColorProperty, (AnimationClock)animationClocks[2]);
                animationClocks[2].Controller.Begin();
            };


            //animationClocks[1].Completed += (s, e) =>
            //{
            //    movableTile.ResourceFigure1.ApplyAnimationClock(Rectangle.HeightProperty, (AnimationClock)animationClocks[2]);
            //};

            //for (int i = 1; i < operations.Count; i++)
            //{
            //    if (operations[i] == "Переезд" && operations[i - 1] == "Переезд")
            //    {
            //        animationClocks[i - 1].Completed += (s, e) =>
            //        {
            //            movableTile.ApplyAnimationClock(Canvas.TopProperty, (AnimationClock)animationClocks[1]);
            //        };                
            //    }
            //    else if (operations[i] == "Переезд" && operations[i - 1] != "Переезд")
            //    {
            //        animationClocks[i - 1].Completed += (s, e) =>
            //        {
            //            movableTile.ApplyAnimationClock(Canvas.LeftProperty, (AnimationClock)animationClocks[i]);
            //        };
            //    }
            //    else if (operations[i] == "Погрузка")
            //    {
            //        animationClocks[i - 1].Completed += (s, e) =>
            //        {
            //            movableTile.ResourceFigure1.ApplyAnimationClock(Button.HeightProperty, animationClocks[2]);
            //        };
            //    }
            //    else if (operations[i] == "Разгрузка")
            //    {
            //        animationClocks[i - 1].Completed += (s, e) => 
            //        { 
            //            movableTile.ResourceFigure1.ApplyAnimationClock(Button.HeightProperty, animationClocks[i]); 
            //        };
            //    }

            //}
        }

    }
}
