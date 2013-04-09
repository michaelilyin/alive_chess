using AliveChessLibrary.Statistics;
using ProtoBuf;

namespace AliveChessLibrary.Commands.StatisticCommand
{
    [ProtoContract]
    public class GetStatisticResponse : ICommand
    {
        [ProtoMember(1)]
        private Statistic _statistic;

        public Command Id
        {
            get { return Command.GetStatisticResponse; }
        }

        public Statistic Statistic
        {
            get { return _statistic; }
            set { _statistic = value; }
        }
    }
}
