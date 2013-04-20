using System;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    [ProtoContract]
    public class Position : IEquatable<ILocation>
    {
        [ProtoMember(1)]
        private int x;
        [ProtoMember(2)]
        private int y;

        public Position()
        {
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(ILocation other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
