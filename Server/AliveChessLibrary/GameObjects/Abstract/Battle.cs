using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    [ProtoContract]
    public class Battle : IInteraction
    {
        [ProtoMember(1)]
        private uint id;

        [ProtoMember(2)]
        private King _respondent;

        [ProtoMember(3)]
        private bool youStep;

        [ProtoMember(4)]
        private List<Unit> _playerArmy;

        [ProtoMember(5)]
        private List<Unit> _opponentArmy;

        private King _organizator;
        private TimeSpan _elapsed;

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public King Organizator
        {
            get { return _organizator; }
            set { _organizator = value; }
        }

        public King Respondent
        {
            get { return _respondent; }
            set { _respondent = value; }
        }

        public bool YouStep
        {
            get { return youStep; }
            set { youStep = value; }
        }

        public TimeSpan Elapsed
        {
            get { return _elapsed; }
            set { _elapsed = value; }
        }

        public List<Unit> PlayerArmy
        {
            get { return _playerArmy; }
            set { _playerArmy = value; }
        }

        public List<Unit> OpponentArmy
        {
            get { return _opponentArmy; }
            set { _opponentArmy = value; }
        }
    }
}
