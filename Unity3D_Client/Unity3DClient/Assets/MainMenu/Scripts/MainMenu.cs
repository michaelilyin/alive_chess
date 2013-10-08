using UnityEngine;
using System.Collections;
using GameLogicLibrary;
using AliveChessLibrary;

public class MainMenu : MonoBehaviour
{
    private Rect _mainMenuWindowRect;

    #region On-methods
    // Use this for initialization
    void Start()
    {
        RecalculateMenuRect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        _mainMenuWindowRect = GUILayout.Window(0, _mainMenuWindowRect, MenuWindow, "Alive chess");
    }
    #endregion

    private void MenuWindow(int id)
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        if (GUILayout.Button("Log in"))
        {
            GameCore.Instance.ConnectToServer();
        }
        if (GUILayout.Button("Start game"))
        {

        }
        if (GUILayout.Button("Settings"))
        {

        }
        if (GUILayout.Button("Exit"))
        {
            Application.Quit();
        }
        GUILayout.Space(10);
        GUILayout.EndHorizontal();
    }

    private void RecalculateMenuRect()
    {
        _mainMenuWindowRect = new Rect(Screen.width / 2 - 90, Screen.height / 2 - 130, 180, 260);
    }
}
