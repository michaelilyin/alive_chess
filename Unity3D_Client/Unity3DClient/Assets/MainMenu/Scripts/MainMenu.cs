using UnityEngine;
using System.Collections;
using System.Net;
using AliveChessLibrary.Commands.RegisterCommand;
using Assets.GameLogic;

public enum MenuType
{
    MainMenu, Settings
}

public class MainMenu : MonoBehaviour
{

    public static MenuType CurrentMenu { get; set; }
    
    private bool _showMenu = true;
    private bool _isGameMode = false;
    private bool _isConnected = false;

    public void Start()
    {
        DontDestroyOnLoad(this);
        GameCore.Instance.Network.OnConnected += Network_OnConnected;
        GameCore.Instance.Authorized += Network_Authorized;
        CurrentMenu = MenuType.MainMenu;
    }

    public void Update()
    {
        if (_isGameMode)
            if (Input.GetButtonDown("Menu"))
                _showMenu = !_showMenu;
    }

    public void OnGUI()
    {
        if (_showMenu)
        {
            switch (CurrentMenu)
            {
                case MenuType.MainMenu:
                    GUILayout.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 300), MainMenuWindow, "Alive Chess");
                    break;
            }
        }
    }

    #region MainMenuGUI

    private void MainMenuWindow(int id)
    {
        GUILayout.BeginVertical();
        if (_isConnected)
        {
            if (_isGameMode)
            {
                if (GUILayout.Button("Resume"))
                {
                    _showMenu = false;
                    _isGameMode = true;
                }
            }
            else
            {
                if (GUILayout.Button("Start"))
                {
                    Application.LoadLevel(1);
                    _showMenu = false;
                    _isGameMode = true;
                }
            }
        }
        else
        {
            if (GUILayout.Button("Connect"))
            {
                ConnectToServer();
            }
        }
        if (GUILayout.Button("Exit"))
        {
            Application.Quit();
        }
        GUILayout.EndVertical();
    }

    #endregion

    #region Connection

    private void ConnectToServer()
    {
        GameCore.Instance.ConnectToServer();
    }

    public void Authorize()
    {
        AuthorizeRequest request = new AuthorizeRequest();
        request.Login = "player";
        request.Password = "pw";
        GameCore.Instance.Network.Send(request);
    }

    void Network_Authorized(object sender, System.EventArgs e)
    {
        this._isConnected = true;
    }

    public void Network_OnConnected(object sender, System.EventArgs e)
    {
        Authorize();
    }

    public void OnApplicationQuit()
    {
        if (GameCore.Instance.Network.IsConnected)
            GameCore.Instance.Network.Disconnect();
    }

    #endregion
}
