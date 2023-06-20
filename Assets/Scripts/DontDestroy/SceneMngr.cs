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
    [SerializeField] private CharacterController _characterController;

    void Start()
    {
        if (changeOnStart)
        {
            _characterController.enabled = false;
            PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene,PlayerRotationInNextScene);
            if(OverrideStartScene != "") NextScene = OverrideStartScene;
            ChangeScene(NextScene);
            _characterController.enabled=true;

        }
    }

    public void ChangeScene(string sceneName)
    {
        _characterController.enabled = false;
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        _characterController.enabled = true;
    }
    public void ChangeScene(int sceneIndex)
    {
        _characterController.enabled = false;
        PlayerManager.PlayerGameObject.transform.SetPositionAndRotation(PlayerPositionInNextScene, PlayerRotationInNextScene);
        loadingCanvas.SetActive(true);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        _characterController.enabled = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        loadingCanvas.SetActive(false);
    }
}
