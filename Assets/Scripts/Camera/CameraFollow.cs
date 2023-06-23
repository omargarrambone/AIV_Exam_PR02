using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform actualCameraTarget;
    public Quaternion DefaultCameraRotation;
    private Vector3 smoothDampVelocity = Vector3.zero;

    [SerializeField] private CameraType cameraType;

    [Header("Follow Player Variables")]
    [SerializeField] private Transform defaultCameraTarget;
    [SerializeField] private float smoothSpeed;
    public Vector3 DefaultCameraOffset, CameraForwardOffset;
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
    public Quaternion OldRotation, NextRotation;
    [SerializeField] private float slerpSpeedRotation;

    static public Vector3 CameraPositionOnChangeScene;
    static public Vector3 CameraRotationOnChangeScene;
    static public CameraType ResetCameraType;

    public Vector3 _CameraRotationOnChangeScene;
    public Vector3 _CameraPositionOnChangeScene;

    private void Awake()
    {
        //get player as target
        actualCameraTarget = defaultCameraTarget;

        //cool inquadratura
        CameraRotationOnChangeScene = new Vector3(16f, 0f, 0f);
        DefaultCameraRotation = Quaternion.Euler(CameraRotationOnChangeScene);
        Camera.main.transform.rotation = DefaultCameraRotation;
        OldRotation = Camera.main.transform.rotation;

        if (CameraPositionOnChangeScene == Vector3.zero) CameraPositionOnChangeScene = SaveDataJSON.SavedData.playerData.playerPos + DefaultCameraOffset;
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
        Quaternion value = Quaternion.Lerp(OldRotation, NextRotation, lerpCounter / lerpTimer);
        Camera.main.transform.rotation = value;

        if (lerpCounter > lerpTimer) { hasStartedLerping = false; lerpCounter = 0; Camera.main.transform.rotation = NextRotation; }
    }

    private void CameraMovement()
    {
        if (!actualCameraTarget) return;

        Vector3 newCameraPosition = actualCameraTarget.position + DefaultCameraOffset;

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
        NextRotation = Quaternion.Euler(diabloCameraRotationOffset);
        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + diabloCameraPositionOffset, ref smoothDampVelocity, smoothSpeed);
    }

    private void FollowPlayer(Vector3 newCameraPosition)
    {
        int newDirection = Mathf.RoundToInt(defaultCameraTarget.forward.x);

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
                CameraForwardOffset = new Vector3(Mathf.Sign(defaultCameraTarget.forward.x), 0, 0) * playerForwardDistance;
                isChanging = false;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + CameraForwardOffset, ref smoothDampVelocity, smoothSpeed);
    }

    private void LookAtPlayer(Vector3 newCameraPosition)
    {
        Vector3 relativePos = defaultCameraTarget.position - transform.position;

        NextRotation = Quaternion.LookRotation(relativePos);
        NextRotation = Quaternion.Slerp(Camera.main.transform.rotation, NextRotation, Time.deltaTime * slerpSpeedRotation);
        Camera.main.transform.rotation = NextRotation;

        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref smoothDampVelocity, smoothSpeed);
    }

    public void SetCameraTarget(Transform target, CameraType type = CameraType.LookAtPlayer)
    {
        actualCameraTarget = target;
        cameraType = type;

        Vector3 relativePos = defaultCameraTarget.position - transform.position;
        NextRotation = Quaternion.LookRotation(relativePos);


        SetNewRotations(NextRotation);
    }

    public void ResetCameraTarget()
    {
        cameraType = CameraType.FollowPlayer;
        actualCameraTarget = defaultCameraTarget;

        SetNewRotations(DefaultCameraRotation);
    }

    void SetNewRotations(Quaternion nextQuaternion)
    {
        lerpCounter = 0;
        OldRotation = Camera.main.transform.rotation;
        NextRotation = nextQuaternion;
        hasStartedLerping = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = CameraPositionOnChangeScene;
        transform.rotation = Quaternion.Euler(CameraRotationOnChangeScene);

    }
}
