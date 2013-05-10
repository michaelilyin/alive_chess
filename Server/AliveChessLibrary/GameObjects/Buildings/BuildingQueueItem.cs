using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class BuildingQueueItem<T>
    {
        [ProtoMember(1)]
        private double _totalCreationTime;
        [ProtoMember(2)]
        private double _remainingCreationTime;
        [ProtoMember(3)]
        private T _type;

        public BuildingQueueItem(T type, double totalCreationTime)
        {
            _type = type;
            _totalCreationTime = totalCreationTime;
            _remainingCreationTime = totalCreationTime;
        }

        public BuildingQueueItem()
        {
        }

        public double TotalCreationTime
        {
            get { return _totalCreationTime; }
            set { _totalCreationTime = value; }
        }

        public double RemainingCreationTime
        {
            get { return _remainingCreationTime; }
            set { _remainingCreationTime = value; }
        }

        public T Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public double TimeToPercent()
        {
            return (1 - _remainingCreationTime / _totalCreationTime) * 100;
        }
    }
}
