using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetObjectsRequest : ICommand
    {
        [ProtoMember(1)]
        private int observerId;
        [ProtoMember(2)]
        private bool forConcreteObserver;
        [ProtoMember(3)]
        private PointTypes observerType;

        public Command Id
        {
            get { return Command.GetObjectsRequest; }
        }

        public int ObserverId
        {
            get { return observerId; }
            set { observerId = value; }
        }

        public bool ForConcreteObserver
        {
            get { return forConcreteObserver; }
            set { forConcreteObserver = value; }
        }

        public PointTypes ObserverType
        {
            get { return observerType; }
            set { observerType = value; }
        }
    }
}