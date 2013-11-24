using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour
{
    private float timeA;
    public int fps;
    public int lastFPS;
    public GUIStyle textStyle;

    void Start()
    {
        timeA = 0;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if ((timeA += Time.deltaTime) <= 0.5f)
            fps++;
        else
        {
            lastFPS = fps + 1;
            timeA = 0;
            fps = 0;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(25);
        GUILayout.Label("FPS: " + lastFPS);
        GUILayout.EndVertical();
    }
}