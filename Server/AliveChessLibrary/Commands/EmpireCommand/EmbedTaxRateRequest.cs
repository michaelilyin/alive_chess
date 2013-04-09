using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// установка новго налога
    /// </summary>
    [ProtoContract]
    public class EmbedTaxRateRequest : ICommand
    {
        [ProtoMember(1)]
        private int _rate;

        public Command Id
        {
            get { return Command.EmbedTaxRateRequest; }
        }

        public int Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }
    }
}
