using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour
{

    private Camera main;

    // Use this for initialization
    void Start()
    {
        main = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = main.transform.position;
    }
}
