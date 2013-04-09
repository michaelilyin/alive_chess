using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// результат проверки пути
    /// </summary>
    [ProtoContract]
    public class VerifyPathResponse : ICommand
    {
        private float _x;
        private float _y;

        public Command Id
        {
            get { return Command.VerifyPathResponse; }
        }

        [ProtoMember(1)]
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        [ProtoMember(2)]
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
