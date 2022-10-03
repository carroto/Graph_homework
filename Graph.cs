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
        public List<List<Node>> nodeList;
        public Graph(int count)
        {
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
    }
}
