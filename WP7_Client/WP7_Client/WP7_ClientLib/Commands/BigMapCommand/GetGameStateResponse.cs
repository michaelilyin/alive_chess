using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение состояния игры
    /// </summary>
    [ProtoContract]
    public class GetGameStateResponse : ICommand
    {
        public GetGameStateResponse()
        {
        }

        public GetGameStateResponse(King king, Castle castle, 
            List<Resource> resources)
        {
            this.King = king;
            this.Castle = castle;
            this.Resources = resources;
        }

        public Command Id
        {
            get { return Command.GetGameStateResponse; }
        }

        [ProtoMember(1)]
        public King King { get; set; }

        [ProtoMember(2)]
        public Castle Castle { get; set; }

        [ProtoMember(3)]
        public List<Resource> Resources { get; set; }
    }
}
