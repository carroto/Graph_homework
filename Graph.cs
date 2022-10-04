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
        public int count;
        public List<List<Node>> nodeList;
        Queue<int> que;
        public Graph(int count)
        {
            this.count = count;
            nodeList = new List<List<Node>>();
            for (int i = 0; i < count; i++)
            {
                nodeList.Add(new List<Node>());
            }
        }
        public void add(int s, int t, int w)
        {
            nodeList[s - 1].Add(new Node(s - 1, t - 1, w));
        }
        public List<List<Node>> getList()
        {
            return nodeList;
        }
        public List<List<Node>> bfs(int s)
        {
            List<List<Node>> res = new List<List<Node>>();
            que = new Queue<int>();
            int[] vis = new int[count];
            if (nodeList == null)
            {
                MessageBox.Show("bfs 失败：当前图为空");
                return null;
            }
            que.Enqueue(s);
            vis[s] = 1;
            for (int i = 0; i < count; i++)
            {
                res.Add(new List<Node>());
            }
            while (que.Count > 0)
            {
                int now = que.First();

                for (int i = 0; i < nodeList[now].Count; i++)
                {

                    if (vis[nodeList[now][i].t] == 0)
                    {

                        que.Enqueue(nodeList[now][i].t);


                        vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;
                        res[vis[nodeList[now][i].t]].Add(nodeList[now][i]);
                    }
                }
                que.Dequeue();
            }
            return res;
        }

    }
}
