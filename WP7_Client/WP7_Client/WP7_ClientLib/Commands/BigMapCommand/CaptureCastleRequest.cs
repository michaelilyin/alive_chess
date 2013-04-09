using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос захвата замка
    /// </summary>
    [ProtoContract]
    public class CaptureCastleRequest : ICommand
    {
        public Command Id
        {
            get { return Command.CaptureCastleRequest; }
        }

        [ProtoMember(1)]
        public int CastleId { get; set; }
    }
}
