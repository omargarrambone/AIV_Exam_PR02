using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private SceneMngr sceneMngr;

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
        if (other.CompareTag("Portal"))
        {
            TeleportPortal portal = other.GetComponent<TeleportPortal>();
            sceneMngr.PlayerPositionInNextScene = portal.PlayerNextPosition;
            sceneMngr.PlayerRotationInNextScene = PlayerManager.PlayerGameObject.transform.rotation;

            sceneMngr.ChangeScene(portal.NextScene);

            CameraFollow.CameraPositionOnChangeScene = portal.CameraNextPosition;
            CameraFollow.CameraRotationOnChangeScene = portal.CameraNextRotation;
        }
    }
}
