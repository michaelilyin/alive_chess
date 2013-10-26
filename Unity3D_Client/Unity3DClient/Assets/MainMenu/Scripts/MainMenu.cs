using UnityEngine;
using System.Collections;
using System.Net;
using AliveChessLibrary.Commands.RegisterCommand;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Assets.GameLogic.Network.Network.OnConnected += Network_OnConnected;
        Assets.GameLogic.Network.Network.Connect(IPAddress.Parse("127.0.0.1"));
	}

    void Network_OnConnected(object sender, System.EventArgs e)
    {
        AuthorizeRequest request = new AuthorizeRequest();
        request.Login = "player";
        request.Password = "pw";
        Assets.GameLogic.Network.Network.Send(request);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnApplicationQuit()
    {
        Assets.GameLogic.Network.Network.Disconnect();
    }
}
