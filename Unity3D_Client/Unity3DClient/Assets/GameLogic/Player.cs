using AliveChessLibrary.GameObjects.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameLogic
{
    public class Player
    {
        private King _king;
        public King King
        {
            get { return _king; }
            set { _king = value; }
        }
    }
}
