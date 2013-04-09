using System;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{ 
    /// <summary>
    /// класс представляет собой отдельную ячейку карты.
    /// </summary>
    public class MapPoint : ILocation
    {
        #region Variables
     
        private int _mapPointX;
        private int _mapPointY; 
        private PointTypes _type;
       
        private MapSector _mapSector;
        private IMapObject _owner;
        private IMapObject _prevOwner;

        #endregion

        public void SetOwner(IMapObject owner)
        {
            if (owner == null)
            {
                if (_prevOwner != null)
                {
                    _owner = _prevOwner;
                    _type = _prevOwner.Type;
                }
                else throw new ArgumentNullException();
            }
            else
            {
                if (_owner != null)
                {
                    if (_owner != owner)
                    {
                        _prevOwner = _owner;
                        _owner = owner;
                        _type = _owner.Type;
                    }
                }
                else
                {
                    _owner = owner;
                    _prevOwner = owner;
                    _type = _owner.Type;
                }
            }
        }

        #region Properties

        public IMapObject Owner
        {
            get { return _owner; }
        }

        public IMapObject Previous
        {
            get { return _prevOwner; }
        }

        public float WayCost
        {
            get { return _owner.WayCost; }
            set { _owner.WayCost = value; }
        }

        /// <summary>
        /// тип ячейки
        /// </summary>
        public PointTypes MapPointType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// ссылка на сектор в случае если ячейка принадлежит сектору
        /// </summary>
        public MapSector MapSector
        {
            get { return _mapSector; }
            set
            {
                if (_mapSector != value)
                {
                    _mapSector = value;
                    _mapSector.MapPoints.Add(this);
                }
            }
        }

        /// <summary>
        /// координата X
        /// </summary>
        public int X
        {
            get { return _mapPointX; }
            set 
            {
                if (_mapPointX != value)
                {
                    _mapPointX = value;
                }
            }
        }

        /// <summary>
        /// координата Y
        /// </summary>
        public int Y
        {
            get { return _mapPointY; }
            set 
            {
                if (_mapPointY != value)
                {
                    _mapPointY = value;
                }
            }
        }

        #endregion
    }
}
