using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public HealthManager HealthManager;

    public SaveDataJSON SavedData;

    // Start is called before the first frame update
    void Start()
    {
        HealthManager = FindObjectOfType<HealthManager>();
        SavedData = FindObjectOfType<SaveDataJSON>();
    }

    public void LoadLastSave()
    {
        SavedData.LoadData();
    }

    public void ResetPlayer()
    {
        HealthManager.ResetHealt();
    }

}
