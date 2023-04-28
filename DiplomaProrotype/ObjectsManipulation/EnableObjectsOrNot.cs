using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProrotype.ObjectsManipulation
{
    internal class EnableObjectsOrNot
    {
        static private List<ResourceTile> resourceTiles = MainWindow.resourceTiles;
        static private List<MachineTile> machineTiles = MainWindow.machineTiles;
        static private List<MovableTile> movableTiles = MainWindow.movableTiles;
        static private List<StopTile> stopTiles = MainWindow.stopTiles;


        static public void SetAllObjectsToUnenabled()
        {
            for (int i = 0; i < resourceTiles.Count; i++)
            {
                resourceTiles[i].IsEnabled = false;
            }

            for (int i = 0; i < machineTiles.Count; i++)
            {
                machineTiles[i].IsEnabled = false;
            }

            for (int i = 0; i < movableTiles.Count; i++)
            {
                movableTiles[i].IsEnabled = false;
            }

            for (int i = 0; i < stopTiles.Count; i++)
            {
                stopTiles[i].IsEnabled = false;
            }
        }
        static public void SetAllObjectsToEnabled()
        {
            for (int i = 0; i < resourceTiles.Count; i++)
            {
                resourceTiles[i].IsEnabled = true;
            }

            for (int i = 0; i < machineTiles.Count; i++)
            {
                machineTiles[i].IsEnabled = true;
            }

            for (int i = 0; i < movableTiles.Count; i++)
            {
                movableTiles[i].IsEnabled = true;
            }

            for (int i = 0; i < stopTiles.Count; i++)
            {
                stopTiles[i].IsEnabled = true;
            }
        }
    }
}
