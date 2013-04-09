using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetGameStateResponse : ICommand
    {
        [ProtoMember(1)]
        private King king;
        [ProtoMember(2)]
        private Castle castle;
        [ProtoMember(3)]
        private List<Resource> startResources;

        public GetGameStateResponse()
        {
        }

        public GetGameStateResponse(King king, Castle castle, List<Resource> resources)
        {
            this.king = king;
            this.castle = castle;
            this.startResources = resources;
        }

        public Command Id
        {
            get { return Command.GetGameStateResponse; }
        }

        public King King
        {
            get { return king; }
            set { king = value; }
        }

        public Castle Castle
        {
            get { return castle; }
            set { castle = value; }
        }

        public List<Resource> StartResources
        {
            get { return startResources; }
            set { startResources = value; }
        }
    }
}
