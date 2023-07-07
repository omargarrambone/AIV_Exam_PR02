using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    public string NextScene;
    public Vector3 PlayerNextPosition;

    public Vector3 CameraNextPosition;
    public Vector3 CameraNextRotation;

    [SerializeField] protected BoxCollider CameraLockWallCollider;

    protected virtual void OnTriggerEnter(Collider other)
    {
        CameraLockWallCollider.enabled = false;
        CameraLockWallCollider.gameObject.SetActive(false);
    }
}
