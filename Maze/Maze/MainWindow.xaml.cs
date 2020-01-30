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
        public MainWindow()
        {
            InitializeComponent();
            init(30, 26);
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

        private class Maze_Grid:Point
        {
            public bool isConnected { set; get; }
            public bool Visited { get; set; }
            public Maze_Grid(int xi, int yi) : base(xi, yi) { }
        }

        private class Player:Point
        {
            public Player(int xi, int yi) : base(xi, xi) { }
            public void MoveToLeft() { x = x - 1; }
            public void MoveToTop() { y = y - 1; }
            public void MoveToRight() { x = x + 1; }
            public void MoveToBottom() { y = y + 1; }
        }

        private void init(int Columns, int Rows)
        {
            // 对网格初始化
            initGrid(Columns, Rows);
            // 用深度优先遍历法生成迷宫形状
            DFS(1, 1, Columns, Rows);
            // 画出迷宫
            drawMaze(Columns, Rows);
            // 初始化玩家
            A = new Player(1,1);
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
                    else
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
                int[] direction = { 1, 2, 3, 4 }; // 分别代表左、上、右、下四个方向
                // 打乱数组
                int rand = rd.Next(1, 4);
                int temp = direction[rand];
                direction[rand] = direction[0];
                direction[0] = temp;
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
        }

        // 画出迷宫
        private void drawMaze(int Columns, int Rows)
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    TextBlock newArea = new TextBlock();
                     newArea.Text = i.ToString() + "," + j.ToString(); // 标出迷宫坐标
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
            init(15, 13);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                A.MoveToLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Up))
            {
                A.MoveToTop();
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                A.MoveToRight();
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                A.MoveToBottom();
            }
            MessageBox.Show(A.X.ToString() + A.Y.ToString());
        }
    }
}
