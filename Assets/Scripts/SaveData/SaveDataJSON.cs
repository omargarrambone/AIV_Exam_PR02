using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveDataJSON : MonoBehaviour
{
    public static SaveData SavedData { get; private set; }

    private HealthManager healthManager;
    [SerializeField] private WeaponsManager weaponsManager;
    [SerializeField] private VolumeManager volumeManager;
    [SerializeField] private SceneMngr sceneManager;
    [SerializeField] private FluteScript fluteScript;
    [SerializeField] private Slider masterSlider, sfxSlider, musicSlider;
    [SerializeField] private SaveData savedData;
    [SerializeField] private UnityEvent OnSave, OnLoad;
    static public string PersistentPath = "";

    void Start()
    {
        SetPaths();
        //InitData();
        healthManager = PlayerManager.PlayerGameObject.GetComponent<HealthManager>();

        if (!DoesSavedDataExist())
        {
            CreateDefaultSaveData();
        }

            LoadData();
    }

    public static void SetPaths()
    {
        PersistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    private void InitData()
    {
        savedData = new SaveData();
        savedData.playerData = new PlayerData();
        savedData.worldData = new WorldData();
    }

    [ContextMenu("Save Game")]
    public void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(PersistentPath))
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

            savedData.worldData.masterVolume = volumeManager.MasterVolume;
            savedData.worldData.musicVolume = volumeManager.MusicVolume;
            savedData.worldData.sfxVolume = volumeManager.SFXvolume;

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
            using (StreamReader reader = new StreamReader(PersistentPath))
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
            using (StreamReader reader = new StreamReader(PersistentPath))
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

                volumeManager.SetMasterAudio(savedData.worldData.masterVolume);
                volumeManager.SetMusicAudio(savedData.worldData.musicVolume);
                volumeManager.SetSFXAudio(savedData.worldData.sfxVolume);

                SetAudioVolumes(volumeManager.MasterVolume, volumeManager.MusicVolume, volumeManager.SFXvolume);

                masterSlider.value = volumeManager.MasterVolume;
                musicSlider.value = volumeManager.MusicVolume;
                sfxSlider.value = volumeManager.SFXvolume;

                //LOAD SCENE

                sceneManager.ChangeScene(sceneManager.NextScene);
                PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(SavedData.playerData.playerPos, SavedData.playerData.playerRot);
            }

            OnLoad.Invoke();
        }
    }

    public void CreateDefaultSaveData()
    {
        using (StreamWriter writer = new StreamWriter(PersistentPath))
        {
            // SAVE VALUES
            savedData.playerData.currentHealth = 100;
            savedData.playerData.playerPos = new Vector3(-64, 2.22016072f, -30);
            savedData.playerData.playerRot = Quaternion.identity;
            savedData.playerData.currentScene = "Caltanissetta";
            savedData.playerData.takenItems = weaponsManager.TakenWeapons;
            savedData.playerData.currentWeapon = 0;

            savedData.worldData.enemiesPurified = 0;
            savedData.worldData.enemiesKilled = 0;
            savedData.worldData.arenasCompleted = new bool[ArenaScript.MaxArenas];

            savedData.worldData.masterVolume = SavedData.worldData.masterVolume;
            savedData.worldData.musicVolume = SavedData.worldData.musicVolume;
            savedData.worldData.sfxVolume = SavedData.worldData.sfxVolume;

            volumeManager.SetMasterAudio(savedData.worldData.masterVolume);
            volumeManager.SetMusicAudio(savedData.worldData.musicVolume);
            volumeManager.SetSFXAudio(savedData.worldData.sfxVolume);

            // SAVE JSON
            string json = JsonUtility.ToJson(savedData);
            writer.Write(json);
        }

        OnSave.Invoke();
    }

    public static void SetAudioVolumes(float master, float music, float sfx)
    {
        SaveData saveData = SavedData;

        saveData.worldData.masterVolume = master;
        saveData.worldData.musicVolume = music;
        saveData.worldData.sfxVolume = sfx;

        SavedData = saveData;
    }
}
