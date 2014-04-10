using UnityEngine;
using System.Collections;
using GameModel;
using AliveChessLibrary.GameObjects.Abstract;

public class PlayerController : MonoBehaviour
{
    public float Speed = 1;
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
    void Update()
    {
        if (transform.position == target)
            UpdateTarget();
        if (Vector3.Distance(transform.position, target) < Speed * Time.deltaTime)
        {
            if (transform.position != target)
            {
                transform.position.Set(target.x, transform.position.y, target.z);
                target = transform.position;
            }
            UpdateTarget();
        }
        else
        {

        }
        if (direction != transform.forward)
            transform.forward = Vector3.Lerp(transform.forward, direction, 16 * Time.deltaTime);
        if (target != transform.position)
        {
            animator.CrossFade(walk.name);
            transform.position += direction * Speed * Time.deltaTime;
        }
        else
            animator.CrossFade(idle.name);
        //if (target != null || transform.position != target)
        //    transform.position = Vector3.Lerp(transform.position, target, 0.09f);
    }

    private void UpdateTarget()
    {
        if (GameCore.Instance.World.Steps.Count > 0)
        {
            Position pos;
            do 
            {
                pos = GameCore.Instance.World.Steps.Dequeue();
            } while (GameCore.Instance.World.Steps.Count > 0 && bigMapController.Cells[pos.X, pos.Y].GetComponentInChildren<CastlleController>() == null
                && bigMapController.Cells[pos.X, pos.Y].GetComponentInChildren<MineController>() == null);

            if (bigMapController.Cells[pos.X, pos.Y].GetComponentInChildren<CastlleController>() == null
                && bigMapController.Cells[pos.X, pos.Y].GetComponentInChildren<MineController>() == null)
            {
                target = new Vector3(pos.X, transform.position.y, pos.Y);
                direction = (target - transform.position).normalized;
            }
            else
                Debug.Log("Illegal movement");
            //transform.forward = direction;
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
