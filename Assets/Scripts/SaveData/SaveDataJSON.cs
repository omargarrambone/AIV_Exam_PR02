using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveDataJSON : MonoBehaviour
{
    public static SaveData SavedData { get; private set; }

    private HealthManager healthManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private SceneMngr sceneManager;

    [SerializeField] private SaveData _savedData;
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
        _savedData = new SaveData();
        _savedData.playerData = new PlayerData();
        _savedData.townData = new TownData();
    }

    [ContextMenu("Save Game")]
    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(persistentPath))
        {
            // SAVE VALUES
            _savedData.playerData.currentHealth = healthManager.CurrentHealth;
            _savedData.playerData.playerPos = PlayerManager.PlayerGameObject.transform.position;
            _savedData.playerData.playerRot = PlayerManager.PlayerGameObject.transform.rotation;
            _savedData.playerData.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            _savedData.playerData.inventoryItems = InventoryManager.InventoryItems;
            _savedData.playerData.currentWeapon = InventoryManager.CurrentSlotIndex;
            _savedData.townData.enemiesPurified = FluteScript.PurifiedEnemies;
            _savedData.townData.enemiesKilled = FluteScript.KilledEnemies;

            // SAVE JSON
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
                // LOAD JSON
                string json = reader.ReadToEnd();

                if (json == "") return;

                SaveData savedData = JsonUtility.FromJson<SaveData>(json);
                _savedData = savedData;
                SavedData = _savedData;

                // LOAD VALUES
                healthManager.CurrentHealth = _savedData.playerData.currentHealth;
                PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(SavedData.playerData.playerPos, SavedData.playerData.playerRot);
                sceneManager.NextScene = _savedData.playerData.currentScene;
                sceneManager.PlayerPositionInNextScene = _savedData.playerData.playerPos;
                sceneManager.PlayerRotationInNextScene =_savedData.playerData.playerRot;
                //FluteScript.PurifiedEnemies = _savedData.townData.enemiesPurified;
                //FluteScript.KilledEnemies = _savedData.townData.enemiesKilled;

            }

            OnLoad.Invoke();

            Debug.Log("Loaded Game!");
        }
    }
}
