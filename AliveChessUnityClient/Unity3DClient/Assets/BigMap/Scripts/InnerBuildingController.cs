using UnityEngine;
using System.Collections;
using AliveChessLibrary.GameObjects.Buildings;

public class InnerBuildingController : MonoBehaviour
{
    public InnerBuildingType type;

    private bool active;
    //public Castle

    // Use this for initialization
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    public void RecalculateView(Castle castle)
    {
        if (castle.HasBuilding(type) && !active)
            Create();
        else if (active)
            Destroy();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void Create()
    {
        gameObject.SetActive(true);
    }


}
