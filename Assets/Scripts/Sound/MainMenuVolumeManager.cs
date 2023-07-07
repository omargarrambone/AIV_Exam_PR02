using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuVolumeManager : MonoBehaviour
{
    [SerializeField] VolumeManager volumeManager;
    [SerializeField] private Slider masterSlider, sfxSlider, musicSlider;
    [SerializeField] private float defaultMasterVolume, defaultSFXVolume, defaultMusicVolume;

    void Start()
    {
        SaveDataJSON.SetPaths();

        if (SaveDataJSON.DoesSavedDataExist())
        {
            using (StreamReader reader = new StreamReader(SaveDataJSON.PersistentPath))
            {
                // LOAD JSON
                string json = reader.ReadToEnd();

                if (json == "") return;

                SaveData savedDataLoadedFromJson = JsonUtility.FromJson<SaveData>(json);

                volumeManager.SetMasterAudio(savedDataLoadedFromJson.worldData.masterVolume);
                volumeManager.SetMusicAudio(savedDataLoadedFromJson.worldData.musicVolume);
                volumeManager.SetSFXAudio(savedDataLoadedFromJson.worldData.sfxVolume);

            }

            masterSlider.value = volumeManager.MasterVolume;
            musicSlider.value = volumeManager.MusicVolume;
            sfxSlider.value = volumeManager.SFXvolume;
        }
        else
        {
            volumeManager.SetMasterAudio(defaultMasterVolume);
            volumeManager.SetMusicAudio(defaultSFXVolume);
            volumeManager.SetSFXAudio(defaultMusicVolume);

            masterSlider.value = defaultMasterVolume;
            musicSlider.value = defaultSFXVolume;
            sfxSlider.value = defaultMusicVolume;
        }
    }

    public void SaveVolumesOnPlay()
    {
        SaveDataJSON.SetAudioVolumes(masterSlider.value, musicSlider.value, sfxSlider.value);
    }

}
