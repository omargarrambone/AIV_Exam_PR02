using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraFollow : MonoBehaviour
{
    private Transform ActualCameraTarget;
    private Quaternion defaultCameraRotation;
    private Vector3 smoothDampVelocity;

    [SerializeField] private CameraType cameraType;

    [Header("Follow Player Variables")]
    [SerializeField] private Transform defaultCameraTarget;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 defaultCameraOffset, cameraForwardOffset;
    [SerializeField] private float playerForwardDistance;


    [Header("Change Forward Camera")]
    [SerializeField] bool isChanging;
    [SerializeField] float forwardTimer, forwardCounter;
    [SerializeField] int lastDirection;

    [Header("Diablo Camera Variables")]
    [SerializeField] Vector3 diabloCameraPositionOffset;
    [SerializeField] Vector3 diabloCameraRotationOffset;

    [Header("Lerp Variables")]
    [SerializeField] bool hasStartedLerping;
    [SerializeField] float lerpTimer, lerpCounter;
    [SerializeField] Quaternion oldRotation, nextRotation;
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

        lerpCounter += Time.deltaTime;
        Quaternion value = Quaternion.Lerp(oldRotation, nextRotation, lerpCounter / lerpTimer);
        Camera.main.transform.rotation = value;

        if (lerpCounter > lerpTimer) { hasStartedLerping = false; lerpCounter = 0; Camera.main.transform.rotation = nextRotation; }
    }

    private void CameraMovement()
    {
        Vector3 newCameraPosition = ActualCameraTarget.position + defaultCameraOffset;

        switch (cameraType)
        {
            case CameraType.FollowPlayer:
                FollowPlayer(newCameraPosition);
                break;

            case CameraType.LookAtPlayer:
                LookAtPlayer(newCameraPosition);
                break;

            case CameraType.DiabloCamera:
                DiabloCamera(newCameraPosition);
                break;
        }
    }


    private void DiabloCamera(Vector3 newCameraPosition)
    {
        nextRotation = Quaternion.Euler(diabloCameraRotationOffset);
        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + diabloCameraPositionOffset, ref smoothDampVelocity, smoothSpeed);
    }

    private void FollowPlayer(Vector3 newCameraPosition)
    {
        int newDirection = Mathf.RoundToInt(ActualCameraTarget.forward.x);

        if (newDirection != lastDirection)
        {
            lastDirection = newDirection;
            forwardCounter = forwardTimer;
            isChanging = true;
        }

        if (isChanging)
        {
            forwardCounter -= Time.deltaTime;

            if (forwardCounter < 0)
            {
                cameraForwardOffset = new Vector3(Mathf.Sign(ActualCameraTarget.forward.x), 0, 0) * playerForwardDistance;
                isChanging = false;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + cameraForwardOffset, ref smoothDampVelocity, smoothSpeed);
    }

    private void LookAtPlayer(Vector3 newCameraPosition)
    {
        Vector3 relativePos = PlayerManager.PlayerGameObject.transform.position - transform.position;
        nextRotation = Quaternion.LookRotation(relativePos);

        nextRotation = Quaternion.Slerp(Camera.main.transform.rotation, nextRotation, Time.deltaTime * slerpSpeedRotation);

        if (!hasStartedLerping) Camera.main.transform.rotation = nextRotation;

        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref smoothDampVelocity, smoothSpeed);
    }

    public void SetCameraTarget(Transform target, CameraType type = CameraType.LookAtPlayer)
    {
        ActualCameraTarget = target;
        cameraType = type;

        Vector3 relativePos = PlayerManager.PlayerGameObject.transform.position - transform.position;
        nextRotation = Quaternion.LookRotation(relativePos);

        SetNewRotations(nextRotation);
    }

    public void ResetCameraTarget()
    {
        ActualCameraTarget = defaultCameraTarget;
        cameraType = CameraType.FollowPlayer;

        SetNewRotations(defaultCameraRotation);
    }

    void SetNewRotations(Quaternion nextQuaternion)
    {
        lerpCounter = 0;
        oldRotation = Camera.main.transform.rotation;
        nextRotation = nextQuaternion;
        hasStartedLerping = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        //ResetCameraTarget();
        transform.position = ActualCameraTarget.position;
        //Debug.Break();
        //transform.position = ActualCameraTarget.position + defaultCameraOffset;
    }
}
