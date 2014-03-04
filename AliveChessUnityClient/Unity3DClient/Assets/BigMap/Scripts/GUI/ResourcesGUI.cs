using UnityEngine;
using System.Collections;
using GameModel;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Resources;
using System;

[Serializable]
public class TableEntry
{
    public ResourceTypes Type;
    public Texture2D Picture;
    public int Value;
}

public class ResourcesGUI : MonoBehaviour
{
    public TableEntry[] Resources;

    public GUISkin Skin;

    //private Rect[] resImages;
    //private Rect[] resLabels;
    private Rect[] resRects;
    private Rect resBox;

    void Start()
    {
        //IndexesCache = new Dictionary<ResourceTypes, int>();
        //foreach (TableEntry te in Table)
        //    IndexesCache[te.type] = te.index;
        //resImages = new Rect[5];
        //resLabels = new Rect[5];
        int n = Resources.Length;
        resRects = new Rect[n];
        float middle = Screen.width / 2;
        float cellw = Screen.height / n;
        float cellh = cellw / 4;

        float barw = cellw * n;
        resBox = new Rect(middle - barw / 2, 0, barw, cellh);
        for (int i = 0; i < n; i++)
        {
            resRects[i] = new Rect(i * cellw, 0, cellw, cellh);
        }
        GameCore.Instance.World.GameStateUpdated += World_GameStateUpdated;
        //for (int i = 0; i < 5; i++)
        //{
        //    resImages[i] = new Rect(resStart + i * resCell, 0, pict, pict);
        //    resLabels[i] = new Rect(resStart + i * resCell + pict, 0, resCell - pict, pict);
        //}
    }

    private bool _updated;
    void World_GameStateUpdated(object sender, EventArgs e)
    {
        _updated = true;
    }

    //private void DrawResources()
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        GUI.DrawTexture(resImages[i], Resources[i]);
    //        GUI.Label(resLabels[i], new GUIContent(Resources[i], Values[i].ToString()));
    //    }
    //}

    void OnGUI()
    {
        if (_updated)
        {
            lock (GameCore.Instance.World.Player.ResourcesCache)
            {
                var resources = GameCore.Instance.World.Player.ResourcesCache;
                for (int i = 0; i < Resources.Length; i++)
                {
                    if (resources.ContainsKey(Resources[i].Type))
                        Resources[i].Value = resources[Resources[i].Type];
                }
            }
        }
        //if (GUI.Button(new Rect(10, 10, 100, 50), icon))
        //{
        //    print("you clicked the icon");
        //}

        //if (GUI.Button(new Rect(10, 70, 100, 20), "This is text"))
        //{
        //    print("you clicked the text button");
        //}
        //GUI.Box(new Rect(10, 100, 100, 50), new GUIContent("This is text", icon));
        //GUI.Button(new Rect(10, 160, 100, 20), new GUIContent("Click me", "This is the tooltip"));
        //GUI.Label(new Rect(10, 190, 100, 20), GUI.tooltip);
        //GUI.Button (new Rect (10,230,100,20), new GUIContent ("Click me", icon, "This is the tooltip"));
        //GUI.Label (new Rect (10,260,100,20), GUI.tooltip);
        GUI.skin = Skin;
        GUI.Box(resBox, "");
        GUI.BeginGroup(resBox);
        for (int i = 0; i < Resources.Length; i++)
        {
            GUI.Label(resRects[i], new GUIContent(Resources[i].Value.ToString(), Resources[i].Picture));
        }
        GUI.EndGroup();
    }
}
