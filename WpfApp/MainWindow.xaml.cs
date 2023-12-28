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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int points;
        public MainWindow()
        {
            InitializeComponent();
        }
        // создание точки
        private void CreatePoint(Point p)
        {
            // по сути наша точка
            Ellipse elipse = new Ellipse();
            // параметры точки как я понял ширина и высота
            elipse.Width = 3;
            elipse.Height = 3;
            // толщина периметра, ограничивающего фигуру
            elipse.StrokeThickness = 3;
            // цвет
            elipse.Stroke = Brushes.Black;
            // создание самой точки где координаты - лево, верх, право и низ
            elipse.Margin = new Thickness(p.X, p.Y, 0, 0);

            // вывод точки
            cnvs.Children.Add(elipse);
        }
        // функция для изменения текста с кол-вом точек
        private void AddPoint() => txtblock.Text = $"Количество точек: {++points}";

        // функция проверки для стартовой точки
        private bool CheckIfPointInside(Point p, Point[] triangle)
        {
            var (x, y) = (p.X, p.Y);
            var (x1, y1) = (triangle[0].X, triangle[0].Y);
            var (x2, y2) = (triangle[1].X, triangle[1].Y);
            var (x3, y3) = (triangle[2].X, triangle[2].Y);

            double d1 = (x - x1) * (y2 - y1) - (x2 - x1) * (y - y1);
            double d2 = (x - x2) * (y3 - y2) - (x3 - x2) * (y - y2);
            double d3 = (x - x3) * (y1 - y3) - (x1 - x3) * (y - y3);

            return !(d1 < 0 && d2 < 0 && d3 < 0) && !(d1 > 0 && d2 > 0 && d3 > 0);
        }
        private async void btn_Click(object sender, RoutedEventArgs e)
        {
            txtblock.Text = $"Количество точек: {points}";
            // стартовый треугольник (координаты - лево и низ)
            // верхняя точка, левая точка и правая точка
            Point[] starttrinagle = { new Point(360, 0), new Point(200, 200), new Point(500, 200)};
            // отрисовка стартового треугольника
            foreach (Point i in starttrinagle)
            {
                // создание точки
                CreatePoint(i);
                // обновление кол-ва точек в текст боксе
                AddPoint();
                // перерыв после создания точки в 0.2 секунды 
                await Task.Delay(200);
            }
            // теперь мы ставим рандомную точку внутри треугольника
            Point startpoint = new Point();
            // для создания рандомных координат
            Random rnd = new Random();
            // проверяем внутри ли треугольника наша стартовая точка
            do
            {
                // пишем от 0 до макс. числа из нашего треугольника
                startpoint.X = rnd.Next(0, 501);
                startpoint.Y = rnd.Next(0, 201);
            }
            while (CheckIfPointInside(startpoint, starttrinagle));

            // создание точки
            CreatePoint(startpoint);
            // обновление кол-ва точек в текст боксе
            AddPoint();
            // перерыв после создания точки в 0.2 секунды 
            await Task.Delay(200);

            // тут уже создание самого треугольника
            while (true)
            {
                // создание треугольника методом хауса происходит так,
                // что мы берем и создаем треугольник, создаем рандомную точку внутри
                // и после этого повторяем следующие действия:
                
                // 1. берем одну из 3 стартовых точек
                var SelectStartPoint = starttrinagle[rnd.Next(0, 3)];
                // 2. рисуем точку между нашей выбраной стартовой и той что мы нарисовали
                // в нашем случае первой точкой является стартовая которая создается рандомно
                // тоесть первый раз мы берем и ставим точку между одной из 3 вершин и стартовой точкой

                //создаем точку которая будет между
                Point midpoint = new Point((SelectStartPoint.X+startpoint.X)/2, (SelectStartPoint.Y + startpoint.Y) / 2);
                // теперь нам нужно поменять координаты нашей startpoint на новые(которые у точки выше)
                // так как в отличии от SelectStartPoint наша startpoint уже создана и ее нужно просто менять
                // то мы просто сначала создадим точку между, а потом присвоем нашей startpoint ее координаты
                startpoint = midpoint;
                
                // создание точки
                CreatePoint(midpoint);
                // обновление кол-ва точек в текст боксе
                AddPoint();
                // перерыв после создания точки в 0.2 секунды 
                await Task.Delay(200);
            }
        }
    }
}
