using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using AliveChessLibrary;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessServer.LogicLayer.AI.PerseptionLayer;

namespace AliveChessServer.LogicLayer.AI.PerceptionLayer
{
    /// <summary>
    /// класс для получения информации об объектах виртуального мира
    /// </summary>
    public class PoolingStation
    {
        private readonly TimeSpan updateInterval = new TimeSpan(0, 0, 0, 2, 0);
        private const int visibleRadius = 8;
        //private PoolEntry<King> kings = new PoolEntry<King>();
        //private PoolEntry<Resource> resources = new PoolEntry<Resource>();
        //private PoolEntry<Mine> mines = new PoolEntry<Mine>();
        //private PoolEntry<Castle> castles = new PoolEntry<Castle>();
        private Map map;
        private Point position;
        private King king;

        private EntitySet<King> m_kings;
        private EntitySet<Resource> m_resources;
        private EntitySet<Mine> m_mines;
        private EntitySet<Castle> m_castles;

        public PoolingStation()
        {
            m_kings = new EntitySet<King>();
            m_mines = new EntitySet<Mine>();
            m_castles = new EntitySet<Castle>();
            m_resources = new EntitySet<Resource>();
        }

        //class PoolEntry<T>
        //{
        //    private DateTime updatedTime = DateTime.Now;
        //    private List<T> value;

        //    public DateTime UpdatedTime
        //    {
        //        get { return updatedTime; }
        //        set { updatedTime = value; }
        //    }

        //    public List<T> Value
        //    {
        //        get { return this.value; }
        //        set { this.value = value; }
        //    }
        //}

        public PoolingStation(King king)
            : this()
        {
            this.map = king.Map;
            this.position = new Point(king.X, king.Y);
            this.king = king;
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

        private EntitySet<T> DecideType<T>() where T : class, IMapObject
        {
            Type t = typeof(EntitySet<T>);
            EntitySet<T> resultCollection = null;

            if (t.Equals(m_kings.GetType()))
            {
                Debug.Assert((resultCollection = (m_kings as EntitySet<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(m_castles.GetType()))
            {
                Debug.Assert((resultCollection = (m_castles as EntitySet<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(m_mines.GetType()))
            {
                Debug.Assert((resultCollection = (m_mines as EntitySet<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(m_resources.GetType()))
            {
                Debug.Assert((resultCollection = (m_resources as EntitySet<T>)) != null);
                return resultCollection;
            }
            throw new AliveChessException("Collection contains incorrect type");
        }

        private IEnumerable<T> DecideMapType<T>() where T : class, IMapObject
        {
            Type t = typeof(EntitySet<T>);
            IEnumerable<T> resultCollection = null;

            if (t.Equals((map.Kings).GetType()))
            {
                Debug.Assert((resultCollection = (map.Kings as IEnumerable<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(map.Castles.GetType()))
            {
                Debug.Assert((resultCollection = (map.Castles as IEnumerable<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(map.Mines.GetType()))
            {
                Debug.Assert((resultCollection = (map.Mines as IEnumerable<T>)) != null);
                return resultCollection;
            }
            if (t.Equals(map.Resources.GetType()))
            {
                Debug.Assert((resultCollection = (map.Resources as IEnumerable<T>)) != null);
                return resultCollection;
            }
            throw new AliveChessException("Collection contains incorrect type");
        }

        private IList<T> GetArray<T>() where T : class, IMapObject
        {
            EntitySet<T> array = DecideType<T>();
            if ((DateTime.Now - array.GetUpdateTime()) > updateInterval)
            {
                List<T> newArray = new List<T>();
                int left = position.X - visibleRadius;
                int right = position.X + visibleRadius;
                int top = position.Y - visibleRadius;
                int bottom = position.Y + visibleRadius;
                foreach (var item in DecideMapType<T>())
                {
                    if (item.GetType().FindInterfaces(Compare, typeof (IMapObject)).Length > 0)
                    {
                        IMapObject singlePoint = (IMapObject) item;
                        if (singlePoint.X > left && singlePoint.X < right &&
                            singlePoint.Y < bottom && singlePoint.Y > top)
                        {
                            newArray.Add(item);
                        }
                    }
                }
                array.Assign(newArray);
                array.SetUpdateTime(DateTime.Now);
            }

            return array;
        }

        private bool Compare(Type type, object criteria)
        {
            return type.ToString() == criteria.ToString();
        }
    }
}
