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
    class LeaveUnitsExequtor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        public LeaveUnitsExequtor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            Player player = msg.Sender;
            King king = msg.Sender.King;
            LeaveUnitsRequest request = (LeaveUnitsRequest)msg.Command;
            LeaveUnitsResponse response = new LeaveUnitsResponse();
            if (!king.CurrentCastle.KingInside)
            {
                player.Messenger.SendNetworkMessage(new ErrorMessage("Король не в замке."));
                return;
            }
            foreach (var item in request.Units)
            {
                if (!king.Army.HasUnits(item.Key, item.Value))
                {
                    player.Messenger.SendNetworkMessage(new ErrorMessage("Запрошено больше юнитов, чем имеется в армии короля."));
                    return;
                }
            }
            foreach (var item in request.Units)
            {
                king.Army.RemoveUnit(item.Key, item.Value);
                king.CurrentCastle.Army.AddUnit(item.Key, item.Value);
            }
            response.KingArmy = king.Army.GetUnitListCopy();
            response.CastleArmy = king.CurrentCastle.Army.GetUnitListCopy();
            player.Messenger.SendNetworkMessage(response);
        }
    }
}
