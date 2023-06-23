using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] bool changeOnStart;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] LoadingScreen loadingScript;
    [SerializeField] public string OverrideStartScene;
    [SerializeField] public string NextScene;
    [SerializeField] public Vector3 PlayerPositionInNextScene;
    [SerializeField] public Quaternion PlayerRotationInNextScene;

    void Start()
    {
        if (changeOnStart)
        {
            SetPlayerPosition();

            if (OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));
        //SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneIndex)
    {
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneIndex));
        //SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            if(loadingScript != null) loadingScript.ChangeText();

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
            loadingScript.ChangeText();

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
}
