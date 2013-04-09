using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос захвата шахты
    /// </summary>
    [ProtoContract]
    public class CaptureMineRequest : ICommand
    {
        [ProtoMember(1)]
        private int _mineId;

        public Command Id
        {
            get { return Command.CaptureMineRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int MineId
        {
            get { return _mineId; }
            set { _mineId = value; }
        }
    }
}
