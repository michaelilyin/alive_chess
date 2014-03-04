using UnityEngine;
using System.Collections;
using GameModel;
using Logger;

public class CloseAppController : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnApplicationQuit()
    {
        GameCore.Instance.StopGame();
        GameCore.Instance.Network.Disconnect();
        Log.Message("Application closed");
        Log.Close();
    }
}
