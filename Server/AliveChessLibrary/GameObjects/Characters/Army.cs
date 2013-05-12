using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChessLibrary.GameObjects.Characters
{
    public class Army
    {
        private Dictionary<UnitType, int> _units = new Dictionary<UnitType, int>();

        /// <summary>
        /// список фигур
        /// </summary>
        public Dictionary<UnitType, int> GetUnitListCopy()
        {
            Dictionary<UnitType, int> result = new Dictionary<UnitType, int>();
            lock (_units)
            {
                foreach (var item in _units)
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        public void AddUnit(UnitType type, int quantity)
        {
            lock (_units)
            {
                if (_units.ContainsKey(type))
                {
                    _units[type] += quantity;
                }
                else
                {
                    _units.Add(type, quantity);
                }
            }
        }

        public void RemoveUnit(UnitType type, int quantity)
        {
            lock (_units)
            {
                if (_units.ContainsKey(type))
                {
                    _units[type] -= quantity;
                }
            }
        }

        public int GetUnitQuantity(UnitType type)
        {
            lock (_units)
            {
                if (_units.ContainsKey(type))
                {
                    return _units[type];
                }
            }
            return 0;
        }

        public bool HasUnits(UnitType type, int quantity)
        {
            lock (_units)
            {
                if (_units.ContainsKey(type))
                {
                    return _units[type] >= quantity;
                }
            }
            return false;
        }

        public bool HasUnits(UnitType type)
        {
            lock (_units)
            {
                return (_units.ContainsKey(type));
            }
        }

        public int GetStrength()
        {
            lock (_units)
            {
                return _units.Sum(item => item.Value);
            }
        }

        public void SetUnits(Dictionary<UnitType, int> units)
        {
            lock (_units)
            {
                _units = units;
            }
        }
    }
}
