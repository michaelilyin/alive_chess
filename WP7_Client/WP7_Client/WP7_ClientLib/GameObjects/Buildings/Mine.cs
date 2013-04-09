using System;
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
    public class Mine : IBuilding, IActive, IEquatable<int>, IEquatable<MapPoint>, IMultyPoint
    {
        #region Variables

        private const int DefaultSize = 1000;

        #endregion

        #region Constructors

        public Mine()
        {
            Distance = 3;
            Map = null;
            King = null;
            VisibleSpace = new VisibleSpace(this);

            if (OnLoad != null)
                OnLoad(this);
        }

        #endregion

        #region Initialization

        public void AddView(MapSector sector)
        {
            ViewOnMap = sector;
            sector.SetOwner(this);
        }

        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        public void Initialize(Map map, MapSector sector, ResourceTypes type,
                               int intensivityMining)
        {
            Initialize(map, type, intensivityMining);

            if (sector != null)
                AddView(sector);
        }

        public void Initialize(Map map, ResourceTypes typeRes,
                               int intensivityMining)
        {
            Map = map;
            Map.Id = map.Id;
            DateLastWorkMine = DateTime.Now;
            Active = false; 
            GainingResource = new Resource {ResourceType = typeRes};
            IntensityMiningMine = intensivityMining;
            SizeMine = DefaultSize;
        }

        public void Initialize(int id, Map map, ResourceTypes typeRes,
                               int size, int intensivityMining)
        {
            Id = id;
            DateLastWorkMine = DateTime.Now;
            Active = false; 
            SizeMine = size;
            Initialize(map, typeRes, intensivityMining);
        }

        public void Initialize(int id, Map map, ResourceTypes typeRes,
                               int size, int intensivityMining, ResourceStore vault)
        {
            ValutResurs = vault;
            Initialize(id, map, typeRes, size, intensivityMining);
        }

        #endregion

        #region Methods

        public void SetOwner(King king)
        {
            if (King != null && king != null)
                throw new AliveChessException("Owner isn't null");
            King = king;
            if (King != null) King.Id = king != null ? king.Id : -1;
        }

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0;
        }

        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0;
        }

        public void Activation()
        {
            if (!MineOverflow())
            {
                Active = true;
            }
            else
            {
                MessegerMine = "Шахта переполнена,заберите ресурсы и перезпаустите шахту";
            }
        }

        public void DoWork(DateTime tmpDateTime)
        {
            if (Active)
            {
                TimeSpan difference = tmpDateTime - DateLastWorkMine;
                var amountResource = (int) Math.Round(difference.TotalSeconds/IntensityMiningMine);
                if (amountResource > 0)
                {
                    СreateResource(amountResource);
                    if (GetResourceEvent != null)
                    {
                        GainingResource.CountResource = amountResource;
                        GetResourceEvent(King, GainingResource, true);
                    }
                    DateLastWorkMine = tmpDateTime;
                }
            }
        }

        public void Deactivation()
        {
            Active = false;
        }

        public void JoinVault(ResourceStore vault)
        {
            if (!PresenceValutResurs())
            {
                ValutResurs = vault;
                TranslationResource();
            }
            else
            {
                MessegerMine = "Шахта уже имеет одно хранилище ресурсов";
            }
        }

        public void DisconnectValut()
        {
            if (PresenceValutResurs())
            {
                ValutResurs = null;
            }
            else
            {
                MessegerMine = "У данной шахты нет своего хранилища ресурсов";
            }
        }

        public void СreateResource(int amountResource)
        {
            if (PresenceValutResurs())
            {
                var tmpRes = new Resource {ResourceType = GainingResource.ResourceType, CountResource = amountResource};
                ValutResurs.AddResourceToRepository(tmpRes);
            }
            else
            {
                GainingResource.CountResource += amountResource;
                if (MineOverflow())
                {
                    Deactivation();
                    MessegerMine = "Шахта переполнена и остановлена";
                }
            }
        }

        public int GetGainResourceCount()
        {
            if (PresenceValutResurs())
            {
                return -1;
            }
            return GainingResource.CountResource;
        }

        private bool MineOverflow()
        {
            return GainingResource.CountResource >= SizeMine;
        }

        private bool PresenceValutResurs()
        {
            return ValutResurs != null;
        }

        private void TranslationResource()
        {
            ValutResurs.AddResourceToRepository(GainingResource);
            GainingResource.CountResource = 0;
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

        public int ImageId { get; set; }

        [ProtoMember(6)]
        public float WayCost { get; set; }

        public PointTypes Type
        {
            get { return PointTypes.Mine; }
        }

        public int Distance { get; set; }

        public IPlayer Player
        {
            get
            {
                return King != null ? King.Player : null;
            }
        }

        public BuildingTypes BuildingType
        {
            get { return BuildingTypes.Mine; }
        }

        public string MessegerMine { get; set; }

        [ProtoMember(8)]
        public int SizeMine { get; set; }

        [ProtoMember(7)]
        public Resource GainingResource { get; set; }

        public ResourceStore ValutResurs { get; set; }

        public int IntensityMiningMine { get; set; }

        public bool Active { get; set; }

        public DateTime DateLastWorkMine { get; set; }

        public VisibleSpace VisibleSpace { get; set; }

        public MapSector ViewOnMap { get; set; }

        [ProtoMember(1)]
        public int Id { get; set; }

        public ResourceTypes MineType { get; set; }

        public Map Map { get; set; }

        public King King { get; set; }

        #endregion

        #region Delegates

        public delegate void GetResourceHandler(King player, Resource r, bool fromMine);

        #endregion

        #region Events

        public static event LoadingHandler<Mine> OnLoad;
        public event GetResourceHandler GetResourceEvent;

        #endregion
    }
}