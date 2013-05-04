using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChessServer.LogicLayer.EconomyEngine
{
    public class Economy
    {
        private Dictionary<InnerBuildingType, CreationCost> _buildingCosts = new Dictionary<InnerBuildingType, CreationCost>();
        private Dictionary<UnitType, CreationCost> _unitCosts = new Dictionary<UnitType, CreationCost>();
        private Dictionary<InnerBuildingType, List<InnerBuildingType>> _requiredForBuilding = new Dictionary<InnerBuildingType, List<InnerBuildingType>>();
        private Dictionary<UnitType, List<InnerBuildingType>> _requiredForUnit = new Dictionary<UnitType, List<InnerBuildingType>>();

        public CreationCost GetBuildingCost(InnerBuildingType type)
        {
            return _buildingCosts[type];
        }

        public CreationCost GetUnitCost(UnitType type)
        {
            return _unitCosts[type];
        }

        public List<InnerBuildingType> GetRequiredForBuilding(InnerBuildingType type)
        {
            return _requiredForBuilding[type];
        }

        public List<InnerBuildingType> GetRequiredForUnit(UnitType type)
        {
            return _requiredForUnit[type];
        }
    }
}
