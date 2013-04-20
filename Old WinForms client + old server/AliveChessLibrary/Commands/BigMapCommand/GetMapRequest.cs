using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetMapRequest : ICommand
    {
        [ProtoMember(1)]
        private bool _is3DClient;

        public Command Id
        {
            get { return Command.GetMapRequest; }
        }

        public bool Is3DClient
        {
            get { return _is3DClient; }
            set { _is3DClient = value; }
        }
    }
}
