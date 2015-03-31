using System;
using System.Collections.Generic;
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
        private Canvas canvas;

        public event EventHandler<KeyDownEventArgs> UIActionHappened;

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

        public WpfGameRenderer(Canvas canvas)
        {
            this.canvas = canvas;


            this.ParentWindow.KeyDown += (sender, args) =>
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

             };
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
            var rect = new Rectangle
            {
                Width = yamato.Bounds.Width,
                Height = yamato.Bounds.Height,
                Fill = Brushes.Black,
                StrokeThickness = 2
            };

            Canvas.SetLeft(rect, yamato.Position.Left);
            Canvas.SetTop(rect, yamato.Position.Top);
            this.canvas.Children.Add(rect);
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

        private void DrawEnemy(GameObject enemy)
        {
            var ell = new Ellipse
            {
                Width = enemy.Bounds.Width,
                Height = enemy.Bounds.Height,
                Fill = Brushes.Brown,
                StrokeThickness = 2
            };

            Canvas.SetLeft(ell, enemy.Position.Left);
            Canvas.SetTop(ell, enemy.Position.Top);
            this.canvas.Children.Add(ell);
        }
    }
}