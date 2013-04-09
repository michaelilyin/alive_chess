using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// возвращение на большую карту
    /// </summary>
    [ProtoContract]
    public class BigMapResponse : ICommand
    {
        public BigMapResponse()
        {
        }

        public BigMapResponse(bool isAllowed)
        {
            IsAllowed = isAllowed;
        }

        [ProtoMember(1)]
        public bool IsAllowed { get; set; }

        #region ICommand Members

        public Command Id
        {
            get { return Command.BigMapResponse; }
        }

        #endregion
    }
}