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
        static private bool matrixBorderOpen = true;
        static private bool clearBorderOpen = true;
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

        static public void CreateAnimationOpacity(Panel panel, double endValue, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = panel.Opacity;
            animation.To = endValue;
            animation.Duration = TimeSpan.FromSeconds(duration);
            panel.BeginAnimation(Button.OpacityProperty, animation);
        }


        static public void ModeBorderAnimation()
        {
            if (modeBorderOpen)
            {
                CreateAnimationWidth(mw.ModeBorder, 20, 0.3);
                CreateAnimationOpacity(mw.ModePanel, 0, 0.3);
                modeBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(mw.ModeBorder, 100, 0.3);
                CreateAnimationOpacity(mw.ModePanel, 1, 0.3);
                modeBorderOpen = true;
            }
        }

        static public void ObjectBorderAnimation()
        {
            if (objectBorderOpen)
            {
                CreateAnimationHeight(mw.ObjectBorder, 20, 0.3);
                CreateAnimationOpacity(mw.ObjectPanel, 0, 0.3);
                objectBorderOpen = false;
            }
            else
            {
                CreateAnimationHeight(mw.ObjectBorder, 100, 0.3);
                CreateAnimationOpacity(mw.ObjectPanel, 1, 0.3);
                objectBorderOpen = true;
            }
        }

        static public void MatrixBorderAnimation()
        {
            if (matrixBorderOpen)
            {
                CreateAnimationWidth(mw.MatrixBorder, 20, 0.3);
                CreateAnimationOpacity(mw.MatrixPanel, 0, 0.3);
                matrixBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(mw.MatrixBorder, 100, 0.3);
                CreateAnimationOpacity(mw.MatrixPanel, 1, 0.3);
                matrixBorderOpen = true;
            }
        }

        static public void ClearBorderAnimation()
        {
            if (clearBorderOpen)
            {
                CreateAnimationWidth(mw.ClearBorder, 20, 0.3);
                CreateAnimationOpacity(mw.ClearPanel, 0, 0.3);
                clearBorderOpen = false;
            }
            else
            {
                CreateAnimationWidth(mw.ClearBorder, 100, 0.3);
                CreateAnimationOpacity(mw.ClearPanel, 1, 0.3);
                clearBorderOpen = true;
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

        static public void ShowAllBordersAnimation()
        {
            modeBorderOpen = false;
            objectBorderOpen = false;
            matrixBorderOpen = false;
            clearBorderOpen = false;

            ModeBorderAnimation();
            ObjectBorderAnimation();
            MatrixBorderAnimation();
            ClearBorderAnimation();
        }

        static public void HideAllBordersAnimation()
        {
            modeBorderOpen = true;
            objectBorderOpen = true;
            matrixBorderOpen = true;
            clearBorderOpen = true;

            ModeBorderAnimation();
            ObjectBorderAnimation();
            MatrixBorderAnimation();
            ClearBorderAnimation();
        }
    }
}
