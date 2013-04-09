using AliveChessLibrary.GameObjects.Buildings;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// захват шахты
    /// </summary>
    [ProtoContract]
    public class CaptureMineResponse : ICommand
    {
        private Mine _mine;

        public CaptureMineResponse()
        {
        }

        public CaptureMineResponse(Mine mine)
        {
            this._mine = mine;
        }

        public Command Id
        {
            get { return Command.CaptureMineResponse; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        [ProtoMember(1)]
        public Mine Mine
        {
            get { return _mine; }
            set { _mine = value; }
        }
    }
}
