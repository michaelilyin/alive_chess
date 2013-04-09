using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    /// <summary>
    /// бюллетень
    /// </summary>
    public struct Ballot
    {
        private bool _yes;
        private King _from;

        /// <summary>
        /// поддержжка кандидата
        /// </summary>
        public bool Yes
        {
            get { return _yes; }
            set { _yes = value; }
        }

        /// <summary>
        /// голосующий участник союза
        /// </summary>
        public King From
        {
            get { return _from; }
            set { _from = value; }
        }
    }
}
