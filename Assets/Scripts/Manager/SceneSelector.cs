using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void HubScene()
    {
        SceneManager.LoadScene(1);
    }

    public void EnemyScene()
    {
        SceneManager.LoadScene(2);
    }
    public void NPCScene()
    {
        SceneManager.LoadScene(3);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
