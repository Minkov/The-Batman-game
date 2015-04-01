using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.Data
{
    public class PlayerHighscore
    {
        private string nickname;
        public int Score { get; set; }

        public string Nickname
        {
            get
            {
                return this.nickname;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3 || value.Length > 15)
                {
                    throw new ArgumentOutOfRangeException("The nickname must be a string between 3 and 15 characters");
                }
                this.nickname = value;
            }
        }


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
