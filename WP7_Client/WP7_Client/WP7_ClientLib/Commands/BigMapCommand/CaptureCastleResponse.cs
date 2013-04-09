using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// захват замка
    /// </summary>
    [ProtoContract]
    public class CaptureCastleResponse : ICommand
    {
        
        private Castle _castle;

        public CaptureCastleResponse()
        {
        }

        public CaptureCastleResponse(Castle castle)
        {
            this._castle = castle;
        }

        public Command Id
        {
            get { return Command.CaptureCastleResponse; }
        }

        [ProtoMember(1)]
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
