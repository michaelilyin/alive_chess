using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LODLevel
{
    DETAILS, 
    MIDDLE, 
    LOW
}

public class LOD : MonoBehaviour
{
    public GameObject DetailsLevel;
    public float DetailsLevelMaxDistance;

    public GameObject MiddleLevel;
    public float MiddleLevelMaxDistance;

    public GameObject LowLevel;

    private LODLevel currentLevel;

    private Camera camera;
    private Dictionary<LODLevel, GameObject> lodCache;

    void Start()
    {
        camera = Camera.main;
        currentLevel = LODLevel.DETAILS;
        lodCache = new Dictionary<LODLevel, GameObject>();
        lodCache[LODLevel.LOW] = LowLevel;
        lodCache[LODLevel.MIDDLE] = MiddleLevel;
        lodCache[LODLevel.DETAILS] = DetailsLevel;
    }

    private float CalculateCameraDistance()
    {
        return Vector3.Distance(camera.transform.position, MiddleLevel.transform.position);
    }

    private void ChangeLOD(LODLevel target)
    {
        if (currentLevel != target)
        {
            lodCache[currentLevel].SetActive(false);
            lodCache[target].SetActive(true);
            currentLevel = target;
        }
    }

    private void ChangeLOD()
    {
        float dist = CalculateCameraDistance();
        if (dist > MiddleLevelMaxDistance)
            ChangeLOD(LODLevel.LOW);
        else if (dist > DetailsLevelMaxDistance)
            ChangeLOD(LODLevel.MIDDLE);
        else
            ChangeLOD(LODLevel.DETAILS);
    }

    void LateUpdate()
    {
        ChangeLOD();
    }
}
