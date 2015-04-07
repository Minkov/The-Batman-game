using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using TheBatmanGame.Extensions;
using TheBatmanGame.GameObjects;
using TheBatmanGame.GameObjects.Factories;
using TheBatmanGame.Misc;
using TheBatmanGame.Renderers;

namespace TheBatmanGame.Engines
{
    public interface IGameEngine
    {
        int HighScore { get; }

        BatwingGameObject Batwing { get; }

        List<GameObject> Projectiles { get; }

        List<GameObject> Enemies { get; }

        ICollisionDetector CollisionDetector { get;}

        void InitGame();

        void StartGame();
    }
}