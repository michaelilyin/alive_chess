using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// попросить ресурсы у союзников
    /// </summary>
    [ProtoContract]
    public class GetHelpResourceResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Resource> _resources;

        public GetHelpResourceResponse()
        {
        }

        public GetHelpResourceResponse(List<Resource> resources)
        {
            this._resources = resources;
        }

        public Command Id
        {
            get { return Command.GetHelpResourceResponse; }
        }

        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
    }
}
