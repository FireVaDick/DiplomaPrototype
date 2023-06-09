using DiplomaProrotype.Models;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiplomaProrotype.ColorsManipulation
{
    internal class ColorChooser
    {
        static private MainWindow mw = (MainWindow)Application.Current.MainWindow;

        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<Link> linksResourceMachine = MainWindow.links;


        static public void DynamicChooseColorFromPalette()
        {

            Type type = MainWindow.chosenOneObject.GetType();

            if (type == typeof(ResourceTile))
            {
                ((ResourceTile)MainWindow.chosenOneObject).ResourceFigure.Fill = mw.ColorPalette.SelectedBrush;

                for (int i = 0; i < linksResourceMachine.Count; i++)
                {
                    if (linksResourceMachine[i].FirstTargetType == "resource" && linksResourceMachine[i].FirstTargetListId == resourceTiles.IndexOf((ResourceTile)MainWindow.chosenOneObject))
                    {
                        linksResourceMachine[i].LineInfo.Stroke = mw.ColorPalette.SelectedBrush;
                    }

                    if (linksResourceMachine[i].LastTargetType == "resource" && linksResourceMachine[i].LastTargetListId == resourceTiles.IndexOf((ResourceTile)MainWindow.chosenOneObject))
                    {
                        linksResourceMachine[i].LineInfo.Stroke = mw.ColorPalette.SelectedBrush;
                    }
                }
            }

            /*if (lastTileType == "machine")
            {
                machineTiles[machineTiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }*/

            /*if (lastTileType == "movable")
            {
                movableTiles[movableTiles.Count - 1].Background = ColorPalette.SelectedBrush;
            }*/
        }
    }
}
