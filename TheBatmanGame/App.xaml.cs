using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TheBatmanGame.Data;
using TheBatmanGame.Windows;

namespace TheBatmanGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnExit(ExitEventArgs e)
        {
            XmlHighscoreStorage.Instance.Save();
            base.OnExit(e);
        }
    }
}
