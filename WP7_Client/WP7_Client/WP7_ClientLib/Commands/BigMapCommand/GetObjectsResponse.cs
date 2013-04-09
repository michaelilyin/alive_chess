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
        private List<Resource> _resources;
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

        [ProtoMember(2)]
        public List<King> Kings
        {
            get { return _kings; }
            set { _kings = value; }
        }

        [ProtoMember(1)]
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
    }
}
