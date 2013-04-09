using ProtoBuf;

namespace AliveChessLibrary.Commands.StatisticCommand
{
    [ProtoContract]
    public class GetStatisticRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetStatisticRequest; }
        }
    }
}
