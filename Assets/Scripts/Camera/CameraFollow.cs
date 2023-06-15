using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] float timer, counter;
    static Quaternion oldRotation, nextRotation, lookAtRotation;
    static bool isStarted;

    private void Awake()
    {
        CameraTarget = staticDefaultCameraTarget = defaultCameraTarget;

        defaultCameraRotation = Quaternion.Euler(new Vector3(16f,0f,0f));

        oldRotation = defaultCameraRotation;
        Camera.main.transform.rotation = defaultCameraRotation;

        timer = 1f;
    }

    private void FixedUpdate()
    {
        CameraMovement();

        if (!isStarted) return;

        counter += Time.deltaTime;

        Quaternion value = Quaternion.Lerp(oldRotation, nextRotation, counter / timer);
        Camera.main.transform.rotation = value;

        if (counter > timer) { isStarted = false; counter = 0; Camera.main.transform.rotation = nextRotation; }
    }

    private void CameraMovement()
    {
        Vector3 targetVector = CameraTarget.position + offset;

        switch (cameraType)
        {
            case CameraType.FollowPlayer:
                targetVector += new Vector3(PlayerManager.PlayerGameObject.transform.forward.x, 0, 0) * 3f;
                break;
            case CameraType.StaticFollowPlayer:

                Vector3 relativePos = PlayerManager.PlayerGameObject.transform.position - transform.position;
                lookAtRotation = Quaternion.LookRotation(relativePos);

                if (!isStarted) Camera.main.transform.rotation = lookAtRotation;
                break;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref currentVelocity, smoothSpeed);
    }

    public static void SetCameraTarget(Transform target, CameraType type = CameraType.StaticFollowPlayer)
    {
        CameraTarget = target;
        cameraType = type;

        oldRotation = Camera.main.transform.rotation;
        nextRotation = lookAtRotation;

        isStarted = true;
    }

    public static void ResetCameraTarget()
    {
        CameraTarget = staticDefaultCameraTarget;
        cameraType = CameraType.FollowPlayer;
        
        oldRotation = Camera.main.transform.rotation;
        nextRotation = defaultCameraRotation;

        isStarted = true;
    }

    private void Update()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = CameraTarget.position + offset;
    }
}
