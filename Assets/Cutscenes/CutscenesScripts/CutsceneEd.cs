using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEd : MonoBehaviour
{

    [SerializeField] private float cutscenesDuration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CutsceneDuration());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator CutsceneDuration()
    {
        yield return new WaitForSeconds(cutscenesDuration);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
