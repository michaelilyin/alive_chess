using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос возвращения на большую карту
    /// </summary>
    [ProtoContract]
    public class BigMapRequest : ICommand
    {
        /// <summary>
        /// Идентификатор:6
        /// </summary>
        public Command Id
        {
            get { return Command.BigMapRequest; }
        }
    }
}
