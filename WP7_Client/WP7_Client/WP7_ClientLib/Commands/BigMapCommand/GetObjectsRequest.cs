using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetObjectsRequest : ICommand
    {
        public Command Id
        {
            get { return Command.GetObjectsRequest; }
        }

        [ProtoMember(1)]
        public int ObserverId { get; set; }

        [ProtoMember(2)]
        public bool ForConcreteObserver { get; set; }

        [ProtoMember(3)]
        public PointTypes ObserverType { get; set; }
    }
}