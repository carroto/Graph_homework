using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace homework
{
    public class Node
    {
        public int s;
        public int t;
        public int w;
        public int depth;
        public Node(int s, int t, int w)
        {
            this.s = s;
            this.t = t;
            this.w = w;
            this.depth = 0;
        }
    }



    public class Graph
    {
    /// 图的存储与基本算法


        public int count;/// 总结点个数
        public List<List<Node>> nodeList;///邻接表
        Queue<int> que;// 

        public Graph(int count)
        {
            this.count = count;
            nodeList = new List<List<Node>>();
            for (int i = 0; i < count; i++) // 初始化时给邻接表开辟空间
            {
                nodeList.Add(new List<Node>());
            }
            return;
        }


        public void add(int s, int t, int w)// 加边
        {
            nodeList[s - 1].Add(new Node(s - 1, t - 1, w));
        }


        public List<List<Node>> getList() /// 获取图的列表
        {
            return nodeList;
        }


        public List<List<Node>> bfs(int s) /// 建图bfs
        {
            List<List<Node>> res = new List<List<Node>>();
            que = new Queue<int>();
            int[] vis = new int[count];
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("bfs 失败：当前图为空");
                return null;
            }
            que.Enqueue(s); // 将首节点入队
            vis[s] = 1;
            for (int i = 0; i < count; i++)
            {
                res.Add(new List<Node>()); // 
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
