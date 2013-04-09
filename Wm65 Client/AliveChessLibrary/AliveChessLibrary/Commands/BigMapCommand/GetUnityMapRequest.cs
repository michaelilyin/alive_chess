using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetUnityMapRequest : ICommand
    {
        [ProtoMember(1)]
        private string name;

        public Command Id
        {
            get { return Command.GetUnityMapRequest; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
