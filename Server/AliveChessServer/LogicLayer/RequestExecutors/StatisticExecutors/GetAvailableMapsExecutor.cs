using System.Collections.Generic;
using System.IO;
using AliveChessLibrary.Commands.StatisticCommand;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.StatisticExecutors
{
    public class GetAvailableMapsExecutor : IExecutor
    {
        private GameWorld _environment;
        private PlayerManager _playerManager;

        private const string Location = @"..\Maps\";

        public GetAvailableMapsExecutor(GameLogic gameLogic)
        {
            this._environment = gameLogic.Environment;
            this._playerManager = gameLogic.PlayerManager;
        }

        public void Execute(Message msg)
        {
            GetAvailableMapsRequest request = (GetAvailableMapsRequest) msg.Command;
            if (msg.Sender != null && msg.Sender.IsAuthorized)
            {
                GetAvailableMapsResponse response = new GetAvailableMapsResponse();
                response.Worlds = new List<WorldDescription>();
                if (request.WorldType == WorldType.Global)
                {
                    string[] maps = Directory.GetFiles(Location, "*.unity3d", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < maps.Length; i++)
                    {
                        WorldDescription worldDescription = new WorldDescription();
                        worldDescription.MapName = maps[i];
                        response.Worlds.Add(worldDescription);
                    }
                }
                else
                {
                    // Not implemented
                }
                msg.Sender.Messenger.SendNetworkMessage(response);
            }
        }
    }
}
