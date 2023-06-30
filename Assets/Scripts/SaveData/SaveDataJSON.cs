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
    static public string persistentPath = "";

    void Start()
    {
        SetPaths();
        healthManager = PlayerManager.PlayerGameObject.GetComponent<HealthManager>();

        if (!DoesSavedDataExist())
        {
            CreateDefaultSaveData();
        }

            LoadData();

    }

    static void DeleteSave()
    {
        //SavedData = null;
    }

    [ContextMenu("SetPaths")]
    private void SetPaths()
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        savedData = new SaveData();
        savedData.playerData = new PlayerData();
        savedData.worldData = new WorldData();
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
            savedData.worldData.enemiesPurified = NPCManager.PurifiedEnemies;
            savedData.worldData.enemiesKilled = NPCManager.KilledEnemies;
            savedData.worldData.arenasCompleted = ArenaScript.CompletedArenas;

            // SAVE JSON
            string json = JsonUtility.ToJson(savedData);
            writer.Write(json);
        }

        OnSave.Invoke();

        Debug.Log("Saved Game!");
    }

    public static bool DoesSavedDataExist()
    {
        string json;

        try
        {
            using (StreamReader reader = new StreamReader(persistentPath))
            {
                json = reader.ReadToEnd();
            }
        }
        catch
        {
            return false;
        }

        if (json == null || json == "")
        {
            return false;
        }

        return true;
    }

    [ContextMenu("Load Game")]
    public void LoadData()
    {
        if (DoesSavedDataExist())
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
                sceneManager.NextScene = savedData.playerData.currentScene;
                sceneManager.PlayerPositionInNextScene = savedData.playerData.playerPos;
                sceneManager.PlayerRotationInNextScene = savedData.playerData.playerRot;
                weaponsManager.SetInventory(savedData.playerData.takenItems);
                weaponsManager.SetActualItem(savedData.playerData.currentWeapon);
                NPCManager.PurifiedEnemies = savedData.worldData.enemiesPurified;
                NPCManager.KilledEnemies = savedData.worldData.enemiesKilled;
                ArenaScript.CompletedArenas = savedData.worldData.arenasCompleted;

                //LOAD SCENE

                sceneManager.ChangeScene(sceneManager.NextScene);
                PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(SavedData.playerData.playerPos, SavedData.playerData.playerRot);
            }

            OnLoad.Invoke();

            Debug.Log("Loaded Game!");
        }
    }

    public void CreateDefaultSaveData()
    {
        using (StreamWriter writer = new StreamWriter(persistentPath))
        {
            // SAVE VALUES
            savedData.playerData.currentHealth = 100;
            savedData.playerData.playerPos = new Vector3(-64f, 2.272f, -30f);
            savedData.playerData.playerRot = Quaternion.identity;
            savedData.playerData.currentScene = "HubBeta";
            savedData.playerData.takenItems = weaponsManager.TakenWeapons;
            savedData.playerData.currentWeapon = 0;
            savedData.worldData.enemiesPurified = 0;
            savedData.worldData.enemiesKilled = 0;
            savedData.worldData.arenasCompleted = new bool[ArenaScript.MaxArenas];

            // SAVE JSON
            string json = JsonUtility.ToJson(savedData);
            writer.Write(json);
        }

        OnSave.Invoke();

        Debug.Log("Saved Game!");
    }
}
