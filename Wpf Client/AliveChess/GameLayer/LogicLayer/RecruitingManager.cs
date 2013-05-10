using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;

namespace AliveChess.GameLayer.LogicLayer
{
    public class RecruitingManager : IRecruitingManager
    {
        private Castle _castle;
        private LinkedList<BuildingQueueItem<InnerBuildingType>> _buildingQueue = new LinkedList<BuildingQueueItem<InnerBuildingType>>();
        private Dictionary<UnitType, CreationRequirements> _creationRequirements = new Dictionary<UnitType, CreationRequirements>();

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public Dictionary<UnitType, CreationRequirements> CreationRequirements
        {
            get { return _creationRequirements; }
            set { _creationRequirements = value; }
        }
    }
}
