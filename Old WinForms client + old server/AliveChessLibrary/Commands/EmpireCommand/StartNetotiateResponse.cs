using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Abstract;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    [ProtoContract]
    public class StartNetotiateResponse : ICommand
    {
        [ProtoMember(1)]
        private Negotiate discussion;

        public Command Id
        {
            get { return Command.StartNegotiateResponse; }
        }

        public Negotiate Negotiate
        {
            get { return discussion; }
            set { discussion = value; }
        }
    }
}
