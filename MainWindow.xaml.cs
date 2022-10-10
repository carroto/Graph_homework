using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph;
        
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)//建图函数
        {
            if (count_input.Text == "") MessageBox.Show("请输入节点总数");
            else if (node_input.Text == "") MessageBox.Show("请输入建图节点");
            else
            {
                
                int count = int.Parse(count_input.Text); /// 获得当前节点个数
        
                graph = new Graph(count);               // 初始化图

                int linecount = node_input.LineCount;//边数目，对此进行建边操作

                for(int i = 0; i < linecount; i++)
                {
                    string s = node_input.GetLineText(i);
                    //count_input 为输入的节点个数

                    //posiible illegal input:
                    //number
                    //number-space:2
                    //number-space-number:2
                    //number-space-number-space:3

                    //right:number-space-number-space-number3

                    string[] temp = s.Split(' ');
                    if((temp.Length != 3 && temp.Length != 2) || s == "\r\n")
                    {
                        //第一种错误输入，特殊错误输入
                        MessageBox.Show("Input error!\r\n Do not end with 'Enter'\r\nInput include origin destination （weight）");
                        break;
                        return;
                    }
                    int a = 0; int b = 0; int c = 0;
                    if (temp.Length == 3)
                    {
                        //针对第四种输入情形的特判，输入错误
                        int last = s.LastIndexOf(' ');
                        if(last == s.Length - 1)
                        {
                            MessageBox.Show("Input error!\r\n Do not end with 'Enter'\r\nInput include origin destination （weight）");
                            break;
                            return;
                        }
                        a = Convert.ToInt32(temp[0]);
                        b = Convert.ToInt32(temp[1]);
                        c = Convert.ToInt32(temp[2]);
                    }

                    if (temp.Length == 2)
                    {
                        //针对第二、三种输入情形的特判
                        //三 没有权重输入，可以生成
                        //二 输入错误，应当检查
                        int last = s.LastIndexOf(' ');
                        if (last == s.Length - 1)
                        {
                            MessageBox.Show("Input error!\r\n Do not end with 'Enter'\r\nInput include origin destination （weight）");
                            break;
                            return;
                        }
                        a = Convert.ToInt32(temp[0]);
                        b = Convert.ToInt32(temp[1]);
                        c = 1;
                    }


                    if (a > count || b > count)
                    {
                        MessageBox.Show("Input error!\r\n Do not end with 'Enter'\r\nInput include origin destination （weight）");
                        break;
                        return;
                    }

                    graph.add(a, b, c);         // 加边
                    if(directed.IsChecked == false) graph.add(b, a, c);
                }

                List<List<Edge>> res = graph.getList(); // 取建图邻接表

                Draw.Generate_Graph(ref playground, graph, ((directed.IsChecked == true) ? true : false), count);// 绘制图形

                //List<List<Node>> tree = graph.bfs(1,8,true);
                //Graph.temp = graph;
                //Draw.Generate_tree(ref playground, graph, tree, count);
                //MessageBox.Show("end");
            }
        }

        

        
        
        private void Button_Click(object sender, RoutedEventArgs e)//新界面进行树的相关操作
        {
            var t = new Display();
            t.ShowDialog();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int source = 1;
            int target = 4;
            graph.bfs(source, target, true);
        }
    }

}
