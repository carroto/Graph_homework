using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace homework
{
    /// <summary>
    /// 绘图类
    /// </summary>
    public class Draw
    {
        /// <summary>
        /// 建图节点
        /// </summary>
        struct graph_node
        {
            public int id;
            public double x;
            public double y;
            public graph_node(int id, double x, double y)
            {
                this.id = id;
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 初始界面绕圈绘图函数，
        /// </summary>
        /// <param name="playground">画布的引用</param>
        /// <param name="graph">图的邻接表</param>
        /// <param name="directed">是否为有向图</param>
        /// <param name="count">节点数</param>
        public static void Generate_Graph(ref Canvas playground, Graph graph, bool directed, int count)
        {
            playground.Children.Clear();
            double R = 150; double x0 = 283; double y0 = 194; double r = 25;

            double du = 2 * 3.1415926535 / count;

            List<graph_node> graph_data = new List<graph_node>();
            graph_data.Add(new graph_node());
            //画圆
            for (int i = 1; i <= count; i++)
            {
                Ellipse e = new Ellipse();
                TextBlock l = new TextBlock();
                l.Text = Convert.ToString(i);
                l.FontSize = 20;

                double y = y0 - Math.Cos(du * i) * R;
                double x = x0 + Math.Sin(du * i) * R;

                Canvas.SetLeft(e, x - r / 2); Canvas.SetTop(e, y - r / 2);
                Canvas.SetLeft(l, x); Canvas.SetTop(l, y);
                Canvas.SetZIndex(e, 1);
                Canvas.SetZIndex(l, 1);

                graph_data.Add(new graph_node(i, x + r / 2, y + r / 2));
                e.Width = e.Height = 2 * r;
                e.Fill = new SolidColorBrush(Color.FromRgb(108, 165, 178));
                playground.Children.Add(e);
                playground.Children.Add(l);
            }

            List<List<Edge>> nodeslist = graph.getList();

            //画直线和箭头
            for (int i = 1; i <= nodeslist.Count - 1; i++)
            {
                for (int j = 1; j <= nodeslist[i].Count - 1; j++)
                {
                    Line l = new Line(); // 该直线为图中连线
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 3;
                    l.X1 = graph_data[i].x; l.Y1 = graph_data[i].y; //指定起点和终点的坐标
                    l.X2 = graph_data[nodeslist[i][j].t].x; l.Y2 = graph_data[nodeslist[i][j].t].y;
                    Canvas.SetZIndex(l, 0);
                    ///////////---------------以下为确定箭头终点-----------------------------------------------------//////////////
                    double k = (l.Y2 - l.Y1) / (l.X2 - l.X1);
                    double xm = l.X2;
                    double ym = l.Y2;
                    if (l.X1 < l.X2)
                    {
                        xm -= r * Math.Cos(Math.Atan(k));
                        ym -= r * Math.Sin(Math.Atan(k));
                    }
                    else
                    {
                        xm += r * Math.Cos(Math.Atan(k));
                        ym += r * Math.Sin(Math.Atan(k));
                    }
                    playground.Children.Add(l);
                    if (directed == true) Draw.DrawArrow(ref playground, xm, ym, Draw.PI / 6, 12, l);
                }
            }
        }

        /// <summary>
        /// 圆周率
        /// </summary>
        public static double PI = 3.1415926535;

        /// <summary>
        /// 为直线绘制箭头的函数，直接将目标画布的引用传入完成绘制
        /// (在循环画边的时候使用)
        /// </summary>
        /// <param name="playground">目标画布的引用</param>
        /// <param name="hx">箭头顶点的横坐标</param>
        /// <param name="hy">箭头顶点的纵坐标</param>
        /// <param name="theta">箭头与直线的夹角</param>
        /// <param name="a">箭头的长度</param>
        /// <param name="L">直线(用于计算斜率)</param>
        public static void DrawArrow(ref Canvas playground, double hx, double hy, double theta, double a, Line L)
        {
            Line l1 = new Line();
            Line l2 = new Line();

            l1.X1 = hx; l1.Y1 = hy; l2.X1 = hx; l2.Y1 = hy;
            double Xa = 0; double Ya = 0; double Xb = 0; double Yb = 0;
            double k = (L.Y1 - L.Y2) / (L.X1 - L.X2); // 直线的斜率
            double alpha = Math.Atan(k);

            if (hx > L.X1) // 一四象限
            {
                Xa = hx - a * Math.Sin(PI / 2 - alpha - theta);
                Ya = hy - a * Math.Cos(PI / 2 - alpha - theta);
                Xb = hx - a * Math.Sin(PI / 2 - alpha + theta);
                Yb = hy - a * Math.Cos(PI / 2 - alpha + theta);
            }
            else
            {   // 二三象限
                Xa = hx + a * Math.Sin(PI / 2 - alpha - theta);
                Ya = hy + a * Math.Cos(PI / 2 - alpha - theta);
                Xb = hx + a * Math.Sin(PI / 2 - alpha + theta);
                Yb = hy + a * Math.Cos(PI / 2 - alpha + theta);
            }

            l1.X2 = Xa; l1.Y2 = Ya; l2.X2 = Xb; l2.Y2 = Yb;
            l1.Stroke = Brushes.Black; l2.Stroke = Brushes.Black;
            l1.StrokeThickness = 3; l2.StrokeThickness = 3;
            Canvas.SetZIndex(l1, 10); Canvas.SetZIndex(l2, 10);
            playground.Children.Add(l1);
            playground.Children.Add(l2);

        }

        /// <summary>
        /// 绘制搜索树
        /// </summary>
        /// <param name="playground">目标画布的引用</param>
        /// <param name="graph">图</param>
        /// <param name="data">搜索节点信息表</param>
        /// <param name="count">节点数目</param>
        public static void Generate_tree(ref Canvas playground, Graph graph, List<List<Node>> data, int count)
        {
            playground.Children.Clear();

            double x0 = 30; double y0 = 200; double r = 25;
            double x_gap = 125; double y_gap = 80;//横向和竖向的偏移量

            //画圆，更新坐标信息
            for (int i = 1; i <= data.Count - 1; i++)
            {//对深度进行遍历
             //当前深度下的所有节点进行绘制
             //一个深度对应一个横坐标
             //当前深度下的节点个数不同对应不同的纵坐标

                int num = data[i].Count - 1;//该深度层的节点个数
                double x = x0 + (i - 1) * x_gap;
                double y = y0;
                double y_top = y0 - (num / 2) * y_gap;

                //画圆
                for (int j = 1; j <= data[i].Count - 1; j++)
                {
                    //从上往下依次排开
                    y = y_top + (j - 1) * y_gap;
                    //记录节点的坐标
                    data[i][j].x = x;
                    data[i][j].y = y;

                    Ellipse e = new Ellipse();
                    TextBlock l = new TextBlock();
                    l.Text = Convert.ToString(data[i][j].id);
                    l.FontSize = 20;

                    Canvas.SetLeft(e, x - r); Canvas.SetTop(e, y - r);
                    Canvas.SetLeft(l, x - r / 2); Canvas.SetTop(l, y - r / 2);
                    Canvas.SetZIndex(e, 1);
                    Canvas.SetZIndex(l, 1);

                    e.Width = e.Height = 2 * r;
                    e.Fill = new SolidColorBrush(Color.FromRgb(108, 165, 178));
                    playground.Children.Add(e);
                    playground.Children.Add(l);
                }
            }
            //画直线和箭头
            for (int i = 1; i <= data.Count - 1; i++)
            {// i 按深度遍历
                for (int j = 1; j <= data[i].Count - 1; j++)
                {
                    //j 遍历该深度下的节点
                    for (int k = 1; k <= data[i][j].child.Count - 1; k++)
                    {
                        //对每个节点及其子节点进行绘制直线和箭头
                        //k 遍历  当前子节点.child 中的节点
                        Line l = new Line(); // 该直线为图中连线
                        l.Stroke = Brushes.Black;
                        l.StrokeThickness = 3;

                        //指定起点和终点的坐标
                        l.X1 = data[i][j].x; l.Y1 = data[i][j].y;

//------------------------------------------------------------------------------------------------------------------------
                        //bug！作为一个通用逻辑，不应默认子节点在下一个深度层中，应当进行数据结构的修改存储在表中的下标

                        //终点：data[i+1]中的某一个，只知序号不知下标，无法索引
                        //终点，须在data[i+1]中按照值寻找，返回下标
                        /*int index = 0;
                        for (int t = 1; t <= data[i + 1].Count - 1; t++)
                        {
                            if (data[i + 1][t].id == data[i][j].child[k])
                            {
                                index = t;
                                //在i+1深度中找到了data[i][j]的子节点，其在data[i+1]中的下标为index
                                //故终点为data[i+1][index]
                                break;
                            }
                        }
                        l.X2 = data[i + 1][index].x; l.Y2 = data[i + 1][index].y;*/

                        int x_index = data[i][j].child[k].index_x;
                        int y_index = data[i][j].child[k].index_y;
                        l.X2 = data[x_index][y_index].x;
                        l.Y2 = data[x_index][y_index].y;
//----------------------------------------------------------------------------------------------------------------------
                        Canvas.SetZIndex(l, 0);
                        ///////////---------------以下为确定箭头终点-----------------------------------------------------//////////////
                        double K = (l.Y2 - l.Y1) / (l.X2 - l.X1);
                        //- -   右半区+下顶点正确
                        //即  l.x1 < l.x2 时结果正确
                        //+ +  左半区+上顶点正确
                        //即  l.x1 > l.x2 时结果正确
                        double xm = l.X2;
                        double ym = l.Y2;
                        if (l.X1 <= l.X2)
                        {
                            xm -= r * Math.Cos(Math.Atan(K));
                            ym -= r * Math.Sin(Math.Atan(K));
                        }
                        else
                        {
                            xm += r * Math.Cos(Math.Atan(K));
                            ym += r * Math.Sin(Math.Atan(K));
                        }
                        playground.Children.Add(l);
                        Draw.DrawArrow(ref playground, xm, ym, Draw.PI / 6, 12, l);
                    }
                }
            }
        }
    }
}

