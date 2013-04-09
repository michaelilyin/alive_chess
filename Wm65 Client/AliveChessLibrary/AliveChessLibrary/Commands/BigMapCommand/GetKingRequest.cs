using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetKingRequest : ICommand
    {
        [ProtoMember(1)]
        private int _kingId;

        public Command Id
        {
            get { return Command.GetKingRequest; }
        }

        public int KingId
        {
            get { return _kingId; }
            set { _kingId = value; }
        }
    }
}
