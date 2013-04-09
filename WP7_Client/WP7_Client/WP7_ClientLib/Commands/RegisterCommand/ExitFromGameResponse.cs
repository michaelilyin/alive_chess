using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class ExitFromGameResponse : ICommand
    {
        public Command Id
        {
            get { return Command.ExitFromGameResponse; }
        }
    }
}
