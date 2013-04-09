using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class CaptureMineResponse : ICommand
    {
        [ProtoMember(1)]
        private Mine mine;

        public CaptureMineResponse()
        {
        }

        public CaptureMineResponse(Mine mine)
        {
            this.mine = mine;
        }

        public Command Id
        {
            get { return Command.CaptureMineResponse; }
        }

        public Mine Mine
        {
            get { return mine; }
            set { mine = value; }
        }
    }
}
