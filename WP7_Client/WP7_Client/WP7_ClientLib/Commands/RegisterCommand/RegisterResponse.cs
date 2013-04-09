using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class RegisterResponse : ICommand
    {
        public Command Id
        {
            get { return Command.RegisterResponse; }
        }

        [ProtoMember(1)]
        public bool IsSuccessed { get; set; }
    }
}
