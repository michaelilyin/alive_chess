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
        private Dictionary<InnerBuildingType, CreationRequirements> _buildingRequirements = new Dictionary<InnerBuildingType, CreationRequirements>();
        private Dictionary<UnitType, CreationRequirements> _recruitingRequirements = new Dictionary<UnitType, CreationRequirements>();

        public Dictionary<InnerBuildingType, CreationRequirements> BuildingRequirements
        {
            get { return _buildingRequirements; }
            set { _buildingRequirements = value; }
        }

        public Dictionary<UnitType, CreationRequirements> RecruitingRequirements
        {
            get { return _recruitingRequirements; }
            set { _recruitingRequirements = value; }
        }

        public CreationRequirements GetCreationRequirements(InnerBuildingType type)
        {
            return _buildingRequirements[type];
        }

        public CreationRequirements GetCreationRequirements(UnitType type)
        {
            return _recruitingRequirements[type];
        }

        public void SetCreationRequirements(InnerBuildingType type, CreationRequirements requirements)
        {
            _buildingRequirements[type] = requirements;
        }

        public void SetCreationRequirements(UnitType type, CreationRequirements requirements)
        {
            _recruitingRequirements[type] = requirements;
        }
    }
}
