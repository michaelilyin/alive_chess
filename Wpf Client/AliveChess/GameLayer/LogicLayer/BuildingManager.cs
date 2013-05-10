using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChess.GameLayer.LogicLayer
{
    public class BuildingManager : IBuildingManager
    {
        private Castle _castle;
        private LinkedList<BuildingQueueItem<InnerBuildingType>> _buildingQueue = new LinkedList<BuildingQueueItem<InnerBuildingType>>();
        private Dictionary<InnerBuildingType, CreationRequirements> _creationRequirements = new Dictionary<InnerBuildingType, CreationRequirements>();

        public CreationRequirements GetCreationRequirements(InnerBuildingType type)
        {
            foreach (var creationRequirements in _creationRequirements)
            {
                if (creationRequirements.Key == type)
                    return creationRequirements.Value;
            }
            return null;
        }

        public void Update(TimeSpan timeFromLastUpdate)
        {
            throw new NotImplementedException();
        }

        public void Build(InnerBuildingType type)
        {
            throw new NotImplementedException();
        }

        public void Destroy(InnerBuildingType type)
        {
            throw new NotImplementedException();
        }

        public bool HasUnfinishedBuilding(InnerBuildingType type)
        {
            return BuildingQueue.Any(buildingQueueItem => buildingQueueItem.Type == type);
        }

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        public LinkedList<BuildingQueueItem<InnerBuildingType>> BuildingQueue
        {
            get { return _buildingQueue; }
            set { _buildingQueue = value; }
        }

        public Dictionary<InnerBuildingType, CreationRequirements> CreationRequirements
        {
            get { return _creationRequirements; }
            set { _creationRequirements = value; }
        }
    }
}
