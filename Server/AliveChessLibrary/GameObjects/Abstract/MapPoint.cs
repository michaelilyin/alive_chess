using System;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Abstract
{ 
    /// <summary>
    /// ячейка карты
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

        /// <summary>
        /// установка объекта - владельца ячейки
        /// </summary>
        /// <param name="owner"></param>
        public void SetOwner(IMapObject owner)
        {
            // необходимо удалить текущего владельца ячейки
            if (owner == null)
            {
                // предыдущий владелец установлен (объект динамический)
                if (_prevOwner != null)
                {
                    _owner = _prevOwner;
                    _type = _prevOwner.Type;
                }
                else 
                    throw new ArgumentNullException("Last owner is null so you shouldn't set current owner to null as well. Probably object is static");
            }
            else // необходимо установить нового владельца
            {
                // предыдущий владелец установлен (объект динамический)
                if (_owner != null)
                {
                    if (_owner != owner)
                    {
                        _prevOwner = _owner;
                        _owner = owner;
                        _type = _owner.Type;
                    }
                }
                else // предыдущий владелец не установлен (объект статический)
                {
                    _owner = owner;
                    _prevOwner = owner;
                    _type = _owner.Type;
                }
            }
        }

        #region Properties

        /// <summary>
        /// владелец ячейки
        /// </summary>
        public IMapObject Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// предыдущий владелец ячейки
        /// </summary>
        public IMapObject Previous
        {
            get { return _prevOwner; }
        }

        /// <summary>
        /// стоимость прохождения объекта, являющегося
        /// владельцем ячейки
        /// </summary>
        public float WayCost
        {
            get { return _owner.WayCost; }
            set { _owner.WayCost = value; }
        }

        /// <summary>
        /// тип ячейки
        /// </summary>
        public PointTypes PointType
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
