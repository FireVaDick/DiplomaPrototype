using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DiplomaProrotype.Animations
{
    internal class PanelAnimation
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private bool modeBorderOpen = true;
        static private bool objectBorderOpen = true;
        static private bool colorBorderOpen = false;


        static public void CreateAnimationWidth(Border border, double endValue, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = border.ActualWidth;
            animation.To = endValue;
            animation.Duration = TimeSpan.FromSeconds(duration);
            border.BeginAnimation(Button.WidthProperty, animation);
        }

        static public void CreateAnimationHeight(Border border, double endValue, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = border.ActualHeight;
            animation.To = endValue;
            animation.Duration = TimeSpan.FromSeconds(duration);
            border.BeginAnimation(Button.HeightProperty, animation);
        }


        static public void ModeBorderAnimation()
        {
            if (modeBorderOpen)
            {
                CreateAnimationWidth(mw.ModeBorder, 20, 0.3);
                modeBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(mw.ModeBorder, 100, 0.3);
                modeBorderOpen = true;
            }
        }

        static public void ObjectBorderAnimation()
        {
            if (objectBorderOpen)
            {
                CreateAnimationHeight(mw.ObjectBorder, 20, 0.3);
                objectBorderOpen = false;
            }
            else
            {
                CreateAnimationHeight(mw.ObjectBorder, 100, 0.3);
                objectBorderOpen = true;
            }
        }

        static public void ColorBorderAnimation()
        {
            if (colorBorderOpen)
            {
                CreateAnimationWidth(mw.ColorBorder, 35.5, 0.7);
                colorBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(mw.ColorBorder, 350, 0.7);
                colorBorderOpen = true;
            }
        }

    }
}
