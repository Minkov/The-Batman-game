using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects
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
