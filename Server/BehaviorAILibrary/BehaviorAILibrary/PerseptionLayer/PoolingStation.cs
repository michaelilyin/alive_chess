using System;
using System.Collections.Generic;
using System.Drawing;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;

namespace BehaviorAILibrary.PerseptionLayer
{
    /// <summary>
    /// класс для получения информации об объектах виртуального мира
    /// </summary>
    public class PoolingStation
    {
        private readonly TimeSpan updateInterval = new TimeSpan(0, 0, 0, 2, 0);
        private const int visibleRadius = 8;
       
        private Map map;
        private Point position;
        private King king;

        private PoolEntry<King> kings;
        private PoolEntry<Resource> resources;
        private PoolEntry<Mine> mines;
        private PoolEntry<Castle> castles;

        class PoolEntry<T>
        {
            private DateTime updatedTime = DateTime.Now;
            private IList<T> value;

            public PoolEntry(IList<T> source)
            {
                this.value = source;
            }

            public DateTime UpdatedTime
            {
                get { return updatedTime; }
                set { updatedTime = value; }
            }

            public IList<T> Value
            {
                get { return this.value; }
                set { this.value = value; }
            }
        }

        public PoolingStation()
        {
            //
        }

        public PoolingStation(King king)
            : this()
        {
            this.king = king;
            this.map = king.Map;
            this.position = new Point(king.X, king.Y);
            this.kings = new PoolEntry<King>(map.Kings);
            this.mines = new PoolEntry<Mine>(map.Mines);
            this.castles = new PoolEntry<Castle>(map.Castles);
            this.resources = new PoolEntry<Resource>(map.Resources);
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public IList<King> Kings
        {
            get { return GetArray<King>(); }
        }

        public IList<Castle> Castles
        {
            get { return GetArray<Castle>(); }
        }
        public IList<Mine> Mines
        {
            get { return GetArray<Mine>(); }
        }

        public IList<Resource> Resources
        {
            get { return GetArray<Resource>(); }
        }

        private PoolEntry<T> DecideType<T>() where T : class, IMapObject
        {
            // FAKE (Must be corrected)
            kings.Value = map.Kings;

            if (typeof(T).Equals(typeof(King)))
                return kings as PoolEntry<T>;
            if (typeof(T).Equals(typeof(Castle)))
                return castles as PoolEntry<T>;
            if (typeof(T).Equals(typeof(Mine)))
                return mines as PoolEntry<T>;
            if (typeof(T).Equals(typeof(Resource)))
                return resources as PoolEntry<T>;
            throw new InvalidCastException("Type isn't supported");
        }

        private IList<T> GetArray<T>() where T : class, IMapObject
        {
            PoolEntry<T> array = DecideType<T>();
            if ((DateTime.Now - array.UpdatedTime) > updateInterval)
            {
                List<T> newArray = new List<T>();
                int left = position.X - visibleRadius;
                int right = position.X + visibleRadius;
                int top = position.Y - visibleRadius;
                int bottom = position.Y + visibleRadius;
                foreach (var view in array.Value)
                {
                    if (view.GetType().FindInterfaces(Compare, typeof (IMapObject)).Length > 0)
                    {
                        if (view.X > left && view.X < right &&
                            view.Y < bottom && view.Y > top)
                        {
                            newArray.Add(view);
                        }
                    }
                }
                array.Value = newArray;
                array.UpdatedTime = DateTime.Now;
            }

            return array.Value;
        }

        private bool Compare(Type type, object criteria)
        {
            return type.ToString() == criteria.ToString();
        }
    }
}
