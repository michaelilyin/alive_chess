using System;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.GameObjects.Resources;
using AliveChessLibrary.Interfaces;
using AliveChessLibrary.Utility;
using ProtoBuf;
#if !UNITY_EDITOR
using System.Data.Linq;
#endif

namespace AliveChessLibrary.GameObjects.Buildings
{
    /// <summary>
    /// шахта
    /// </summary>
    [ProtoContract]
    public class Mine : IBuilding, IActive, IEquatable<int>, IEquatable<MapPoint>, IMultyPoint
    {
        #region Variables

        [ProtoMember(1)]
        private int _mineId;

        [ProtoMember(2)]
        private int _leftX;
        [ProtoMember(3)]
        private int _topY;
        [ProtoMember(4)]
        private int _width;
        [ProtoMember(5)]
        private int _height;
        [ProtoMember(6)]
        private float _wayCost;
        [ProtoMember(7)]
        private Resource _gainingResource;
        [ProtoMember(8)]
        private int _sizeMine;

        private int _imageId;
        private VisibleSpace _sector;
        private ResourceTypes _mineType;

        //[ProtoMember(2)]
        private MapSector _viewOnMap; // сектор на карте

        private string _messegerMine; // сообщение от шахты
        private ResourceStore _valutResurs; // указатель на Хранилище ресурсов
        private int _intensityMiningMine; // интенсивность добычи ресурсов(количество ресурсов производимых за раз)
        private bool _active; // активна ли шахта
        private DateTime _dateLastWorkMine; // дата последенй работы шахты

#if !UNITY_EDITOR
        private int? _mapId;
        private int? _kingId;
        private EntityRef<Map> _map; // ссылка на карту
        private EntityRef<King> _king; // ссылка на короля
#else
        private Map _map;
        private King _king;
#endif
        private int _distance = 3;
      
        private const int DEFAULT_SIZE = 1000;

        #endregion

        #region Constructors

        public Mine()
        {
#if !UNITY_EDITOR
            this._map = default(EntityRef<Map>);
            this._king = default(EntityRef<King>);
#else
            this.Map = null;
            this.King = null;
#endif
            _sector = new VisibleSpace(this);

            if (OnLoad != null)
                OnLoad(this);
        }

        #endregion

        #region Initialization

        /// <summary>
        /// добавления на карту занимаемого сектора
        /// </summary>
        /// <param name="sector"></param>
        public void AddView(MapSector sector)
        {
            ViewOnMap = sector;
            sector.SetOwner(this);
        }

        /// <summary>
        /// удаление с карты сектора
        /// </summary>
        public void RemoveView()
        {
            ViewOnMap.SetOwner(null);
        }

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="map">карта</param>
        /// <param name="sector">область арты</param>
        /// <param name="type">тип ресурса</param>
        /// <param name="intensivityMining">интенсивность добычи</param>
        public void Initialize(Map map, MapSector sector, ResourceTypes type, 
            int intensivityMining)
        {
            Initialize(map, type, intensivityMining);

            if (sector != null)
                AddView(sector);
        }

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="map">карта</param>
        /// <param name="typeRes">тип ресурса</param>
        /// <param name="intensivityMining">коэффициент активности</param>
        public void Initialize(Map map, ResourceTypes typeRes,
            int intensivityMining)
        {
            this._map.Entity = map;
            this._mapId = map.Id;
            this._dateLastWorkMine = DateTime.Now;
            this._active = false; // активировать шахту
            this._gainingResource = new Resource();
            this._gainingResource.ResourceType = typeRes;
            this._intensityMiningMine = intensivityMining;
            this._sizeMine = DEFAULT_SIZE;
        }

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="map">карта</param>
        /// <param name="typeRes">тип ресурса</param>
        /// <param name="size">размер</param>
        /// <param name="intensivityMining">коэффициент активности</param>
        public void Initialize(int id, Map map, ResourceTypes typeRes,
            int size, int intensivityMining)
        {
            this._mineId = id;
            this._dateLastWorkMine = DateTime.Now;
            this._active = false; // активировать шахту
            this._sizeMine = size;
            Initialize(map, typeRes, intensivityMining);
        }

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="map">карта</param>
        /// <param name="typeRes">тип ресурса</param>
        /// <param name="size">размер</param>
        /// <param name="intensivityMining">коэффициент активности</param>
        /// <param name="vault">назначенно хранилище</param>
        public void Initialize(int id, Map map, ResourceTypes typeRes,
            int size, int intensivityMining, ResourceStore vault)
        {
            this._valutResurs = vault;
            Initialize(id, map, typeRes, size, intensivityMining);
        }

