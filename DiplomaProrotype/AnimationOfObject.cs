using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DiplomaProrotype
{
    internal class AnimationOfObject
    {
        Canvas canvas;
        PathGeometry pathGeometry;
        Storyboard storyboard;
        public AnimationOfObject(Canvas canvas,PathGeometry pathGeometry, Storyboard storyboard) 
        {
            this.canvas = canvas;
            this.pathGeometry = pathGeometry;
            this.storyboard = storyboard;
        }

        public void Animation(List<Point> coordinates)
        {
            List<Point> points = coordinates; //Список координат по которым будет двигаться объект


            PathFigure pFigure = new PathFigure(); //Создание фигуры,описывающей движение анимации
            pFigure.StartPoint = new Point(35, 0); //Стартовая точка анимации

            for (int i = 0; i < points.Count; i++)
            {

                int X = (int)points[i].X;
                int Y = (int)points[i].Y;
                LineSegment lineSegment = new() //Создание линии с координатами конечной точки
                {
                    Point = new Point(X, Y)
                };
                pFigure.Segments.Add(lineSegment); //Добавление линии к фигуре,описывающей движение
            }


            pathGeometry.Figures.Clear(); //Очистка массива фигур на случай предыдущих данных

            pathGeometry.Figures.Add(pFigure); //Добавление фигуры в качестве пути движения

            pathGeometry.Figures.Clear();

            pathGeometry.Figures.Add(pFigure);
            storyboard.Begin();

        }
    }
}
