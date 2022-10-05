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



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (count_input.Text == "") MessageBox.Show("请输入节点总数");
            else if (node_input.Text == "") MessageBox.Show("请输入建图节点");
            else
            {
                
                int count = int.Parse(count_input.Text); /// 获得当前节点个数
                
                graph = new Graph(count);               // 初始化图
                int linecount = node_input.LineCount;
                for(int i = 0; i < linecount; i++)
                {
                    string s = node_input.GetLineText(i);
                    //count_input 为输入的节点个数
                    //posiible illegal input:
                    //number
                    //number-space
                    //number-space-number:2
                    //number-space-number-space:3

                    //right:number-space-number-space-number

                    string[] temp = s.Split(' ');
                    //MessageBox.Show("temp.length:" + temp.Length);
                    if(temp.Length != 3 || s == "\r\n")
                    {
                        MessageBox.Show("Input error!\r\n Do not end with 'Enter'\r\nInput include origin destination weight");
                        break;
                        return;
                    }
                    if (temp.Length == 3)
                    {
                        //针对第四种输入情形的特判
                        int last = s.LastIndexOf(' ');
                        if(last == s.Length - 1)
                        {
                            MessageBox.Show("input error");
                            break;
                            return;
                        }
                    }

                    int a = Convert.ToInt32(temp[0]);
                    int b = Convert.ToInt32(temp[1]);
                    int c = Convert.ToInt32(temp[2]);

                    //MessageBox.Show("a=" + a + "b=" + b + "c=" + c);

                    if (a > count || b > count || c > count)
                    {
                        MessageBox.Show("输入异常：建图节点框输入异常\r\n\r\n正确输入格式应为\r\n \t起点 终点 权值\r\n\t起点 终点 权值\r\n\t...");
                        break;
                        return;
                    }

                    graph.add(a, b, c);         // 加边
                    graph.add(b, a, c);
                    //MessageBox.Show("当前输入节点" + Convert.ToString(a) + " " + Convert.ToString(b));
                }


                
                List<List<Node>> res = graph.bfs(0);

                /// 测试bfs///////////////////////////////////////////
                string ans = "";
                for( int i = 0; i < res.Count; i++)
                {
                    for(int j = 0; j < res[i].Count; j++)
                    {
                        
                        if (j != 0)
                        {
                            
                            ans += "--";
                        }
                        ans += (res[i][j].t + 1).ToString();
                        ans = ans + ("(" + (res[i][j].s + 1).ToString() + ")");
                    }
                    ans += "\r\n";
                }
                MessageBox.Show(ans);
                ////////////////////////////////////////////

                Generate_Graph(res, 10, 10, count);

            }
            
        }


        public void Generate_Graph(List<List<Node>> source, int max_nodes, int max_depth, int count)
        {
            Ellipse e = new Ellipse();
            e.Width = e.Height = 50;
            e.Fill = new SolidColorBrush(Color.FromRgb(108, 165, 178));

            double y = 360 / 2;
            double x = 30;

            Canvas.SetLeft(e, x);Canvas.SetTop(e, y);
            playground.Children.Add(e);
        }
    }

}
