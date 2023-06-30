using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public SceneSelector SceneSelector;
    public SaveDataJSON DataSaver;
    public EventSystemManger EventSystemManger;
    public GameObject FirstSelectedButton;

    // Start is called before the first frame update
    void Start()
    {
        DataSaver = FindObjectOfType<SaveDataJSON>();
        EventSystemManger = FindObjectOfType<EventSystemManger>();

        EventSystemManger.SwitchCurrentButton(FirstSelectedButton);

        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LoadLastSave()
    {
        if (SaveDataJSON.DoesSavedDataExist())
        {
            DataSaver.LoadData();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            SceneSelector.HubScene();
            Cursor.lockState = CursorLockMode.Locked;
        }

        PlayerManager.EnablePlayerMovement();
    }
}
