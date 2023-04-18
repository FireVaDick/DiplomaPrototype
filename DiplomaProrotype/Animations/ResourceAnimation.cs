using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace DiplomaProrotype.Animations
{
    internal class ResourceAnimation
    {
        static public void ResourceHeightAnimation(int from, int to, int time, Thickness inputMargin, Thickness outputMargin)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);
            if (MainWindow.resourceTileFromContextMenu != null) 
                MainWindow.resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);

            thicknessAnimation.From = inputMargin;
            thicknessAnimation.To = outputMargin;
            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            if (MainWindow.resourceTileFromContextMenu != null)
                MainWindow.resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
        }

        static public void ResourceOnMovableHeightAnimation(int from, int to, int time, int inputTop, int outputTop)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            doubleAnimation.From = from;
            doubleAnimation.To = to;
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);

            if (MainWindow.movableTileFromContextMenu != null)
            {
                MainWindow.movableTileFromContextMenu.ResourceFigure1.BeginAnimation(Button.HeightProperty, doubleAnimation);
                MainWindow.movableTileFromContextMenu.ResourceFigure2.BeginAnimation(Button.HeightProperty, doubleAnimation);
                MainWindow.movableTileFromContextMenu.ResourceFigure3.BeginAnimation(Button.HeightProperty, doubleAnimation);
            }

            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            if (MainWindow.movableTileFromContextMenu != null)
            {
                thicknessAnimation.From = new Thickness(0, inputTop, 10, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, 10, 0);
                MainWindow.movableTileFromContextMenu.ResourceFigure1.BeginAnimation(Button.MarginProperty, thicknessAnimation);

                thicknessAnimation.From = new Thickness(0, inputTop, -12, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, -12, 0);
                MainWindow.movableTileFromContextMenu.ResourceFigure2.BeginAnimation(Button.MarginProperty, thicknessAnimation);

                thicknessAnimation.From = new Thickness(0, inputTop, -34, 0);
                thicknessAnimation.To = new Thickness(0, outputTop, -34, 0);
                MainWindow.movableTileFromContextMenu.ResourceFigure3.BeginAnimation(Button.MarginProperty, thicknessAnimation);
            }
        }
    }
}
