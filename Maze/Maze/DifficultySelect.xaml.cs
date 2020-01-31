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
    /// DifficultySelect.xaml 的交互逻辑
    /// </summary>
    public partial class DifficultySelect : Window
    {
        MainWindow mainWindow;
        public DifficultySelect(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == difficult) mainWindow.init(2);
            else if (sender == middle) mainWindow.init(1);
            else if (sender == eazy) mainWindow.init(0);
            mainWindow.Visibility = Visibility.Visible;
            Close();
        }
    }
}
