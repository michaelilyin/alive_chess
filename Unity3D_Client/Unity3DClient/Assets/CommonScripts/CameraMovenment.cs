using UnityEngine;
using System.Collections;

public class CameraMovenment : MonoBehaviour
{

    public Transform Target = null;
    public float Speed = 10f;
    public float RotateFactor = 0.5f;

    public void SetTarget(Transform target)
    {
        Target = target;
        if (Target == null)
            this.transform.position = new Vector3(25, 10, 25);
        else
        {
            this.transform.position = new Vector3(target.position.x, 5, target.position.y);
            Debug.Log("Set transform in camera target");
        }
    }

    void Start()
    {
        if (Target == null)
            this.transform.position = new Vector3(25, 10, 25);
        else
        {
            this.transform.position = Target.position;
            this.transform.Translate(0, 5, 0, Space.World);
        }
        this.transform.rotation = Quaternion.Euler(75, 180, 0);
    }

    void Update()
    {
        this.transform.Translate(-Speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0, Space.World);
        this.transform.Translate(0, 0, -Speed * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);

        float whell = Input.GetAxisRaw("Mouse ScrollWheel");
        if (whell > 0)
        {
            if (this.transform.position.y > .5f)
            {
                this.transform.Translate(0, -Speed * 2 * Time.deltaTime, 0, Space.World);
                this.transform.Rotate(this.transform.right, RotateFactor * this.transform.position.y);
            }
        }
        else if (whell < 0)
        {
            if (this.transform.position.y < 10)
            {
                this.transform.Translate(0, Speed * 2 * Time.deltaTime, 0, Space.World);
                this.transform.Rotate(this.transform.right, -RotateFactor * this.transform.position.y);
            }
        }
    }
}
