using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform actualCameraTarget;
    public Quaternion DefaultCameraRotation;
    private Vector3 smoothDampVelocity = Vector3.zero;

    [SerializeField] private CameraType cameraType;

    [Header("Follow Player Variables")]
    [SerializeField] private Transform defaultCameraTarget;
    [SerializeField] private float smoothSpeed, lerpSpeed;
    public Vector3 DefaultCameraOffset, CameraForwardOffset;
    [SerializeField] private float playerForwardDistance;

    [Header("Change Forward Camera")]
    [SerializeField] bool isChanging;
    [SerializeField] float forwardTimer, forwardCounter;
    [SerializeField] int lastDirection;

    [Header("Diablo Camera Variables")]
    [SerializeField] Vector3 diabloCameraPositionOffset;
    [SerializeField] Vector3 diabloCameraRotationOffset;

    [Header("Diablo Camera Slums Variables")]
    [SerializeField] Vector3 diabloCameraSlumsPositionOffset;
    [SerializeField] Vector3 diabloCameraSlumsRotationOffset;

    [Header("Maze Camera Variables")]
    [SerializeField] Vector3 mazeCameraPositionOffset;
    [SerializeField] Vector3 mazeCameraRotationOffset;

    [Header("Saving Camera Variables")]
    [SerializeField] Vector3 savingCameraPositionOffset;
    [SerializeField] Vector3 savingCameraRotationOffset;

    [Header("MinionsPhaseBossBattleCamera Camera Variables")]
    [SerializeField] Vector3 minionsPhaseBossBattleCameraPositionOffset;
    [SerializeField] Vector3 minionsPhaseBossBattleCameraRotationOffset;

    [Header("Lerp Variables")]
    [SerializeField] bool hasStartedLerping;
    [SerializeField] float lerpTimer, lerpCounter;
    public Quaternion OldRotation, NextRotation;
    [SerializeField] private float slerpSpeedRotation;

    static public Vector3 CameraPositionOnChangeScene;
    static public Vector3 CameraRotationOnChangeScene;
    static public CameraType ResetCameraType;

    private void Awake()
    {
        //get player as target
        actualCameraTarget = defaultCameraTarget;

        Camera.main.transform.rotation = DefaultCameraRotation;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnNextSceneLoad;

    }

    private void OnNextSceneLoad(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        transform.position = CameraPositionOnChangeScene;
        transform.rotation = Quaternion.Euler(CameraRotationOnChangeScene);
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnNextSceneLoad;
    }

    public void LoadCameraPosition()
    {
        CameraRotationOnChangeScene = DefaultCameraRotation.eulerAngles;
        CameraForwardOffset = new Vector3(Mathf.Sign(defaultCameraTarget.forward.x), 0, 0) * playerForwardDistance;

        CameraPositionOnChangeScene = SaveDataJSON.SavedData.playerData.playerPos + CameraForwardOffset + DefaultCameraOffset + new Vector3(0,2,0);

        ResetCameraTarget();
        OldRotation = DefaultCameraRotation;
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
                SimpleCameraWithOffset(diabloCameraRotationOffset, diabloCameraPositionOffset,newCameraPosition);
                break;

            case CameraType.DiabloCameraSlums:
                SimpleCameraWithOffset(diabloCameraSlumsRotationOffset, diabloCameraSlumsPositionOffset,newCameraPosition);
                break;

            case CameraType.MazeCamera:
                SimpleCameraWithOffset(mazeCameraRotationOffset, mazeCameraPositionOffset, actualCameraTarget.position);
                break;

            case CameraType.SavingCamera:
                SimpleCameraWithOffset(savingCameraRotationOffset, savingCameraPositionOffset,newCameraPosition);
                break;

            case CameraType.MinionsPhaseBossBattleCamera:
                SimpleCameraWithOffset(minionsPhaseBossBattleCameraRotationOffset,minionsPhaseBossBattleCameraPositionOffset,newCameraPosition);
                break;

        }
    }

    private void SimpleCameraWithOffset(Vector3 rotation, Vector3 offset,Vector3 newCameraPosition)
    {
        NextRotation = Quaternion.Euler(rotation);
        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + offset, ref smoothDampVelocity, smoothSpeed);
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

        //transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition + CameraForwardOffset, ref smoothDampVelocity, smoothSpeed);
        transform.position = Vector3.Lerp(transform.position, newCameraPosition + CameraForwardOffset, Time.deltaTime * smoothSpeed * lerpSpeed);
    }

    private void LookAtPlayer(Vector3 newCameraPosition)
    {
        Vector3 relativePos = defaultCameraTarget.position - transform.position;

        NextRotation = Quaternion.LookRotation(relativePos);
        NextRotation = Quaternion.Slerp(Camera.main.transform.rotation, NextRotation, Time.deltaTime * slerpSpeedRotation);
        Camera.main.transform.rotation = NextRotation;

        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref smoothDampVelocity, smoothSpeed);
    }

    public void SetCameraTarget(Transform target = null, CameraType type = CameraType.LookAtPlayer)
    {
        if(target != null)
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
}
