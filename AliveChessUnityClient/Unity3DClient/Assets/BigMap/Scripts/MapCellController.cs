using UnityEngine;
using System.Collections;
using Logger;
using GameModel;

public class MapCellController : MonoBehaviour
{
    //public Color ActiveCellColor;
    public Transform Selector;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.z;
        Log.Message(string.Format("Move King x={0} y={1}", x, y));
        GameCore.Instance.Network.BigMapCommandController.SendMoveKingRequest(x, y);
        //GameCore.Instance.World.Map.Locate(x, y);
    }

    void OnMouseEnter()
    {
        //this.renderer.material.color = ActiveCellColor;
        //selector.Translate(transform.position.x, selector.position.y, transform.position.z);
        Selector.position = new Vector3(transform.position.x, Selector.position.y, transform.position.z);

    }

    void OnMouseExit()
    {
        //this.renderer.material.color = Color.white;
    }


}
