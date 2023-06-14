using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] bool changeOnStart;
    [SerializeField] Transform player;
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
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
