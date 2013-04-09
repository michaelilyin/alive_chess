using ProtoBuf;

namespace AliveChessLibrary.Commands.StatisticCommand
{
    public enum WorldType
    {
        Local,
        Global
    }

    [ProtoContract]
    public class GetAvailableMapsRequest : ICommand
    {
        [ProtoMember(1)]
        private WorldType _worldType;

        public Command Id
        {
            get { return Command.GetAvailableMapsRequest; }
        }

        public WorldType WorldType
        {
            get { return _worldType; }
            set { _worldType = value; }
        }
    }
}
