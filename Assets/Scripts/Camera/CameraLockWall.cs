using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraLockWall : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private UnityEvent OnEnter, OnExit;
    [SerializeField] private bool isTargetPlayer;
    [SerializeField] private CameraType cameraType=CameraType.LookAtPlayer;
    static private CameraFollow cameraFollow;


    private void Start()
    {
        if (!cameraFollow)
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CallOnEnter();
    }

    public void CallOnEnter()
    {
        if (isTargetPlayer)
        {
            cameraFollow.SetCameraTarget(PlayerManager.PlayerGameObject.transform, cameraType);
        }
        else cameraFollow.SetCameraTarget(cameraTarget, cameraType);

        OnEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CallOnExit();
    }

    public void CallOnExit()
    {
        cameraFollow.ResetCameraTarget();
        OnExit.Invoke();
    }
}
