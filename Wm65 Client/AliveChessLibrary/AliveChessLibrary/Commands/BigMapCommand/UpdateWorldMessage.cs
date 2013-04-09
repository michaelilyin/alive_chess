using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.Interfaces;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
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

        public int ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        public FPosition Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public UpdateType UpdateType
        {
            get { return _updateType; }
            set { _updateType = value; }
        }
    }
}
