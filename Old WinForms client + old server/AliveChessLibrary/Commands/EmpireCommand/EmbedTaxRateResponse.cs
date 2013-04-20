using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// установление налога
    /// </summary>
    [ProtoContract]
    public class EmbedTaxRateResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public EmbedTaxRateResponse()
        {
        }

        public EmbedTaxRateResponse(bool successed)
        {
            this._successed = successed;
        }

        public Command Id
        {
            get { return Command.EmbedTaxRateResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
