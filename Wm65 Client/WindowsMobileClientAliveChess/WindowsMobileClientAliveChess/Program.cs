using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WindowsMobileClientAliveChess.GameLayer.Forms;
using WindowsMobileClientAliveChess.GameLayer;

namespace WindowsMobileClientAliveChess
{
    class Program
    {
        public static void Main()
        {
            LanguageSwitcher.Initialize();
            Game game = new Game();
            Application.Run(game.StartForm);
        }
    }
}
