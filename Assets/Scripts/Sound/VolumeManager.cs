using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer Mixer;

    public void SetAudio(float Value)
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(Value) * 20);
    }

}
