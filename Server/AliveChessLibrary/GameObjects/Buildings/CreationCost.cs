using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class CreationCost
    {
        [ProtoMember(1)]
        private Dictionary<ResourceTypes, int> _resources; // ресурсы

        [ProtoMember(2)]
        private double _time = 1; // время в секундах

        public Dictionary<ResourceTypes, int> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
