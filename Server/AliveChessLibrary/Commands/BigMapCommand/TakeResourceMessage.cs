using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// изъятие ресурса
    /// </summary>
    [ProtoContract]
    public class TakeResourceMessage : ICommand
    {
        [ProtoMember(1)]
        private Resource _resource;

        public TakeResourceMessage()
        {
        }

        public TakeResourceMessage(Resource resource)
        {
            this._resource = resource;
        }

        public Command Id
        {
            get { return Command.TakeResourceMessage; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public Resource Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }
    }
}
