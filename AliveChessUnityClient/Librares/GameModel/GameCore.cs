using AliveChessLibrary.Commands;
using GameModel.ResponseHandlers;
using GameModel.ResponseHandlers.CastleHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel
{
    public class GameCore
    {
        private static GameCore _instance;
        public static GameCore Instance { get { return _instance == null ? _instance = new GameCore() : _instance; } }

        private GameCore()
        {
            World = new GameModel.World();
            Network = new Network.Network();
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            Network.RegisterHandler(Command.AuthorizeResponse, new AuthorizeResponseHandler());
            Network.RegisterHandler(Command.GetMapResponse, new GetMapResponseHandler());
            Network.RegisterHandler(Command.ErrorMessage, new ErrorMessageHandler());
            Network.RegisterHandler(Command.GetGameStateResponse, new GetGameStateHandler());
            Network.RegisterHandler(Command.GetObjectsResponse, new GetObjectsHandler());
            Network.RegisterHandler(Command.MoveKingResponse, new MoveKingResponseHandler());
            Network.RegisterHandler(Command.ComeInCastleResponse, new ComeInCastleResponseHandler());

            Network.RegisterHandler(Command.LeaveCastleResponse, new LeaveCastleResponseHandler());
            Network.RegisterHandler(Command.GetCreationRequirementsResponse, new GetCreationRequirementsHandler());
            Network.RegisterHandler(Command.GetBuildingsResponse, new GetBuildingsHandler());
        }

        public event EventHandler Authorized;

        public Network.Network Network { get; private set; }
        public World World { get; private set; }
        private bool _isAuthorized;
        public bool IsAuthorized 
        { 
            get 
            {
                return _isAuthorized; 
            }
            internal set
            {
                _isAuthorized = value;
                if (value)
                {
                    if (Authorized != null)
                        Authorized(Network, new EventArgs());
                }
            }
        }

        public void StartGame()
        {
            Network.BigMapCommandController.SendGetMapRequest();
            Network.BigMapCommandController.SendBigMapRequest();
            Network.BigMapCommandController.StartGameStateUpdate();
            Network.BigMapCommandController.StartObjectsUpdate();
        }

        public void StopGame()
        {
            Network.BigMapCommandController.StopGameStateUpdate();
            Network.BigMapCommandController.StopObjectsUpdate();
        }
    }
}
