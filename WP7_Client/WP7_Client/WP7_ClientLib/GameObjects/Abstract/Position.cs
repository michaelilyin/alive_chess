using System;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    [ProtoContract]
    public class Position : IPosition<int>, IEquatable<ILocation>
    {
        public Position()
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(ILocation other)
        {
            return X == other.X && Y == other.Y;
        }

        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }

        public override string ToString()
        {
            return String.Concat(X, ":", Y);
        }
    }
}
