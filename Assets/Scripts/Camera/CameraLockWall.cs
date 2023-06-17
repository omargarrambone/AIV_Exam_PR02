using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockWall : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CameraFollow cameraFollow;

    private void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        cameraFollow.SetCameraTarget(cameraTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        cameraFollow.ResetCameraTarget();
    }
}
