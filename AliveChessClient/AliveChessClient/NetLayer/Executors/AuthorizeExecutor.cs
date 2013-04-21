using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;

namespace AliveChessClient.NetLayer.Executors
{
    public class AuthorizeExecutor : IExecutor
    {
        private Game game;

        public AuthorizeExecutor(Game game)
        {
            this.game = game;
        }

        public void Execute(ICommand cmd)
        {
            AuthorizeResponse authorize = (AuthorizeResponse)cmd;

            King king = authorize.King;
            Castle castle = authorize.Castle;

            game.Player.King = authorize.King;
            game.Player.King.AttachStartCastle(authorize.Castle);
            game.Player.King.AddCastle(authorize.Castle);
            game.Player.IsSuperUser = authorize.IsSuperUser;
            game.Player.King.StartCastle.ResourceStore = new ResourceStore();
            foreach (Resource r in authorize.StartResources)
                game.Player.King.StartCastle.ResourceStore.AddResourceToRepository(r);

            king.UpdateSectorEvent += new King.UpdateSectorHandler(game.BigMap.UpdateKingVisibleSpace);
            king.ComeInCastleEvent += new King.ComeInCastleHandler(game.BigMap.ComeInCastle);
            king.ContactWithCastleEvent += new King.ContactWithCastleHandler(game.BigMap.MeetCastle);
            king.ContactWithKingEvent += new King.ContactWithKingHandler(game.BigMap.MeetKing);
           
            if (AuthorizeEvent != null)
                AuthorizeEvent();
        }

        public delegate void AuthorizeHandler();

        public event AuthorizeHandler AuthorizeEvent;
    }
}
