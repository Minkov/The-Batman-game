using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheBatmanGame.Data
{
    public class XmlHighscoreStorage : IHighscoreStorage
    {
        private string filePath;
        private XDocument document;

        private List<PlayerHighscore> highscores;

        private static XmlHighscoreStorage instance;
        private const string HighScoresFilePath = "highscores.xml";

        public const int MaxHighscoresCount = 10;

        private XmlHighscoreStorage(string filePath)
        {
            this.filePath = filePath;
            this.document = LoadOrCreateDocument(this.filePath);
            this.LoadHighscores();
        }

        public static XmlHighscoreStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XmlHighscoreStorage(XmlHighscoreStorage.HighScoresFilePath);
                }

                return instance;
            }
        }

        private XDocument LoadOrCreateDocument(string filepath)
        {
            if (File.Exists(filePath))
            {
                return XDocument.Load(filepath);
            }
            var doc = new XDocument();
            doc.Add(new XElement("highscores"));
            doc.Save(filepath);
            return doc;
        }

        private void LoadHighscores()
        {
            this.Highscores = this.document.Root
                                  .Elements()
                                  .Select(element => new PlayerHighscore()
                                         {
                                             Nickname = element.Element("nickname").Value,
                                             Score = int.Parse(element.Element("score").Value)
                                         });
        }

        public void Add(PlayerHighscore highscore)
        {
            this.highscores.Add(highscore);
        }

        public void Save()
        {
            var root = this.document.Root;
            root.RemoveAll();
            var sortedHighscores = this.Highscores.ToList();
            for (int i = 0; i < sortedHighscores.Count && i < MaxHighscoresCount; i++)
            {
                var highscore = sortedHighscores[i];
                root.Add(new XElement("highscore",
                    new XElement("nickname", highscore.Nickname),
                    new XElement("score", highscore.Score)));
            }
            this.document.Save(this.filePath);
        }

        public IEnumerable<PlayerHighscore> Highscores
        {
            get
            {
                return this.highscores.OrderBy(highscore => highscore.Score)
                           .ThenBy(highscore => highscore.Nickname)
                           .Take(MaxHighscoresCount);
            }
            private set
            {
                this.highscores = new List<PlayerHighscore>(value);
            }
        }
    }
}