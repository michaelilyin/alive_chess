using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using ProtoBuf;

namespace AliveChessLibrary.Commands.CastleCommand
{
    [ProtoContract]
    public class GetCreationRequirementsResponse : ICommand
    {
        [ProtoMember(1)]
        private Dictionary<InnerBuildingType, CreationRequirements> _buildingRequirements;
        [ProtoMember(2)]
        private Dictionary<UnitType, CreationRequirements> _recruitingRequirements;
        
        public Command Id
        {
            get { return Command.GetCreationRequirementsResponse; }
        }

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
    }
}
