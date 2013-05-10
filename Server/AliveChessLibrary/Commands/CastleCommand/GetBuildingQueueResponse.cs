using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetBuildingQueueResponse : ICommand
    {
        [ProtoMember(1)]
        private LinkedList<BuildingQueueItem<InnerBuildingType>> _buildingQueue;
        
        public Command Id
        {
            get { return Command.GetBuildingQueueResponse; }
        }

        public LinkedList<BuildingQueueItem<InnerBuildingType>> BuildingQueue
        {
            get { return _buildingQueue; }
            set { _buildingQueue = value; }
        }
    }
}
