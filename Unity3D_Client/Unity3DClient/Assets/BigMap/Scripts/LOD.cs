using UnityEngine;
using System.Collections;

public class LOD : MonoBehaviour
{

    public GameObject[] Views;
    public float[] Distances;

    private Camera main;
    private int current;

    // Use this for initialization
    void Start()
    {
        current = 0;
        foreach (GameObject go in Views)
            go.SetActive(false);
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(main.transform.position, transform.position);
        int level = 0;
        for (int i = 0; i < Distances.Length; i++)
        {
            if (dist < Distances[i])
            {
                level = i;
                break;
            }
        }
        SetLOD(level);
    }

    private void SetLOD(int level)
    {
        //if (current != level)
        {
            Views[current].SetActive(false);
            Views[level].SetActive(true);
            current = level;
        }
    }
}
