using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Collider[] colliders;
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();

        SetRagdollColliders(false);
    }

    public void SetRagdollColliders(bool value)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = value;
        }
    }

    [ContextMenu("Enable")]
    public void EnableRagdoll()
    {
        SetRagdollColliders(true);
    }

    [ContextMenu("Disable")]
    public void DisableRagdoll()
    {
        SetRagdollColliders(false);
    }
}
