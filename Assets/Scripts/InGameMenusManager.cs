using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenusManager : MonoBehaviour
{
    public static GameObject PauseMenu, SaveMenu;
    [SerializeField] private GameObject pauseMenu, saveMenu;
    private static CameraFollow staticCameraFollow;
    [SerializeField] private CameraFollow cameraFollow;

    void Awake()
    {
        PauseMenu = pauseMenu;
        SaveMenu = saveMenu;
        staticCameraFollow = cameraFollow;
    }



    public static void ShowHidePauseMenu(bool value)
    {
        PauseMenu.SetActive(value);
        ChangeCameraOnSaving();

    }

    public static void ShowHidePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

    public static void ShowHideSaveMenu(bool value)
    {
        SaveMenu.SetActive(value);
    }

    public static void ShowHideSaveMenu()
    {
        SaveMenu.SetActive(!SaveMenu.activeSelf);
        ChangeCameraOnSaving();
    }

    public static void ChangeCameraOnSaving()
    {
        if (SaveMenu.activeSelf)
        {
            staticCameraFollow.SetCameraTarget(type:CameraType.SavingCamera);
        }
        else
        {
            staticCameraFollow.ResetCameraTarget();
        }
    }
}
