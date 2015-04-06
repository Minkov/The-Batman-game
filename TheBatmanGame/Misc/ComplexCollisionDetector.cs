using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBatmanGame.GameObjects;

namespace TheBatmanGame.Misc
{
    public class ComplexCollisionDetector : SimpleCollisionDetector
    {
        public override bool AreCollided(GameObject go1, GameObject go2)
        {
            if (base.AreCollided(go1, go2))
            {
                return true;
            }
            var go1Bounds = this.GetObjectBounds(go1);
            var go2Bounds = this.GetObjectBounds(go2);

            bool shouldDie = this.CheckforInsideCollision(go1Bounds, go2Bounds);
            return shouldDie;
        }

        private bool CheckforInsideCollision(GameObjectBounds go1Bounds, GameObjectBounds go2Bounds)
        {
            return false;
        }
    }
}
