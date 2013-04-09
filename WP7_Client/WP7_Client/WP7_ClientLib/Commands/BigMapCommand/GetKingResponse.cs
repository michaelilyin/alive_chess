using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetKingResponse : ICommand
    {
        public Command Id
        {
            get { return Command.GetKingResponse; }
        }

        [ProtoMember(1)]
        public King King { get; set; }
    }
}
