using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    public string NextScene;
    public Vector3 PlayerNextPosition;

    public Vector3 CameraNextPosition;
    public Vector3 CameraNextRotation;



    [SerializeField] private BoxCollider CameraLockWallCollider;

    private void OnTriggerEnter(Collider other)
    {
        CameraLockWallCollider.enabled = false;
        CameraLockWallCollider.gameObject.SetActive(false);
    }

    public void Teleport()
    {
        TeleportPortal portal = GetComponent<TeleportPortal>();
        PlayerTeleporter.sceneMngr.PlayerPositionInNextScene = portal.PlayerNextPosition;
        PlayerTeleporter.sceneMngr.PlayerRotationInNextScene = PlayerManager.PlayerGameObject.transform.rotation;

        PlayerTeleporter.sceneMngr.ChangeScene(portal.NextScene);

        CameraFollow.CameraPositionOnChangeScene = portal.CameraNextPosition;
        CameraFollow.CameraRotationOnChangeScene = portal.CameraNextRotation;

    }
}
