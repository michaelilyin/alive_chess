using UnityEngine;
using System.Collections;
using AliveChessLibrary.GameObjects.Buildings;
using GameModel;
using Logger;

public enum MineOwner
{
    PLAYER, 
    ENEMY, 
    NONE
}

public class MineController : MonoBehaviour
{
    public Material PlayerMaterial;
    public Material EnemyMaterial;
    public Material NeutralMaterial;
    public GameObject ControlObject;
    public GameObject MapView;

    public GameObject MineSelector;
    public GameObject CellSelector;

    public Mine Model { get; set; }
    private MineOwner owner;

    void Start()
    {
        owner = MineOwner.NONE;
    }

    void Update()
    {
        if (Model != null && Model.KingId.HasValue)
        {
            if (Model.KingId.Value == GameCore.Instance.World.Player.King.Id && owner != MineOwner.PLAYER)
            {
                Material[] mats = ControlObject.renderer.materials;
                mats[1] = PlayerMaterial;
                ControlObject.renderer.materials = mats;
                MapView.renderer.material = PlayerMaterial;
                owner = MineOwner.PLAYER;
            } 
            else if (Model.KingId.Value != GameCore.Instance.World.Player.King.Id && owner != MineOwner.ENEMY)
            {
                Material[] mats = ControlObject.renderer.materials;
                mats[1] = EnemyMaterial;
                ControlObject.renderer.materials = mats;
                MapView.renderer.material = EnemyMaterial;
                owner = MineOwner.ENEMY;
            }
        }
        else if (owner != MineOwner.NONE)
        {
            Material[] mats = ControlObject.renderer.materials;
            mats[1] = NeutralMaterial;
            ControlObject.renderer.materials = mats;
            MapView.renderer.material = NeutralMaterial;
            owner = MineOwner.NONE;
        }
    }

    void OnMouseDown()
    {
        if (Model != null)
        {
            int x = Model.X;
            int y = Model.Y;
            Log.Message(string.Format("Move King x={0} y={1}", x, y));
            GameCore.Instance.Network.BigMapCommandController.SendMoveKingRequest(x, y);
        }
        //GameCore.Instance.World.Map.Locate(x, y);
    }

    void OnMouseEnter()
    {
        MineSelector.SetActive(true);
        CellSelector.SetActive(false);
    }

    void OnMouseExit()
    {
        MineSelector.SetActive(false);
        CellSelector.SetActive(true);
    }
}
