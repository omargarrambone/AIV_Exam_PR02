using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void EnemyScene()
    {
        SceneManager.LoadScene(0);
    }
    public void NPCScene()
    {
        SceneManager.LoadScene(1);
    }

    public void InventoryScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
