using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private SceneMngr sceneMngr;

    public void HubScene()
    {
        sceneMngr.PlayerPositionInNextScene = new Vector3(-64f, 2.3f, -31f);
        sceneMngr.ChangeScene(1);
    }

    public void EnemyScene()
    {
        sceneMngr.PlayerPositionInNextScene = Vector3.zero;
        sceneMngr.ChangeScene(2);
    }
    public void NPCScene()
    {
        sceneMngr.PlayerPositionInNextScene = Vector3.zero;
        sceneMngr.ChangeScene(3);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
