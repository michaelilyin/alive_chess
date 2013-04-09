using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос карты
    /// </summary>
    [ProtoContract]
    public class GetMapRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetMapRequest; }
        }
    }
}
