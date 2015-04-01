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

namespace TheBatmanGame.Windows
{
    /// <summary>
    /// Interaction logic for InitialGameWindow.xaml
    /// </summary>
    public partial class InitialGameWindow : Window
    {
        public InitialGameWindow()
        {
            InitializeComponent();
        }

        public void OnNewGameButtonClick(object sender, RoutedEventArgs e)
        {
            new GameWindow().Show();
            this.Close();
        }

        public void OnShowHighScoresButtonClick(object sender, RoutedEventArgs e)
        {
            new HighScoreWindow().Show();
            this.Close();
        }

        public void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}