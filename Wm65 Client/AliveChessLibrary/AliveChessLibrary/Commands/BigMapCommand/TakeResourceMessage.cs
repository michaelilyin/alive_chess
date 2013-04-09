using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class TakeResourceMessage : ICommand
    {
        [ProtoMember(1)]
        private Resource resource;

        public TakeResourceMessage()
        {
        }

        public TakeResourceMessage(Resource resource)
        {
            this.resource = resource;
        }

        public Command Id
        {
            get { return Command.TakeResourceMessage; }
        }

        public Resource Resource
        {
            get { return resource; }
            set { resource = value; }
        }
    }
}
