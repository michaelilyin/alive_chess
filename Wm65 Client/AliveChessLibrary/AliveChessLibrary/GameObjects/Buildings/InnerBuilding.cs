using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    public enum InnerBuildingType
    {
        Voencomat = 0, //для создания пешек
        Stable = 1, // для создания коней
        SchoolOfficers = 2, // для создание офицеров
        VVU = 3, // для создания лодьи
        GeneralStaff = 4  // для создания королев
    }

    /// <summary>
    /// внутреннее здание
    /// </summary>
    [ProtoContract]
    public class InnerBuilding : IInnerBuilding
    {
        [ProtoMember(1)]
        private int _innerBuildingId;
        [ProtoMember(7)]
        private string _name;
        [ProtoMember(2)]
        private int _resourceCountToBuild;
        [ProtoMember(3)]
        private int _resourceCountToProduceUnit;
        [ProtoMember(4)]
        private ResourceTypes _resourceTypeToBuild;
        [ProtoMember(5)]
        private ResourceTypes _resourceTypeToProduceUnit;
        [ProtoMember(6)]
        private InnerBuildingType _innerBuildingType;

        private UnitType _unitType;
       
        private Castle _castle;

        public InnerBuilding()
        {
            this.Castle = null;
        }

        public Unit CreateUnit(int count, UnitType type)
        {
            Unit unit = new Unit();
            unit.UnitCount = count;
            unit.UnitType = type;
            return unit;
        }

        public UnitType ProducedUnitType
        {
            get { return _unitType; }
            set { _unitType = value; }
        }

        public int ResourceCountToBuild
        {
            get { return _resourceCountToBuild; }
            set { _resourceCountToBuild = value; }
        }

        public ResourceTypes ResourceTypeToBuild
        {
            get { return _resourceTypeToBuild; }
            set { _resourceTypeToBuild = value; }
        }

        public int ResourceCountToProduceUnit
        {
            get { return _resourceCountToProduceUnit; }
            set { _resourceCountToProduceUnit = value; }
        }

        public ResourceTypes ResourceTypeToProduceUnit
        {
            get { return _resourceTypeToProduceUnit; }
            set { _resourceTypeToProduceUnit = value; }
        }

        //[Column(Name = "inner_building_id", Storage = "_innerBuildingId", CanBeNull = false,
        //    DbType = Constants.DB_INT, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._innerBuildingId;
            }
            set
            {
                if (this._innerBuildingId != value)
                {
                    this._innerBuildingId = value;
                }
            }
        }

        //[Column(Name = "inner_building_name", Storage = "_name", CanBeNull = false,
        //  DbType = "VarChar(20)")]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                }
            }
        }

        //[Column(Name = "inner_building_type", Storage = "_innerBuildingType", CanBeNull = false, 
        //    DbType = Constants.DB_INT)]
        public InnerBuildingType InnerBuildingType
        {
            get
            {
                return this._innerBuildingType;
            }
            set
            {
                if (this._innerBuildingType != value)
                {
                    this._innerBuildingType = value;
                }
            }
        }
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
    }
}
