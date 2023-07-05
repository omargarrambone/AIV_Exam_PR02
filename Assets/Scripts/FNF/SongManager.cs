using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource, bossBattleAudioSource;
    [SerializeField] TMPro.TMP_Text audiotext;
    [SerializeField] GameState lastState;
    [SerializeField] UnityEvent OnSongStart,OnSongEnded;
    [SerializeField] Camera songCamera;
    [SerializeField] Transform playerRythmTransform,endPlayerTransform;
    [SerializeField] RythmArrowsManager rythmArrowsManager;

    bool isPlaying;

    void Update()
    {
        if (!isPlaying) return;

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

        Debug.Log(audioSource.time);
        if (GameManager.GameState == GameState.Paused) return;

        if(!audioSource.isPlaying && audioSource.time == 0)
        {
            isPlaying = false;
            PlayerManager.SetTriggerAnimation("IsNotDancing");
            if (rythmArrowsManager.PlayerPoints >= rythmArrowsManager.EnemyPoints)
            {
               gameObject.SetActive(false);
               PlayerManager.EnableDisablePlayerMovement(true);
               PlayerManager.SetPosition(endPlayerTransform.position);
               OnSongEnded.Invoke();
            }
            else
            {
                PlayerManager.Death();
            }
        }

    }

    [ContextMenu("Play song")]
    public void PlaySong()
    {
        if (isPlaying) return;

        bossBattleAudioSource.Stop();
        audioSource.Play();
        isPlaying = true;
        PlayerManager.EnableDisablePlayerMovement(false);

        PlayerManager.SetPosition(playerRythmTransform.position);
        PlayerManager.SetRotation(playerRythmTransform.rotation);
        PlayerManager.SetTriggerAnimation("IsDancing");
        OnSongStart.Invoke();
    }
}
