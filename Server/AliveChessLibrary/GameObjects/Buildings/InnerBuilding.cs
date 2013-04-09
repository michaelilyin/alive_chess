using System;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

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
    [ProtoContract, Serializable]
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
       
#if !UNITY_EDITOR
        private int _castleId;
        private EntityRef<Castle> _castle;
#else
        private Castle _castle;
#endif
        public InnerBuilding()
        {
#if !UNITY_EDITOR
            this._castle = default(EntityRef<Castle>);
#else
            this.Castle = null;
#endif
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

#if !UNITY_EDITOR
        //[Column(Name = "castle_id", Storage = "_castleId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int CastleId
        {
            get
            {
                return this._castleId;
            }
            set
            {
                if (this._castleId != value)
                {
                    if (this._castle.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._castleId = value;
                }
            }
        }
#endif
#if !UNITY_EDITOR
        //[Association(Name = "fk_inner_building_castle", Storage = "_castle", ThisKey = "CastleId",
        //    IsForeignKey = true)]
        public Castle Castle
        {
            get
            {
                return this._castle.Entity;
            }
            set
            {
                if (_castle.Entity != value)
                {
                    if (_castle.Entity != null)
                    {
                        var previousCastle = _castle.Entity;
                        _castle.Entity = null;
                        previousCastle.InnerBuildings.Remove(this);
                    }
                    _castle.Entity = value;
                    if (value != null)
                    {
                        _castleId = value.Id;
                    }
                }
            }
        }
#else
        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }
#endif
    }
}
