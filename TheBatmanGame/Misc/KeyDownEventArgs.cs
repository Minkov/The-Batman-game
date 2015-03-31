using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.Misc
{
    public enum GameCommand
    {
        Fire,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    };

    public class KeyDownEventArgs : EventArgs
    {
        public GameCommand Command { get; set; }

        public KeyDownEventArgs(GameCommand command)
        {
            this.Command = command;
        }
    }
}