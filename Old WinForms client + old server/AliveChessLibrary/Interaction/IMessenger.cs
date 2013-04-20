using AliveChessLibrary.Commands;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Interaction
{
    public interface IMessenger
    {
        void SendAIMessage<T>(T message) where T : INotification;
        void SendNetworkMessage<T>(T messsge) where T : ICommand;
    }
}
