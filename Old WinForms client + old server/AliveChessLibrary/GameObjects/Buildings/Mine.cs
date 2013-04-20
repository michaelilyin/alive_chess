using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
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
    [Table(Name = "dbo.mine")]
    public class Mine : IBuilding, IActive, IEquatable<int>, IMultyPoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _mineId;

        private int _mapId;
        private int? _kingId;
        private int _mapSectorId;
        private VisibleSpace _sector;
        private ResourceTypes _mineType;

        [ProtoMember(2)]
        private MapSector _viewOnMap; // сектор на карте
        [ProtoMember(3)]
        private Resource _gainingResource; // тип ресурса добываемого в шахте
        [ProtoMember(4)]
        private int _sizeMine; // размер шахты

        private string _messegerMine; // сообщение от шахты
        private ResourceStore _valutResurs; // указатель на Хранилище ресурсов
        private int _intensityMiningMine; // интенсивность добычи ресурсов(количество ресурсов производимых за раз)
        private bool _active; // активна ли шахта
        private DateTime _dateLastWorkMine; // дата последенй работы шахты

        private EntityRef<Map> _map; // ссылка на карту
        private EntityRef<King> _king; // ссылка на короля

        private int _distance = 3;
        private GameData _gameData;

        private const int DEFAULT_SIZE = 1000;

        private const string _errorMessage = "View on map must be not null";
       
        #endregion

        private UpdateCheck updateCheck;
        public UpdateCheck UpdateCheck
        {
            get { return updateCheck; }
            set { updateCheck = value; }
        }

        #region Constructors

        public Mine()
        {
            this._map = default(EntityRef<Map>);
            this._king = default(EntityRef<King>);
        }

        #endregion

        #region Initialization

        public void AddView(MapSector sector)
        {
            this.ViewOnMap = sector;
            foreach (MapPoint mp in sector.MapPoints)
                _map.Entity.SetObject(mp);
        }

        // начальная инициализация
        public void Initialize(GameData context, Map map, ResourceTypes typeRes,
            int intensivityMining)
        {
            this.Map = map;
            this._dateLastWorkMine = DateTime.Now;
            this._active = false; // активировать шахту
            this._gameData = context;
            this._gainingResource = new Resource();
            this._gainingResource.ResourceType = typeRes;
            this._intensityMiningMine = intensivityMining;
            this._sizeMine = DEFAULT_SIZE;
        }

        // метод инициализации шахты. Испоьзовать конструктор нельзя потому что Protocol Buffer
        // позволяет передавать только классы с пустыми конструкторами
        // создает обычную шахту определенного типа
        public void Initialize(Guid id, GameData context, Map map, ResourceTypes typeRes,
            int size, int intensivityMining)
        {
           // this._mineId = id;
            this._dateLastWorkMine = DateTime.Now;
            this._active = false; // активировать шахту
            this._sizeMine = size;
            Initialize(context, map, typeRes, intensivityMining);
        }

        // создает шахту с привязанным Хранилищем ресурсов
        public void Initialize(Guid id, GameData context, Map map, ResourceTypes typeRes,
            int size, int intensivityMining, ResourceStore vault)
        {
            this._valutResurs = vault;
            Initialize(id, context, map, typeRes, size, intensivityMining);
        }

        [ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.MapSectorId = this.ViewOnMap.Id;
        }

        #endregion

        #region Methods

        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        public void UpdateVisibleSpace(VisibleSpace sector)
        {
            this._sector = sector;
        }

        /* Метод активизирует работу шахты */
        public void Activation()
        {
            // Если шахта не переполнена
            if (!this.MineOverflow())
            {
                //Установить флажок Activate в положение true
                this._active = true;
            }
            // Если шахта переполнена
            else
            {
                //Сформировать соответствующее сообщение от шахты
                this._messegerMine = "Шахта переполнена,заберите ресурсы и перезпаустите шахту";
            }

        }

        /* Метод, заставляюший работать шахту*/
        public void DoWork(DateTime tmpDateTime)
        {
            // если шахта находиться в рабочем состоянии
            if (this._active)
            {
                // найти разницу между временем посленей работы шахты и текущем временем
                TimeSpan difference = tmpDateTime - this._dateLastWorkMine;
                // разделить полученное значение на интенсивность работы шахты и округлить до ближайшего целого
                int amountResource = (int)Math.Round(difference.TotalSeconds / this._intensityMiningMine);
                // если полученое значение больше нуля
                if (amountResource > 0)
                {
                    //создать необходимое количество ресурса
                    this.СreateResource(amountResource);

                    // когда шахта создает ресурс то сообщение об этом отправляется
                    // игроку владеющему данной шахтой
                    if (GetResourceEvent != null)
                    {
                        _gainingResource.CountResource = amountResource;
                        GetResourceEvent(this._king.Entity, _gainingResource, true);
                    }

                    // сохранить новую дату работы шахты
                    this._dateLastWorkMine = tmpDateTime;
                }
            }
        }

        /* Метод останавливающий работу шахты*/
        public void Deactivation()
        {
            this._active = false;
        }

        /* Метод присоединяющий хранилище ресурсов к шахте.
       * На вход метод получает хранилище, которое необходимо присоединить.
       */
        public void JoinVault(ResourceStore vault)
        {
            // Если шахта еще не имеет хранилища ресурсв
            if (!this.PresenceValutResurs())
            {
                //присоединить хранилище к шахте
                this._valutResurs = vault;
                // произвести перевод ресурсов из шахты в Хранилище
                this.TranslationResource();

            }
            // Если шахта уже имеет хранилище 
            else
            {
                //сформировать соответствующие сообщение шахты
                this._messegerMine = "Шахта уже имеет одно хранилище ресурсов";
            }
        }

        /* Метод отсоединяет хранилище ресурсов от шахты*/
        public void DisconnectValut()
        {
            // Если у шахты есть Хранилище ресурсов
            if (this.PresenceValutResurs())
            {
                //отсоединить это хранилище
                this._valutResurs = null;
            }
            // Если хранилища у шахты нет
            else
            {
                //вернуть соответствующее сообщение
                this._messegerMine = "У данной шахты нет своего хранилища ресурсов";
            }
        }


        /* Метод позволяющий шахте создавать ресурсы */
        public void СreateResource(int amountResource)
        {
            Resource tmpRes = null;

            // Если имеется Хранилище ресурсов
            if (this.PresenceValutResurs())
            {
                //создать ресурс
                tmpRes = new Resource();
                tmpRes.ResourceType = _gainingResource.ResourceType;
                tmpRes.CountResource = amountResource;
                //передать в хранилище
                this._valutResurs.AddResourceToRepository(tmpRes);

            }
            // Если Хранилища ресурсов нет
            else
            {
                //Увеличить счетчик количества добываемого ресурса в Шахте
                this._gainingResource.CountResource += amountResource;
                //Если шахта переполнена
                if (this.MineOverflow())
                {
                    //отсановить работу шахты
                    this.Deactivation();
                    //сформировать соответствующее сообщение
                    this._messegerMine = "Шахта переполнена и остановлена";
                    // MineExeption mineExeption = new MineExeption(this.messegerMine);
                    // throw mineExeption;

                }
            }

        }

        /* Метод возвращает количество ресурса в шахте */
        public int GetGainResourceCount()
        {
            // Если у шахты есть Хранилище ресурсов
            if (this.PresenceValutResurs())
            {
                //ошибка, невозможно просмотреть количество ресурса, т.к ресурс передается в Хран рес
                return -1;

            }
            // Если Хранилища ресурсов у шахты нет
            else
            {
                //вернуть количество ресурса в шахте
                return this._gainingResource.CountResource;
            }
        }

        /* Метод проверяет переполнена ли шахта
         * Если шахта переполнена врзвращает True
         * В противном случае False
         */
        private bool MineOverflow()
        {
            // Если размер ресурса превышает максимальный размер шахты
            if (this._gainingResource.CountResource >= this._sizeMine)
                return true;
            else
                return false;
        }

        /* Метод проверяет наличие Хранилища ресурсов у шахты
         * Если Хранилище ресурсов присуиствует метод возвращает True
         * В противном случае метод возвращает False
         */
        private bool PresenceValutResurs()
        {
            // Проверить присутствие Хранилища ресурсов
            if (this._valutResurs != null)
                return true;
            else
                return false;
        }

        /* Метод переводит ресурсы из шахты в хранилище ресурсов */
        private void TranslationResource()
        {
            // передать ресурс из шахты в Хранилище 
            this._valutResurs.AddResourceToRepository(this._gainingResource);
            // обнулить количества ресурсов в шахте
            this._gainingResource.CountResource = 0;
        }

        #endregion

        #region Properties

        public int X
        {
            get 
            {
                if (_viewOnMap != null)
                    return _viewOnMap.X;
                else throw new NullReferenceException(_errorMessage);
            }
            set
            {
                if (_viewOnMap != null)
                    _viewOnMap.X = value;
                else throw new NullReferenceException(_errorMessage);
            }
        }

        public int Y
        {
            get
            {
                if (_viewOnMap != null)
                    return _viewOnMap.Y;
                else throw new NullReferenceException(_errorMessage);
            }
            set
            {
                if (_viewOnMap != null)
                    _viewOnMap.Y = value;
                else throw new NullReferenceException(_errorMessage);
            }
        }

        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public IPlayer Player
        {
            get
            {
                if (_king.Entity != null)
                    return _king.Entity.Player;
                else return null;
            }
        }

        public BuildingTypes BuildingType
        {
            get { return BuildingTypes.Mine; }
        }

        public string MessegerMine
        {
            get { return _messegerMine; }
            set { _messegerMine = value; }
        }

        public int SizeMine
        {
            get { return _sizeMine; }
            set { _sizeMine = value; }
        }

        public Resource GainingResource
        {
            get { return _gainingResource; }
            set { _gainingResource = value; }
        }

        public ResourceStore ValutResurs
        {
            get { return _valutResurs; }
            set { _valutResurs = value; }
        }

        public int IntensityMiningMine
        {
            get { return _intensityMiningMine; }
            set { _intensityMiningMine = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public GameData GameData
        {
            get { return _gameData; }
            set { _gameData = value; }
        }

        public DateTime DateLastWorkMine
        {
            get { return _dateLastWorkMine; }
            set { _dateLastWorkMine = value; }
        }

        public VisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value; }
        }

        /// <summary>
        /// сектор на карте
        /// </summary>
        public MapSector ViewOnMap
        {
            get
            {
                return this._viewOnMap;
            }
            set
            {
                if (_viewOnMap != value)
                {
                    if (value != null)
                    {
                        _viewOnMap = value;
                        _mapSectorId = _viewOnMap.Id;
                    }
                }
            }
        }

        /// <summary>
        /// идентификатор шахты (первичный ключ)
        /// </summary>
        [Column(Name = "mine_id", Storage = "_mineId", CanBeNull = false, DbType = Constants.DB_INT,
          IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._mineId;
            }
            set
            {
                if (this._mineId != value)
                {
                    this._mineId = value;
                }
            }
        }

        [Column(Name = "map_sector_id", Storage = "_mapSectorId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int MapSectorId
        {
            get
            {
                return this._mapSectorId;
            }
            set
            {
                if (this._mapSectorId != value)
                {
                    this._mapSectorId = value;
                }
            }
        }

        /// <summary>
        /// идентификатор карты (внешний ключ)
        /// </summary>
        [Column(Name = "map_id", Storage = "_mapId", CanBeNull = false, DbType = Constants.DB_INT)]
        public int MapId
        {
            get
            {
                return this._mapId;
            }
            set
            {
                if (this._mapId != value)
                {
                    if (this._map.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._mapId = value;
                }
            }
        }

        [Column(Name = "mine_type", Storage = "_mineType", CanBeNull = false, DbType = Constants.DB_INT)]
        public ResourceTypes MineType
        {
            get
            {
                return this._mineType;
            }
            set
            {
                if (this._mineType != value)
                {
                    this._mineType = value;
                }
            }
        }

        /// <summary>
        /// идентификатор короля (внешний ключ)
        /// </summary>
        [Column(Name = "king_id", Storage = "_kingId", CanBeNull = true, DbType = Constants.DB_INT)]
        public int? KingId
        {
            get
            {
                return this._kingId;
            }
            set
            {
                if (this._kingId != value)
                {
                    if (this._king.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this._kingId = value;
                }
            }
        }

        /// <summary>
        /// ссылка на короля
        /// </summary>
        [Association(Name = "fk_mine_king", Storage = "_king", ThisKey = "KingId", IsForeignKey = true)]
        public King King
        {
            get
            {
                return this._king.Entity;
            }
            set
            {
                if (_king.Entity != value)
                {
                    if (_king.Entity != null)
                    {
                        var previousKing = _king.Entity;
                        _king.Entity = null;
                        previousKing.Mines.Remove(this);
                    }
                    _king.Entity = value;
                    if (value != null)
                    {
                        value.Mines.Add(this);
                        _kingId = value.Id;
                    }
                    else
                    {
                        _kingId = null;
                    }
                }
            }
        }

        /// <summary>
        /// ссылка на карту
        /// </summary>
        [Association(Name = "fk_mine_map", Storage = "_map", ThisKey = "MapId", IsForeignKey = true)]
        public Map Map
        {
            get
            {
                return this._map.Entity;
            }
            set
            {
                if (_map.Entity != value)
                {
                    if (_map.Entity != null)
                    {
                        var previousMap = _map.Entity;
                        _map.Entity = null;
                        previousMap.Mines.Remove(this);
                    }
                    _map.Entity = value;
                    if (value != null)
                    {
                        _mapId = value.Id;
                        _sector = new VisibleSpace(value);
                    }
                    else
                    {
                        _mapId = -1;
                    }
                }
            }
        }

        #endregion

        #region Delegates
        public delegate void GetResourceHandler(King player, Resource r, bool fromMine);
        #endregion

        #region Events

        public event GetResourceHandler GetResourceEvent;
       
        #endregion
    }
}
