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
using TheBatmanGame.Data;

namespace TheBatmanGame.Windows
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        public GameOverWindow()
        {
            InitializeComponent();
        }

        public GameOverWindow(int highscore) : this()
        {
            this.Highscore = highscore;
            this.TextBlockHighScore.Text = string.Format("Your highscore is {0}", this.Highscore);
        }


        public void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public int Highscore { get; set; }

        private void OnSaveHighscoreButtonClick(object sender, RoutedEventArgs e)
        {
            XmlHighscoreStorage.Instance.Add(new PlayerHighscore(this.TextBoxNickname.Text, this.Highscore));
            new InitialGameWindow().Show();
            this.Close();
        }

        public void OnDontSaveButtonClick(object sender, RoutedEventArgs e)
        {
            new InitialGameWindow().Show();
            this.Close();
        }
    }
}