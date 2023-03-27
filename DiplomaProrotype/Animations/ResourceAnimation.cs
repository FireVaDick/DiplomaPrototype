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
            MainWindow.resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.HeightProperty, doubleAnimation);

            thicknessAnimation.From = inputMargin;
            thicknessAnimation.To = outputMargin;
            thicknessAnimation.Duration = TimeSpan.FromSeconds(time);
            MainWindow.resourceTileFromContextMenu.ResourceFigure.BeginAnimation(Button.MarginProperty, thicknessAnimation);
        }
    }
}
