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
using System.Windows.Shapes;

namespace homework
{
    /// <summary>
    /// Display.xaml 的交互逻辑
    /// </summary>
    public partial class Display : Window
    {


        private bool _isMouseDown = false;
        Point _mouseDownPosition;
        Point _mouseDownControlPosition;

        public Display()
        {
            InitializeComponent();
            
        }

        private void test_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var c = sender as Canvas;
            _isMouseDown = true;
            _mouseDownPosition = e.GetPosition(this);
            _mouseDownControlPosition = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));
            c.CaptureMouse();

        }

        private void test_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                var c = sender as Canvas;
                var pos = e.GetPosition(this);
                var dp = pos - _mouseDownPosition;
                Canvas.SetLeft(c, _mouseDownControlPosition.X + dp.X);
                Canvas.SetTop(c, _mouseDownControlPosition.Y + dp.Y); 
            }

        }

        private void test_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var c = sender as Canvas;
            _isMouseDown = false;
            c.ReleaseMouseCapture();
        }

        private void test_Wheel(object sender, MouseWheelEventArgs e)
        {
            //MessageBox.Show("wheel up");

            Point currentPoint = e.GetPosition(outside);

            double s = ((double)e.Delta) / 1000 + 1;

            TransformGroup tg = playground.RenderTransform as TransformGroup;
            tg.Children.Add(new ScaleTransform(s, s, currentPoint.X, currentPoint.Y));
        }
    }
}
