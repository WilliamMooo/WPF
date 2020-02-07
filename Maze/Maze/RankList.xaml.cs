using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Data;

namespace Maze
{
    /// <summary>
    /// RankList.xaml 的交互逻辑
    /// </summary>
    public partial class RankList : Window
    {
        private MainWindow mainWindow;
        private Record[] R;
        private List<Button> B;
        public RankList(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            init();
        }

        public class Record
        {
            private string name;
            private double score;
            private string route;
            private string walls;
            private string paths;
            public string getName() { return name; }
            public double getScore() { return score; }
            public string getRoute() { return route; }
            public string getWalls() { return walls; }
            public string getPaths() { return paths; }
            public Record(string n, double s, string r, string w, string p) { name = n; score = s; route = r; walls = w; paths = p; }
        }

        private void init()
        {
            // 读取数据库数据
            R = ReadRecord();
            // 按得分从高到低排序
            sortRecord(R);
            // 在窗体中显示数据
            int i = 1;
            B = new List<Button>();
            foreach(Record item in R)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(30);
                rank.RowDefinitions.Add(row);
                TextBlock name = new TextBlock();
                TextBlock score = new TextBlock();
                Button route = new Button();
                name.Text = R[i - 1].getName();
                score.Text = R[i - 1].getScore().ToString();
                route.Content = "查看路线";
                B.Add(route);
                route.Click += Show_Route;
                Grid.SetColumn(name, 0);
                Grid.SetColumn(score, 1);
                Grid.SetColumn(route, 2);
                rank.Children.Add(name);
                rank.Children.Add(score);
                rank.Children.Add(route);
                Grid.SetRow(name, i);
                Grid.SetRow(score, i);
                Grid.SetRow(route, i);
                i++;
            }
        }

        public void sortRecord(Record[] R)
        {
            int len = R.Length;
            Record temp;
            for (int i = 0; i < len; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (R[i].getScore() < R[j].getScore())
                    {
                        temp = R[i];
                        R[i] = R[j];
                        R[j] = temp;
                    }
                }
            }
        }

        public Record[] ReadRecord()
        {
            // 连接数据库
            string s = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Desktop\\maze\\WPF\\Maze\\Maze\\data\\rank_list.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection conn = new SqlConnection(s); //创建一个连接实例
            string queryStr = "SELECT * FROM Users";
            string countStr = "SELECT COUNT(*) FROM Users";
            SqlCommand query = new SqlCommand(queryStr, conn);
            SqlCommand count = new SqlCommand(countStr, conn);
            conn.Open(); //打开连接
            SqlDataReader counts = count.ExecuteReader();
            counts.Read();
            int len = counts.GetInt32(0);
            counts.Close();
            Record[] R = new Record[len]; // 记录
            SqlDataReader res = query.ExecuteReader();
            int i = 0;
            while (res.Read())
            {
                R[i] = new Record(res.GetSqlString(0).ToString(), Convert.ToDouble(res.GetSqlDouble(1).ToString()), res.GetSqlString(2).ToString(),
                    res.GetSqlString(3).ToString(), res.GetSqlString(4).ToString());
                i++;
            }
            conn.Close(); //关闭连接
            return R;
        }

        private void Show_Route(object sender, RoutedEventArgs e)
        {
            int len = R.Length;
            for (int i = 0; i < len; i++)
            {
                if (sender == B[i])
                {
                    Route d1 = new Route(R[i]);
                    d1.Show();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            DifficultySelect d1 = new DifficultySelect(mainWindow);
            d1.ShowDialog();
        }
    }
}
