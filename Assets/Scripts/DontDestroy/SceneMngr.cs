using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] bool changeOnStart, debugDontChangeScene;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] LoadingScreen loadingScript;
    public string OverrideStartScene;
    public string NextScene;
    public Vector3 PlayerPositionInNextScene;
    public Quaternion PlayerRotationInNextScene;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] TMP_Text sceneNameText;    
    [SerializeField] Animator sceneNameAnimator;

    void Start()
    {

#if UNITY_EDITOR
        if (debugDontChangeScene) return;
#endif

        if (!SaveDataJSON.DoesSavedDataExist())
        {
            SetPlayerPosition();

            if (OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
        }

        if (changeOnStart)
        {
            SetPlayerPosition();

            if (OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
        }

    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));       
        StartCoroutine(FadeSceneName(sceneName));
    }
    public void ChangeScene(int sceneIndex)
    {
        Time.timeScale = 1;
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            if(loadingScript != null) loadingScript.ChangeText(operation);

            yield return null;
        }

        SetPlayerPosition();
        loadingCanvas.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            loadingScript.ChangeText(operation);

            yield return null;
        }

        SetPlayerPosition();
        loadingCanvas.gameObject.SetActive(false);
        yield break;
    }

    void SetPlayerPosition()
    {
        PlayerManager.PlayerCharactercontroller.enabled = false;
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        PlayerManager.PlayerCharactercontroller.enabled = true;
    }

    private IEnumerator FadeSceneName(string nameScene)
    {
        sceneNameText.text = nameScene;
        sceneNameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        sceneNameAnimator.SetBool("TextFadeIn", true);
        
        yield return new WaitForSeconds(2);
        sceneNameAnimator.SetBool("TextFadeIn", false);

        yield return new WaitForSeconds(2);
        sceneNameText.gameObject.SetActive(false);

    }
}
