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

        private bool _resourcesModified;
        private bool _dynamicObjectsModified;
        private bool _buildingOwnerModified;
        private bool _mapModified;
        private bool _kingInCastle;

        public bool ResourcesModified
        {
            get { return _resourcesModified; }
            set { _resourcesModified = value; }
        }

        public bool DynamicObjectsModified
        {
            get { return _dynamicObjectsModified; }
            set { _dynamicObjectsModified = value; }
        }

        public bool BuildingsModified
        {
            get { return _buildingOwnerModified; }
            set { _buildingOwnerModified = value; }
        }

        public bool MapModified
        {
            get { return _mapModified; }
            set { _mapModified = value; }
        }

        public bool KingInCastle
        {
            get { return _kingInCastle; }
            set { _kingInCastle = value; }
        }

        private GameCore _gameCore;

        public BigMapCommandController(GameCore gameCore)
        {
            AliveChessLibrary.DebugConsole.AllocConsole();
            _gameCore = gameCore;
            timerUpdateGameState.Tick += new EventHandler(timerUpdateGameState_Tick);
            timerUpdateGameState.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerGetObjects.Tick += new EventHandler(timerGetObjects_Tick);
            timerGetObjects.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }

        /// <summary>
        /// Обновление состояния игры (ресурсы в хранилище, положение короля)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerUpdateGameState_Tick(object sender, EventArgs e)
        {
            AliveChessLibrary.DebugConsole.WriteLine(this, DateTime.Now.ToString());
            SendGetGameStateRequest();
            //timerUpdateGameState.Stop();
        }

        /// <summary>
        /// Обновление области видимости (ресурсы, короли, замки, шахты)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerGetObjects_Tick(object sender, EventArgs e)
        {
            SendGetObjectsRequest();
            //timerGetObjects.Stop();
        }

        public void StartGameStateUpdate()
        {
            timerUpdateGameState.Start();
        }

        public void StartObjectsUpdate()
        {
            timerGetObjects.Start();
        }

        public void StopGameStateUpdate()
        {
            timerUpdateGameState.Stop();
        }

        public void StopObjectsUpdate()
        {
            timerGetObjects.Stop();
        }

        public void SendGetMapRequest()
        {
            GetMapRequest request = new GetMapRequest();
            _gameCore.Network.Send(request);
        }

        public void SendBigMapRequest()
        {
            BigMapRequest request = new BigMapRequest();
            _gameCore.Network.Send(request);
        }

        public void SendGetKingRequest()
        {
            GetKingRequest request = new GetKingRequest();
            _gameCore.Network.Send(request);
        }
        
        public void SendGetGameStateRequest()
        {
            GetGameStateRequest request = new GetGameStateRequest();
            _gameCore.Network.Send(request);
            AliveChessLibrary.DebugConsole.WriteLine(this, "sent");
        }

        public void SendGetObjectsRequest()
        {
            GetObjectsRequest r = new GetObjectsRequest();
            _gameCore.Network.Send(r);
        }

        public void SendComeInCastleRequest(int id)
        {
            ComeInCastleRequest request = new ComeInCastleRequest();
            request.CastleId = id;
            _gameCore.Network.Send(request);
        }

        public void SendMoveKingRequest(Point kingDest)
        {
            MoveKingRequest request = new MoveKingRequest();
            request.X = (int)kingDest.X;
            request.Y = (int)kingDest.Y;
            _gameCore.Network.Send(request);
        }
    }
}
