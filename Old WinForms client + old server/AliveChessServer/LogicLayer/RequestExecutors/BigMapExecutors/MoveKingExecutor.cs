using System.Collections.Generic;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.LogicLayer.AI.MotionLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
   //static class ListExtension
   //{
   //    public static List<Position> TransformNodes(this IList<Node> source)
   //    {
   //        List<Position> result = new List<Position>();
   //        for (int i = 0; i < source.Count; i++)
   //            result.Add(new Position(source[i].X, source[i].Y));
   //        return result;
   //    }
   //}

    /// <summary>
    /// обработчик перемещения игрока
    /// </summary>
    public class MoveKingExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public MoveKingExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            MoveKingRequest request = (MoveKingRequest)msg.Command;
            List<Position> path = null;
            // получаем контекст игрока
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);

            Map map = msg.Sender.Map;
            King king = msg.Sender.King;

            // если игрок на карте
            if (king.State == KingState.BigMap)
            {
                // ищем минимальный путь по алгоритму A*
                //AStar astar = new AStar(map);
                //astar.SetStartEnd(new Point(info.Player.King.X, info.Player.King.Y),
                //                  new Point(request.X, request.Y));

                //path = astar.FindPath().TransformNodes();

                Position start = new Position(king.X, king.Y);
                Position goal = new Position(request.X, request.Y);
                AStar astar = new AStar(map, _environment.LevelManager.GameData);
                astar.SetStartEnd(start, goal);
                path = astar.FindPath();

                if (path != null)
                {
                    // убираем из очереди текущую позицию короля
                    path.RemoveAt(0);
                    // сообщаем королю о найденном пути
                    king.AddSteps(new Queue<Position>(path));

                    // отправляем найденный путь клиенту
                    king.Player.Messenger.SendNetworkMessage(new MoveKingResponse(path));
                }
            }
        }
    }
}
