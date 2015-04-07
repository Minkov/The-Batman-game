using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBatmanGame.GameObjects.Projectiles;

namespace TheBatmanGame.GameObjects.Factories
{
    public class ProjectileFactory : IGameObjectFactory
    {
        const int ProjectileBoundsWidth = 55;
        const int ProjectileBoundsHeight = 15;

        const int YamatoBoundsWidth = 155;
        const int YamatoBoundsHeight = 100;

        const int YamatoChance = 15;

        static Random rand = new Random();

        public GameObject Get(int left, int top)
        {
            int choice = rand.Next(100);
            if (choice < YamatoChance)
            {
                return new YamatoGameObject()
                {
                    Position = new Position(left, top - 30),
                    Bounds = new Size(YamatoBoundsWidth, YamatoBoundsHeight)
                };
            }
            return new ProjectileGameObject()
            {
                Position = new Position(left, top),
                Bounds = new Size(ProjectileBoundsWidth, ProjectileBoundsHeight)
            };
        }
    }
}