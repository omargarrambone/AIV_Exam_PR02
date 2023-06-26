using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCCounter
{
    static public Action OnKilledEnemy, OnPurifiedEnemy, OnFinishedEnemy;

    static NPCCounter()
    {
        OnKilledEnemy = new Action( () => { } );
        OnPurifiedEnemy = new Action( () => { } );
        OnFinishedEnemy = new Action( () => { } );

    }

    public static void ResetCounters()
    {
        NPCManager.KilledEnemies = 0;
        NPCManager.PurifiedEnemies = 0;
    }

    public static void AddKilledNPCToCounter()
    {
        NPCManager.KilledEnemies++;
        OnKilledEnemy.Invoke();
        OnFinishedEnemy.Invoke();
    }

    public static void AddPurifiedNPCToCounter()
    {
        NPCManager.PurifiedEnemies++;
        OnPurifiedEnemy.Invoke();
        OnFinishedEnemy.Invoke();
    }
}
