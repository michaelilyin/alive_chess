using UnityEngine;
using System.Collections;
using GameModel;
using AliveChessLibrary.GameObjects.Abstract;

public class PlayerController : MonoBehaviour
{
    private Vector3 target;

    void Start()
    {
        //player = GameCore.Instance.World.Player;
        target = transform.position;
    }

    void Update()
    {
        if (transform.position == target)
            UpdateTarget();
        //if (target != null || transform.position != target)
        //    transform.position = Vector3.Lerp(transform.position, target, 0.09f);
    }

    private void UpdateTarget()
    {
        if (GameCore.Instance.World.Steps.Count > 0)
        {
            Position pos = GameCore.Instance.World.Steps.Dequeue();
            target = new Vector3(pos.X, transform.position.y, pos.Y);
        }
    }

    private int prevX = 0;
    private int prevY = 0;
    public void UpdatePosition(Player player)
    {
        if (player != null)
        {
            if (player.King != null)
            {
                lock (player.King)
                {
                    int x = player.King.X;
                    int y = player.King.Y;
                    if ((x != prevX || y != prevY) || ((int)transform.position.x != x || (int)transform.position.z != y))
                    {
                        this.transform.position = new Vector3(x, this.transform.position.y, y);
                        prevX = x;
                        prevY = y;
                    }
                }
                UpdateTarget();
            }
        }
    }
}
