using SDKSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<Point> points = new() //Список координат по которым будет двигаться объект
            {
                new Point(135, 0),
                new Point(135, 200),
                new Point(200, 200),
                new Point(200, 300),
                new Point(300, 300),
                new Point(300, 0),
                new Point(35, 0)
            };
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

            TargetPathGeometry.Figures.Clear(); //Очистка массива фигур на случай предыдущих данных

            TargetPathGeometry.Figures.Add(pFigure); //Добавление фигуры в качестве пути движения

        }
    }
}
