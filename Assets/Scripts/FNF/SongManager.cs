using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMPro.TMP_Text audiotext;
    [SerializeField] GameState lastState;
    [SerializeField] UnityEvent OnSongEnded;

    void Start()
    {
        PlaySong();
    }

    void Update()
    {

        if(lastState != GameState.Paused && GameManager.GameState == GameState.Paused)
        {
            audioSource.Pause();
            lastState = GameState.Paused;
        }
        else if(lastState != GameState.Playing && GameManager.GameState == GameState.Playing) 
        {
            audioSource.UnPause();
            lastState = GameState.Playing;
        }

        if((audioSource.time / audioSource.clip.length) > 0.95f)
        {
            OnSongEnded.Invoke();
            audioSource.Stop();
            gameObject.SetActive(false);
        }
    }

    [ContextMenu("Play song")]
    public void PlaySong()
    {
        audioSource.Play();
    }
}
