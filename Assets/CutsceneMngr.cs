using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class CutsceneMngr : MonoBehaviour
{
    public GameObject cutsceneDirector;
    [SerializeField] private GameObject ninjaForCutscene;
    [SerializeField] private GameObject enemyForCutscene;
    [SerializeField] private Camera CutsceneCamera;
    private Camera MainCamera;
    [SerializeField] private float cutsceneDuration;

    private void Start()
    {
            MainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cutsceneDirector.SetActive(true);
            PlayerManager.DisablePlayerMovement();
            PlayerManager.SetPosition(240,100,187);
            MainCamera.gameObject.SetActive(false);
            CutsceneCamera.gameObject.SetActive(true);
            StartCoroutine(FinishCutscene());
        }

    }

    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        GetComponent<BoxCollider>().enabled = false;
        ninjaForCutscene.SetActive(false);
        enemyForCutscene.SetActive(false);
        CutsceneCamera.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(true);
        PlayerManager.SetPosition(229, 100, 200);
        PlayerManager.EnablePlayerMovement();

    }
}
