using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTeleportPortal : TeleportPortal
{
    public void Teleport()
    {
        CameraLockWallCollider.enabled = false;
        CameraLockWallCollider.gameObject.SetActive(false);

        TeleportPortal portal = GetComponent<TeleportPortal>();
        PlayerTeleporter.SceneMngr.PlayerPositionInNextScene = portal.PlayerNextPosition;
        PlayerTeleporter.SceneMngr.PlayerRotationInNextScene = PlayerManager.PlayerGameObject.transform.rotation;

        PlayerTeleporter.SceneMngr.ChangeScene(portal.NextScene);

        CameraFollow.CameraPositionOnChangeScene = portal.CameraNextPosition;
        CameraFollow.CameraRotationOnChangeScene = portal.CameraNextRotation;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
}
