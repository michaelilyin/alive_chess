using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// выгнать короля из империи
    /// </summary>
    [ProtoContract]
    public class ExcludeKingFromEmpireResponse : ICommand
    {
        [ProtoMember(1)]
        private bool _successed;

        public ExcludeKingFromEmpireResponse()
        {
        }

        public ExcludeKingFromEmpireResponse(bool successed)
        {
            this._successed = successed;
        }

        public Command Id
        {
            get { return Command.ExcludeKingFromEmpireResponse; }
        }

        public bool Successed
        {
            get { return _successed; }
            set { _successed = value; }
        }
    }
}
