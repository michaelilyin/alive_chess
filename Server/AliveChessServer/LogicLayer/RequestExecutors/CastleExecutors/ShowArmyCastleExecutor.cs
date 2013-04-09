using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using System.Collections.Generic;
using System.Data.Linq;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class ShowArmyCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;
       
        public ShowArmyCastleExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }
        public void Execute(Message msg)
        {
            this._queryManager = msg.Sender;
            ShowArmyCastleRequest request = (ShowArmyCastleRequest)msg.Command;
            //PlayerInfo info = _playerManager.GetPlayerInfoById(msg.Sender.Id);
            EntitySet<Unit> arm = msg.Sender.King.CurrentCastle.FigureStore.Units;
            List<Unit> resp_list = new List<Unit>();
            foreach(var u in arm)
            {
                resp_list.Add(u);
            }
            ShowArmyCastleResponse response = new ShowArmyCastleResponse();
            response.Army_list = resp_list;
            _queryManager.Messenger.SendNetworkMessage(response);

        }
    }
}
