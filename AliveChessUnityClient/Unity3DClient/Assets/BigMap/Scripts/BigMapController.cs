using UnityEngine;
using System.Collections;
using GameModel;
using Logger;
using AliveChessLibrary.GameObjects.Landscapes;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Objects;
using AliveChessLibrary.GameObjects.Resources;
using System.Data.Linq;
using System.Linq;

public class BigMapController : MonoBehaviour
{
    public GameObject PlayerPrefub;
    public GameObject EnemyPrefub;

    public GameObject CellPrefub;

    public GameObject CastlePrefub;

    public GameObject WoodMinePrefub;
    public GameObject GoldMinePrefub;
    public GameObject StoneMinePrefub;
    public GameObject CoalMinePrefub;
    public GameObject IronMinePrefub;

    public GameObject ObstaclePrefub;
    public GameObject TreePrefub;

    public GameObject WoodResourcePrefub;
    public GameObject GoldResourcePrefub;
    public GameObject StoneResourcePrefub;
    public GameObject CoalResourcePrefab;
    public GameObject IronResourcePrefub;

    public Material GrassMaterial;
    public Material SnowMaterial;
    public Material MinimapGrass;
    public Material MinimapSnow;

    public GameObject ResourcesGUIController;

    public GameObject CellSelector;

    private GameObject ground;
    private GameObject[,] cells;
    private PlayerController player;
    private GameObject resources;
    private ResourcesGUI resourcesGUI;

    void Awake()
    {
        //Screen.showCursor = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!GameCore.Instance.Network.IsConnected) Application.LoadLevel(0);
        GameCore.Instance.World.GameStateUpdated += World_GameStateUpdated;
        GameCore.Instance.World.ObjectsUpdated += World_ObjectsUpdated;
        resourcesGUI = ResourcesGUIController.GetComponent<ResourcesGUI>();
    }

    private bool _gameStateUpdated;
    private bool _objectsUpdated;

