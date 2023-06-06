using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform target;
    private float smoothTime = 0.125f;
    private Vector3 currentVelocity = Vector3.zero;

    public LayerMask layerMask;

    public float rayDistance = 10f;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, rayDistance, layerMask))
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
        }
        else
        {
            Debug.Log("Don't Hit");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayDistance, Color.green);
        }
    }
}
