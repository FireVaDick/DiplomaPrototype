using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DiplomaPrototype.Animations
{
    internal class Paths
    {
        public List<PathGeometry> paths;

        public void InitializePathFigure()
        {
            paths = new List<PathGeometry>();
        }
    }
}
