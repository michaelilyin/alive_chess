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
        private int _objectId;
        private FPosition _location;
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

        [ProtoMember(1)]
        public int ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        [ProtoMember(2)]
        public FPosition Location
        {
            get { return _location; }
            set { _location = value; }
        }

        [ProtoMember(3)]
        public UpdateType UpdateType
        {
            get { return _updateType; }
            set { _updateType = value; }
        }
    }
}
