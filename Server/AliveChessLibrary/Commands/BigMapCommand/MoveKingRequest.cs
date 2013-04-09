using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос перемещения короля
    /// </summary>
    [ProtoContract]
    public class MoveKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int _x;
        [ProtoMember(2)]
        private int _y;

        public Command Id
        {
            get { return Command.MoveKingRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
