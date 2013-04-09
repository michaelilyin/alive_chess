using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение короля
    /// </summary>
    [ProtoContract]
    public class GetKingResponse : ICommand
    {
        [ProtoMember(1)]
        private King _king;

        public Command Id
        {
            get { return Command.GetKingResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }
    }
}
