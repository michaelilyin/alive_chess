using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.BigMapCommand;

namespace AliveChess.GameLayer.LogicLayer
{
    public class BigMapCommandController
    {
        DispatcherTimer timerUpdateGameState = new DispatcherTimer();
        DispatcherTimer timerGetObjects = new DispatcherTimer();

        private bool _waitingGetGameStateResponse = false;
        private bool _waitingGetObjectsResponse = false;

        private MapScene _mapScene = null;

        private GameCore _gameCore;

        public MapScene MapScene
        {
            get { return _mapScene; }
            set { _mapScene = value; }
        }

        public BigMapCommandController(GameCore gameCore)
        {
            _gameCore = gameCore;
            timerUpdateGameState.Tick += new EventHandler(timerUpdateGameState_Tick);
            timerUpdateGameState.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timerGetObjects.Tick += new EventHandler(timerGetObjects_Tick);
            timerGetObjects.Interval = new TimeSpan(0, 0, 0, 0, 20);
        }

        /// <summary>
        /// Обновление состояния игры (ресурсы в хранилище, положение короля)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerUpdateGameState_Tick(object sender, EventArgs e)
        {
            SendGetGameStateRequest();
            timerUpdateGameState.Stop();
        }

        /// <summary>
        /// Обновление области видимости (ресурсы, короли, замки, шахты)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerGetObjects_Tick(object sender, EventArgs e)
        {
            SendGetObjectsRequest();
            timerGetObjects.Stop();
        }

        public void SendGetMapRequest()
        {
            GetMapRequest request = new GetMapRequest();
            _gameCore.Network.Send(request);
        }

        public void ReceiveGetMapResponse(GetMapResponse responce)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_mapScene.ShowGetMapResult));
            timerUpdateGameState.Start();
            timerGetObjects.Start();
        }

        public void SendGetKingRequest()
        {
            GetKingRequest request = new GetKingRequest();
            _gameCore.Network.Send(request);
        }

        public void ReceiveGetKingResponse(GetKingResponse response)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Background, new Action(_mapScene.ShowGetKingResult));
        }

        public void SendGetGameStateRequest()
        {
            GetGameStateRequest request = new GetGameStateRequest();
            _gameCore.Network.Send(request);
        }

        public void ReceiveGetGameStateResponse(GetGameStateResponse response)
        {
            if (_mapScene != null)
            {
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Render, new Action(_mapScene.ShowGetGameStateResult));
                timerUpdateGameState.Start();
            }
        }

        public void SendGetObjectsRequest()
        {
            GetObjectsRequest r = new GetObjectsRequest();
            _gameCore.Network.Send(r);
        }

        public void ReceiveGetObjectsResponse(GetObjectsResponse response)
        {
            if (_mapScene != null)
            {
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Render, new Action(_mapScene.ShowGetObjectsResult));
                timerGetObjects.Start();
            }
        }

        public void SendComeInCastleRequest(int id)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = id;
            _gameCore.Network.Send(request);
        }

        public void ReceiveComeInCastleResponse(ComeInCastleResponse response)
        {
            timerUpdateGameState.Stop();
            timerGetObjects.Stop();
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Render, new Action(_mapScene.ShowComeInCastleResult));
            _mapScene = null;
        }

        public void SendMoveKingRequest(Point kingDest)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = (int)kingDest.X;
            request.Y = (int)kingDest.Y;
            _gameCore.Network.Send(request);
        }

        public void ReceiveMoveKingResponse(MoveKingResponse response)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Background, new Action(_mapScene.ShowMoveKingResult));
        }

        public void ReceiveCaptureMineResponse(CaptureMineResponse response)
        {
            if (_mapScene != null)
            {
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Background, new Action(_mapScene.ShowCaptureMineResult));
            }
        }
    }
}
