using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveDataJSON : MonoBehaviour
{
    public static SaveData SavedData { get; private set; }

    [SerializeField] private HealthManager healthManager;
    [SerializeField] private Transform player;
    [SerializeField] private SaveData _savedData;
    [SerializeField] private UnityEvent OnSave, OnLoad;
    private string persistentPath = "";

    void Awake()
    {
        SetPaths();
    }

    [ContextMenu("SetPaths")]
    private void SetPaths()
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json"; 
    }

    [ContextMenu("Save Game")]
    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(persistentPath))
        {
            SavedData.playerData.inventoryItems = InventoryManager.InventoryItems;
            SavedData.playerData.currentWeapon = InventoryManager.CurrentSlotIndex;
            SavedData.playerData.health = healthManager.CurrentHealth;
            SavedData.playerData.playerPos = player.position;
            SavedData.playerData.playerRot = player.rotation;

            string json = JsonUtility.ToJson(_savedData);

            writer.Write(json);

        }

        OnSave.Invoke();

        Debug.Log("Saved Game!");
    }

    [ContextMenu("Load Game")]
    public void LoadData()
    {
        if (File.Exists(persistentPath))
        {
            using (StreamReader reader = new StreamReader(persistentPath))
            {
                string json = reader.ReadToEnd();

                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                _savedData = saveData;
                SavedData = _savedData;

                player.SetPositionAndRotation(SavedData.playerData.playerPos, SavedData.playerData.playerRot);

            }

            OnLoad.Invoke();

            Debug.Log("Loaded Game!");
        }
    }
}
