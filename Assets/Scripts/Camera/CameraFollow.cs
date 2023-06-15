using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static public Transform CameraTarget { get; private set; }

    static private Transform staticDefaultCameraTarget;
    static private Quaternion defaultCameraRotation;
    static private CameraType cameraType;

    [SerializeField] private Transform defaultCameraTarget;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;
    private Vector3 currentVelocity;

    private void Awake()
    {
        CameraTarget = staticDefaultCameraTarget = defaultCameraTarget;

        defaultCameraRotation = Quaternion.Euler(new Vector3(16f,0f,0f));

        Camera.main.transform.rotation = defaultCameraRotation;
    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        switch (cameraType)
        {
            case CameraType.FollowPlayer:
                transform.position = Vector3.SmoothDamp(transform.position, CameraTarget.position + offset + new Vector3(PlayerManager.PlayerGameObject.transform.forward.x, 0, 0) * 3f, ref currentVelocity, smoothSpeed);
                break;
            case CameraType.StaticFollowPlayer:
                transform.position = Vector3.SmoothDamp(transform.position, CameraTarget.position + offset, ref currentVelocity, smoothSpeed);
                Vector3 relativePos = PlayerManager.PlayerGameObject.transform.position - transform.position;
                Camera.main.transform.rotation = Quaternion.LookRotation(relativePos);
                break;
        }
    }

    public static void SetCameraTarget(Transform target, CameraType type = CameraType.StaticFollowPlayer)
    {
        CameraTarget = target;
        cameraType = type;
    }

    public static void ResetCameraTarget()
    {
        CameraTarget = staticDefaultCameraTarget;
        cameraType = CameraType.FollowPlayer;
        Camera.main.transform.rotation = defaultCameraRotation;
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = CameraTarget.position + offset;
    }
}
