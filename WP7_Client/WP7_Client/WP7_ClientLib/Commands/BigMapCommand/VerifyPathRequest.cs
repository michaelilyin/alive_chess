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
        private List<FPosition> _path;

        public Command Id
        {
            get { return Command.VerifyPathRequest; }
        }

        [ProtoMember(1)]
        public List<FPosition> Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
