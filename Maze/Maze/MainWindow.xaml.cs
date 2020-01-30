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

namespace Maze
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Maze_Grid[,] g;
        private Player A;
        private Rectangle P;
        public MainWindow()
        {
            InitializeComponent();
            entry();
        }
        private class Point
        {
            protected int x;
            protected int y; // 表示网格的坐标
            public int X
            {
                get { return x; }
                set { if (value >= 0) x = value; }
            }
            public int Y
            {
                get { return y; }
                set { if (value >= 0) y = value; }
            }
            public Point(int xi, int yi) // 初始化坐标
            {
                x = xi;
                y = yi;
            }
        }

        private class Maze_Grid : Point
        {
            public bool isConnected { set; get; }
            public bool Visited { get; set; }
            public Maze_Grid(int xi, int yi) : base(xi, yi) { }
        }

        private class Player : Point
        {
            private int xout;
            private int yout;
            public double xstep { get; set; } // x轴上的步长
            public double ystep { get; set; } // y轴上的步长
            public void MoveToLeft() { x = x - 1; }
            public void MoveToTop() { y = y - 1; }
            public void MoveToRight() { x = x + 1; }
            public void MoveToBottom() { y = y + 1; }
            public Player(int xi, int yi, int xo, int yo) : base(xi, xi) { xout = xo; yout = yo; }
            public bool IsOut()
            {
                if (x == xout && y == yout) return true;
                else return false;
            }
        }

        // 第一次进入游戏
        private void entry()
        {
            // 先介绍游戏规则
            About_Click(about, null);
            // 进行难度选择
            DifficultySelect d1 = new DifficultySelect(this);
            d1.ShowDialog();
        }

        // 初始化迷宫游戏
        public void init(int difficulty)
        {
            // 根据难度确定迷宫大小
            int Columns = 0, Rows = 0;
            switch (difficulty)
            {
                case 0:
                    Columns = 13;
                    Rows = 9;
                    break;
                case 1:
                    Columns = 17;
                    Rows = 13;
                    break;
                case 2:
                    Columns = 31;
                    Rows = 21;
                    break;
                default:
                    break;
            }
            // 对网格初始化
            initGrid(Columns, Rows);
            // 用深度优先遍历法生成迷宫形状
            DFS(1, 1, Columns, Rows);
            // 画出迷宫
            drawMaze(Columns, Rows);
            // 初始化玩家
            initPlayer(1, 1, Columns, Rows);
            // 初始化终点
            initEnd(Columns, Rows);
        }

        // 初始化终点
        private void initEnd(int Columns, int Rows)
        {
            Rectangle E = new Rectangle();
            E.Fill = Brushes.Black;
            Thickness end = new Thickness(E.Margin.Left + (Columns - 2) * A.xstep, E.Margin.Top + (Rows - 2) * A.ystep, E.Margin.Left - (Columns - 2) * A.xstep, E.Margin.Top - (Rows - 2) * A.ystep);
            E.Margin = end;
            Maze.Children.Add(E);
        }

        // 初始化玩家
        private void initPlayer(int x, int y, int Columns, int Rows)
        {
            A = new Player(x, y, Columns-2, Rows-2);
            A.xstep = Maze.Width / Columns;
            A.ystep = Maze.Height / Rows;
            P = new Rectangle();
            P.Fill = Brushes.Blue;
            Thickness start = new Thickness(P.Margin.Left + A.xstep, P.Margin.Top + A.ystep, P.Margin.Right - A.xstep, P.Margin.Bottom - A.ystep);
            P.Margin = start;
            Maze.Children.Add(P);
        } 

        // 对网格初始化
        private void initGrid(int Columns, int Rows)
        {
            for (int i = 0; i < Columns; i++) Maze.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < Rows; i++) Maze.RowDefinitions.Add(new RowDefinition());
            g = new Maze_Grid[Columns, Rows];
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    g[i, j] = new Maze_Grid(i, j); // 初始化坐标
                    if (i == 0 || j == 0 || i + 1 == Columns || j + 1 == Rows) // 标边界
                    {
                        g[i, j].Visited = true;
                        g[i, j].isConnected = false;
                    }
                    else if (i % 2 == 0 || j % 2 == 0) // 标记墙
                    {
                        g[i, j].Visited = false;
                        g[i, j].isConnected = false;
                    }
                    else // 待打通的网格
                    {
                        g[i, j].Visited = false;
                        g[i, j].isConnected = true;
                    }
                }
            }
        }

        // 深度优先遍历法生成迷宫形状
        private void DFS(int xi, int yi, int xo, int yo)
        {
            Stack<Maze_Grid> st = new Stack<Maze_Grid>();
            st.Push(g[xi, yi]);
            Random rd = new Random();
            while (st.Count != 0)
            {
                Maze_Grid e = st.Pop(); // 出栈
                int[] direction = new int[4]; //  1, 2, 3, 4 分别代表左、上、右、下四个方向
                // 打乱数组
                int j = 0;
                bool exist = false;
                while (j < 4)
                {
                    exist = false;
                    int rand = rd.Next(1, 5);
                    foreach(int k in direction)
                    {
                        if (k == rand) exist = true;
                    }
                    if (!exist)
                    {
                        direction[j] = rand;
                        j++;
                    }
                }
                foreach(int i in direction)
                {
                    switch (i)
                    {
                        case 1:
                            if (e.X - 2 > 0 && e.X - 2 > 1 && g[e.X - 2, e.Y].Visited == false)
                            {
                                if (e.X - 2 > 0) g[e.X - 2, e.Y].Visited = true;
                                st.Push(g[e.X - 2, e.Y]); //左方网格进栈
                                g[e.X - 1, e.Y].isConnected = true; //打通两个方块
                            }
                            break;
                        case 2:
                            if (e.Y + 2 > 0 && e.Y + 2 < yo && g[e.X, e.Y + 2].Visited == false)
                            {
                                g[e.X, e.Y + 2].Visited = true;
                               st.Push(g[e.X, e.Y + 2]); //上方网格进栈
                                g[e.X, e.Y + 1].isConnected = true; //打通两个方块
                            }
                            break;
                        case 3:
                            if (e.X + 2 < xo && e.X + 2 < xo && g[e.X + 2, e.Y].Visited == false)
                            {
                                g[e.X + 2, e.Y].Visited = true;
                                st.Push(g[e.X + 2, e.Y]); // 右方网格进栈
                                g[e.X + 1, e.Y].isConnected = true; //打通两个方块
                            }
                            break;
                        case 4:
                            if (e.Y - 2 > 0 && e.Y - 2 < yo && g[e.X, e.Y - 2].Visited == false)
                            {
                                if (e.Y - 2 > 0) g[e.X, e.Y - 2].Visited = true;
                                st.Push(g[e.X, e.Y - 2]); // 下方网格进栈
                                g[e.X, e.Y - 1].isConnected = true; //打通两个方块
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            // 去除边缘多余的墙
            if (xo % 2 == 0)
            {
                for (int i = 1;  i < yo; i++)
                {
                    if (g[xo - 3, i].isConnected == true) g[xo-2, i].isConnected = true;
                }
            }
            if (yo % 2 == 0)
            {
                for (int i = 1; i < xo; i++)
                {
                    if (g[i, yo - 3].isConnected == true) g[i, yo - 2].isConnected = true;
                }
            }
        }

        // 画出迷宫
        private void drawMaze(int Columns, int Rows)
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    TextBlock newArea = new TextBlock();
                    //newArea.Text = i.ToString() + "," + j.ToString(); // 标出迷宫坐标
                    Maze.Children.Add(newArea);
                    Grid.SetColumn(newArea, i);
                    Grid.SetRow(newArea, j);
                    if (g[i, j].isConnected == false) newArea.Background = Brushes.Red;
                    else newArea.Background = Brushes.Yellow;
                }
            }
        }

        private void Recreate_Click(object sender, RoutedEventArgs e)
        {
            Maze.ColumnDefinitions.RemoveRange(0, g.GetLength(0));
            Maze.RowDefinitions.RemoveRange(0,g.GetLength(1));
            // 进行难度选择
            DifficultySelect d1 = new DifficultySelect(this);
            d1.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Thickness move;
            if (Keyboard.IsKeyDown(Key.Left)) // 左方向键
            {
                if (g[A.X-1,A.Y].isConnected == true)
                {
                    move = new Thickness(P.Margin.Left - A.xstep, P.Margin.Top, P.Margin.Right + A.xstep, P.Margin.Bottom);
                    A.MoveToLeft();
                    P.Margin = move;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Up)) // 上方向键
            {
                if (g[A.X, A.Y-1].isConnected == true)
                {
                    move = new Thickness(P.Margin.Left, P.Margin.Top - A.ystep, P.Margin.Right, P.Margin.Bottom + A.ystep);
                    A.MoveToTop();
                    P.Margin = move;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Right)) // 右方向键
            {
                if (g[A.X + 1, A.Y].isConnected == true)
                {
                    move = new Thickness(P.Margin.Left + A.xstep, P.Margin.Top, P.Margin.Right - A.xstep, P.Margin.Bottom);
                    A.MoveToRight();
                    P.Margin = move;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Down)) // 下方向键
            {
                if (g[A.X, A.Y+1].isConnected == true)
                {
                    move = new Thickness(P.Margin.Left, P.Margin.Top + A.ystep, P.Margin.Right, P.Margin.Bottom - A.ystep);
                    A.MoveToBottom();
                    P.Margin = move;
                }
            }
            if (A.IsOut() == true) MessageBox.Show(A.X.ToString() + "," + A.Y.ToString());
        }

        // 退出程序
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // 规则
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About d1 = new About();
            d1.ShowDialog();
        }

        // 排行榜
        private void Rank_Click(object sender, RoutedEventArgs e)
        {
            RankList d1 = new RankList();
            d1.Owner = this;
            d1.ShowDialog();
        }
    }
}
