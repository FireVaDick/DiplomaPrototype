using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiplomaProrotype.CanvasManipulation
{
    internal class ModeChooser
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;


        static public void ChooseMoveMode()
        {
            MainWindow.currentMode = "move";

            mw.ModeMove.Foreground = Brushes.Crimson;
            mw.ModeLink.Foreground = Brushes.Black;
            mw.ModeRoute.Foreground = Brushes.Black;
            mw.ModePath.Foreground = Brushes.Black;
        }

        static public void ChooseLinkMode()
        {
            MainWindow.currentMode = "link";

            mw.ModeMove.Foreground = Brushes.Black;
            mw.ModeLink.Foreground = Brushes.Crimson;
            mw.ModeRoute.Foreground = Brushes.Black;
            mw.ModePath.Foreground = Brushes.Black;
        }

        static public void ChooseRouteMode()
        {
            MainWindow.currentMode = "route";

            mw.ModeMove.Foreground = Brushes.Black;
            mw.ModeLink.Foreground = Brushes.Black;
            mw.ModeRoute.Foreground = Brushes.Crimson;
            mw.ModePath.Foreground = Brushes.Black;
        }

        static public void ChoosePathMode()
        {
            MainWindow.currentMode = "path";

            mw.ModeMove.Foreground = Brushes.Black;
            mw.ModeLink.Foreground = Brushes.Black;
            mw.ModeRoute.Foreground = Brushes.Black;
            mw.ModePath.Foreground = Brushes.Crimson;
        }
    }
}
