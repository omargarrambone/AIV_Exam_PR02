using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockWall : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    private void OnTriggerEnter(Collider other)
    {
        CameraFollow.SetCameraTarget(cameraTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        CameraFollow.ResetCameraTarget();
    }
}
