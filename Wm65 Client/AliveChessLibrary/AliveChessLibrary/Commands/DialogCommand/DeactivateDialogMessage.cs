using ProtoBuf;

namespace AliveChessLibrary.Commands.DialogCommand
{
    [ProtoContract]
    public class DeactivateDialogMessage : ICommand
    {
        public Command Id
        {
            get { return Command.DeactivateDialogMessage; }
        }

    }
}
