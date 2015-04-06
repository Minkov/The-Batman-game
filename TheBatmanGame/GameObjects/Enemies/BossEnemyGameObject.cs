using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects.Enemies
{
    class BossEnemyGameObject:BigEnemyGameObject
    {
        private const int DefaultHealth = 5;

        public BossEnemyGameObject()
            : base(DefaultHealth)
        {

        }
    }
}
