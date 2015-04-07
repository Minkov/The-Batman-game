using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TheBatmanGame.GameObjects;
using TheBatmanGame.GameObjects.Enemies;
using TheBatmanGame.GameObjects.Factories;
using TheBatmanGame.GameObjects.Projectiles;
using TheBatmanGame.Misc;
using TheBatmanGame.Renderers;
using TheBatmanGame.Extensions;

namespace TheBatmanGame.Engines
{
    public class GameEngine : IGameEngine
    {
        private const int BatwingSizeHeight = 100;
        private const int BatwingSizeWidth = 100;
        private const int BatmanSpeed = 25;
        private const int TimerTickIntervalInMilliseconds = 100;
        private const int SpawnEnemyChange = 90;
        private const int ScoreForKill = 45;
        private const int ScoreForTick = 10;
        private const int ProjectileMoveSpeed = 105;
        private const int EnemyMoveSpeed = -50;

        private IGameRenderer renderer;
        private IGameObjectFactory projectilesFactory;
        private IGameObjectFactory enemiesFactory;
        private DispatcherTimer timer;

        public int HighScore { get; private set; }

        static Random rand = new Random();

        public BatwingGameObject Batwing { get; private set; }

        public List<GameObject> Projectiles { get; private set; }

        public List<GameObject> Enemies { get; private set; }

        public List<GameObject> GameObjects { get; private set; }

        public ICollisionDetector CollisionDetector { get; private set; }

        public GameEngine(IGameRenderer renderer)
        {
            this.renderer = renderer;
            this.renderer.UIActionHappened +=
                this.HandleUIActionHappened;

            this.Projectiles = new List<GameObject>();
            this.projectilesFactory = new ProjectileFactory();

            this.Enemies = new List<GameObject>();
            this.enemiesFactory = new EnemiesFactory();

            this.GameObjects = new List<GameObject>();

            this.CollisionDetector = new ComplexCollisionDetector();
        }

        private void HandleUIActionHappened(object sender, KeyDownEventArgs e)
        {
            if (e.Command == GameCommand.Fire)
            {
                this.FireProjectile();
            }
            else if (e.Command == GameCommand.PlayPause)
            {
                this.PlayPauseGame();
            }
            else
            {
                int updateTop = 0;
                int updateLeft = 0;

                switch (e.Command)
                {
                    case GameCommand.MoveUp:
                        updateTop = -BatmanSpeed;
                        break;
                    case GameCommand.MoveDown:
                        updateTop = +BatmanSpeed;
                        break;
                    case GameCommand.MoveLeft:
                        updateLeft = -BatmanSpeed;
                        break;
                    case GameCommand.MoveRight:
                        updateLeft = +BatmanSpeed;
                        break;
                    default:
                        break;
                }

                int left = this.Batwing.Position.Left + updateLeft;
                int top = this.Batwing.Position.Top + updateTop;
                var position = new Position(left, top);
                if (this.renderer.IsInBounds(position))
                {
                    this.Batwing.Position = position;
                }
            }
        }

        private void PlayPauseGame()
        {
            if (this.timer.IsEnabled)
            {
                this.timer.Stop();
            }
            else
            {
                this.timer.Start();
            }
        }

        private void FireProjectile()
        {
            int top = this.Batwing.Position.Top;
            int left = this.Batwing.Position.Left;
            var projectileTop = this.projectilesFactory.Get(left, top);
            var projectileBottom = this.projectilesFactory.Get(left, top + this.Batwing.Bounds.Height);
            this.Projectiles.Add(projectileTop);
            this.Projectiles.Add(projectileBottom);

            this.GameObjects.Add(projectileTop);
            this.GameObjects.Add(projectileBottom);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            this.HighScore += ScoreForTick;
            if (this.Enemies.Any(enemy => this.CollisionDetector.AreCollided(this.Batwing, enemy)))
            {
                this.timer.Stop();
                this.renderer.ShowEndGameScreen(this.HighScore);
                return;
            }

            this.renderer.Clear();
            this.renderer.Draw(this.Batwing);

            if (rand.Next(100) < SpawnEnemyChange)
            {
                var enemy = this.enemiesFactory.Get(this.renderer.ScreenWidth, rand.Next(this.renderer.ScreenHeight));
                this.Enemies.Add(enemy);
                this.GameObjects.Add(enemy);
            }

            this.KillEnemiesIfColliding();

            this.HighScore += this.Enemies.Count(enemy => !enemy.IsAlive) * ScoreForKill;
            this.RemoveNotAliveGameObjects();
            this.UpdateObjectsPositions();
            this.DrawGameObjects();
        }

        private void KillEnemiesIfColliding()
        {
            foreach (var projectile in this.Projectiles)
            {
                foreach (var enemy in this.Enemies)
                {
                    if (this.CollisionDetector.AreCollided(projectile, enemy))
                    {
                        enemy.IsAlive = false;
                        projectile.IsAlive = false;
                        break;
                    }
                }
            }
        }

        private void UpdateObjectsPositions()
        {
            foreach (var go in this.GameObjects)
            {
                int top = 0;
                int left = 0;
                if (go is ProjectileGameObject)
                {
                    top = go.Position.Top;
                    left = go.Position.Left + ProjectileMoveSpeed;
                }
                else if (go is EnemyGameObject)
                {
                    top = go.Position.Top + rand.Next(-10, 10);
                    left = go.Position.Left + EnemyMoveSpeed;
                }
                go.Position = new Position(left, top);
            }
        }

        private void DrawGameObjects()
        {
            this.GameObjects.ForEach(go => this.renderer.Draw(go));
        }

        private void RemoveNotAliveGameObjects()
        {
            this.GameObjects.Where(go => !this.renderer.IsInBounds(go.Position))
                .ForEach(go => go.IsAlive = false);

            this.GameObjects.RemoveAll(go => !go.IsAlive);
            this.Enemies.RemoveAll(enemy => !enemy.IsAlive);
            this.Projectiles.RemoveAll(projectile => !projectile.IsAlive);
        }

        public void InitGame()
        {
            this.Batwing = new BatwingGameObject
            {
                Position = new Position(0, (this.renderer.ScreenHeight - BatwingSizeHeight) / 2),
                Bounds = new Size(BatwingSizeWidth, BatwingSizeHeight),
            };
            this.Projectiles.Clear();
        }

        public void StartGame()
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(TimerTickIntervalInMilliseconds);
            //game loop
            this.timer.Tick += this.GameLoop;
            this.timer.Start();
        }
    }
}