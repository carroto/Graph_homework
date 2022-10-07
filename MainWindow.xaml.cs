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

                int linecount = node_input.LineCount;//边数目，对此进行建边操作

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
                    if(directed.IsChecked == false) graph.add(b, a, c);
                    //MessageBox.Show("当前输入节点" + Convert.ToString(a) + " " + Convert.ToString(b));
                }

                List<List<Node>> res = graph.getList();

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
                
            }
            
        }

        struct graph_node
        {
            public int id;
            public double x;
            public double y;
            public graph_node(int id, double x, double y)
            {
                this.id = id;
                this.x = x;
                this.y = y;
            }
        }

        public void Generate_Graph(int count)
        {
            


            double R = 150; double x0 = 283; double y0 = 194;double r = 25;

            double du = 2 * 3.1415926535 / count;


            List<graph_node> graph_data = new List<graph_node>();
            

            for(int i = 1; i <= count; i++)
            {
                Ellipse e = new Ellipse();
                TextBlock l = new TextBlock();
                l.Text = Convert.ToString(i);
                l.FontSize = 20;


                double y = y0 - Math.Cos(du * i) * R;
                double x = x0 + Math.Sin(du * i) * R;
                //MessageBox.Show("创建位置：" + Convert.ToString(x - r) + " " + Convert.ToString(y - r));
                Canvas.SetLeft(e, x - r/2); Canvas.SetTop(e, y - r/2);
                Canvas.SetLeft(l, x); Canvas.SetTop(l, y);
                Canvas.SetZIndex(e, 1);
                Canvas.SetZIndex(l, 1);
                graph_data.Add(new graph_node(i, x + r / 2, y + r / 2));


                e.Width = e.Height = 2 * r;
                e.Fill = new SolidColorBrush(Color.FromRgb(108, 165, 178));
                playground.Children.Add(e);
                playground.Children.Add(l);
            }

            List<List<Node>> nodeslist = graph.getList();

            

            for(int i = 0; i < nodeslist.Count; i++)
            {

                if (nodeslist[i].Count != 0)
                {
                    for (int j = 0; j < nodeslist[i].Count; j++)
                    {
                        
                        //MessageBox.Show(Convert.ToString(nodeslist[i][j].t + 1));
                        
                        Line l = new Line(); // 该直线为图中连线
                        l.Stroke = Brushes.Black;
                        l.StrokeThickness = 3; 
                        l.X1 = graph_data[i].x;l.Y1 = graph_data[i].y;
                        l.X2 = graph_data[nodeslist[i][j].t].x; l.Y2 = graph_data[nodeslist[i][j].t].y;
                        Canvas.SetZIndex(l, 0);

                        ///////////--------------------------------------------------------------------//////////////

                        Ellipse mark = new Ellipse();
                        mark.Height = 5; mark.Width = 5;
                        double xm = l.X2 + (r / R) * (x0 - l.X2);
                        double ym = l.Y2 + (r / R) * (y0 - l.Y2);
                        mark.Fill = Brushes.Red;
                        Canvas.SetLeft(mark, xm); Canvas.SetTop(mark, ym);
                        Canvas.SetZIndex(mark, 9);
                        playground.Children.Add(mark);
                        

                        playground.Children.Add(l);
                        
                    }
                }
                    
                
            }

        }

        private void Test(object sender, RoutedEventArgs e)
        {
            int test_count = int.Parse(count_input.Text);
            playground.Children.Clear();
            Generate_Graph(test_count);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = new Display();
            t.ShowDialog();
        }
    }

}
