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
                
                int count = int.Parse(count_input.Text);
                int linecount = node_input.LineCount;
                for(int i = 0; i < linecount; i++)
                {
                    string s = node_input.GetLineText(i);
                    int a = s[0] - '0'; int b = s[2] - '0';
                    MessageBox.Show("当前输入节点" + Convert.ToString(a) +" " + Convert.ToString(b));
                }
                
            }
            
        }
    }
    public class graph
    {
        //construct the data structure

        public void create()
        {
            //create graph by the input box
            //or it will be NULL
        }
        public void algorithm1()
        {
            //once choiced in the box, use this to travel
        }
        public void travel()
        {
            //parameter: choice in the front box:  algorithm   travel-way

            //while traveling, update the open-closed form
            //draw the node on the front page
        }
    }
}
