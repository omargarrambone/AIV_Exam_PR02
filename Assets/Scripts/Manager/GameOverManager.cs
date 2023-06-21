using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public SceneSelector SceneSelector;
    public SaveDataJSON DataSaver;

    // Start is called before the first frame update
    void Start()
    {
        DataSaver = FindObjectOfType<SaveDataJSON>();
    }

    public void LoadLastSave()
    {
        if (DataSaver.DoesSavesExist())
        {
            DataSaver.LoadData();
        }
        else
        {
            SceneSelector.HubScene();
        }
    }
}
