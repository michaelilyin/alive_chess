using AliveChessLibrary.Interfaces;
using ProtoBuf;
using System.Collections.Generic;

namespace AliveChessLibrary.Commands.ChatCommand
{
    [ProtoContract]
    public class SendContactListCommand : ICommand
    {
        public Command Id
        {
            get { return Command.SendContactListCommand; }
        }

        [ProtoMember(1)]
        public List<int> Ids { get; set; }

        [ProtoMember(3)]
        public byte ChannelType { get; set; }

        [ProtoMember(2)]
        public List<string> Logins { get; set; }
    }
}
