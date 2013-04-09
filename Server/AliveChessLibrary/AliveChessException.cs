using System;

namespace AliveChessLibrary
{
    public class AliveChessException : ApplicationException
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
