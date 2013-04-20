using System;

namespace AliveChessServer.LogicLayer
{
    public class AliveChessException : ApplicationException
    {
        public AliveChessException(string message)
            : base(message)
        {
        }
    }
}
