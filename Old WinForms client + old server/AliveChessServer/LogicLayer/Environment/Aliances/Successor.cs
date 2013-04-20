using System;
using System.Data.Linq.Mapping;
using AliveChessLibrary.Utility;

namespace AliveChessServer.LogicLayer.Environment.Aliances
{
    [Table(Name = "dbo.successor")]
    public class Successor
    {
        private int _successorId;
        private int _leaderId;
        private int _kingId;
        private int _queueNumber;

        [Column(Name = "successor_id", Storage = "_successorId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true, IsDbGenerated = true)]
        public int DbId
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

        [Column(Name = "leader_id", Storage = "_leaderId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true)]
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

        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = false, DbType = Constants.DB_INT,
            IsPrimaryKey = true)]
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

        [Column(Name = "queue_number", Storage = "_queueNumber", CanBeNull = false, DbType = "int")]
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
