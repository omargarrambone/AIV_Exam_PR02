using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public float MasterVolume, SFXvolume, MusicVolume;

    public void SetMasterAudio(float Value)
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(Value) * 20);
        MasterVolume = Value;
    }

    public void SetSFXAudio(float Value)
    {
        Mixer.SetFloat("SFXVolume", Mathf.Log10(Value) * 20);
        SFXvolume = Value;
    }
    
    public void SetMusicAudio(float Value)
    {
        Mixer.SetFloat("MusicVolume", Mathf.Log10(Value) * 20);
        MusicVolume = Value;
    }

}
