using AliveChessLibrary.Commands;

namespace AliveChessLibrary.Interaction
{
    public interface IMessenger
    {
        /// <summary>
        /// Send message to AI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        void SendAIMessage<T>(T message) where T : IStimulus;
       
        /// <summary>
        /// Send message to user by network
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messsge"></param>
        void SendNetworkMessage<T>(T messsge) where T : ICommand;

        void SendNonSerializedMessage<T>(T message) where T : INonSerializable;
    }
}
