using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private SceneMngr sceneMngr;

    public void HubScene()
    {
        sceneMngr.PlayerPositionInNextScene = new Vector3(-64f, 2.3f, -31f);
        sceneMngr.ChangeScene("HubBeta");
    }

    public void EnemyScene()
    {
        sceneMngr.PlayerPositionInNextScene = Vector3.zero;
        sceneMngr.ChangeScene("Enemy Scene");
    }
    public void NPCScene()
    {
        sceneMngr.PlayerPositionInNextScene = Vector3.zero;
        sceneMngr.ChangeScene("NPC Scene");
    }

    public void OvestScene()
    {
        sceneMngr.PlayerPositionInNextScene = new Vector3(121.758484f, 0.523557901f, -4.22627974f);
        sceneMngr.ChangeScene("OvestMap");
    }

    public void MainMenu()
    {
        sceneMngr.PlayerPositionInNextScene = Vector3.zero;
        sceneMngr.ChangeScene(0);
        Destroy(PlayerManager.PlayerGameObject.transform.parent.gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
