using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    float impulseForce=30f;
    Collider[] colliders;
    Rigidbody[] rigidbodies;
    [SerializeField] Collider headCollider;
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRagdoll(false);
    }

    public void SetRagdoll(bool value)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = value;

        }

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !value;

            if (value) rigidbodies[i].AddForce((transform.position - PlayerManager.PlayerGameObject.transform.position).normalized * impulseForce, ForceMode.Impulse);
        }

        if (value)
        {
            transform.localPosition = new Vector3(0f, 1f, 0f);
        }

        if (headCollider != null ) headCollider.enabled = !value;
    }

    [ContextMenu("Enable")]
    public void EnableRagdoll()
    {
        SetRagdoll(true);
    }

    [ContextMenu("Disable")]
    public void DisableRagdoll()
    {
        SetRagdoll(false);
    }
}
