using System;

namespace AliveChessLibrary.Utility
{
    public struct GuidIDPair
    {
        private int _id;
        private Guid _guid;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
    }
}
