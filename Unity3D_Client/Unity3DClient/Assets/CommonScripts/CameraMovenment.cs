using UnityEngine;
using System.Collections;

public class CameraMovenment : MonoBehaviour
{

    public Transform Target = null;
    public float MoveSpeed = 10f;
    public float AngleSpeed = 1f;

    public void SetTarget(Transform target)
    {
        Target = target;
        if (Target == null)
            this.transform.position = new Vector3(25, 10, 25);
        else
        {
            this.transform.position = new Vector3(target.position.x, 5, target.position.y);
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
        this.transform.rotation = Quaternion.Euler(58, 180, 0);
    }

    void Update()
    {
        this.transform.Translate(-MoveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0, Space.World);
        this.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);

        float whell = Input.GetAxisRaw("Mouse ScrollWheel");
        if (whell > 0)
        {
            if (this.transform.position.y > .1f)
            {
                this.transform.Translate(0, -MoveSpeed * Time.deltaTime, 0, Space.World);
                //this.transform.Rotate(this.transform.right, AngleSpeed);
            }
        }
        else if (whell < 0)
        {
            if (this.transform.position.y < 20)
            {
                this.transform.Translate(0, MoveSpeed * Time.deltaTime, 0, Space.World);
                //this.transform.Rotate(this.transform.right, -AngleSpeed);
            }
        }
    }
}
