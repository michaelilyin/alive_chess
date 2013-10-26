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
        GameCore.Instance.Network.OnConnected += Network_OnConnected;
        GameCore.Instance.Authorized += Network_Authorized;
        CurrentMenu = MenuType.MainMenu;
    }

    public void OnGUI()
    {
        if (_showMenu)
        {
            switch (CurrentMenu)
            {
                case MenuType.MainMenu:
                    GUILayout.Window(0, new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 200), MainMenuWindow, "Alive Chess");
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
            if (GUILayout.Button("Start"))
            {

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
        GameCore.Instance.Network.Disconnect();
    }

    #endregion
}
