﻿using System;
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
using TheBatmanGame.GameObjects.Enemies;
using TheBatmanGame.GameObjects.Projectiles;
using TheBatmanGame.Misc;
using TheBatmanGame.Windows;

namespace TheBatmanGame.Renderers
{
    public class WpfGameRenderer : IGameRenderer
    {
        private const string BatwingImagePath = "/Images/batwing.png";
        private const string YamatoImagePath = "/Images/projectiles/yamato.png";
        private const string EnemyImagePath = "/Images/enemies/enemy.png";
        private const string BossEnemyImagePath = "/Images/enemies/boss-enemy.png";

        public event EventHandler<KeyDownEventArgs> UIActionHappened;

        private Canvas canvas;

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
                else if (go is BossEnemyGameObject)
                {
                    this.DrawBossEnemy(go);
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
            var image = this.CreateImageForCanvas(YamatoImagePath, yamato.Position, yamato.Bounds);
            this.canvas.Children.Add(image);
        }

        private void DrawProjectile(GameObject projectile)
        {
            var rect = new Border
            {
                Width = projectile.Bounds.Width,
                Height = projectile.Bounds.Height,
                Background = Brushes.Orange,
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

        private void DrawBossEnemy(GameObject enemy)
        {
            var image = CreateImageForCanvas(BossEnemyImagePath, enemy.Position, enemy.Bounds);
            this.canvas.Children.Add(image);
        }

        private void DrawBigEnemy(GameObject bigEnemy)
        {
            var enemy = new Rectangle
            {
                Fill = Brushes.Black,
                Width = bigEnemy.Bounds.Width,
                Height = bigEnemy.Bounds.Height
            };
            Canvas.SetLeft(enemy, bigEnemy.Position.Left);
            Canvas.SetTop(enemy, bigEnemy.Position.Top);
            this.canvas.Children.Add(enemy);
        }

        private void DrawEnemy(GameObject enemy)
        {
            var image = CreateImageForCanvas(EnemyImagePath, enemy.Position, enemy.Bounds);
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
                case Key.Enter:
                    command = GameCommand.PlayPause;
                    break;
                default:
                    command = GameCommand.Fire;
                    break;
            }

            this.UIActionHappened(this, new KeyDownEventArgs(command));
        }
    }
}