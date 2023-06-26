using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKilled : MonoBehaviour
{
    public void AddEnemyToEnemiesKilled()
    {
        NPCCounter.AddKilledNPCToCounter();
    }
}