        #endregion

        #region Methods

        /// <summary>
        /// назначаем владельца шахты
        /// </summary>
        /// <param name="king"></param>
        public void SetOwner(King king)
        {
            if (_king.Entity != null && king != null)
                throw new AliveChessException("Owner isn't null");
            else
            {
                _king.Entity = king;
                _kingId = king != null ? king.Id : -1;
            }
        }

        /// <summary>
        /// сравнение по идентификатору
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(int other)
        {
            return Id.CompareTo(other) == 0 ? true : false;
        }

        /// <summary>
        /// сравненеи по ячейке
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MapPoint other)
        {
            return Id.CompareTo(other.Owner.Id) == 0 ? true : false;
        }

        /// <summary>
        /// активация шахты
        /// </summary>
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

        /// <summary>
        /// процесс работы шахты
        /// </summary>
        /// <param name="tmpDateTime"></param>
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
                        GetResourceEvent(this.King, _gainingResource, true);
                    }

                    // сохранить новую дату работы шахты
                    this._dateLastWorkMine = tmpDateTime;
                }
            }
        }

        /// <summary>
        /// деактивация шахты
        /// </summary>
        public void Deactivation()
        {
            this._active = false;
        }

        /// <summary>
        /// присоединение хранилища ресурсов
        /// </summary>
        /// <param name="vault"></param>
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

        /// <summary>
        /// отсоеддинение хранилища ресурсов
        /// </summary>
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


        /// <summary>
        /// создание ресурса
        /// </summary>
        /// <param name="amountResource"></param>
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

        /// <summary>
        /// получение количества ресурсов в шахте
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// проверка шахты на переполнение
        /// </summary>
        /// <returns></returns>
        private bool MineOverflow()
        {
            // Если размер ресурса превышает максимальный размер шахты
            if (this._gainingResource.CountResource >= this._sizeMine)
                return true;
            else
                return false;
        }

        /// <summary>
        /// проверка наличия хранилища ресурсов
        /// </summary>
        /// <returns></returns>
        private bool PresenceValutResurs()
        {
            // Проверить присутствие Хранилища ресурсов
            if (this._valutResurs != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// отправка ресурсов из шахты в хранилище
        /// </summary>
        private void TranslationResource()
        {
            // передать ресурс из шахты в Хранилище 
            this._valutResurs.AddResourceToRepository(this._gainingResource);
            // обнулить количества ресурсов в шахте
            this._gainingResource.CountResource = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// координала левого верхнего угла по X
        /// </summary>
        public int X
        {
            get { return _leftX; }
            set { _leftX = value; }
        }

        /// <summary>
        /// координала левого верхнего угла по Y
        /// </summary>
        public int Y
        {
            get { return _topY; }
            set { _topY = value; }
        }

        /// <summary>
        /// размер по X
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// размер по Y
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// идентификатор картинки
        /// </summary>
        public int ImageId
        {
            get { return _imageId; }
            set { _imageId = value; }
        }

        /// <summary>
        /// стоимость прохождения
        /// </summary>
        public float WayCost
        {
            get { return _wayCost; }
            set { _wayCost = value; }
        }

        /// <summary>
        /// тип ячейки
        /// </summary>
        public PointTypes Type
        {
            get { return PointTypes.Mine; }
        }

        /// <summary>
        /// дистанция видимости
        /// </summary>
        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// получаем игрока владеющего шахтой либо null
        /// </summary>
        public IPlayer Player
        {
            get
            {
                if (King != null)
                    return King.Player;
                else return null;
            }
        }

        /// <summary>
        /// тип здания
        /// </summary>
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

        public DateTime DateLastWorkMine
        {
            get { return _dateLastWorkMine; }
            set { _dateLastWorkMine = value; }
        }

        /// <summary>
        /// область видимости
        /// </summary>
        public VisibleSpace VisibleSpace
        {
            get { return _sector; }
            set { _sector = value; }
        }

        public MapSector ViewOnMap
        {
            get { return _viewOnMap; }
            set { _viewOnMap = value; }
        }

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

#if !UNITY_EDITOR
       
        public int? MapId
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
#endif

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

#if !UNITY_EDITOR
   
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
#endif
#if !UNITY_EDITOR
        
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
                    }
                    else
                    {
                        _mapId = null;
                    }
                }
            }
        }
#else
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public King King
        {
            get { return _king; }
            set { _king = value; }
        }
#endif
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
