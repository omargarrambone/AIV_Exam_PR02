using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData playerData;
    public TownData townData;
}

[System.Serializable]
public class PlayerData
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public float health;
    public int currentWeapon;
    public InventorySlot[] inventoryItems;
}

[System.Serializable]
public class TownData
{
    public int enemiesKilled;
    public int unlockedLevel;
}