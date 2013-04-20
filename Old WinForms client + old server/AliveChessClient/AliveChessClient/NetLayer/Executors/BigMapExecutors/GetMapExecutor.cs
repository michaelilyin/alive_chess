using AliveChessClient.GameLayer;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChessClient.NetLayer.Executors.BigMapExecutors
{
    public class GetMapExecutor : IExecutor
    {
        private Game game;
        //private DrawHandler handler;

        public GetMapExecutor(Game game)
        {
            this.game = game;
            //this.handler = new DrawHandler(game.BigMap.GraphicManager.Draw);
        }

        public void Execute(ICommand cmd)
        {
            GetMapResponse2D response = (GetMapResponse2D)cmd;

            game.Map = new Map(response.SizeMapX, response.SizeMapY);
          
            game.Player.King.Map = game.Map;
            game.Player.King.GameData = game.BigMap.Context;
            game.Player.King.StartCastle.Initialize(game.Map, game.BigMap.Context);

            game.Player.Map = game.Map;
            game.Player.King.Map = game.Map;

            game.InitMap(response.BasePoints, response.Points, response.Objects);

            game.Player.King.ViewOnMap.ObjectUnderThis = game.Player.Map[game.Player.King.X, game.Player.King.Y];
            game.Player.Map.SetObject(game.Player.King.ViewOnMap);
            game.Player.Map[game.Player.King.X, game.Player.King.Y].Detected = true;

            game.Player.Ready = true;
            // game.GameForm.Invoke(handler);
        }
    }
}
