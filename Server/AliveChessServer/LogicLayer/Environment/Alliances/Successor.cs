namespace AliveChessServer.LogicLayer.Environment.Alliances
{
    public class Successor
    {
        private int _successorId;
        private int _leaderId;
        private int _kingId;
        private int _queueNumber;

        public int Id
        {
            get
            {
                return this._successorId;
            }
            set
            {
                if (this._successorId != value)
                {
                    this._successorId = value;
                }
            }
        }

        public int LeaderId
        {
            get
            {
                return this._leaderId;
            }
            set
            {
                if (this._leaderId != value)
                {
                    this._leaderId = value;
                }
            }
        }

        public int KingId
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    this._kingId = value;
                }
            }
        }

        public int QueueNumber
        {
            get
            {
                return this._queueNumber;
            }
            set
            {
                if (this._queueNumber != value)
                {
                    this._queueNumber = value;
                }
            }
        }
    }
}
