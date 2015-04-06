using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects
{
    public class BigEnemyGameObject : EnemyGameObject
    {
        private const int DefaultBigEnemyHealth = 3;

        protected int Health { get; set; }

        public BigEnemyGameObject(int health)
        {
            this.Health = health;
        }

        public BigEnemyGameObject()
            :this(DefaultBigEnemyHealth)
        {
        }

        public override bool IsAlive
        {
            get
            {
                return this.Health > 0;
            }
            set
            {
                if (!value)
                {
                    --this.Health;
                }
            }
        }
    }
}