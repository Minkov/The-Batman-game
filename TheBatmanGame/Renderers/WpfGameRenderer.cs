using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheBatmanGame.Engines;
using TheBatmanGame.GameObjects;
using TheBatmanGame.Misc;

namespace TheBatmanGame.Renderers
{
    public class WpfGameRenderer :
        IGameRenderer
    {
        private static string[] enemyImageSources;
        private Canvas canvas;

        public event EventHandler<KeyDownEventArgs> UIActionHappened;

        static WpfGameRenderer()
        {
            var dir = new DirectoryInfo("./Images/Enemies");
            enemyImageSources = dir.GetFiles()
                                   .Select(file => "/Images/Enemies/" + file.Name)
                                   .ToArray();
            var b = 5;
        }

        public WpfGameRenderer(Canvas canvas)
        {
            this.canvas = canvas;
            this.ParentWindow.KeyDown += HandleKeyDown;
        }
 
        private void HandleKeyDown(object sender, KeyEventArgs args)
        {
            var key = args.Key;
            GameCommand command;
            switch (key)
            {
                case Key.Up:
                    command = GameCommand.MoveUp;
                    break;
                case Key.Down:
                    command = GameCommand.MoveDown;
                    break;
                case Key.Left:
                    command = GameCommand.MoveLeft;
                    break;
                case Key.Right:
                    command = GameCommand.MoveRight;
                    break;
                default:
                    command = GameCommand.Fire;
                    break;
            }

            this.UIActionHappened(this, new KeyDownEventArgs(command));
        }

        public bool IsInBounds(Position position)
        {
            return 0 <= position.Left && position.Left <= this.ScreenWidth &&
                   0 <= position.Top && position.Top <= this.ScreenHeight;
        }

        public void ShowEndGameScreen(int highscore)
        {
            var parent = this.canvas.Parent;
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            var stackPanel = new StackPanel();
            var window = new Window
            {
                Content = stackPanel
            };

            var btn = new Button
            {
                Content = "Play again"
            };

            var tb = new TextBlock()
            {
                Text = string.Format("You highscore is {0}", highscore)
            };

            stackPanel.Children.Add(tb);
            stackPanel.Children.Add(btn);

            btn.Click += (snd, ev) =>
            {
                new MainWindow().Show();
                window.Close();
            };
            window.Show();
            (parent as Window).Close();
        }

        public int ScreenWidth
        {
            get
            {
                return (int)this.ParentWindow.ActualWidth;
            }
        }

        public int ScreenHeight
        {
            get
            {
                return (int)this.ParentWindow.Height;
            }
        }

        public Window ParentWindow
        {
            get
            {
                var parent = canvas.Parent;
                while (!(parent is Window))
                {
                    parent = LogicalTreeHelper.GetParent(parent);
                }
                return parent as Window;
            }
        }

        public void Clear()
        {
            this.canvas.Children.Clear();
        }

        public void Draw(params GameObject[] gameObjects)
        {
            foreach (var go in gameObjects)
            {
                if (go is BatwingGameObject)
                {
                    this.DrawBatwing(go);
                }
                else if (go is EnemyGameObject)
                {
                    this.DrawEnemy(go);
                }
                else if (go is YamatoGameObject)
                {
                    this.DrawYamato(go);
                }
                else if (go is ProjectileGameObject)
                {
                    this.DrawProjectile(go);
                }
            }
        }

        private void DrawYamato(GameObject yamato)
        {
            var ell = new Ellipse
            {
                Width = yamato.Bounds.Width,
                Height = yamato.Bounds.Height,
                Fill = Brushes.Black,
                StrokeThickness = 2
            };

            Canvas.SetLeft(ell, yamato.Position.Left);
            Canvas.SetTop(ell, yamato.Position.Top);
            this.canvas.Children.Add(ell);
        }

        private void DrawProjectile(GameObject projectile)
        {
            var rect = new Rectangle
            {
                Width = projectile.Bounds.Width,
                Height = projectile.Bounds.Height,
                Fill = Brushes.White,
                StrokeThickness = 2
            };

            Canvas.SetLeft(rect, projectile.Position.Left);
            Canvas.SetTop(rect, projectile.Position.Top);
            this.canvas.Children.Add(rect);
        }

        private void DrawBatwing(GameObject batwing)
        {
            Image image = new Image();
            BitmapImage batwingImageSource = new BitmapImage();
            batwingImageSource.BeginInit();
            batwingImageSource.UriSource = new Uri("/Images/batwing.png", UriKind.Relative);
            batwingImageSource.EndInit();

            image.Source = batwingImageSource;
            image.Width = batwing.Bounds.Width;
            image.Height = batwing.Bounds.Height;

            Canvas.SetLeft(image, batwing.Position.Left);
            Canvas.SetTop(image, batwing.Position.Top);
            this.canvas.Children.Add(image);
        }

        static Random rand = new Random();

        private void DrawEnemy(GameObject enemy)
        
        {
            var enemyPath = enemyImageSources[rand.Next(enemyImageSources.Length)];
            Image image = new Image();
            BitmapImage batwingImageSource = new BitmapImage();
            batwingImageSource.BeginInit();
            batwingImageSource.UriSource = new Uri(enemyPath, UriKind.Relative);
            batwingImageSource.EndInit();

            image.Source = batwingImageSource;
            image.Width = enemy.Bounds.Width;
            image.Height = enemy.Bounds.Height;

            Canvas.SetLeft(image, enemy.Position.Left);
            Canvas.SetTop(image, enemy.Position.Top);
            this.canvas.Children.Add(image);
        
        }
    }
}