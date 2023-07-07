using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneHandler : MonoBehaviour
{
    [SerializeField] float cutsceneDuration;
    [SerializeField] float cutsceneFadeInOut;
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(CutsceneCounter());
        StartCoroutine(CutsceneFadeInOut());
    }

    // Update is called once per frame
    
    IEnumerator CutsceneCounter()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        SceneManager.LoadScene("DontDestroyScene", LoadSceneMode.Single);
        
    }

    IEnumerator CutsceneFadeInOut()
    {
        yield return new WaitForSeconds(cutsceneFadeInOut);
        Invoke("FadeInAndOut", 75f);
        FadeInAndOut();
    }

    public void FadeInAndOut()
    {
        FadeInOut.FadeinOut = true;
        
    }
}
