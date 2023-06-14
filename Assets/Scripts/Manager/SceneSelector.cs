using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void HubScene()
    {
        SceneManager.LoadScene(1);
        PlayerManager.SetPosition(new Vector3(-64f, 2.3f, -31f));
    }

    public void EnemyScene()
    {
        SceneManager.LoadScene(2);
        PlayerManager.SetPosition(0, 0, 0);
    }
    public void NPCScene()
    {
        SceneManager.LoadScene(3);
        PlayerManager.SetPosition(0, 0, 0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
