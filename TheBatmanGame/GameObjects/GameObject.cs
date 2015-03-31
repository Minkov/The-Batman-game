using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects
{
    public abstract class GameObject
    {
        public GameObject()
        {
            this.IsAlive = true;
        }

        public Position Position { get; set; }

        public Size Bounds { get; set; }

        public virtual bool IsAlive { get; set; }
    }
}