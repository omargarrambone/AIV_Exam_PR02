using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer Mixer;

    public void SetMasterAudio(float Value)
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(Value) * 20);
    }

    public void SetSFXAudio(float Value)
    {
        Mixer.SetFloat("SFXVolume", Mathf.Log10(Value) * 20);
    }
    
    public void SetMusicAudio(float Value)
    {
        Mixer.SetFloat("MusicVolume", Mathf.Log10(Value) * 20);
    }

}
