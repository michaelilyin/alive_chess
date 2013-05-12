using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChess.GameLayer.LogicLayer
{
    public class CastleCommandController
    {
        private GameCore _gameCore;

        private bool _buildingsModified;
        private bool _unitsModified;
        private bool _buildingQueueModified;
        private bool _recruitingQueueModified;
        private bool _kingOnMap;

        DispatcherTimer timerUpdate = new DispatcherTimer();

        public bool BuildingsModified
        {
            get { return _buildingsModified; }
            set { _buildingsModified = value; }
        }

        public bool UnitsModified
        {
            get { return _unitsModified; }
            set { _unitsModified = value; }
        }

        public bool BuildingQueueModified
        {
            get { return _buildingQueueModified; }
            set { _buildingQueueModified = value; }
        }

        public bool RecruitingQueueModified
        {
            get { return _recruitingQueueModified; }
            set { _recruitingQueueModified = value; }
        }

        public bool KingOnMap
        {
            get { return _kingOnMap; }
            set { _kingOnMap = value; }
        }

        public CastleCommandController(GameCore gameCore)
        {
            _gameCore = gameCore;
            timerUpdate.Tick += new EventHandler(timerUpdate_Tick);
            timerUpdate.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            SendGetProductionQueueRequest();
            SendGetBuildingsRequest();
            SendGetCastleArmyRequest();
            SendGetKingArmyRequest();
        }

        public void StartUpdate()
        {
            timerUpdate.Start();
        }

        public void StopUpdate()
        {
            timerUpdate.Stop();
        }

        public void SendGetBuildingsRequest()
        {
            GetBuildingsRequest request = new GetBuildingsRequest();
            _gameCore.Network.Send(request);
        }

        public void SendGetCreationRequirementsRequest()
        {
            GetCreationRequirementsRequest request = new GetCreationRequirementsRequest();
            _gameCore.Network.Send(request);
        }

        public void SendGetProductionQueueRequest()
        {
            GetProductionQueueRequest request = new GetProductionQueueRequest();
            _gameCore.Network.Send(request);
        }

        public void SendCreateBuildingRequest(InnerBuildingType type)
        {
            CreateBuildingRequest request = new CreateBuildingRequest();
            request.InnerBuildingType = type;
            _gameCore.Network.Send(request);
        }

        public void SendDestroyBuildingRequest(InnerBuildingType type)
        {
            DestroyBuildingRequest request = new DestroyBuildingRequest();
            request.InnerBuildingType = type;
            _gameCore.Network.Send(request);
        }

        public void SendLeaveCastleRequest()
        {
            LeaveCastleRequest request = new LeaveCastleRequest();
            _gameCore.Network.Send(request);
        }

        public void SendGetCastleArmyRequest()
        {
            GetCastleArmyRequest request = new GetCastleArmyRequest();
            _gameCore.Network.Send(request);
        }

        public void SendGetKingArmyRequest()
        {
            GetKingArmyRequest request = new GetKingArmyRequest();
            _gameCore.Network.Send(request);
        }

        public void SendCreateUnitRequest(UnitType type)
        {
            CreateUnitRequest request = new CreateUnitRequest();
            request.UnitType = type;
            _gameCore.Network.Send(request);
        }

        public void SendCancelUnitRecruitingRequest(UnitType type)
        {
            CancelUnitRecruitingRequest request = new CancelUnitRecruitingRequest();
            request.UnitType = type;
            _gameCore.Network.Send(request);
        }

        public void SendCollectUnitsRequest(Dictionary<UnitType, int> units)
        {
            CollectUnitsRequest request = new CollectUnitsRequest();
            request.Units = units;
            _gameCore.Network.Send(request);
        }

        public void SendLeaveUnitsRequest(Dictionary<UnitType, int> units)
        {
            LeaveUnitsRequest request = new LeaveUnitsRequest();
            request.Units = units;
            _gameCore.Network.Send(request);
        }
    }
}
