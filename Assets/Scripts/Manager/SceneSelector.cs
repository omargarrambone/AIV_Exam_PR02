using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void HubScene()
    {
        SceneManager.LoadScene(1);
        PlayerManager.SetPosition(new Vector3(-55, -2, -25));
    }

    public void EnemyScene()
    {
        SceneManager.LoadScene(2);
        PlayerManager.SetPosition(00, 0, 0);
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
