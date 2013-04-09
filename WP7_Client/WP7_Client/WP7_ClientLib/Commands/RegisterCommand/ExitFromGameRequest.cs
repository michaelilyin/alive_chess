using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [ProtoContract]
    public class ExitFromGameRequest : ICommand
    {
        public Command Id
        {
            get { return Command.ExitFromGameRequest; }
        }
    }
}
