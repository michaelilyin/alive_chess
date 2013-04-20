using System.Collections.Generic;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessServer.DBLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class BuildingInCastleRequestExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
       
        public BuildingInCastleRequestExecutor(LogicEntryPoint gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message cmd)
        {
            BuildingInCastleRequest request = (BuildingInCastleRequest)cmd.Command;

            King king = cmd.Sender.King;
            king.CurrentCastle.AddBuildings(delegate() { return GuidGenerator.Instance.GeneratePair(); },
                request.Type);
            int l = king.CurrentCastle.SizeListbuilldingsInCastle();
            List<InnerBuilding> s = new List<InnerBuilding>();
            for (int i = 0; i < l; i++)
            {
                s.Add(king.CurrentCastle.GetBuildings(i));
            }

            //_queryManager.SendGetListBuildings(plInfo, s);

        }
    }
}
