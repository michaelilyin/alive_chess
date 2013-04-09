using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class GetKingResponse : ICommand
    {
        [ProtoMember(1)]
        private King _king;

        public Command Id
        {
            get { return Command.GetKingResponse; }
        }

        public King King
        {
            get { return _king; }
            set { _king = value; }
        }
    }
}
