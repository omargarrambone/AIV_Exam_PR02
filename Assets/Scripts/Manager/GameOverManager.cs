using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public SaveDataJSON SavedData;

    // Start is called before the first frame update
    void Start()
    {
        SavedData = FindObjectOfType<SaveDataJSON>();
    }

    public void LoadLastSave()
    {
        SavedData.LoadData();
    }
}
