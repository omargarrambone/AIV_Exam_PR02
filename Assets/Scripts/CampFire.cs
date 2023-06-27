using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public void ShowHideSaveMenu()
    {
        InGameMenusManager.ShowHideSaveMenu();
    }

    public void HideSaveMenu()
    {
        InGameMenusManager.ShowHideSaveMenu(false);
    }
}
