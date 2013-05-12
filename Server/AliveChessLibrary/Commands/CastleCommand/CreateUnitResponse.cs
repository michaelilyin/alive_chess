using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class CreateUnitResponse : ICommand
    {
        [ProtoMember(1)]
        private LinkedList<BuildingQueueItem<UnitType>> _productionQueue;

        public LinkedList<BuildingQueueItem<UnitType>> ProductionQueue
        {
            get { return _productionQueue; }
            set { _productionQueue = value; }
        }

        public Command Id
        {
            get { return Command.CreateUnitResponse; }
        }
    }
}
