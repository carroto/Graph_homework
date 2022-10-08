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
        /// <param name="a">箭头与直线的夹角</param>
        /// <param name="len">箭头的长度</param>
        /// <param name="L">直线(用于计算斜率)</param>
        public static void DrawArrow(ref Canvas playground, double hx, double hy, double a, double len, Line L)
        {
            Line l1 = new Line();
            Line l2 = new Line();
            //testTarget(ref playground, hx, hy); // 测试箭头顶点位置是否正确



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
