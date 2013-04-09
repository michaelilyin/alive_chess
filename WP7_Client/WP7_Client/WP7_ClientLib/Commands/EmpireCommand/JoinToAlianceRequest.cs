using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class JoinToAlianceRequest : ICommand
    {
        [ProtoMember(1)]
        private int _alianceId;

        public Command Id
        {
            get { return Command.JoinToAlianceRequest; }
        }

        public int AlianceId
        {
            get { return _alianceId; }
            set { _alianceId = value; }
        }
    }
}
