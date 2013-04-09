using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// результат проверки пути
    /// </summary>
    [ProtoContract]
    public class VerifyPathResponse : ICommand
    {
        [ProtoMember(1)]
        private float _x;
        [ProtoMember(2)]
        private float _y;

        public Command Id
        {
            get { return Command.VerifyPathResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
