using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// расчитанный путь игрока
    /// </summary>
    [ProtoContract]
    public class ComputePathResponse : ICommand
    {
        [ProtoMember(1)]
        private List<FPosition> _path;

        public Command Id
        {
            get { return Command.ComputePathResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public List<FPosition> Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
