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
using TheBatmanGame.Engines;
using TheBatmanGame.Renderers;

namespace TheBatmanGame.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            this.InitializeComponent();
            var wpfRenderer = new WpfGameRenderer(this.GameCanvas);
            this.Engine = new GameEngine(wpfRenderer);
            this.Engine.InitGame();
            this.Engine.StartGame();
        }

        public IGameEngine Engine { get; set; }
    }
}
