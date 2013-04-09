using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    /// <summary>
    /// получение ресурса
    /// </summary>
    [ProtoContract]
    public class GetResourceMessage : ICommand
    {
        [ProtoMember(1)]
        private Resource _resource;
        [ProtoMember(2)]
        private bool _fromMine;

        public GetResourceMessage()
        {
        }

        public GetResourceMessage(Resource resource, bool fromMine)
        {
            this._fromMine = fromMine;
            this._resource = resource;
        }

        public Command Id
        {
            get { return Command.GetResourceMessage; }
        }

        /// <summary>
        /// Прото-атрибут: 1
        /// </summary>
        public Resource Resource
        {
            get { return _resource; }
            set { _resource = value; }
        }

        /// <summary>
        /// Прото-атрибут: 2
        /// </summary>
        public bool FromMine
        {
            get { return _fromMine; }
            set { _fromMine = value; }
        }
    }
}
