using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockWall : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    static private CameraFollow cameraFollow;

    private void Start()
    {
        if (!cameraFollow)
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
            Debug.Log(cameraFollow);
        }
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
