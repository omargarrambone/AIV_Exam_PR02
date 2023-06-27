using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenusManager : MonoBehaviour
{
    public static GameObject PauseMenu, SaveMenu;
    [SerializeField] private GameObject pauseMenu, saveMenu;
    void Awake()
    {
        PauseMenu = pauseMenu;
        SaveMenu = saveMenu;
    }

    public static void ShowHidePauseMenu(bool value)
    {
        PauseMenu.SetActive(value);
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
    }
}
