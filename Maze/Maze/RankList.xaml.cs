using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Maze
{
    /// <summary>
    /// RankList.xaml 的交互逻辑
    /// </summary>
    public partial class RankList : Window
    {
        MainWindow mainWindow;
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
            public string getName() { return name; }
            public double getScore() { return score; }
            public Record(string n, double s) { name = n; score = s; }
        }

        private void init()
        {
            // 读取数据库数据
            Record[] R = ReadRecord();
            // 按得分从高到低排序
            sortRecord(R);
            // 在窗体中显示数据
            int i = 1;
            foreach(Record item in R)
            {
                rank.RowDefinitions.Add(new RowDefinition());
                TextBlock name = new TextBlock();
                TextBlock score = new TextBlock();
                name.Text = R[i - 1].getName();
                score.Text = R[i - 1].getScore().ToString();
                Grid.SetColumn(name, 0);
                Grid.SetColumn(score, 1);
                rank.Children.Add(name);
                rank.Children.Add(score);
                Grid.SetRow(name, i);
                Grid.SetRow(score, i);
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
                R[i] = new Record(res.GetSqlString(0).ToString(), Convert.ToDouble(res.GetSqlDouble(1).ToString()));
                i++;
            }
            conn.Close(); //关闭连接
            return R;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            DifficultySelect d1 = new DifficultySelect(mainWindow);
            d1.ShowDialog();
        }
    }
}
