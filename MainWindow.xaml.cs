using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int SIZE = 16;
        DispatcherTimer timer;
        Random random;
        Point food;
        Point snake;
        int width, height;
        int stepx, stepy;
        int points = 0;
        int total = 100;
        int length = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            width = (int)CanvasMap.ActualWidth;
            height = (int)CanvasMap.ActualHeight;            
            random = new Random();
            timer = new DispatcherTimer();
            timer.Tick += GameRun;
            timer.Interval = new TimeSpan (10000);
            timer.IsEnabled = true;

            KeyDown += MainWindow_KeyDown;
            
            AddFood();
            
            snake = new Point(width / 2, height / 2);
            stepx = 0;
            stepy = 0;
            MoveSnake();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {   case (Key.Down):    stepx = 0; stepy = -1; break;
                case (Key.Up):      stepx = 0; stepy = +1; break;
                case (Key.Right):   stepx = +1; stepy = 0; break;
                case (Key.Left):    stepx = -1; stepy = 0; break;
            }
        }

        private void GameRun(object sender, EventArgs e)
        {
            MoveSnake();
            if (OutOfScreen(snake))
                GameOver();
            if (IsCross(snake, food))
                if (++points == total)
                {
                    CanvasMap.Children.RemoveAt(0);
                    GameWin();
                }
                else
                {
                    AddFood();
                }
        }

        

        private void GameWin()
        {
            MessageBox.Show("You win! Congratulations!", "Winner");
            Close();
        }

        private void GameOver()
        {
            MessageBox.Show("You loose");
            Close();
        }

        private void MoveSnake()
        {
            snake.X += stepx * 3;
            snake.Y += stepy * 3;
            Ellipse ellipse = CreateEllipse(snake, Brushes.Red);
            if (CanvasMap.Children.Count > length)
                CanvasMap.Children.RemoveAt(length);
            CanvasMap.Children.Insert(1, ellipse);

        }

        private void AddFood()
        {
            food = new Point(random.Next(width - SIZE), random.Next(height - SIZE));
            Ellipse ellipse = CreateEllipse(food, Brushes.Blue);
            if (CanvasMap.Children.Count > 0)
                CanvasMap.Children.RemoveAt(0);
            CanvasMap.Children.Insert(0, ellipse);
            length++;
        }

        private Ellipse CreateEllipse(Point point, Brush brush)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = SIZE;
            ellipse.Height = SIZE;
            ellipse.Fill = brush;
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetBottom(ellipse, point.Y);
            return ellipse;
        }

        private bool IsCross(Point A, Point B)
        {
            return (Math.Abs(A.X - B.X)) < SIZE &&
                   (Math.Abs(A.Y - B.Y)) < SIZE;
        }

        private bool OutOfScreen (Point A)
        {
            return A.X < 0 || A.X > width - SIZE ||
                   A.Y < 0 || A.Y > height - SIZE;

        }
    }
}
