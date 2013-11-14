using UnityEngine;
using System.Collections;
using Assets.GameLogic;

public class PlayerControl : MonoBehaviour
{
    private Player _player;
    void Start()
    {
        _player = GameCore.Instance.Player;
    }

    void Update()
    {
        if (_player.King != null)
        {
            lock (_player.King)
            {
                this.transform.position = new Vector3(_player.King.X * MapManager.SIZE_FACTOR, 0, _player.King.Y * MapManager.SIZE_FACTOR);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        if (_player.King != null)
            GUILayout.Label("King position " + _player.King.X + " " + _player.King.Y);
        GUILayout.EndVertical();
    }
}
