using System;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// позиция
    /// </summary>
    [ProtoContract]
    public class Position : IPosition<int>, IEquatable<ILocation>
    {
        [ProtoMember(1)]
        private int _x;
        [ProtoMember(2)]
        private int _y;

        public Position()
        {
        }

        public Position(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public bool Equals(ILocation other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public override string ToString()
        {
            return String.Concat(X, ":", Y);
        }
    }
}
