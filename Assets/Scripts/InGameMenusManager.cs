using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenusManager : MonoBehaviour
{
    public static GameObject PauseMenu, SaveMenu;
    public static GameObject CommandUiPanel;
    public static GameObject VolumeUIPanel;
    public static GameObject SecondPauseMenu;

    private static CameraFollow staticCameraFollow;
    private static EventSystemManger staticEventSystemManger;

    [SerializeField] private GameObject pauseMenu, saveMenu;
    [SerializeField] private GameObject commandUiPanel;
    [SerializeField] private GameObject volumeUIPanel;
    [SerializeField] private GameObject secondPauseMenu;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private EventSystemManger eventSystemManger;

    private static bool hasOpenedMenu;

    void Awake()
    {
        PauseMenu = pauseMenu;
        SecondPauseMenu = secondPauseMenu;
        SaveMenu = saveMenu;
        CommandUiPanel = commandUiPanel;
        VolumeUIPanel = volumeUIPanel;
        staticCameraFollow = cameraFollow;
        staticEventSystemManger = eventSystemManger;
    }

    public static void ShowHidePauseMenu(bool value)
    {
        if (hasOpenedMenu) { CloseAllMenus(); return; }

        Time.timeScale = value  ? 0 : 1;

        PauseMenu.SetActive(value);
       
        CommandUiPanel.SetActive(false);       
        
        VolumeUIPanel.SetActive(false);
        
        SecondPauseMenu.SetActive(true);

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

        Time.timeScale = 1;
    }

    public static void ChangeCameraOnSaving(bool value)
    {
        if (value)
        {
            staticEventSystemManger.SwitchCurrentButton(SaveMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
            PlayerManager.PlayerGameObject.GetComponent<Animator>().SetTrigger("IsResting");
            staticCameraFollow.SetCameraTarget(type:CameraType.SavingCamera);
            PlayerManager.EnableDisablePlayerMovement(false);
        }
        else
        {
            PlayerManager.PlayerGameObject.GetComponent<Animator>().SetTrigger("IsNotResting");
            staticCameraFollow.ResetCameraTarget();
            PlayerManager.EnableDisablePlayerMovement(true);
        }

        CursorLocker(value);
    }

    private static void CursorLocker(bool value)
    {
        if (value)
        {
            GameManager.GameState = GameState.Paused;
            Cursor.lockState = CursorLockMode.Confined;
            hasOpenedMenu = true;
        }
        else
        {
            GameManager.GameState = GameState.Playing;
            Cursor.lockState = CursorLockMode.Locked;
            hasOpenedMenu = false;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        CloseAllMenus();
    }
}
