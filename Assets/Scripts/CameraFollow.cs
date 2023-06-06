using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 0.125f;
    public Vector3 offset;

    public LayerMask layerMask;
    public float distance = 10f;

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, distance, layerMask))
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.forward) * hitInfo.distance, Color.red);
        }
        else
        {
            Debug.Log("Don't Hit");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.green);
        }
    }
}
