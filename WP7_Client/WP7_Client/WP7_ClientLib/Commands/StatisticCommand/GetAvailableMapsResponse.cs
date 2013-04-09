using System.Collections.Generic;
using ProtoBuf;

namespace AliveChessLibrary.Commands.StatisticCommand
{
    [ProtoContract]
    public class WorldDescription
    {
        [ProtoMember(1)]
        private string _mapName;

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }
    }

    [ProtoContract]
    public class GetAvailableMapsResponse : ICommand
    {
        [ProtoMember(1)]
        private List<WorldDescription> _worlds;

        public Command Id
        {
            get { return Command.GetAvailableMapsResponse; }
        }

        public List<WorldDescription> Worlds
        {
            get { return _worlds; }
            set { _worlds = value; }
        }
    }

}
