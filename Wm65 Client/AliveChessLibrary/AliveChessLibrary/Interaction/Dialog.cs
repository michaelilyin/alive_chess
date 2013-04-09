using System;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Interaction
{
    [ProtoContract]
    public class Dialog : IDispute
    {
        [ProtoMember(1)]
        private int id;

        [ProtoMember(2)]
        private King _respondent;

        [ProtoMember(3)]
        private bool yourStep;

        private King _organizator;
        private DialogState _state;
        private DialogTheme _theme;
        private TimeSpan _elapsed;

        public Dialog Clone()
        {
            return new Dialog
            {
                Id = Id,
                State = State,
                Theme = Theme,
                Elapsed = Elapsed,
                YouStep = YouStep,
                Respondent = Respondent,
                Organizator = Organizator,
            };
        }

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
            get { return yourStep; }
            set { yourStep = value; }
        }

        public DialogState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public DialogTheme Theme
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
            get { return InteractionType.Dialog; }
        }

    }
}
