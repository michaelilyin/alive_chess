using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// изъятие ресурса
    /// </summary>
    [ProtoContract]
    public class TakeResourceMessage : ICommand
    {
        private Resource _resource;

        public TakeResourceMessage()
        {
        }

        public TakeResourceMessage(Resource resource)
        {
            this._resource = resource;
        }

        public Command Id
        {
            get { return Command.TakeResourceMessage; }
        }

        [ProtoMember(1)]
        public Resource Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }
    }
}
