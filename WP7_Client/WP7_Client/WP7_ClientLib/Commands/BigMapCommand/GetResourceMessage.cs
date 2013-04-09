using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение ресурса
    /// </summary>
    [ProtoContract]
    public class GetResourceMessage : ICommand
    {
        private Resource _resource;
        private bool _fromMine;

        public GetResourceMessage()
        {
        }

        public GetResourceMessage(Resource resource, bool fromMine)
        {
            this._fromMine = fromMine;
            this._resource = resource;
        }

        public Command Id
        {
            get { return Command.GetResourceMessage; }
        }

        [ProtoMember(1)]
        public Resource Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }

        [ProtoMember(2)]
        public bool FromMine
        {
            get { return _fromMine; }
            set { _fromMine = value; }
        }
    }
}
