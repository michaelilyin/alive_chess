using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class VerifyPathResponse : ICommand
    {
        [ProtoMember(1)]
        private float _x;
        [ProtoMember(2)]
        private float _y;

        public Command Id
        {
            get { return Command.VerifyPathResponse; }
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
