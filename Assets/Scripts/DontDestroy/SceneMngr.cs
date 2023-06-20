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
            SetPlayerPosition();

            if (OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneIndex)
    {
        SetPlayerPosition();
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    void SetPlayerPosition()
    {
        PlayerManager.PlayerCharactercontroller.enabled = false;
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        PlayerManager.PlayerCharactercontroller.enabled = true;
    }


    private void OnLevelWasLoaded(int level)
    {
        loadingCanvas.SetActive(false);
    }
}
