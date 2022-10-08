using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;



namespace homework
{
    /// <summary>
    /// 绘图类
    /// </summary>
    public class Draw
    {
        /// <summary>
        /// 圆周率
        /// </summary>
        public static double PI = 3.1415926535;

        /// <summary>
        /// 存放箭头两边终点坐标的类型
        /// </summary>
        private struct ArrowPoints
        {
            public double X1;
            public double Y1;
            public double X2;
            public double Y2;
        }

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
            //testTarget(ref playground, hx, hy); // 测试箭头顶点位置是否正确
            l1.X1 = hx; l1.Y1 = hy; l2.X1 = hx; l2.Y1 = hy;
            double Xa = 0; double Ya = 0; double Xb = 0; double Yb = 0;
            double k = (L.Y1 - L.Y2) / (L.X1 - L.X2); // 直线的斜率
            double alpha = Math.Atan(k);

            if(hx > L.X1) // 一四象限
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
            
            l1.X2 = Xa;l1.Y2 = Ya; l2.X2 = Xb; l2.Y2 = Yb;
            l1.Stroke = Brushes.Black;l2.Stroke = Brushes.Black;
            l1.StrokeThickness = 3; l2.StrokeThickness = 3;
            Canvas.SetZIndex(l1, 10); Canvas.SetZIndex(l2, 10);
            playground.Children.Add(l1);
            playground.Children.Add(l2);

        }

        /// <summary>
        /// 测试箭头顶点位置函数
        /// </summary>
        /// <param name="playground">目标画布的引用</param>
        /// <param name="hx">顶点横坐标/param>
        /// <param name="hy">顶点纵坐标</param>
        private static void testTarget(ref Canvas playground, double hx, double hy)
        {
            Ellipse mark = new Ellipse();
            mark.Height = 5; mark.Width = 5;
            mark.Fill = Brushes.Red;
            Canvas.SetLeft(mark, hx); Canvas.SetTop(mark, hy);
            Canvas.SetZIndex(mark, 9);
            playground.Children.Add(mark);
        }
    }
}
