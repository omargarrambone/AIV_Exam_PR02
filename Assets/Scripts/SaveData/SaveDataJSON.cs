using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveDataJSON : MonoBehaviour
{
    public static SaveData SavedData { get; private set; }

    private HealthManager healthManager;
    [SerializeField] private WeaponsManager weaponsManager;
    [SerializeField] private SceneMngr sceneManager;
    [SerializeField] private FluteScript fluteScript;
    [SerializeField] private SaveData savedData;
    [SerializeField] private UnityEvent OnSave, OnLoad;
    private string persistentPath = "";

    void Start()
    {
        SetPaths();
        healthManager = PlayerManager.PlayerGameObject.GetComponent<HealthManager>();


        LoadData();
    }

    [ContextMenu("SetPaths")]
    private void SetPaths()
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        savedData = new SaveData();
        savedData.playerData = new PlayerData();
        savedData.townData = new TownData();
    }

    [ContextMenu("Save Game")]
    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(persistentPath))
        {
            // SAVE VALUES
            savedData.playerData.currentHealth = healthManager.CurrentHealth;
            savedData.playerData.playerPos = PlayerManager.PlayerGameObject.transform.position;
            savedData.playerData.playerRot = PlayerManager.PlayerGameObject.transform.rotation;
            savedData.playerData.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            savedData.playerData.takenItems = weaponsManager.TakenWeapons;
            savedData.playerData.currentWeapon = weaponsManager.CurrentSlotIndex;
            savedData.townData.enemiesPurified = NPCSpawner.PurifiedEnemies;
            savedData.townData.enemiesKilled = NPCSpawner.KilledEnemies;

            // SAVE JSON
            string json = JsonUtility.ToJson(savedData);
            writer.Write(json);
        }

        OnSave.Invoke();

        Debug.Log("Saved Game!");
    }

    public bool DoesSavesExist()
    {
       return File.Exists(persistentPath);
    }

    [ContextMenu("Load Game")]
    public void LoadData()
    {
        if (DoesSavesExist())
        {
            using (StreamReader reader = new StreamReader(persistentPath))
            {
                // LOAD JSON
                string json = reader.ReadToEnd();

                if (json == "") return;

                SaveData savedDataLoadedFromJson = JsonUtility.FromJson<SaveData>(json);
                savedData = savedDataLoadedFromJson;
                SavedData = savedData;

                // LOAD VALUES
                healthManager.CurrentHealth = savedData.playerData.currentHealth;
                PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(SavedData.playerData.playerPos, SavedData.playerData.playerRot);
                sceneManager.NextScene = savedData.playerData.currentScene;
                sceneManager.PlayerPositionInNextScene = savedData.playerData.playerPos;
                sceneManager.PlayerRotationInNextScene = savedData.playerData.playerRot;
                weaponsManager.SetInventory(savedData.playerData.takenItems);
                weaponsManager.SetActualItem(savedData.playerData.currentWeapon);
                NPCSpawner.PurifiedEnemies = savedData.townData.enemiesPurified;
                NPCSpawner.KilledEnemies = savedData.townData.enemiesKilled;

                sceneManager.ChangeScene(sceneManager.NextScene);
            }

            OnLoad.Invoke();

            Debug.Log("Loaded Game!");
        }
    }
}
