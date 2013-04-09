using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Objects
{
    [ProtoContract]
    public class SingleObject : ISinglePoint
    {
        public SingleObject()
        {
        }

        public void Initialize(Map map)
        {
            Map = map;
        }


        public void Initialize(int id, Map map)
        {
            Initialize(map);
            Id = id;
        }

        public void Initialize(Map map, MapPoint point)
        {
            Initialize(map);
            if (point != null)
                AddView(point);
        }

        public virtual void AddView(MapPoint point)
        {
            ViewOnMap = point;
            ViewOnMap.SetOwner(this);
        }

        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        [ProtoMember(2)]
        public int X { get; set; }

        [ProtoMember(3)]
        public int Y { get; set; }

        public int ImageId { get; set; }

        [ProtoMember(5)]
        public float WayCost { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.SingleObject; }
        }

        public MapPoint ViewOnMap { get; set; }

        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(4)]
        public SingleObjectType SingleObjectType { get; set; }

        public Map Map { get; set; }
    }
}
