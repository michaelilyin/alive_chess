using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Network.CommandControllers
{
    public class CastleCommandController
    {
        private Network _network;

        private Timer _updateCastleTimer;

        public CastleCommandController(Network network)
        {
            _network = network;
            _updateCastleTimer = new Timer();
            _updateCastleTimer.Interval = 250;
            _updateCastleTimer.Elapsed += _updateCastleTimer_Elapsed;
        }

        public void StartCastleUpdate()
        {
            _updateCastleTimer.Start();
        }

        public void StopCastleUpdate() 
        {
            _updateCastleTimer.Stop();
        }

        void _updateCastleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendGetProductionQueueRequest();
            SendGetBuildingsRequest();
            SendGetCastleArmyRequest();
            SendGetKingArmyRequest();
        }

        public void SendLeaveCastleRequest()
        {
            LeaveCastleRequest request = new LeaveCastleRequest();
            _network.Send(request);
        }

        public void SendGetBuildingsRequest()
        {
            GetBuildingsRequest request = new GetBuildingsRequest();
            _network.Send(request);
        }

        public void SendGetCreationRequirementsRequest()
        {
            GetCreationRequirementsRequest request = new GetCreationRequirementsRequest();
            _network.Send(request);
        }

        public void SendGetProductionQueueRequest()
        {
            GetProductionQueueRequest request = new GetProductionQueueRequest();
            _network.Send(request);
        }

        public void SendCreateBuildingRequest(InnerBuildingType type)
        {
            CreateBuildingRequest request = new CreateBuildingRequest();
            request.InnerBuildingType = type;
            _network.Send(request);
            Logger.Log.Debug(String.Format("Create <<{0}>> building request sended", type.ToString()));
        }

        public void SendDestroyBuildingRequest(InnerBuildingType type)
        {
            DestroyBuildingRequest request = new DestroyBuildingRequest();
            request.InnerBuildingType = type;
            _network.Send(request);
        }

        public void SendGetCastleArmyRequest()
        {
            GetCastleArmyRequest request = new GetCastleArmyRequest();
            _network.Send(request);
        }

        public void SendGetKingArmyRequest()
        {
            GetKingArmyRequest request = new GetKingArmyRequest();
            _network.Send(request);
        }

        public void SendCreateUnitRequest(UnitType type)
        {
            CreateUnitRequest request = new CreateUnitRequest();
            request.UnitType = type;
            _network.Send(request);
        }

        public void SendCancelUnitRecruitingRequest(UnitType type)
        {
            CancelUnitRecruitingRequest request = new CancelUnitRecruitingRequest();
            request.UnitType = type;
            _network.Send(request);
        }

        public void SendCollectUnitsRequest(Dictionary<UnitType, int> units)
        {
            CollectUnitsRequest request = new CollectUnitsRequest();
            request.Units = units;
            _network.Send(request);
        }

        public void SendLeaveUnitsRequest(Dictionary<UnitType, int> units)
        {
            LeaveUnitsRequest request = new LeaveUnitsRequest();
            request.Units = units;
            _network.Send(request);
        }
    }
}
