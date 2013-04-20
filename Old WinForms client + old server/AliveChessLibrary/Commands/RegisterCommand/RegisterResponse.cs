using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class RegisterResponse : ICommand
    {
        [ProtoMember(1)]
        private bool isSuccessed;

        public Command Id
        {
            get { return Command.RegisterResponse; }
        }

        public bool IsSuccessed
        {
            get { return isSuccessed; }
            set { isSuccessed = value; }
        }
    }
}
