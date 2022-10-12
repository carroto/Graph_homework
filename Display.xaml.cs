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
        Canvas _canvas;

        public partial class Quantity
        {
            public int count;
            public Quantity()
            {
                this.count = 0;
            }
        }
        Quantity execute_count = new Quantity();
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
                _canvas = c;
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

        private void Button_Click(object sender, RoutedEventArgs e)//复位 -- 按钮
        {
            outside.Children.Remove(playground);
            outside.Children.Add(_canvas);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//执行算法 -- 按钮
        {
            string choice = Algorithm.Text;
            //获取combobox中的当前值，进行分支判断

            if(Source_Node.Text == "" || Target_Node.Text == "")
            {//未输入信息的情况
                MessageBox.Show("输入信息不全");
                return;
            }

            int start = Convert.ToInt32(Source_Node.Text);
            int target = Convert.ToInt32(Target_Node.Text);

            int num_node = Graph.temp.count;//节点数目

            if(start > num_node || target > num_node || start < 1 || target < 1)
            {
                MessageBox.Show("节点输入范围错误.");
                return;
            }
            switch (choice){
                case "请选择算法":
                    MessageBox.Show("未选择算法！请重新选择！");
                    break;
                case "广度优先搜索":
                    List<List<Node>> tree_bfs = Graph.temp.Breadth_first_search(start, target);
                    Draw.Generate_tree(ref playground, Graph.temp, tree_bfs, Graph.temp.count);
                    break;
                case "深度优先搜索":
                    List<List<Node>> tree_dfs = Graph.temp.Deep_first_search(start,target);
                    Draw.Generate_tree(ref playground,Graph.temp, tree_dfs, Graph.temp.count);
                    break;
                case "深度受限搜索":
                    break;
                case "迭代加深搜索":
                    break;
                case "等代价搜索":
                    break;
                case "最佳优先搜索":
                    break;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//单步执行 -- 按钮
        {
            execute_count.count++;
            MessageBox.Show(Convert.ToString(execute_count.count));
        }
    }
}
