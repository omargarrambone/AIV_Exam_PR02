using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraFollow : MonoBehaviour
{
    private Transform ActualCameraTarget;
    private Quaternion defaultCameraRotation;

    [SerializeField] private CameraType cameraType;

    [Header("Follow Player Variables")]
    [SerializeField] private Transform defaultCameraTarget;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 defaultCameraOffset, cameraForwardOffset;
    [SerializeField] private float maxDistanceFromCamera;
    [SerializeField] private float playerForwardDistance;

    private Vector3 smoothDampVelocity;

    [Header("Lerp Variables")]
    [SerializeField] bool hasStartedLerping;
    [SerializeField] float timer, counter;
    static Quaternion oldRotation, nextRotation, lookAtRotation;
    [SerializeField] private float slerpSpeedRotation;

    private void Awake()
    {
        ActualCameraTarget = defaultCameraTarget;

        defaultCameraRotation = Quaternion.Euler(new Vector3(16f,0f,0f));

        oldRotation = defaultCameraRotation;
        Camera.main.transform.rotation = defaultCameraRotation;
    }

    private void Update()
    {
        CameraMovement();
        LerpCamera();
    }

    void LerpCamera()
    {
        if (!hasStartedLerping) return;

        counter += Time.deltaTime;
        Quaternion value = Quaternion.Lerp(oldRotation, nextRotation, counter / timer);
        Camera.main.transform.rotation = value;

        if (counter > timer) { hasStartedLerping = false; counter = 0; Camera.main.transform.rotation = nextRotation; }
    }

    private void CameraMovement()
    {
        Vector3 cameraPosition = ActualCameraTarget.position + defaultCameraOffset;

        switch (cameraType)
        {
            case CameraType.FollowPlayer:
                Vector3 cameraNoOffsetPosition = new Vector3(transform.position.x - cameraForwardOffset.x, ActualCameraTarget.position.y, ActualCameraTarget.position.z);
                Vector3 newTarget = new Vector3(ActualCameraTarget.position.x, ActualCameraTarget.position.y, ActualCameraTarget.position.z);

                if (Vector3.Distance(cameraNoOffsetPosition, newTarget) > maxDistanceFromCamera)
                {
                    cameraForwardOffset = new Vector3(Mathf.Sign(ActualCameraTarget.forward.x), 0, 0) * playerForwardDistance;
                }

                transform.position = Vector3.SmoothDamp(transform.position, cameraPosition + cameraForwardOffset, ref smoothDampVelocity, smoothSpeed);

                break;

            case CameraType.LookAtPlayer:
                Vector3 relativePos = PlayerManager.PlayerGameObject.transform.position - transform.position;
                lookAtRotation = Quaternion.LookRotation(relativePos);

                lookAtRotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAtRotation, Time.deltaTime * slerpSpeedRotation);

                nextRotation = lookAtRotation;
                if (!hasStartedLerping) Camera.main.transform.rotation = lookAtRotation;

                transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref smoothDampVelocity, smoothSpeed);
                break;
        }

    }

    public void SetCameraTarget(Transform target, CameraType type = CameraType.LookAtPlayer)
    {
        ActualCameraTarget = target;
        cameraType = type;
        
        SetNewRotations(lookAtRotation);
    }

    public void ResetCameraTarget()
    {
        ActualCameraTarget = defaultCameraTarget;
        cameraType = CameraType.FollowPlayer;

        SetNewRotations(defaultCameraRotation);
    }

    void SetNewRotations(Quaternion nextQuaternion)
    {
        counter = 0;
        oldRotation = Camera.main.transform.rotation;
        nextRotation = nextQuaternion;
        hasStartedLerping = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        ResetCameraTarget();
        transform.position = ActualCameraTarget.position + defaultCameraOffset;
    }
}
