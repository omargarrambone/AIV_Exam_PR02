using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenusManager : MonoBehaviour
{
    public static GameObject PauseMenu, SaveMenu;
    private static CameraFollow staticCameraFollow;
    private static EventSystemManger staticEventSystemManger;

    [SerializeField] private GameObject pauseMenu, saveMenu;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private EventSystemManger eventSystemManger;

    private static bool hasOpenedMenu;

    void Awake()
    {
        PauseMenu = pauseMenu;
        SaveMenu = saveMenu;
        staticCameraFollow = cameraFollow;
        staticEventSystemManger = eventSystemManger;
    }

    public static void ShowHidePauseMenu(bool value)
    {
        if (hasOpenedMenu) { CloseAllMenus(); return; }

        PauseMenu.SetActive(value);
        CursorLocker(value);
    }

    public static void ShowHidePauseMenu()
    {
        ShowHidePauseMenu(!PauseMenu.activeSelf);
    }

    public static void ShowHideSaveMenu(bool value)
    {
        if (hasOpenedMenu) { CloseAllMenus(); return; }

        SaveMenu.SetActive(value);
        ChangeCameraOnSaving(value);
    }

    public static void ShowHideSaveMenu()
    {
        ShowHideSaveMenu(!SaveMenu.activeSelf);
    }

    public static void CloseAllMenus()
    {
        if (PauseMenu.activeSelf) PauseMenu.SetActive(false);

        if (SaveMenu.activeSelf) { SaveMenu.SetActive(false); ChangeCameraOnSaving(false); }

        CursorLocker(false);
    }

    public static void ChangeCameraOnSaving(bool value)
    {
        if (value)
        {
            staticEventSystemManger.SwitchCurrentButton(SaveMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
            staticCameraFollow.SetCameraTarget(type:CameraType.SavingCamera);
        }
        else
        {
            staticCameraFollow.ResetCameraTarget();
        }

        CursorLocker(value);
    }

    private static void CursorLocker(bool value)
    {
        if (value)
        {
            PlayerManager.PlayerGameObject.GetComponent<Animator>().SetTrigger("IsResting");
            PlayerManager.EnableDisablePlayerMovement(false);
            GameManager.GameState = GameState.Paused;
            Cursor.lockState = CursorLockMode.Confined;
            hasOpenedMenu = true;
        }
        else
        {
            PlayerManager.PlayerGameObject.GetComponent<Animator>().SetTrigger("IsNotResting");
            PlayerManager.EnableDisablePlayerMovement(true);
            GameManager.GameState = GameState.Playing;
            Cursor.lockState = CursorLockMode.Locked;
            hasOpenedMenu = false;
        }
    }
}
