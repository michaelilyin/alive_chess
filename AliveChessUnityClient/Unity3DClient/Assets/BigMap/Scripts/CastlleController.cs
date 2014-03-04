using UnityEngine;
using System.Collections;
using AliveChessLibrary.GameObjects.Buildings;
using Logger;
using GameModel;


public enum CastleOwner
{
    PLAYER,
    ENEMY,
    NONE
}

public class CastlleController : MonoBehaviour
{
    public Material PlayerMaterial;
    public Material EnemyMaterial;
    public Material NeutralMaterial;
    public GameObject[] ControlObjects;
    public GameObject CastleSelector;
    public GameObject CellSelector;

    public Castle Model { get; set; }

    private CastleOwner owner = CastleOwner.NONE;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Model != null && Model.KingId.HasValue)
        {
            if (Model.KingId.Value == GameCore.Instance.World.Player.King.Id && owner != CastleOwner.PLAYER)
            {
                for (int i = 0; i < ControlObjects.Length; i++)
                {
                    ControlObjects[i].renderer.material = PlayerMaterial;
                }
                owner = CastleOwner.PLAYER;
            }
            else if (Model.KingId.Value != GameCore.Instance.World.Player.King.Id && owner != CastleOwner.ENEMY)
            {
                for (int i = 0; i < ControlObjects.Length; i++)
                {
                    ControlObjects[i].renderer.material = EnemyMaterial;
                }
                owner = CastleOwner.ENEMY;
            }
        }
        else if (owner != CastleOwner.NONE)
        {
            for (int i = 0; i < ControlObjects.Length; i++)
            {
                ControlObjects[i].renderer.material = NeutralMaterial;
            }
            owner = CastleOwner.NONE;
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
        CastleSelector.SetActive(true);
        CellSelector.SetActive(false);
    }

    void OnMouseExit()
    {
        CastleSelector.SetActive(false);
        CellSelector.SetActive(true);
    }
}
