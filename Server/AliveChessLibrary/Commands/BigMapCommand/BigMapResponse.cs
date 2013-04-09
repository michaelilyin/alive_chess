using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// возвращение на большую карту
    /// </summary>
    [ProtoContract]
    public class BigMapResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _isAllowed;

        public BigMapResponse()
        {
        }

        public BigMapResponse(bool isAllowed)
        {
            this._isAllowed = isAllowed;
        }

        /// <summary>
        /// Идентификатор:7
        /// </summary>
        public Command Id
        {
            get { return Command.BigMapResponse; }
        }

        /// <summary>
        /// Разрешение вернуться. Прото-атрибут: 1
        /// </summary>
        public bool IsAllowed
        {
            get { return _isAllowed; }
            set { _isAllowed = value; }
        }
    }
}
