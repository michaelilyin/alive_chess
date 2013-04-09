using System;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public interface ILocation : IPosition<int>
    {
        float WayCost { get; set; }
    }
}
