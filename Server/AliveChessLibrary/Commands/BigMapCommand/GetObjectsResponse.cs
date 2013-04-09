using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение объектов в области видимости
    /// </summary>
    [ProtoContract]
    public class GetObjectsResponse : ICommand
    {
        [ProtoMember(1)] 
        private List<Resource> _resources;
        [ProtoMember(2)] 
        private List<King> _kings;

        public GetObjectsResponse(){}

        public GetObjectsResponse(List<Resource> resources, List<King> king)
        {
            this._resources = resources;
            this._kings = king;
        }

        public Command Id
        {
            get { return Command.GetObjectsResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public List<King> Kings
        {
            get { return _kings; }
            set { _kings = value; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        //public List<MapPoint> MovableObjects
        //{
        //    get { return _movableObjects; }
        //    set { _movableObjects = value; }
        //}

        //public List<MapSector> Sectors
        //{
        //    get { return _sectors; }
        //    set { _sectors = value; }
        //}
    }
}
