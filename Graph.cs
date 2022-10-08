using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace homework
{
    /// <summary>
    /// 邻接表
    /// </summary>
    public class Edge
    {
        public int s;//source
        public int t;//target
        public int w;//weight
        public int depth;
        //depth用于绘图，判定层数

        public Edge(int s, int t, int w)
        {
            this.s = s;
            this.t = t;
            this.w = w;
            this.depth = 0;
        }
    }

    /// <summary>
    /// 图的存储与基本算法
    /// </summary>
    public class Graph
    {

        public int count;/// 总结点个数
        public List<List<Edge>> nodeList;///邻接表
        Queue<int> que;// 

        public Graph(int count)
        {
            this.count = count;
            nodeList = new List<List<Edge>>();
            for (int i = 0; i <= count; i++) // 初始化时给邻接表开辟空间
            {
                nodeList.Add(new List<Edge>());
                nodeList[i].Add(new Edge(0,0,0));
            }
            return;
        }



        /// <summary>
        /// 加边
        /// </summary>
        /// <param name="s">起点</param>
        /// <param name="t">终点</param>
        /// <param name="w">权值</param>
        public void add(int s, int t, int w)
        {
            nodeList[s].Add(new Edge(s, t, w));
            //数组下标和实际数量的差别
        }

        /// <summary>
        /// 获取图的列表
        /// </summary>
        /// <returns>邻接表</returns>
        public List<List<Edge>> getList() 
        {
            return nodeList;
        }

        //欲实现的算法：BFS，DFS，等代价，深度受限，迭代加深，最佳优先搜索
        //已实现：BFS


        /// <summary>
        /// 用于绘制一个树状图的bfs
        /// </summary>
        /// <param name="s">起点</param>
        /// <returns></returns>
        public List<List<Edge>> bfs(int s) 
        {
            List<List<Edge>> res = new List<List<Edge>>();
            que = new Queue<int>();
            int[] vis = new int[count];
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("bfs 失败：当前图为空");
                return null;
            }
            que.Enqueue(s); // 将首节点入队
            vis[s] = 1;
            for (int i = 0; i <= count; i++)
            {
                res.Add(new List<Edge>()); // 
            }
            while (que.Count > 0) 
            {
                int now = que.First();
                for (int i = 0; i < nodeList[now].Count; i++)
                {
                    if (vis[nodeList[now][i].t] == 0)
                    {
                        que.Enqueue(nodeList[now][i].t);// 将子节点入队列

                        vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 这里vis还表示节点在图中深度 (为父深度 + 1)

                        res[vis[nodeList[now][i].t]].Add(nodeList[now][i]); // 将边的终点添加到对应深度(层次)的返回列表中
                    }
                }
                que.Dequeue();// 首节点出队
            }
            return res;
        }

        

    }
}
