using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBatmanGame.GameObjects.Projectiles;

namespace TheBatmanGame.GameObjects.Projectiles
{
    public class YamatoGameObject : ProjectileGameObject
    {
        public override bool IsAlive
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
    }
}
