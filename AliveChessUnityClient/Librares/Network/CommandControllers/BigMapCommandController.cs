using AliveChessLibrary.Commands.BigMapCommand;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Network.CommandControllers
{
    public class BigMapCommandController
    {
        private Timer _updateGameStateTimer;
        private Timer _updateObjectsTimer;
        private Network _network;

        public BigMapCommandController(Network network)
        {
            _network = network;
        }

        #region timers
        void updateObjectsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendGetGameStateRequest();
        }

        void updateGameStateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendGetObjectsRequest();
        }

        public void StartGameStateUpdate()
        {
            _updateGameStateTimer = new Timer();
            _updateGameStateTimer.Elapsed += updateGameStateTimer_Elapsed;
            _updateGameStateTimer.Interval = 500;
            _updateGameStateTimer.Start();
            Log.Debug("Start updating game state");
        }

        public void StartObjectsUpdate()
        {
            _updateObjectsTimer = new Timer();
            _updateObjectsTimer.Elapsed += updateObjectsTimer_Elapsed;
            _updateObjectsTimer.Interval = 500;
            _updateObjectsTimer.Start();
            Log.Debug("Start updating game objects");
        }

        public void StopGameStateUpdate()
        {
            _updateGameStateTimer.Stop();
            _updateGameStateTimer.Dispose();
            Log.Debug("Stop updating game state");
        }

        public void StopObjectsUpdate()
        {
            _updateObjectsTimer.Stop();
            _updateObjectsTimer.Dispose();
            Log.Debug("Stop updating game objects");
        }
        #endregion

        public void SendGetMapRequest()
        {
            GetMapRequest request = new GetMapRequest();
            _network.Send(request);
        }

        public void SendBigMapRequest()
        {
            BigMapRequest request = new BigMapRequest();
            _network.Send(request);
            Log.Debug("Send big map request");
        }

        //public void SendGetKingRequest()
        //{
        //    GetKingRequest request = new GetKingRequest();
        //    _network.Send(request);
        //}

        public void SendGetGameStateRequest()
        {
            GetGameStateRequest request = new GetGameStateRequest();
            _network.Send(request);
        }

        public void SendGetObjectsRequest()
        {
            GetObjectsRequest request = new GetObjectsRequest();
            _network.Send(request);
        }

        public void SendComeInCastleRequest(int id)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = id;
            _network.Send(request);
        }

        public void SendMoveKingRequest(int x, int y)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = x;
            request.Y = y;
            _network.Send(request);
        }
    }
}
