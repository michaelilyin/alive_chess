using UnityEngine;
using System.Collections;
using Logger;

public class CameraController : MonoBehaviour
{

    public Transform CameraTarget = null;
    public float Acceleration = 0.1f;
    public float MaxHeight = 20;
    public float MinHeight = 0.5f;
    public float CameraSpeed = 2;
    public float ZoomSpeed = 10;
    public float RotationCoeff = 4;
    

    private bool centerMode = false;
    private bool overviewMode = false;
    private float x = 0;
    private float z = 0;
    private float y = 0;
    private float height;

    private Vector3 target;
    private Quaternion targetRotation;

    // Use this for initialization
    void Start()
    {
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        ChangeTargetHeight(transform.position.y);
        CenterCamera();
        targetRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void CenterCamera()
    {
        if (CameraTarget != null)
        {
            centerMode = true;
            target.x = CameraTarget.position.x;
            target.z = CameraTarget.position.z;
        }
    }

    private void ChangeTargetHeight(float value)
    {
        target.y = value;
        targetRotation = Quaternion.Euler(value * RotationCoeff, 180, 0);
    }

    private void HandleInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        y = Input.GetAxisRaw("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.C))
        {
            CenterCamera();
        } 
        else
        {
            if (x != 0 || z != 0)
            {
                centerMode = false;
                target.x = transform.position.x - x * CameraSpeed * transform.position.y;
                target.z = transform.position.z - z * CameraSpeed * transform.position.y;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            overviewMode = !overviewMode;
            if (overviewMode)
            {
                height = target.y;
                ChangeTargetHeight(MaxHeight);

            }
            else
            {
                ChangeTargetHeight(height);
            }
        }
        if (y != 0)
        {
            overviewMode = false;
            ChangeTargetHeight(target.y - y * ZoomSpeed);
            if (target.y < MinHeight) ChangeTargetHeight(MinHeight);
            if (target.y > MaxHeight) ChangeTargetHeight(MaxHeight);
        }
    }


    private void RecalculatePosition()
    {
        if (transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position, target, Acceleration);
        }
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Acceleration);
        }
    }

    void Update()
    {
        HandleInput();
    }

    float swp;
    void LateUpdate()
    {
        if (centerMode)
        {
            swp = target.y;
            target = CameraTarget.transform.position - (transform.forward * swp * 1.7f);
            target.y = swp;
        }
        RecalculatePosition();
    }
}
