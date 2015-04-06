using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects.Factories
{
    public class EnemiesFactory : IGameObjectFactory
    {
        const int EnemyBoundsWidth = 65;
        const int EnemyBoundsHeight = 75;

        const int BigEnemyBoundsWidth = 105;
        const int BigEnemyBoundsHeight = 75;
        const int BigEnemySpawnChance = 15;

        const int BossEnemyBoundsWidth = 150;
        const int BossEnemyBoundsHeight = 150;
        const int BossEnemySpawnChance = 5;

        static readonly Random rand = new Random();

        public GameObject Get(int left, int top)
        {
            if (rand.Next(100) <= BigEnemySpawnChance)
            {
                if (rand.Next(100) < BossEnemySpawnChance)
                {
                    return new BigEnemyGameObject(5)
                    {
                        Position = new Position(left, top),
                        Bounds = new Size(BossEnemyBoundsWidth, BossEnemyBoundsHeight)
                    };
                }
                return new BigEnemyGameObject()
                {
                    Position = new Position(left, top),
                    Bounds = new Size(BigEnemyBoundsWidth, BigEnemyBoundsHeight)
                };
            }
            return new EnemyGameObject()
             {
                 Position = new Position(left, top),
                 Bounds = new Size(EnemyBoundsWidth, EnemyBoundsHeight)
             };
        }
    }
}
