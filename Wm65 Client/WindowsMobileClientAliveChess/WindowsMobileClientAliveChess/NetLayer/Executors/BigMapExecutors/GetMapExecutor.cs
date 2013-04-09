using WindowsMobileClientAliveChess.GameLayer;
using System.Threading;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Objects;

namespace WindowsMobileClientAliveChess.NetLayer.Executors.BigMapExecutors
{
    public class GetMapExecutor : IExecutor
    {
        private Game game;

        public GetMapExecutor(Game game)
        {
            this.game = game;
        }

        public void Execute(ICommand cmd)
        {
            GetMapResponse map = (GetMapResponse)cmd;
            Monitor.Enter(this.game);
            try
            {
                if (map != null)
                {
                    game.Map = new Map(map.SizeMapX, map.SizeMapY);
                    game.Map.Initialize();
                    game.Map.Id = map.MapId;
                    game.Map.BasePoints = map.BasePoints;
                    game.Map.Castles = map.Castles;
                    foreach (Castle c in game.Map.Castles)
                    {
                        for (int i = c.X; i <= c.X + c.Width; i++)
                        {
                            for (int j = c.Y; j <= c.Y + c.Height; j++)
                            {
                                game.Map.SetObject(Map.CreatePoint(i, j, PointTypes.Castle));
                            }
                        }
                    }
                    game.Map.Mines = map.Mines;
                    foreach (Mine m in game.Map.Mines)
                    {
                        for (int i = m.X; i <= m.X + m.Width; i++)
                        {
                            for (int j = m.Y; j <= m.Y + m.Height; j++)
                            {
                                game.Map.SetObject(Map.CreatePoint(i, j, PointTypes.Mine));
                            }
                        }
                    }
                    game.Player.Map = game.Map;
                    game.Player.King.Map = game.Map;
                    game.HandleGetMap();
                    
                }
                else
                {
                    game.Map = null;
                }
            }
            finally
            {
                Monitor.Exit(this.game);
            }
        }
    }
}
