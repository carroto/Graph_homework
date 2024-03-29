﻿using homework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public List<Children> child;
        //可能有多个子节点，故用列表存储
        public Node(int id,int depth)
        {
            this.id = id;
            this.depth=depth;
            this.child = new List<Children>();
            this.child.Add(new Children(0,0,0));//index=0时，填充一个值，保证下标从1开始
        }
    }
    /// <summary>
    /// 子节点信息，存储子节点id,在res数组中的横纵下标
    /// </summary>
    public class Children
    {
        public int id;
        public int index_x;
        public int index_y;
        public Children(int id, int index_x, int index_y)
        {
            this.id = id;
            this.index_x = index_x;
            this.index_y = index_y;
        }
    }
    /// <summary>
    /// 图的存储与基本算法
    /// </summary>
    public class Graph
    {

        public int count;/// 总结点个数
        public List<List<Edge>> nodeList;///邻接表
        public bool isbuild;

        Queue<int> que;//bfs用的队列

        Queue<int> Open;
        Queue<int> Close;//方便起见，统一用队列
        public static Graph temp;

        public Graph(int count)
        {
            this.count = count;

            nodeList = new List<List<Edge>>();
            isbuild = false;
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

        //绘制搜索树时按照深度进行放置节点，按照搜索的子节点关系进行绘制

        // 我们用的保存结果的二维List中，使用了空闲的res[0]保存遍历的节点顺序，使用res[0][0].depth保存遍历的节点总数
        
        
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
            res[0].Add(new Node(source, vis[source]));
            //新增：利用闲置的res[0]这一行，进行存储遍历顺序
            int num = 1;//表示节点遍历表的总数目


            while (que.Count > 0)
            {
                int now = que.First();
                Open.Dequeue();//open表第一个节点出队，加入closed表中


                for (int i = 1; i <= nodeList[now].Count - 1; i++)
                {
                    //对当前节点的子节点的操作：加入指定深度的res中
                    //对res[vis[now]][res[vis[now]].Count-1]的元素子节点，添加为当前节点的子节点
                    //将子节点加入open表中
                    if (vis[nodeList[now][i].t] == 0)//子节点未遍历,亦即不在closed表中
                    {
                        num++;
                       //加入open表，更新搜索表，遍历顺序
                        Open.Enqueue(nodeList[now][i].t);//加入open表中,不管是不是目标节点
                        

                        if (nodeList[now][i].t == target)//找到目标节点，存入res，更新有关信息即可
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)
                            res[0].Add(new Node(nodeList[now][i].t, vis[nodeList[now][i].t]));//遍历顺序记录

                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中

                           
                            //应当在res的vis[now]深度下找到 对应id的元素的下标，进行添加子节点信息
                            //对应的下标为 res[ vis[now] ] [?] .id = now
                            //在当前节点的child字段里加入子节点的信息
                            int child_index = 0;
                            for (int j = 1; j <= res[vis[now]].Count - 1; j++)
                            {
                                if (res[vis[now]][j].id == now)
                                {
                                    child_index = j;
                                }
                            }
                            int index_x = thisNode_dep;//当前子节点的深度，即在res中的x坐标
                            int index_y = res[thisNode_dep].Count - 1;//子节点是新加进来的，必然是最后一个
                            res[vis[now]][child_index].child.Add(new Children(thisNode_id, index_x, index_y));//添加子节点的信息

                            res[0][0].depth = num;
                            MessageBox.Show("从节点 " + source + " 到节点 " + target + " 的通路找到了！");
                            return res;
                        }
                        else//不是目标节点
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)
                                                                                   //now == nodeList[now][i].s
                            res[0].Add(new Node(nodeList[now][i].t, vis[nodeList[now][i].t]));//遍历顺序记录

                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中

                            //应当在res的vis[now]深度下找到 对应id的元素的下标，进行添加子节点信息
                            //对应的下标为 res[ vis[now] ] [?] .id = now
                            //在当前节点的child字段里加入子节点的信息
                            int child_index = 0;
                            for (int j = 1; j <= res[vis[now]].Count - 1; j++)
                            {
                                if (res[vis[now]][j].id == now)
                                {
                                    child_index = j;
                                }
                            }
                            int index_x = thisNode_dep;//当前子节点的深度，即在res中的x坐标
                            int index_y = res[thisNode_dep].Count - 1;//子节点是新加进来的，必然是最后一个
                            res[vis[now]][child_index].child.Add(new Children(thisNode_id,index_x,index_y));//添加子节点的信息
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

            Open = new Queue<int>();
            Close = new Queue<int>();

            int[] vis = new int[count + 1];
            int num = 0;//遍历表的总数目,遍历顺序
            Stack<int> temp = new Stack<int>();

            for (int i = 0; i <= count; i++)
            {
                //count 个节点。最大深度即为depth = count
                res.Add(new List<Node>()); // depth=i 的层
                res[i].Add(new Node(0, 0));
            }

            vis[source] = 1;
            Open.Enqueue(source);
            temp.Push(source);//压入起始的源节点

            // 目的：在后续的搜索树的绘制中按源位置关系（深度）放置节点
            //      按遍历的次序绘制箭头，形成搜索树
            res[0].Add(new Node(source, vis[source]));
            res[vis[source]].Add(new Node(source, vis[source]));

            while (temp.Count > 0)
            {
                //不能在子节点内部进行涉及到遍历顺序的操作--因为这是：dfs--栈
                //遍历顺序：即栈顶的出栈顺序0
                // 当前出栈的元素在--遍历顺序上的子节点应当是--下一个栈顶元素

                //保证res保存的是遍历过的节点，多余的子节点应在栈内部
                int now = temp.Peek();
                temp.Pop();
                num++;

                //首节点出栈，操作子节点
                for (int i = 1; i <= nodeList[now].Count-1; i++)
                {
                    if (vis[nodeList[now][i].t] == 0)
                    {
                        //子节点并未被访问过，压入栈中
                        temp.Push(nodeList[now][i].t);
                        
                        if (nodeList[now][i].t == target)
                        {
                            //找到子节点
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;

                            int thisnode_id = nodeList[now][i].t;
                            int thisnode_dep = vis[thisnode_id];

                            res[thisnode_dep].Add(new Node(thisnode_id, thisnode_dep));
                            res[0].Add(new Node(thisnode_id, thisnode_dep));//即将结束，必须增加这最后一个遍历的节点
                            res[0][0].depth = num;

                            //在当前节点的child字段中加入x，y下标
                            int x_index = thisnode_dep;
                            int y_index = res[thisnode_dep].Count-1;
                            for(int j = 1; j <= res[x_index].Count - 1; j++)
                            {
                                if (res[vis[now]][j].id == temp.Peek())
                                {
                                     y_index = j;
                                }
                            }
                            res[vis[now]][res[vis[now]].Count - 1].child.Add(new Children(temp.Peek(),x_index,y_index));

                            MessageBox.Show("从节点" + source + "到节点" + target + "的通路找到了。");
                            return res;
                        }
                        else//未找到子节点的情况
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;
                        }
                    }
                }
                res[0].Add(new Node(temp.Peek(), vis[temp.Peek()]));
                res[vis[temp.Peek()]].Add(new Node(temp.Peek(), vis[temp.Peek()]));

                //在当前节点的child字段中加入子节点信息--栈顶信息
                int peek_x_index = vis[temp.Peek()];
                int peek_y_index = 0;// res[vis[temp.peek]][?].id = temp.peek

                for(int j = 1; j <= res[peek_x_index].Count - 1; j++)
                {
                    if (res[peek_x_index][j].id == temp.Peek())
                    {
                        peek_y_index = j;
                    }
                }
                int now_index = 0;
                for(int i = 1; i <= res[vis[now]].Count - 1; i++)
                {
                    if(res[vis[now]][i].id == now)
                    {
                        now_index = i;
                    }
                }
                res[vis[now]][now_index].child.Add(new Children(temp.Peek(),peek_x_index,peek_y_index));
            }
            MessageBox.Show("通路不存在！");
            return res;
        }
        public List<List<Node>> Depth_limit_search(int source, int target)
        {
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("DFS 失败：当前图为空");
                return null;
            }

            int d = 3;//预设限制深度为3
            List<List<Node>> res = new List<List<Node>>();

            Open = new Queue<int>();
            Close = new Queue<int>();

            int[] vis = new int[count + 1];
            int num = 0;//遍历表的总数目,遍历顺序
            Stack<int> temp = new Stack<int>();

            for (int i = 0; i <= count; i++)
            {
                //count 个节点。最大深度即为depth = count
                res.Add(new List<Node>()); // depth=i 的层
                res[i].Add(new Node(0, 0));
            }

            vis[source] = 1;
            Open.Enqueue(source);
            //res[vis[source]].Add(new Node(source, vis[source]));
            temp.Push(source);//压入起始的源节点

            // 目的：在后续的搜索树的绘制中按源位置关系（深度）放置节点
            //      按遍历的次序绘制箭头，形成搜索树
            res[0].Add(new Node(source, vis[source]));
            res[vis[source]].Add(new Node(source, vis[source]));

            while (temp.Count > 0)
            {
                //不能在子节点内部进行涉及到遍历顺序的操作--因为这是：dfs--栈
                //遍历顺序：即栈顶的出栈顺序0
                // 当前出栈的元素在--遍历顺序上的子节点应当是--下一个栈顶元素

                //保证res保存的是遍历过的节点，多余的子节点应在栈内部
                int now = temp.Peek();
                temp.Pop();
                num++;
                if (vis[now] >= d)
                {
                    //深度受到限制，无法继续进行
                    break;
                }
                else
                {
                    //首节点出栈，操作子节点
                    for (int i = 1; i <= nodeList[now].Count - 1; i++)
                    {
                        if (vis[nodeList[now][i].t] == 0)
                        {
                            //子节点并未被访问过，压入栈中
                            temp.Push(nodeList[now][i].t);

                            if (nodeList[now][i].t == target)
                            {
                                //找到子节点
                                vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;

                                int thisnode_id = nodeList[now][i].t;
                                int thisnode_dep = vis[thisnode_id];

                                res[thisnode_dep].Add(new Node(thisnode_id, thisnode_dep));
                                res[0].Add(new Node(thisnode_id, thisnode_dep));//即将结束，必须增加这最后一个遍历的节点
                                res[0][0].depth = num;

                                //在当前节点的child字段中加入x，y下标
                                int x_index = thisnode_dep;
                                int y_index = res[thisnode_dep].Count - 1;
                                for (int j = 1; j <= res[x_index].Count - 1; j++)
                                {
                                    if (res[vis[now]][j].id == temp.Peek())
                                    {
                                        y_index = j;
                                    }
                                }
                                res[vis[now]][res[vis[now]].Count - 1].child.Add(new Children(temp.Peek(), x_index, y_index));

                                MessageBox.Show("从节点" + source + "到节点" + target + "的通路找到了。");
                                return res;
                            }
                            else//未找到子节点的情况
                            {
                                vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;
                            }
                        }
                    }
                    res[0].Add(new Node(temp.Peek(), vis[temp.Peek()]));
                    res[vis[temp.Peek()]].Add(new Node(temp.Peek(), vis[temp.Peek()]));

                    //在当前节点的child字段中加入子节点信息--栈顶信息
                    int peek_x_index = vis[temp.Peek()];
                    int peek_y_index = 0;// res[vis[temp.peek]][?].id = temp.peek

                    for (int j = 1; j <= res[peek_x_index].Count - 1; j++)
                    {
                        if (res[peek_x_index][j].id == temp.Peek())
                        {
                            peek_y_index = j;
                        }
                    }
                    int now_index = 0;
                    for (int i = 1; i <= res[vis[now]].Count - 1; i++)
                    {
                        if (res[vis[now]][i].id == now)
                        {
                            now_index = i;
                        }
                    }
                    res[vis[now]][now_index].child.Add(new Children(temp.Peek(), peek_x_index, peek_y_index));
                }
            }
            MessageBox.Show("限定深度 "+d+" 内的通路不存在！");
            return res;
        }
        public List<List<Node>> Uniform_cost_research(int source, int target)
        {
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("BFS 失败：当前图为空");
                return null;
            }
            int[] cost = new int[count + 1];//代价数组
            //代价函数等于原点和该点之间便利通路的权重之和
            //cost【子节点】 = cost【父节点】+ nodelist[father][child].w 

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
            res[0].Add(new Node(source, vis[source]));
            //新增：利用闲置的res[0]这一行，进行存储遍历顺序
            int num = 1;//表示节点遍历表的总数目
            cost[source] = 0;

            while (que.Count > 0)
            {
                int now = que.First();
                Open.Dequeue();//open表第一个节点出队，加入closed表中


                for (int i = 1; i <= nodeList[now].Count - 1; i++)
                {
                    //对当前节点的子节点的操作：加入指定深度的res中
                    //对res[vis[now]][res[vis[now]].Count-1]的元素子节点，添加为当前节点的子节点
                    //将子节点加入open表中
                    //更新代价
                    if (vis[nodeList[now][i].t] == 0)//子节点未遍历,亦即不在closed表中
                    {
                        num++;
                        //加入open表，更新搜索表，遍历顺序
                        Open.Enqueue(nodeList[now][i].t);//加入open表中,不管是不是目标节点
                        cost[nodeList[now][i].t] = cost[nodeList[now][i].s] + nodeList[now][i].w;

                        if (nodeList[now][i].t == target)//找到目标节点，存入res，更新有关信息即可
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)
                            res[0].Add(new Node(nodeList[now][i].t, vis[nodeList[now][i].t]));//遍历顺序记录

                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中


                            //应当在res的vis[now]深度下找到 对应id的元素的下标，进行添加子节点信息
                            //对应的下标为 res[ vis[now] ] [?] .id = now
                            //在当前节点的child字段里加入子节点的信息
                            int child_index = 0;
                            for (int j = 1; j <= res[vis[now]].Count - 1; j++)
                            {
                                if (res[vis[now]][j].id == now)
                                {
                                    child_index = j;
                                }
                            }
                            int index_x = thisNode_dep;//当前子节点的深度，即在res中的x坐标
                            int index_y = res[thisNode_dep].Count - 1;//子节点是新加进来的，必然是最后一个
                            res[vis[now]][child_index].child.Add(new Children(thisNode_id, index_x, index_y));//添加子节点的信息

                            res[0][0].depth = num;
                            MessageBox.Show("从节点 " + source + " 到节点 " + target + " 的通路找到了！");
                            return res;
                        }
                        else//不是目标节点
                        {
                            vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1; // 子节点深度 (为父深度 + 1)
                                                                                   //now == nodeList[now][i].s
                            res[0].Add(new Node(nodeList[now][i].t, vis[nodeList[now][i].t]));//遍历顺序记录

                            int thisNode_id = nodeList[now][i].t;
                            int thisNode_dep = vis[thisNode_id];
                            que.Enqueue(thisNode_id);

                            res[thisNode_dep].Add(new Node(thisNode_id, thisNode_dep));//子节点加入对应的深度中


                            //res[vis[now]][res[vis[now]].Count-1].child.Add(thisNode_id);//添加子节点的信息
                            //bug，需更改
                            //应当在res的vis[now]深度下找到 对应id的元素的下标，进行添加子节点信息
                            //对应的下标为 res[ vis[now] ] [?] .id = now

                            //在当前节点的child字段里加入子节点的信息
                            int child_index = 0;
                            for (int j = 1; j <= res[vis[now]].Count - 1; j++)
                            {
                                if (res[vis[now]][j].id == now)
                                {
                                    child_index = j;
                                }
                            }
                            int index_x = thisNode_dep;//当前子节点的深度，即在res中的x坐标
                            int index_y = res[thisNode_dep].Count - 1;//子节点是新加进来的，必然是最后一个
                            res[vis[now]][child_index].child.Add(new Children(thisNode_id, index_x, index_y));//添加子节点的信息
                        }
                    }
                }
                que.Dequeue();// 首节点出队
                //ReOrder_queue(que,cost);
            }

            MessageBox.Show("从节点 " + source + " 到节点 " + target + " 不存在通路");
            return res;

        }
        public List<List<Node>> Iterative_deepening_search(int source, int target)
        {
            if (nodeList == null)  // 特判:是否建图出错
            {
                MessageBox.Show("DFS 失败：当前图为空");
                return null;
            }

            int d = 3;//预设限制深度为3
            List<List<Node>> res = new List<List<Node>>();

            Open = new Queue<int>();
            Close = new Queue<int>();

            int[] vis = new int[count + 1];
            int num = 0;//遍历表的总数目,遍历顺序
            Stack<int> temp = new Stack<int>();

            for (int i = 0; i <= count; i++)
            {
                //count 个节点。最大深度即为depth = count
                res.Add(new List<Node>()); // depth=i 的层
                res[i].Add(new Node(0, 0));
            }

            vis[source] = 1;
            Open.Enqueue(source);
            //res[vis[source]].Add(new Node(source, vis[source]));
            temp.Push(source);//压入起始的源节点

            // 目的：在后续的搜索树的绘制中按源位置关系（深度）放置节点
            //      按遍历的次序绘制箭头，形成搜索树
            res[0].Add(new Node(source, vis[source]));
            res[vis[source]].Add(new Node(source, vis[source]));
            while (true)
            {
                int mid = 0;
                while (temp.Count > 0)
                {
                    //不能在子节点内部进行涉及到遍历顺序的操作--因为这是：dfs--栈
                    //遍历顺序：即栈顶的出栈顺序0
                    // 当前出栈的元素在--遍历顺序上的子节点应当是--下一个栈顶元素

                    //保证res保存的是遍历过的节点，多余的子节点应在栈内部
                    int now = temp.Peek();
                    temp.Pop();
                    num++;
                    if (vis[now] >= d)
                    {
                        mid = now;
                        break;
                    }

                    //首节点出栈，操作子节点
                    for (int i = 1; i <= nodeList[now].Count - 1; i++)
                    {
                        if (vis[nodeList[now][i].t] == 0)
                        {
                            //子节点并未被访问过，压入栈中
                            temp.Push(nodeList[now][i].t);

                            if (nodeList[now][i].t == target)
                            {
                                //找到子节点
                                vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;

                                int thisnode_id = nodeList[now][i].t;
                                int thisnode_dep = vis[thisnode_id];

                                res[thisnode_dep].Add(new Node(thisnode_id, thisnode_dep));
                                res[0].Add(new Node(thisnode_id, thisnode_dep));//即将结束，必须增加这最后一个遍历的节点
                                res[0][0].depth = num;

                                //在当前节点的child字段中加入x，y下标
                                int x_index = thisnode_dep;
                                int y_index = res[thisnode_dep].Count - 1;
                                for (int j = 1; j <= res[x_index].Count - 1; j++)
                                {
                                    if (res[vis[now]][j].id == temp.Peek())
                                    {
                                        y_index = j;
                                    }
                                }
                                res[vis[now]][res[vis[now]].Count - 1].child.Add(new Children(temp.Peek(), x_index, y_index));

                                MessageBox.Show("从节点" + source + "到节点" + target + "的通路找到了。");
                                return res;
                            }
                            else//未找到子节点的情况
                            {
                                vis[nodeList[now][i].t] = vis[nodeList[now][i].s] + 1;
                            }
                        }
                    }
                    if(temp.Count == 0)
                    {
                        MessageBox.Show("empty stack");
                        break;
                    }
                    res[0].Add(new Node(temp.Peek(), vis[temp.Peek()]));
                    res[vis[temp.Peek()]].Add(new Node(temp.Peek(), vis[temp.Peek()]));

                    //在当前节点的child字段中加入子节点信息--栈顶信息
                    int peek_x_index = vis[temp.Peek()];
                    int peek_y_index = 0;// res[vis[temp.peek]][?].id = temp.peek

                    for (int j = 1; j <= res[peek_x_index].Count - 1; j++)
                    {
                        if (res[peek_x_index][j].id == temp.Peek())
                        {
                            peek_y_index = j;
                        }
                    }
                    int now_index = 0;
                    for (int i = 1; i <= res[vis[now]].Count - 1; i++)
                    {
                        if (res[vis[now]][i].id == now)
                        {
                            now_index = i;
                        }
                    }
                    res[vis[now]][now_index].child.Add(new Children(temp.Peek(), peek_x_index, peek_y_index));
                }
                d = d + 1;
                MessageBox.Show("depth+1");
                temp.Push(mid);
                if(d > count)
                {
                    MessageBox.Show("无通路");
                    return res;
                }
            }
        }
        public List<List<Node>> Best_first_search(int source, int target)
        {
            List<List<Node>> res = new List<List<Node>>();
            return res;
        }


        /*public Queue<int> ReOrder_queue(Queue<int> data, int[] cost)
        {
            //将栈data的元素,按照cost数组的信息重新升序排列
     
            int[] cos = new int[data.Count];
            int[] id = new int[data.Count];
            List<Node> res = new List<Node>();
            res.Add(new Node(0, 0));

            int num = 0;
            while(data.Count > 0)
            {
                num++;
                id[num] = data.First();
                cos[num] = cost[id[num]];
                res.Add(new)

                data.Dequeue();
            }

            Array.Sort(cos);
            for(int j = 1;j<= cos.Length - 1; j++)
            {
                data.Enqueue(cos[j]);
            }
            return data;
        }*/
    }
}
