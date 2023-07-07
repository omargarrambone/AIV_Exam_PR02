using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKilled : MonoBehaviour
{
    [SerializeField] Rigidbody ragdollRb;


    public void AddEnemyToEnemiesKilled()
    {
        NPCCounter.AddKilledNPCToCounter();
        ragdollRb.maxLinearVelocity = 1f;
        ragdollRb.angularVelocity = Vector3.zero;
        ragdollRb.velocity = Vector3.zero;
        ragdollRb.transform.position = transform.position;
    }
}
