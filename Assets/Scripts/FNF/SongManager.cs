using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SongManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMPro.TMP_Text audiotext;
    [SerializeField] GameState lastState;
    [SerializeField] UnityEvent OnSongStart,OnSongEnded;
    [SerializeField] Camera songCamera;
    UniversalAdditionalCameraData universalAdditionalCameraData;
    [SerializeField] Vector3 playerPosOnStartSong;
    [SerializeField] Vector3 playerRotOnStartSong;
    [SerializeField] RythmArrowsManager rythmArrowsManager;

    bool isPlaying;

    private void Start()
    {
        universalAdditionalCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
    }

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

        if(!audioSource.isPlaying && audioSource.time/audioSource.clip.length > 0.99f)
        {
            isPlaying = false;
            PlayerManager.SetTriggerAnimation("IsNotDancing");
            if (rythmArrowsManager.PlayerPoints >= rythmArrowsManager.EnemyPoints)
            {
               gameObject.SetActive(false);
               PlayerManager.EnableDisablePlayerMovement(true);
               OnSongEnded.Invoke();
               universalAdditionalCameraData.cameraStack.Remove(songCamera);
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

        audioSource.Play();
        isPlaying = true;
        PlayerManager.EnableDisablePlayerMovement(false);

        PlayerManager.SetPosition(playerPosOnStartSong);
        PlayerManager.SetRotation(playerRotOnStartSong);
        PlayerManager.SetTriggerAnimation("IsDancing");

        OnSongStart.Invoke();

        universalAdditionalCameraData.cameraStack.Add(songCamera);
    }
}
