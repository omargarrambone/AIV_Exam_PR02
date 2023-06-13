using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] bool changeOnStart;
    [SerializeField] Transform player;
    [SerializeField] string nextScene;
    [SerializeField] Vector3 playerPositionInNextScene;

    void Start()
    {
        if (changeOnStart)
        {
            player.position = playerPositionInNextScene;
            ChangeScene(nextScene);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
