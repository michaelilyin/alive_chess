using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetProductionQueueResponse : ICommand
    {
        [ProtoMember(1)]
        private LinkedList<BuildingQueueItem<InnerBuildingType>> _buildingQueue;
        [ProtoMember(2)]
        private LinkedList<BuildingQueueItem<UnitType>> _recruitingQueue;
        
        public Command Id
        {
            get { return Command.GetProductionQueueResponse; }
        }

        public LinkedList<BuildingQueueItem<InnerBuildingType>> BuildingQueue
        {
            get { return _buildingQueue; }
            set { _buildingQueue = value; }
        }

        public LinkedList<BuildingQueueItem<UnitType>> RecruitingQueue
        {
            get { return _recruitingQueue; }
            set { _recruitingQueue = value; }
        }
    }
}
