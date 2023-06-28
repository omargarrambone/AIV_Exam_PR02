using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickScript : MonoBehaviour
{
    [SerializeField] private Collider kickCollider;

    public void CallOnStartKick()
    {
        kickCollider.enabled = true;
    }

    public void CallOnEndKick()
    {
        kickCollider.enabled = false;
        PlayerManager.EnablePlayerMovement();
    }
}