    void World_ObjectsUpdated(object sender, System.EventArgs e)
    {
        _objectsUpdated = true;
    }
    void World_GameStateUpdated(object sender, System.EventArgs e)
    {
        _gameStateUpdated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCore.Instance.World.IsCreated)
        {
            CreateWorld();
        }
        if (_gameStateUpdated)
        {
            UpdateGameState();
        }
        if (_objectsUpdated)
        {
            UpdateObjects();
        }
    }

    #region create
    private void CreateWorld()
    {
        Log.Debug("Create map view");
        Map map = GameCore.Instance.World.Map;
        CreateLandscape(map);
        CreateCastles(map);
        CreateMines(map);
        CreateSingleObjects(map);
        CreatePlayer();
        resources = new GameObject("Resources");
        resList = new Dictionary<int, ResourceController>();
        Log.Debug("Map have been cerated");
    }
    private void CreateLandscape(Map map)
    {
        ground = new GameObject("Ground");
        lock (map.BasePoints)
        {
            cells = new GameObject[map.SizeX, map.SizeY];
            foreach (var basePoint in map.BasePoints)
            {
                cells[basePoint.X, basePoint.Y] = CreateLandscapeCell(basePoint.X, basePoint.Y, basePoint.LandscapeType);
            }
            foreach (var basePoint in map.BasePoints)
            {
                Queue<MapPoint> cellsQueue = new Queue<MapPoint>();
                cellsQueue.Enqueue(map[basePoint.X, basePoint.Y]);

                while (cellsQueue.Count > 0)
                {
                    MapPoint landscape = cellsQueue.Dequeue();

                    if (landscape.X > 0 && landscape.X < map.SizeX &&
                        cells[landscape.X - 1, landscape.Y] == null)
                    {
                        cellsQueue.Enqueue(map[landscape.X - 1, landscape.Y]);
                        cells[landscape.X - 1, landscape.Y] = CreateLandscapeCell(landscape.X - 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.X < map.SizeX - 1 &&
                        cells[landscape.X + 1, landscape.Y] == null)
                    {
                        cellsQueue.Enqueue(map[landscape.X + 1, landscape.Y]);
                        cells[landscape.X + 1, landscape.Y] = CreateLandscapeCell(landscape.X + 1, landscape.Y, basePoint.LandscapeType);
                    }
                    if (landscape.Y > 0 && landscape.Y < map.SizeY &&
                        cells[landscape.X, landscape.Y - 1] == null)
                    {
                        cellsQueue.Enqueue(map[landscape.X, landscape.Y - 1]);
                        cells[landscape.X, landscape.Y - 1] = CreateLandscapeCell(landscape.X, landscape.Y - 1, basePoint.LandscapeType);
                    }
                    if (landscape.Y < map.SizeY - 1 &&
                        cells[landscape.X, landscape.Y + 1] == null)
                    {
                        cellsQueue.Enqueue(map[landscape.X, landscape.Y + 1]);
                        cells[landscape.X, landscape.Y + 1] = CreateLandscapeCell(landscape.X, landscape.Y + 1, basePoint.LandscapeType);
                    }
                }
            }
        }
    }
    private GameObject CreateLandscapeCell(int x, int z, LandscapeTypes type)
    {
        GameObject cell = Instantiate(CellPrefub, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
        Transform minimapView = cell.transform.FindChild("MinimapView");
        switch (type)
        {
            case LandscapeTypes.Grass:
                cell.renderer.material = GrassMaterial;
                if (minimapView != null) minimapView.renderer.material = MinimapGrass;
                break;
            case LandscapeTypes.Snow:
                cell.renderer.material = SnowMaterial;
                if (minimapView != null) minimapView.renderer.material = MinimapSnow;
                break;
        }
        cell.transform.parent = ground.transform;
        cell.GetComponent<MapCellController>().Selector = CellSelector.transform;
        return cell;
    }
    private void CreateCastles(Map map)
    {
        GameObject castles = new GameObject("Castles");
        foreach (Castle castle in map.Castles)
        {
            GameObject go = Instantiate(CastlePrefub, new Vector3(castle.X + 0.5f, 0.015f, castle.Y + 0.5f), Quaternion.identity) as GameObject;
            go.transform.parent = castles.transform;
            CastlleController cc = go.GetComponent<CastlleController>();
            cc.Model = castle;
            cc.CellSelector = CellSelector;
        }
    }
    public void CreateSingleObjects(Map map)
    {
        GameObject singleObjects = new GameObject("SingleObjects");
        foreach (SingleObject obj in map.SingleObjects)
        {
            GameObject go = null;
            switch (obj.SingleObjectType)
            {
                case SingleObjectType.Obstacle:
                    go = (Instantiate(ObstaclePrefub, new Vector3(obj.X, 0, obj.Y), Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject);
                    break;
                case SingleObjectType.Tree:
                    go = (Instantiate(TreePrefub, new Vector3(obj.X, 0, obj.Y), Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject);
                    break;
            }
            go.transform.parent = singleObjects.transform;
            Transform mapView = go.transform.FindChild("MapView");
            if (mapView != null) mapView.rotation = Quaternion.identity;
        }
    }
    public void CreateMines(Map map)
    {
        GameObject mines = new GameObject("Mines");
        foreach (Mine mine in map.Mines)
        {
            GameObject m = null;
            switch (mine.MineType)
            {
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Coal:
                    m = Instantiate(CoalMinePrefub, new Vector3(mine.X + 0.5f, 0, mine.Y + 0.5f), Quaternion.identity) as GameObject;
                    break;
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold:
                    m = Instantiate(GoldMinePrefub, new Vector3(mine.X + 0.5f, 0, mine.Y + 0.5f), Quaternion.identity) as GameObject;
                    break;
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Iron:
                    m = Instantiate(IronMinePrefub, new Vector3(mine.X + 0.5f, 0, mine.Y + 0.5f), Quaternion.identity) as GameObject;
                    break;
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Stone:
                    m = Instantiate(StoneMinePrefub, new Vector3(mine.X + 0.5f, 0, mine.Y + 0.5f), Quaternion.identity) as GameObject;
                    break;
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood:
                    m = Instantiate(WoodMinePrefub, new Vector3(mine.X + 0.5f, 0, mine.Y + 0.5f), Quaternion.identity) as GameObject;
                    break;
            }
            m.transform.parent = mines.transform;
            MineController mc = m.GetComponent<MineController>();
            if (mc != null)
            {
                mc.Model = mine;
                mc.CellSelector = CellSelector;
            }
        }
    }
    public void CreateMultyObjects(Map map)
    {
        foreach (MultyObject obj in map.MultyObjects)
        {
            switch (obj.MultyObjectType)
            {
                case MultyObjectTypes.Rock:
                    break;
            }
        }
    }
    public void CreatePlayer()
    {
        player = (Instantiate(PlayerPrefub) as GameObject).GetComponent<PlayerController>();
        Camera.main.GetComponent<CameraController>().CameraTarget = player.transform;
    }
    #endregion

    private void UpdateGameState()
    {
        Player pl = GameCore.Instance.World.Player;
        player.UpdatePosition(pl);
        _gameStateUpdated = false;
        //resourcesGUI.UpdateValues(pl);

    }

    private Dictionary<int, ResourceController> resList;
    private ResourceController CreateResource(Resource resource)
    {
        GameObject res = null;
        switch (resource.ResourceType)
        {
            case ResourceTypes.Coal:
                res = Instantiate(CoalResourcePrefab, new Vector3(resource.X, 0, resource.Y), Quaternion.identity) as GameObject;
                break;
            case ResourceTypes.Gold:
                res = Instantiate(GoldResourcePrefub, new Vector3(resource.X, 0, resource.Y), Quaternion.identity) as GameObject;
                break;
            case ResourceTypes.Iron:
                res = Instantiate(IronResourcePrefub, new Vector3(resource.X, 0, resource.Y), Quaternion.identity) as GameObject;
                break;
            case ResourceTypes.Stone:
                res = Instantiate(StoneResourcePrefub, new Vector3(resource.X, 0, resource.Y), Quaternion.identity) as GameObject;
                break;
            case ResourceTypes.Wood:
                res = Instantiate(WoodResourcePrefub, new Vector3(resource.X, 0, resource.Y), Quaternion.identity) as GameObject;
                break;
        }
        res.transform.parent = resources.transform;
        ResourceController rc = res.AddComponent<ResourceController>();
        rc.ID = resource.Id;
        return rc;
    }
    
    #region update
    private void UpdateResourcesOnMap(Map map)
    {
        HashSet<int> resCache = new HashSet<int>();
        lock (map.Resources)
        {
            foreach (var res in map.Resources)
            {
                if (!resList.ContainsKey(res.Id))
                    resList[res.Id] = CreateResource(res);
                resCache.Add(res.Id);

            }
            var tmp = resList.Keys.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!resCache.Contains(tmp[i]))
                {
                    Destroy(resList[tmp[i]].gameObject);
                    resList.Remove(tmp[i]);
                }

            }

        }
    }
    private void UpdateObjects()
    {
        Map map = GameCore.Instance.World.Map;
        UpdateResourcesOnMap(map);
        _objectsUpdated = false;
    }
    #endregion
}
