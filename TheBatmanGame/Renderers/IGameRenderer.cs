using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBatmanGame.GameObjects;
using TheBatmanGame.Misc;

namespace TheBatmanGame.Renderers
{
    public interface IGameRenderer
    {

        int ScreenWidth { get; }
        int ScreenHeight { get; }

        void Clear();
        void Draw(params GameObject[] gameObjects);

        event EventHandler<KeyDownEventArgs> UIActionHappened;

        bool IsInBounds(Position position);

        void ShowEndGameScreen(int highscore);
    }
}
