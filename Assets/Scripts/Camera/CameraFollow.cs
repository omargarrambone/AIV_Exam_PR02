using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    //public float distance;
    [SerializeField] private Transform target;
    private float smoothSpeed = 0.125f;
    private Vector3 currentVelocity = Vector3.zero;


    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);

        
    }

    public void RotateCameraWalk()
    {
        if (target.rot)
        {

        }
    }
}
