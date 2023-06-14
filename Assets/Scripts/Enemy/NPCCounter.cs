using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCounter : MonoBehaviour
{
    public void ResetCounters()
    {
        NPCSpawner.KilledEnemies = 0;
        NPCSpawner.PurifiedEnemies = 0;
    }

    public void AddKilledNPCToCounter()
    {
        NPCSpawner.KilledEnemies++;
    }

    public void AddPurifiedNPCToCounter()
    {
        NPCSpawner.PurifiedEnemies++;
    }
}
