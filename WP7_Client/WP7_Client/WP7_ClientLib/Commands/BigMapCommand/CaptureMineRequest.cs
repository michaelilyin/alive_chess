using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос захвата шахты
    /// </summary>
    [ProtoContract]
    public class CaptureMineRequest : ICommand
    {
        private int _mineId;

        public Command Id
        {
            get { return Command.CaptureMineRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        [ProtoMember(1)]
        public int MineId
        {
            get { return _mineId; }
            set { _mineId = value; }
        }
    }
}
