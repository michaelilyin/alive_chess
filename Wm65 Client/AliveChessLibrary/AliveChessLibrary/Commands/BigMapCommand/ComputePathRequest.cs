using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ComputePathRequest : ICommand
    {
        [ProtoMember(1)]
        private FPosition _start;
        [ProtoMember(2)]
        private FPosition _finish;

        public Command Id
        {
            get { return Command.ComputePathRequest; }
        }

        public FPosition Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public FPosition Finish
        {
            get { return _finish; }
            set { _finish = value; }
        }
    }
}
