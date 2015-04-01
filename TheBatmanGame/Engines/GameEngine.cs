using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TheBatmanGame.GameObjects;
using TheBatmanGame.GameObjects.Factories;
using TheBatmanGame.Misc;
using TheBatmanGame.Renderers;

namespace TheBatmanGame.Engines
{
    public class GameEngine
    {
        const int BatwingSizeHeight = 100;
        const int BatwingSizeWidth = 100;
        const int BatmanSpeed = 25;
        const int TimerTickIntervalInMilliseconds = 100;
        const int GenerateEnemyChange = 90;
        const int ScoreForKill = 45;
        const int ScoreForTick = 10;

        private IGameRenderer renderer;
        private IGameObjectFactory projectilesFactory;
        private IGameObjectFactory enemiesFactory;

        static Random rand = new Random();
        private DispatcherTimer timer;

        private int HighScore { get; set; }

        private BatwingGameObject Batwing { get; set; }

        private List<GameObject> Projectiles { get; set; }

        private List<GameObject> Enemies { get; set; }

        public List<GameObject> GameObjects { get; set; }

        public CollisionDetector CollisionDetector { get; set; }

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

            this.CollisionDetector = new CollisionDetector();
        }

        private void HandleUIActionHappened(object sender, KeyDownEventArgs e)
        {
            if (e.Command == GameCommand.Fire)
            {
                this.FireProjectile();
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

            if (rand.Next(100) < GenerateEnemyChange)
            {
                var enemy = this.enemiesFactory.Get(this.renderer.ScreenWidth, rand.Next(this.renderer.ScreenHeight));
                this.Enemies.Add(enemy);
                this.GameObjects.Add(enemy);
            }

            KillEnemiesIfColliding();

            this.HighScore += this.Enemies.Count(enemy => !enemy.IsAlive) * ScoreForKill;
            RemoveNotAliveGameObjects();
            DrawGameObjects();
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

        private void DrawGameObjects()
        {
            foreach (var go in this.GameObjects)
            {
                int top = 0;
                int left = 0;
                if (go is ProjectileGameObject)
                {
                    top = go.Position.Top;
                    left = go.Position.Left + 105;
                }
                else if (go is EnemyGameObject)
                {
                    top = go.Position.Top + rand.Next(-10, 10);
                    left = go.Position.Left - 50;
                }
                go.Position = new Position(left, top);
                this.renderer.Draw(go);
            }
        }

        private void RemoveNotAliveGameObjects()
        {
            foreach (var go in this.GameObjects)
            {
                if (!this.renderer.IsInBounds(go.Position))
                {
                    go.IsAlive = false;
                }
            }

            this.GameObjects.RemoveAll(go => !go.IsAlive);
            this.Enemies.RemoveAll(enemy => !enemy.IsAlive);
            this.Projectiles.RemoveAll(projectile => !projectile.IsAlive);
        }
    }
}