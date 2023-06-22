using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public void ShowHideMenu()
    {
        PauseMenuManager.PauseMenu.SetActive(!PauseMenuManager.PauseMenu.activeSelf);
    }

    public void HideMenu()
    {
        PauseMenuManager.PauseMenu.SetActive(false);
    }
}
