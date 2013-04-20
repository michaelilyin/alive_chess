using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartImpeachmentResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public Command Id
        {
            get { return Command.StartImpeachmentResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
