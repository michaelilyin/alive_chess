using System.Collections.Generic;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessServer.NetLayer;
using BehaviorAILibrary.MotionLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    /// <summary>
    /// обработчик перемещения игрока
    /// </summary>
    public class MoveKingExecutor : IExecutor
    {
        public void Execute(Message msg)
        {
            MoveKingRequest request = (MoveKingRequest)msg.Command;
            if (!msg.Sender.Waiting)
            {
                List<Position> path = null;

                Map map = msg.Sender.Map;
                King king = msg.Sender.King;

                // если игрок на карте
                if (king.State == KingState.BigMap)
                {
                    Position start = new Position(king.X, king.Y);
                    Position goal = new Position(request.X, request.Y);
                    AStar astar = new AStar(map);
                    astar.Dialonals = true;
                    astar.HeavyDiagonals = true;
                    astar.SetStartEnd(start, goal);
                    path = astar.FindPath();

                    if (path != null)
                    {
                        // убираем из очереди текущую позицию короля
                        path.RemoveAt(0);
                        // сообщаем королю о найденном пути
                        king.AddSteps(Motion.Transform(path));

                        // отправляем найденный путь клиенту
                        king.Player.Messenger.SendNetworkMessage(new MoveKingResponse(path));
                    }
                    else
                    {
                        king.Player.Messenger.SendNetworkMessage(
                            new ErrorMessage("You're inside castle"));
                    }
                }
            }
        }
    }
}
