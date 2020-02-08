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
using System.Windows.Shapes;

namespace Maze
{
    /// <summary>
    /// Route.xaml 的交互逻辑
    /// </summary>
    public partial class Route : Window
    {
        public Route(RankList.Record R)
        {
            InitializeComponent();
            init(R);
        }

        // 重现玩家绘制过关路线
        private void init(RankList.Record R)
        {
            string[] walls = R.getWalls().Split(',');
            string[] paths = R.getPaths().Split(',');
            string[] route = R.getRoute().Split(',');
            string[] start = { "1", "1" };
            int Columns = Convert.ToInt32(walls[walls.Length - 3]) + 1;
            int Rows = Convert.ToInt32(walls[walls.Length - 2]) + 1;
            string[] end = { (Columns - 2).ToString(), (Rows - 2).ToString() };
            // 画出网格
            drawGrid(Columns, Rows);
            // 画墙
            Draw(walls, Brushes.Red);
            // 画通路
            Draw(paths, Brushes.Yellow);
            // 画过关路线
            Draw(route, Brushes.Pink);
            // 画起点
            Draw(start, Brushes.Blue);
            // 画终点
            Draw(end, Brushes.Black);
        }

        private void drawGrid(int Columns, int Rows)
        {
            for (int i = 0; i < Columns; i++) Maze.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < Rows; i++) Maze.RowDefinitions.Add(new RowDefinition());
        }

        private void Draw(string[] walls, Brush e)
        {
            for (int i = 0; i < walls.Length - 1; i += 2)
            {
                TextBlock newArea = new TextBlock();
                Maze.Children.Add(newArea);
                Grid.SetColumn(newArea, Convert.ToInt32(walls[i]));
                Grid.SetRow(newArea, Convert.ToInt32(walls[i+1]));
                newArea.Background = e;
            }
        }
    }
}
