using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос возвращения на большую карту
    /// </summary>
    [ProtoContract]
    public class BigMapRequest : ICommand
    {
        public Command Id
        {
            get { return Command.BigMapRequest; }
        }
    }
}
