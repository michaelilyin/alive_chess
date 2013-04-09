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
        [ProtoMember(1)]
        private Castle _castle;

        public CaptureCastleResponse()
        {
        }

        public CaptureCastleResponse(Castle castle)
        {
            this._castle = castle;
        }

        /// <summary>
        /// Идентификатор:7
        /// </summary>
        public Command Id
        {
            get { return Command.CaptureCastleResponse; }
        }

        /// <summary>
        /// Замок. Прото-атрибут: 1
        /// </summary>
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
