using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.Commands.CastleCommand
{
    public class ShowArmyKingResponse : ICommand
    {
        [ProtoMember(1)]
        private List<Unit> army_list;

        public List<Unit> Army_list
        {
            get { return army_list; }
            set { army_list = value; }
        }

        public Command Id
        {
            get { return Command.ShowArmyKingResponse; }
        }
    }
}
