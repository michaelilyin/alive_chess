using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// попросить ресурсы у союзников
    /// </summary>
    [ProtoContract]
    public class GetHelpResourceRequest : ICommand
    {
        [ProtoMember(1)]
        private int _resourceCount;
        [ProtoMember(2)]
        private ResourceTypes _resourceType;

        public Command Id
        {
            get { return Command.GetHelpResourceRequest; }
        }

        public int ResourceCount
        {
            get { return _resourceCount; }
            set { _resourceCount = value; }
        }

        public ResourceTypes ResourceType
        {
            get { return _resourceType; }
            set { _resourceType = value; }
        }
    }
}
