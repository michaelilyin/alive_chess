using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Resources
{
    [ProtoContract]
    public class Resource : IResource, IEquatable<int>, IEquatable<MapPoint>, IDynamic<Resource>, ISinglePoint
    {

        #region Constructors

        public Resource()
        {
            Map = null;
            Vault = null;
        }

        #endregion

        #region Initialization

        public void Initialize(Map map)
        {
            Map = map;
        }

        public void Initialize(Map map, ResourceTypes rType)
        {
            ResourceType = rType;

            Initialize(map);
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

        #endregion

        #region Methods

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0;
        }

        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0;
        }

        public void Disappear()
        {
            if (ChangeMapStateEvent != null)
                ChangeMapStateEvent(this, new UpdateWorldEventArgs(Map, new FPosition(X, Y),
                                                                   UpdateType.ResourceDisappear));
        }

        #endregion

        #region Properties

        [ProtoMember(2)]
        public int X { get; set; }

        [ProtoMember(3)]
        public int Y { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.Resource; }
        }

        public int ImageId { get; set; }

        [ProtoMember(6)]
        public float WayCost { get; set; }

        [ProtoMember(1)]
        public int Id { get; set; }

        public MapPoint ViewOnMap { get; set; }

        public int? MapPointId { get; set; }

        [ProtoMember(4)]
        public ResourceTypes ResourceType { get; set; }

        [ProtoMember(5)]
        public int CountResource { get; set; }

        public Map Map { get; set; }

        public King King { get; set; }

        public ResourceStore Vault { get; set; }

        #endregion

        public event ChangeMapStateHandler<Resource> ChangeMapStateEvent;
    }
}
