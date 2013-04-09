using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Objects;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение карты
    /// </summary>
    [ProtoContract]
    public class GetMapResponse : ICommand
    {
        [ProtoMember(1)]
        private int _mapId;
        [ProtoMember(2)]
        private int _sizeMapX;
        [ProtoMember(3)]
        private int _sizeMapY;
        [ProtoMember(4)]
        private List<Castle> _castles;
        [ProtoMember(5)]
        private List<Mine> _mines;
        [ProtoMember(6)]
        private List<BasePoint> _basePoints;
        [ProtoMember(7)]
        private List<SingleObject> _singleObjects;
        [ProtoMember(8)]
        private List<MultyObject> _multyObjects;
        [ProtoMember(9)]
        private List<Border> _borders;

        public GetMapResponse()
        {
        }

        public GetMapResponse(int mapId, int sizeX, int sizeY,
            List<Castle> castles,
            List<Mine> mines,
            List<BasePoint> basePoints,
            List<Border> borders,
            List<SingleObject> single,
            List<MultyObject> multy)
        {
            this._mapId = mapId;
            this._sizeMapX = sizeX;
            this._sizeMapY = sizeY;
            this._castles = castles;
            this._mines = mines;
            this._basePoints = basePoints;
            this.Borders = borders;
            this.SingleObjects = single;
            this.MultyObjects = multy;
        }

        public virtual Command Id
        {
            get { return Command.GetMapResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public int SizeMapX
        {
            get { return _sizeMapX; }
            set { _sizeMapX = value; }
        }

        /// <summary>
        /// Прото-атрибут: 3
        /// </summary>
        public int SizeMapY
        {
            get { return _sizeMapY; }
            set { _sizeMapY = value; }
        }

        /// <summary>
        /// Прото-атрибут: 4
        /// </summary>
        public List<Castle> Castles
        {
            get { return _castles; }
            set { _castles = value; }
        }

        /// <summary>
        /// Прото-атрибут: 5
        /// </summary>
        public List<Mine> Mines
        {
            get { return _mines; }
            set { _mines = value; }
        }

        /// <summary>
        /// Прото-атрибут: 6
        /// </summary>
        public List<BasePoint> BasePoints
        {
            get { return _basePoints; }
            set { _basePoints = value; }
        }

        /// <summary>
        /// Прото-атрибут: 7
        /// </summary>
        public List<SingleObject> SingleObjects
        {
            get { return _singleObjects; }
            set { _singleObjects = value; }
        }

        /// <summary>
        /// Прото-атрибут: 8
        /// </summary>
        public List<MultyObject> MultyObjects
        {
            get { return _multyObjects; }
            set { _multyObjects = value; }
        }

        /// <summary>
        /// Прото-атрибут: 9
        /// </summary>
        public List<Border> Borders
        {
            get { return _borders; }
            set { _borders = value; }
        }
    }
}
