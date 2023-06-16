using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private SceneMngr sceneMngr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            TeleportPortal portal = other.GetComponent<TeleportPortal>();

            sceneMngr.PlayerPositionInNextScene = portal.NextPosition;
            //sceneMngr.PlayerRotationInNextScene = Quaternion.Euler(portal.NextRotation);
            sceneMngr.PlayerRotationInNextScene = PlayerManager.PlayerGameObject.transform.rotation;
            sceneMngr.ChangeScene(portal.NextScene);
        }
    }
}
