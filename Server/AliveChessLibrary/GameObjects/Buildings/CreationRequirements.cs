using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class CreationRequirements
    {
        [ProtoMember(1)]
        private Dictionary<ResourceTypes, int> _resources = new Dictionary<ResourceTypes, int>(); // ресурсы

        [ProtoMember(2)]
        private double _time = 1; // время в секундах

        [ProtoMember(3)]
        private SortedSet<InnerBuildingType> _requiredBuildings = new SortedSet<InnerBuildingType>(); // необходимые для создания постройки

        public Dictionary<ResourceTypes, int> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public double CreationTime
        {
            get { return _time; }
            set { _time = value; }
        }

        public SortedSet<InnerBuildingType> RequiredBuildings
        {
            get { return _requiredBuildings; }
            set { _requiredBuildings = value; }
        }
    }
}
