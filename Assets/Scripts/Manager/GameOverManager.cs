using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

public class GameOverManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void DontDestroyScene()
    {
        SceneManager.LoadScene("DontDestroyScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu Scene");
    }

}
