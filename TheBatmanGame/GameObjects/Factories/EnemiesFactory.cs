using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects.Factories
{
    public class EnemiesFactory : IGameObjectFactory
    {
        const int EnemyBoundsWidth = 45;
        const int EnemyBoundsHeight = 55;
        public GameObject Get(int left, int top)
        {
            return new EnemyGameObject()
            {
                Position = new Position(left, top),
                Bounds = new Size(EnemyBoundsWidth, EnemyBoundsHeight)
            };
        }
    }
}
