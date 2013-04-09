using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос проверки пути
    /// </summary>
    [ProtoContract]
    public class VerifyPathRequest : ICommand
    {
        [ProtoMember(1)]
        private List<FPosition> _path;

        public Command Id
        {
            get { return Command.VerifyPathRequest; }
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
