using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Maze
{
    /// <summary>
    /// Success.xaml 的交互逻辑
    /// </summary>
    public partial class Success : Window
    {
        MainWindow mainWindow;
        public string name;
        public Success(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            name = userName.Text;
            // 检查是否重名
            if (config() == true)
            {
                MessageBox.Show("用户名已存在，请重新输入！");
                return;
            }
            if (mainWindow.Post() == true) 
            {
                // 打开排行榜
                Close();
                RankList d1 = new RankList(mainWindow);
                d1.ShowDialog();
            }
            else
            {
                MessageBox.Show("上传信息失败，请联系管理员");
            }
        }

        private bool config()
        {
            try
            {
                string s = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Desktop\\maze\\WPF\\Maze\\Maze\\data\\rank_list.mdf;Integrated Security=True;Connect Timeout=30";
                SqlConnection conn = new SqlConnection(s); //创建一个连接实例
                string queryStr = "SELECT NAME FROM Users";
                SqlCommand query = new SqlCommand(queryStr, conn);
                conn.Open(); //打开连接
                SqlDataReader res = query.ExecuteReader();
                while (res.Read())
                {
                    if (res.GetSqlString(0).ToString() == name) return true;
                }
                conn.Close();
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
