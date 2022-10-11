using homework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
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
        public Edge(int s, int t, int w)
        {
            this.s = s;
            this.t = t;
            this.w = w;
        }
    }
    /// <summary>
    /// 搜索节点信息
    /// </summary>
    public class Node
    {
        public int id;
        public int depth;
        public double x;//坐标
        public double y;
        public List<int> child;
        //可能有多个子节点，故用列表存储
        public Node(int id,int depth)
        {
            this.id = id;
            this.depth=depth;
            this.child = new List<int>();
            this.child.Add(0);//index=0时，填充一个值，保证下标从1开始
        }
    }

    /// <summary>
    /// 图的存储与基本算法
    /// </summary>
    public class Graph
    {

        public int count;/// 总结点个数
        public List<List<Edge>> nodeList;///邻接表
        Queue<int> que;//bfs用的队列

        Queue<int> Open;
        Queue<int> Close;//方便起见，统一用队列
        public static Graph temp;

        public Graph(int count)
        {
            this.count = count;
            nodeList = new List<List<Edge>>();
            for (int i = 0; i <= count; i++) // 初始化时给邻接表开辟空间
            {
                //多分配一个空间，日后使用从1开始
                nodeList.Add(new List<Edge>());
                nodeList[i].Add(new Edge(0,0,0));//每个新开辟的行填充一个空列表，保证下标从1开始
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
        //算法通用参数:  起点s，终点t，是否单步执行
        //算法目标：搜索寻得一条路径，过程中存储搜索路径上的节点信息：序号，深度，子节点列表
        //同步更新 open表和 closed表的信息，在图中显示
        //返回值：邻接表res：深度为索引，List<List<Node>>，提示是否能得到一条路径

        //绘制搜索树时按照深度进行放置节点，按照子节点关系进行绘制


        
        
        /// <summary>
        /// 用于绘制一个树状图的bfs
        /// </summary>
        /// <param name="source">起点</param>
        /// <param name="target">终点</param>
        /// <returns></returns>
        public List<List<Node>> Breadth_first_search(int source,int target)
        {
           if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("BFS 失败：当前图为空");
                return null;
            }

            List<List<Node>> res = new List<List<Node>>();
            Open = new Queue<int>();
            Close = new Queue<int>();
            que = new Queue<int>();
            int[] vis = new int[count + 1];//标记是否访问，记录深度

            for (int i = 0; i <= count; i++)
            {
                //count 个节点。最大深度即为depth = count
                res.Add(new List<Node>()); // depth=i 的层
                res[i].Add(new Node(0, 0));
            }

            que.Enqueue(source); // 将首节点入队，深度标记为1
            Open.Enqueue(source);
            vis[source] = 1;  //vis有值代表访问过，值代表深度
            res[vis[source]].Add(new Node(source, vis[source]));


            while (que.Count > 0)
            {
                int now = que.First();
                Open.Dequeue();//open表第一个节点出队，加入closed表中


                for (int i = 1; i <= nodeList[now].Count - 1; i++)
                {
                    //对当前节点的子节点的操作：加入指定深度的res中
                    //对res[vis[now]][res[vis[now]].Count-1]的元素子节点，添加为当前节点的子节点
                    //将子节点加入open表中


                    if (vis[nodeList[now][i].t] == 0)//节点未遍历,亦即不在closed表中
                    {
                        Open.Enqueue(nodeList[now][i].t);//加入open表中,不管是不是目标节点
                                                         //Open.Enqueue(thisNode_id);

                        if (nodeList[now][i].t == target)//找到目标节点，存入res，更新有关信息即可
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)

                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中
                            res[vis[now]][res[vis[now]].Count - 1].child.Add(thisNode_id);//添加子节点的信息
                            MessageBox.Show("从节点 " + source + " 到节点 " + target + " 的通路找到了！");
                            return res;
                        }
                        else//不是目标节点
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)
                                                                                   //now == nodeList[now][i].s
                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中
                            res[vis[now]][res[vis[now]].Count - 1].child.Add(thisNode_id);//添加子节点的信息
                        }
                    }
                }
                que.Dequeue();// 首节点出队
            }
            MessageBox.Show("从节点 " + source + " 到节点 " + target + " 不存在通路");
            return res;

        }
        public List<List<Node>> Deep_first_search(int source, int target)
        {
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("DFS 失败：当前图为空");
                return null;
            }

            List<List<Node>> res = new List<List<Node>>();
            return res;


            Open = new Queue<int>();
            Close = new Queue<int>();
            int[] vis = new int[count + 1];

            for (int i = 0; i <= count; i++)
            {
                //count 个节点。最大深度即为depth = count
                res.Add(new List<Node>()); // depth=i 的层
                res[i].Add(new Node(0, 0));
            }
            vis[source] = 1;
            Open.Enqueue(source);
            res[vis[source]].Add(new Node(source, vis[source]));

            for (int i = 1; i <= count; i++)
            {
                //dfstravel();
            }

        }
        public List<List<Node>> Depth_limit_search(int source, int target)
        {
            List<List<Node>> res = new List<List<Node>>();
            return res;
        }
        public List<List<Node>> Uniform_cost_research(int source, int target)
        {
            List<List<Node>> res = new List<List<Node>>();
            return res;
        }
        public List<List<Node>> Iterative_deepening_search(int source, int target)
        {
            List<List<Node>> res = new List<List<Node>>();
            return res;
        }
        public List<List<Node>> Best_first_search(int source, int target)
        {
            List<List<Node>> res = new List<List<Node>>();
            return res;
        }

    }
}
