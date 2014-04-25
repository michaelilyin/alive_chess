using UnityEngine;
using System.Collections;
using GameModel;
using AliveChessLibrary.GameObjects.Abstract;
using Assets.CommonScripts.Utils;

public class PlayerController : MonoBehaviour
{
    public float ErrorRange = 1.6f;

    public GameObject CaracterMesh;
    public AnimationClip idle;
    public AnimationClip walk;
    private Vector3 target;
    private Vector3 direction;
    private Animation animator;

    private BigMapController bigMapController;

    private Player player;

    void Start()
    {
        bigMapController = FindObjectOfType<BigMapController>();
        player = GameCore.Instance.World.Player;
        animator = CaracterMesh.GetComponent<Animation>();
        target = transform.position;
        direction = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
    }

    private bool _active = true;
    private bool _stay = true;
    private float speed = 0;

    private void MoveKing(Vector3 pos)
    {
        if (Vector3.Distance(pos, target) >= ErrorRange)
            pos = target;
        else
            pos += direction * speed * Time.deltaTime;
        transform.position = pos;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (Vector3.Distance(pos, target) <= speed * Time.deltaTime)
        {
            pos = target;
            transform.position = pos;
            if (UpdateTarget())
                MoveKing(pos);
        }
        else
        {
            MoveKing(pos);
        }

        if (direction != transform.forward)
            transform.forward = Vector3.Lerp(transform.forward, direction, 16 * Time.deltaTime);

        if (_stay)
            animator.CrossFade(idle.name);
        else
            animator.CrossFade(walk.name);
        
    }

    private bool UpdateTarget()
    {
        if (GameCore.Instance.World.Steps.Count > 0)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.z;
            Position pos = GameCore.Instance.World.Steps.Dequeue();

            target = new Vector3(pos.X, transform.position.y, pos.Y);
            direction = (target - transform.position).normalized;
            float dist = (float)Vector2Utils.Distance(pos.X, pos.Y, x, y);

            float oldSpeed = speed;
            float cost = GameCore.Instance.World.Map.GetWayCost(pos.X, pos.Y);
            Debug.Log(cost);
            speed = dist/cost;
            if (_stay)
            {
                speed /= 2;
                _stay = false;
            }
            return true;
        }
        else
        {
            speed = 0;
            _stay = true;
            return false;
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
                    if ((x != prevX || y != prevY))
                    {
                        float factor = 0.7f;
                        if (x != prevX && y != prevY)
                            factor = 1;
                        if (Vector3.Distance(new Vector3(x,transform.position.y, y), transform.position) >= factor)
                        {
                            this.transform.position = new Vector3(x, this.transform.position.y, y);
                            prevX = x;
                            prevY = y;
                        }
                    }
                }
            }
        }
    }
}
