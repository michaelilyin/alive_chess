using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class UpdateWorldMessage : ICommand
    {
        [ProtoMember(1)]
        private MapPoint mObject;

        public UpdateWorldMessage()
        {
        }

        public UpdateWorldMessage(MapPoint point)
        {
            this.mObject = point;
        }

        public Command Id
        {
            get { return Command.UpdateWorldMessage; }
        }

        public MapPoint MObject
        {
            get { return mObject; }
            set { mObject = value; }
        }
    }
}
