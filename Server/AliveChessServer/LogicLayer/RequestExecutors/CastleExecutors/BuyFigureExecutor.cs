using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.DBLayer;
using System.Collections.Generic;
using System.Data.Linq;
 

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class BuyFigureExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;
       
        public BuyFigureExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            this._queryManager = cmd.Sender;
            BuyFigureRequest request = (BuyFigureRequest)cmd.Command;
            //PlayerInfo plInfo = _playerManager.GetPlayerInfoById(cmd.Sender.Id);
            King king = cmd.Sender.King;
            king.CurrentCastle.CreateUnitAndAddInArmy(request.FigureCount, request.FigureType);
            EntitySet<Unit> arm = cmd.Sender.King.CurrentCastle.FigureStore.Units;
            List<Unit> response_list = new List<Unit>();
            foreach (var u in arm)
            {
                response_list.Add(u);
            }
            var response = new BuyFigureResponse();
            response.Units = response_list;
            _queryManager.Messenger.SendNetworkMessage(response);

        }
    }
}
