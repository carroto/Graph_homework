using System;
using System.Collections.Generic;
using System.Linq;
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
                    string s = node_input.GetLineText(i);   // 根据输入建图
                    if(s.Length != 5 && s.Length != 6)
                    {
                        MessageBox.Show("输入异常：建图节点框输入异常\r\n\r\n正确输入格式应为\r\n \t起点 终点 权值\r\n\t起点 终点 权值\r\n\t...");
                        break;
                    }
                    int a = s[0] - '0'; int b = s[2] - '0';int c = s[4] - '0';// ！！！！根据输入字符串中获得起点,终点, 边权(待修改)
                    graph.add(a, b, c);         // 加边
                    graph.add(b, a, c);
                    //MessageBox.Show("当前输入节点" + Convert.ToString(a) +" " + Convert.ToString(b));
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

                Generate_Graph(res, 10);

            }
            
        }


        public void Generate_Graph(List<List<Node>> source, int max_nodes)
        {
            Ellipse e = new Ellipse();
            e.Width = e.Height = 10;
            e.Fill = new SolidColorBrush(Color.FromRgb(108, 165, 178));
            Canvas.SetLeft(e, 10);Canvas.SetTop(e, 10);
            playground.Children.Add(e);
        }
    }

}
