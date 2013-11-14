using UnityEngine;
using System.Collections;
using Assets.GameLogic;

public class MapClickController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            int x = (int)hit.point.x / MapManager.SIZE_FACTOR;
            int y = (int)hit.point.z / MapManager.SIZE_FACTOR;
            GameCore.Instance.BigMapCommandController.SendMoveKingRequest(x, y);
            Debug.Log("new position " + x + " " + y);
        }
    }
}
