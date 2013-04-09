using System;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// позиция в вещественных координатах
    /// </summary>
    [ProtoContract]
    public class FPosition : IPosition<float>
    {
        [ProtoMember(1)]
        private float _x;
        [ProtoMember(2)]
        private float _y;

        public FPosition()
        {
        }

        public FPosition(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
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
