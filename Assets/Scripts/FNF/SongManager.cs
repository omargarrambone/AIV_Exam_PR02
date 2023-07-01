using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip song;
    [SerializeField] TMPro.TMP_Text audiotext;

    void Start()
    {
        song = Resources.Load<AudioClip>("song");

        PlaySong();
    }

    void Update()
    {
        audiotext.text = song.loadState.ToString();
    }

    [ContextMenu("Play song")]
    public void PlaySong()
    {
        audioSource.PlayOneShot(song);

        Debug.LogWarning("PLAY");
    }
}
