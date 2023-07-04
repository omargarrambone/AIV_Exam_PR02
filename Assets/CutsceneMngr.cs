using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class CutsceneMngr : MonoBehaviour
{
    [SerializeField] public GameObject cutsceneDirector;
    [SerializeField] private GameObject ninjaForCutscene;
    [SerializeField] private Camera CutsceneCamera;
    [SerializeField] float cutsceneDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cutsceneDirector.SetActive(true);
            PlayerManager.DisablePlayerMovement();
            PlayerManager.SetPosition(240,100,187);
            Camera.main.gameObject.SetActive(false);
            CutsceneCamera.gameObject.SetActive(true);
            StartCoroutine(FinishCutscene());
        }

    }

    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        GetComponent<BoxCollider>().enabled = false;
        ninjaForCutscene.SetActive(false);
        CutsceneCamera.gameObject.SetActive(false);
       Camera.main.gameObject.SetActive(true);
        PlayerManager.SetPosition(229, 100, 200);
        PlayerManager.EnablePlayerMovement();

    }
}
