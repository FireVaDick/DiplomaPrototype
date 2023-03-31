using DiplomaProrotype.ObjectsManipulation;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DiplomaProrotype.CanvasManipulation
{
    internal class Cleanup
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<MovableTile> movableTiles = MainWindow.movableTiles;


        static public void UndoLastElementPlacement()
        {
            Type type = mw.TargetCanvas.Children[^1].GetType();

            if (mw.TargetCanvas.Children.Count != 0)
            {
                if (type == typeof(Ellipse)) // Удаление линии с кружком
                {
                    mw.TargetCanvas.Children.RemoveAt(mw.TargetCanvas.Children.Count - 1);
                }

                mw.TargetCanvas.Children.RemoveAt(mw.TargetCanvas.Children.Count - 1);
            }

            EnableObjectsOrNot.SetAllObjectsToEnabled();
        }


        static public void CreateCleanupContextMenu()
        {
            var contextMenu = new ContextMenu();

            mw.Eraser.ContextMenu = contextMenu;

            var menuItemDeleteObjects = new MenuItem();
            menuItemDeleteObjects.Click += new RoutedEventHandler(CMDeleteObjects_Click);
            menuItemDeleteObjects.Header = "Удалить только объекты";

            var menuItemDeleteAll = new MenuItem();
            menuItemDeleteAll.Click += new RoutedEventHandler(CMDeleteAll_Click);
            menuItemDeleteAll.Header = "Удалить всё";

            contextMenu.Items.Add(menuItemDeleteObjects);
            contextMenu.Items.Add(menuItemDeleteAll);
        }


        static private void CMDeleteObjects_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.linksCounter = 0;

            for (int i = 0; i < resourceTiles.Count;)
            {
                mw.TargetCanvas.Children.Remove(resourceTiles[i]);
                resourceTiles.Remove(resourceTiles[i]);
            }

            for (int i = 0; i < machineTiles.Count;)
            {
                mw.TargetCanvas.Children.Remove(machineTiles[i]);
                machineTiles.Remove(machineTiles[i]);
            }

            for (int i = 0; i < movableTiles.Count;)
            {
                mw.TargetCanvas.Children.Remove(movableTiles[i]);
                movableTiles.Remove(movableTiles[i]);
            }

            EnableObjectsOrNot.SetAllObjectsToEnabled();
        }

        static private void CMDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.linksCounter = 0;

            for (int i = 0; i < resourceTiles.Count;)
            {
                resourceTiles.Remove(resourceTiles[i]);
            }

            for (int i = 0; i < machineTiles.Count;)
            {
                machineTiles.Remove(machineTiles[i]);
            }

            for (int i = 0; i < movableTiles.Count;)
            {
                movableTiles.Remove(movableTiles[i]);
            }

            mw.TargetCanvas.Children.Clear();

            EnableObjectsOrNot.SetAllObjectsToEnabled();
        }
    }
}
