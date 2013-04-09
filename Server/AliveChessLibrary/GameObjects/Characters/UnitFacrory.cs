using System;

namespace AliveChessLibrary.GameObjects.Characters
{
    public class UnitFacrory
    {
        public Unit Create(Guid guid, int id, int count, UnitType type)
        {
            Unit unit = new Unit();
            unit.UnitCount = count;
            unit.UnitType = type;
            return unit;
        }
    }
}
