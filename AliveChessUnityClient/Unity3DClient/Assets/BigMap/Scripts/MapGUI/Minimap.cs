using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{

    public RenderTexture MiniMapTexture;
    public Texture Face;
    public Material MinimapMaterial;
    public float Size;
    private Rect position;
    public float Offset;

    // Use this for initialization
    void Awake()
    {
        position = new Rect(Screen.width - Size - Offset, Offset, Size, Size);
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            Graphics.DrawTexture(position, MiniMapTexture, MinimapMaterial);
            Graphics.DrawTexture(position, Face);
        }
    }
}
