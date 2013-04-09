using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public interface IPosition<T> where T: struct, IComparable<T>
    {
        T X { get; set; }

        T Y { get; set; }
    }
}
