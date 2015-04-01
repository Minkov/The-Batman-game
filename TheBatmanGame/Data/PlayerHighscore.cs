using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.Data
{
    public class PlayerHighscore
    {
        public int Score { get; set; }

        public string Nickname { get; set; }


        public PlayerHighscore()
        {

        }

        public PlayerHighscore(string nickname, int score)
            : this()
        {
            this.Score = score;
            this.Nickname = nickname;
        }
    }
}
