using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetResourceMessage : ICommand
    {
        [ProtoMember(1)]
        private Resource resource;
        [ProtoMember(2)]
        private bool fromMine;

        public GetResourceMessage()
        {
        }

        public GetResourceMessage(Resource resource, bool fromMine)
        {
            this.fromMine = fromMine;
            this.resource = resource;
        }

        public Command Id
        {
            get { return Command.GetResourceMessage; }
        }

        public Resource Resource
        {
            get { return resource; }
            set { resource = value; }
        }

        public bool FromMine
        {
            get { return fromMine; }
            set { fromMine = value; }
        }
    }
}
