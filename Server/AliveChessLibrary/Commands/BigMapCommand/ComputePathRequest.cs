using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос расчета пути игрока
    /// </summary>
    [ProtoContract]
    public class ComputePathRequest : ICommand
    {
        [ProtoMember(1)]
        private FPosition _start;
        [ProtoMember(2)]
        private FPosition _finish;

        public Command Id
        {
            get { return Command.ComputePathRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public FPosition Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public FPosition Finish
        {
            get { return _finish; }
            set { _finish = value; }
        }
    }
}
