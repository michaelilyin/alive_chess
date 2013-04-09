using System;

namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// позиция
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPosition<T> where T: struct, IComparable<T>
    {
        T X { get; set; }

        T Y { get; set; }
    }
}
