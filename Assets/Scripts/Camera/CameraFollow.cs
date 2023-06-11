using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;


    private void Start()
    {
        //offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 relativePos = target.position - transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
        transform.rotation = Quaternion.LookRotation(relativePos);
    }
}
