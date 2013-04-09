using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// запрос объектов в области видимости
    /// </summary>
    [ProtoContract]
    public class GetObjectsRequest : ICommand
    {
        [ProtoMember(1)]
        private int _observerId;
        [ProtoMember(2)]
        private bool _forConcreteObserver;
        [ProtoMember(3)]
        private PointTypes _observerType;

        public Command Id
        {
            get { return Command.GetObjectsRequest; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public int ObserverId
        {
            get { return _observerId; }
            set { _observerId = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public bool ForConcreteObserver
        {
            get { return _forConcreteObserver; }
            set { _forConcreteObserver = value; }
        }

        /// <summary>
        /// Прото-атрибут: 3
        /// </summary>
        public PointTypes ObserverType
        {
            get { return _observerType; }
            set { _observerType = value; }
        }
    }
}