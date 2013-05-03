using System.Collections.Generic;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class BuildingInCastleRequestExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;
       
        public BuildingInCastleRequestExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            BuildingInCastleRequest request = (BuildingInCastleRequest)cmd.Command;
            this._queryManager = cmd.Sender;
            King king = cmd.Sender.King;
            king.CurrentCastle.AddBuilding(request.Type);
            int l = king.CurrentCastle.NumberOfBuildings();
            List<InnerBuilding> s = new List<InnerBuilding>();
            for (int i = 0; i < l; i++)
            {
                s.Add(king.CurrentCastle.GetBuilding(i));
            }
            var response = new BuildingInCastleResponse();
            response.Buildings_list = s;
            _queryManager.Messenger.SendNetworkMessage(response);

        }
    }
}
