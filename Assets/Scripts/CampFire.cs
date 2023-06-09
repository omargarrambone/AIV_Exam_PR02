using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{

    public void ShowHideSaveMenu()
    {
        if (!PlayerManager.PlayerCharactercontroller.isGrounded) return;

        InGameMenusManager.ShowHideSaveMenu();

        if (InGameMenusManager.SaveMenu.activeSelf)
        {
            PlayerManager.PlayerGameObject.transform.forward = (transform.position - PlayerManager.PlayerGameObject.transform.position);
            PlayerManager.PlayerGameObject.transform.eulerAngles = new Vector3(0, PlayerManager.PlayerGameObject.transform.eulerAngles.y, 0);
        }
    }
}
