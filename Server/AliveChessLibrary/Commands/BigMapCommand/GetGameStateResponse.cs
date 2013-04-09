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
        [ProtoMember(1)]
        private King _king;
        [ProtoMember(2)]
        private Castle _castle;
        [ProtoMember(3)]
        private List<Resource> _resources;

        public GetGameStateResponse()
        {
        }

        public GetGameStateResponse(King king, Castle castle, 
            List<Resource> resources)
        {
            this._king = king;
            this._castle = castle;
            this._resources = resources;
        }

        public Command Id
        {
            get { return Command.GetGameStateResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        /// <summary>
        /// Прото-атрибут: 3
        /// </summary>
        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
    }
}
