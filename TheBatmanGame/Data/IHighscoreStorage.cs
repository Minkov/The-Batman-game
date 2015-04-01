using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.Data
{
    public interface IHighscoreStorage
    {
        void Add(PlayerHighscore highscore);
        void Save();

        IEnumerable<PlayerHighscore> Highscores { get; }
    }
}
