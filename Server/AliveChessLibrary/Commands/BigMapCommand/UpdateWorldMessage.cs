using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// обновление объекта в области видимости
    /// </summary>
    [ProtoContract]
    public class UpdateWorldMessage : ICommand
    {
        [ProtoMember(1)]
        private int _objectId;
        [ProtoMember(2)]
        private FPosition _location;
        [ProtoMember(3)]
        private UpdateType _updateType;

        public UpdateWorldMessage()
        {
        }

        public UpdateWorldMessage(IMapObject sender, FPosition position, UpdateType updateType)
        {
            this._objectId = sender.Id;
            this._location = position;
            this._updateType = updateType;
        }

        public Command Id
        {
            get { return Command.UpdateWorldMessage; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public FPosition Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// Прото-атрибут: 3
        /// </summary>
        public UpdateType UpdateType
        {
            get { return _updateType; }
            set { _updateType = value; }
        }
    }
}
