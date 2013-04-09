using System;

namespace AliveChessLibrary
{
    public class AliveChessException : Exception
    {
        public AliveChessException()
            : base()
        {
        }

        public AliveChessException(string message)
            : base(message)
        {
        }
    }
}
