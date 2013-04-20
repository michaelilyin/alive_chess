using System.Collections.Generic;
using System.Linq;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.Environment.Aliances
{
    /// <summary>
    /// ящик для голосования
    /// </summary>
    public class BallotBox
    {
        private Union _union;
        private King _candidate;
        private List<Ballot> _ballots;
        private bool _isCandidateVictory;

        public BallotBox(Union union)
        {
            this._union = union;
            this._ballots = new List<Ballot>();
        }

        public void AddBallot(Ballot ballot)
        {
            _ballots.Add(ballot);
        }

        public void RemoveBallot(Ballot ballot)
        {
            _ballots.Remove(ballot);
        }

        public void Calculate()
        {
            int count = _ballots.Count<Ballot>(x => { return x.Yes; });
            if (count >= 5 * _union.Kings.Count / 100) this._isCandidateVictory = true;
        }

        public King Candidate
        {
            get { return _candidate; }
            set { _candidate = value; }
        }

        public bool IsCandidateVictory
        {
            get { return _isCandidateVictory; }
        }
    }
}
