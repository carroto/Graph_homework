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

        List<List<Node>> tree_bfs;
        List<List<Node>> tree_dfs;
        List<List<Node>> tree_dls;//深度受限
        List<List<Node>> tree_ucs;//等代价
        List<List<Node>> tree_ids;//迭代加深
        List<List<Node>> tree_be_fs;//最佳优先

        public partial class Quantity
        {
            public int count;
            public Quantity()
            {
                this.count = 0;
            }
        }
        Quantity Execute_Count = new Quantity();
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
                    tree_bfs = Graph.temp.Breadth_first_search(start, target);
                    Draw.Generate_tree(ref playground, Graph.temp, tree_bfs, Graph.temp.count);
                    break;
                case "深度优先搜索":
                    tree_dfs = Graph.temp.Deep_first_search(start,target);
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
        /// <summary>
        /// 根据单步执行的次数进行原位置上的覆盖式重绘
        /// 故在算法内部须维护一个搜索顺序表
        /// 利用空余的res【0】进行存储！
        /// </summary>
        /// <param name="playground"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public void Single_Step(ref Canvas playground,List<List<Node>> data,int Execute_Count)
        {

            //从data[0]获取遍历顺序，从data找到对应节点，重绘制指定个数的节点进行覆盖
            int index = 0;
            for(int i = 1; i <= Execute_Count; i++)
            {
                Node new_node = data[0][i];//只有id和depth信息，无x，y
                for(int j = 1; j <= data[new_node.depth].Count-1; j++)
                {
                    //找下标
                    if (data[new_node.depth][j].id == new_node.id)
                    {
                        index = j;
                    }
                }
                double x = data[new_node.depth][index].x;
                double y = data[new_node.depth][index].y;
                double r = 25;
                //开始画圆！！

                Ellipse e = new Ellipse();
                TextBlock l = new TextBlock();
                l.Text = Convert.ToString(data[0][i].id);
                l.FontSize = 20;

                //MessageBox.Show("创建位置：" + Convert.ToString(x - r) + " " + Convert.ToString(y - r));
                Canvas.SetLeft(e, x - r); Canvas.SetTop(e, y - r);
                Canvas.SetLeft(l, x - r / 2); Canvas.SetTop(l, y - r / 2);
                Canvas.SetZIndex(e, 1);
                Canvas.SetZIndex(l, 1);

                e.Width = e.Height = 2 * r;
                e.Fill = new SolidColorBrush(Color.FromRgb(0,255,255));
                playground.Children.Add(e);
                playground.Children.Add(l);
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)//单步执行 -- 按钮
        {
            //因为每次调用generate_tree时，playground都会clear一次（控件？）
            //
            Execute_Count.count++;
            //点击次数过多的判定！
            //用res【0】【0】.depth 存储搜索节点总数
     

            string choice = Algorithm.Text;
            switch (choice)
            {
                case "广度优先搜索":
                    //点击次数过多
                    if (Execute_Count.count > tree_bfs[0][0].depth)
                    {
                        MessageBox.Show("搜索过程已结束！");
                        return;
                    }
                    Draw.Generate_tree(ref playground, Graph.temp, tree_bfs, Graph.temp.count);
                    Single_Step(ref playground, tree_bfs, Execute_Count.count);
                    break;
                case "深度优先搜索":
                    Draw.Generate_tree(ref playground, Graph.temp, tree_dfs, Graph.temp.count);
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
    }
}
