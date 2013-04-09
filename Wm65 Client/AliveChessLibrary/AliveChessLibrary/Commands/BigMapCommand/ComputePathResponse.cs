using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Abstract;
using ProtoBuf;

namespace AliveChessLibrary.Commands.BigMapCommand
{
    [ProtoContract]
    public class ComputePathResponse : ICommand
    {
        [ProtoMember(1)]
        private List<FPosition> _path;

        public Command Id
        {
            get { return Command.ComputePathResponse; }
        }

        public List<FPosition> Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
