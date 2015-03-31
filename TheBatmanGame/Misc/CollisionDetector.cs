using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBatmanGame.GameObjects;

namespace TheBatmanGame.Misc
{
    public class CollisionDetector
    {
        public bool AreCollided(GameObject go1, GameObject go2)
        {
            int projectileLeft = go1.Position.Left;
            int projectileRight = go1.Position.Left + go1.Bounds.Width;
            int projectileTop = go1.Position.Top;
            int projectileBottom = go1.Position.Top + go1.Bounds.Height;

            int enemyLeft = go2.Position.Left;
            int enemyRight = go2.Position.Left + go2.Bounds.Width;
            int enemyTop = go2.Position.Top;
            int enemyBottom = go2.Position.Top + go2.Bounds.Height;

            bool shouldDie = ((enemyLeft <= projectileLeft && projectileLeft <= enemyRight) ||
                              ((enemyLeft <= projectileRight && projectileRight <= enemyRight))) &&
                             ((enemyTop <= projectileTop && projectileTop <= enemyBottom) ||
                              (enemyTop <= projectileBottom && projectileBottom <= enemyBottom));
            return shouldDie;
        }
    }
}
