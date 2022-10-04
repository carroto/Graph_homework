﻿using System;
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
        public int t;
        public int w;
        public Node(int t, int w)
        {
            this.t = t;
            this.w = w;
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
            nodeList[s-1].Add(new Node(t-1, w));
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
            if(nodeList == null)
            {
                MessageBox.Show("bfs 失败：当前图为空");
                return null;
            }
            que.Enqueue(s);
            vis[s] = 1;
            
           while(que.Count > 0)
            {
                int now = que.First();
                res.Add(new List<Node>());
                for(int i = 0; i < nodeList[now].Count; i++)
                {
                    res[res.Count-1].Add(nodeList[now][i]);

                    if (vis[nodeList[now][i].t] == 0)
                    {
                        que.Enqueue(nodeList[now][i].t);
                        vis[nodeList[now][i].t] = 1;
                    }
                }
                que.Dequeue();
            }
            return res;
        }
    }
}
