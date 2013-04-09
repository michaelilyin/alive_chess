using AliveChessLibrary.Interfaces;
using ProtoBuf;
using System.Collections.Generic;

namespace AliveChessLibrary.Commands.ChatCommand
{
    [ProtoContract]
    public class SendContactListCommand : ICommand
    {
        [ProtoMember(1)]
        private List<int> ids;
        [ProtoMember(2)]
        private List<string> logins;
        [ProtoMember(3)]
        private byte channeltype;
        public Command Id
        {
            get { return Command.SendContactListCommand; }
        }
        public List<int> Ids
        {
            get { return ids;}
            set { ids = value; }
        }
        public byte ChannelType
        {
            get { return channeltype; }
        }
        public List<string> Logins
        {
            get { return logins; }
            set { logins = value; }
        }
    }
}
