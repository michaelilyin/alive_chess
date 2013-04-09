using System;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Interaction
{
    [ProtoContract]
    public class Negotiate : IDispute
    {
        [ProtoMember(1)]
        private int id;

        [ProtoMember(2)]
        private King _respondent;

        [ProtoMember(3)]
        private bool youStep;

        private King _organizator;
        private DialogState _state;
        private NegotiateTheme _theme;
        private TimeSpan _elapsed;

        public int Id
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

        public InteractionType InteractionType
        {
            get { return InteractionType.Negotiate; }
        }
    }
}
