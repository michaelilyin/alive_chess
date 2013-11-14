using UnityEngine;
using System.Collections;
using Assets.GameLogic;

public class MapManager : MonoBehaviour
{
    public const int SIZE_FACTOR = 1;

    public Light MainLight;

    public GameObject PlayerPrefab;

    public GameObject CastlePrefab;

    public GameObject DefaultMinePrefab;
    public GameObject StoneMinePrefab;
    public GameObject GoldMinePrefab;
    public GameObject CoalMinePrefab;
    public GameObject IronMinePrefab;
    public GameObject WoodMinePrefab;

    public GameObject[] TreePrefubs;

    public Material EarthGrassMaterial;

    private World _world;

    public void Start()
    {
        _world = GameCore.Instance.World;
        GameCore.Instance.BigMapCommandController.SendGetMapRequest();
        GameCore.Instance.BigMapCommandController.StartGameStateUpdate();
    }

    #region CreateMapObjects

    private void CreateLight()
    {
        Object light = Instantiate(MainLight, new Vector3(0, 15, 0), Quaternion.Euler(45, 180, 0));
        light.name = "Sun";
    }
    private void CreateEarth()
    {
        GameObject earth = GameObject.CreatePrimitive(PrimitiveType.Cube);
        earth.transform.localScale = new Vector3(_world.Map.SizeX * SIZE_FACTOR, 0.5f, _world.Map.SizeY * SIZE_FACTOR);
        earth.transform.position = new Vector3(_world.Map.SizeX / 2 * SIZE_FACTOR, -0.75f, _world.Map.SizeY / 2 * SIZE_FACTOR);
        earth.transform.rotation = Quaternion.identity;
        earth.name = "Earth";
        earth.GetComponent<MeshRenderer>().material = new Material(EarthGrassMaterial);
        earth.AddComponent<MapClickController>();        
    }
    private void CreateCastles()
    {
        foreach (var castle in _world.Map.Castles)
        {
            Instantiate(CastlePrefab, new Vector3(castle.X * SIZE_FACTOR, -0.5f, castle.Y * SIZE_FACTOR), Quaternion.Euler(0, 90, 0));
        }
    }
    private void CreateMines()
    {
        foreach (var mine in _world.Map.Mines)
        {
            GameObject prefub = DefaultMinePrefab;
            switch (mine.MineType)
            {
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Coal:
                    prefub = CoalMinePrefab;
                    break;
            //    case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Gold:
            //        prefub = GoldMinePrefab;
            //        break;
            //    case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Iron:
            //        prefub = IronMinePrefab;
            //        break;
                case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Stone:
                    prefub = StoneMinePrefab;
                    break;
            //    case AliveChessLibrary.GameObjects.Resources.ResourceTypes.Wood:
            //        prefub = WoodMinePrefab;
            //        break;
            }
            Instantiate(prefub, new Vector3(mine.X * SIZE_FACTOR, -0.5f, mine.Y * SIZE_FACTOR), Quaternion.identity);
        }
    }
    private void CreateSingleObjects()
    {
        foreach (var obj in _world.Map.SingleObjects)
        {
            switch(obj.SingleObjectType)
            {
                case AliveChessLibrary.GameObjects.Objects.SingleObjectType.Tree:
                    Instantiate(TreePrefubs[Random.Range(0, TreePrefubs.Length)], new Vector3(obj.X * SIZE_FACTOR, 0, obj.Y * SIZE_FACTOR), Quaternion.identity);
                    break;
            }
        }
    }

    #endregion

    private void CreateMap()
    {
        CreateLight();
        CreateEarth();
        CreateCastles();
        CreateMines();
        CreateSingleObjects();
        Transform pl = Instantiate(PlayerPrefab) as Transform;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovenment>().SetTarget(pl);
    }

    public void Update()
    {
        if (_world.Updated)
        {
            CreateMap();
            _world.Updated = false;
        }
        if (_world.Map != null)
            for (int i = 0; i < _world.Map.SizeX * SIZE_FACTOR; i += SIZE_FACTOR)
            {
                Debug.DrawLine(new Vector3(0, 0, i), new Vector3(_world.Map.SizeX * SIZE_FACTOR, 0, i));
                Debug.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 0, _world.Map.SizeX * SIZE_FACTOR));
            }
    }
}
