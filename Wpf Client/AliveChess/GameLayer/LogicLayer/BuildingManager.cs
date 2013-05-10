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

        public CreationRequirements GetCreationRequirements(InnerBuildingType type)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}
