using AliveChessLibrary.Commands.ErrorCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using System.Collections.Generic;
using System.Data.Linq;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    class CollectUnitsExequtor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public CollectUnitsExequtor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            Player player = msg.Sender;
            King king = msg.Sender.King;
            CollectUnitsRequest request = (CollectUnitsRequest)msg.Command;
            CollectUnitsResponse response = new CollectUnitsResponse();
            if (!king.CurrentCastle.KingInside)
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Король не в замке."));
                return;
            }
            foreach (var item in request.Units)
            {
                if (!king.CurrentCastle.Army.HasUnits(item.Key, item.Value))
                {
                    player.Messenger.SendNetworkMessage(new ErrorMessage("Запрошено больше юнитов, чем имеется в замке."));
                    return;
                }
            }
            foreach (var item in request.Units)
            {
                king.CurrentCastle.Army.RemoveUnit(item.Key, item.Value);
                king.Army.AddUnit(item.Key, item.Value);
            }
            response.KingArmy = king.Army.GetUnitListCopy();
            response.CastleArmy = king.CurrentCastle.Army.GetUnitListCopy();
            /*response.Units = king.CurrentCastle.Army.GetUnitListCopy();
            response.Units2 = king.Army.GetUnitListCopy();*/
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
