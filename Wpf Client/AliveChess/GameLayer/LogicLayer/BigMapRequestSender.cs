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
    public class BigMapRequestSender
    {
        DispatcherTimer timerUpdateGameState = new DispatcherTimer();
        DispatcherTimer timerGetObjects = new DispatcherTimer();

        private bool _waitingGetGameStateResponse = false;
        private bool _waitingGetObjectsResponse = false;

        private MapScene _mapScene = null;

        public MapScene MapScene
        {
            get { return _mapScene; }
            set { _mapScene = value; }
        }

        public BigMapRequestSender()
        {
            timerUpdateGameState.Tick += new EventHandler(timerUpdateGameState_Tick);
            timerUpdateGameState.Interval = new TimeSpan(0, 0, 0, 0, 200);
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
            GameCore.Instance.Network.Send(request);
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
            GameCore.Instance.Network.Send(request);
        }

        public void SendGetGameStateRequest()
        {
            GetGameStateRequest request = new GetGameStateRequest();
            GameCore.Instance.Network.Send(request);
        }

        public void ReceiveGetStateResponse(GetGameStateResponse response)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_mapScene.ShowGetStateResult));
            timerUpdateGameState.Start();
        }

        public void SendGetObjectsRequest()
        {
            GetObjectsRequest r = new GetObjectsRequest();
            GameCore.Instance.Network.Send(r);
        }

        public void ReceiveGetObjectsResponse(GetObjectsResponse response)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_mapScene.ShowGetObjectsResult));
            timerGetObjects.Start();
        }

        public void SendComeInCastleRequest(int id)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = id;
            GameCore.Instance.Network.Send(request);
        }

        public void ReceiveComeInCastleResponse(ComeInCastleResponse response)
        {
            timerUpdateGameState.Stop();
            Uri uri = new Uri("/GameLayer/PresentationLayer/CastleScene.xaml",
                                                       UriKind.Relative);
            /*base.MoveTo(uri);
            if ((response.CastleId == _player.GetKingList().First().Castles[0].Id) && (NavigationService != null))
                NavigationService.Navigate(uri);*/
        }

        public void SendMoveKingRequest(Point kingDest)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = (int)kingDest.X;
            request.Y = (int)kingDest.Y;
            GameCore.Instance.Network.Send(request);
        }

        public void ReceiveMoveKingResponse(MoveKingResponse response)
        {
            if (_mapScene != null)
                _mapScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_mapScene.ShowMoveKingResult));
        }
    }
}
