using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.GameObjects
{
    public struct Position
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public Position(int left, int top) : this()
        {
            this.Left = left;
            this.Top = top;
        }
    }
}
