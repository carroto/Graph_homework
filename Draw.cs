using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework
{
    /// <summary>
    /// 存放箭头两边终点坐标的类型
    /// </summary>
    public struct ArrowPoints
    {
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
    }


    /// <summary>
    /// 绘图类
    /// </summary>
    public class Draw
    {   

        /// <summary>
        /// 为直线绘制箭头的函数
        /// </summary>
        /// <param name="hx">箭头顶点的横坐标</param>
        /// <param name="hy">箭头顶点的纵坐标</param>
        /// <param name="theta">箭头与直线的夹角</param>
        /// <param name="len">箭头的长度</param>
        /// <param name="k">直线的斜率</param>
        public static ArrowPoints DrawArrow(double hx, double hy, double theta, double len, double k)
        {
            ArrowPoints res = new ArrowPoints();


            return res;
        }
    }
}
