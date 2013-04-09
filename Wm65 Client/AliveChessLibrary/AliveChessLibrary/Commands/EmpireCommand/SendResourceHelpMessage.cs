using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class SendResourceHelpMessage : ICommand
    {
        [ProtoMember(1)]
        private int _receiverId;
        [ProtoMember(2)]
        private List<Resource> _resources;

        public SendResourceHelpMessage()
        {
        }

        public SendResourceHelpMessage(List<Resource> resources)
        {
            this._resources = resources;
        }

        public Command Id
        {
            get { return Command.SendResourceHelpMessage; }
        }

        public void AddResource(Resource r)
        {
            if (_resources == null)
                _resources = new List<Resource>();

            int index = -1;
            if ((index = _resources.FindIndex<Resource>(x =>
                x.ResourceType == r.ResourceType)) < 0)
                _resources.Add(r);
            else _resources[index].CountResource += r.CountResource;
        }

        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public int ReceiverId
        {
            get { return _receiverId; }
            set { _receiverId = value; }
        }
    }
}
