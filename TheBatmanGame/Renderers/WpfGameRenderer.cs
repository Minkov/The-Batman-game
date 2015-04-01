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
using TheBatmanGame.Windows;

namespace TheBatmanGame.Renderers
{
    public class WpfGameRenderer : IGameRenderer
    {
        private const string BatwingImagePath = "/Images/batwing.png";

        public event EventHandler<KeyDownEventArgs> UIActionHappened;
        
        private static string[] enemyImageSources;
        static Random rand = new Random();

        private Canvas canvas;

        static WpfGameRenderer()
        {
            var dir = new DirectoryInfo("./Images/Enemies");
            enemyImageSources = dir.GetFiles()
                                   .Select(file=>file.FullName)
                                   .ToArray();
        }

        public WpfGameRenderer(Canvas canvas)
        {
            this.canvas = canvas;
            this.ParentWindow.KeyDown += HandleKeyDown;
        }

        public bool IsInBounds(Position position)
        {
            return 0 <= position.Left && position.Left <= this.ScreenWidth &&
                   0 <= position.Top && position.Top <= this.ScreenHeight;
        }

        public void ShowEndGameScreen(int highscore)
        {
            new GameOverWindow(highscore).Show();
            this.ParentWindow.Close();
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
            var rect = new Border
            {
                Width = projectile.Bounds.Width,
                Height = projectile.Bounds.Height,
                Background = Brushes.White,
                CornerRadius = new CornerRadius(2, 5, 5, 2)
            };

            Canvas.SetLeft(rect, projectile.Position.Left);
            Canvas.SetTop(rect, projectile.Position.Top);
            this.canvas.Children.Add(rect);
        }

        private void DrawBatwing(GameObject batwing)
        {
            var image = this.CreateImageForCanvas(BatwingImagePath, batwing.Position, batwing.Bounds);
            this.canvas.Children.Add(image);
        }

        private void DrawEnemy(GameObject enemy)
        {
            
            //var image = new Rectangle()
            //{
            //    Fill = Brushes.Black,
            //    Stroke = Brushes.Blue,
            //    StrokeThickness = 3,
            //    Width = enemy.Bounds.Width,
            //    Height = enemy.Bounds.Height
            //};
            //Canvas.SetTop(image, enemy.Position.Top);
            //Canvas.SetLeft(image, enemy.Position.Left);

            var enemyPath = enemyImageSources[rand.Next(enemyImageSources.Length)];
          
            var image = CreateImageForCanvas(enemyPath, enemy.Position, enemy.Bounds);

            this.canvas.Children.Add(image);
        }

        private Image CreateImageForCanvas(string path, Position position, TheBatmanGame.GameObjects.Size bounds)
        {
            Image image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            image.Source = bitmap;
            image.Width = bounds.Width;
            image.Height = bounds.Height;

            Canvas.SetLeft(image, position.Left);
            Canvas.SetTop(image, position.Top);
            return image;
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
    }
}