using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiplomaPrototype.ImitationalModelDataAnalysing
{
    internal class RoutesCreating
    {
        PathFigure figure  = new PathFigure();
        PathGeometry routeFrom1To2;

       
        PathGeometry routeFrom2To4;
        PathGeometry routeFrom6To2;
        PathGeometry routeFrom6To3;
        PathGeometry routeFrom7To2;
        public RoutesCreating() 
        {
            DataReading.Read();
            string[] operations = DataReading.routes.Split('\n');

            //foreach (string operation in operations)
            //{
            //    if(operation.Contains("Переезд 1-2"))
            //    {

            //    }
            //    else if (operation.Contains("Погрузка"))
            //    {

            //    }
            //    else if (operation.Contains("Разгрузка"))
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            LineSegment lineSegment = new() //Создание линии с координатами конечной точки
            {
                Point = new Point(0, 0)
            };
            figure.Segments.Add(lineSegment);

        }
    }
}
