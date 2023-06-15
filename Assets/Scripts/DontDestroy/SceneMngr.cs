using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] bool changeOnStart;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] public string OverrideStartScene;
    [SerializeField] public string NextScene;
    [SerializeField] public Vector3 PlayerPositionInNextScene;
    [SerializeField] public Quaternion PlayerRotationInNextScene;

    void Start()
    {
        if (changeOnStart)
        {
            PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene,PlayerRotationInNextScene);
            if(OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
        }
    }

    public void ChangeScene(string sceneName)
    {
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneIndex)
    {
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    private void OnLevelWasLoaded(int level)
    {
        loadingCanvas.SetActive(false);
    }
}
