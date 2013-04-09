using WindowsMobileClientAliveChess.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using WindowsMobileClientAliveChess.NetLayer.Main;
using WindowsMobileClientAliveChess.NetLayer;
using System.Windows.Forms;

namespace WindowsMobileClientAliveChess.NetLayer.Executors
{
    public class AuthorizeExecutor : IExecutor
    {
        private Game game;
        private object sync = new object();

        public AuthorizeExecutor(Game game)
        {
            this.game = game;
        }


        public void Execute(ICommand cmd)
        {
            AuthorizeResponse authorize = (AuthorizeResponse)cmd;
            if (!authorize.IsAuthorized)
            {
                MessageBox.Show(LanguageSwitcher.GetExceptionMessage("UnauthorizedException"));
                game.Authorized = false;
                game.Stop();
            }
            else
            {
                game.Authorized = true;
                game.HandleSuccesAuthorization();
            }
        }
    }
}
