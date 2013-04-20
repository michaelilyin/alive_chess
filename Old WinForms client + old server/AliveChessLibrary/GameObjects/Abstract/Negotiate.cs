using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.Interfaces;
using ProtoBuf;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessLibrary.GameObjects.Abstract
{
    [ProtoContract]
    public class Negotiate : IInteraction
    {
        [ProtoMember(1)]
        private uint id;

        [ProtoMember(2)]
        private King _respondent;

        [ProtoMember(3)]
        private bool youStep;

        private King _organizator;
        private DialogState _state;
        private NegotiateTheme _theme;
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

        public DialogState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public NegotiateTheme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        public TimeSpan Elapsed
        {
            get { return _elapsed; }
            set { _elapsed = value; }
        }
    }
}
