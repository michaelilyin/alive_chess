using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос состояния игры
    /// </summary>
    [ProtoContract]
    public class GetGameStateRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetGameStateRequest; }
        }
    }
}
