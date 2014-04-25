using UnityEngine;
using System.Collections;
using Assets.BigMap.Scripts.MapGUI;
using GameModel;
using System.Text;

[RequireComponent(typeof(AudioSource))]
public class MessageWindow : MonoBehaviour
{
    public GUISkin skin;

    public AudioSource ClickSound;

    private Rect _windowPos;
    private Vector2 _scrollPos;
    // Use this for initialization
    void Start()
    {
        _windowPos = new Rect(0, Screen.height - 350, 300, 350);
        _scrollPos = new Vector2(0,1000);
        text = new StringBuilder("Game started\n---------------\n");
    }

    void OnGUI()
    {
        GUI.skin = skin;
        _windowPos = GUILayout.Window((int)GUIIdentifers.ChatWindow, _windowPos, ChatWindow, "Chat");
    }

    private string message = "";
    private void ChatWindow(int id)
    {
        GUI.skin = skin;
        GUILayout.BeginVertical();
        GUILayout.BeginScrollView(_scrollPos, false, true);
        GUILayout.TextArea(text.ToString());
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        message = GUILayout.TextField(message);
        if (GUILayout.Button("Send"))
        {
            ClickSound.Play();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (text.Length > 1000)
            text.Remove(0, 500);
        if (GameCore.Instance.Network.Messages.HasMessages)
        {
            string[] messages = GameCore.Instance.Network.Messages.GetMessages(GameCore.Instance.Network.Messages.Count);
            foreach (var s in messages)
            {
                text.AppendLine(s);
                text.AppendLine("---------------");
            }
        }
    }

    private StringBuilder text { get; set; }

    public void AddMessage(string message)
    {
        text.AppendLine(message);
        text.AppendLine("---------------");
    }
}
