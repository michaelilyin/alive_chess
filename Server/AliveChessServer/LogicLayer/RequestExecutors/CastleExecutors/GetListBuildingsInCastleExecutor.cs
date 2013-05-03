﻿using System.Collections.Generic;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors
{
    public class GetListBuildingsInCastleExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private Player _queryManager;

        public GetListBuildingsInCastleExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;

        }
        public void Execute(Message cmd)
        {
            this._queryManager = cmd.Sender;
            GetListBuildingsInCastleRequest request = (GetListBuildingsInCastleRequest)cmd.Command;
            Player info = _playerManager.GetPlayerInfoById(cmd.Sender.Id);
            int l = info.King.CurrentCastle.InnerBuildings.Count;
            List<InnerBuilding> s = new List<InnerBuilding>();
            for (int i = 0; i < l; i++) s.Add(info.King.CurrentCastle.GetBuilding(i));
            var response = new GetListBuildingsInCastleResponse();
            response.List = s;// as IList<InnerBuilding>;
            _queryManager.Messenger.SendNetworkMessage(response);
        }
    }
}
