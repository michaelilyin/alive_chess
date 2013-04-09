using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;

namespace AliveChessLibrary.GameObjects.Buildings
{
    [ProtoContract]
    public class Castle : IBuilding, IEquatable<int>, IEquatable<MapPoint>, IMultyPoint
    {
        #region Variables

        private int? _kingId;

        private InnerBuilding _recruitmentOffice;

        #endregion

        #region Constructors

        public Castle()
        {
            Distance = 5;
            Map = null;
            King = null;
            Vicegerent = null;
            FigureStore = null;
            ResourceStore = null;
            InnerBuildings = new List<InnerBuilding>();
            VisibleSpace = new VisibleSpace(this);

            if (OnLoad != null)
                OnLoad(this);
        }

        public Castle(int id, Map map)
            : this()
        {
            Initialize(id, map);
        }

        #endregion

        #region Initialization
       
        public void Initialize(Map map)
        {
            Map = map;
            _recruitmentOffice = new InnerBuilding
                                     {
                                         ProducedUnitType = UnitType.Pawn,
                                         InnerBuildingType = InnerBuildingType.Voencomat,
                                         Name = "Recruitment office"
                                     };
            InnerBuildings.Add(_recruitmentOffice);
        }

        public void Initialize(int id, Map map)
        {
            Initialize(map);
            Id = id;
        }

        public void Initialize(Map map, MapSector sector)
        {
            Initialize(map);

            if (sector != null)
                AddView(sector);
        }

        public void AddView(MapSector sector)
        {
            ViewOnMap = sector;
            sector.SetOwner(this);
        }

        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        #endregion

        #region Methods

        public void SetOwner(King king)
        {
            if (King != null && king != null)
                throw new AliveChessException("Owner isn't null");
            King = king;
            _kingId = king != null ? king.Id : -1;
        }

        public bool IsBelongTo(King king)
        {
            return King == king;
        }

      
        public int SizeListbuilldingsInCastle()
        {
            return InnerBuildings.Count;
        }

        public void CreateUnitAndAddInArmy(int count, UnitType type)
        {
            foreach (var t in InnerBuildings)
            {
                if (t.ProducedUnitType == type)
                {
                    AddInArmy(t.CreateUnit(count, type));
                }
            }
        }

        private void AddInArmy(Unit un)
        {
            var t = false;
            foreach (var t1 in FigureStore.Units)
            {
                if (t1.UnitType != un.UnitType) continue;
                t1.UnitCount += un.UnitCount;
                t = true;
                break;
            }
            if (!t) FigureStore.Units.Add(un);
        }

        public bool TestRes(int[] f)
        {
            if (f[0] > Vicegerent.Castle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Coal))
                return false;
            if (f[1] > Vicegerent.Castle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Gold))
                return false;
            if (f[2] > Vicegerent.Castle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Iron))
                return false;
            if (f[3] > Vicegerent.Castle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Stone))
                return false;
            if (f[4] > Vicegerent.Castle.ResourceStore.GetResourceCountInRepository(ResourceTypes.Wood))
                return false;
            return true;
        }

        public void GetArmyToKing()
        {
            bool t = false;
            foreach (var t1 in FigureStore.Units)
            {
                foreach (var t2 in King.Units)
                {
                    if (t1.UnitType != t2.UnitType) continue;
                    t2.UnitCount += t1.UnitCount;
                    t = true;
                    break;
                }
                if (t) continue;
                King.Units.Add(t1);
            }
            FigureStore.Units.Clear();
        }
       
        public InnerBuilding GetBuildings(int i)
        {
            return InnerBuildings[i];
        }

        public void AddBuildings(InnerBuildingType type)
        {
            //_innerBuildings.Add(Fabric.Build(pair.Guid, pair.Id, type, type.ToString(), _gameData));
        }

        //public List<Unit> ArmyInsideCastle
        //{
        //    get { return FigureStore.Units.ToList(); }
        //}

        public InnerBuilding RecruitmentOffice
        {
            get { return _recruitmentOffice; }
        }

        public void CreatStartArmy()
        {
          
        }

        public void AddUnit(Unit un, IList<Unit> arm)
        {
            bool ok = true;
            for (int i = 0; i < FigureStore.Units.Count; i++)
            {
                if (un.Id == arm[i].Id)
                {
                    arm[i].UnitCount++;
                    ok = false;
                    break;
                }

            }
            if (ok) arm.Add(un);
        }

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0;
        }

        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0;
        }

        #endregion

        #region Properties

        [ProtoMember(2)]
        public int X { get; set; }

        [ProtoMember(3)]
        public int Y { get; set; }

        [ProtoMember(4)]
        public int Width { get; set; }

        [ProtoMember(5)]
        public int Height { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.Castle; }
        }

        public int ImageId { get; set; }

        [ProtoMember(6)]
        public float WayCost { get; set; }

        public bool IsFree
        {
            get { return !_kingId.HasValue; }
        }

        public IPlayer Player
        {
            get 
            {
                return King != null ? King.Player : null;                   
            }
        }

        public bool KingInside { get; set; }

        public int Distance { get; set; }

        public VisibleSpace VisibleSpace { get; set; }

        public BuildingTypes BuildingType
        {
            get { return BuildingTypes.Castle; }
        }

        public MapSector ViewOnMap { get; set; }

        public bool IsAttached { get; set; }

        public Vicegerent Vicegerent { get; set; }

        public ResourceStore ResourceStore { get; set; }

        public FigureStore FigureStore { get; set; }

        [ProtoMember(1)]
        public int Id { get; set; }

        public King King { get; set; }

        public Map Map { get; set; }

        public List<InnerBuilding> InnerBuildings { get; set; }

        #endregion

        public static event LoadingHandler<Castle> OnLoad;
    }
}
