using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public PlayerData playerData;
    public TownData townData;
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public float currentHealth;
    public string currentScene;
    public int currentWeapon;
    public bool[] takenItems;
}

[System.Serializable]
public struct TownData
{
    public int enemiesKilled;
    public int enemiesPurified;
    public int unlockedLevel;
}