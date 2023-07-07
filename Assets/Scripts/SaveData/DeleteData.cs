using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteData : MonoBehaviour
{
    string persistentPath = "";

    private void Start()
    {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void DeleteSaves()
    {
        File.Delete(persistentPath);
    }

}
