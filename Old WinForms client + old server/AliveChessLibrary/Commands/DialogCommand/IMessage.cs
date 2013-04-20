using AliveChessLibrary.Interaction;

namespace AliveChessLibrary.Commands.DialogCommand
{
    public interface IMessage : ICommand
    {
        int DisputeId
        {
            get;
            set;
        }

        DialogState State
        {
            get;
            set;
        }
    }
}
