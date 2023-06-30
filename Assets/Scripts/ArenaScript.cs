using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{
    public int ArenaIndex;
    static public int MaxArenas = 10;
    static public bool[] CompletedArenas = new bool[MaxArenas];

    [SerializeField] private GameObject[] fogWalls;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int enemiesToKillInThisArena;
    [SerializeField] private int lastEnemiesKilled;
    [SerializeField] private bool isStarted;

    private void Start()
    {
        if (CompletedArenas[ArenaIndex] == true) OnFinish();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isStarted) return;

        if (!other.CompareTag("Player")) return;

        for (int i = 0; i < fogWalls.Length; i++)
        {
            fogWalls[i].SetActive(true);
        }

        lastEnemiesKilled = NPCManager.FinishedEnemies;

        isStarted = true;

        NPCCounter.OnFinishedEnemy += CheckFinishedEnemies;
    }

    void CheckFinishedEnemies()
    {
        if(NPCManager.FinishedEnemies >= enemiesToKillInThisArena + lastEnemiesKilled)
        {
            CompletedArenas[ArenaIndex] = true;
            OnFinish();
        }
    }

    void OnFinish()
    {
        for (int i = 0; i < fogWalls.Length; i++)
        {
            fogWalls[i].SetActive(false);
        }

        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null) Destroy(enemies[i]);
            }
        }

        enabled = false;
        Destroy(gameObject);
    }

}
