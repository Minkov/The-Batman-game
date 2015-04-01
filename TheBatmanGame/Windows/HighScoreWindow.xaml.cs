using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheBatmanGame.Data;

namespace TheBatmanGame.Windows
{
    /// <summary>
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        public HighScoreWindow()
        {
            InitializeComponent();
            var scores = XmlHighscoreStorage.Instance.Highscores;
            foreach (var score in scores)
            {
                this.PanelScores.Children.Add(new UniformGrid
                {
                    Rows = 1,
                    Children = {
                        new TextBlock { Text = score.Nickname },
                        new TextBlock { Text = score.Score.ToString() }
                    }
                });
            }
        }

        public void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            new InitialGameWindow().Show();
            this.Close();
        }

        public void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}